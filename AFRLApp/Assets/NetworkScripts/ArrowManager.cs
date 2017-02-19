using UnityEngine;
using System.Collections;
using HoloToolkit.Unity;

public class ArrowManager : MonoBehaviour {

    private HLNetwork.ObjectReceiver _objr;
    private System.Collections.Generic.IDictionary<uint, HLNetwork.ImagePosition> _imagePositions;
    
	void Start () {
        _objr = HLNetwork.ObjectReceiver.getTheInstance();
        _objr.PositionIDRequestReceived += OnPositionIDRequestReceived;

        _imagePositions = new System.Collections.Generic.Dictionary<uint, HLNetwork.ImagePosition>();
        spatialMappingManager = SpatialMappingManager.Instance;
    }

    public Transform arrowPrefab;
    private SpatialMappingManager spatialMappingManager;
    void Update () {
        Instantiate(arrowPrefab, Camera.main.transform.position, Quaternion.identity);

        // Code largely thanks to HoloToolkit/SpatialMapping/Scripts/TapToPlace.cs
        var headPosition = Camera.main.transform.position;
        var gazeDirection = Camera.main.transform.forward;

        RaycastHit hitInfo;
        if (Physics.Raycast(headPosition, gazeDirection, out hitInfo,
            30.0f, spatialMappingManager.LayerMask))
        {
            Quaternion toQuat = Camera.main.transform.localRotation;
            Instantiate(arrowPrefab, hitInfo.point, toQuat);
        }
        else
        {
            Vector3 pos = Camera.main.transform.forward;
            pos.Scale(new Vector3(3.0f, 3.0f, 3.0f));
            pos += Camera.main.transform.position;
            Instantiate(arrowPrefab, pos, Quaternion.identity);
        }
    }

    void OnPositionIDRequestReceived(object obj, HLNetwork.PositionIDRequestReceivedEventArgs args)
    {
        HLNetwork.ImagePosition imgPos = new HLNetwork.ImagePosition();
        _imagePositions.Add(imgPos.ID, imgPos);
        _objr.SendPositionIDResponse(imgPos.ID);
    }
}
