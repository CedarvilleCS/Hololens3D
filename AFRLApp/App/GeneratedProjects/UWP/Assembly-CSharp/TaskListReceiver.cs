using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        private void Awake()
        {
            taskLists = new List<TaskList>();
            HLNetwork.ObjectReceiver objr = HLNetwork.ObjectReceiver.getTheInstance();
            objr.TaskListReceived += OnTaskListReceived;
            if (!FirstInstance)
            {
                NumRcvdTaskLists = ResetNumRcvdTaskLists;
            }
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
    }
}
