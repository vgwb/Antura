using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Antura.Core;
using Antura.Utilities;

namespace Antura.Minigames.DiscoverCountry
{
    public class Progress
    {
        private HashSet<string> visitedNodes;
        private int total_progress;
        private int current_progress;

        public Progress()
        {
            visitedNodes = new HashSet<string>();
        }

        public void Init(int maxNodes)
        {
            total_progress = maxNodes;
            current_progress = 0;
        }

        public bool VisitNode(string nodePermalink)
        {

            if (visitedNodes.Contains(nodePermalink))
            {
                //Debug.Log($"Node {nodePermalink} already collected.");
                return false;
            }
            else
            {
                visitedNodes.Add(nodePermalink);
                current_progress++;
                Debug.Log("VISITING NODE " + nodePermalink + " - " + current_progress + " / " + total_progress);
                QuestManager.I.UpateProgressCounter(current_progress, total_progress);
                //Debug.Log($"Node {nodePermalink} collected successfully.");
                return true;
            }
        }
    }
}
