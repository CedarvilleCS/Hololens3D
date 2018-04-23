using Assets.UIScripts.ImageGallery;
using HoloToolkit.Unity;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VR.WSA.WebCam;

public class PanoTakerController : MonoBehaviour
{
    // Use this for initialization
    PhotoCapture photoCaptureObject = null;
    Texture2D targetTexture = null;
    HLNetwork.ImagePosition targetImagePosition = null;
    Resolution cameraResolution;
    public GameObject[] markers;
    public ImageReceiver ipc;
    public MasterHider masterObject;
    public Vector3 starterScale;
    public bool doneWithPano;
    public StatusTextClearer statusText;
    public Text instructionText;
    public int markerIndex; //!
    public int previousMarkerIndex;
    public GameObject gridCan;
    public Vector3 gridCanStarterScale;
    private bool newImageTaken;

    // Use this for initialization
    void Start()
    {
        cameraResolution = PhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();
        targetTexture = new Texture2D(cameraResolution.width, cameraResolution.height);
        doneWithPano = false;
        starterScale = this.transform.localScale;

        ipc = GameObject.Find("ImagePaneCollection").GetComponent<ImageReceiver>();

        statusText = GameObject.Find("StatusText").GetComponent<StatusTextClearer>();

        instructionText.text = "";
        statusText.myText.text = "";

        gridCanStarterScale = gridCan.transform.localScale;

        this.Hide();
    }

    private void Update()
    {
        if (newImageTaken)
        {
            newImageTaken = false;
            PanoImage image = new PanoImage(targetTexture.EncodeToPNG(), targetImagePosition);
            doneWithPano = ipc.ReceivePanoJpeg(image, previousMarkerIndex);
        }
        if (doneWithPano)
        {
            doneWithPano = false;

            statusText.panoTaken = true;

            instructionText.text = "";

            this.Hide();
            masterObject.Show();

            //    GetComponent<Billboard>().enabled = true;
            //    GetComponent<SimpleTagalong>().enabled = true;
        }
    }

    public void TakePano()
    {
        //    GetComponent<Billboard>().enabled = false;
        //    GetComponent<SimpleTagalong>().enabled = false;

        this.Show();
        instructionText.text = "Now gaze at each of the orbs to take a picture.";
    }

    public void TakeSinglePicture(int index)
    {
        statusText.myText.text = "Taking Picture (System will freeze)";
        PhotoCapture.CreateAsync(/*true*/ false, delegate (PhotoCapture captureObject)
        {
            photoCaptureObject = captureObject;
            CameraParameters cameraParameters = new CameraParameters();
            cameraParameters.hologramOpacity = 0.0f;//1.0f;
            cameraParameters.cameraResolutionWidth = cameraResolution.width;/// 2;
            cameraParameters.cameraResolutionHeight = cameraResolution.height;/// 2;
            cameraParameters.pixelFormat = CapturePixelFormat.BGRA32;

            // Activate the camera
            photoCaptureObject.StartPhotoModeAsync(cameraParameters, delegate (PhotoCapture.PhotoCaptureResult result)
            {
                // Take a picture
                markerIndex = index;
                targetImagePosition = new HLNetwork.ImagePosition(Camera.main.transform);
                photoCaptureObject.TakePhotoAsync(OnCapturedPhotoToMemory);
            });
        });
    }

    void OnCapturedPhotoToMemory(PhotoCapture.PhotoCaptureResult result, PhotoCaptureFrame photoCaptureFrame)
    {
        // Copy the raw image data into the target texture
        photoCaptureFrame.UploadImageDataToTexture(targetTexture);
        // Deactivate the camera
        photoCaptureObject.StopPhotoModeAsync(OnStoppedPhotoMode);

        previousMarkerIndex = markerIndex;
        newImageTaken = true;
    }

    void OnStoppedPhotoMode(PhotoCapture.PhotoCaptureResult result)
    {
        // Shutdown the photo capture resource
        photoCaptureObject.Dispose();
        photoCaptureObject = null;

        markers[markerIndex].GetComponent<PanoMarkerController>().PictureDone();   //!
    }

    internal void Show()
    {
        gridCan.transform.localScale = gridCanStarterScale;
        foreach (GameObject marker in markers)
        {
            marker.GetComponent<PanoMarkerController>().Show();
        }
        this.GetComponent<SimpleTagalong>().enabled = false;
        this.GetComponent<Billboard>().enabled = false;
    }

    internal void Hide()
    {
        gridCan.transform.localScale = new Vector3(0, 0, 0);
        foreach (GameObject marker in markers)
        {
            marker.GetComponent<PanoMarkerController>().Hide();
        }
        this.GetComponent<SimpleTagalong>().enabled = true;
        this.GetComponent<Billboard>().enabled = true;
    }

    private Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight)
    {
        Texture2D result = new Texture2D(targetWidth, targetHeight, source.format, false);
        float incX = (1.0f / (float)targetWidth);
        float incY = (1.0f / (float)targetHeight);
        for (int i = 0; i < result.height; ++i)
        {
            for (int j = 0; j < result.width; ++j)
            {
                Color newColor = source.GetPixelBilinear((float)j / (float)result.width, (float)i / (float)result.height);
                result.SetPixel(j, i, newColor);
            }
        }
        result.Apply();
        return result;
    }
}