using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskListReturnButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Hide();
	}

    internal void Show()
    {
        //TODO
    }

    internal void Hide()
    {
        //TODO
    }

    void OnSelect()
    {
        Hide();
        GameObject.Find("TaskListViewer").GetComponent<TaskListViewerController>().Show();
    }
}
