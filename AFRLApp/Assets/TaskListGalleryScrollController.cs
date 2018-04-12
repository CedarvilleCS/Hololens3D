using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskListGalleryScrollController : MonoBehaviour
{
    private int _upOrDown;
    public bool isDown;
    public bool isVisible;
    public TaskListGalleryScrollController sibling;
    private Vector3 starterScale;
    public TaskListGalleryController tlgc;

    // Use this for initialization
    void Start()
    {
        _upOrDown = isDown ? 1 : -1;
        starterScale = this.transform.localScale;
        Hide();
    }

    // Update is called once per frame
    void Update()
    {
        if (tlgc != null)
        {
            CheckStatus();
        }
    }

    public void OnSelect()
    {
        Hide();
        tlgc.pageIncrement = tlgc.pageIncrement + _upOrDown;
        tlgc.UpdateThumbnails();
        CheckStatus();
        sibling.CheckStatus();
    }

    public void CheckStatus()
    {
        int numThumbs = tlgc.taskListThumbnails.Length;
        int incr = tlgc.pageIncrement;
        int numTaskLists = tlgc.taskLists.Count;

        if (isDown)
        {
            if ((numTaskLists <= numThumbs) || (numTaskLists <= (incr + 1) * numThumbs))
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
