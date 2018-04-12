using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PDFGalleryPreviousNextScript : MonoBehaviour
{
    public bool isNext;
    private int _nextOrPrevious;
    bool isVisible;
    private int currentPageNum;
    private List<PDFDocument> docs;
    public Transform gallery;
    private bool _setThumbnails;
    private bool _galleryVisible;
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
        isVisible = false;
        Hide();
        currentPageNum = 0;
        _galleryVisible = false;
    }

    private void Update()
    {
        currentPageNum = GetGalleryCurrentPageNum();
        int docCount = GetComponentInParent<PDFReceiver>().docCount;
        int maxPages = (docCount - 1) / 15;
        isVisible = gallery.GetComponent<PDFGalleryController>().GalleryIsVisible;

        //Use the first page of the PDF as a thumbnail
        if (_setThumbnails)
        {
            _setThumbnails = false;
            int startIndex = (currentPageNum) * 15;
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
        }

        //Controls for appearance depend on whether it is the previous button on the next 
        if (_galleryVisible)
        {
            if (isNext)
            {
                if (currentPageNum < maxPages && maxPages > 0)
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

    internal void ShowWindow()
    {
        _galleryVisible = true;
    }

    internal void HideWindow()
    {
        Hide();
        _galleryVisible = false;
    }

    void OnSelect()
    {
        this.Hide();
        _setThumbnails = true;
        docs = GetComponentInParent<PDFReceiver>().documents;
        gallery.GetComponent<PDFGalleryController>().currentPageNum = currentPageNum + _nextOrPrevious;
        currentPageNum = currentPageNum + _nextOrPrevious;
    }
    private int GetGalleryCurrentPageNum()
    {
        return gallery.GetComponent<PDFGalleryController>().currentPageNum;
    }

    public bool HasNext()
    {
        return (GetGalleryCurrentPageNum() < (GetComponentInParent<PDFReceiver>().documents.Count / 15));
    }

    public bool HasPrevious()
    {
        return (GetGalleryCurrentPageNum() > 0);
    }

    public void Hide()
    {
        this.transform.localScale = new Vector3(0, 0, 0);
        isVisible = false;
    }

    public void Show()
    {
        this.transform.localScale = new Vector3(0.1009104f, 0.1009104f, 0);
        isVisible = true;
    }
}