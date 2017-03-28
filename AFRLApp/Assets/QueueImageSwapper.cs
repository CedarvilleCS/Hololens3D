using UnityEngine;
using System.Collections;

public class QueueImageSwapper : MonoBehaviour 
{
    private GameObject ImageQueue;
    public int ImageId;

    // Use this for initialization
    void Start () {
        GameObject ImagePaneCollection = this.transform.root.gameObject;
        ImageQueue = ImagePaneCollection.transform.Find("ImageQueue").gameObject;
    }
	
	void OnSelect ()
    {
        Debug.Log("Inside QueueImageSwapper.OnSelect");
        ImageQueue.GetComponent<ImageQueueController>().updateCurrViewedQueuePane(this.ImageId);
    }
}
