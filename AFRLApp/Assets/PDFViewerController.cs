using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PDFViewerController : MonoBehaviour
{

    bool ViewerIsVisible;
    public PDFDocument currentDocument;
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
    }

    public void ShowPDFFromIndex(int id)
    {
        //Get the document to show
        currentDocument = GetComponentInParent<PDFReceiver>().documents[id];

        SetPageVisible(0);

        //Show the PDF pages
        GameObject PDFPane = this.transform.root.gameObject;
        GameObject PDFViewer = PDFPane.transform.Find("PDFViewer").gameObject;
        GameObject PDFPages = PDFViewer.transform.Find("PDFPages").gameObject;
        //GameObject pages = this.transform.Find("PDFPages").gameObject;
            //GameObject.Find("PDFPages").transform;
        PDFPane.GetComponentInChildren<PDFGalleryController>().currViewedPDFIndex = id;

        if(currentDocument.pages.Count > 0)
        {
            Renderer rend1 = PDFPages.transform.Find("PDFPage1").GetComponent<Renderer>();
            Texture2D pageTex1 = new Texture2D(2, 2);
            pageTex1.LoadImage(currentDocument.pages[0]);
            rend1.material.SetTexture("PDFTexture", pageTex1);
            if(currentDocument.pages.Count > 1)
            {
                Renderer rend2 = PDFPages.transform.Find("PDFPage2").GetComponent<Renderer>();
                Texture2D pageTex2 = new Texture2D(2, 2);
                pageTex2.LoadImage(currentDocument.pages[0]);
                rend2.material.SetTexture("PDFTexture", pageTex2);
                if(currentDocument.pages.Count > 2)
                {
                    Renderer rend3 = PDFPages.transform.Find("PDFPage3").GetComponent<Renderer>();
                    Texture2D pageTex3 = new Texture2D(2, 2);
                    pageTex3.LoadImage(currentDocument.pages[0]);
                    rend3.material.SetTexture("PDFTexture", pageTex3);
                }
            }
        }

        //for (int i = 0; i < 3 && i < currentDocument.pages.Count; i++)
        //{
        //    Renderer rend = PDFPages.transform.GetChild(i).GetComponent<Renderer>();
        //    Texture2D pageTex = new Texture2D(2, 2);
        //    pageTex.LoadImage(currentDocument.pages[i]);
        //    rend.material.SetTexture("PDFTexture", pageTex);
        //}
    }

    public void SetPageVisible(int pageNum)
    {
        if (pageNum >= 0 && pageNum < currentDocument.pages.Count)
        {
            byte[] page = currentDocument.pages[pageNum];
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadRawTextureData(page);
            PDFPageViewer.GetComponent<Renderer>().material.mainTexture = tex;
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