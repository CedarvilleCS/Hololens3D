using UnityEngine;
using System.Collections;
using System.Threading;
using HoloToolkit.Unity;

/// <summary>
/// A manager of all things related to the 3D arrow placement feature.
/// Responds to PositionIDRequest network messages and places arrows based
/// on ArrowPlacement network messages.  Must exist in the application
/// scene exactly once.
/// </summary>
public class ArrowManager : MonoBehaviour
{

    #region Fields

    /// <summary>
    /// The network connection object
    /// </summary>
    private HLNetwork.ObjectReceiver _objr;

    /// <summary>
    /// Stores ImagePosition objects by ID so that we can look them up
    /// later in order to place arrows based on them
    /// </summary>
    private System.Collections.Generic.IDictionary<uint, HLNetwork.ImagePosition> _imagePositions;

    /// <summary>
    /// The most recently saved ImagePosition object
    /// </summary>
    private HLNetwork.ImagePosition _lastPosition;

    /// <summary>
    /// Used to ensure that _lastPosition is not simultaneously written
    /// and read by two different threads
    /// </summary>
    private Mutex _lastPositionMutex;

    /// <summary>
    /// For transferring ArrowPlacement events from the network to the
    /// main thread.  This is needed because the events from the network
    /// are necessarily asynchronous, whereas modifying the 'game' world
    /// (by placing an arrow) must be done from the main thread.
    /// </summary>
    private Queue _arrowPlacementQueue;

    #endregion

    /// <summary>
    /// Called by Unity when an object is created with this script attached
    /// </summary>
    void Start()
    {
        _objr = HLNetwork.ObjectReceiver.getTheInstance();
        _objr.PositionIDRequestReceived += OnPositionIDRequestReceived;
        _objr.ArrowPlacementReceived += OnArrowPlacementReceived;

        _imagePositions = new System.Collections.Generic.Dictionary<uint, HLNetwork.ImagePosition>();
        _lastPositionMutex = new Mutex();
        _arrowPlacementQueue = Queue.Synchronized(new Queue());
        spatialMappingManager = SpatialMappingManager.Instance;
    }

    /// <summary>
    /// Called by Unity once every frame
    /// </summary>
    public Transform arrowPrefab;
    private SpatialMappingManager spatialMappingManager;
    void Update()
    {

        ///
        /// Update _lastPosition in a thread-safe manner
        ///

        _lastPositionMutex.WaitOne();
        _lastPosition = new HLNetwork.ImagePosition(Camera.main.transform);
        _lastPositionMutex.ReleaseMutex();

        ///
        /// Check if an ArrowPlacement has come in and handle it if so
        ///

        if (_arrowPlacementQueue.Count > 0)
        {
            HLNetwork.ArrowPlacementReceivedEventArgs arrowPlacement = 
                _arrowPlacementQueue.Dequeue() as HLNetwork.ArrowPlacementReceivedEventArgs;
            if (arrowPlacement != null)
            {
                PlaceArrow(arrowPlacement);
            }
        }

    }

    /// <summary>
    /// Event handler for responding to incoming PositionIDRequests
    /// </summary>
    void OnPositionIDRequestReceived(object obj, HLNetwork.PositionIDRequestReceivedEventArgs args)
    {
        //System.Diagnostics.Debug.WriteLine("Fetching most recent ImagePosition");
        _lastPositionMutex.WaitOne();
        HLNetwork.ImagePosition imgPos = _lastPosition;
        _lastPositionMutex.ReleaseMutex();

        _imagePositions.Add(imgPos.ID, imgPos);
        //System.Diagnostics.Debug.WriteLine("Sending Response to PositionIDRequest");
        _objr.SendPositionIDResponse(imgPos.ID);
    }

    /// <summary>
    /// Event handler for receiving ArrowPlacements.  Since this is not raised
    /// on the main thread, but the arrow placement must occur on the main
    /// thread, all this does is send it to the main thread via a synchronized
    /// queue.
    /// </summary>
    void OnArrowPlacementReceived(object obj, HLNetwork.ArrowPlacementReceivedEventArgs args)
    {
        _arrowPlacementQueue.Enqueue(args);
    }

    /// <summary>
    /// This function actually places the arrow in space.  It is called on the
    /// main thread.
    /// </summary>
    /// <param name="arrowPlacement">The information with which to place the arrow</param>
    void PlaceArrow(HLNetwork.ArrowPlacementReceivedEventArgs arrowPlacement)
    {
        System.Diagnostics.Debug.WriteLine("ArrowManager received ArrowPlacement:");
        System.Diagnostics.Debug.WriteLine("ID: " + arrowPlacement.id);
        System.Diagnostics.Debug.WriteLine("Width: " + arrowPlacement.width);
        System.Diagnostics.Debug.WriteLine("Height: " + arrowPlacement.height);
        System.Diagnostics.Debug.WriteLine("x: " + arrowPlacement.x);
        System.Diagnostics.Debug.WriteLine("y: " + arrowPlacement.y);
        
        //Instantiate(arrowPrefab, Camera.main.transform.position, Quaternion.identity);

        //// Code largely thanks to HoloToolkit/SpatialMapping/Scripts/TapToPlace.cs
        //var headPosition = Camera.main.transform.position;
        //var gazeDirection = Camera.main.transform.forward;

        //RaycastHit hitInfo;
        //if (Physics.Raycast(headPosition, gazeDirection, out hitInfo,
        //    30.0f, spatialMappingManager.LayerMask))
        //{
        //    Quaternion toQuat = Camera.main.transform.localRotation;
        //    Instantiate(arrowPrefab, hitInfo.point, toQuat);
        //}
        //else
        //{
        //    Vector3 pos = Camera.main.transform.forward;
        //    pos.Scale(new Vector3(3.0f, 3.0f, 3.0f));
        //    pos += Camera.main.transform.position;
        //    Instantiate(arrowPrefab, pos, Quaternion.identity);
        //}
    }
}
