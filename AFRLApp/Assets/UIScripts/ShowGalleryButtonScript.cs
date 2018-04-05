using UnityEngine;
using System.Collections;

public class ShowGalleryButtonScript : MonoBehaviour
{
    public GameObject AnnotatedImage;
    public GameObject ImageGallery;
    public GameObject ImageQueue;
    public GameObject ImagePaneCollection;
    
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
