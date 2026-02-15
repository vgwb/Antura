using Antura.UI;
using Antura.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
namespace Antura.Discover.Activities
{
    public class ActivityManager : SingletonMonoBehaviour<ActivityManager>
    {
        public ActivityListData ActivityList;

        [Tooltip("Optional parent for instantiated activity prefabs. If null, spawned under this manager.")]
        public Transform ActivitiesParent;

        public string returnNode { get; private set; }
        private ActivityBase currentActivity;
        private string currentSettingsCode;
        private int lastResultScore;
        private GameObject spawnedInstanceGO;   // if we instantiate, keep a handle to destroy on close
        private bool ownsCurrentInstance;       // true when _currentActivity belongs to _spawnedInstanceGO
        private readonly Dictionary<string, int> lastResultsBySettings = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        public bool Launch(string settingsCode, string nodeReturn = "", string difficulty = "")
        {
            // Debug.Log($"ActivityManager.Launch: {settingsCode} -> {nodeReturn}");
            lastResultScore = 0;
            returnNode = nodeReturn ?? string.Empty;
            currentSettingsCode = settingsCode;

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

            if (difficulty != "")
            {
                var overrideDifficulty = Difficulty.Default;
                if (string.Equals(difficulty, "tutorial", StringComparison.OrdinalIgnoreCase))
                    overrideDifficulty = Difficulty.Tutorial;
                if (string.Equals(difficulty, "easy", StringComparison.OrdinalIgnoreCase))
                    overrideDifficulty = Difficulty.Easy;
                else if (string.Equals(difficulty, "normal", StringComparison.OrdinalIgnoreCase))
                    overrideDifficulty = Difficulty.Normal;
                else if (string.Equals(difficulty, "expert", StringComparison.OrdinalIgnoreCase))
                    overrideDifficulty = Difficulty.Expert;

                activityConfig.ActivitySettings.Difficulty = overrideDifficulty;
            }

            return Launch(activityConfig.ActivitySettings, nodeReturn);
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
            lastResultScore = 0;
            returnNode = nodeReturn ?? string.Empty;
            currentSettingsCode = settings != null ? settings.Id : string.Empty;

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
            currentActivity = activityBase;
            ownsCurrentInstance = true;
            spawnedInstanceGO = go;

            // Configure from data
            activityBase.ConfigureSettings(settings);
            activityBase.ActivityData = actData;
            activityBase.ActivityCode = actData.Code.ToString();
            ApplyActivityLabels(activityBase, settings, actData);
            GlobalUI.ShowPauseMenu(false);
            activityBase.OpenFresh();
            return true;
        }

        /// <summary>
        /// Saves result and optionally jumps to Yarn return node.
        /// </summary>
        public void OnActivityClosed(string activitySettingsCode, int resultScore, int durationSec)
        {
            lastResultScore = resultScore;

            if (!string.IsNullOrEmpty(currentSettingsCode))
            {
                lastResultsBySettings[currentSettingsCode] = resultScore;
            }
            else if (!string.IsNullOrEmpty(activitySettingsCode))
            {
                lastResultsBySettings[activitySettingsCode] = resultScore;
                currentSettingsCode = activitySettingsCode;
            }

            try
            {
                DiscoverAppManager.I?.RecordActivityEnd(new ActivityEnd
                {
                    activityId = !string.IsNullOrEmpty(currentSettingsCode) ? currentSettingsCode : activitySettingsCode,
                    score = resultScore,
                    durationSec = durationSec
                });
            }
            catch { }

            // If a return node is provided, enter Dialogue and start it; otherwise resume Play3D
            if (!string.IsNullOrEmpty(returnNode))
            {
                DiscoverGameManager.I?.ChangeState(GameplayState.Dialogue, true);
                YarnAnturaManager.I?.StartDialogue(returnNode);
            }
            else
            {
                DiscoverGameManager.I?.ChangeState(GameplayState.Play3D, true);
            }
            returnNode = string.Empty;
            // If we spawned an instance for this session, destroy it now
            if (ownsCurrentInstance && spawnedInstanceGO != null)
            {
                try
                { Destroy(spawnedInstanceGO); }
                catch { }
            }
            spawnedInstanceGO = null;
            ownsCurrentInstance = false;
            currentActivity = null;
            currentSettingsCode = string.Empty;
            GlobalUI.ShowPauseMenu(true);
        }

        /// <summary>
        /// Return last activity result score for Yarn function access.
        /// </summary>
        public int GetResult(string activitySettingsCode)
        {
            if (string.IsNullOrEmpty(activitySettingsCode))
                return lastResultScore;

            if (lastResultsBySettings.TryGetValue(activitySettingsCode, out var value))
            {
                return value;
            }

            if (string.Equals(activitySettingsCode, currentSettingsCode, StringComparison.OrdinalIgnoreCase))
            {
                return lastResultScore;
            }

            return 0;
        }

        /// <summary>
        /// Cleanup any previous owned instance before launching a new one.
        /// </summary>
        private void PrepareNewLaunch()
        {
            if (currentActivity != null)
            {
                // If we own it, destroy. Otherwise, just hide to avoid duplicates.
                if (ownsCurrentInstance && spawnedInstanceGO != null)
                {
                    try
                    { Destroy(spawnedInstanceGO); }
                    catch { }
                }
                else
                {
                    try
                    { currentActivity.HidePanel(); }
                    catch { }
                }
            }
            spawnedInstanceGO = null;
            ownsCurrentInstance = false;
            currentActivity = null;
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

        private void ApplyActivityLabels(ActivityBase activityBase, ActivitySettingsAbstract settings, ActivityData activityData)
        {
            if (activityBase == null)
                return;

            string activityName = GetLocalizedOrFallback(activityData?.Name, activityData != null ? activityData.name : activityBase.name);
            if (string.IsNullOrEmpty(activityName))
            {
                activityName = settings != null ? settings.name : string.Empty;
            }

            string topicLabel = BuildTopicLabel(settings);

            if (string.IsNullOrEmpty(activityName) && string.IsNullOrEmpty(topicLabel))
                return;

            activityBase.SetActivityLabels(activityName, topicLabel);
        }

        private static string GetLocalizedOrFallback(LocalizedString localized, string fallback)
        {
            if (localized != null && !localized.IsEmpty)
            {
                try
                {
                    var value = localized.GetLocalizedString();
                    if (!string.IsNullOrEmpty(value))
                        return value;
                }
                catch { }
            }

            return fallback ?? string.Empty;
        }

        private static string BuildTopicLabel(ActivitySettingsAbstract settings)
        {
            if (settings?.MainTopic == null)
                return string.Empty;

            var topicName = settings.MainTopic.Name ?? string.Empty;
            var coreCard = settings.MainTopic.CoreCard;
            string cardTitle = string.Empty;

            if (coreCard != null)
            {
                cardTitle = GetLocalizedOrFallback(coreCard.Title, !string.IsNullOrEmpty(coreCard.TitleEn) ? coreCard.TitleEn : coreCard.name);
            }

            if (string.IsNullOrEmpty(topicName))
                return cardTitle ?? string.Empty;

            if (string.IsNullOrEmpty(cardTitle))
                return topicName;

            return $"{topicName} · {cardTitle}";
        }

    }
}
