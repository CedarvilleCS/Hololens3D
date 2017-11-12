using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PDFPageSwapper : MonoBehaviour {

    public int PDFPageNum;

    void OnSelect()
    {
        GameObject ViewerController = this.transform.root.transform.Find("PDFViewer").gameObject;
        ViewerController.GetComponent<PDFViewerController>().SetVisiblePage(PDFPageNum);

        this.GetComponentInParent<PDFPageController>().PageNum = PDFPageNum;
    }

    public void SetPage(GameObject PDFPages, int pageNum, int PDFId)
    {
        GameObject PDFPane = this.transform.root.gameObject;
        List<PDFDocument> documents = PDFPane.GetComponent<PDFReceiver>().documents;
        PDFDocument pdf = documents[PDFId];

        byte[] page = pdf.pages[pageNum];
        Renderer currObjRenderer = this.transform.GetComponent<Renderer>();
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(page);
        currObjRenderer.material.mainTexture = tex;

        PDFPageNum = pageNum;
    }
}
