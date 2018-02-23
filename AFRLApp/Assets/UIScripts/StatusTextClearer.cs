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
    public bool pictureTaken;
    public bool panoTaken;
    public float timeToWait;
    private float timer;
    private bool countingDown;
    public string textToShow;
    // Use this for initialization
    void Start()
    {
        myText = this.GetComponent<UnityEngine.UI.Text>();
        textColor = myText.color;
        timer = 0f;
        pictureTaken = false;
        panoTaken = false;
        textToShow = "";
    }


    // Update is called once per frame
    void Update()
    {

        //The purpose of this is to clear the status text after a certain period of time.
        if (panoTaken)
        {
            textToShow = "Panorama sent.";
            pictureTaken = false;
            panoTaken = false;
            countingDown = true;
            timer = timeToWait;
        }
        else if (pictureTaken)
        {
            textToShow = "Picture taken.";
            pictureTaken = false;
            countingDown = true;
            timer = timeToWait;
        }

        if (timer > 0f)
        {
            timer -= Time.deltaTime;
            myText.text = textToShow;
        }

        if (countingDown && timer < 0f)
        {
            myText.text = "";
            countingDown = false;
        }

    }
}
