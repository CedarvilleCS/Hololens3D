using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.UIScripts.TaskLists
{
    //class TaskListChangeEvent
    //{
    //    int TaskListId;
    //    byte[] TaskStatuses;

    //    TaskListChangeEvent(int id, byte[] statuses)
    //    {
    //        this.TaskListId = id;
    //        this.TaskStatuses = statuses;
    //    }

    //    public TaskListChangeEvent FromTaskList(TaskList list)
    //    {
    //        bool[] completionValues = new bool[list.Tasks.Count];
    //        for(int i = 0; i < list.Tasks.Count; i++)
    //        {
    //            completionValues[i] = list.Tasks[i].IsCompleted;
    //        }
    //        return new TaskListChangeEvent(list.Id, BoolArrayToByteArray(completionValues));
    //    }

    //    public static byte[] BoolArrayToByteArray(bool[] boolArray)
    //    {
    //        byte[] toReturn = new byte[boolArray.Length / 8];
    //        for(int i = 0; i < boolArray.Length; i+= 8)
    //        {
    //            int length = 8;
    //            if(boolArray.Length - i < 8)
    //            {
    //                length = boolArray.Length - i;
    //            }
    //            toReturn[i / 8] = BoolArrayToByte(SubArray(boolArray, i, length));
    //        }
    //        return toReturn;
    //    }

    //    public static byte BoolArrayToByte(bool[] boolArray)
    //    {
    //        //boolArray must be size 8 or less
    //        byte toReturn = 0;
    //        foreach(bool b in boolArray)
    //        {
    //            if (b)
    //                toReturn++;
    //            toReturn <<= 1;
    //        }
    //        return toReturn;
    //    }

    //    public static T[] SubArray<T>(this T[] data, int index, int length)
    //    {
    //        T[] result = new T[length];
    //        Array.Copy(data, index, result, 0, length);
    //        return result;
    //    }
    //}
}
