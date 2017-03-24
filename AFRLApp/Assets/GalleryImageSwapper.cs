using UnityEngine;
using System.Collections;

public class GalleryImageSwapper : MonoBehaviour
{
    private GameObject ShowGalleryButton;
    private GameObject ImageGallery;
    private GameObject[] galleryImagePanes;

    // Use this for initialization
    void Start () {
        ShowGalleryButton = GameObject.Find("ShowGalleryButton");
        ImageGallery = GameObject.Find("ImageGallery");
        galleryImagePanes = ImageGallery.GetComponent<ImageGalleryController>().galleryImagePanes;
    }

    public void OnSelect()
    {
        Debug.Log("Inside GalleryImageSwapper.OnSelect");
        var queueImageRenderer = this.gameObject.GetComponent<Renderer>();
        var queueImageTexture = queueImageRenderer.material.mainTexture;
        var imagePaneCollection = this.transform.parent.transform.parent.gameObject;
        var mainImagePane = imagePaneCollection.transform.GetChild(0);
        var mainImageRenderer = mainImagePane.GetComponent<Renderer>();
        mainImageRenderer.material.mainTexture = queueImageTexture;

        ImageGallery.GetComponent<ImageGalleryController>().UpdateCurrGalleryPane(this.gameObject);
        bool GalleryVisible = ImageGallery.GetComponent<ImageGalleryController>().GalleryIsVisible;
        if (GalleryVisible)
        {
            ShowGalleryButton.GetComponent<ShowGalleryButtonScript>().hideGalleryWindow();
        }
    }
}