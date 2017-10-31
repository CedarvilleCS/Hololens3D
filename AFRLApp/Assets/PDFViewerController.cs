using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PDFViewerController : MonoBehaviour
{

    bool isVisible;
    public PDFDocument currentDocument;
    public int currentPageVisible;
    public GameObject PDFPageViewer;
    private RawImage img;
    // Use this for initialization
    void Start()
    {
        currentDocument = null;
        currentPageVisible = 0;
        img = (RawImage)PDFPageViewer.GetComponent<RawImage>();
    }

    public void ShowWindow()
    {
        this.enabled = true;
        isVisible = true;
    }

    public void HideWindow()
    {
        this.enabled = false;
        isVisible = false;
    }

    public void RcvNewPDF(PDFDocument newPDF, int NumRcvdPDFs)
    {
        if (NumRcvdPDFs == 1)
        {
            ShowPDFFromIndex(NumRcvdPDFs);
            GetComponentInParent<PDFGalleryController>().currViewedPDFIndex = newPDF.id;
        }

        //Add to gallery if it's on the correct gallery page.

    }

    public void ShowPDFFromIndex(int id)
    {
        //Get the document to show
        currentDocument = GetComponentInParent<PDFReceiver>().documents[id];

        SetPageVisible(0);

        //Show the PDF pages
        Transform pages = GameObject.Find("PDFPages").transform;
        GetComponentInParent<PDFGalleryController>().currViewedPDFIndex = id;

        for (int i = 0; i < 3; i++)
        {
            Renderer rend = pages.GetChild(i).GetComponent<Renderer>();
            Texture2D pageTex = new Texture2D(2, 2);
            pageTex.LoadImage(currentDocument.pages[i]);
            rend.material.SetTexture("PDFTexture", pageTex);
        }
    }

    public void SetPageVisible(int pageNum)
    {
        if (pageNum > 0 && pageNum < currentDocument.pages.Count)
        {
            byte[] page = currentDocument.pages[pageNum];
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(page);
            img.texture = tex;
        }
        else
        {
            Debug.Log("Warning: SetPageVisible: You tried to reference a pageNum that was out of range: " + pageNum);
        }
    }
}
