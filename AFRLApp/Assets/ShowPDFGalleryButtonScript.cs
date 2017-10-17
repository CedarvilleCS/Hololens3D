using UnityEngine;
using System.Collections;

public class ShowPDFGalleryButtonScript : MonoBehaviour
{
    private GameObject AnnotatedPDF;
    private GameObject PDFGallery;
    private GameObject PDFQueue;
    private GameObject PDFPaneCollection;

    // Use this for initialization
    void Start()
    {
        // acquire and store the original attributes of the gallery

        PDFPaneCollection = this.transform.root.gameObject;
        AnnotatedPDF = PDFPaneCollection.transform.Find("AnnotatedPDF").gameObject;
        PDFQueue = PDFPaneCollection.transform.Find("PDFPages").gameObject;
        PDFGallery = PDFPaneCollection.transform.Find("PDFGallery").gameObject;
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
            hideGalleryWindow();
        }
        else
        {
            showGalleryWindow();
        }
    }

    /// <summary>
    /// Hides the gallery window
    /// </summary>
    public void hideGalleryWindow()
    {
        PDFGallery.GetComponent<PDFGalleryController>().HideWindow();
        //PDFQueue.GetComponent<PDFPagesController>().showWindow();
        AnnotatedPDF.GetComponent<AnnotatedPDFController>().showWindow();
    }

    /// <summary>
    /// Makes the gallery window visible
    /// </summary>
    public void showGalleryWindow()
    {
        PDFGallery.GetComponent<PDFGalleryController>().ShowWindow();
        //PDFQueue.GetComponent<PDFPagesController>().showWindow();
        AnnotatedPDF.GetComponent<AnnotatedPDFController>().hideWindow();
    }
}
