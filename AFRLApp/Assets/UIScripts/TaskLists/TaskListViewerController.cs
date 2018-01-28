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
        Hide();

        starterScale = this.transform.parent.transform.localScale;

        tlgc = GameObject.Find("TaskListGallery").GetComponent<TaskListGalleryController>();
    }

    void Update()
    {
        taskListId = currTaskList.Id;
        currTaskList = tlgc.taskLists[taskListId];
    }

    internal void DisplayTaskList(int newID)
    {
        this.currTaskList = tlgc.taskLists[newID];
        //TODO: switch this to the dynamic count version
        Title.GetComponent<TaskListTitleController>().SetTitle(currTaskList.Name);
        int i = 0;
        foreach (GameObject taskThumbs in TaskThumbnails)
        {
            taskThumbs.GetComponent<TaskController>().SetTask(i);
            i++;
        }       
    }

    internal void Show()
    {
        this.transform.parent.transform.localScale = starterScale;
    }

    internal void Hide()
    {
        this.transform.parent.transform.localScale = new Vector3(0, 0, 0);
    }

    //Dont use this if you can help it
    public void RcvNewTaskList(TaskList taskList, int numRcvdTaskLists)
    {

    }

    internal void UpdateTasks()
    {
        tlgc.taskLists[taskListId] = currTaskList;
        int i = 0;
        foreach (GameObject task in TaskThumbnails)
        {
            task.GetComponent<TaskController>().SetTask(i + (TaskThumbnails.Length * increment));
        }
    }
}
