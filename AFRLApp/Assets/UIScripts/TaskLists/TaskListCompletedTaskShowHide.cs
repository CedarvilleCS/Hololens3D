using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskListCompletedTaskShowHide : MonoBehaviour
{
    public bool showCompleted;

    // Use this for initialization
    void Start()
    {
        showCompleted = false;
    }

    // Update is called once per frame
    void OnSelect()
    {
        if (showCompleted)
        {
            //foreach (completed in ActiveTaskList){
            //Show them
            //}
        }
        else //hideCompleted
        {
            //foreach (completed in ActiveTaskList){
            //Hide them and make sure another one takes their place.
            //}
        }

        showCompleted = !showCompleted;
    }
}
