using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Antura.Core;
using Antura.Utilities;

namespace Antura.Minigames.DiscoverCountry
{
    public class Progress
    {
        private HashSet<string> visitedSteps;
        private int total_steps;
        private int current_steps;

        public Progress()
        {
            visitedSteps = new HashSet<string>();
        }

        public void Init(int maxSteps)
        {
            total_steps = maxSteps;
            current_steps = 0;
        }

        public bool VisitNode(string stepName)
        {

            if (visitedSteps.Contains(stepName))
            {
                //Debug.Log($"Node {nodePermalink} already collected.");
                return false;
            }
            else
            {
                visitedSteps.Add(stepName);
                current_steps++;
                //                Debug.Log("VISITING NODE " + stepName + " - " + current_steps + " / " + total_steps);
                QuestManager.I.UpateProgressCounter(current_steps, total_steps);
                //Debug.Log($"Node {nodePermalink} collected successfully.");
                return true;
            }
        }
    }
}
