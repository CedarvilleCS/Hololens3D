using UnityEngine;
using System.Collections;

public class QueueImageSwapper : MonoBehaviour {

	// Use this for initialization
	void Start () {

    }
	
	void OnSelect (){
        var queueImageRenderer = this.gameObject.GetComponent<Renderer>();
        var queueImageTexture = queueImageRenderer.material.mainTexture;
        var imagePaneCollection = this.transform.parent.transform.parent.gameObject;
        var mainImagePane = imagePaneCollection.transform.GetChild(0);
        var mainImageRenderer = mainImagePane.GetComponent<Renderer>();
        mainImageRenderer.material.mainTexture = queueImageTexture;
    }

    void OnSelectParam (GameObject queueImagePaneObj)
    {
        var queueImageRenderer = queueImagePaneObj.gameObject.GetComponent<Renderer>();
        var queueImageTexture = queueImageRenderer.material.mainTexture;
        var imagePaneCollection = queueImagePaneObj.transform.parent.transform.parent.gameObject;
        var mainImagePane = imagePaneCollection.transform.GetChild(0);
        var mainImageRenderer = mainImagePane.GetComponent<Renderer>();
        mainImageRenderer.material.mainTexture = queueImageTexture;
    }

    void OnNextImage()
    {

    }
}
