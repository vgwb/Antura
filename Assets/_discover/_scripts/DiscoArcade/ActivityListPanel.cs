using System;
using System.Collections.Generic;
using Antura.Discover.Activities;
using UnityEngine;

namespace Antura.Discover
{
    /// <summary>
    /// Handles the list of activities for the selected topic.
    /// </summary>
    public class ActivityListPanel : MonoBehaviour
    {
        [SerializeField] private RectTransform content;
        [SerializeField] private ActivityListItem activityItemPrefab;
        [SerializeField] private GameObject emptyState;

        private readonly List<ActivityListItem> _items = new();

        public event Action<ActivityData> PlayRequested;
        public event Action<ActivitySettingsAbstract> SettingsRequested;

        public void ShowActivities(TopicData topic,
            IReadOnlyList<ActivityData> activities,
            IReadOnlyList<ActivitySettingsAbstract> allSettings,
            Func<ActivityData, ActivitySettingsAbstract> runtimeFactory)
        {
            if (activities == null || activities.Count == 0)
            {
                ToggleEmptyState(true);
                ClearItems();
                return;
            }

            ToggleEmptyState(false);
            EnsureItemCount(activities.Count);

            for (int i = 0; i < _items.Count; i++)
            {
                if (i < activities.Count)
                {
                    var activity = activities[i];
                    var matchingSettings = FilterSettings(activity, topic, allSettings);
                    _items[i].gameObject.SetActive(true);
                    _items[i].Bind(activity, topic, matchingSettings, runtimeFactory, HandlePlayRequested, HandleSettingsRequested);
                }
                else
                {
                    _items[i].gameObject.SetActive(false);
                }
            }
        }

        private List<ActivitySettingsAbstract> FilterSettings(ActivityData activity, TopicData topic, IReadOnlyList<ActivitySettingsAbstract> allSettings)
        {
            if (allSettings == null)
                return new List<ActivitySettingsAbstract>();

            var result = new List<ActivitySettingsAbstract>();
            for (int i = 0; i < allSettings.Count; i++)
            {
                var settings = allSettings[i];
                if (settings == null)
                    continue;

                if (settings.ActivityCode != activity.Code)
                    continue;

                if (topic != null && settings.MainTopic != topic)
                    continue;

                result.Add(settings);
            }

            return result;
        }

        private void HandlePlayRequested(ActivityData activity)
        {
            PlayRequested?.Invoke(activity);
        }

        private void HandleSettingsRequested(ActivitySettingsAbstract settings)
        {
            SettingsRequested?.Invoke(settings);
        }

        private void EnsureItemCount(int desired)
        {
            if (activityItemPrefab == null || content == null)
            {
                Debug.LogWarning("ActivityListPanel: missing prefab or content.");
                return;
            }

            while (_items.Count < desired)
            {
                var item = Instantiate(activityItemPrefab, content);
                _items.Add(item);
            }
        }

        private void ClearItems()
        {
            for (int i = 0; i < _items.Count; i++)
            {
                if (_items[i] != null)
                    _items[i].gameObject.SetActive(false);
            }
        }

        private void ToggleEmptyState(bool isEmpty)
        {
            if (emptyState != null)
            {
                emptyState.SetActive(isEmpty);
            }
        }
    }
}
