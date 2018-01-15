using UnityEngine;
using System.Collections;
using System.Threading;
using HoloToolkit.Unity;
using System.Collections.Generic;

/// <summary>
/// A manager of all things related to the 3D marker placement feature.
/// Responds to PositionIDRequest network messages and places markers based
/// on MarkerPlacement network messages.  Must exist in the application
/// scene exactly once.
/// </summary>
public class MarkerManager : MonoBehaviour
{

    #region Fields

    /// <summary>
    /// Experimentally determined video delay.
    /// Likely to vary between networks, but not easy to measure at runtime
    /// since neither side has any idea when any moment in the video stream is.
    /// </summary>
    private const float videoStreamDelay = 2.40f;

    /// <summary>
    /// The network connection object
    /// </summary>
    private HLNetwork.ObjectReceiver _objr;

    /// <summary>
    /// Stores ImagePosition objects by ID so that we can look them up
    /// later in order to place markers based on them
    /// </summary>
    private System.Collections.Generic.IDictionary<int, HLNetwork.ImagePosition> _imagePositions;

    /// <summary>
    /// Associates each position ID with a list of markers previously
    /// placed with that ID
    /// </summary>
    private System.Collections.Generic.IDictionary<int, System.Collections.ArrayList> _placedMarkersByID;

    /// <summary>
    /// Cache to store image positions from the recent past for associating
    /// with image IDs in responding to PositionIDRequests
    /// </summary>
    private HLNetwork.ImagePositionCache _imgPosCache;

    /// <summary>
    /// For transferring MarkerPlacement events from the network to the
    /// main thread.  This is needed because the events from the network
    /// are necessarily asynchronous, whereas modifying the 'game' world
    /// (by placing a marker) must be done from the main thread.
    /// </summary>
    private Queue _markerPlacementQueue;

    /// <summary>
    /// For transferring MarkerErasure events from the network to the
    /// main thread.  This is needed because the events from the network
    /// are necessarily asynchronous, whereas modifying the 'game' world
    /// (by erasing a marker) must be done from the main thread.
    /// </summary>
    private Queue _markerErasureQueue;

    #endregion

    /// <summary>
    /// Called by Unity when an object is created with this script attached
    /// </summary>
    void Start()
    {
        _objr = HLNetwork.ObjectReceiver.getTheInstance();
        _objr.PositionIDRequestReceived += OnPositionIDRequestReceived;
        _objr.MarkerPlacementReceived += OnMarkerPlacementReceived;
        _objr.MarkerErasureReceived += OnMarkerErasureReceived;
        _objr.DeleteSingleMarkerReceived += OnDeleteSingleMarkerReceived;

        _imgPosCache = new HLNetwork.ImagePositionCache(videoStreamDelay);
        _imagePositions = new System.Collections.Generic.Dictionary<int, HLNetwork.ImagePosition>();
        _placedMarkersByID = new System.Collections.Generic.Dictionary<int, System.Collections.ArrayList>();
        _markerPlacementQueue = Queue.Synchronized(new Queue());
        _markerErasureQueue = Queue.Synchronized(new Queue());
        spatialMappingManager = SpatialMappingManager.Instance;
    }

    /// <summary>
    /// Called by Unity once every frame
    /// </summary>
    public Transform markerPrefab;
    public Transform pyramidPrefab;
    private SpatialMappingManager spatialMappingManager;
    void Update()
    {

        ///
        /// Update the ImagePositionCache
        ///

        _imgPosCache.Update();

        ///
        /// Check if a MarkerPlacement has come in and handle it if so
        ///

        if (_markerPlacementQueue.Count > 0)
        {
            HLNetwork.MarkerPlacementReceivedEventArgs markerPlacement =
                _markerPlacementQueue.Dequeue() as HLNetwork.MarkerPlacementReceivedEventArgs;
            if (markerPlacement != null)
            {
                PlaceMarker(markerPlacement);
            }
        }

        ///
        /// Check if a MarkerErasure has come in and handle it if so
        ///

        if (_markerErasureQueue.Count > 0)
        {
            HLNetwork.MarkerErasureReceivedEventArgs markerErasure =
                _markerErasureQueue.Dequeue() as HLNetwork.MarkerErasureReceivedEventArgs;
            if (markerErasure != null)
            {
                EraseMarkers(markerErasure);
            }
        }

    }

