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
    private float timer;
    bool takePicture;
    public GameObject Popup;
    public MasterHider masterObject;
    public float timeToCountDown;

    // Use this for initialization
    void Start()
    {
        timer = 0;
        takePicture = false;
        countdownText.text = "";
        instructionText.text = "";
        statusText.text = "";
    }

    public void OnSelect()
    {
        if (isAccept)
        {
            instructionText.text = "Gaze in the direction you want the panorama taken.";
            timer = timeToCountDown;
            takePicture = true;

            masterObject.Hide();
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
                countdownText.text = "";
                takePicture = false;
                ptc.TakePano();
            }
        }
    }

}
