using UnityEngine;
using System.Collections;
using System.Threading;
using HoloToolkit.Unity;

public class ArrowManager : MonoBehaviour
{

    private HLNetwork.ObjectReceiver _objr;
    private System.Collections.Generic.IDictionary<uint, HLNetwork.ImagePosition> _imagePositions;
    private HLNetwork.ImagePosition _lastPosition;
    private Mutex _lastPositionMutex;
    private Queue _arrowPlacementQueue;

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

    public Transform arrowPrefab;
    private SpatialMappingManager spatialMappingManager;
    void Update()
    {
        _lastPositionMutex.WaitOne();
        _lastPosition = new HLNetwork.ImagePosition(Camera.main.transform);
        _lastPositionMutex.ReleaseMutex();

        if (_arrowPlacementQueue.Count > 0)
        {
            HLNetwork.ArrowPlacementReceivedEventArgs arrowPlacement = 
                _arrowPlacementQueue.Dequeue() as HLNetwork.ArrowPlacementReceivedEventArgs;
            if (arrowPlacement != null)
            {
                PlaceArrow(arrowPlacement);
            }
        }

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

    void OnArrowPlacementReceived(object obj, HLNetwork.ArrowPlacementReceivedEventArgs args)
    {
        _arrowPlacementQueue.Enqueue(args);
    }

    void PlaceArrow(HLNetwork.ArrowPlacementReceivedEventArgs arrowPlacement)
    {
        System.Diagnostics.Debug.WriteLine("ArrowManager received ArrowPlacement:");
        System.Diagnostics.Debug.WriteLine("ID: " + arrowPlacement.id);
        System.Diagnostics.Debug.WriteLine("Width: " + arrowPlacement.width);
        System.Diagnostics.Debug.WriteLine("Height: " + arrowPlacement.height);
        System.Diagnostics.Debug.WriteLine("x: " + arrowPlacement.x);
        System.Diagnostics.Debug.WriteLine("y: " + arrowPlacement.y);
    }
}
