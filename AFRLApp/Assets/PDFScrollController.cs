using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PDFScrollController : MonoBehaviour {
    public bool IsDown;
    private int _upOrDown;
	// Use this for initialization
	void Start () {
		//TODO
	}
	
	// Update is called once per frame
	void Update () {
		//TODO
	}

    private PDFDocument GetCurrDoc()
    {
        return this.GetComponentInParent<PDFViewerController>().GetCurrDoc();
    }
}
