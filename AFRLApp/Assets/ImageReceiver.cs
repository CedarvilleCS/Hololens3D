using UnityEngine;
using System.Collections;

public class ImageReceiver : MonoBehaviour
{
    private byte[] _nextImageData;
    private bool _newImagePresent;
    public int numRcvdImages { get; private set; }

    void Start()
    {
        HLNetwork.ObjectReceiver objr = new HLNetwork.ObjectReceiver();
        objr.JpegReceived += OnJpegReceived;
        numRcvdImages = 0;
    }


    void Update()
    {
        if (_newImagePresent)
        {
            numRcvdImages++;
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(_nextImageData);
            
            GameObject ImageGallery = this.transform.Find("ImageGallery").gameObject;
            GameObject ImageQueue = this.transform.Find("ImageQueue").gameObject;
            GameObject AnnotatedImage = this.transform.Find("AnnotatedImage").gameObject;
            ImageGallery.GetComponent<ImageGalleryController>().RcvNewImage(tex, numRcvdImages);
            ImageQueue.GetComponent<ImageQueueController>().RcvNewImage(tex, numRcvdImages);
            AnnotatedImage.GetComponent<AnnotatedImageController>().DisplayImage(tex);

            _newImagePresent = false;
        }
    }

    void OnJpegReceived(object obj, HLNetwork.JpegReceivedEventArgs args)
    {
        _nextImageData = args.Image;
        _newImagePresent = true;
    }
}