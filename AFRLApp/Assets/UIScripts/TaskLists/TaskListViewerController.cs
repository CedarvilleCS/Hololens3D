using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine.Experimental.UIElements;

public class TaskListViewerController : MonoBehaviour {
    public GameObject[] Tasks;
    public TaskList currTaskList;
    public int increment;

	// Use this for initialization
	void Start () {
        GameObject TasksHolder = GameObject.Find("Tasks");
        Tasks = new GameObject[TasksHolder.transform.childCount];
        int i = 0;
        foreach(Transform task in TasksHolder.transform)
        {
            Tasks[i] = task.gameObject;
            i++;
        }

        increment = 0;
        currTaskList = null;
        Hide();
	}

    internal void DisplayTaskList(TaskList newTaskList)
    {
        this.currTaskList = newTaskList;
        //TODO
    }

    private void Hide()
    {
        //TODO:
    }

    internal void Show()
    {
        //TODO:
    }

    //Dont use this if you can help it
    public void RcvNewTaskList(TaskList taskList, int numRcvdTaskLists){
        
    }

    internal void UpdateTasks()
    {
        int i = 0;
        foreach(GameObject task in Tasks)
        {
            task.GetComponent<TaskController>().SetTask(i + (Tasks.Length * increment));
        }
    }
}
