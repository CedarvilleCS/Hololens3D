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
        PDFGallery.GetComponent<PDFGalleryController>().hideWindow();
        //PDFQueue.GetComponent<PDFPagesController>().showWindow();
        PDFViewer.GetComponent<PDFViewerController>().showWindow();
    }

    /// <summary>
    /// Makes the gallery window visible
    /// </summary>
    public void showGalleryWindow()
    {
        PDFGallery.GetComponent<PDFGalleryController>().showWindow();
        //PDFQueue.GetComponent<PDFPagesController>().showWindow();
        PDFViewer.GetComponent<PDFViewerController>().hideWindow();
    }
}
