using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskListGalleryController : MonoBehaviour
{

    public GameObject[] taskListThumbnails;
    public List<TaskList> taskLists;
    private bool _newListRecieved;
    public int pageIncrement;

    // Use this for initialization
    void Start()
    {
        taskLists = new List<TaskList>();
        _newListRecieved = false;
        pageIncrement = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (_newListRecieved)
        {
            if ((pageIncrement + 1) * taskListThumbnails.Length > taskLists.Count)
            {
                int x = (taskLists.Count-1) % taskListThumbnails.Length;
                taskListThumbnails[x].GetComponent<TaskListThumbnailController>().SetThumbnail(taskLists[taskLists.Count - 1]);
            }
        }
    }

    public void RcvNewTaskList(TaskList taskList, int numRcvdTaskLists)
    {
        taskLists.Add(taskList);
        _newListRecieved = true;
    }
}
