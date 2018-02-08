using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.VR.WSA.WebCam;
using HoloToolkit.Unity;
using UnityEngine.UI;
using Assets.UIScripts.ImageGallery;

public class ManualPictureTaking : MonoBehaviour
{
    PhotoCapture photoCaptureObject = null;
    Texture2D targetTexture = null;
    HLNetwork.ImagePosition targetImagePosition = null;
    Resolution cameraResolution;
    ImageReceiver imageReceiver;
    public float timer;
    bool takePicture;
    public Text countdownText;

    // Use this for initialization
    void Start()
    {
        cameraResolution = PhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();
        targetTexture = new Texture2D(cameraResolution.width, cameraResolution.height);
        
        imageReceiver = GameObject.Find("ImagePaneCollection").GetComponent<ImageReceiver>();
        timer = 0;
        countdownText.text = "";
    }


    void OnSelect()
    {
        timer = 6.0f;
        takePicture = true;
    }

    void OnCapturedPhotoToMemory(PhotoCapture.PhotoCaptureResult result, PhotoCaptureFrame photoCaptureFrame)
    {
        // Copy the raw image data into the target texture
        photoCaptureFrame.UploadImageDataToTexture(targetTexture);

        // Deactivate the camera
        photoCaptureObject.StopPhotoModeAsync(OnStoppedPhotoMode);
    }

    void OnStoppedPhotoMode(PhotoCapture.PhotoCaptureResult result)
    {
        // Shutdown the photo capture resource
        photoCaptureObject.Dispose();
        photoCaptureObject = null;
    }

    private void Update()
    {
        if (takePicture)
        {
            timer -= Time.deltaTime;
            countdownText.text = ((int) timer + 1).ToString();
            if (timer < 0f)
            {
                takePicture = false;
                PhotoCapture.CreateAsync(false, delegate (PhotoCapture captureObject)
                {
                    photoCaptureObject = captureObject;
                    CameraParameters cameraParameters = new CameraParameters();
                    cameraParameters.hologramOpacity = 0.0f; 
                    cameraParameters.cameraResolutionWidth = cameraResolution.width;
                    cameraParameters.cameraResolutionHeight = cameraResolution.height;
                    cameraParameters.pixelFormat = CapturePixelFormat.BGRA32;

                    // Activate the camera
                    photoCaptureObject.StartPhotoModeAsync(cameraParameters, delegate (PhotoCapture.PhotoCaptureResult result)
                        {
                            // Take a picture
                            photoCaptureObject.TakePhotoAsync(OnCapturedPhotoToMemory);
                            targetImagePosition = new HLNetwork.ImagePosition(Camera.main.transform);
                        });
                });

                PanoImage image = new PanoImage(targetTexture.GetRawTextureData(), targetImagePosition);
                imageReceiver.ReceivePanoJpeg(image);
                this.GetComponent<Renderer>().material.mainTexture = targetTexture;
            }
        }
    }
}
