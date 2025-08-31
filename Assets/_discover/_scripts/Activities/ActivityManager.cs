using Antura.Utilities;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace Antura.Discover.Activities
{
    public class ActivityManager : SingletonMonoBehaviour<ActivityManager>
    {
        public ActivityListData ActivityList;

        public string _returnNode;
        private ActivityBase _currentActivity;
        private int _lastResultScore;

        public bool Launch(string configCode, string nodeReturn = "")
        {
            Debug.Log($"ActivityManager.Launch: {configCode} -> {nodeReturn}");
            _lastResultScore = 0;
            _returnNode = nodeReturn ?? string.Empty;

            // Find activity config from QuestManager list
            ActivityConfig activityConfig = null;
            if (QuestManager.I.ActivityConfigs != null)
            {
                foreach (var c in QuestManager.I.ActivityConfigs)
                {
                    if (c != null && string.Equals(c.Code, configCode, StringComparison.OrdinalIgnoreCase))
                    { activityConfig = c; break; }
                }
            }
            if (activityConfig == null || activityConfig.ActivityGO == null)
            {
                Debug.LogWarning($"ActivityManager.Launch: config or prefab not found for '{configCode}'");
                return false;
            }

            var activityBase = activityConfig.ActivityGO.GetComponentInChildren<ActivityBase>(true);
            if (activityBase == null)
            {
                Debug.LogError($"ActivityManager.Launch: ActivityBase missing on '{configCode}'");
                return false;
            }

            // Enter PlayActivity and hide world UI
            DiscoverGameManager.I?.ChangeState(GameplayState.PlayActivity, true);
            _currentActivity = activityBase;


            // Configure from data
            activityBase.ConfigureSettings(activityConfig.ActivitySettings);
            activityBase.OpenFresh();
            return true;
        }

        /// <summary>
        /// Saves result and optionally jumps to Yarn return node.
        /// </summary>
        public void OnActivityClosed(string activityId, int resultScore, int durationSec)
        {
            _lastResultScore = resultScore;

            try
            {
                DiscoverAppManager.I?.RecordActivityEnd(new ActivityEnd
                {
                    activityId = activityId, score = resultScore, durationSec = durationSec
                });
            }
            catch { }

            // If a return node is provided, enter Dialogue and start it; otherwise resume Play3D
            if (!string.IsNullOrEmpty(_returnNode))
            {
                DiscoverGameManager.I?.ChangeState(GameplayState.Dialogue, true);
                YarnAnturaManager.I?.StartDialogue(_returnNode);
            }
            else
            {
                DiscoverGameManager.I?.ChangeState(GameplayState.Play3D, true);
            }
            _returnNode = string.Empty;
            _currentActivity = null;
        }

        /// <summary>
        /// Return last activity result score for Yarn function access.
        /// </summary>
        public int GetResult(string code)
        {
            return _lastResultScore;
        }

    }
}
