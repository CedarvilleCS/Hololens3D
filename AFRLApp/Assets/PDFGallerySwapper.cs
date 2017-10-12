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
        GameObject ShowGalleryButton = ImagePaneCollection.transform.Find("ShowPDFGalleryButton").gameObject;
        GameObject PDFGallery = ImagePaneCollection.transform.Find("PDFGallery").gameObject;
        GameObject MainPDFPane = ImagePaneCollection.transform.Find("AnnotatedPDF").gameObject;

        int numPDFs = ImagePaneCollection.GetComponent<PDFReceiver>().NumRcvdPDFs;

        Debug.Log("PDF ID is " + ImageId);
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