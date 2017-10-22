using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PDFViewerController : MonoBehaviour {

    bool isVisible;
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
        //TODO
        i
    }
}
