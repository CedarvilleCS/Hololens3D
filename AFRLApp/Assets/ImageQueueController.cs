using UnityEngine;
using System.Collections;

public class ImageQueueController : MonoBehaviour
{
    private GameObject currViewedGalleryPane;
    private GameObject[] galleryImagePanes;
    private GameObject ImageGallery;

    // Use this for initialization
    void Start()
    {
        ImageGallery = GameObject.Find("ImageGallery");
        GameObject imageGalleryContainer = this.transform.parent.gameObject;
        int numGalleryPanes = imageGalleryContainer.transform.childCount;
        for (int i = 0; i < numGalleryPanes; i++)
        {
            galleryImagePanes[i] = imageGalleryContainer.transform.GetChild(i).gameObject;
        }
        currViewedGalleryPane = galleryImagePanes[0];
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnNextImage()
    {
        GameObject nextPane = null;
        for (int i = 0; i < galleryImagePanes.Length; i++)
        {
            if (currViewedGalleryPane == galleryImagePanes[i])
            {
                Debug.Log("Found the current gallery image!");
                if (i < galleryImagePanes.Length - 1)
                {
                    nextPane = galleryImagePanes[i + 1];
                    nextPane.GetComponent<GalleryImageSwapper>().OnSelect();
                    currViewedGalleryPane = nextPane;
                }
                break;
            }
        }

        // If currently displayed image has overflowed out
        // of the gallery, display first gallery image

        if (nextPane == null)
        {
            nextPane = galleryImagePanes[0];
            nextPane.GetComponent<GalleryImageSwapper>().OnSelect();
        }
    }

    void OnPreviousImage()
    {
        GameObject nextPane = null;
        for (int i = 0; i < galleryImagePanes.Length; i++)
        {
            if (currViewedGalleryPane == galleryImagePanes[i])
            {
                Debug.Log("Found the current gallery image!");
                if (i > 0)
                {
                    nextPane = galleryImagePanes[i - 1];
                    nextPane.GetComponent<GalleryImageSwapper>().OnSelect();
                    currViewedGalleryPane = nextPane;
                }
                break;
            }
        }

        // If currently displayed image has overflowed out
        // of the gallery, display first gallery image

        if (nextPane == null)
        {
            nextPane = galleryImagePanes[0];
            nextPane.GetComponent<GalleryImageSwapper>().OnSelect();
        }
    }

    public void updateCurrViewedQueuePane(GameObject newPane)
    {
        ImageGallery.GetComponent<ImageGalleryController>().updateCurrViewedGalleryPane(newPane);
    }

}
