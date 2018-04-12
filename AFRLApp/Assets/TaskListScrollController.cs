using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskListScrollController : MonoBehaviour
{
    private int _upOrDown;
    public bool isDown;
    public bool isVisible;
    public TaskListScrollController sibling;
    private Vector3 starterScale;
    public TaskListViewerController tlvc;

    // Use this for initialization
    void Start()
    {
        _upOrDown = isDown ? 1 : -1;
        starterScale = this.transform.localScale;
        Hide();
    }

    public void OnSelect()
    {
        Hide();
        tlvc.increment = tlvc.increment + _upOrDown;
        tlvc.UpdateTasks();
        CheckStatus();
        sibling.CheckStatus();
    }

    private void Update()
    {
        if (tlvc != null)
        {
            CheckStatus();
        }
    }

    public void CheckStatus()
    {
        int numThumbs = tlvc.TaskThumbnails.Length;
        int incr = tlvc.increment;
        if (tlvc.currTaskList != null)
        {
            int numTasks = tlvc.currTaskList.Tasks.Count;


            if (isDown)
            {
                if ((numTasks <= numThumbs) || (numTasks <= (incr + 1) * numThumbs))
                {
                    this.Hide();
                }
                else
                {
                    this.Show();
                }
            }
            else
            {
                if (incr == 0)
                {
                    this.Hide();
                }
                else
                {
                    this.Show();
                }
            }
        }
    }

    void Show()
    {
        this.transform.localScale = starterScale;
        isVisible = true;
    }

    void Hide()
    {
        this.transform.localScale = new Vector3(0, 0, 0);
        isVisible = false;
    }
}
