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
    public GameObject Title;
    private Vector3 starterScale;
    private TaskListGalleryController tlgc;
    public int taskListId;
    public GameObject Tasks;
    private bool _isPopout;

    // Use this for initialization
    void Awake()
    {
        TaskThumbnails = new GameObject[Tasks.transform.childCount];
        int i = 0;
        foreach (Transform task in Tasks.transform)
        {
            TaskThumbnails[i] = task.gameObject;
            i++;
        }

        increment = 0;
        currTaskList = null;

        starterScale = this.transform.localScale;

        tlgc = GameObject.Find("MasterObject/MainMenu/TaskListPane/TaskListGallery").GetComponent<TaskListGalleryController>();
        _isPopout = false;
        //Hide();
    }

    void Update()
    {
        if (currTaskList != null)
        {
            taskListId = currTaskList.Id;
            currTaskList = tlgc.taskLists.Find(x => x.Id == taskListId);
            UpdateTasks();
        }
    }

    public void DisplayTaskList(int id, int incr, bool is_popout)
    {
        if (is_popout)
        {
            this.Awake();
            foreach (Transform task in Tasks.transform)
            {
                TaskCheckController checker = task.GetComponentInChildren<TaskCheckController>();
                if (checker != null)
                {
                    checker.Start();
                }
            }
            _isPopout = is_popout;
        }
        DisplayTaskList(id, incr);
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
