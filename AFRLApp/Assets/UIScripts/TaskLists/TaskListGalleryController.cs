using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskListGalleryController : MonoBehaviour
{

    public GameObject[] taskListThumbnails;
    public List<TaskList> taskLists;
    private bool _newListRecieved;
    private TaskList _newTaskList;
    public int pageIncrement;
    private Vector3 starterScale;

    // Use this for initialization
    void Awake()
    {
        taskLists = new List<TaskList>();
        _newListRecieved = false;
        pageIncrement = 0;

        starterScale = this.transform.localScale;
    }
    // Update is called once per frame
    void Update()
    {
        if (_newListRecieved)
        {
            _newListRecieved = false;
            UpdateThumbnails();
        }
    }

    public void UpdateThumbnails()
    {
        int i = 0;
        foreach (GameObject t in taskListThumbnails)
        {
            TaskListThumbnailController thumbnail = t.GetComponent<TaskListThumbnailController>();

            if (taskLists.Count > (i + (pageIncrement * taskListThumbnails.Length)))
            {
                TaskList tasklist = taskLists[i + pageIncrement * taskListThumbnails.Length];
                thumbnail.ThumbText.text = tasklist.Name;
                thumbnail.ID = tasklist.Id;
            }
            else
            {
                thumbnail.ThumbText.text = "";
                thumbnail.ID = -1;
            }
            i++;
        }
    }

    internal void Show()
    {
        this.transform.localScale = starterScale;
    }

    internal void Hide()
    {
        this.transform.localScale = new Vector3(0, 0, 0);
    }

    public void RcvNewTaskList(List<TaskList> taskListss, int numRcvdTaskLists)
    {
        taskLists = taskListss;
        _newListRecieved = true;
    }
}
