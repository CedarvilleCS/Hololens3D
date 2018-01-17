using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PDFScrollController : MonoBehaviour
{
    public bool IsDown;
    private int _upOrDown;
    public int pageIncrement { get; set; }
    public GameObject Sibling;
    public PDFDocument PDF;
    public bool isVisible;
    public GameObject[] PDFPages;
    private Renderer[] renderers;
    private bool _scrolling;

    // Use this for initialization
    void Start()
    {
        if (IsDown)
        {
            _upOrDown = 3;
        }
        else //isUp
        {
            _upOrDown = -3;
        }

        PDF = null;

        GameObject PageHolder = GameObject.Find("PDFPages");
        renderers = new Renderer[PageHolder.transform.childCount];
        PDFPages = new GameObject[PageHolder.transform.childCount];
        int i = 0;
        foreach (Transform child in PageHolder.transform)
        {
            PDFPages[i] = child.gameObject;
            renderers[i] = PDFPages[i].GetComponent<Renderer>();
            i++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_scrolling)
        {
            Hide();
            _scrolling = false;
            PDF = GetCurrDoc();

            for (int i = 0; i < PDFPages.Length; i++)
            {
                Texture2D tex = new Texture2D(2, 2);
                tex.LoadImage(PDF.pages[i]);
                renderers[i].material.mainTexture = tex;
            }

            Sibling.GetComponent<PDFScrollController>().pageIncrement = this.pageIncrement;

        }

        if (PDF == null)
        {
            Hide();
        }
        else
        {
            if (IsDown)
            {
                //If there are less than 3 pages ore 
                if (PDF.pages.Count < 3 || PDF.pages.Count < (pageIncrement + 1) * 3)
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
                if (pageIncrement > 0)
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
        return this.GetComponentInParent<PDFViewerController>().currentDocument;
    }

    public void Onselect()
    {
        pageIncrement = pageIncrement + _upOrDown;
        _scrolling = true;
    }
}
