using UnityEngine;
using System.Collections;

public class GalleryImageSwapper : MonoBehaviour
{
    private GameObject ShowGalleryButton;
    private GameObject ImageGallery;
    // Use this for initialization
    void Start () {
        ShowGalleryButton = GameObject.Find("ShowGalleryButton");
        ImageGallery = GameObject.Find("ImageGallery");
    }

    public void OnSelect()
    {
        var queueImageRenderer = this.gameObject.GetComponent<Renderer>();
        var queueImageTexture = queueImageRenderer.material.mainTexture;
        var imagePaneCollection = this.transform.parent.transform.parent.gameObject;
        var mainImagePane = imagePaneCollection.transform.GetChild(0);
        var mainImageRenderer = mainImagePane.GetComponent<Renderer>();
        mainImageRenderer.material.mainTexture = queueImageTexture;

        Debug.Log("Selecting a gallery image");

        ImageGallery.GetComponent<ImageGalleryController>().updateCurrViewedGalleryPane(this.gameObject);
        ShowGalleryButton.GetComponent<ShowGalleryButtonScript>().hideGalleryWindow();
    }

    public void OnSelectParam (GameObject galleryImagePaneObj)
    {
        var queueImageRenderer = galleryImagePaneObj.GetComponent<Renderer>();
        var queueImageTexture = queueImageRenderer.material.mainTexture;
        var imagePaneCollection = galleryImagePaneObj.transform.parent.transform.parent.gameObject;
        var mainImagePane = imagePaneCollection.transform.GetChild(0);
        var mainImageRenderer = mainImagePane.GetComponent<Renderer>();
        mainImageRenderer.material.mainTexture = queueImageTexture;
    }
}