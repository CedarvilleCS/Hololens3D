using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PDFViewerController : MonoBehaviour
{

    bool ViewerIsVisible;
    public PDFDocument currentDocument;
    public GameObject PDFPane;
    public GameObject[] pdfPageThumbnails;
    public Renderer[] pdfPageRenderers;
    private byte[] blankImage;

    void Start()
    {
        currentDocument = new PDFDocument();
        pdfPageThumbnails = new GameObject[3];
        pdfPageRenderers = new Renderer[3];
        for (int i = 0; i < 3; i++)
        {
            pdfPageThumbnails[i] = this.transform.Find("PDFPages").gameObject.transform.GetChild(i).gameObject;
            pdfPageRenderers[i] = pdfPageThumbnails[i].GetComponent<Renderer>();
            pdfPageRenderers[i].material.SetTextureScale("_MainTex", new Vector2(-1, -1));
        }

        blankImage = new byte[600000];
        for (int i = 0; i < 600000; i++)
        {
            blankImage[i] = 0x00;
        }
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
            ShowPDFFromIndex(newPDF.id);
            this.transform.root.GetComponentInChildren<PDFGalleryController>().currViewedPDFIndex = newPDF.id;
        }
    }

    public void ShowPDFFromIndex(int id)
    {
        //Get the document to show
        //Find() returns the default value if it doesn't find anything.
        currentDocument = GetComponentInParent<PDFReceiver>().documents.Find(x => x.id.Equals(id));

        if (currentDocument != new PDFDocument())
        {
            SetPageVisible(0);
        }
        //Show the PDF pages
        GameObject PDFPane = this.transform.root.gameObject;
        //GameObject PDFViewer = PDFPane.transform.Find("PDFViewer").gameObject;
        //GameObject PDFPages = PDFViewer.transform.Find("PDFPages").gameObject;
        //GameObject pages = this.transform.Find("PDFPages").gameObject;
        //GameObject.Find("PDFPages").transform;
        PDFPane.GetComponentInChildren<PDFGalleryController>().currViewedPDFIndex = id;

        for (int i = 0; i < 3; i++)
        {
            Texture2D pageTex = new Texture2D(2, 2);

            if (i < currentDocument.pages.Count)
            {
                pageTex.LoadImage(currentDocument.pages[i]);
            }
            pdfPageRenderers[i].material.mainTexture = pageTex;
            pdfPageThumbnails[i].GetComponent<PDFPageController>().pageNum = i;
        }
    }

    public void SetPageVisible(int pageNum)
    {
        if (pageNum >= 0 && pageNum < currentDocument.pages.Count)
        {
            byte[] page = currentDocument.pages[pageNum];
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(page);
            PDFPane.GetComponent<Renderer>().material.mainTexture = tex;
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