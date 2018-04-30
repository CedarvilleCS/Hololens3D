using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PDFReceiver : MonoBehaviour
{
    private PDFDocument _nextPDF;
    private bool _newPDFPresent;
    public bool FirstInstance = true;
    public int NumRcvdPDFs = 0;
    public int ResetNumRcvdPDFs;
    public List<PDFDocument> documents;
    internal int docCount;
    public Transform PDFPopout;
    private GameObject PDFGallery;
    private GameObject PDFViewer;

    void Awake()
    {
        documents = new List<PDFDocument>();
        HLNetwork.ObjectReceiver objr = HLNetwork.ObjectReceiver.getTheInstance();
        objr.PDFReceived += OnPDFReceived;
        if (!FirstInstance)
        {
            NumRcvdPDFs = ResetNumRcvdPDFs;
        }
        starterScale = this.transform.localScale;
        PDFGallery = this.transform.Find("PDFGallery").gameObject;
        PDFViewer = this.transform.Find("PDFViewer").gameObject;
    }


    void Update()
    {
        if (_newPDFPresent)
        {
            documents.Add(_nextPDF);
            _newPDFPresent = false;
            Debug.Log("Received new pdf");
            NumRcvdPDFs++;

            PDFGallery.GetComponent<PDFGalleryController>().RcvNewPDF(_nextPDF, NumRcvdPDFs);
            PDFViewer.GetComponent<PDFViewerController>().RcvNewPDF(_nextPDF, NumRcvdPDFs);

            docCount = documents.Count;

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

    private Vector3 starterScale;

    internal void Show()
    {
        this.transform.localScale = starterScale;
    }

    internal void Hide()
    {
        this.transform.localScale = new Vector3(0, 0, 0);
    }

    internal void MakeNewPopOut()
    {
        Transform newPopout = Instantiate(PDFPopout, this.transform.position, this.transform.rotation);
        newPopout.GetComponent<PDFViewerController>().ShowPDFFromIndex(this.GetComponentInChildren<PDFViewerController>().currentDocument.Id, true);
    }
}