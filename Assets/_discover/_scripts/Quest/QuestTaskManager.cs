using Antura.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Discover
{
    public class QuestTaskManager : SingletonMonoBehaviour<QuestTaskManager>
    {
        private readonly Dictionary<string, QuestTask> _tasksByCode = new();
        private readonly HashSet<string> _activeTaskCodes = new();

        public void RegisterTasks(IEnumerable<QuestTask> tasks)
        {
            _tasksByCode.Clear();
            _activeTaskCodes.Clear();
            if (tasks == null)
                return;
            foreach (var t in tasks)
            {
                if (t == null || string.IsNullOrEmpty(t.Code))
                    continue;
                if (_tasksByCode.ContainsKey(t.Code))
                {
                    Debug.LogWarning($"TaskManager.RegisterTasks: duplicate task code '{t.Code}' — overriding.");
                }
                _tasksByCode[t.Code] = t;
            }
        }

        public bool StartTask(string taskCode)
        {
            if (string.IsNullOrEmpty(taskCode))
                return false;
            if (!_tasksByCode.TryGetValue(taskCode, out var task))
            {
                Debug.LogWarning($"TaskManager.StartTask: task not found for code '{taskCode}'.");
                return false;
            }
            _activeTaskCodes.Add(taskCode);
            // Delegate to current quest logic for activation/UI for now
            QuestManager.I?.TaskStart(taskCode);
            return true;
        }

        public bool EndTask(string taskCode, bool success = true)
        {
            if (string.IsNullOrEmpty(taskCode))
                return false;
            if (!_activeTaskCodes.Contains(taskCode))
            {
                // Allow end even if not tracked yet (defensive)
                Debug.LogWarning($"TaskManager.EndTask: task '{taskCode}' not active.");
            }
            if (success)
                QuestManager.I?.TaskSuccess(taskCode);
            else
                QuestManager.I?.TaskFail(taskCode);
            _activeTaskCodes.Remove(taskCode);
            return true;
        }

        public void OnCollectItem(string tag)
        {
            QuestManager.I?.OnCollectItem(tag);
        }

        public void OnReachTarget(string taskCode)
        {
            EndTask(taskCode, true);
        }

        public void OnInteract(string taskCode)
        {
            EndTask(taskCode, true);
        }

        public bool TryGetTask(string code, out QuestTask task)
        {
            return _tasksByCode.TryGetValue(code, out task);
        }
    }
}
