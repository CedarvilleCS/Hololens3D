﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PDFReceiver : MonoBehaviour
{
    private PDFDocument _nextPDF;
    private bool _newPDFPresent;
    public bool FirstInstance = true;
    public int NumRcvdPDFs = 0;
    public int ResetNumRcvdPDFs;

    void Start()
    {
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
            NumRcvdPDFs++;
      
            GameObject PDFGallery = this.transform.Find("PDFGallery").gameObject;
            GameObject PDFViewer = this.transform.Find("PDFViewer").gameObject;
            //TODO: Set the first page of the PDF as the "icon" of the PDF gallery
            //Notify of new PDF (however we wanna do that)
            PDFGallery.GetComponent<PDFGalleryController>().RcvNewPDF(_nextPDF, NumRcvdPDFs);
            PDFViewer.GetComponent<PDFViewerController>().RcvNewPDF(_nextPDF, NumRcvdPDFs);

            GameObject.Find("Managers").GetComponent<DataManager>().documents.Add(_nextPDF);

            _newPDFPresent = false;
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
}