    /// <summary>
    /// Event handler for responding to incoming PositionIDRequests
    /// </summary>
    void OnPositionIDRequestReceived(object obj, HLNetwork.PositionIDRequestReceivedEventArgs args)
    {
        //System.Diagnostics.Debug.WriteLine("Fetching an ImagePosition");
        HLNetwork.ImagePosition imgPos = _imgPosCache.GetDelayedPosition();

        _imagePositions.Add(imgPos.ID, imgPos);
        //System.Diagnostics.Debug.WriteLine("Sending Response to PositionIDRequest");
        _objr.SendPositionIDResponse(imgPos.ID);
    }

    /// <summary>
    /// Event handler for receiving MarkerPlacements.  Since this is not raised
    /// on the main thread, but the marker placement must occur on the main
    /// thread, all this does is send it to the main thread via a synchronized
    /// queue.
    /// </summary>
    void OnMarkerPlacementReceived(object obj, HLNetwork.MarkerPlacementReceivedEventArgs args)
    {
        _markerPlacementQueue.Enqueue(args);
    }

    /// <summary>
    /// Event handler for receiving MarkerErasures.  Since this is not raised
    /// on the main thread, but the marker erasure must occur on the main
    /// thread, all this does is send it to the main thread via a synchronized
    /// queue.
    /// </summary>
    void OnMarkerErasureReceived(object obj, HLNetwork.MarkerErasureReceivedEventArgs args)
    {
        _markerErasureQueue.Enqueue(args);
    }

    void OnDeleteSingleMarkerReceived(object obj, HLNetwork.MarkerErasureReceivedEventArgs args)
    {
        //
        _markerErasureQueue.Enqueue(args);
    }

