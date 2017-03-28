using UnityEngine;
using System.Collections;

public class GalleryImageSwapper : MonoBehaviour
{
    private GameObject ShowGalleryButton;
    private GameObject ImageGallery;
    private GameObject ImagePaneCollection;
    private GameObject MainImagePane;
    private GameObject[] galleryImagePanes;
    private Material UnInitMat;

    // Use this for initialization
    void Start () {
        ImagePaneCollection = this.transform.root.gameObject;
        ShowGalleryButton   = ImagePaneCollection.transform.Find("ShowGalleryButton").gameObject;
        ImageGallery        = ImagePaneCollection.transform.Find("ImageGallery").gameObject;
        MainImagePane       = ImagePaneCollection.transform.Find("AnnotatedImage").gameObject;
        UnInitMat = ImagePaneCollection.GetComponent<ImageReceiver>().DefaultMat;
    }

    public void OnSelect()
    {
        if (this.GetComponent<Renderer>().material != UnInitMat)
        {
            Debug.Log("Inside GalleryImageSwapper.OnSelect");
            var queueImageRenderer = this.GetComponent<Renderer>();
            var queueImageTexture = queueImageRenderer.material.mainTexture;
            var mainImageRenderer = MainImagePane.GetComponent<Renderer>();
            mainImageRenderer.material.mainTexture = queueImageTexture;

            ImageGallery.GetComponent<ImageGalleryController>().UpdateCurrGalleryPane(this.gameObject);
            bool GalleryVisible = ImageGallery.GetComponent<ImageGalleryController>().GalleryIsVisible;
            if (GalleryVisible)
            {
                ShowGalleryButton.GetComponent<ShowGalleryButtonScript>().hideGalleryWindow();
            }
        }
    }
}