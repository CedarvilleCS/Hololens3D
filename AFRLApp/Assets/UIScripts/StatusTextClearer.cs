using HoloToolkit.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusTextClearer : MonoBehaviour
{
    //This is to make the status text disappear after use.
    public Text myText;
    public Color textColor;
    public float fadeSpeed;
    public CursorManager cursorManager;
    public float timeToWait;
    private float timer;
    private bool countingDown;
    public enum TextStatus { HoldStill, PanoTaken, PictureTaken, noText};
    public TextStatus status;
    // Use this for initialization
    void Start()
    {
        status = TextStatus.noText;
        myText = this.GetComponent<UnityEngine.UI.Text>();
        textColor = myText.color;
        timer = 0f;
        countingDown = false;
    }


    // Update is called once per frame
    void Update()
    {
        //The purpose of this is to clear the status text after a certain period of time.
        if (status == TextStatus.PanoTaken && !countingDown)
        {
            myText.text = "Panorama sent.";
            if (!countingDown)
            {
                timer = timeToWait;
            }
            countingDown = true;
        }
        else if (status == TextStatus.PictureTaken && !countingDown)
        {
            myText.text = "Picture taken.";
            if (!countingDown)
            {
                timer = timeToWait;
            }
            countingDown = true;
        }
        else if (status == TextStatus.HoldStill)
        {
            myText.text = "HOLD STILL!";
            timer = -0.1f;
            countingDown = false;
            status = TextStatus.noText;
        }
        else
        {
            myText.text = "";
        }

        if (timer > 0f)
        {
            timer -= Time.deltaTime;
        }

        if (countingDown && timer < 0f)
        {
            status = TextStatus.noText;
            myText.text = "";
            countingDown = false;
        }
    }
}
