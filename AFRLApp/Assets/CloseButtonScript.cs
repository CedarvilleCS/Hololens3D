using UnityEngine;
using System.Collections;

//test commit

public class CloseButtonScript : MonoBehaviour {
    public Vector3 OrigScale;
    public Vector3 ResetScale;

    // Use this for initialization
    void Start () {
        GameObject parentPaneToClose = this.transform.parent.gameObject;
        bool isFirstWindow = parentPaneToClose.GetComponent<ImageReceiver>().FirstInstance;

        // Do not render the close-window button for the first opened window

        if (isFirstWindow)
        {
            this.enabled = false;
        }
        else
        {
            this.transform.localScale = OrigScale;
        }
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

        GameObject parentPaneToClose = this.transform.parent.gameObject;
        bool isFirstWindow = parentPaneToClose.GetComponent<ImageReceiver>().FirstInstance;

        if (!isFirstWindow)
        {
            parentPaneToClose.GetComponent<ImageReceiver>().OnWindowClosed();
            Destroy(this.transform.root.gameObject);
        }
    }
}
