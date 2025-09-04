using Antura.Utilities;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace Antura.Discover.Activities
{
    public class ActivityManager : SingletonMonoBehaviour<ActivityManager>
    {
        public ActivityListData ActivityList;

        [Tooltip("Optional parent for instantiated activity prefabs. If null, spawned under this manager.")]
        public Transform ActivitiesParent;

        public string _returnNode;
        private ActivityBase _currentActivity;
        private int _lastResultScore;
        private GameObject _spawnedInstanceGO;   // if we instantiate, keep a handle to destroy on close
        private bool _ownsCurrentInstance;       // true when _currentActivity belongs to _spawnedInstanceGO

        public bool Launch(string settingsCode, string nodeReturn = "")
        {
            Debug.Log($"ActivityManager.Launch: {settingsCode} -> {nodeReturn}");
            _lastResultScore = 0;
            _returnNode = nodeReturn ?? string.Empty;

            // Find activity config from QuestManager list
            ActivityConfig activityConfig = null;
            if (QuestManager.I.ActivityConfigs != null)
            {
                foreach (var c in QuestManager.I.ActivityConfigs)
                {
                    if (c != null && string.Equals(c.ActivitySettings.Id, settingsCode, StringComparison.OrdinalIgnoreCase))
                    { activityConfig = c; break; }
                }
            }
            if (activityConfig == null)
            {
                Debug.LogWarning($"ActivityManager.Launch: config not found for '{settingsCode}'");
                return false;
            }

            // Preferred path: if no prefab reference on config, try instantiating by settings
            if (activityConfig.ActivityGO == null)
            {
                return Launch(activityConfig.ActivitySettings, nodeReturn);
            }

            // Legacy path: use existing scene object reference
            var activityBase = activityConfig.ActivityGO.GetComponentInChildren<ActivityBase>(true);
            if (activityBase == null)
            {
                Debug.LogError($"ActivityManager.Launch: ActivityBase missing on '{settingsCode}'");
                return false;
            }

            PrepareNewLaunch();
            DiscoverGameManager.I?.ChangeState(GameplayState.PlayActivity, true);
            _currentActivity = activityBase;
            _ownsCurrentInstance = false;
            _spawnedInstanceGO = null;

            // Configure from data
            activityBase.ConfigureSettings(activityConfig.ActivitySettings);
            // Try populate metadata for tracking
            TryPopulateActivityMeta(activityBase, activityConfig.ActivitySettings);
            activityBase.OpenFresh();
            return true;
        }

        /// <summary>
        /// Launch an activity by just passing its settings. The prefab is resolved from ActivityList using ActivityCode.
        /// </summary>
        public bool Launch(ActivitySettingsAbstract settings, string nodeReturn = "")
        {
            if (settings == null)
            {
                Debug.LogWarning("ActivityManager.Launch(settings): settings is null");
                return false;
            }
            _lastResultScore = 0;
            _returnNode = nodeReturn ?? string.Empty;

            // Resolve ActivityData by code
            if (ActivityList == null || ActivityList.Activities == null)
            {
                Debug.LogWarning("ActivityManager.Launch(settings): ActivityList not assigned or empty");
                return false;
            }

            ActivityData actData = null;
            foreach (var a in ActivityList.Activities)
            {
                if (a != null && a.Code == settings.ActivityCode)
                { actData = a; break; }
            }
            if (actData == null)
            {
                Debug.LogWarning($"ActivityManager.Launch(settings): no ActivityData found for code '{settings.ActivityCode}'");
                return false;
            }
            if (actData.ActivityPrefab == null)
            {
                Debug.LogWarning($"ActivityManager.Launch(settings): ActivityPrefab missing on ActivityData '{actData.name}'");
                return false;
            }

            // Clean up any previous owned instance
            PrepareNewLaunch();

            // Instantiate prefab
            var parent = ActivitiesParent != null ? ActivitiesParent : this.transform;
            var go = Instantiate(actData.ActivityPrefab, parent);
            go.name = actData.ActivityPrefab.name;
            var activityBase = go.GetComponentInChildren<ActivityBase>(true);
            if (activityBase == null)
            {
                Debug.LogError($"ActivityManager.Launch(settings): ActivityBase missing in prefab '{actData.ActivityPrefab.name}'");
                Destroy(go);
                return false;
            }

            // Enter PlayActivity and hide world UI
            DiscoverGameManager.I?.ChangeState(GameplayState.PlayActivity, true);
            _currentActivity = activityBase;
            _ownsCurrentInstance = true;
            _spawnedInstanceGO = go;

            // Configure from data
            activityBase.ConfigureSettings(settings);
            // Populate meta
            activityBase.ActivityData = actData;
            try
            { activityBase.ActivityCode = actData.Code.ToString(); }
            catch { }
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
            // If we spawned an instance for this session, destroy it now
            if (_ownsCurrentInstance && _spawnedInstanceGO != null)
            {
                try
                { Destroy(_spawnedInstanceGO); }
                catch { }
            }
            _spawnedInstanceGO = null;
            _ownsCurrentInstance = false;
            _currentActivity = null;
        }

        /// <summary>
        /// Return last activity result score for Yarn function access.
        /// </summary>
        public int GetResult(string code)
        {
            return _lastResultScore;
        }

        /// <summary>
        /// Cleanup any previous owned instance before launching a new one.
        /// </summary>
        private void PrepareNewLaunch()
        {
            if (_currentActivity != null)
            {
                // If we own it, destroy. Otherwise, just hide to avoid duplicates.
                if (_ownsCurrentInstance && _spawnedInstanceGO != null)
                {
                    try
                    { Destroy(_spawnedInstanceGO); }
                    catch { }
                }
                else
                {
                    try
                    { _currentActivity.HidePanel(); }
                    catch { }
                }
            }
            _spawnedInstanceGO = null;
            _ownsCurrentInstance = false;
            _currentActivity = null;
        }

        private void TryPopulateActivityMeta(ActivityBase activityBase, ActivitySettingsAbstract settings)
        {
            if (activityBase == null || settings == null || ActivityList == null || ActivityList.Activities == null)
                return;
            try
            {
                foreach (var a in ActivityList.Activities)
                {
                    if (a != null && a.Code == settings.ActivityCode)
                    {
                        activityBase.ActivityData = a;
                        try
                        { activityBase.ActivityCode = a.Code.ToString(); }
                        catch { }
                        break;
                    }
                }
            }
            catch { }
        }

    }
}
