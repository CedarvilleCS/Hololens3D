using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskListReturnButton : MonoBehaviour {

    private Vector3 starterScale;
	// Use this for initialization
	void Start () {
        starterScale = this.transform.parent.transform.localScale;
        Hide();
	}

    internal void Show()
    {
        this.transform.parent.transform.localScale = starterScale;
    }

    internal void Hide()
    {
        this.transform.parent.transform.localScale = new Vector3(0, 0, 0);
    }

    void OnSelect()
    {
        Hide();
        GameObject.Find("TaskListViewer").GetComponent<TaskListViewerController>().Show();
    }
}
