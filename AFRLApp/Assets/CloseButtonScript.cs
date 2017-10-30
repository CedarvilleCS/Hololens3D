using UnityEngine;
using System.Collections;

//test commit

public class CloseButtonScript : MonoBehaviour
{
    public Vector3 OrigScale;
    public Vector3 ResetScale;
    bool isFirstWindow;
    // Use this for initialization
    void Start()
    {
        OrigScale = new Vector3(0.099f, 0.099f, 0.0001f);

        GameObject parentPaneToClose = this.transform.parent.gameObject;
        string parentName = this.transform.parent.transform.name;


        if (parentName.Equals("PDFPane"))
        {
            isFirstWindow = parentPaneToClose.GetComponent<PDFReceiver>().FirstInstance;
        }
        else if (parentName.Equals("ImagePaneCollection"))
        {
            isFirstWindow = parentPaneToClose.GetComponent<ImageReceiver>().FirstInstance;
        }
        else
        {
            throw new System.InvalidOperationException("Error: CloseButtonScript can only be attached to PDFPane or ImagePaneCollection");
        }

        // Do not render the close-window button for the first opened window

        if (isFirstWindow)
        {
            this.transform.localScale = new Vector3(0, 0, 0);
        }
    }

    /// <summary>
    /// Simulates an air-tap on the CloseButton gameobject, which, so long as the
    /// focused gameobject does not belong to the hierarchy of the first-opened
    /// image pane collection window, will delete and remove the currently focused
    /// gameobject's image pane collection window
    /// </summary>

    public void OnSelect()
    {
        // Only destroy the current image pane collection if it is not
        // the first one created

        GameObject parentPaneToClose = this.transform.parent.gameObject;

        if (!isFirstWindow)
        {
            Destroy(this.transform.root.gameObject);
        }
    }
}
