using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine.Experimental.UIElements;

public class PDFGalleryController : MonoBehaviour
{

    public int currViewedPDFIndex;
    public bool GalleryIsVisible;
    public GameObject[] galleryPDFPanes { get; private set; }
    public Renderer[] galleryPDFRenderers { get; private set; }
    public int currentPageNum;
    private Texture2D _blankTex;

    void Start()
    {
        _blankTex = Resources.Load("DefaultPageTexture") as Texture2D;

        GalleryIsVisible = true;

        // Set PDfId of all Gallery PDF Thumbnails and acquire their renderers
        // for the purpose of applying textures later

        int numGalleryPanes = this.transform.childCount;
        galleryPDFPanes = new GameObject[numGalleryPanes];
        galleryPDFRenderers = new Renderer[numGalleryPanes];
        for (int i = 0; i < galleryPDFPanes.Length; i++)
        {
            galleryPDFPanes[i] = this.transform.GetChild(i).gameObject;
            galleryPDFRenderers[i] = galleryPDFPanes[i].GetComponent<Renderer>();
            galleryPDFRenderers[i].material.SetTextureScale("_MainTex", new Vector2(-1, -1));
        }
        currViewedPDFIndex = 0;
        currentPageNum = 0;

        GameObject PdfPane = this.transform.parent.gameObject;
        bool IsFirstInstance = PdfPane.GetComponent<PDFReceiver>().FirstInstance;

        HideWindow();
    }

    public void SetThumbnail(PDFDocument PDF, int thumbnailNum)
    {
        GameObject currThumbnail = galleryPDFPanes[thumbnailNum - 1];
        Renderer currObjRenderer = galleryPDFRenderers[thumbnailNum - 1];


        if (PDF != null)
        {
            if (PDF.thumbnail == null)
            {
                PDF.thumbnail = new Texture2D(2, 2);
                PDF.thumbnail.LoadImage(PDF.pages[0]);
            }
            currThumbnail.GetComponent<PDFGallerySwapper>().PDFId = PDF.id;
            currObjRenderer.material.mainTexture = PDF.thumbnail;
        }
        else
        {
            currThumbnail.GetComponent<PDFGallerySwapper>().PDFId = -1;
            currObjRenderer.material.mainTexture = _blankTex;
        }
    }


    /// <summary>
    /// Displays the first pdf received on the 
    /// main image pane
    /// </summary>

    public void OnFirstPDF()
    {
        OnSelectByGalleryIndex(0);
    }

    /// <summary>
    /// Display the gallery image received immediately after the current one
    /// </summary>

    public void OnNextPDF()
    {
        List<PDFDocument> documents = GetComponentInParent<PDFReceiver>().documents;
        if (currViewedPDFIndex < documents.Count)
        {
            OnSelectByGalleryIndex(currViewedPDFIndex + 1);
        }
    }

    /// <summary>
    /// Display the gallery image received immediately before the current one
    /// </summary>

    public void OnPreviousPDF()
    {
        if (currViewedPDFIndex > 0)
        {
            OnSelectByGalleryIndex(currViewedPDFIndex - 1);
        }
    }

    /// <summary>
    /// Sets the index of the currently displayed gallery image
    /// </summary>
    /// <param name="PDFId"></param>

    public void UpdateCurrGalleryIndex(int PDFId)
    {
        List<PDFDocument> documents = GetComponentInParent<PDFReceiver>().documents;
        currViewedPDFIndex = documents.FindIndex(x => x.id.Equals(PDFId));
        currentPageNum = currViewedPDFIndex / 15;
    }

    /// <summary>
    /// Selects a gallery image pane to display based on its index
    /// </summary>
    /// <param name="GalleryPDFIndex"></param>

    public void OnSelectByGalleryIndex(int GalleryPDFIndex)
    {
        GameObject galleryPDFThumbnailObj = galleryPDFPanes[GalleryPDFIndex];
        galleryPDFThumbnailObj.GetComponent<PDFGallerySwapper>().OnSelect();
    }

    /// <summary>
    /// Adds in a newly received PDF into the gallery, making sure it only appears if 
    /// the proper page is visible
    /// </summary>
    /// <param name="PDF"></param>
    /// <param name="numRcvdPDFs"></param>

    public void RcvNewPDF(PDFDocument PDF, int numRcvdPDFs)
    {
        int pageItShouldBeOn = (numRcvdPDFs - 1) / 15;
        int thumbnailNum = (numRcvdPDFs % 15);

        if (thumbnailNum == 0) //15 % 15 = 0
        {
            thumbnailNum = 15;
        }

        if (currentPageNum == pageItShouldBeOn)
        {
            SetThumbnail(PDF, thumbnailNum);
        }
    }

    /// <summary>
    /// Hides the gallery window
    /// </summary>

    public void HideWindow()
    {
        this.transform.localScale = new Vector3(0, 0, 0);
        GalleryIsVisible = false;
    }

    /// <summary>
    /// Makes the gallery window visible
    /// </summary>

    public void ShowWindow()
    {
        this.transform.localScale = new Vector3(1, 1, 1);
        GalleryIsVisible = true;
    }
}