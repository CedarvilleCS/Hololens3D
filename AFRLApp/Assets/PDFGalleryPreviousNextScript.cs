using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PDFGalleryPreviousNextScript : MonoBehaviour
{
    public bool isNext;
    private int _nextOrPrevious;
    void Start()
    {
        this.enabled = false;
        if (isNext)
        {
            _nextOrPrevious = 1;
        }
        else
        {
            _nextOrPrevious = -1;
        }
    }

    private void Update()
    {

        int currentPageNum = GetCurrentPageNum();
        List<PDFDocument> docs = GetComponentInParent<PDFReceiver>().documents;
        //List<PDFDocument> docs = GameObject.Find("Managers").GetComponent<DataManager>().documents;
        int maxPages = docs.Count / 15;

        //Controls for appearance depend on whether it is the previous button on the next button
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
    void OnSelect()
    {
        Transform gallery = GameObject.Find("PDFGallery").transform;
        int currentPageNum = GetCurrentPageNum();
        int startIndex = (currentPageNum + _nextOrPrevious) * 15;

        List<PDFDocument> docs = GetComponentInParent<PDFReceiver>().documents;
        //List<PDFDocument> docs = GameObject.Find("Managers").GetComponent<DataManager>().documents;
        int maxPages = (docs.Count) / 15;

        List<PDFDocument> pagesToShow = docs.GetRange(startIndex, 15);

        //Use the first page of the PDF as a thumbnail
        for (int i = 0; i < 14; i++)
        {
            Transform child = gallery.GetChild(i);
            byte[] firstPage = docs[startIndex + i].pages[0];
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(firstPage);
            child.GetComponent<Renderer>().material.mainTexture = tex;
        }

        gallery.GetComponent<PDFGalleryController>().currentPageNum = currentPageNum + _nextOrPrevious;
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
}
