using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace AssemblyCSharpWSA
{
    class TaskListReceiver : MonoBehaviour
    {
        private TaskList _nextTaskList;
        private bool _newTaskListPresent;
        public bool FirstInstance = true;
        public int NumRcvdTaskLists = 0;
        public int ResetNumRcvdTaskLists;
        public List<TaskList> taskLists;
        public int debugCount;

        public bool DebugMake { get; private set; }

        private void Awake()
        {
            taskLists = new List<TaskList>();
            HLNetwork.ObjectReceiver objr = HLNetwork.ObjectReceiver.getTheInstance();
            objr.TaskListReceived += OnTaskListReceived;
            if (!FirstInstance)
            {
                NumRcvdTaskLists = ResetNumRcvdTaskLists;
            }

            debugCount = 0;

            DebugMake = true;
        }

        private void MakeDebug()
        {
            GameObject taskListGallery = this.transform.Find("TaskListGallery").gameObject;
            GameObject taskListViewer = this.transform.Find("TaskListViewer").gameObject;

            _nextTaskList = new TaskList();
            _nextTaskList.Id = debugCount + 1;
            _nextTaskList.Name = "TaskList1";
            _nextTaskList.Tasks.Add(new TaskItem(0, "Hello"));
            _nextTaskList.Tasks.Add(new TaskItem(1, "World!"));
            _nextTaskList.Tasks.Add(new TaskItem(2, "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA!"));
            NumRcvdTaskLists++;
            taskListGallery.GetComponent<TaskListGalleryController>().RcvNewTaskList(_nextTaskList, NumRcvdTaskLists);
            taskListViewer.GetComponent<TaskListViewerController>().RcvNewTaskList(_nextTaskList, NumRcvdTaskLists);

            //_nextTaskList = new TaskList();
            //_nextTaskList.Id = debugCount + 1;
            //_nextTaskList.Name = "TaskList2";
            //_nextTaskList.Tasks.Add(new TaskItem(0, "This is"));
            //_nextTaskList.Tasks.Add(new TaskItem(1, "also"));
            //_nextTaskList.Tasks.Add(new TaskItem(2, "a task list."));
            //NumRcvdTaskLists++;
            //taskListGallery.GetComponent<TaskListGalleryController>().RcvNewTaskList(_nextTaskList, NumRcvdTaskLists);
            //taskListViewer.GetComponent<TaskListViewerController>().RcvNewTaskList(_nextTaskList, NumRcvdTaskLists);

            //_nextTaskList = new TaskList();
            //_nextTaskList.Id = debugCount + 1;
            //_nextTaskList.Name = "TaskList4";
            //_nextTaskList.Tasks.Add(new TaskItem(0, "This"));
            //_nextTaskList.Tasks.Add(new TaskItem(1, "task list"));
            //_nextTaskList.Tasks.Add(new TaskItem(2, "has"));
            //_nextTaskList.Tasks.Add(new TaskItem(3, "several"));
            //_nextTaskList.Tasks.Add(new TaskItem(4, "more tasks than your average task list and this should cause it to have more than one page"));
            //NumRcvdTaskLists++;
            //taskListGallery.GetComponent<TaskListGalleryController>().RcvNewTaskList(_nextTaskList, NumRcvdTaskLists);
            //taskListViewer.GetComponent<TaskListViewerController>().RcvNewTaskList(_nextTaskList, NumRcvdTaskLists);

            //_nextTaskList = new TaskList();
            //_nextTaskList.Id = debugCount + 1;
            //_nextTaskList.Name = "TaskListAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA!";
            //_nextTaskList.Tasks.Add(new TaskItem(0, "One task here"));
            //NumRcvdTaskLists++;
            //taskListGallery.GetComponent<TaskListGalleryController>().RcvNewTaskList(_nextTaskList, NumRcvdTaskLists);
            //taskListViewer.GetComponent<TaskListViewerController>().RcvNewTaskList(_nextTaskList, NumRcvdTaskLists);

            //_nextTaskList = new TaskList();
            //_nextTaskList.Id = debugCount + 1;
            //_nextTaskList.Name = "TaskList6";
            //_nextTaskList.Tasks.Add(new TaskItem(0, "This"));
            //_nextTaskList.Tasks.Add(new TaskItem(1, "task list"));
            //_nextTaskList.Tasks.Add(new TaskItem(2, "is on a different page!"));
            //NumRcvdTaskLists++;
            //taskListGallery.GetComponent<TaskListGalleryController>().RcvNewTaskList(_nextTaskList, NumRcvdTaskLists);
            //taskListViewer.GetComponent<TaskListViewerController>().RcvNewTaskList(_nextTaskList, NumRcvdTaskLists);

            DebugMake = false;
        }

        private void Update()
        {
            if (_newTaskListPresent)
            {
                taskLists.Add(_nextTaskList);
                _newTaskListPresent = false;
                Debug.Log("Received new Task List");
                NumRcvdTaskLists++;

                //TODO: send task list to GUI
                GameObject taskListGallery = this.transform.Find("TaskListGallery").gameObject;
                GameObject taskListViewer = this.transform.Find("TaskListViewer").gameObject;

                taskListGallery.GetComponent<TaskListGalleryController>().RcvNewTaskList(_nextTaskList, NumRcvdTaskLists);
                taskListViewer.GetComponent<TaskListViewerController>().RcvNewTaskList(_nextTaskList, NumRcvdTaskLists);
            }
            if (DebugMake == true)
            {
                MakeDebug();
            }
        }

        void OnTaskListReceived(object obj, HLNetwork.TaskListReceivedEventArgs args)
        {
            _nextTaskList = args.TaskList;
            _newTaskListPresent = true;
        }

        public void OnWindowClosed()
        {
            HLNetwork.ObjectReceiver objr = HLNetwork.ObjectReceiver.getTheInstance();
            objr.TaskListReceived -= OnTaskListReceived;
        }

        public int GetNumTaskLists()
        {
            return taskLists.Count;
        }

        public void SendTaskItemCompleteNotification(int taskListId, int taskIndex, bool completed)
        {
            //4 bytes
            byte[] taskListIdData = System.BitConverter.GetBytes(taskListId);
            //4 bytes
            byte[] taskIndexData = System.BitConverter.GetBytes(taskListId);
            //1 byte
            byte[] completedData = System.BitConverter.GetBytes(completed);
            //9 bytes
            byte[] data = new byte[9];
            taskListIdData.CopyTo(data, 0);
            taskIndexData.CopyTo(data, 4);
            completedData.CopyTo(data, 8);
            HLNetwork.ObjectReceiver objr = HLNetwork.ObjectReceiver.getTheInstance();
            objr.SendData(HLNetwork.ObjectReceiver.MessageType.TaskListComplete, data);
        }
    }
}
