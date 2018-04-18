using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskController : MonoBehaviour
{

    public TaskCheckController checkButton;
    public GameObject showImageButton;
    public Text TaskText;
    public int TaskNum;
    public TaskListViewerController tlvc;
    public TaskListCompletedTaskShowHide showChecked;
    public TaskListTitleController title;
    private Material _InvisibleMaterial;

    // Use this for initialization
    void Start()
    {
        TaskNum = -1;
        _InvisibleMaterial = ((Material)Resources.Load("InvisibleMaterial"));
    }

    public void SetTask(int t)
    {
        //if (showChecked.showCompleted)
        //{
        if (tlvc.currTaskList.Tasks.Count <= t)
        {
            TaskText.text = "";
            checkButton.SetBoxChecked(false);
            checkButton.Hide();
            TaskNum = -1;
            showImageButton.GetComponent<Renderer>().material = _InvisibleMaterial;
            showImageButton.GetComponent<TaskListViewImageController>().SetImage(null);
        }
        else
        {
            TaskItem task = tlvc.currTaskList.Tasks[t];
            TaskText.text = task.Name;
            checkButton.SetBoxChecked(task.IsCompleted);
            checkButton.Show();
            //TODO: Bug Tyler until this works
            showImageButton.GetComponent<TaskListViewImageController>().SetImage(task);
            TaskNum = t;
        }
        //}
        //else //hide completed
        //{
        //if (tlvc.incompleteTasks.Tasks.Count <= t)
        //{
        //    TaskText.text = "";
        //    checkButton.GetComponent<TaskCheckController>().SetBoxChecked(false);
        //    checkButton.GetComponent<TaskCheckController>().Hide();
        //    taskNum = -1;
        //    showImageButton.GetComponent<Renderer>().material = _InvisibleMaterial;
        //    showImageButton.GetComponent<TaskListViewImageController>().SetImage(null);
        //}
        //else
        //{
        //    TaskItem task = tlvc.incompleteTasks.Tasks[t];
        //    TaskText.text = task.Name;
        //    checkButton.GetComponent<TaskCheckController>().SetBoxChecked(task.IsCompleted);
        //    checkButton.GetComponent<TaskCheckController>().Show();
        //    //TODO: Bug Tyler until this works
        //    showImageButton.GetComponent<Renderer>().material.SetTexture("image", task.AttachmentTexture);
        //    taskNum = t;
        //    showImageButton.GetComponent<TaskListViewImageController>().SetImage(task);
        //}
        //}
    }


    internal void Checked(bool boxChecked)
    {
        tlvc.currTaskList.Tasks[TaskNum].IsCompleted = boxChecked;
        tlvc.UpdateTasks();
        //if (boxChecked)
        //{
        //    tlvc.incompleteTasks.Tasks.RemoveAt(taskNum);
        //}
        //else
        //{
        //    tlvc.incompleteTasks.Tasks.Insert(taskNum, tlvc.currTaskList.Tasks[taskNum]);
        //}
        title.SetTitle(tlvc.currTaskList.GetTitleWithNumCompleted());

        //TODO: Pass something to Tyler about how the task is checked.
        GameObject.Find("TaskListPane").GetComponent<TaskListReceiver>().SendTaskItemCompleteNotification(tlvc.taskListId, TaskNum, boxChecked);
    }

    internal bool HasImage()
    {
        if (TaskNum == -1)
        {
            return false;
        }
        return tlvc.currTaskList.Tasks[TaskNum].Attachment != null;
    }
}