using UnityEngine;
using System.Collections;

public class ArrowManager : MonoBehaviour {

    private HLNetwork.ObjectReceiver _objr;
    private System.Collections.Generic.IDictionary<uint, HLNetwork.ImagePosition> _imagePositions;
    
	void Start () {
        _objr = HLNetwork.ObjectReceiver.getTheInstance();
        _objr.PositionIDRequestReceived += OnPositionIDRequestReceived;

        _imagePositions = new System.Collections.Generic.Dictionary<uint, HLNetwork.ImagePosition>();
    }

    public Transform arrowPrefab;
	void Update () {
        Instantiate(arrowPrefab, Camera.main.transform.position, Quaternion.identity);
    }

    void OnPositionIDRequestReceived(object obj, HLNetwork.PositionIDRequestReceivedEventArgs args)
    {
        HLNetwork.ImagePosition imgPos = new HLNetwork.ImagePosition();
        _imagePositions.Add(imgPos.ID, imgPos);
        _objr.SendPositionIDResponse(imgPos.ID);
    }
}
