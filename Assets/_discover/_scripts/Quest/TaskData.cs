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

    [System.Serializable]
    public class TaskData
    {
        [Tooltip("Task Code used in a Node to activate it")]
        public string Code;
        public TaskType Type;
        [Tooltip("Optional for reach and interact tasks")]
        public GameObject InteractGO;
        [Tooltip("Total or per item is collect Task")]
        public int ProgressPoints;

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

        private int itemsCollected = 0;

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
            }
            itemsCollected = 0;
        }

        public void Activate()
        {
            // Debug.Log($"Activating Task: {Code}, Type: {Type}, ProgressPoints: {ProgressPoints}");
            if (Type == TaskType.Interact || Type == TaskType.Reach)
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
                UIManager.I.TaskDisplay.Show(Code, 0);
                if (TargetPoint != null)
                {
                    ActionManager.I.FocusTarget(TargetPoint.transform);
                }
            }
            else if (Type == TaskType.Collect)
            {
                if (ItemsContainer != null)
                {
                    ItemsContainer.SetActive(true);
                }
                UIManager.I.TaskDisplay.Show(Code, ItemCount);
            }

        }

        public void ItemCollected()
        {
            if (Type == TaskType.Collect && ItemsContainer != null)
            {
                itemsCollected++;
                if (itemsCollected >= ItemCount)
                {
                    // Task completed
                    QuestManager.I.TaskSuccess(Code);
                }
                else
                {
                    UIManager.I.TaskDisplay.SetTotItemsCollected(itemsCollected);
                }
            }
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
            // collect points are collected during the task
            return 0;
        }

    }
}
