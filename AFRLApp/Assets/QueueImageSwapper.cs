using UnityEngine;
using System.Collections;

public class QueueImageSwapper : MonoBehaviour 
{
    private GameObject ImageQueue;
    private int SelectedQueueIndex;
    private GameObject[] siblingPanes;

    // Use this for initialization
    void Start () {
        GameObject ImagePaneCollection = this.transform.root.gameObject;
        ImageQueue = ImagePaneCollection.transform.Find("ImageQueue").gameObject;
        SelectedQueueIndex = 0;
    }
	
	void OnSelect ()
    {
        siblingPanes = ImageQueue.GetComponent<ImageQueueController>().queueImagePanes;
        Debug.Log("Inside QueueImageSwapper.OnSelect");
        for(int i = 0; i < siblingPanes.Length; i++)
        {
            Debug.Log("Inside Loop");
            if(siblingPanes[i] == this)
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
