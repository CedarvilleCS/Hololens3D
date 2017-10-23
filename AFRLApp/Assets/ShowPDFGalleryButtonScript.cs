using UnityEngine;
using System.Collections;

public class ShowPDFGalleryButtonScript : MonoBehaviour
{
    private GameObject PDFViewer;
    private GameObject PDFGallery;
    private GameObject PDFQueue;
    private GameObject PDFPaneCollection;

    // Use this for initialization
    void Start()
    {
        // acquire and store the original attributes of the gallery

        PDFPaneCollection = this.transform.root.gameObject;
        PDFViewer = PDFPaneCollection.transform.Find("PDFViewer").gameObject;
        PDFQueue = PDFViewer.transform.Find("PDFPages").gameObject;
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
        //TODO: Hide the navigation arrows
        PDFGallery.GetComponent<PDFGalleryController>().HideWindow();
        //PDFQueue.GetComponent<PDFPagesController>().showWindow();
        PDFViewer.GetComponent<PDFViewerController>().ShowWindow();
    }

    /// <summary>
    /// Makes the gallery window visible
    /// </summary>
    public void ShowGalleryWindow()
    {
        //TODO: Show the navigation arrows
        PDFGallery.GetComponent<PDFGalleryController>().ShowWindow();
        //PDFQueue.GetComponent<PDFPagesController>().showWindow();
        PDFViewer.GetComponent<PDFViewerController>().HideWindow();
    }
}
