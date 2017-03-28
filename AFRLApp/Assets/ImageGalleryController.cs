using UnityEngine;
using System.Collections;

public class ImageGalleryController : MonoBehaviour {

    private int currViewedGalleryIndex;
    public bool GalleryIsVisible = false;
    public GameObject[] galleryImagePanes { get; private set; }
    // Use this for initialization
    void Start () {
        int numGalleryPanes = this.transform.childCount;
        galleryImagePanes = new GameObject[numGalleryPanes];
        Debug.Log("Number of Panes: " + numGalleryPanes);
        for (int i = 0; i < galleryImagePanes.Length; i++)
        {
            galleryImagePanes[i] = this.transform.GetChild(i).gameObject;
            galleryImagePanes[i].GetComponent<GalleryImageSwapper>().ImageId = i;
            Debug.Log("Adding Panes");
        }
        currViewedGalleryIndex = 0;
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
        Debug.Log("Inside OnNextImage");
        Debug.Log("Size of array: " + galleryImagePanes.Length);
        
        if (currViewedGalleryIndex < galleryImagePanes.Length - 1)
        {
            OnSelectByIndex(currViewedGalleryIndex + 1);
        }
    }

    public void OnPreviousImage()
    {
        if (currViewedGalleryIndex > 0)
        {
            OnSelectByIndex(currViewedGalleryIndex - 1);
        }
    }

    public void UpdateCurrGalleryPane(int newIndex)
    {
        currViewedGalleryIndex = newIndex;
    }
    
    public void OnSelectByIndex(int GalleryImageIndex)
    {
        Debug.Log("Inside ImageGalleryController.OnSelectByIndex");
        GameObject galleryImagePaneObj = galleryImagePanes[GalleryImageIndex];
        galleryImagePaneObj.GetComponent<GalleryImageSwapper>().OnSelect();
    }
}