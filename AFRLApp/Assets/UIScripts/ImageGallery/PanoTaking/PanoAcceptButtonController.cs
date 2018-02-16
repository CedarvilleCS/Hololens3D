using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanoAcceptButtonController : MonoBehaviour
{ 
    
    
    //TODO: Add a more user friendly instruction set to the GUI to make use easier





    public PanoTakerController ptc;
    public bool isAccept;
    public Text countdownText;
    public Text instructionText;
    public Text statusText;
    public float timer;
    bool takePicture;
    public GameObject Popup;

    // Use this for initialization
    void Start()
    {
        ptc = GameObject.Find("PanoramaTaker").GetComponent<PanoTakerController>();
        timer = 0;
        takePicture = false;
        countdownText.text = "";
    }

    public void OnSelect()
    {
        if (isAccept)
        {
            statusText.text = "Gaze in the direction you want to panorama taken.";
            timer = 5.0f;
            takePicture = true;
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
