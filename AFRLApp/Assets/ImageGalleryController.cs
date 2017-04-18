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
    void Start ()
    {
        OrigScale = this.transform.localScale;  
        GalleryIsVisible = true;

        // Set ImageId of all Gallery Image Panes and acquire their renderers
        // for the purpose of applying textures later

        int numGalleryPanes = this.transform.childCount;
        galleryImagePanes = new GameObject[numGalleryPanes];
        galleryImageRenderers = new Renderer[numGalleryPanes];
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
        
        if (!IsFirstInstance && OrigScale == new Vector3(0, 0, 0))
        {
            OrigScale = ResetScale;
        }
        hideWindow();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    /// <summary>
    /// Displays the first image received (last image in the gallery) on the 
    /// main image pane
    /// </summary>

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

    /// <summary>
    /// Display the gallery image received immediately after the current one
    /// </summary>

    public void OnNextImage()
    {
        if (currViewedGalleryIndex > 0)
        {
            OnSelectByIndex(currViewedGalleryIndex - 1);
        }
    }

    /// <summary>
    /// /// Display the gallery image received immediately before the current one
    /// </summary>

    public void OnPreviousImage()
    {
        if (currViewedGalleryIndex < galleryImagePanes.Length - 1)
        {
            OnSelectByIndex(currViewedGalleryIndex + 1);
        }
    }

    /// <summary>
    /// Sets the index of the currently displayed gallery image.  The
    /// parameter indicates the gallery index to be set.
    /// </summary>
    /// <param name="newIndex"></param>

    public void UpdateCurrGalleryIndex(int newIndex)
    {
        currViewedGalleryIndex = newIndex;
    }
    
    /// <summary>
    /// Selects a gallery image pane to display based on its index.  Parameter
    /// indicates the index in the gallery of the image to be selected/displayed.
    /// </summary>
    /// <param name="GalleryImageIndex"></param>

    public void OnSelectByIndex(int GalleryImageIndex)
    {
        GameObject galleryImagePaneObj = galleryImagePanes[GalleryImageIndex];
        galleryImagePaneObj.GetComponent<GalleryImageSwapper>().OnSelect();
    }

    /// <summary>
    /// Shifts in a newly received image into the gallery, shifting all current
    /// gallery images appropriately.  The first parameter is the texture of the newly
    /// received image and the second is the number of image received by the app so far
    /// </summary>
    /// <param name="ImageTexture"></param>
    /// <param name="numRcvdImages"></param>

    public void RcvNewImage(Texture2D ImageTexture, int numRcvdImages)
    {
        if (numRcvdImages > 1)
        {
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

            Renderer galleryRenderer = galleryImageRenderers[0];
            galleryRenderer.material.mainTexture = ImageTexture;
        }
        else
        {
            // Load image, but do not shift (first image rcv'd, so nothing to shift)
            Renderer galleryRenderer = galleryImageRenderers[0];
            galleryRenderer.material.mainTexture = ImageTexture;
        }
    }

    /// <summary>
    /// Hides the gallery window
    /// </summary>

    public void hideWindow()
    {
        this.transform.localScale = new Vector3(0, 0, 0);
        this.GalleryIsVisible = false;
    }

    /// <summary>
    /// Makes the gallery window visible
    /// </summary>

    public void showWindow()
    {
        this.transform.localScale = OrigScale;
        this.GalleryIsVisible = true;
    }
}