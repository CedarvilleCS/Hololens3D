using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PDFPageController : MonoBehaviour {
    public int pageNum;
    private void Awake()
    {
        pageNum = -1;
    }

    public void OnSelect()
    {
        this.transform.parent.GetComponentInParent<PDFViewerController>().SetPageVisible(pageNum);
    }
}
