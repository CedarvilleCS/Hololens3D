using UnityEngine;
using System.Collections;

public class GalleryImageSwapper : MonoBehaviour
{
    public int ImageId;

    // Use this for initialization
    void Start () {

    }

    /// <summary>
    /// Simulates an air-tap on a gallery image.  This is ultimately where every
    /// gallery and queue image selection takes place.
    /// </summary>

    public void OnSelect()
    {
        GameObject ImagePaneCollection = this.transform.root.gameObject;
        GameObject ShowGalleryButton = ImagePaneCollection.transform.Find("ShowGalleryButton").gameObject;
        GameObject ImageGallery = ImagePaneCollection.transform.Find("ImageGallery").gameObject;
        GameObject MainImagePane = ImagePaneCollection.transform.Find("AnnotatedImage").gameObject;

        int numImgs = ImagePaneCollection.GetComponent<ImageReceiver>().NumRcvdImages;
        
        // If a non-blank image pane is selected, display the image

        if (ImageId <= numImgs - 1)
        {
            Renderer ImageRenderer = this.GetComponent<Renderer>();
            Texture ImageTexture = ImageRenderer.material.mainTexture;
            MainImagePane.GetComponent<AnnotatedImageController>().DisplayImage(ImageTexture);

            // Then update the index of the currently displayed image and, if this selection was made
            // directly from the gallery window, close the gallery

            ImageGallery.GetComponent<ImageGalleryController>().UpdateCurrGalleryIndex(ImageId);
            bool GalleryVisible = ImageGallery.GetComponent<ImageGalleryController>().GalleryIsVisible;
            if (GalleryVisible)
            {
                ShowGalleryButton.GetComponent<ShowGalleryButtonScript>().hideGalleryWindow();
            }
        }
    }
}