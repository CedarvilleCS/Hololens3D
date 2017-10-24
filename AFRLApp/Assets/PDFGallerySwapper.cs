using UnityEngine;
using System.Collections;

public class PDFGallerySwapper : MonoBehaviour
{
    public int PDFId;

    public void OnSelect()
    {
        GameObject PDFPaneCollection = this.transform.root.gameObject;
        GameObject ShowGalleryButton = PDFPaneCollection.transform.Find("ShowPDFGalleryButton").gameObject;
        GameObject PDFGallery = PDFPaneCollection.transform.Find("PDFGallery").gameObject;
        GameObject PDFViewer = PDFPaneCollection.transform.Find("PDFViewer").gameObject;

        int numPDFs = this.transform.parent.GetComponentInParent<PDFReceiver>().documents.Count;

        Debug.Log("PDF ID is " + PDFId);
        Debug.Log("NumRcvdPDFs is " + numPDFs);

        if (PDFId <= numPDFs - 1)
        {
            //TODO: Display selected PDF

            //Debug.Log("Inside PDFGallerySwapper.OnSelect");
            //Renderer PDFRenderer = this.GetComponent<Renderer>();
            //Texture PDFPageTexture = PDFRenderer.material.mainTexture;
            //PDFViewer.GetComponent<PDFViewerController>().DisplayPDF
            //TODO: dispaly pdf in PDFViewerController

            //Old code:
            //MainPDFPane.GetComponent<AnnotatedPDFController>().DisplayPDF(PDFPageTexture);

            PDFGallery.GetComponent<PDFGalleryController>().UpdateCurrGalleryIndex(PDFId);
            bool GalleryVisible = PDFGallery.GetComponent<PDFGalleryController>().GalleryIsVisible;
            if (GalleryVisible)
            {
                ShowGalleryButton.GetComponent<ShowPDFGalleryButtonScript>().HideGalleryWindow();
            }
        }
    }
}