using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.DeInspektor.Attributes;
using System;

namespace Antura.Discover
{

    [Serializable]
    public class QuestTask
    {
        [Tooltip("Task Code used in a Node to activate it")]
        public string Code;
        public TaskType Type;
        [Tooltip("Optional for reach and interact tasks")]
        public GameObject InteractGO;
        [Tooltip("Total or per item is collect Task")]
        public int ProgressPoints;

        [Tooltip("The line with the title")]
        public string LineTitle;

        [Tooltip("Optional permalink of the Node with the mission")]
        public string NodeDescription;

        [Tooltip("Optional permalink of the Node activated if success task")]
        public string NodeSuccess;

        [Tooltip("Optional permalink of the Node activated if fail task")]
        public string NodeFail;

        [DeConditional("Type", (int)TaskType.Collect)]
        public GameObject ItemsContainer;
        [Tooltip("If Collect, how many items to collect?")]
        public int ItemCount;
        [Tooltip("If Collect, what tag to count?")]
        public string ItemTag;

        [Tooltip("Optional timer for the task, in seconds")]
        public int TimerSeconds = 0;

        [Tooltip("Optional target to activate.")]
        public GameObject TargetPoint;
        [Tooltip("Optional area to activate.")]
        public GameObject Area;
        public QuestNode InfoNode;
        public int Collected { get; private set; }

        public bool Completed { get; private set; }

        [NonSerialized]
        private string pendingReturnNode = string.Empty;
        public string PendingReturnNode => pendingReturnNode;


        public void Setup()
        {
            if (ItemsContainer != null)
            {
                ItemsContainer.SetActive(false);
            }

            // count items in the container
            if (ItemCount <= 0 && ItemsContainer != null)
            {
                ItemCount = ItemsContainer.transform.childCount;
                Debug.LogWarning($"QuestTask {Code} ItemCount was not set, using {ItemCount} from ItemsContainer");
            }

            ResetRuntimeState();
        }

        public void Activate()
        {
            // Debug.Log($"Activating Task: {Code}, Type: {Type}, ProgressPoints: {ProgressPoints}");
            if (Type == TaskType.Interact)
            {
                if (InteractGO != null)
                {
                    InteractGO.SetActive(true);
                    Interactable interactable = InteractGO.GetComponent<Interactable>();
                    if (interactable != null)
                    {
                        interactable.SetActivated(true);
                    }
                }
                UIManager.I.TaskDisplay.Show(InfoNode, 0);
                if (TargetPoint != null)
                {
                    ActionManager.I.Target(TargetPoint.transform);
                }
            }
            else if (Type == TaskType.Collect)
            {
                if (ItemsContainer != null)
                {
                    ItemsContainer.SetActive(true);
                }
                UIManager.I.TaskDisplay.Show(InfoNode, ItemCount);
            }

        }


        public void ResetRuntimeState()
        {
            Collected = 0;
            Completed = false;
            pendingReturnNode = string.Empty;
        }

        public void Begin(string nodeReturn)
        {
            Collected = 0;
            Completed = false;
            pendingReturnNode = nodeReturn ?? string.Empty;
        }

        public void IncrementCollected()
        {
            Collected++;
        }

        public void MarkCompleted()
        {
            Completed = true;
        }

        public void ClearReturnNode()
        {
            pendingReturnNode = string.Empty;
        }


        public int GetTaskTotalePoints()
        {
            if (Type == TaskType.Collect && ItemsContainer != null)
            {
                // If it's a collect task, return the number of items to collect
                return ItemCount * ProgressPoints;
            }
            return ProgressPoints;
        }

        public int GetSuccessPoints()
        {
            if (Type != TaskType.Collect)
            {
                return ProgressPoints;
            }
            return ProgressPoints * ItemCount;
        }

    }
}
