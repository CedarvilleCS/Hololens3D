using UnityEngine;
using System.Collections;

public class ShowGalleryButtonScript : MonoBehaviour {
    private GameObject AnnotatedImage;
    private GameObject ImageGallery;
    private GameObject ImageQueue;
    private GameObject ImagePaneCollection;

    // Use this for initialization
    void Start () {
        // acquire and store the original attributes of the gallery

        ImagePaneCollection = this.transform.root.gameObject;
        AnnotatedImage = ImagePaneCollection.transform.Find("AnnotatedImage").gameObject;
        ImageQueue = ImagePaneCollection.transform.Find("ImageQueue").gameObject;
        ImageGallery = ImagePaneCollection.transform.Find("ImageGallery").gameObject;
    }

    /// <summary>
    /// Simulates a click on the Show Gallery Button. Depending on 
    /// the current state of the gallery window, it either hides or
    /// shows the window.
    /// </summary>
    void OnSelect()
    {        
        bool IsVisible = ImageGallery.GetComponent<ImageGalleryController>().GalleryIsVisible;

        if (IsVisible)
        {
            hideGalleryWindow();
        }
        else
        {
            showGalleryWindow();
        }
    }

    /// <summary>
    /// Hides the gallery window
    /// </summary>
    public void hideGalleryWindow()
    {
        ImageGallery.GetComponent<ImageGalleryController>().hideWindow();
        ImageQueue.GetComponent<ImageQueueController>().showWindow();
        AnnotatedImage.GetComponent<AnnotatedImageController>().showWindow();
    }

    /// <summary>
    /// Makes the gallery window visible
    /// </summary>
    public void showGalleryWindow()
    {
        ImageGallery.GetComponent<ImageGalleryController>().showWindow();
        ImageQueue.GetComponent<ImageQueueController>().hideWindow();
        AnnotatedImage.GetComponent<AnnotatedImageController>().hideWindow();
    }
}
