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

    void OnSelect()
    {
        Debug.Log("Inside ShowGalleryButtonScript.OnSelect()");
        
        bool IsVisible = ImageGallery.GetComponent<ImageGalleryController>().GalleryIsVisible;

        if (IsVisible)
        {
            Debug.Log("About to Show Gallery");
            hideGalleryWindow();
        }
        else
        {
            Debug.Log("About to Hide Gallery");
            showGalleryWindow();
        }
    }

    public void hideGalleryWindow()
    {
        // Make gallery invisible

        Debug.Log("Inside ShowGalleryButtonScript.hideGalleryWindow()");

        ImageGallery.GetComponent<ImageGalleryController>().hideWindow();
        ImageQueue.GetComponent<ImageQueueController>().showWindow();
        AnnotatedImage.GetComponent<AnnotatedImageController>().showWindow();
    }

    public void showGalleryWindow()
    {
        // Make gallery visible

        // acquire and store the original attributes of the gallery
        GameObject ImagePaneCollection = this.transform.root.gameObject;
        GameObject AnnotatedImage = ImagePaneCollection.transform.Find("AnnotatedImage").gameObject;
        GameObject ImageQueue = ImagePaneCollection.transform.Find("ImageQueue").gameObject;
        GameObject ImageGallery = ImagePaneCollection.transform.Find("ImageGallery").gameObject;

        Debug.Log("Inside ShowGalleryButtonScript.hideGalleryWindow()");

        ImageGallery.GetComponent<ImageGalleryController>().showWindow();
        ImageQueue.GetComponent<ImageQueueController>().hideWindow();
        AnnotatedImage.GetComponent<AnnotatedImageController>().hideWindow();
    }
}
