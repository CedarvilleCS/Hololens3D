using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskListShowGalleryController : MonoBehaviour {
    public enum TaskListWindows { TaskListImageViewer, TaskListViewer, TaskListGallery};
    public TaskListWindows currentlyShown;

	// Use this for initialization
	void Start () {
        currentlyShown = TaskListWindows.TaskListGallery;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
