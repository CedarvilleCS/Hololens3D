using UnityEngine;
using System.Collections;

public class QueueImageSwapper : MonoBehaviour 
{
    public int ImageId;

    // Use this for initialization
    void Start () {

    }
	
    /// <summary>
    /// Simluates air-tap on an image pane in the image queue.  Updates the
    /// currently viewed image pane and makes the necessary function call to
    /// have the corresponding gallery image selected and displayed.
    /// </summary>

	void OnSelect ()
    {
        GameObject ImageQueue = this.transform.parent.gameObject;
        ImageQueue.GetComponent<ImageQueueController>().updateCurrViewedQueueIndex(this.ImageId);
    }
}
