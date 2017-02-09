using UnityEngine;
using System.Collections;

public class QueueImageSwapper : MonoBehaviour {

	// Use this for initialization
	void Start () {

    }
	
	void OnSelect (){
        var queueImageRenderer = this.gameObject.GetComponent<Renderer>();
        var queueImageTexture = queueImageRenderer.material.mainTexture;
        queueImageRenderer.material.mainTexture = null;

        var mainImageRenderer = this.gameObject.transform.parent.gameObject.transform.parent.gameObject.GetComponent<Renderer>();
        var mainImageTexture = mainImageRenderer.material.mainTexture;
        mainImageRenderer.material.mainTexture = null;

        queueImageRenderer.material.mainTexture = mainImageTexture;
        queueImageRenderer.material.SetTextureScale("_MainTex", new Vector2(-1, -1));
        mainImageRenderer.material.mainTexture = queueImageTexture;

    }
}
