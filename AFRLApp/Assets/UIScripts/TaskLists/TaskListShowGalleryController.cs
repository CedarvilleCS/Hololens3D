using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskListShowGalleryController : MonoBehaviour
{
    public enum TaskListWindows { TaskListImageViewer, TaskListViewer, TaskListGallery };
    public TaskListWindows currentlyShown;
    public TaskListGalleryController tlgc;
    public TaskListViewerController tlvc;
    public TaskListReturnButton tlrb;

    // Use this for initialization
    void Start()
    {
        currentlyShown = TaskListWindows.TaskListGallery;
        tlgc = GameObject.Find("TaskListGallery").GetComponent<TaskListGalleryController>();
        tlvc = GameObject.Find("TaskListViewer").GetComponent<TaskListViewerController>();
        tlrb = GameObject.Find("TaskListImageViewer/BackButton").GetComponent<TaskListReturnButton>();
    }

    void OnSelect()
    {
        if (currentlyShown == TaskListWindows.TaskListImageViewer)
        {
            //currentlyShown = 
        }
        else if (currentlyShown == TaskListWindows.TaskListViewer)
        {

        }
        else //TaskListGallery 
        {

        }
    }

    public void ImageViewerCurrentlyShown()
    {
        currentlyShown = TaskListWindows.TaskListImageViewer;
    }

    public void TaskViewerCurrentlyShown()
    {
        currentlyShown = TaskListWindows.TaskListViewer;
    }

    public void ListGalleryCurrentlyShown()
    {
        currentlyShown = TaskListWindows.TaskListGallery;
    }
}
