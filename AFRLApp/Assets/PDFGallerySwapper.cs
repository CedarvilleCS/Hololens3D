using UnityEngine;
using System.Collections;

public class PDFGallerySwapper : MonoBehaviour
{
    public int PDFId;

    // Use this for initialization
    void Start()
    {

    }

    public void OnSelect()
    {
        GameObject PDFPaneCollection = this.transform.root.gameObject;
        GameObject ShowGalleryButton = PDFPaneCollection.transform.Find("ShowPDFGalleryButton").gameObject;
        GameObject PDFGallery = PDFPaneCollection.transform.Find("PDFGallery").gameObject;
        GameObject MainPDFPane = PDFPaneCollection.transform.Find("AnnotatedPDF").gameObject;

        int numPDFs = PDFPaneCollection.GetComponent<PDFReceiver>().NumRcvdPDFs;

        Debug.Log("PDF ID is " + PDFId);
        Debug.Log("NumRcvdPDFs is " + numPDFs);

        if (PDFId <= numPDFs - 1)
        {
            Debug.Log("Inside PDFGallerySwapper.OnSelect");
            Renderer PDFRenderer = this.GetComponent<Renderer>();
            Texture PDFPageTexture = PDFRenderer.material.mainTexture;
            MainPDFPane.GetComponent<AnnotatedPDFController>().DisplayPDF(PDFPageTexture);

            PDFGallery.GetComponent<PDFGalleryController>().UpdateCurrGalleryIndex(PDFId);
            bool GalleryVisible = PDFGallery.GetComponent<PDFGalleryController>().GalleryIsVisible;
            if (GalleryVisible)
            {
                ShowGalleryButton.GetComponent<ShowPDFGalleryButtonScript>().hideGalleryWindow();
            }
        }
    }
}