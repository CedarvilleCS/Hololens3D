using UnityEngine;
using System.Collections;

public class PDFGalleryController : MonoBehaviour
{

    private int currViewedPDFIndex;
    public Vector3 OrigScale;
    public Vector3 ResetScale;
    public bool GalleryIsVisible;
    public GameObject[] galleryPDFPanes { get; private set; }
    public Renderer[] galleryPDFRenderers { get; private set; }
    // Use this for initialization
    void Start()
    {
        OrigScale = this.transform.localScale;
        GalleryIsVisible = true;

        // Set ImageId of all Gallery Image Panes and acquire their renderers
        // for the purpose of applying textures later

        int numGalleryPanes = this.transform.childCount;
        galleryPDFPanes = new GameObject[numGalleryPanes];
        galleryPDFRenderers = new Renderer[numGalleryPanes];
        for (int i = 0; i < galleryPDFPanes.Length; i++)
        {
            galleryPDFPanes[i] = this.transform.GetChild(i).gameObject;
            galleryPDFPanes[i].GetComponent<PDFGallerySwapper>().PDFId = i;
            galleryPDFRenderers[i] = galleryPDFPanes[i].GetComponent<Renderer>();
            galleryPDFRenderers[i].material.SetTextureScale("_MainTex", new Vector2(-1, -1));
        }
        currViewedPDFIndex = 0;

        GameObject PDFPaneCollection = this.transform.parent.gameObject;
        bool IsFirstInstance = PDFPaneCollection.GetComponent<PDFReceiver>().FirstInstance;

        if (!IsFirstInstance && OrigScale == new Vector3(0, 0, 0))
        {
            OrigScale = ResetScale;
        }
        hideWindow();
    }
    

    /// <summary>
    /// Displays the first image received (last image in the gallery) on the 
    /// main image pane
    /// </summary>

    public void OnFirstPDF()
    {
        GameObject PDFPaneCollection = this.transform.parent.gameObject;
        int NumRcvdPDFs = PDFPaneCollection.GetComponent<PDFReceiver>().NumRcvdPDFs;
        if (NumRcvdPDFs <= galleryPDFPanes.Length && NumRcvdPDFs > 0)
        {
            OnSelectByIndex(NumRcvdPDFs - 1);
        }
        else if (NumRcvdPDFs > galleryPDFPanes.Length)
        {
            OnSelectByIndex(galleryPDFPanes.Length - 1);
        }

    }

    /// <summary>
    /// Display the gallery image received immediately after the current one
    /// </summary>

    public void OnNextPDF()
    {
        if (currViewedPDFIndex > 0)
        {
            if (currViewedPDFIndex < galleryPDFPanes.Length)
            {
                OnSelectByIndex(currViewedPDFIndex - 1);
            }
            else
            {
                OnSelectByIndex(galleryPDFPanes.Length - 1);
            }
        }
    }

    /// <summary>
    /// Display the gallery image received immediately before the current one
    /// </summary>

    public void OnPreviousPDF()
    {
        if (currViewedPDFIndex < galleryPDFPanes.Length - 1)
        {
            OnSelectByIndex(currViewedPDFIndex + 1);
        }
        else
        {
            OnSelectByIndex(galleryPDFPanes.Length - 1);
        }
    }

    /// <summary>
    /// Sets the index of the currently displayed gallery image
    /// </summary>
    /// <param name="newIndex"></param>

    public void UpdateCurrGalleryIndex(int newIndex)
    {
        currViewedPDFIndex = newIndex;
    }

    /// <summary>
    /// Selects a gallery image pane to display based on its index
    /// </summary>
    /// <param name="GalleryPDFIndex"></param>

    public void OnSelectByIndex(int GalleryPDFIndex)
    {
        GameObject galleryPDFPaneObj = galleryPDFPanes[GalleryPDFIndex];
        galleryPDFPaneObj.GetComponent<PDFGallerySwapper>().OnSelect();
    }

    /// <summary>
    /// Shifts in a newly received image into the gallery, shifting all current
    /// gallery images appropriately
    /// </summary>
    /// <param name="PDFTexture"></param>
    /// <param name="numRcvdPDFs"></param>

    public void RcvNewPDF(Texture2D PDFTexture, int numRcvdPDFs)
    {
        if (numRcvdPDFs > 1)
        {
            // Determine minimum images to shift to avoid unnecesary operations

            int gallerySize = galleryPDFPanes.Length;
            if (numRcvdPDFs < gallerySize)
            {
                gallerySize = numRcvdPDFs;
            }

            // shift image gallery to the right

            for (int i = gallerySize - 1; i > 0; i--)
            {
                Renderer prevObjRenderer = galleryPDFRenderers[i - 1];
                Renderer currObjRenderer = galleryPDFRenderers[i];
                Texture prevObjTexture = prevObjRenderer.material.mainTexture;
                currObjRenderer.material.mainTexture = prevObjTexture;
            }

            Renderer galleryRenderer = galleryPDFRenderers[0];
            galleryRenderer.material.mainTexture = PDFTexture;
            UpdateCurrGalleryIndex(currViewedPDFIndex + 1);
        }
        else
        {
            // Load image, but do not shift (first image rcv'd, so nothing to shift)
            Renderer galleryRenderer = galleryPDFRenderers[0];
            galleryRenderer.material.mainTexture = PDFTexture;
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