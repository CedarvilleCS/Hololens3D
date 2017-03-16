using UnityEngine;
using System.Collections;

public class ImageGalleryController : MonoBehaviour {

    private GameObject currViewedGalleryPane;
    private GameObject[] galleryImagePanes;
    // Use this for initialization
    void Start () {
        int numGalleryPanes = this.transform.childCount;
        galleryImagePanes = new GameObject[numGalleryPanes];
        Debug.Log("Number of Panes: " + numGalleryPanes);
        for (int i = 0; i < numGalleryPanes; i++)
        {
            galleryImagePanes[i] = this.transform.GetChild(i).gameObject;
            Debug.Log("Adding Panes");
        }
        currViewedGalleryPane = galleryImagePanes[0];
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnNextImage()
    {
        Debug.Log("Inside OnNextImage");
        Debug.Log("Size of array: " + galleryImagePanes.Length);
        GameObject nextPane = null;
        for (int i = 0; i < galleryImagePanes.Length; i++)
        {
            Debug.Log("Inside loop");
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

    public void OnPreviousImage()
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

    public void updateCurrViewedGalleryPane(GameObject newPane)
    {
        this.currViewedGalleryPane = newPane;
    }
}
