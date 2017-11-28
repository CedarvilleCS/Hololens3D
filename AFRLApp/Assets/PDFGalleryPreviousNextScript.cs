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
    private Transform gallery;
    private bool turningPage;

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
        turningPage = false;
    }

    private void Update()
    {
        currentPageNum = GetGalleryCurrentPageNum();
        List<PDFDocument> docs = GetComponentInParent<PDFReceiver>().documents;
        int maxPages = docs.Count / 15;
        isVisible = this.transform.parent.GetComponentInChildren<PDFGalleryController>().GalleryIsVisible;
        //Controls for appearance depend on whether it is the previous button on the next button

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


        if (turningPage)
        {
            int startIndex = (currentPageNum) * 15;

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
            turningPage = false;
        }

    }

    void OnSelect()
    {
        gallery = GameObject.Find("PDFGallery").transform;
        docs = GetComponentInParent<PDFReceiver>().documents;
        gallery.GetComponent<PDFGalleryController>().currentPageNum = currentPageNum + _nextOrPrevious;
        currentPageNum = currentPageNum + _nextOrPrevious;
        turningPage = true;
    }

    private int GetGalleryCurrentPageNum()
    {
        Transform gallery = GameObject.Find("PDFGallery").transform;
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