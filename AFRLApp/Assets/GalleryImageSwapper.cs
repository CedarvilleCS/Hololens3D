using UnityEngine;
using System.Collections;

public class GalleryImageSwapper : MonoBehaviour {

    // Use this for initialization
    void Start () {
    }

    /// <summary>
    /// Simulates a click or selection of an image from the gallery 
    /// window.  Without the implementation of voice commands, this
    /// selection can only happen when the gallery window is open;
    /// therefore, after the selection operation is finished, the 
    /// gallery window is re-hidden.
    /// </summary>

    void OnSelect()
    {
        var queueImageRenderer = this.gameObject.GetComponent<Renderer>();
        var queueImageTexture = queueImageRenderer.material.mainTexture;
        var imagePaneCollection = this.transform.parent.transform.parent.gameObject;
        var mainImagePane = imagePaneCollection.transform.GetChild(0);
        var mainImageRenderer = mainImagePane.GetComponent<Renderer>();
        mainImageRenderer.material.mainTexture = queueImageTexture;

        // Hide the gallery window

        var hideGalleryButtonObj = this.transform.parent.transform.parent.transform.GetChild(5);
        hideGalleryButtonObj.GetComponent<ShowGalleryButtonScript>().hideGalleryWindow();
    }
}