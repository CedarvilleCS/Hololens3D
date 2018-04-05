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
            HideGalleryWindow();
        }
        else
        {
            ShowGalleryWindow();
        }
    }

    /// <summary>
    /// Hides the gallery window
    /// </summary>
    public void HideGalleryWindow()
    {
        ImageGallery.GetComponent<ImageGalleryController>().hideWindow();
        ImageQueue.GetComponent<ImageQueueController>().showWindow();
        AnnotatedImage.GetComponent<AnnotatedImageController>().showWindow();
        //TODO: Hide gallery scroll arrows
    }

    /// <summary>
    /// Makes the gallery window visible
    /// </summary>
    public void ShowGalleryWindow()
    {
        ImageGallery.GetComponent<ImageGalleryController>().showWindow();
        ImageQueue.GetComponent<ImageQueueController>().hideWindow();
        AnnotatedImage.GetComponent<AnnotatedImageController>().hideWindow();
        //TODO: Show gallery scroll arrows
    }
}
