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
    Resolution cameraResolution;
    ImageReceiver imageReceiver;
    public float timer;
    bool takePicture;
    public Text countdownText;

    // Use this for initialization
    void Start()
    {
        
        imageReceiver = GameObject.Find("ImagePaneCollection").GetComponent<ImageReceiver>();
        timer = 0;
        countdownText.text = "";
    }


    void OnSelect()
    {
        timer = 6.0f;
        takePicture = true;
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
                GameObject.Find("PanoramaTaker").GetComponent<PanoTakerController>().TakePano();
            }
        }
    }
}
