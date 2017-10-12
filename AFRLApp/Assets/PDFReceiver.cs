using UnityEngine;
using System.Collections;
using Datatypes;

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
        objr.OnPDFReceived += OnPDFReceived;
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
            foreach(byte[] page in _nextPDF.pages)
            {
                Texture2D tex = new Texture2D(2, 2);
                tex.LoadImage(page);
                textures.add(tex);
            }

            GameObject PDFGallery = this.transform.Find("PDFGallery").gameObject;
            GameObject PDFQueue = this.transform.Find("PDFQueue").gameObject;
            PDFGallery.GetComponent<PDFGalleryController>().RcvNewPDF(textures, NumRcvdPDFs);
            PDFQueue.GetComponent<PDFQueueController>().RcvNewPDF(textures, NumRcvdPDFs);

            // Only load received image into main image pane if it is the first image received

            if (NumRcvdPDFs == 1)
            {
                GameObject AnnotatedPDF = this.transform.Find("AnnotatedPDF").gameObject;
                AnnotatedPDF.GetComponent<AnnotatedPDFController>().DisplayImages(textures);
            }

            _newPDFPresent = false;
        }
    }

    void OnPDFReceived(object obj, HLNetwork.PDFReceivedEventArgs args)
    {
        _nextPDF = args.PDFdoc;
        _newPDFPresent = true;
    }

    public void OnWindowClosed()
    {
        HLNetwork.ObjectReceiver objr = HLNetwork.ObjectReceiver.getTheInstance();
        objr.PDFReceived -= OnPDFReceived;
    }
}