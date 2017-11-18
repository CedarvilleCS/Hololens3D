using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PDFReceiver : MonoBehaviour
{
    private PDFDocument _nextPDF;
    private bool _newPDFPresent;
    public bool FirstInstance = true;
    public int NumRcvdPDFs = 0;
    public int ResetNumRcvdPDFs;
    public List<PDFDocument> documents;

    void Awake()
    {
        documents = new List<PDFDocument>();
        HLNetwork.ObjectReceiver objr = HLNetwork.ObjectReceiver.getTheInstance();
        objr.PDFReceived += OnPDFReceived;
        if (!FirstInstance)
        {
            NumRcvdPDFs = ResetNumRcvdPDFs;
        }
    }


    void Update()
    {
        if (_newPDFPresent)
        {
            documents.Add(_nextPDF);
            _newPDFPresent = false;
            Debug.Log("Received new pdf");
            NumRcvdPDFs++;

            GameObject PDFGallery = this.transform.Find("PDFGallery").gameObject;
            GameObject PDFViewer = this.transform.Find("PDFViewer").gameObject;

            PDFGallery.GetComponent<PDFGalleryController>().RcvNewPDF(_nextPDF, NumRcvdPDFs);
            PDFViewer.GetComponent<PDFViewerController>().RcvNewPDF(_nextPDF, NumRcvdPDFs);

            

            int i = 0;
        } 
    }

    void OnPDFReceived(object obj, HLNetwork.PDFReceivedEventArgs args)
    {
        _nextPDF = args.PDFDoc;
        _newPDFPresent = true;
    }

    public void OnWindowClosed()
    {
        HLNetwork.ObjectReceiver objr = HLNetwork.ObjectReceiver.getTheInstance();
        objr.PDFReceived -= OnPDFReceived;
    }

    public int GetNumDocuments()
    {
        return documents.Count;
    }
}