﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskCheckController : MonoBehaviour
{
    public bool boxChecked;
    public Material CheckedMat;
    public Material UncheckedMat;
    public TaskListViewerController tlvc;
    public TaskController parentTask;
    private Vector3 starterScale;
    // Use this for initialization
    public void Start()
    {
        boxChecked = false;
        CheckedMat = Resources.Load("Materials/CheckedBox") as Material;
        UncheckedMat = Resources.Load("Materials/UncheckedBox") as Material;
        if (tlvc == null)
        {
            tlvc = this.gameObject.transform.root.Find("TaskListViewerPopout").GetComponent<TaskListViewerController>();
        }
        parentTask = this.transform.parent.GetComponent<TaskController>();
        starterScale = new Vector3(0.1f, 0.8f, 0.1f);
    }

    // Update is called once per frame
    void OnSelect()
    {
        if (parentTask.TaskNum > -1)
        {
            boxChecked = !boxChecked;
            parentTask.Checked(boxChecked);
            if (boxChecked)
            {
                this.GetComponent<Renderer>().material = CheckedMat;
            }
            else
            {
                this.GetComponent<Renderer>().material = UncheckedMat;
            }
        }
    }

    internal void SetBoxChecked(bool isBoxChecked)
    {
        Show();
        if (isBoxChecked)
        {
            this.GetComponent<Renderer>().material = CheckedMat;
        }
        else //box not checked
        {
            this.GetComponent<Renderer>().material = UncheckedMat;
        }

        this.boxChecked = isBoxChecked;
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
