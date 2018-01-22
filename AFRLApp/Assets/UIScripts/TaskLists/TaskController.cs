using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskController : MonoBehaviour {

    public GameObject checkButton;
    public GameObject showImageButton;
    public Text TaskText;
    private bool hideChecked;
    private bool taskVisible;
    private int taskNum;

    // Use this for initialization
    void Start () {
        hideChecked = false;
	}

    public void SetTask(int t)
    {

    }
    

    internal void Checked(bool boxChecked)
    {
        if (boxChecked == true && hideChecked)
        {
            this.SetTask(taskNum+1);
        } else
        {

        }
    }
}
