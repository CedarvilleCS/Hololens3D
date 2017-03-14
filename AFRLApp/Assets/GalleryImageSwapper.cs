using UnityEngine;
using System.Collections;

public class GalleryImageSwapper : MonoBehaviour {

    // Use this for initialization
    void Start () {
    }

    void OnSelect()
    {
        var queueImageRenderer = this.gameObject.GetComponent<Renderer>();
        var queueImageTexture = queueImageRenderer.material.mainTexture;
        var imagePaneCollection = this.transform.parent.transform.parent.gameObject;
        var mainImagePane = imagePaneCollection.transform.GetChild(0);
        var mainImageRenderer = mainImagePane.GetComponent<Renderer>();
        mainImageRenderer.material.mainTexture = queueImageTexture;

        Debug.Log("Selecting a gallery image");

        var hideGalleryButtonObj = this.transform.parent.transform.parent.transform.GetChild(5);
        hideGalleryButtonObj.GetComponent<ShowGalleryButtonScript>().hideGalleryWindow();
    }

    void OnSelectParam (GameObject galleryImagePaneObj)
    {
        var queueImageRenderer = galleryImagePaneObj.GetComponent<Renderer>();
        var queueImageTexture = queueImageRenderer.material.mainTexture;
        var imagePaneCollection = galleryImagePaneObj.transform.parent.transform.parent.gameObject;
        var mainImagePane = imagePaneCollection.transform.GetChild(0);
        var mainImageRenderer = mainImagePane.GetComponent<Renderer>();
        mainImageRenderer.material.mainTexture = queueImageTexture;
    }
}