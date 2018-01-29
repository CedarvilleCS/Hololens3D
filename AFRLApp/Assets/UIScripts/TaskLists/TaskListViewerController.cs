﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine.Experimental.UIElements;

public class TaskListViewerController : MonoBehaviour
{
    public GameObject[] TaskThumbnails;
    public TaskList currTaskList;
    public TaskList incompleteTasks;
    public int currTLid;
    public int increment;
    internal GameObject Title;
    private Vector3 starterScale;
    internal TaskListGalleryController tlgc;
    public int taskListId;


    // Use this for initialization
    void Start()
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

        starterScale = this.transform.parent.transform.localScale;

        tlgc = GameObject.Find("TaskListGallery").GetComponent<TaskListGalleryController>();
        incompleteTasks = new TaskList();

        Hide();
    }

    void Update()
    {
        if (currTaskList != null)
        {
            taskListId = currTaskList.Id;
            currTaskList = tlgc.taskLists[taskListId - 1];
        }
    }

    internal void DisplayTaskList(int newID, int pageIncrement)
    {
        if (newID > -1)
        {
            bool initIncomplete = false;
            if (currTaskList == null)
            {
                initIncomplete = true;
            }
            else if (currTaskList.Id == newID)
            {
                initIncomplete = true;
            }
            this.currTaskList = tlgc.taskLists[newID];
            //TODO: switch this to the dynamic count version
            Title.GetComponent<TaskListTitleController>().SetTitle(currTaskList.Name);
            int i = 0;
            foreach (GameObject taskThumbs in TaskThumbnails)
            {
                taskThumbs.GetComponent<TaskController>().SetTask(i);
                i++;
            }
            if (initIncomplete)
            {
                foreach (TaskItem task in currTaskList.Tasks)
                {
                    if (!task.IsCompleted)
                    {
                        incompleteTasks.Tasks.Add(task);
                    }
                }
                Show();
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

    internal void UpdateTasks()
    {
        tlgc.taskLists[taskListId - 1] = currTaskList;
        int i = 0;
        foreach (GameObject task in TaskThumbnails)
        {
            task.GetComponent<TaskController>().SetTask(i + (TaskThumbnails.Length * increment));
            i++;
        }
    }
}