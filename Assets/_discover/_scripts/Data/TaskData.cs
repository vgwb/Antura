using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.DeInspektor.Attributes;
using System;

namespace Antura.Discover
{
    public enum TaskType
    {
        None = 0,
        Collect = 1,
        Reach = 2,
        Interact = 3,
    }

    [CreateAssetMenu(fileName = "TaskData", menuName = "Antura/Discover/Task")]
    public class TaskData : ScriptableObject
    {
        [Tooltip("Task Code used in a Node to activate it")]
        public string Code;
        public TaskType Type;
        [Tooltip("Total or per item is collect Task")]
        public int ProgressPoints;

        [Tooltip("Optional permalink of the Node with the mission")]
        public string NodeDescription;

        [Tooltip("Optional permalink of the Node activated if success task")]
        public string NodeSuccess;

        [Tooltip("Optional permalink of the Node activated if fail task")]
        public string NodeFail;

        [Tooltip("If Collect, how many items to collect?")]
        public int ItemCount;
        [Tooltip("If Collect, what tag to count?")]
        public string ItemTag;

        [Tooltip("Optional timer for the task, in seconds")]
        public int TimerSeconds = 0;

    }
}
