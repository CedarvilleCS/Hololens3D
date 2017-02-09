using UnityEngine;
using System.Collections;

public class ImageReceiver : MonoBehaviour
{
    private byte[] _nextImageData;
    private bool _newImagePresent;
    private GameObject[] queueImages;
    private GameObject[] galleryImages;

    void Start()
    {
        System.Diagnostics.Debug.WriteLine("Inside Start of ImageReceiver");
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

        // var mainImageRenderer = this.gameObject.transform.parent.gameObject.transform.parent.gameObject.GetComponent<Renderer>();
        System.Diagnostics.Debug.WriteLine("DummyCommand finished Start() funciton");
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

            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(_nextImageData);
            var renderer = this.gameObject.GetComponent<Renderer>();

            // If the main image pane isn't blank, shift

            //if(renderer.material.mainTexture != null)
            //{
                ShiftImages();
            //}

            // Load received image onto the main image pane

            renderer.material.mainTexture = tex;
            //renderer.material.SetTextureScale("_MainTex", new Vector2(-1, -1));
            _newImagePresent = false;
        }
    }

    void OnJpegReceived(object obj, HLNetwork.JpegReceivedEventArgs args)
    {
        _nextImageData = args.Image;
        _newImagePresent = true;
    }

    void ShiftImages()
    {
        // shift the image queue to the right

        for (int i = queueImages.Length-1; i > 0; i--)
        {
            Debug.Log("i is " + i);

            var prevObj = queueImages[i-1];
            var currObj = queueImages[i];

            var prevObjRenderer = prevObj.GetComponent<Renderer>();
            var prevObjTexture = prevObjRenderer.material.mainTexture;
            prevObjRenderer.material.mainTexture = null;

            var currObjRenderer = currObj.GetComponent<Renderer>();
            var currObjTexture = currObjRenderer.material.mainTexture;
            currObjRenderer.material.mainTexture = null;

            currObjRenderer.material.mainTexture = prevObjTexture;
        }

        // shift main image into the first image queue slot

        var mainImage = this.gameObject;
        var mainImageRenderer = mainImage.GetComponent<Renderer>();
        var mainImageTexture = mainImageRenderer.material.mainTexture;
        mainImageRenderer.material.mainTexture = null;

        var firstObj = queueImages[0];
        var firstObjRenderer = firstObj.GetComponent<Renderer>();
        var firstObjTexture = firstObjRenderer.material.mainTexture;
        firstObjRenderer.material.mainTexture = null;

        firstObjRenderer.material.mainTexture = mainImageTexture;
        //firstObjRenderer.material.SetTextureScale("_MainTex", new Vector2(-1, -1));
    }
}