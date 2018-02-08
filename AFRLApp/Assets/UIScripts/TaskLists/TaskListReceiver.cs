using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace AssemblyCSharpWSA
{
    public class TaskListReceiver : MonoBehaviour
    {
        private TaskList _nextTaskList;
        private bool _newTaskListPresent;
        public bool FirstInstance = true;
        public int NumRcvdTaskLists = 0;
        public int ResetNumRcvdTaskLists;
        public List<TaskList> taskLists;

        private void Awake()
        {
            taskLists = new List<TaskList>();
            HLNetwork.ObjectReceiver objr = HLNetwork.ObjectReceiver.getTheInstance();
            objr.TaskListReceived += OnTaskListReceived;
            if (!FirstInstance)
            {
                NumRcvdTaskLists = ResetNumRcvdTaskLists;
            }
            starterScale = this.transform.parent.transform.localScale;
        }

        private void Update()
        {
            if (_newTaskListPresent)
            {
                _newTaskListPresent = false;
                GameObject taskListGallery = this.transform.Find("TaskListGallery").gameObject;
                GameObject taskListViewer = this.transform.Find("TaskListViewer").gameObject;

                if (taskLists.Exists(x => x.Id == _nextTaskList.Id))
                {
                    int indexOf = taskLists.FindIndex(x => x.Id == _nextTaskList.Id);
                    taskLists[indexOf] = _nextTaskList;
                    Debug.Log("Updated existing Task List");
                }
                else
                {
                    taskLists.Add(_nextTaskList);
                    Debug.Log("Received new Task List");
                    NumRcvdTaskLists++;
                }
                taskListGallery.GetComponent<TaskListGalleryController>().RcvNewTaskList(taskLists, NumRcvdTaskLists);
                taskListViewer.GetComponent<TaskListViewerController>().RcvNewTaskList();
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
        private Vector3 starterScale;

        internal void Show()
        {
            this.transform.localScale = starterScale;
        }

        internal void Hide()
        {
            this.transform.localScale = new Vector3(0, 0, 0);
        }
    }
}
