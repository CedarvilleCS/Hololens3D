using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskListThumbnailController : MonoBehaviour
{
    public TaskListViewerController tlvc;
    public int ID;
    public Text ThumbText;

    // Use this for initialization
    void Start()
    {
        tlvc = GameObject.Find("TaskListViewer").GetComponent<TaskListViewerController>();
    }

    void OnSelect()
    {
        tlvc.DisplayTaskList(ID);
    }

    internal void SetThumbnail(int id)
    {
        ThumbText.text = this.transform.parent.GetComponent<TaskListGalleryController>().taskLists[id].Name;
        ID = id;
    }
}
