using UnityEngine;
using System.Collections;

public class AnnotatedImageController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Renderer AnnotatedImageRenderer = this.GetComponent<Renderer>();
        AnnotatedImageRenderer.material.SetTextureScale("_MainTex", new Vector2(-1, -1));
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void DisplayImage(Texture NewImageTexture)
    {
        Renderer Renderer = this.GetComponent<Renderer>();
        Renderer.material.mainTexture = NewImageTexture;
    }
}
