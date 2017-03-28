using UnityEngine;
using System.Collections;

public class GalleryImageSwapper : MonoBehaviour
{
    private GameObject ShowGalleryButton;
    private GameObject ImageGallery;
    private GameObject ImagePaneCollection;
    private GameObject MainImagePane;
    public int ImageId;

    // Use this for initialization
    void Start () {
        ImagePaneCollection = this.transform.root.gameObject;
        ShowGalleryButton   = ImagePaneCollection.transform.Find("ShowGalleryButton").gameObject;
        ImageGallery        = ImagePaneCollection.transform.Find("ImageGallery").gameObject;
        MainImagePane       = ImagePaneCollection.transform.Find("AnnotatedImage").gameObject;
    }

    public void OnSelect()
    {
        int numImgs = ImagePaneCollection.GetComponent<ImageReceiver>().numRcvdImages;
        if (ImageId <= numImgs - 1)
        {
            Debug.Log("Inside GalleryImageSwapper.OnSelect");
            Renderer queueImageRenderer = this.GetComponent<Renderer>();
            Texture queueImageTexture = queueImageRenderer.material.mainTexture;
            Renderer mainImageRenderer = MainImagePane.GetComponent<Renderer>();
            mainImageRenderer.material.mainTexture = queueImageTexture;

            ImageGallery.GetComponent<ImageGalleryController>().UpdateCurrGalleryPane(ImageId);
            bool GalleryVisible = ImageGallery.GetComponent<ImageGalleryController>().GalleryIsVisible;
            if (GalleryVisible)
            {
                ShowGalleryButton.GetComponent<ShowGalleryButtonScript>().hideGalleryWindow();
            }
        }
    }
}