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
    internal int taskNum;
    private TaskListViewerController tlvc;
    private TaskListCompletedTaskShowHide showChecked;
    private TaskListTitleController title;

    // Use this for initialization
    void Start()
    {
        showChecked = GameObject.Find("TaskListShowCompleted").GetComponent<TaskListCompletedTaskShowHide>();
        //Viewer is the grandparent
        tlvc = this.transform.parent.GetComponentInParent<TaskListViewerController>();
        title = GameObject.Find("TaskListTitle").GetComponent<TaskListTitleController>();
        taskNum = -1;
    }

    public void SetTask(int t)
    {
        if (showChecked.showCompleted)
        {
            if (tlvc.currTaskList.Tasks.Count <= t)
            {
                TaskText.text = "";
                checkButton.GetComponent<TaskCheckController>().SetBoxChecked(false);
                checkButton.GetComponent<TaskCheckController>().Hide();
               taskNum = -1;
            }
            else
            {
                TaskItem task = tlvc.currTaskList.Tasks[t];
                TaskText.text = task.Name;
                checkButton.GetComponent<TaskCheckController>().SetBoxChecked(task.IsCompleted);
                checkButton.GetComponent<TaskCheckController>().Show();
                //TODO: Bug Tyler until this works
                //showImageButton.GetComponent<Renderer>().material.mainTexture = task.AttachmentTexture;
                taskNum = t;
            }
        }
        else //hide completed
        {
            if (tlvc.incompleteTasks.Tasks.Count <= t)
            {
                TaskText.text = "";
                checkButton.GetComponent<TaskCheckController>().SetBoxChecked(false);
                checkButton.GetComponent<TaskCheckController>().Hide();
                taskNum = -1;
            }
            else
            {
                TaskItem task = tlvc.incompleteTasks.Tasks[t];
                TaskText.text = task.Name;
                checkButton.GetComponent<TaskCheckController>().SetBoxChecked(task.IsCompleted);
                checkButton.GetComponent<TaskCheckController>().Show();
                //TODO: Bug Tyler until this works
                //showImageButton.GetComponent<Renderer>().material.mainTexture = task.AttachmentTexture;
                taskNum = t;
            }
        }
    }


    internal void Checked(bool boxChecked)
    {
        tlvc.currTaskList.Tasks[taskNum].IsCompleted = boxChecked;
        tlvc.UpdateTasks();
        if (boxChecked)
        {
            tlvc.incompleteTasks.Tasks.RemoveAt(taskNum);
        } else
        {
            tlvc.incompleteTasks.Tasks.Insert(taskNum, tlvc.currTaskList.Tasks[taskNum]);
        }
        title.SetTitle(tlvc.currTaskList.GetTitleWithNumCompleted());

        //TODO: Pass something to Tyler about how the task is checked.
        //TaskListReciever.UpdateChecked(boxChecked, tlvc.taskListId, taskNum)
    }
}