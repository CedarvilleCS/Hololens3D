using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskListViewImageController : MonoBehaviour {
    public GameObject imageViewer;

    private void Start()
    {
        imageViewer = GameObject.Find("TaskListImageViewer");
    }

    void OnSelect()
    {
        imageViewer.GetComponent<Renderer>().material.mainTexture = this.GetComponent<Renderer>().material.mainTexture;
        imageViewer.GetComponentInChildren<TaskListReturnButton>().Show();
    }
}
