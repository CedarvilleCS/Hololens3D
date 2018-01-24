using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskListThumbnailController : MonoBehaviour
{
    public TaskListViewerController tlvc;
    public TaskList currTaskList;
    public Text ThumbText;

    // Use this for initialization
    void Start()
    {
        tlvc = GameObject.Find("TaskListViewer").GetComponent<TaskListViewerController>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    void OnSelect()
    {
        tlvc.DisplayTaskList(currTaskList);
    }

    internal void SetThumbnail(TaskList tl)
    {
        ThumbText.text = tl.Name;
        currTaskList = tl;
    }
}
