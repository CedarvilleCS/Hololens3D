using UnityEngine;
using System.Collections;

public class ImageGalleryController : MonoBehaviour {

    private GameObject currViewedGalleryPane;

    // TODO - update this field
    public bool GalleryIsVisible;
    public GameObject[] galleryImagePanes { get; private set; }
    // Use this for initialization
    void Start () {
        int numGalleryPanes = this.transform.childCount;
        galleryImagePanes = new GameObject[numGalleryPanes];
        Debug.Log("Number of Panes: " + numGalleryPanes);
        for (int i = 0; i < galleryImagePanes.Length; i++)
        {
            galleryImagePanes[i] = this.transform.GetChild(i).gameObject;
            Debug.Log("Adding Panes");
        }
        currViewedGalleryPane = galleryImagePanes[0];
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnFirstImage()
    {
        GameObject ImagePaneCollection = this.transform.parent.gameObject;
        int NumRcvdImgs = ImagePaneCollection.GetComponent<ImageReceiver>().numRcvdImages;
        OnSelectByIndex(NumRcvdImgs - 1);
    }

    public void OnNextImage()
    {
        GameObject ImagePaneCollection = this.transform.parent.gameObject;
        int NumRcvdImgs = ImagePaneCollection.GetComponent<ImageReceiver>().numRcvdImages;
        Debug.Log("Inside OnNextImage");
        Debug.Log("Size of array: " + galleryImagePanes.Length);
        int nextIndex = -1;
        for (int i = 0; i < galleryImagePanes.Length; i++)
        {
            Debug.Log("Inside loop");
            if (currViewedGalleryPane == galleryImagePanes[i])
            {
                Debug.Log("Found the current gallery image!");
                if (i < galleryImagePanes.Length - 1)
                {
                    nextIndex = i + 1;
                    OnSelectByIndex(nextIndex);
                }
                break;
            }
        }

        // If currently displayed image has overflowed out
        // of the gallery, display first gallery image

        if (nextIndex == -1)
        {
            OnSelectByIndex(0);
        }
    }

    public void OnPreviousImage()
    {
        int nextIndex = -1;
        for (int i = 0; i < galleryImagePanes.Length; i++)
        {
            if (currViewedGalleryPane == galleryImagePanes[i])
            {
                Debug.Log("Found the current gallery image!");
                if (i > 0)
                {
                    nextIndex = i - 1;
                    OnSelectByIndex(nextIndex);
                }
                break;
            }
        }

        // If currently displayed image has overflowed out
        // of the gallery, display first gallery image

        if (nextIndex == -1)
        {
            OnSelectByIndex(0);
        }
    }

    public void UpdateCurrGalleryPane(GameObject newPane)
    {
        currViewedGalleryPane = newPane;
    }

    public void UpdateCurrGalleryPaneByIndex(int GalleryPaneIndex)
    {
        UpdateCurrGalleryPane(galleryImagePanes[GalleryPaneIndex]);
    }
    
    public void OnSelectByIndex(int GalleryImageIndex)
    {
        Debug.Log("Inside ImageGalleryController.OnSelectByIndex");
        GameObject galleryImagePaneObj = galleryImagePanes[GalleryImageIndex];
        galleryImagePaneObj.GetComponent<GalleryImageSwapper>().OnSelect();
    }
}