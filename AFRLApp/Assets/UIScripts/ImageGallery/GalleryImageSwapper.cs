using UnityEngine;
using System.Collections;

public class GalleryImageSwapper : MonoBehaviour
{
    public int ImageId;
    public GameObject ShowGalleryButton;
    public GameObject ImagePaneCollection;
    public GameObject ImageGallery;
    public GameObject MainImagePane;


    public void OnSelect()
    {
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
                ShowGalleryButton.GetComponent<ShowGalleryButtonScript>().HideGalleryWindow();
            }
        }
    }
}