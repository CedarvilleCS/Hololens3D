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
    public float timer;
    private bool countingDown;

    // Use this for initialization
    void Start()
    {
        myText = this.GetComponent<UnityEngine.UI.Text>();
        textColor = myText.color;
        cursorManager = GameObject.Find("Cursor").GetComponent<CursorManager>();
        timer = 0f;
        pictureTaken = false;
        panoTaken = false;
    }


    // Update is called once per frame
    void Update()
    {

        //The purpose of this is to clear the status text after a certain period of time.
        if (panoTaken)
        {
            pictureTaken = false;
            panoTaken = false;
            countingDown = true;
            timer = 2f;
        }
        else if (pictureTaken)
        {
            pictureTaken = false;
            countingDown = true;
            timer = 2f;
        }

        if (timer > 0f)
        {
            timer -= Time.deltaTime;
        }

        if (countingDown && timer < 0f)
        {
            myText.text = "";
            countingDown = false;
        }

    }
}
