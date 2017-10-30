using UnityEngine;
using System.Collections;

public class PDFGallerySwapper : MonoBehaviour
{
    public int PDFId;

    public void OnSelect()
    {
        GameObject PDFPane = this.transform.root.gameObject;
        GameObject ShowGalleryButton = PDFPane.transform.Find("ShowPDFGalleryButton").gameObject;
        GameObject PDFGallery = PDFPane.transform.Find("PDFGallery").gameObject;
        GameObject PDFViewer = PDFPane.transform.Find("PDFViewer").gameObject;

        int numPDFs = PDFPane.GetComponent<PDFReceiver>().documents.Count;
        //int numPDFs = GameObject.Find("Managers").GetComponent<DataManager>().documents.Count;

        Debug.Log("PDF ID is " + PDFId);
        Debug.Log("NumRcvdPDFs is " + numPDFs);

        if (PDFId < numPDFs)
        {
            Debug.Log("Inside PDFGallerySwapper.OnSelect");

            PDFViewer.GetComponent<PDFViewerController>().ShowPDFFromIndex(PDFId);

            PDFGallery.GetComponent<PDFGalleryController>().UpdateCurrGalleryIndex(PDFId);
            bool GalleryVisible = PDFGallery.GetComponent<PDFGalleryController>().GalleryIsVisible;
            if (GalleryVisible)
            {
                ShowGalleryButton.GetComponent<ShowPDFGalleryButtonScript>().HideGalleryWindow();
            }
        }
    }
}