using UnityEngine;
using System.Collections;

public class ArrowManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
        HLNetwork.ObjectReceiver objr = HLNetwork.ObjectReceiver.getTheInstance();
        objr.PositionIDRequestReceived += OnPositionIDRequestReceived;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnPositionIDRequestReceived(object obj, HLNetwork.PositionIDRequestReceivedEventArgs args)
    {
        
    }
}
