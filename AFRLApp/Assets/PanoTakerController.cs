using AssemblyCSharpWSA;
using Assets.UIScripts.ImageGallery;
using HoloToolkit.Unity;
using System.Collections;
using System.Collections.Generic;
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
    public TaskListReceiver tlp;
    public PDFReceiver pdfp;
    public Vector3 starterScale;
    public bool doneWithPano;


    // Use this for initialization
    void Start()
    {
        doneWithPano = false;
        starterScale = this.transform.localScale;
        cameraResolution = PhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();
        targetTexture = new Texture2D(cameraResolution.width, cameraResolution.height);


        ipc = GameObject.Find("ImagePaneCollection").GetComponent<ImageReceiver>();
        tlp = GameObject.Find("TaskListPane").GetComponent<TaskListReceiver>();
        pdfp = GameObject.Find("PDFPane").GetComponent<PDFReceiver>();

        this.Hide();
    }

    private void Update()
    {
        if (doneWithPano)
        {
            doneWithPano = false;
            this.Hide();
            ipc.Show();
            tlp.Show();
            pdfp.Show();
            GetComponent<Billboard>().enabled = true;
            GetComponent<SimpleTagalong>().enabled = true;

            foreach (GameObject marker in markers)
            {
                marker.GetComponent<PanoMarkerController>().Show();
            }
        }
    }

    public void TakePano()
    {

        GetComponent<Billboard>().enabled = false;
        GetComponent<SimpleTagalong>().enabled = false;
        ipc.Hide();
        tlp.Hide();
        pdfp.Hide();

        this.Show();

    }

    public void TakeSinglePicture(int markerIndex)
    {
        PhotoCapture.CreateAsync(false, delegate (PhotoCapture captureObject)
        {
            photoCaptureObject = captureObject;
            CameraParameters cameraParameters = new CameraParameters();
            cameraParameters.hologramOpacity = 0.0f;
            cameraParameters.cameraResolutionWidth = cameraResolution.width;/// 2;
            cameraParameters.cameraResolutionHeight = cameraResolution.height;/// 2;
            cameraParameters.pixelFormat = CapturePixelFormat.BGRA32;

            // Activate the camera
            photoCaptureObject.StartPhotoModeAsync(cameraParameters, delegate (PhotoCapture.PhotoCaptureResult result)
            {
                // Take a picture
                targetImagePosition = new HLNetwork.ImagePosition(Camera.main.transform);
                photoCaptureObject.TakePhotoAsync(OnCapturedPhotoToMemory);
                PanoImage image = new PanoImage(targetTexture.GetRawTextureData(), targetImagePosition);
                doneWithPano = ipc.ReceivePanoJpeg(image, markerIndex);
                //markers[markerIndex].GetComponent<PanoMarkerController>().Hide();
            });
        });
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
    internal void Show()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            this.transform.GetChild(i).GetComponent<PanoMarkerController>().Show();
        }
    }

    internal void Hide()
    {
        for(int i = 0; i < this.transform.childCount; i++)
        {
            this.transform.GetChild(i).GetComponent<PanoMarkerController>().Hide();
        }
    }
}
