using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PDFViewerController : MonoBehaviour {

    bool isVisible;
    public PDFDocument currentDocument;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ShowWindow()
    {
        this.enabled = true;
        isVisible = true;
    }

    public void HideWindow()
    {
        this.enabled = false;
        isVisible = false;
    }

    public void RcvNewPDF(PDFDocument newPDF, int NumRcvdPDFs)
    {
        if (NumRcvdPDFs == 1)
        {
            //Display the PDF in the image viewer
        }
        
    }
}
