using UnityEngine;
using System.Collections;

public class GalleryImageSwapper : MonoBehaviour
{
    public int ImageId;

    // Use this for initialization
    void Start () {

    }

    public void OnSelect()
    {
        GameObject ImagePaneCollection = this.transform.root.gameObject;
        GameObject ShowGalleryButton = ImagePaneCollection.transform.Find("ShowGalleryButton").gameObject;
        GameObject ImageGallery = ImagePaneCollection.transform.Find("ImageGallery").gameObject;
        GameObject MainImagePane = ImagePaneCollection.transform.Find("AnnotatedImage").gameObject;

        int numImgs = ImagePaneCollection.GetComponent<ImageReceiver>().NumRcvdImages;

        Debug.Log("Image ID is " + ImageId);
        Debug.Log("NumRcvdImages is " + numImgs);

        if (ImageId <= numImgs - 1)
        {
            Debug.Log("Inside GalleryImageSwapper.OnSelect");
            Renderer ImageRenderer = this.GetComponent<Renderer>();
            Texture ImageTexture = ImageRenderer.material.mainTexture;
            MainImagePane.GetComponent<AnnotatedImageController>().DisplayImage(ImageTexture);

            ImageGallery.GetComponent<ImageGalleryController>().UpdateCurrGalleryIndex(ImageId);
            bool GalleryVisible = ImageGallery.GetComponent<ImageGalleryController>().GalleryIsVisible;
            if (GalleryVisible)
            {
                ShowGalleryButton.GetComponent<ShowGalleryButtonScript>().hideGalleryWindow();
            }
        }
    }
}