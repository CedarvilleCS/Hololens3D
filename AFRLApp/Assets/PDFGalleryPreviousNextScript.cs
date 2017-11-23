using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PDFGalleryPreviousNextScript : MonoBehaviour
{
    public bool isNext;
    private int _nextOrPrevious;
    bool isVisible;
    private int currentPageNum;
    void Start()
    {

        if (isNext)
        {
            _nextOrPrevious = 1;
        }
        else
        {
            _nextOrPrevious = -1;
        }
        isVisible = true;
        Hide();
        currentPageNum = 0;
    }

    private void Update()
    {
        currentPageNum = GetCurrentPageNum();
        List<PDFDocument> docs = GetComponentInParent<PDFReceiver>().documents;
        int maxPages = docs.Count / 15;
        isVisible = this.transform.parent.GetComponentInChildren<PDFGalleryController>().GalleryIsVisible;

        //Controls for appearance depend on whether it is the previous button on the next button
        if (isVisible)
        {
            if (isNext)
            {
                if (currentPageNum < maxPages)
                {
                    this.Show();
                }
                else
                {
                    this.Hide();
                }
            }
            else //isPrevious
            {
                if (currentPageNum > 0)
                {
                    this.Show();
                }
                else
                {
                    this.Hide();
                }
            }
        }
    }
    void OnSelect()
    {
        Transform gallery = GameObject.Find("PDFGallery").transform;
        int startIndex = (currentPageNum + _nextOrPrevious) * 15;

        List<PDFDocument> docs = GetComponentInParent<PDFReceiver>().documents;
        int maxPages = (docs.Count) / 15;

        //Use the first page of the PDF as a thumbnail
        for (int i = 1; i <= 15; i++)
        {
            if (docs.Count >= (startIndex + i))
            {
                gallery.GetComponent<PDFGalleryController>().SetThumbnail(docs[startIndex + i - 1], i);
            }
            else
            {
                gallery.GetComponent<PDFGalleryController>().SetThumbnail(null, i);
            }
        }

        gallery.GetComponent<PDFGalleryController>().currentPageNum = currentPageNum + _nextOrPrevious;
        currentPageNum = currentPageNum + _nextOrPrevious;
        if (isNext)
        {
            if (currentPageNum < ((docs.Count - 1) / 15))
            {
                this.Show();
            }
            else
            {
                this.Hide();
            }
        }
        else
        {
            if (currentPageNum > 0)
            {
                this.Show();
            }
            else
            {
                this.Hide();
            }
        }
    }
    private int GetCurrentPageNum()
    {
        Transform gallery = GameObject.Find("PDFGallery").transform;
        return gallery.GetComponent<PDFGalleryController>().currentPageNum;
    }

    public bool HasNext()
    {
        return (GetCurrentPageNum() < (GetComponentInParent<PDFReceiver>().documents.Count / 15));
    }

    public bool HasPrevious()
    {
        return (GetCurrentPageNum() > 0);
    }

    public void Hide()
    {
        if (isVisible)
        {
            this.transform.localScale = new Vector3(0, 0, 0);
            isVisible = false;
        }
    }

    public void Show()
    {
        if (!isVisible)
        {
            this.transform.localScale = new Vector3(0.1009104f, 0.1009104f, 0);
            isVisible = true;
        }
    }
}