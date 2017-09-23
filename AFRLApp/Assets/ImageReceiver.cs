using UnityEngine;

public class ImageReceiver : MonoBehaviour
{
    private byte[] _nextImageData;
    private bool _newImagePresent;
    private bool _newTestImagePresent;
    public bool FirstInstance = true;
    public int NumRcvdImages = 0;
    public int ResetNumRcvdImages; 

    void Start()
    {
        HLNetwork.ObjectReceiver objr = HLNetwork.ObjectReceiver.getTheInstance();
        HLNetwork.ObjectReceiver testReceiver = HLNetwork.ObjectReceiver.getTestInstance();

        objr.JpegReceived += OnJpegReceived;
        testReceiver.JpegReceived += TestJpegReceived;

        if (!FirstInstance)
        {
            NumRcvdImages = ResetNumRcvdImages;
        }
    }


    void Update()
    {
        if (_newImagePresent || _newTestImagePresent)
        {
            NumRcvdImages++;
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(_nextImageData);

            GameObject ImageGallery = transform.Find("ImageGallery").gameObject;
            GameObject ImageQueue = transform.Find("ImageQueue").gameObject;
            ImageGallery.GetComponent<ImageGalleryController>().RcvNewImage(tex, NumRcvdImages);
            ImageQueue.GetComponent<ImageQueueController>().RcvNewImage(tex, NumRcvdImages);

            // Only load received image into main image pane if it is the first image received

            if (NumRcvdImages == 1 && !_newTestImagePresent)
            {
                GameObject AnnotatedImage = transform.Find("AnnotatedImage").gameObject;
                AnnotatedImage.GetComponent<AnnotatedImageController>().DisplayImage(tex);
            }

            _newImagePresent = _newTestImagePresent = false;
        }
    }

    void TestJpegReceived(object obj, HLNetwork.JpegReceivedEventArgs args)
    {
        _nextImageData = args.Image;
        _newTestImagePresent = true;
    }

    void OnJpegReceived(object obj, HLNetwork.JpegReceivedEventArgs args)
    {
        _nextImageData = args.Image;
        _newImagePresent = true;
    }

    public void OnWindowClosed()
    {
        HLNetwork.ObjectReceiver objr = HLNetwork.ObjectReceiver.getTheInstance();
        HLNetwork.ObjectReceiver testReceiver = HLNetwork.ObjectReceiver.getTestInstance();

        objr.JpegReceived -= OnJpegReceived;
        testReceiver.JpegReceived -= TestJpegReceived;
    }
}