using UnityEngine;
using System.Collections;

public class AnnotatedImageController : MonoBehaviour {
    private Vector3 OrigScale;
	// Use this for initialization
	void Start () {
        Debug.Log("AnnotatedImageController start - current scale: " + this.transform.localScale);
        OrigScale = this.transform.localScale;
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

    public void hideWindow()
    {
        this.transform.localScale = new Vector3(0, 0, 0);
    }

    public void showWindow()
    {
        this.transform.localScale = OrigScale;
    }
}
