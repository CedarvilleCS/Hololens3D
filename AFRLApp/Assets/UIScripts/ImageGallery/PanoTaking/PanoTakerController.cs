using AssemblyCSharpWSA;
using Assets.UIScripts.ImageGallery;
using HoloToolkit.Unity;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VR.WSA.WebCam;
#if WINDOWS_UWP
using System.Threading.Tasks;
#endif

public class PanoTakerController : MonoBehaviour
{
    // Use this for initialization
    PhotoCapture photoCaptureObject = null;
    Texture2D[] targetTextures;
    HLNetwork.ImagePosition[] targetImagePosition;
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
    public int markerIndex; //!
    public int previousMarkerIndex;
    public GameObject gridCan;
    public Vector3 gridCanStarterScale;
    private bool newImageTaken;
    private bool countDown;
    private float timer;

    // Use this for initialization
    void Start()
    {
        timer = 0;
        cameraResolution = PhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();
        targetTextures = new Texture2D[5];
        targetImagePosition = new HLNetwork.ImagePosition[5];
        checkboxes = new bool[5];
        for (int i = 0; i < targetTextures.Length; i++)
        {
            targetTextures[i] = new Texture2D(cameraResolution.width, cameraResolution.height);
            targetImagePosition[i] = null;
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
        statusText.myText.text = "";

        gridCanStarterScale = gridCan.transform.localScale;

        this.Hide();
    }

    private void Update()
    {
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


        if (sendPano)
        {
            sendPano = false;

            //TODO: Get and send the grid PNG
            //Note: May not need the async process; only did this because I could copy the code from somewhere else;
            //If there is a way to replace task.Wait() with something to know when the task ends so we can let the system run
            //normally, that would be nice
            //CAUTION! this is being called in Update(), so all kinds of tomfoolery is possible if you don't lock this out
#if WINDOWS_UWP
                    Task task = new Task(
                        async() => {
                            for (int i = 0; i < targetTextures.Length; i++)
                            {
                                PanoImage image = new PanoImage(targetTextures[i].EncodeToPNG(), targetImagePosition[i]);
                                ipc.ReceivePanoJpeg(image, i);

                                checkboxes[i] = false;
                            }
                        }
                    );
                    task.Start();
                    task.Wait();
#endif

            statusText.panoTaken = true;

            ipc.Show();
            tlp.Show();
            pdfp.Show();

            //    GetComponent<Billboard>().enabled = true;
            //    GetComponent<SimpleTagalong>().enabled = true;
        }

        if (newImageTaken)
        {
            newImageTaken = false;
            checkboxes[markerIndex] = true;
            doneTakingPano = DoneTakingPano();
            statusText.pictureTaken = true;
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
        PhotoCapture.CreateAsync(false, delegate (PhotoCapture captureObject)
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
                markerIndex = index; //!
                targetImagePosition[markerIndex] = new HLNetwork.ImagePosition(Camera.main.transform);
                photoCaptureObject.TakePhotoAsync(OnCapturedPhotoToMemory);
            });
        });
    }

    void OnCapturedPhotoToMemory(PhotoCapture.PhotoCaptureResult result, PhotoCaptureFrame photoCaptureFrame)
    {
        // Copy the raw image data into the target texture
        photoCaptureFrame.UploadImageDataToTexture(targetTextures[markerIndex]);
        //Get the grid capture
        ScreenCapture.CaptureScreenshot("Screenshot" + markerIndex.ToString() + ".png");
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

    private bool DoneTakingPano()
    {
        foreach (bool b in checkboxes)
        {
            if (!b) return false;
        }
        return true;
    }

    //private void PrepareAndSentPanoImages()
    //{
    //    for (int i = 0; i < targetTextures.Length; i++)
    //    {
    //        PanoImage image = new PanoImage(targetTextures[i].EncodeToPNG(), targetImagePosition[i]);
    //        ipc.ReceivePanoJpeg(image, i);

    //        checkboxes[i] = false;
    //    }
    //}
}
