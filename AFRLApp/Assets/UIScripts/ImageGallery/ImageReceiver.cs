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
            Buffer.BlockCopy(length, 0, finalArray, index, 4);
            index += 4;
            Buffer.BlockCopy(imageData, 0, finalArray, index, imageData.Length);
            index += imageData.Length;
        }
        byte[] compressedArray;
        using (var compressStream = new MemoryStream())
        {

            using (var compressor = new DeflateStream(compressStream, CompressionMode.Compress))
            {
                //finalArray.CopyTo(compressor);
                //compressor.Close();
                //return compressStream.ToArray();
                compressor.Write(finalArray, 0, finalArray.Length);

            }
            compressedArray = compressStream.ToArray();
        }

        HLNetwork.ObjectReceiver objr = HLNetwork.ObjectReceiver.getTheInstance();
        objr.SendData(HLNetwork.ObjectReceiver.MessageType.PanoImage, compressedArray);
    }

    public void OnPanoramaRequestReceived(object obj, HLNetwork.PanoramaRequestReceivedEventArgs args)
    {
        GameObject.Find("PanoramaPopup").GetComponent<PanoPopupController>().OnPanoRequestReceived(args.ip);
    }

    public void OnWindowClosed()
    {
        HLNetwork.ObjectReceiver objr = HLNetwork.ObjectReceiver.getTheInstance();
        objr.JpegReceived -= OnJpegReceived;
    }


    private Vector3 starterScale;

    internal void Show()
    {
        this.transform.localScale = starterScale;
    }

    internal void Hide()
    {
        this.transform.localScale = new Vector3(0, 0, 0);
    }
}