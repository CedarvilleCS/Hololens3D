using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PDFScrollController : MonoBehaviour
{
    public bool IsDown;
    private int _upOrDown;
    public GameObject Sibling;
    public PDFDocument PDF;
    public bool isVisible;
    public GameObject[] PDFPages;
    private Renderer[] renderers;
    private bool _scrolling;
    private Texture2D _blankTex;
    public PDFViewerController ViewerReference;

    // Use this for initialization
    void Start()
    {
        ViewerReference = this.transform.parent.GetComponent<PDFViewerController>();

        _blankTex = Resources.Load("DefaultPageTexture") as Texture2D;

        PDF = null;

        Transform PageHolder = ViewerReference.transform.Find("PDFPages");
        renderers = new Renderer[PageHolder.transform.childCount];
        PDFPages = new GameObject[PageHolder.transform.childCount];
        int i = 0;
        foreach (Transform child in PageHolder.transform)
        {
            PDFPages[i] = child.gameObject;
            renderers[i] = PDFPages[i].GetComponent<Renderer>();
            i++;
        }

        if (IsDown)
        {
            _upOrDown = 1;
        }
        else //isUp
        {
            _upOrDown = -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_scrolling)
        {
            Hide();
            _scrolling = false;
            ViewerReference.pageIncrement = ViewerReference.pageIncrement + _upOrDown;
            int pageGroup = ViewerReference.pageIncrement * 3;
            PDF = GetCurrDoc();

            for (int i = 0; i < PDFPages.Length; i++)
            {
                Texture2D tex = new Texture2D(2, 2);
                if (i + pageGroup < PDF.Pages.Count)
                {
                    tex.LoadImage(PDF.Pages[i + pageGroup]);
                    PDFPages[i].GetComponent<PDFPageController>().pageNum = i + pageGroup;

                }
                else
                {
                    tex = _blankTex;
                    PDFPages[i].GetComponent<PDFPageController>().pageNum = -1;
                }
                renderers[i].material.mainTexture = tex;
            }
            CheckStatus();
            Sibling.GetComponent<PDFScrollController>().CheckStatus();
        }

    }

    public void CheckStatus()
    {
        PDF = GetCurrDoc();
        if (PDF == null)
        {
            Hide();
        }
        else
        {
            int pageGroup = ViewerReference.pageIncrement;
            if (IsDown)
            {
                //If there are less than 3 pages 
                if (PDF.Pages.Count <= PDFPages.Length || PDF.Pages.Count < (pageGroup + 1) * PDFPages.Length)
                {
                    Hide();
                }
                else
                {
                    Show();
                }
            }
            else //isUp
            {
                if (pageGroup > 0)
                {
                    Show();
                }
                else
                {
                    Hide();
                }
            }
        }
    }

    public void Hide()
    {
        this.transform.localScale = new Vector3(0, 0, 0);
        isVisible = false;
    }

    public void Show()
    {
        this.transform.localScale = new Vector3(0.08312999f, 0.08312999f, 1e-05f);
        isVisible = true;
    }

    private PDFDocument GetCurrDoc()
    {
        return ViewerReference.currentDocument;
    }

    public void OnSelect()
    {
        _scrolling = true;
    }
}
