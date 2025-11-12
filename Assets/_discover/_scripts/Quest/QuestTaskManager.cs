using System;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Discover
{
    public class QuestTaskManager
    {
        private readonly List<QuestTask> registeredTasks = new List<QuestTask>();
        public QuestTask CurrentTask { get; private set; }

        private int tasksMaxPoints = 0;
        public int GetMaxPoints() { return tasksMaxPoints; }

        public string CurrentTaskCode => CurrentTask != null ? CurrentTask.Code : string.Empty;
        private string EndTaskNodeToRun = "";

        public void RegisterTasks(IEnumerable<QuestTask> tasks)
        {
            registeredTasks.Clear();
            CurrentTask = null;
            tasksMaxPoints = 0;

            if (tasks == null)
                return;

            foreach (var task in tasks)
            {
                if (task == null || string.IsNullOrEmpty(task.Code))
                    continue;
                task.ResetRuntimeState();
                registeredTasks.Add(task);
                tasksMaxPoints += task.GetSuccessPoints();
            }
        }

        public void SetTaskDescription(string taskCode, QuestNode questNode)
        {
            if (string.IsNullOrEmpty(taskCode))
                return;

            // Set description from the quest node text
            if (TryGetTask(taskCode, out var task) && task != null)
            {
                task.InfoNode = questNode;
            }
        }

        public bool StartTask(string taskCode, string nodeReturn = "")
        {
            if (string.IsNullOrEmpty(taskCode))
                return false;

            if (!TryGetTask(taskCode, out var task) || task == null)
            {
                Debug.LogWarning($"QuestTaskManager.StartTask: task not found '{taskCode}'.");
                return false;
            }

            CurrentTask = task;
            EndTaskNodeToRun = "";
            task.Begin(nodeReturn);

            // Activate task content; QuestTask.Activate handles showing TaskDisplay appropriately
            task.Activate();
            if (task.Type == TaskType.Collect)
            {
                UIManager.I.TaskDisplay.SetTotItemsCollected(0);
            }

            return true;
        }

        public bool EndTask(string taskCode, bool success = true)
        {
            if (string.IsNullOrEmpty(taskCode))
                return false;

            if (!TryGetTask(taskCode, out var task) || task == null)
                return false;

            if (success)
            {
                task.MarkCompleted();
                // Award points via QuestManager Progress
                QuestManager.I.Progress.AddProgressPoints(task.GetSuccessPoints());
            }

            UIManager.I.TaskDisplay.Hide();

            if (CurrentTask == task)
                CurrentTask = null;

            task.ClearReturnNode();

            return true;
        }

        public void OnCollectItemTag(string tag)
        {
            if (CurrentTask == null)
                return;

            var task = CurrentTask;

            if (task.Type != TaskType.Collect)
                return;

            if (!string.IsNullOrEmpty(task.ItemTag) && !string.Equals(task.ItemTag, tag))
                return;

            task.IncrementCollected();
            UIManager.I.TaskDisplay.SetTotItemsCollected(task.Collected);

            if (task.ItemCount > 0 && task.Collected >= task.ItemCount)
            {
                var code = task.Code;
                var nodeReturn = task.PendingReturnNode;
                EndTask(code, true);
                if (!string.IsNullOrEmpty(nodeReturn))
                {
                    Debug.Log($"QuestTaskManager: Starting dialogue node '{nodeReturn}' after collecting items for task '{code}'.");
                    if (DiscoverGameManager.I.State != GameplayState.Dialogue)
                    {
                        Debug.Log($"NOT dialogue.");
                        EndTaskNodeToRun = "";
                        YarnAnturaManager.I?.StartDialogue(nodeReturn);
                    }
                    else
                    {
                        Debug.Log($"IN dialogue and nodeReturn is " + nodeReturn);
                        EndTaskNodeToRun = nodeReturn;
                    }
                }
            }
        }

        public void CheckEndTaskNode()
        {
            Debug.Log($"QuestTaskManager: CheckEndTaskNode called, EndTaskNodeToRun is '{EndTaskNodeToRun}'");
            if (!string.IsNullOrEmpty(EndTaskNodeToRun))
            {
                var nodeToRun = EndTaskNodeToRun;
                EndTaskNodeToRun = "";
                YarnAnturaManager.I?.StartDialogueAgain(nodeToRun);
            }
        }

        public void OnReachTarget(string taskCode)
        {
            if (string.IsNullOrEmpty(taskCode))
                return;

            string nodeReturn = null;
            if (TryGetTask(taskCode, out var task) && task != null)
                nodeReturn = task.PendingReturnNode;

            if (EndTask(taskCode, true) && !string.IsNullOrEmpty(nodeReturn))
            {
                YarnAnturaManager.I?.StartDialogue(nodeReturn);
            }
        }

        public void OnInteract(string taskCode)
        {
            if (string.IsNullOrEmpty(taskCode))
                return;

            string nodeReturn = null;
            if (TryGetTask(taskCode, out var task) && task != null)
                nodeReturn = task.PendingReturnNode;

            if (EndTask(taskCode, true) && !string.IsNullOrEmpty(nodeReturn))
            {
                YarnAnturaManager.I?.StartDialogue(nodeReturn);
            }
        }

        /// <summary>
        /// Call when an Interactable has been used by the player. Completes the current Interact task if it targets this object.
        /// </summary>
        public void OnInteractableUsed(Interactable interacted)
        {
            if (interacted == null)
                return;
            var task = CurrentTask;
            if (task == null)
                return;
            if (task.Type != TaskType.Interact)
                return;
            if (task.InteractGO == null)
                return;

            if (task.InteractGO != interacted.gameObject)
                return;

            Debug.Log($"HELP QuestTaskManager: Interact task '{task.Code}' completed by using interactable '{interacted.gameObject.name}'.");

            // Capture nodeReturn, end task, then jump if provided
            var nodeReturn = task.PendingReturnNode;
            var code = task.Code;
            EndTask(code, true);
            if (!string.IsNullOrEmpty(nodeReturn))
            {
                YarnAnturaManager.I?.StartDialogue(nodeReturn);
            }
        }

        public void OnInteractCard(CardData card)
        {
            if (string.IsNullOrEmpty(card.CustomTag))
                return;
            OnCollectItemTag(card.CustomTag);
        }

        public int GetCollectedCount(string taskCode)
        {
            return TryGetTask(taskCode, out var task) && task != null ? task.Collected : 0;
        }

        public bool IsTaskCompleted(string taskCode)
        {
            return TryGetTask(taskCode, out var task) && task != null && task.Completed;
        }

        public bool TryGetTask(string code, out QuestTask task)
        {
            task = null;
            if (string.IsNullOrEmpty(code))
                return false;

            for (int i = 0; i < registeredTasks.Count; i++)
            {
                var candidate = registeredTasks[i];
                if (candidate != null && string.Equals(candidate.Code, code, StringComparison.OrdinalIgnoreCase))
                {
                    task = candidate;
                    return true;
                }
            }

            var questTasks = QuestManager.I != null ? QuestManager.I.QuestTasks : null;
            if (questTasks != null)
            {
                for (int i = 0; i < questTasks.Length; i++)
                {
                    var candidate = questTasks[i];
                    if (candidate != null && string.Equals(candidate.Code, code, StringComparison.OrdinalIgnoreCase))
                    {
                        task = candidate;
                        bool alreadyRegistered = false;
                        for (int j = 0; j < registeredTasks.Count; j++)
                        {
                            var existing = registeredTasks[j];
                            if (existing != null && string.Equals(existing.Code, candidate.Code, StringComparison.OrdinalIgnoreCase))
                            {
                                registeredTasks[j] = candidate;
                                alreadyRegistered = true;
                                break;
                            }
                        }
                        if (!alreadyRegistered)
                        {
                            registeredTasks.Add(candidate);
                        }
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
