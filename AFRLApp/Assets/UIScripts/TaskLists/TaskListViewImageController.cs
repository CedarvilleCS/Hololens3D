using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskListViewImageController : MonoBehaviour
{
    public TaskListViewerController tlvc;
    public TaskListShowGalleryController tlsgc;
    public GameObject imageViewer;
    private Vector3 starterScale;

    private void Start()
    {
        imageViewer = GameObject.Find("TaskListImageViewer");
        starterScale = this.transform.localScale;
        tlvc = GameObject.Find("TaskListViewer").GetComponent<TaskListViewerController>();
        tlsgc = GameObject.Find("TaskListShowGalleryButton").GetComponent<TaskListShowGalleryController>();
    }

    void OnSelect()
    {
        if (this.transform.parent.GetComponent<TaskController>().HasImage())
        {
            imageViewer.GetComponent<Renderer>().material.mainTexture = this.GetComponent<Renderer>().material.mainTexture;
            imageViewer.GetComponentInChildren<TaskListReturnButton>().Show();
            tlvc.Hide();
            tlsgc.ImageViewerCurrentlyShown();
        }
    }

    internal void SetImage(TaskItem task)
    {
        if (task == null || task.AttachmentTexture == null)
        {
            this.Hide();
        }
        else
        {
            this.Show();
            this.GetComponent<Renderer>().material.mainTexture = task.AttachmentTexture;
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
}
