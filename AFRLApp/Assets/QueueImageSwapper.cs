using UnityEngine;
using System.Collections;

public class QueueImageSwapper : MonoBehaviour {

	// Use this for initialization
	void Start () {

    }
	
	void OnSelect (){
        var queueImageRenderer = this.gameObject.GetComponent<Renderer>();
        var queueImageTexture = queueImageRenderer.material.mainTexture;
        var mainImagePane = this.gameObject.transform.parent.gameObject.transform.parent.gameObject;
        var mainImageRenderer = mainImagePane.GetComponent<Renderer>();
        mainImageRenderer.material.mainTexture = queueImageTexture;
    }
}
