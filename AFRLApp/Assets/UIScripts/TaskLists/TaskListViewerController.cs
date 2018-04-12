using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine.Experimental.UIElements;

public class TaskListViewerController : MonoBehaviour
{
    public GameObject[] TaskThumbnails;
    public TaskList currTaskList;
    public int increment;
    internal GameObject Title;
    private Vector3 starterScale;
    internal TaskListGalleryController tlgc;
    public int taskListId;


    // Use this for initialization
    void Awake()
    {
        GameObject TasksHolder = GameObject.Find("Tasks");
        TaskThumbnails = new GameObject[TasksHolder.transform.childCount];
        int i = 0;
        foreach (Transform task in TasksHolder.transform)
        {
            TaskThumbnails[i] = task.gameObject;
            i++;
        }

        Title = GameObject.Find("TaskListTitle");
        increment = 0;
        currTaskList = null;

        starterScale = this.transform.localScale;

        tlgc = GameObject.Find("TaskListGallery").GetComponent<TaskListGalleryController>();

        Hide();
    }

    void Update()
    {
        if (currTaskList != null)
        {
            taskListId = currTaskList.Id;
            currTaskList = tlgc.taskLists.Find(x => x.Id == taskListId);
        }
    }

    internal void DisplayTaskList(int newID, int incr)
    {
        if (newID > -1)
        {
            increment = incr;
            this.currTaskList = tlgc.taskLists.Find(x => x.Id == newID);
            Title.GetComponent<TaskListTitleController>().SetTitle(currTaskList.GetTitleWithNumCompleted());

            int i = 0;
            foreach (GameObject taskThumbs in TaskThumbnails)
            {
                taskThumbs.GetComponent<TaskController>().SetTask(i + (TaskThumbnails.Length * incr));
                i++;
            }

            //bool initIncomplete = false;
            //if (currTaskList == null)
            //{
            //    initIncomplete = true;
            //}
            //else if (currTaskList.Id == newID)
            //{
            //    initIncomplete = true;
            //}
            ////else we have a totally new task list and they are all incomplete.
            //if (initIncomplete)
            //{
            //    Show();
            //}
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

    internal void UpdateTasks()
    {
        tlgc.taskLists[taskListId] = currTaskList;
        int i = 0;
        foreach (GameObject task in TaskThumbnails)
        {
            task.GetComponent<TaskController>().SetTask(i + (TaskThumbnails.Length * increment));
            i++;
        }
    }

    internal void RcvNewTaskList()
    {
        if (currTaskList != null)
        {
            DisplayTaskList(currTaskList.Id, increment);
        }
    }
}
