﻿using UnityEngine;
using System.Collections;

public class ImageQueueController : MonoBehaviour
{
    private GameObject ImagePaneCollection;
    private GameObject ImageGallery;
    public GameObject[] queueImagePanes { get; private set; }

    // Use this for initialization
    void Start()
    {
        ImagePaneCollection = this.transform.root.gameObject;
        ImageGallery = ImagePaneCollection.transform.Find("ImageGallery").gameObject;
        int numQueuePanes = this.transform.childCount;
        queueImagePanes = new GameObject[numQueuePanes];
        Debug.Log("num of queue panes is " + numQueuePanes);
        for (int i = 0; i < queueImagePanes.Length; i++)
        {
            Debug.Log("Loop " + i + " of queue array assignment");
            queueImagePanes[i] = this.transform.GetChild(i).gameObject;
        }
    }

    public void updateCurrViewedQueuePane(int NextGalleryIndex)
   { 
        Debug.Log("Inside ImageQueueController.updateCurrViewedQueuePane");
        int NumImagesRcvd = ImagePaneCollection.GetComponent<ImageReceiver>().numRcvdImages;
        if (NumImagesRcvd > 0)
        {
            ImageGallery.GetComponent<ImageGalleryController>().OnSelectByIndex(NextGalleryIndex);
        }
    }
}