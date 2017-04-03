using UnityEngine;
using System.Collections;

public class QueueImageSwapper : MonoBehaviour 
{
    public int ImageId;

    // Use this for initialization
    void Start () {
        Debug.Log("QueueImageSwapper start");
    }
	
	void OnSelect ()
    {
        Debug.Log("Inside QueueImageSwapper.OnSelect");
        GameObject ImageQueue = this.transform.parent.gameObject;
        ImageQueue.GetComponent<ImageQueueController>().updateCurrViewedQueueIndex(this.ImageId);
    }
}
