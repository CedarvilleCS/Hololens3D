using AssemblyCSharpWSA;
using Assets.UIScripts.ImageGallery;
using HoloToolkit.Unity;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VR.WSA.WebCam;
using System.IO;
using System.Collections.Generic;
using System;
#if WINDOWS_UWP
using System.Threading.Tasks;
#endif

public class PanoTakerController : MonoBehaviour
{
    // Use this for initialization
    PhotoCapture photoCaptureObject = null;
    Texture2D[] targetTextures;
    Texture2D[] gridTextures;
    Texture2D holderTexture;
    HLNetwork.ImagePosition[] targetImagePosition = new HLNetwork.ImagePosition[5];
    Resolution cameraResolution;
    bool[] checkboxes;
    bool sendPano;
    public GameObject[] markers;
    public ImageReceiver ipc;
    public TaskListReceiver tlp;
    public PDFReceiver pdfp;
    public Vector3 starterScale;
    public bool doneTakingPano;
    public StatusTextClearer statusText;
    public Text instructionText;
    public int markerIndex;
    public GameObject gridCan;
    public Vector3 gridCanStarterScale;
    private bool newImageTaken;
    private bool countDown;
    private float timer;
    private byte[][] screenshots = new byte[5][];
    private int pictureToTake;
    private bool takePicture;

    // Use this for initialization
    void Start()
    {
        //No picture take
        pictureToTake = 0;
        timer = 0;
        cameraResolution = PhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();
        targetTextures = new Texture2D[5];
        gridTextures = new Texture2D[5];
        checkboxes = new bool[5];
        for (int i = 0; i < targetTextures.Length; i++)
        {
            targetTextures[i] = new Texture2D(cameraResolution.width, cameraResolution.height);
            gridTextures[i] = new Texture2D(cameraResolution.width, cameraResolution.height);
            checkboxes[i] = false;
        }
        sendPano = false;
        doneTakingPano = false;

        starterScale = this.transform.localScale;

        ipc = GameObject.Find("ImagePaneCollection").GetComponent<ImageReceiver>();
        tlp = GameObject.Find("TaskListPane").GetComponent<TaskListReceiver>();
        pdfp = GameObject.Find("PDFPane").GetComponent<PDFReceiver>();

        statusText = GameObject.Find("StatusText").GetComponent<StatusTextClearer>();

        instructionText.text = "";

        holderTexture = new Texture2D(cameraResolution.width, cameraResolution.height);
        gridCanStarterScale = gridCan.transform.localScale;

        this.Hide();

        takePicture = false;
    }

    internal void TakePicture(int myIndex)
    {
        markerIndex = myIndex;
        pictureToTake = 1;
        takePicture = true;
    }

    private void Update()
    {
        if (takePicture)
        {
            takePicture = false;
            if (pictureToTake == 1)
            {
                TakeSinglePicture(markerIndex, false);
            }
            else if (pictureToTake == 2)
            {
                TakeSinglePicture(markerIndex, true);
            }
        }

        if (doneTakingPano)
        {
            //TODO: Do things to clear the screen and pop up a "please wait" message
            this.Hide();
            instructionText.text = "Sending panorama...";
            countDown = true;
            timer = 1.0f;
            doneTakingPano = false;
        }
        if (countDown)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                countDown = false;
                sendPano = true;
            }
        }

        if (newImageTaken)
        {
            newImageTaken = false;

            checkboxes[markerIndex] = true;
            doneTakingPano = DoneTakingPano();
        }
        if (sendPano)
        {
            sendPano = false;

            for (int i = 0; i < targetTextures.Length; i++)
            {
                PanoImage image = new PanoImage(targetTextures[i].EncodeToPNG(), targetImagePosition[i]);
                ipc.ReceivePanoJpeg(image, i, false); //without holograms

                image = new PanoImage(gridTextures[i].EncodeToPNG(), targetImagePosition[i]);
                ipc.ReceivePanoJpeg(image, i, true); //with holograms

                checkboxes[i] = false;
            }
            instructionText.text = "";
            statusText.status = StatusTextClearer.TextStatus.PanoTaken;

            ipc.Show();
            tlp.Show();
            pdfp.Show();
        }
    }
    public void TakePano()
    {
        for (int i = 0; i < targetTextures.Length; i++)
        {
            targetTextures[i] = new Texture2D(cameraResolution.width, cameraResolution.height);
            gridTextures[i] = new Texture2D(cameraResolution.width, cameraResolution.height);
            checkboxes[i] = false;
        }
        this.Show();
        instructionText.text = "Now gaze at each of the orbs to take a picture.";
    }

    public void TakeSinglePicture(int index, bool showHolograms)
    {
        float opacity = showHolograms ? 1.0f : 0.0f;

        PhotoCapture.CreateAsync(true, delegate (PhotoCapture captureObject)
        {
            photoCaptureObject = captureObject;
            CameraParameters cameraParameters = new CameraParameters();
            cameraParameters.hologramOpacity = opacity;
            cameraParameters.cameraResolutionWidth = cameraResolution.width;
            cameraParameters.cameraResolutionHeight = cameraResolution.height;
            cameraParameters.pixelFormat = CapturePixelFormat.BGRA32;

            // Activate the camera
            photoCaptureObject.StartPhotoModeAsync(cameraParameters, delegate (PhotoCapture.PhotoCaptureResult result)
            {
                // Take a picture
                markerIndex = index;
                targetImagePosition[markerIndex] = new HLNetwork.ImagePosition(Camera.main.transform);
                photoCaptureObject.TakePhotoAsync(OnCapturedPhotoToMemory);
            });
        });
    }

    void OnCapturedPhotoToMemory(PhotoCapture.PhotoCaptureResult result, PhotoCaptureFrame photoCaptureFrame)
    {
        //Copy the raw image data into the target texture
        if (pictureToTake == 1)
        {
            photoCaptureFrame.UploadImageDataToTexture(targetTextures[markerIndex]);
        }
        else
        {
            photoCaptureFrame.UploadImageDataToTexture(gridTextures[markerIndex]);
        }
        
        //Deactivate the camera
        photoCaptureObject.StopPhotoModeAsync(OnStoppedPhotoMode);
    }

    void OnStoppedPhotoMode(PhotoCapture.PhotoCaptureResult result)
    {
        // Shutdown the photo capture resource
        photoCaptureObject.Dispose();

        photoCaptureObject = null;

        pictureToTake = (pictureToTake + 1) % 3;
        newImageTaken = (pictureToTake == 0);
        if (newImageTaken)
        {
            markers[markerIndex].GetComponent<PanoMarkerController>().PictureDone();
        }
        takePicture = (pictureToTake == 2);
        checkboxes[markerIndex] = true;
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
    
    private bool DoneTakingPano()
    {
        foreach (bool b in checkboxes)
        {
            if (!b) return false;
        }
        return true;
    }
}
