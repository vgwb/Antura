using System;
using System.Collections.Generic;
using Antura.Discover.Activities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.Discover
{
    /// <summary>
    /// Displays a single activity row with quick play and preset buttons.
    /// </summary>
    public class ActivityListItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text activityNameLabel;
        [SerializeField] private TMP_Text descriptionLabel;
        [SerializeField] private Button quickPlayButton;
        [SerializeField] private RectTransform settingsContent;
        [SerializeField] private ActivitySettingsItem settingsItemPrefab;
        [SerializeField] private GameObject presetsHeader;

        private readonly List<ActivitySettingsItem> _settingItems = new();
        private ActivityData _activity;
        private TopicData _topic;
        private Func<ActivityData, ActivitySettingsAbstract> _runtimeFactory;
        private Action<ActivityData> _onPlay;
        private Action<ActivitySettingsAbstract> _onSettings;

        public void Bind(ActivityData activity,
            TopicData topic,
            IReadOnlyList<ActivitySettingsAbstract> presets,
            Func<ActivityData, ActivitySettingsAbstract> runtimeFactory,
            Action<ActivityData> onPlay,
            Action<ActivitySettingsAbstract> onSettings)
        {
            _activity = activity;
            _topic = topic;
            _runtimeFactory = runtimeFactory;
            _onPlay = onPlay;
            _onSettings = onSettings;

            RefreshLabels();
            SetupQuickPlay();
            PopulatePresets(presets);
        }

        private void RefreshLabels()
        {
            if (activityNameLabel != null)
            {
                activityNameLabel.text = GetActivityName(_activity);
            }

            if (descriptionLabel != null)
            {
                descriptionLabel.text = _topic != null ? _topic.Name : string.Empty;
            }
        }

        private string GetActivityName(ActivityData activity)
        {
            if (activity == null)
                return string.Empty;

            if (activity.Name != null && !activity.Name.IsEmpty)
            {
                try
                {
                    var localized = activity.Name.GetLocalizedString();
                    if (!string.IsNullOrEmpty(localized))
                        return localized;
                }
                catch { }
            }
            return activity.name;
        }

        private void SetupQuickPlay()
        {
            if (quickPlayButton == null)
                return;

            quickPlayButton.onClick.RemoveAllListeners();
            quickPlayButton.interactable = _activity != null && _runtimeFactory != null;
            if (quickPlayButton.interactable)
            {
                quickPlayButton.onClick.AddListener(HandleQuickPlay);
            }
        }

        private void HandleQuickPlay()
        {
            if (_activity == null || _runtimeFactory == null)
                return;

            _onPlay?.Invoke(_activity);
        }

        private void PopulatePresets(IReadOnlyList<ActivitySettingsAbstract> presets)
        {
            bool hasPresets = presets != null && presets.Count > 0;

            if (presetsHeader != null)
                presetsHeader.SetActive(hasPresets);

            if (!hasPresets)
            {
                HideAllSettingItems();
                return;
            }

            EnsureSettingsItemCount(presets.Count);

            for (int i = 0; i < _settingItems.Count; i++)
            {
                if (i < presets.Count)
                {
                    var settings = presets[i];
                    _settingItems[i].gameObject.SetActive(true);
                    _settingItems[i].Bind(settings, HandleSettingsClicked);
                }
                else
                {
                    _settingItems[i].gameObject.SetActive(false);
                }
            }
        }

        private void EnsureSettingsItemCount(int desired)
        {
            if (settingsItemPrefab == null || settingsContent == null)
            {
                Debug.LogWarning("ActivityListItem: missing settings prefab/content.");
                return;
            }

            while (_settingItems.Count < desired)
            {
                var item = Instantiate(settingsItemPrefab, settingsContent);
                _settingItems.Add(item);
            }
        }

        private void HideAllSettingItems()
        {
            for (int i = 0; i < _settingItems.Count; i++)
            {
                if (_settingItems[i] != null)
                    _settingItems[i].gameObject.SetActive(false);
            }
        }

        private void HandleSettingsClicked(ActivitySettingsAbstract settings)
        {
            _onSettings?.Invoke(settings);
        }
    }
}
