using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanoMarkerController : MonoBehaviour
{
    public int myIndex;
    public PanoTakerController TakerController;
    public Vector3 starterScale;
    public bool focused;
    public int counter;
    public Text countdownText;
    public StatusTextClearer statusText;
    public Text instructionText;
    private bool takingPicture;

    // Use this for initialization
    void Awake()
    {
        starterScale = this.transform.localScale;
        focused = false;
        counter = 0;
        statusText = GameObject.Find("StatusText").GetComponent<StatusTextClearer>();
        takingPicture = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!takingPicture)
        {
            //Once we've been on it for a certain period of time, remove it.
            if (counter > 10)
            {
                takingPicture = true;
                counter = 0;
                this.TakePicture();
            }
            if (focused)
            {
                counter++;
            }
            else
            {
                counter = 0;
            }
        }
        focused = false;

    }

    internal void TakePicture()
    {
        TakerController.TakePicture(myIndex);
    }

    internal void PictureDone()
    {
        counter = 0;
        statusText.status = StatusTextClearer.TextStatus.PictureTaken;
        this.Hide();
        takingPicture = false;
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
