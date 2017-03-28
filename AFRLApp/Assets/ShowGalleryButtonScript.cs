using UnityEngine;
using System.Collections;

public class ShowGalleryButtonScript : MonoBehaviour {
    private Vector3 OrigGalleryScale;
    private Vector3 OrigQueueScale;
    private Vector3 OrigMainImagePaneScale;
    private GameObject ImageGallery;
    private GameObject MainImagePane;
    private GameObject ImageQueue;
    private static Vector3 ScaleWhenHidden;

    // Use this for initialization
    void Start () {

        // acquire and store the original attributes of the gallery
        GameObject ImagePaneCollection = this.transform.root.gameObject;
        MainImagePane = ImagePaneCollection.transform.Find("AnnotatedImage").gameObject;
        ImageQueue    = ImagePaneCollection.transform.Find("ImageQueue").gameObject;
        ImageGallery  = ImagePaneCollection.transform.Find("ImageGallery").gameObject;

        OrigMainImagePaneScale = MainImagePane.transform.localScale;
        OrigQueueScale         = ImageQueue.transform.localScale;
        OrigGalleryScale       = ImageGallery.transform.localScale;

        ScaleWhenHidden = new Vector3(0, 0, 0);

        hideGalleryWindow();
    }

    void OnSelect()
    {
        Debug.Log("Inside ShowGalleryButtonScript.OnSelect()");
        if (ImageGallery.transform.localScale == ScaleWhenHidden)
        {
            Debug.Log("About to Show Gallery");
            showGalleryWindow();
        }
        else
        {
            Debug.Log("About to Hide Gallery");
            hideGalleryWindow();
        }
    }

    public void hideGalleryWindow()
    {
        // Make gallery invisible

        Debug.Log("Inside ShowGalleryButtonScript.hideGalleryWindow()");

        ImageGallery.transform.localScale = ScaleWhenHidden;
        ImageQueue.transform.localScale = OrigQueueScale;
        MainImagePane.transform.localScale = OrigMainImagePaneScale;
        ImageGallery.GetComponent<ImageGalleryController>().GalleryIsVisible = false;
    }

    public void showGalleryWindow()
    {
        // Make gallery visible
        
        Debug.Log("Inside ShowGalleryButtonScript.showGalleryWindow()");

        ImageGallery.transform.localScale = OrigGalleryScale;
        ImageQueue.transform.localScale = ScaleWhenHidden;
        MainImagePane.transform.localScale = ScaleWhenHidden;
        ImageGallery.GetComponent<ImageGalleryController>().GalleryIsVisible = true;
    }
}
