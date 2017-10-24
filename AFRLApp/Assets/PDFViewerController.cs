using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PDFViewerController : MonoBehaviour
{

    bool ViewerIsVisible;
    PDFDocument currentDocument;
    public int currentPageVisible;
    public GameObject PDFPageViewer;
    private RawImage img;

    void Start()
    {
        currentDocument = null;
        img = (RawImage)PDFPageViewer.GetComponent<RawImage>();
    }

    public void ShowWindow()
    {
        this.transform.localScale = new Vector3(1, 1, 1);
        ViewerIsVisible = true;
    }

    public void HideWindow()
    {
        this.transform.localScale = new Vector3(0, 0, 0);
        ViewerIsVisible = false;
    }

    public void RcvNewPDF(PDFDocument newPDF, int NumRcvdPDFs)
    {
        if (NumRcvdPDFs == 1)
        {
            ShowPDFFromIndex(NumRcvdPDFs);
            GetComponentInParent<PDFGalleryController>().currViewedPDFIndex = newPDF.id;
        }

        //Add to gallery if it's on the correct gallery page.
        int pageNum = GetComponentInParent<PDFGalleryController>().currentPageNum;
        if (pageNum == newPDF.id / 15)
        {
            int thumbnailNum = newPDF.id % 15;
            GetComponentInParent<PDFGalleryController>().SetThumbnail(newPDF, thumbnailNum);
        }
    }

    public void ShowPDFFromIndex(int id)
    {
        //Get the document to show
        currentDocument = GameObject.Find("Managers").GetComponent<DataManager>().documents[id];

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
            Debug.Log("Warning: in SetPageVisible, You tried to reference a pageNum that was out of range: " + pageNum);
        }
    }

    internal PDFDocument GetCurrDoc()
    {
        return currentDocument;
    }
}
