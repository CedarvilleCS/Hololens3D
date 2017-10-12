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
            List<Texture2D> textures = new List<Texture2D>();
            for(int i = 0; i<_nextPDF.pages.Count; i++)
            {
                byte[] page = _nextPDF.pages[i];
                Texture2D tex = new Texture2D(2, 2);
                tex.LoadImage(page);
                textures.Add(tex);
            }

            GameObject PDFGallery = this.transform.Find("PDFGallery").gameObject;
            GameObject PDFQueue = this.transform.Find("PDFQueue").gameObject;
            //TODO: Set the first page of the PDF as the "icon" of the PDF gallery
            //Notify of new PDF (however we wanna do that)
            PDFGallery.GetComponent<PDFGalleryController>().RcvNewPDF(textures, NumRcvdPDFs);

            

            // Only load received image into main image pane if it is the first image received

            if (NumRcvdPDFs == 1)
            {
                //TODO: If its the first PDF, make it appear in the viewer
                //May want to use an "OnPDFSelected(PDFId)" function
                PDFPages.GetComponent<PDFPagesController>().RcvNewPDF(textures, NumRcvdPDFs);
                GameObject AnnotatedPDF = this.transform.Find("AnnotatedPDF").gameObject;
                AnnotatedPDF.GetComponent<AnnotatedPDFController>().DisplayPDF(textures);//Still in progress - JR
            }

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