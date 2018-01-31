using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskListGalleryController : MonoBehaviour
{

    public GameObject[] taskListThumbnails;
    public List<TaskList> taskLists;
    private bool _newListRecieved;
    public int pageIncrement;
    private Vector3 starterScale;

    // Use this for initialization
    void Start()
    {
        taskLists = new List<TaskList>();
        _newListRecieved = false;
        pageIncrement = 0;

        starterScale = this.transform.parent.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (_newListRecieved)
        {
            _newListRecieved = false;
            //if there is space on the currently shown page
            if ((pageIncrement + 1) * taskListThumbnails.Length >= taskLists.Count)
            {
                int x = (taskLists.Count - 1) % taskListThumbnails.Length;
                taskListThumbnails[x].GetComponent<TaskListThumbnailController>().SetThumbnail(taskLists[taskLists.Count - 1].Id - 1);
            }
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

    public void RcvNewTaskList(TaskList taskList, int numRcvdTaskLists)
    {
        taskLists.Add(taskList);
        _newListRecieved = true;
    }

    
}
