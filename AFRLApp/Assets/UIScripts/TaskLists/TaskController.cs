using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskController : MonoBehaviour
{

    public GameObject checkButton;
    public GameObject showImageButton;
    public Text TaskText;
    private bool taskVisible;
    private int taskNum;
    private TaskListViewerController tlvc;
    private TaskListCompletedTaskShowHide showChecked;

    // Use this for initialization
    void Start()
    {
        showChecked = GameObject.Find("TaskListShowCompleted").GetComponent<TaskListCompletedTaskShowHide>();
        //Viewer is the grandparent
        tlvc = this.transform.parent.GetComponentInParent<TaskListViewerController>();
    }

    public void SetTask(int t)
    {
        if (tlvc.currTaskList.Tasks.Count >= t)
        {
            TaskText.text = "";
        }
        else if (tlvc.currTaskList.Tasks[t].IsCompleted && !showChecked.showCompleted)
        {
            SetTask(t + 1);
        }
        else
        {
            TaskItem task = tlvc.currTaskList.Tasks[t];
            TaskText.text = task.Name;
            //TODO: Bug Tyler until this works
            //showImageButton.GetComponent<Renderer>().material.mainTexture = task.AttachmentTexture;
            taskNum = t;
        }
    }


    internal void Checked(bool boxChecked)
    {
        tlvc.currTaskList.Tasks[taskNum].IsCompleted = boxChecked;
        tlvc.UpdateTasks();
    }
}
