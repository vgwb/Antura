using System.Collections.Generic;
using UnityEngine;

namespace Antura.Discover
{
    public class QuestTaskManager
    {
        public class TaskState
        {
            public string Code;
            public int Collected;
            public bool Completed;
            public string NodeReturn; // optional node to return to on completion
        }

        private readonly QuestManager _quest;
        private readonly Dictionary<string, QuestTask> _tasksByCode = new Dictionary<string, QuestTask>();
        private readonly Dictionary<string, TaskState> _statesByCode = new Dictionary<string, TaskState>();
        private string _currentTaskCode;

        private int maxPoints = 0;
        public int GetMaxPoints() { return maxPoints; }

        public string CurrentTaskCode => _currentTaskCode;

        public QuestTaskManager(QuestManager quest)
        {
            _quest = quest;
        }

        public void RegisterTasks(IEnumerable<QuestTask> tasks)
        {
            _tasksByCode.Clear();
            _statesByCode.Clear();
            _currentTaskCode = null;

            if (tasks == null)
                return;

            foreach (var t in tasks)
            {
                if (t == null || string.IsNullOrEmpty(t.Code))
                    continue;
                _tasksByCode[t.Code] = t;
                _statesByCode[t.Code] = new TaskState
                {
                    Code = t.Code,
                    Collected = 0,
                    Completed = false,
                    NodeReturn = string.Empty
                };
                maxPoints += t.GetSuccessPoints();
            }
        }

        public bool StartTask(string taskCode, string nodeReturn = "")
        {
            if (string.IsNullOrEmpty(taskCode))
                return false;

            if (!_tasksByCode.TryGetValue(taskCode, out var task))
            {
                Debug.LogWarning($"QuestTaskManager.StartTask: task not found '{taskCode}'.");
                return false;
            }

            if (!string.IsNullOrEmpty(_currentTaskCode) && _currentTaskCode != taskCode)
            {
                Debug.LogWarning($"QuestTaskManager: replacing active task '{_currentTaskCode}' with '{taskCode}'.");
            }

            _currentTaskCode = taskCode;
            var state = _statesByCode[taskCode];
            state.Collected = 0;
            state.Completed = false;
            state.NodeReturn = nodeReturn ?? string.Empty;

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

            if (!_tasksByCode.TryGetValue(taskCode, out var task))
                return false;

            if (!_statesByCode.TryGetValue(taskCode, out var state))
                return false;

            if (success)
            {
                state.Completed = true;
                // Award points via QuestManager Progress
                _quest.Progress.AddProgressPoints(task.GetSuccessPoints());
            }

            UIManager.I.TaskDisplay.Hide();

            if (_currentTaskCode == taskCode)
                _currentTaskCode = null;

            return true;
        }

        public void OnCollectItemTag(string tag)
        {
            if (string.IsNullOrEmpty(_currentTaskCode))
                return;

            if (!_tasksByCode.TryGetValue(_currentTaskCode, out var task))
                return;

            if (task.Type != TaskType.Collect)
                return;

            if (!string.IsNullOrEmpty(task.ItemTag) && !string.Equals(task.ItemTag, tag))
                return;

            var state = _statesByCode[_currentTaskCode];
            state.Collected++;
            UIManager.I.TaskDisplay.SetTotItemsCollected(state.Collected);

            if (task.ItemCount > 0 && state.Collected >= task.ItemCount)
            {
                var code = _currentTaskCode;
                var nodeReturn = state.NodeReturn;
                EndTask(code, true);
                if (!string.IsNullOrEmpty(nodeReturn))
                {
                    YarnAnturaManager.I?.StartDialogue(nodeReturn);
                }
            }
        }

        public void OnReachTarget(string taskCode) => EndTask(taskCode, true);
        public void OnInteract(string taskCode) => EndTask(taskCode, true);

        /// <summary>
        /// Call when an Interactable has been used by the player. Completes the current Interact task if it targets this object.
        /// </summary>
        public void OnInteractableUsed(Interactable interacted)
        {
            if (interacted == null)
                return;
            if (string.IsNullOrEmpty(_currentTaskCode))
                return;
            if (!_tasksByCode.TryGetValue(_currentTaskCode, out var task))
                return;
            if (task.Type != TaskType.Interact)
                return;
            if (task.InteractGO == null)
                return;

            var target = task.InteractGO;
            bool matches = interacted.gameObject == target || interacted.transform.IsChildOf(target.transform) || target.transform.IsChildOf(interacted.transform);
            if (!matches)
                return;

            // Capture nodeReturn, end task, then jump if provided
            var nodeReturn = _statesByCode.TryGetValue(_currentTaskCode, out var st) ? st.NodeReturn : string.Empty;
            var code = _currentTaskCode;
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
            return _statesByCode.TryGetValue(taskCode, out var st) ? st.Collected : 0;
        }

        public bool IsTaskCompleted(string taskCode)
        {
            return _statesByCode.TryGetValue(taskCode, out var st) && st.Completed;
        }

        public bool TryGetTask(string code, out QuestTask task)
        {
            return _tasksByCode.TryGetValue(code, out task);
        }
    }
}
