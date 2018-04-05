using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabSelector : MonoBehaviour
{
    public enum TabState { None, Image, PDF, TaskList, HelpSheet, Other };

    public TabState tabState;
    public ImageReceiver imageReceiver;
    public PDFReceiver pdfReceiver;
    public TaskListReceiver taskListReceiver;
    public HelpSheetController helpSheetController;

    void Start()
    {
        tabState = TabState.HelpSheet;
    }

    // Update is called once per frame
    void Update()
    {
        //TODO: Hide everything except what needs to be shown for the given case;
        switch (tabState)
        {
            case TabState.HelpSheet:
                imageReceiver.Hide();
                taskListReceiver.Hide();
                pdfReceiver.Hide();
                helpSheetController.Show();
                break;
            case TabState.Image:
                imageReceiver.Show();
                taskListReceiver.Hide();
                pdfReceiver.Hide();
                helpSheetController.Hide();
                break;
            case TabState.PDF:
                imageReceiver.Hide();
                taskListReceiver.Hide();
                pdfReceiver.Show();
                helpSheetController.Hide();
                break;
            case TabState.TaskList:
                imageReceiver.Hide();
                taskListReceiver.Show();
                pdfReceiver.Hide();
                helpSheetController.Hide();
                break;
            case TabState.Other:
                //Do nothing
                break;
            case TabState.None:
                //Do nothing
                break;
        }
    }

    internal TabState CurrentTabState()
    {
        return tabState;
    }

    internal void SetCurrentState(TabState holder)
    {
        tabState = holder;
    }

    internal void MakePopout()
    {
        switch (tabState)
        {
            case TabState.Image:
                imageReceiver.MakeNewPopOut();
                break;
            case TabState.PDF:
                pdfReceiver.MakeNewPopOut();
                break;
            case TabState.TaskList:
                taskListReceiver.MakeNewPopOut();
                break;
            default:
                //Do nothing
                return;
        }
    }
}
