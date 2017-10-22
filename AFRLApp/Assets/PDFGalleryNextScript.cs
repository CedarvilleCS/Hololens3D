using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PDFGalleryNextScript : MonoBehaviour
{


    void Start()
    {
        this.enabled = false;
    }
    void OnSelect()
    {
        Transform gallery = GameObject.Find("PDFGallery").transform;
        int pageNum = gallery.GetComponent<PDFGalleryController>().currentPageNum;
        List<PDFDocument> docs = GameObject.Find("Managers").GetComponent<DataManager>().documents;
        int maxPages = (docs.Count) / 15;
        int startIndex = pageNum * 15;
        List<PDFDocument> pagesToShow = docs.GetRange(startIndex, 15);

        for (int i = 0; i < 14; i++)
        {
            Transform child = gallery.GetChild(i);
            byte[] firstPage = docs[startIndex + i].pages[0];
            Texture PDFTexture = child.GetComponent<Renderer>().material.mainTexture;
            PDFTexture.
        }

        

        
    }
}
