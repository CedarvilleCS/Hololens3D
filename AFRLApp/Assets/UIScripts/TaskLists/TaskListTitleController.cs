using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskListTitleController : MonoBehaviour {

    public Text titleText;

    internal void SetTitle(string name)
    {
        titleText.text = name;
    }
}
