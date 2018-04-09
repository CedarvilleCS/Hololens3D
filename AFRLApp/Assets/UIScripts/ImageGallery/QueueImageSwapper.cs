using UnityEngine;
using System.Collections;

public class QueueImageSwapper : MonoBehaviour 
{
    public int ImageId;

    public GameObject ImageQueue;

    // Use this for initialization
    void Start () {
        Debug.Log("QueueImageSwapper start");
    }
	
	void OnSelect ()
    {
        Debug.Log("Inside QueueImageSwapper.OnSelect");
        ImageQueue.GetComponent<ImageQueueController>().updateCurrViewedQueueIndex(this.ImageId);
    }
}
