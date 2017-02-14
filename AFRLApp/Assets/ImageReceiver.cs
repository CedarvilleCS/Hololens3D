using UnityEngine;
using System.Collections;

public class ImageReceiver : MonoBehaviour
{
    private byte[] _nextImageData;
    private bool _waitingForFirstImage;
    private bool _newImagePresent;
    private GameObject[] queueImages;
    private GameObject[] galleryImages;
    private int numRcvdImages;

    void Start()
    {
     
        HLNetwork.ObjectReceiver objr = new HLNetwork.ObjectReceiver();
        objr.JpegReceived += OnJpegReceived;

        // initialize image arrays

        int numQueueImages = this.gameObject.transform.GetChild(0).childCount;
        int numGalleryImages = this.gameObject.transform.GetChild(1).childCount;

        queueImages = new GameObject[numQueueImages];
        galleryImages = new GameObject[numGalleryImages];

        for (int i = 0; i < queueImages.Length; i++)
        {
            queueImages[i] = this.gameObject.transform.GetChild(0).GetChild(i).gameObject;
        }


        for (int i = 0; i < galleryImages.Length; i++)
        {
            galleryImages[i] = this.gameObject.transform.GetChild(1).GetChild(i).gameObject;
        }

        Debug.Log("Number of queue images:   " + queueImages.Length);
        Debug.Log("Number of gallery images: " + galleryImages.Length);

        // used to determine when first image arrives

        _waitingForFirstImage = true;
    }
    /*
    // Called by GazeGestureManager when the user performs a Select gesture

    // Currently, this pulls an image from the internet and assigns it to the floating black pane when the user performs a finger-tap on the floating black pane.
    // This was to prove that we could dynamically assign an image (that wasn't known by the Unity editor) to the image pane
    // Areas to change:
    // 1. This event should be called when an image is received, not when the user finger-taps on the screen
    // 2. The picture put on the image pane should be the one received from the Surface app, not a random imgur picture
    // 3. We need to figure out how to make the image not show up upside down when it is put on the image-pane
    IEnumerator OnSelect()
    {

        var url = "http://i.imgur.com/g3D5jNz.jpg";
        WWW www = new WWW(url);
        yield return www; //the yield return is to make the async web request work; it's irrelevant once we get images from the app instead of imgur.com
        var renderer = this.gameObject.GetComponent<Renderer>();
        // The way we to put an image on the pane (as far as we can tell) is to overwrite the mainTexture property of the pane's Renderer object
        renderer.material.mainTexture = www.texture; 
       
    }*/

    void Update()
    {
        if (_newImagePresent)
        {
            numRcvdImages++;
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(_nextImageData);

            if (!_waitingForFirstImage)
            {
                ShiftImages(numRcvdImages);

                var queueRenderer = queueImages[0].GetComponent<Renderer>();
                queueRenderer.material.mainTexture = tex;
                var galleryRenderer = galleryImages[0].GetComponent<Renderer>();
                galleryRenderer.material.mainTexture = tex;
            }
            else
            {
                // Load received image onto the main image pane and into queue

                var renderer = this.gameObject.GetComponent<Renderer>();
                renderer.material.mainTexture = tex;
                //renderer.material.SetTextureScale("_MainTex", new Vector2(-1, -1));
                var queueRenderer = queueImages[0].GetComponent<Renderer>();
                queueRenderer.material.mainTexture = tex;
                var galleryRenderer = galleryImages[0].GetComponent<Renderer>();
                galleryRenderer.material.mainTexture = tex;
                _waitingForFirstImage = false;
            }

            _newImagePresent = false;
        }
    }

    void OnJpegReceived(object obj, HLNetwork.JpegReceivedEventArgs args)
    {
        _nextImageData = args.Image;
        _newImagePresent = true;
    }

    void ShiftImages(int numRcvdImages)
    {
        // Determine minimum shift distance to avoid unnecesary operations

        var queueSize   = queueImages.Length - 1;
        var gallerySize = galleryImages.Length - 1;

        if (numRcvdImages < queueImages.Length)
        {
            queueSize = numRcvdImages;
        }
        if(numRcvdImages < gallerySize)
        {
            gallerySize = numRcvdImages;
        }

        // shift the image queue to the right

        for (int i = queueSize; i > 0; i--)
        {
            Debug.Log("i is " + i);

            var prevObj = queueImages[i-1];
            var currObj = queueImages[i];

            var prevObjRenderer = prevObj.GetComponent<Renderer>();
            var prevObjTexture = prevObjRenderer.material.mainTexture;

            var currObjRenderer = currObj.GetComponent<Renderer>();
            var currObjTexture = currObjRenderer.material.mainTexture;

            currObjRenderer.material.mainTexture = prevObjTexture;
        }

        // shift image gallery to the right
        
        for (int i = gallerySize - 1; i > 0; i--)
        {
            Debug.Log("i is " + i);

            var prevObj = galleryImages[i-1];
            var currObj = galleryImages[i];

            var prevObjRenderer = prevObj.GetComponent<Renderer>();
            var prevObjTexture = prevObjRenderer.material.mainTexture;
            prevObjRenderer.material.mainTexture = null;

            var currObjRenderer = currObj.GetComponent<Renderer>();
            var currObjTexture = currObjRenderer.material.mainTexture;
            currObjRenderer.material.mainTexture = null;

            currObjRenderer.material.mainTexture = prevObjTexture;
        }
    }
}