using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusTextClearer : MonoBehaviour
{
    //This is to make the status text disappear after use.
    public Text myText;
    public Color textColor;
    public string currText;
    public float fadeSpeed;

    // Use this for initialization
    void Start()
    {
        myText = this.GetComponent<UnityEngine.UI.Text>();
        textColor = myText.color;
        currText = "";
    }

    // Update is called once per frame
    void Update()
    {
        //if there was a change
        if (myText.text != currText)
        {
            currText = myText.text;
            textColor.a = 1.0f;

        }
        if (textColor.a > 0f)
        {
            textColor.a -= fadeSpeed;
        }
    }
}
