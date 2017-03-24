using UnityEngine;
using System.Collections;

public class QueueImageSwapper : MonoBehaviour 
{
    private GameObject ImageQueue;
    private int SelectedQueueIndex;
    private GameObject[] queueImagePanes;

    // Use this for initialization
    void Start () {
        ImageQueue = GameObject.Find("ImageQueue");
        SelectedQueueIndex = 0;
    }
	
	void OnSelect ()
    {
        this.queueImagePanes = ImageQueue.GetComponent<ImageQueueController>().queueImagePanes;
        Debug.Log("Inside QueueImageSwapper.OnSelect");
        for(int i = 0; i < queueImagePanes.Length; i++)
        {
            Debug.Log("Inside Loop");
            if(queueImagePanes[i] == this.gameObject)
            {
                Debug.Log("Inside conditional");
                SelectedQueueIndex = i;
                break;
            }
        }

        Debug.Log("Selecting Queue Image");
        Debug.Log("Selected Queue Image #" + SelectedQueueIndex);

        ImageQueue.GetComponent<ImageQueueController>().updateCurrViewedQueuePane(SelectedQueueIndex);
    }
}