    /// <summary>
    /// This function actually places the marker in space.  It is called on the
    /// main thread.
    /// </summary>
    /// <param name="markerPlacement">The information with which to place the marker</param>
    void PlaceMarker(HLNetwork.MarkerPlacementReceivedEventArgs markerPlacement)
    {
        System.Diagnostics.Debug.WriteLine("MarkerManager received MarkerPlacement:");
        System.Diagnostics.Debug.WriteLine("ID: " + markerPlacement.id);
        System.Diagnostics.Debug.WriteLine("Width: " + markerPlacement.width);
        System.Diagnostics.Debug.WriteLine("Height: " + markerPlacement.height);
        System.Diagnostics.Debug.WriteLine("x: " + markerPlacement.x);
        System.Diagnostics.Debug.WriteLine("y: " + markerPlacement.y);
        System.Diagnostics.Debug.WriteLine("dir: " + markerPlacement.dir);

        HLNetwork.ImagePosition imp;

        try
        {
            imp = _imagePositions[markerPlacement.id];
        }
        catch (KeyNotFoundException)
        {
            System.Diagnostics.Debug.WriteLine("Could not place marker: Bad position ID");
            return;
        }

        Vector3 resultDirection = imp.Forward;
        Vector3 up = imp.Up;
        Vector3 right = Vector3.Cross(up, imp.Forward);

        System.Diagnostics.Debug.WriteLine("Head Position: " + imp.Position);
        System.Diagnostics.Debug.WriteLine("Forward: " + imp.Forward);
        System.Diagnostics.Debug.WriteLine("Up: " + up);
        System.Diagnostics.Debug.WriteLine("Right: " + right);

        float x = markerPlacement.x;
        float y = markerPlacement.y;
        float h = markerPlacement.height;
        float w = markerPlacement.width;
        const float horizontalFOV = (float)(22.5 * System.Math.PI / 180.0);

        float rightFactor = ((2 * x - w) / w) * (float)System.Math.Tan(horizontalFOV);
        float upFactor = ((h - 2 * y) / w) * (float)System.Math.Tan(horizontalFOV);

        right.Scale(new Vector3(rightFactor, rightFactor, rightFactor));
        up.Scale(new Vector3(upFactor, upFactor, upFactor));

        resultDirection += right;
        resultDirection += up;

        float r = (float)(markerPlacement.r / 255.0);
        float g = (float)(markerPlacement.g / 255.0);
        float b = (float)(markerPlacement.b / 255.0);
        Color markerColor = new Color(r, g, b);

        int direction = markerPlacement.dir;
        Transform prefabToPlace;
        if (direction == 4)
        {
            prefabToPlace = markerPrefab;
        }
        else
        {
            prefabToPlace = pyramidPrefab;
        }


        // Code largely thanks to HoloToolkit/SpatialMapping/Scripts/TapToPlace.cs
        RaycastHit hitInfo;
        Transform placedMarker;
        Quaternion angle;

        switch (direction)
        {
            case 0:
                angle = Quaternion.AngleAxis(45f, resultDirection);
                break;
            case 1: //vertical case
                angle = Quaternion.identity;
                break;
            case 2:
                angle = Quaternion.AngleAxis(-45f, resultDirection);
                break;
            case 3:
                angle = Quaternion.AngleAxis(90f, resultDirection);
                break;
            case 4: //sphere case
                angle = Quaternion.identity;
                break;
            case 5:
                angle = Quaternion.AngleAxis(-90f, resultDirection);
                break;
            case 6:
                angle = Quaternion.AngleAxis(135f, resultDirection);
                break;
            case 7:
                angle = Quaternion.AngleAxis(180f, resultDirection);
                break;
            case 8:
                angle = Quaternion.AngleAxis(-135f, resultDirection);
                break;
            default:
                angle = Quaternion.identity;
                break;
        }

        if (Physics.Raycast(imp.Position, resultDirection, out hitInfo,
            30.0f, spatialMappingManager.LayerMask))
        {
            placedMarker = (Transform)Instantiate(prefabToPlace, hitInfo.point, angle);
            if (direction == 4)
            {
                placedMarker.GetComponentInChildren<Renderer>().material.color = markerColor;
            }
            else
            {
                placedMarker.GetComponent<MeshRenderer>().material.color = markerColor;
            }
        }
        else
        {
            Vector3 pos = resultDirection;
            pos.Scale(new Vector3(3.0f, 3.0f, 3.0f));
            pos += imp.Position;
            placedMarker = (Transform)Instantiate(prefabToPlace, pos, angle);
            if (prefabToPlace == pyramidPrefab)
            {
                placedMarker.GetComponentInChildren<Renderer>().material.color = markerColor;
            }
            else
            {
                placedMarker.GetComponent<MeshRenderer>().material.color = markerColor;
            }
        }

        if (!_placedMarkersByID.ContainsKey(markerPlacement.id))
        {
            _placedMarkersByID[markerPlacement.id] = new System.Collections.ArrayList();
        }
        _placedMarkersByID[markerPlacement.id].Add(placedMarker);

    }

    /// <summary>
    /// This function removes markers.  It is called on the main thread.
    /// </summary>
    /// <param name="markerErasure">The information about markers to erase</param>
    void EraseMarkers(HLNetwork.MarkerErasureReceivedEventArgs markerErasure)
    {
        if (!markerErasure.all)
        {
            ///
            /// Erase only the markers placed using a specific image
            ///

            System.Collections.ArrayList markers = _placedMarkersByID[markerErasure.id];

            if (markers == null)
            {
                System.Diagnostics.Debug.WriteLine("Received erasure message for nonexistent markers");
                return;
            }

            foreach (Transform marker in markers)
            {
                Destroy(marker.gameObject);
            }

            _placedMarkersByID.Remove(markerErasure.id);
        }
        else
        {
            ///
            /// Erase all markers
            ///

            foreach (KeyValuePair<int, ArrayList> entry in _placedMarkersByID)
            {
                ArrayList markers = entry.Value;
                foreach (Transform marker in markers)
                {
                    Destroy(marker.gameObject);
                }
            }

            _placedMarkersByID.Clear();
        }

    }
}