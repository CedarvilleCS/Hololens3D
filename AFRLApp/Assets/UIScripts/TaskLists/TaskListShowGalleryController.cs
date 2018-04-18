using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskListShowGalleryController : MonoBehaviour
{
    public enum TaskListWindows { TaskListImageViewer, TaskListViewer, TaskListGallery };
    public TaskListWindows currentlyShown;
    public TaskListGalleryController tlGalleryController;
    public TaskListViewerController tlViewerController;
    public TaskListReturnButton tlReturnButton;

    private void Start()
    {
        currentlyShown = TaskListWindows.TaskListGallery;
        OnSelect();
    }

    void OnSelect()
    {
        if (currentlyShown == TaskListWindows.TaskListGallery && tlViewerController.currTaskList == null)
        {
            tlViewerController.Hide();
            tlGalleryController.Show();
            tlReturnButton.Hide();
        }
        else if (currentlyShown == TaskListWindows.TaskListGallery)
        {
            tlViewerController.Show();
            tlGalleryController.Hide();
            tlReturnButton.Hide();
            currentlyShown = TaskListWindows.TaskListViewer;
        }
        else if (currentlyShown == TaskListWindows.TaskListViewer)
        {
            tlViewerController.Hide();
            tlGalleryController.Show();
            tlReturnButton.Hide();
            currentlyShown = TaskListWindows.TaskListGallery;
        }
        else //TaskListImageViewer
        {
            tlViewerController.Show();
            tlGalleryController.Hide();
            tlReturnButton.Hide();
            currentlyShown = TaskListWindows.TaskListViewer;
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
