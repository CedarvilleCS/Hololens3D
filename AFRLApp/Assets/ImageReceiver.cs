using UnityEngine;
using System.Collections;

public class ImageReceiver : MonoBehaviour
{
    private byte[] _nextImageData;
    private bool _newImagePresent;
    public bool FirstInstance = true;
    public int NumRcvdImages = 0;
    public int ResetNumRcvdImages;

    void Start()
    {
        HLNetwork.ObjectReceiver objr = HLNetwork.ObjectReceiver.getTheInstance();
        objr.JpegReceived += OnJpegReceived;
        if(!FirstInstance)
        {
            NumRcvdImages = ResetNumRcvdImages;
        }
    }


    void Update()
    {
        if (_newImagePresent)
        {
            NumRcvdImages++;
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(_nextImageData);
            
            GameObject ImageGallery = this.transform.Find("ImageGallery").gameObject;
            GameObject ImageQueue = this.transform.Find("ImageQueue").gameObject;
            ImageGallery.GetComponent<ImageGalleryController>().RcvNewImage(tex, NumRcvdImages);
            ImageQueue.GetComponent<ImageQueueController>().RcvNewImage(tex, NumRcvdImages);

            // Only load received image into main image pane if it is the first image received

            if (NumRcvdImages == 1)
            {
                GameObject AnnotatedImage = this.transform.Find("AnnotatedImage").gameObject;
                AnnotatedImage.GetComponent<AnnotatedImageController>().DisplayImage(tex);
            }
            
            _newImagePresent = false;
        }
    }

    void OnJpegReceived(object obj, HLNetwork.JpegReceivedEventArgs args)
    {
        _nextImageData = args.Image;
        _newImagePresent = true;
    }
}