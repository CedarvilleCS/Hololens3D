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

        MainImagePane = GameObject.Find("AnnotatedImage");
        ImageQueue    = GameObject.Find("ImageQueue");
        ImageGallery  = GameObject.Find("ImageGallery");

        OrigMainImagePaneScale = MainImagePane.transform.localScale;
        OrigQueueScale         = ImageQueue.transform.localScale;
        OrigGalleryScale       = ImageGallery.transform.localScale;

        ScaleWhenHidden = new Vector3(0, 0, 0);

        hideGalleryWindow();
    }

    void OnSelect()
    {
        //Renderer ObjRend = imageGalleryObj.GetComponent<Renderer>();
        //if (ObjRend.enabled == true)
        //{
        //    hideGalleryWindow();
        //}
        //else
        //{
        //    showGalleryWindow();
        //}
        //if (imageGalleryObj.activeSelf)
        //{
        //    hideGalleryWindow();
        //} else
        //{
        //    showGalleryWindow();
        //}

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
        //Renderer GalObjRend = imageGalleryObj.GetComponent<Renderer>();
        //Renderer QueObjRend = imageQueueObj.GetComponent<Renderer>();
        //Renderer MainObjRend = mainImagePaneObj.GetComponent<Renderer>();

        //GalObjRend.enabled = false;
        //QueObjRend.enabled = true;
        //MainObjRend.enabled = true;

        //imageGalleryObj.SetActive(false);
        //imageQueueObj.SetActive(true);
        //mainImagePaneObj.SetActive(true);



        Debug.Log("Inside ShowGalleryButtonScript.hideGalleryWindow()");

        ImageGallery.transform.localScale = ScaleWhenHidden;
        ImageQueue.transform.localScale = OrigQueueScale;
        MainImagePane.transform.localScale = OrigMainImagePaneScale;
        ImageGallery.GetComponent<ImageGalleryController>().GalleryIsVisible = false;
    }

    public void showGalleryWindow()
    {
        // Make gallery visible

        //Renderer GalObjRend = imageGalleryObj.GetComponent<Renderer>();
        //Renderer QueObjRend = imageQueueObj.GetComponent<Renderer>();
        //Renderer MainObjRend = mainImagePaneObj.GetComponent<Renderer>();

        //GalObjRend.enabled = false;
        //QueObjRend.enabled = false;
        //MainObjRend.enabled = true;

        //mainImagePaneObj.SetActive(false);
        //imageQueueObj.SetActive(false);
        //imageGalleryObj.SetActive(true);


        Debug.Log("Inside ShowGalleryButtonScript.showGalleryWindow()");

        ImageGallery.transform.localScale = OrigGalleryScale;
        ImageQueue.transform.localScale = ScaleWhenHidden;
        MainImagePane.transform.localScale = ScaleWhenHidden;
        ImageGallery.GetComponent<ImageGalleryController>().GalleryIsVisible = true;
    }
}
