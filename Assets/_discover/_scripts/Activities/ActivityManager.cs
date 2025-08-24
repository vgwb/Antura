using Antura.Utilities;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace Antura.Discover.Activities
{
    public class ActivityManager : SingletonMonoBehaviour<ActivityManager>
    {
        public ActivityListData ActivityList;

        public string _pendingReturnNode;
        private ActivityBase _currentActivity;
        private int _lastResultScore;

        /// <summary>
        /// Launch an activity by string code. Looks up the ActivityConfig from QuestManager.
        /// settings are taken from the ActivityConfig.ActivityData only (no prefab overrides).
        /// When the activity finishes, if nodeReturn is provided, it triggers Yarn with the result stored.
        /// </summary>
        public bool Launch(string code, string nodeReturn)
        {
            _lastResultScore = 0;
            _pendingReturnNode = nodeReturn ?? string.Empty;

            var qm = Antura.Discover.QuestManager.I;
            if (qm == null || string.IsNullOrEmpty(code))
            {
                Debug.LogWarning($"ActivityManager.Launch: missing QuestManager or code");
                return false;
            }

            // Find activity config from QuestManager list
            ActivityConfig cfg = null;
            if (qm.ActivityConfigs != null)
            {
                foreach (var c in qm.ActivityConfigs)
                {
                    if (c != null && string.Equals(c.Code, code, StringComparison.OrdinalIgnoreCase))
                    { cfg = c; break; }
                }
            }
            if (cfg == null || cfg.ActivityGO == null)
            {
                Debug.LogWarning($"ActivityManager.Launch: config or prefab not found for '{code}'");
                return false;
            }

            var act = cfg.ActivityGO.GetComponentInChildren<ActivityBase>(true);
            if (act == null)
            {
                Debug.LogError($"ActivityManager.Launch: ActivityBase missing on '{code}'");
                return false;
            }

            // Configure from data (only source of truth)
            act.ConfigureSettings(cfg.ActivityData);
            _currentActivity = act;

            // Ensure activity started fresh (self-manages overlay/panel)
            act.OpenFresh();
            return true;
        }

        /// <summary>
        /// To be called by ActivityBase when it truly finishes and wants to close.
        /// Saves result and optionally jumps to Yarn return node.
        /// </summary>
        public void OnActivityClosed(string activityId, int resultScore, int durationSec)
        {
            _lastResultScore = resultScore;

            // Persist activity run stats
            try
            {
                DiscoverAppManager.I?.RecordQuestEnd(new Antura.Discover.QuestEnd
                {
                    questId = QuestManager.I.CurrentQuest != null ? QuestManager.I.CurrentQuest.Id : string.Empty,
                    score = 0,
                    durationSec = 0,
                    stars = 0,
                    activities = new List<ActivityEnd>
                    {
                        new ActivityEnd { activityId = activityId, score = resultScore, durationSec = durationSec }
                    }
                });
            }
            catch { }

            // Jump back to Yarn if requested
            if (!string.IsNullOrEmpty(_pendingReturnNode))
            {
                YarnAnturaManager.I?.StartDialogue(_pendingReturnNode);
            }

            _pendingReturnNode = string.Empty;
            _currentActivity = null;
        }

        /// <summary>
        /// Return last activity result score for Yarn function access.
        /// </summary>
        public int TryGetResult(string code)
        {
            return _lastResultScore;
        }

    }
}
