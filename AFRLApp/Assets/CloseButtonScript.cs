using UnityEngine;
using System.Collections;

public class CloseButtonScript : MonoBehaviour {
    private bool isFirstWindow;
    public Vector3 OrigScale;
    public Vector3 ResetScale;

    // Use this for initialization
    void Start () {
        OrigScale = this.transform.localScale;
        GameObject ImagePaneCollection = this.transform.parent.gameObject;
        isFirstWindow = ImagePaneCollection.GetComponent<ImageReceiver>().FirstInstance;

        // Do not render the close-window button for the first opened window

        if (isFirstWindow)
        {
            this.transform.localScale = new Vector3(0, 0, 0);
        }
        else
        {
            OrigScale = ResetScale;
            this.transform.localScale = OrigScale;
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    /// <summary>
    /// Simulates an air-tap on the CloseButton gameobject, which, so long as the
    /// focused gameobject does not belong to the hierarchy of the first-opened
    /// image pane collection window, will delete and remove the currently focused
    /// gameobject's image pane collection window
    /// </summary>

    public void OnSelect ()
    {
        // Only destroy the current image pane collection if it is not
        // the first one created

        if (!isFirstWindow)
        {
            Destroy(this.transform.root.gameObject);
        }
    }
}
