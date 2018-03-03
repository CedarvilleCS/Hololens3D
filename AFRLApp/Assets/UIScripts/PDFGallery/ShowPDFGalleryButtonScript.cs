using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowPDFGalleryButtonScript : MonoBehaviour
{
    public GameObject PDFViewer;
    public GameObject PDFGallery;
    public GameObject PDFPages;
    public GameObject PDFPane;
    public GameObject GalleryPrevious;
    public GameObject GalleryNext;

    // Use this for initialization
    void Start()
    {
        // acquire and store the original attributes of the gallery

        PDFPane = this.transform.root.gameObject;
        PDFViewer = PDFPane.transform.Find("PDFViewer").gameObject;
        PDFPages = PDFViewer.transform.Find("PDFPages").gameObject;
        PDFGallery = PDFPane.transform.Find("PDFGallery").gameObject;
    }

    /// <summary>
    /// Simulates a click on the Show Gallery Button. Depending on 
    /// the current state of the gallery window, it either hides or
    /// shows the window.
    /// </summary>
    void OnSelect()
    {
        bool IsVisible = PDFGallery.GetComponent<PDFGalleryController>().GalleryIsVisible;

        if (IsVisible)
        {
            HideGalleryWindow();
        }
        else
        {
            ShowGalleryWindow();
        }
    }

    /// <summary>
    /// Hides the gallery window
    /// </summary>
    public void HideGalleryWindow()
    {
        PDFGallery.GetComponent<PDFGalleryController>().HideWindow();
        GalleryPrevious.GetComponent<PDFGalleryPreviousNextScript>().HideWindow();
        GalleryNext.GetComponent<PDFGalleryPreviousNextScript>().HideWindow();
        PDFViewer.GetComponent<PDFViewerController>().ShowWindow();
    }

    /// <summary>
    /// Makes the gallery window visible
    /// </summary>
    public void ShowGalleryWindow()
    {
        PDFGallery.GetComponent<PDFGalleryController>().ShowWindow();
        GalleryPrevious.GetComponent<PDFGalleryPreviousNextScript>().ShowWindow();
        GalleryNext.GetComponent<PDFGalleryPreviousNextScript>().ShowWindow();
        PDFViewer.GetComponent<PDFViewerController>().HideWindow();
    }
}