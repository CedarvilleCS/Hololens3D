using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanoMarkerController : MonoBehaviour
{
    public int myIndex;
    public GameObject TakerController;
    public Vector3 starterScale;
    public bool focused;
    public int counter;
    public Text countdownText;
    public StatusTextClearer statusText;
    public Text instructionText;

    // Use this for initialization
    void Awake()
    {
        starterScale = this.transform.localScale;
        focused = false;
        counter = 0;
        statusText = GameObject.Find("StatusText").GetComponent<StatusTextClearer>();
    }

    // Update is called once per frame
    void Update()
    {
        //Remove "Hold still" if not focused
        //Have to do it this way or the other makers will overwrite this
        if (counter > 0 && !focused)
        {
            statusText.myText.text = "";
        }

        //Alert the user if focused
        if (focused)
        {
            counter++;
            statusText.myText.text = "HOLD STILL!";
        }
        //Stop counting up to make disappear if not focused
        else
        {
            counter = 0;
        }

        //Once we've been on it for a certain period of time, remove it.
        if (counter > 10)
        {
            counter = 0;

            TakerController.GetComponent<PanoTakerController>().TakeSinglePicture(myIndex);
        }
        focused = false;
    }
    internal void PictureDone()
    {
        statusText.pictureTaken = true;
        statusText.myText.text = "Picture Taken.";
        this.Hide();
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
