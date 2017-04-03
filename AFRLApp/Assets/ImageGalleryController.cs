using UnityEngine;
using System.Collections;

public class ImageGalleryController : MonoBehaviour {

    private int currViewedGalleryIndex;
    public Vector3 OrigScale;
    public Vector3 ResetScale;
    public bool GalleryIsVisible;
    public GameObject[] galleryImagePanes { get; private set; }
    public Renderer[] galleryImageRenderers { get; private set; }
    // Use this for initialization
    void Start () {

        OrigScale = this.transform.localScale;  
        Debug.Log("ImageGalleryController start - current scale: " + this.transform.localScale);
        GalleryIsVisible = true;
        int numGalleryPanes = this.transform.childCount;
        galleryImagePanes = new GameObject[numGalleryPanes];
        galleryImageRenderers = new Renderer[numGalleryPanes];
        Debug.Log("Number of Panes: " + numGalleryPanes);
        for (int i = 0; i < galleryImagePanes.Length; i++)
        {
            galleryImagePanes[i] = this.transform.GetChild(i).gameObject;
            galleryImagePanes[i].GetComponent<GalleryImageSwapper>().ImageId = i;
            galleryImageRenderers[i] = galleryImagePanes[i].GetComponent<Renderer>();
            galleryImageRenderers[i].material.SetTextureScale("_MainTex", new Vector2(-1, -1));
        }
        currViewedGalleryIndex = 0;

        GameObject ImagePaneCollection = this.transform.parent.gameObject;
        bool IsFirstInstance = ImagePaneCollection.GetComponent<ImageReceiver>().FirstInstance;

        Debug.Log("ImageReceiver.InstanceNum is " + IsFirstInstance);

        if (!IsFirstInstance && OrigScale == new Vector3(0, 0, 0))
        {
            OrigScale = ResetScale;
        }
        hideWindow();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnFirstImage()
    {
        GameObject ImagePaneCollection = this.transform.parent.gameObject;
        int NumRcvdImgs = ImagePaneCollection.GetComponent<ImageReceiver>().NumRcvdImages;
        if(NumRcvdImgs <= galleryImagePanes.Length && NumRcvdImgs > 0)
        {
            OnSelectByIndex(NumRcvdImgs - 1);
        }
        else if(NumRcvdImgs > galleryImagePanes.Length)
        {
            OnSelectByIndex(galleryImagePanes.Length - 1);
        }
        
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

    public void UpdateCurrGalleryIndex(int newIndex)
    {
        currViewedGalleryIndex = newIndex;
    }
    
    public void OnSelectByIndex(int GalleryImageIndex)
    {
        Debug.Log("Inside ImageGalleryController.OnSelectByIndex");
        GameObject galleryImagePaneObj = galleryImagePanes[GalleryImageIndex];
        galleryImagePaneObj.GetComponent<GalleryImageSwapper>().OnSelect();
    }

    public void RcvNewImage(Texture2D ImageTexture, int numRcvdImages)
    {
        Debug.Log("Inside ImageGalleryController.RcvdNewImage");
        if (numRcvdImages > 1)
        {
            Debug.Log("num images > 1");
            // Determine minimum images to shift to avoid unnecesary operations

            int gallerySize = galleryImagePanes.Length;
            if (numRcvdImages < gallerySize)
            {
                gallerySize = numRcvdImages;
            }

            // shift image gallery to the right

            for (int i = gallerySize - 1; i > 0; i--)
            {
                Renderer prevObjRenderer = galleryImageRenderers[i - 1];
                Renderer currObjRenderer = galleryImageRenderers[i];
                Texture prevObjTexture = prevObjRenderer.material.mainTexture;
                currObjRenderer.material.mainTexture = prevObjTexture;
            }

            Debug.Log("Applying texture");

            Renderer galleryRenderer = galleryImageRenderers[0];
            galleryRenderer.material.mainTexture = ImageTexture;
        }
        else
        {
            Debug.Log("num images == 1");
            // Load image, but do not shift (first image rcv'd, so nothing to shift)
            Debug.Log("Applying texture");
            Renderer galleryRenderer = galleryImageRenderers[0];
            galleryRenderer.material.mainTexture = ImageTexture;
        }
    }

    public void hideWindow()
    {
        this.transform.localScale = new Vector3(0, 0, 0);
        this.GalleryIsVisible = false;
    }

    public void showWindow()
    {
        Debug.Log("ImageGalleryController start - Orig scale: " + this.OrigScale);
        this.transform.localScale = OrigScale;
        this.GalleryIsVisible = true;
    }
}