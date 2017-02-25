using UnityEngine;
using System.Collections;
using HoloToolkit.Unity;

public class ArrowManager : MonoBehaviour {

    private HLNetwork.ObjectReceiver _objr;
    private System.Collections.Generic.IDictionary<uint, HLNetwork.ImagePosition> _imagePositions;
    private HLNetwork.ImagePosition _lastPosition;
    
	void Start () {
        _objr = HLNetwork.ObjectReceiver.getTheInstance();
        _objr.PositionIDRequestReceived += OnPositionIDRequestReceived;

        _imagePositions = new System.Collections.Generic.Dictionary<uint, HLNetwork.ImagePosition>();
        spatialMappingManager = SpatialMappingManager.Instance;
    }

    public Transform arrowPrefab;
    private SpatialMappingManager spatialMappingManager;
    void Update () {
        _lastPosition = new HLNetwork.ImagePosition(Camera.main.transform);
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
        System.Diagnostics.Debug.WriteLine("Fetching most recent ImagePosition");
        HLNetwork.ImagePosition imgPos = _lastPosition;
        _imagePositions.Add(imgPos.ID, imgPos);
        System.Diagnostics.Debug.WriteLine("Sending Response to PositionIDRequest");
        _objr.SendPositionIDResponse(imgPos.ID);
    }
}
