using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Antura.Core;
using Antura.Utilities;

namespace Antura.Discover
{
    public class ProgressManager
    {
        private HashSet<string> visitedSteps;
        private int total_progress;
        private int current_progress;

        public ProgressManager()
        {
            visitedSteps = new HashSet<string>();
        }

        public void Init(QuestTask[] questTasks)
        {

            current_progress = 0;
            total_progress = 0;
            if (questTasks != null)
            {
                foreach (var task in questTasks)
                {
                    if (task != null)
                        total_progress += task.GetTaskTotalePoints();
                }
            }
            UpdateUI();
        }

        private void UpdateUI()
        {
            QuestManager.I.UpdateProgressScore(current_progress);
        }

        public void AddProgressPoints(int points, string stepName = "")
        {
            if (!string.IsNullOrEmpty(stepName) && visitedSteps.Contains(stepName))
            {
                //Debug.Log($"Step {stepName} already visited.");
                return;
            }

            if (stepName != "")
            {
                visitedSteps.Add(stepName);
            }

            current_progress += points;
            UpdateUI();
        }

        public int GetCurrentStarsAchieved()
        {
            if (total_progress == 0)
                return 0;
            float progressRatio = current_progress / total_progress;
            if (progressRatio >= 0.9f)
                return 3;
            else if (progressRatio >= 0.6f)
                return 2;
            else if (progressRatio >= 0.3f)
                return 1;
            else
                return 0;
        }
    }
}
