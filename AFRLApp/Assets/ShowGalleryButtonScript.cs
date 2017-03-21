using UnityEngine;
using System.Collections;

public class ShowGalleryButtonScript : MonoBehaviour {
    Vector3 origGalleryScale;
    Vector3 origQueueScale;
    Vector3 origMainImagePaneScale;
    GameObject imageGalleryObj;
    GameObject mainImagePaneObj;
    GameObject imageQueueObj;

    // Use this for initialization
    void Start () {
        var imagePaneCollection = this.gameObject.transform.parent;
        mainImagePaneObj = imagePaneCollection.transform.GetChild(0).gameObject;
        imageQueueObj    = imagePaneCollection.transform.GetChild(1).gameObject;
        imageGalleryObj  = imagePaneCollection.transform.GetChild(2).gameObject;

        hideGalleryWindow();
    }

    /// <summary>
    /// Simulates a click on the Show Gallery Button. Depending on 
    /// the current state of the gallery window, it either hides or
    /// shows the window.
    /// </summary>
    void OnSelect()
    {
        if (imageGalleryObj.activeSelf)
        {
            hideGalleryWindow();
        } else
        {
            showGalleryWindow();
        }
    }

    /// <summary>
    /// Hides the gallery window
    /// </summary>
    public void hideGalleryWindow()
    {
        // Make gallery invisible

        imageGalleryObj.SetActive(false);
        imageQueueObj.SetActive(true);
        mainImagePaneObj.SetActive(true);
    }

    /// <summary>
    /// Shows the gallery window
    /// </summary>
    public void showGalleryWindow()
    {
        // Make gallery visible

        mainImagePaneObj.SetActive(false);
        imageQueueObj.SetActive(false);
        imageGalleryObj.SetActive(true);
    }
}
