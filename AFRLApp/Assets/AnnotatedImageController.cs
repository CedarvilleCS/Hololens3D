using UnityEngine;
using System.Collections;

public class AnnotatedImageController : MonoBehaviour {
    public Vector3 OrigScale;
    public Vector3 ResetScale;
    // Use this for initialization
    void Start () {
        Debug.Log("AnnotatedImageController start - current scale: " + this.transform.localScale);
        OrigScale = this.transform.localScale;
        Renderer AnnotatedImageRenderer = this.GetComponent<Renderer>();
        AnnotatedImageRenderer.material.SetTextureScale("_MainTex", new Vector2(-1, -1));

        GameObject ImagePaneCollection = this.transform.parent.gameObject;
        bool IsFirstInstance = ImagePaneCollection.GetComponent<ImageReceiver>().FirstInstance;

        Debug.Log("ImageReceiver.InstanceNum is " + IsFirstInstance);

        if (!IsFirstInstance && OrigScale == new Vector3(0, 0, 0))
        {
            OrigScale = ResetScale;
            Debug.Log("New Annotated OrigScale: " + OrigScale);
        }
        showWindow();
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
