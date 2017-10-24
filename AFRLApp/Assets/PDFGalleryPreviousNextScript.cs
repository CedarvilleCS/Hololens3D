using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PDFGalleryPreviousNextScript : MonoBehaviour
{
    public bool isNext;
    private int _nextOrPrevious;
    bool isVisible;
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
        this.Hide();
    }

    private void Update()
    {
        int currentPageNum = GetCurrentPageNum();
        List<PDFDocument> docs = GetComponentInParent<PDFReceiver>().documents;
        int maxPages = docs.Count / 15;

        //Controls for appearance depend on whether it is the previous button on the next button
        if (isVisible)
        {
            if (isNext)
            {
                if (currentPageNum < maxPages)
                {
                    this.enabled = true;
                }
                else
                {
                    this.enabled = false;
                }
            }
            else
            {
                if (currentPageNum > 0)
                {
                    this.enabled = true;
                }
                else
                {
                    this.enabled = false;
                }
            }
        }
    }
    void OnSelect()
    {
        Transform gallery = GameObject.Find("PDFGallery").transform;
        int currentPageNum = GetCurrentPageNum();
        int startIndex = (currentPageNum + _nextOrPrevious) * 15;

        List<PDFDocument> docs = GameObject.Find("Managers").GetComponent<DataManager>().documents;

        //Use the first page of the PDF as a thumbnail
        for (int i = 0; i < 14; i++)
        {
            gallery.GetComponent<PDFGalleryController>().SetThumbnail(docs[startIndex + i], i);
        }

        gallery.GetComponent<PDFGalleryController>().currentPageNum = currentPageNum + _nextOrPrevious;

        if (isNext)
        {
            if (currentPageNum < (docs.Count / 15))
            {
                this.enabled = true;
            }
            else
            {
                this.enabled = false;
            }
        }
        else
        {
            if (currentPageNum > 0)
            {
                this.enabled = true;
            }
            else
            {
                this.enabled = false;
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
        return (GetCurrentPageNum() < (GameObject.Find("Managers").GetComponent<DataManager>().documents.Count / 15));
    }

    public bool HasPrevious()
    {
        return (GetCurrentPageNum() > 0);
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
