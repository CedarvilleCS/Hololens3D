using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.VR.WSA.WebCam;
using HoloToolkit.Unity;
using UnityEngine.UI;
using Assets.UIScripts.ImageGallery;
using AssemblyCSharpWSA;

public class ManualPictureTaking : MonoBehaviour
{

    //TODO: Add a more user friendly instruction set to the GUI to make use easier






    Resolution cameraResolution;
    ImageReceiver imageReceiver;
    public float timer;
    bool takePicture;
    public Text countdownText;
    internal ImageReceiver ipc;
    internal TaskListReceiver tlp;
    internal PDFReceiver pdfp;
    private Vector3 starterScale;

    // Use this for initialization
    void Start()
    {
        imageReceiver = GameObject.Find("ImagePaneCollection").GetComponent<ImageReceiver>();
        timer = 0;
        countdownText.text = "";
        starterScale = transform.localScale;
        ipc = GameObject.Find("ImagePaneCollection").GetComponent<ImageReceiver>();
        tlp = GameObject.Find("TaskListPane").GetComponent<TaskListReceiver>();
        pdfp = GameObject.Find("PDFPane").GetComponent<PDFReceiver>();
    }


    void OnSelect()
    {
        timer = 6.0f;
        takePicture = true;

        //TODO: Update instruction text
        Hide();
        ipc.Hide();
        tlp.Hide();
        pdfp.Hide();
    }

    private void Update()
    {
        if (takePicture)
        {
            timer -= Time.deltaTime;
            countdownText.text = ((int) timer + 1).ToString();
            if (timer < 0f)
            {
                countdownText.text = "";
                takePicture = false;
                GameObject.Find("PanoramaTaker").GetComponent<PanoTakerController>().TakePano();
            }
        }
    }

    public void SetCountdownText(int num)
    {
        countdownText.text = num.ToString();
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
