using UnityEngine;
using System.Collections;
using Assets.UIScripts.ImageGallery;
using System;
using System.IO.Compression;
using System.IO;
#if WINDOWS_UWP
using System.Threading.Tasks;
#endif

public class ImageReceiver : MonoBehaviour
{
    private byte[] _nextImageData;
    private bool _newImagePresent;
    public bool FirstInstance = true;
    public int NumRcvdImages = 0;
    public int ResetNumRcvdImages;
    private PanoImage[] panoImages = new PanoImage[5];
    private Vector3 starterScale;
    private bool _newPanoRequestRecieved;
    private string _panoIp;
    public Transform imagePopout;
    public AnnotatedImageController aic;
    private Vector3 popoutPosition;

    void Start()
    {
        HLNetwork.ObjectReceiver objr = HLNetwork.ObjectReceiver.getTheInstance();
        objr.JpegReceived += OnJpegReceived;
        objr.PanoramaRequestReceived += OnPanoramaRequestReceived;
        if (!FirstInstance)
        {
            NumRcvdImages = ResetNumRcvdImages;
        }

        starterScale = this.transform.localScale;
        _newPanoRequestRecieved = false;
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

        if (_newPanoRequestRecieved)
        {
            GameObject PanoPopup = GameObject.Find("PanoramaPopup").gameObject;
            PanoPopup.GetComponent<PanoPopupController>().OnPanoRequestReceived(_panoIp);

            _newPanoRequestRecieved = false;
        }
    }

    internal void MakeNewPopOut()
    {
        Transform newPopout = Instantiate(imagePopout, this.transform.position, this.transform.rotation );

        newPopout.GetComponent<Renderer>().material = aic.GetCurrentImage();
        newPopout.GetComponentInChildren<PlaceButtonScript>().OnSelect();
    }

    public bool ReceivePanoJpeg(PanoImage image, int panoNum)
    {
        panoImages[panoNum] = image;
        foreach (PanoImage img in panoImages)
        {
            if (img == null)
                return false;
        }
#if WINDOWS_UWP
        Task task = new Task(
            async() => {
                SendPanoImagesToSurface();
            }
        );
        task.Start();
        task.Wait();
#endif
        return true;
    }

    public void SendPanoImagesToSurface()
    {
        byte[] panoArray1 = panoImages[0].ToByteArray();
        byte[] panoArray2 = panoImages[1].ToByteArray();
        byte[] panoArray3 = panoImages[2].ToByteArray();
        byte[] panoArray4 = panoImages[3].ToByteArray();
        byte[] panoArray5 = panoImages[4].ToByteArray();
        byte[] finalArray = new byte[panoArray1.Length + panoArray2.Length +
                                     panoArray3.Length + panoArray4.Length +
                                     panoArray5.Length + 20];
        int index = 0;
        foreach (byte[] imageData in new byte[][] { panoArray1, panoArray2, panoArray3, panoArray4, panoArray5 })
        {
            byte[] length = BitConverter.GetBytes(imageData.Length);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(length);
            }
            Buffer.BlockCopy(length, 0, finalArray, index, 4);
            index += 4;
            Buffer.BlockCopy(imageData, 0, finalArray, index, imageData.Length);
            index += imageData.Length;
        }
        HLNetwork.ObjectReceiver objr = HLNetwork.ObjectReceiver.getTheInstance();
        objr.SendData(HLNetwork.ObjectReceiver.MessageType.PanoImage, finalArray);
    }

    void OnPanoramaRequestReceived(object obj, HLNetwork.PanoramaRequestReceivedEventArgs args)
    {
        _panoIp = args.ip;
        _newPanoRequestRecieved = true;
    }

    void OnJpegReceived(object obj, HLNetwork.JpegReceivedEventArgs args)
    {
        _nextImageData = args.Image;
        _newImagePresent = true;
    }

    public void OnWindowClosed()
    {
        HLNetwork.ObjectReceiver objr = HLNetwork.ObjectReceiver.getTheInstance();
        objr.JpegReceived -= OnJpegReceived;
    }

    internal void Show()
    {
        this.transform.localScale = starterScale;
    }

    internal void Hide()
    {
        this.transform.localScale = new Vector3(0, 0, 0);
    }
}