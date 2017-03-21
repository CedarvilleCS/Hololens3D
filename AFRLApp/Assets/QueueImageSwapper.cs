using UnityEngine;
using System.Collections;

public class QueueImageSwapper : MonoBehaviour {

	// Use this for initialization
	void Start () {

    }

    /// <summary>
    /// Simulates a click or selection of an image from the queue.  
    /// </summary>
    void OnSelect (){
        var queueImageRenderer = this.gameObject.GetComponent<Renderer>();
        var queueImageTexture = queueImageRenderer.material.mainTexture;
        var imagePaneCollection = this.transform.parent.transform.parent.gameObject;
        var mainImagePane = imagePaneCollection.transform.GetChild(0);
        var mainImageRenderer = mainImagePane.GetComponent<Renderer>();
        mainImageRenderer.material.mainTexture = queueImageTexture;
    }
}
