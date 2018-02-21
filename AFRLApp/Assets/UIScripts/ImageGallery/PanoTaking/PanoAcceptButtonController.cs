using AssemblyCSharpWSA;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanoAcceptButtonController : MonoBehaviour
{ 
    public PanoTakerController ptc;
    public bool isAccept;
    public Text countdownText;
    public Text instructionText;
    public Text statusText;
    public float timer;
    bool takePicture;
    public GameObject Popup;
    public ImageReceiver ipc;
    public TaskListReceiver tlp;
    public PDFReceiver pdfp;

    // Use this for initialization
    void Start()
    {
        ptc = GameObject.Find("PanoramaTaker").GetComponent<PanoTakerController>();
        timer = 0;
        takePicture = false;
        countdownText.text = "";


        ipc = GameObject.Find("ImagePaneCollection").GetComponent<ImageReceiver>();
        tlp = GameObject.Find("TaskListPane").GetComponent<TaskListReceiver>();
        pdfp = GameObject.Find("PDFPane").GetComponent<PDFReceiver>();
    }

    public void OnSelect()
    {
        if (isAccept)
        {
            instructionText.text = "Gaze in the direction you want to panorama taken.";
            timer = 5.0f;
            takePicture = true;

            ipc.Hide();
            tlp.Hide();
            pdfp.Hide();
        }
        this.transform.parent.gameObject.GetComponent<PanoPopupController>().Hide();
    }

    private void Update()
    {
        if (takePicture)
        {
            timer -= Time.deltaTime;
            countdownText.text = ((int)timer + 1).ToString();
            if (timer < 0f)
            {
                statusText.text = "";
                countdownText.text = "";
                takePicture = false;
                ptc.TakePano();
            }
        }
    }

}
