using UnityEngine;
using System.Collections;

public class AnnotatedImageController : MonoBehaviour {
    public Vector3 OrigScale;
    public Vector3 ResetScale;
    // Use this for initialization
    void Start () {
        OrigScale = this.transform.localScale;
        Renderer AnnotatedImageRenderer = this.GetComponent<Renderer>();
        AnnotatedImageRenderer.material.SetTextureScale("_MainTex", new Vector2(-1, -1));

        GameObject ImagePaneCollection = this.transform.parent.gameObject;
        bool IsFirstInstance = ImagePaneCollection.GetComponent<ImageReceiver>().FirstInstance;
        
        if (!IsFirstInstance && OrigScale == new Vector3(0, 0, 0))
        {
            OrigScale = ResetScale;
        }
        showWindow();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    /// <summary>
    /// Displays the passed in image
    /// </summary>
    /// <param name="NewImageTexture"></param>

    public void DisplayImage(Texture NewImageTexture)
    {
        Renderer Renderer = this.GetComponent<Renderer>();
        Renderer.material.mainTexture = NewImageTexture;
    }

    /// <summary>
    /// Hides the main image pane
    /// </summary>

    public void hideWindow()
    {
        this.transform.localScale = new Vector3(0, 0, 0);
    }

    /// <summary>
    /// Makes the main image pane visible
    /// </summary>

    public void showWindow()
    {
        this.transform.localScale = OrigScale;
    }
}
