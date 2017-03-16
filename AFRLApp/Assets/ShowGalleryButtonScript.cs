using UnityEngine;
using System.Collections;

public class ShowGalleryButtonScript : MonoBehaviour {
    Vector3 origGalleryScale;
    Vector3 origQueueScale;
    Vector3 origMainImagePaneScale;
    GameObject imageGalleryObj;
    GameObject mainImagePaneObj;
    GameObject imageQueueObj;

    // Use this for initialization
    void Start () {

        // acquire and store the original attributes of the gallery

        var imagePaneCollection = this.gameObject.transform.parent;
        mainImagePaneObj = imagePaneCollection.transform.GetChild(0).gameObject;
        imageQueueObj    = imagePaneCollection.transform.GetChild(1).transform.GetChild(0).gameObject;
        imageGalleryObj  = imagePaneCollection.transform.GetChild(2).gameObject;
        
        hideGalleryWindow();
    }

    void OnSelect()
    {
        Renderer ObjRend = imageGalleryObj.GetComponent<Renderer>();
        if (ObjRend.enabled == true)
        {
            hideGalleryWindow();
        }
        else
        {
            showGalleryWindow();
        }
        //if (imageGalleryObj.activeSelf)
        //{
        //    hideGalleryWindow();
        //} else
        //{
        //    showGalleryWindow();
        //}
    }

    public void hideGalleryWindow()
    {
        // Make gallery invisible
        Renderer GalObjRend = imageGalleryObj.GetComponent<Renderer>();
        Renderer QueObjRend = imageQueueObj.GetComponent<Renderer>();
        Renderer MainObjRend = mainImagePaneObj.GetComponent<Renderer>();

        GalObjRend.enabled = false;
        QueObjRend.enabled = true;
        MainObjRend.enabled = true;

        //imageGalleryObj.SetActive(false);
        //imageQueueObj.SetActive(true);
        //mainImagePaneObj.SetActive(true);
    }

    public void showGalleryWindow()
    {
        // Make gallery visible

        Renderer GalObjRend = imageGalleryObj.GetComponent<Renderer>();
        Renderer QueObjRend = imageQueueObj.GetComponent<Renderer>();
        Renderer MainObjRend = mainImagePaneObj.GetComponent<Renderer>();

        GalObjRend.enabled = false;
        QueObjRend.enabled = false;
        MainObjRend.enabled = true;

        //mainImagePaneObj.SetActive(false);
        //imageQueueObj.SetActive(false);
        //imageGalleryObj.SetActive(true);
    }
}
