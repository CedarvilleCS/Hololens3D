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
            if (counter > 15)
            {
                takingPicture = true;
                counter = 0;
                this.TakePicture();
            }
            //Alert the user if focused
            else if (focused)
            {
                counter++;
                statusText.myText.text = "HOLD STILL!";
            }
            //remove alert if not focused
            else if (counter > 0 && !focused)
            {
                statusText.myText.text = "";
                counter = 0;
            }
        }

        focused = false;
    }

    internal void TakePicture()
    {
        TakerController.TakeSinglePicture(myIndex);
    }
    internal void PictureDone()
    {
        counter = 0;
        statusText.pictureTaken = true;
        this.Hide();
        statusText.myText.text = "Picture Taken.";
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
