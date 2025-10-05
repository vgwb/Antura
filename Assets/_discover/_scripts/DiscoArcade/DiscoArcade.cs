using Antura.Discover.Activities;
using Antura.Discover.UI;
using Antura.UI;
using Antura.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.Discover
{
    /// <summary>
    /// DiscoArcade show topics and let you play withthem
    /// </summary>
    public class DiscoArcade : SingletonMonoBehaviour<DiscoArcade>
    {
        [Header("Columns")]
        [SerializeField] private DiscoArcadeFilterPanel filterPanel;
        [SerializeField] private TopicListPanel topicListView;
        [SerializeField] private TopicDetailsPanel topicDetailsPanel;
        [SerializeField] private ActivityListPanel activityListPanel;

        [Header("Shared UI")]
        [SerializeField] private Button btnClose;
        [SerializeField] private CardDetailsPanel cardDetailsPanel;

        private readonly List<TopicData> filteredTopics = new();
        private List<TopicData> allTopics = new();
        private List<ActivityData> allActivities = new();
        private List<ActivitySettingsAbstract> allActivitySettings = new();
        private readonly Dictionary<ActivityCode, ActivitySettingsAbstract> activityTemplates = new();

        private TopicData currentTopic;
        private bool initialized;

        protected override void Init()
        {
            if (btnClose != null)
            {
                btnClose.onClick.AddListener(CloseDiscoArcade);
            }

            if (topicListView != null)
            {
                topicListView.SelectionRequested += HandleTopicSelectedFromList;
            }

            if (topicDetailsPanel != null)
            {
                topicDetailsPanel.CardSelected += HandleTopicCardSelected;
            }

            if (activityListPanel != null)
            {
                activityListPanel.PlayRequested += HandlePlayActivity;
                activityListPanel.SettingsRequested += HandleLaunchSettings;
            }

            if (filterPanel != null)
            {
                filterPanel.FiltersChanged += HandleFiltersChanged;
            }
        }

        private void OnDestroy()
        {
            if (btnClose != null)
            {
                btnClose.onClick.RemoveListener(CloseDiscoArcade);
            }

            if (topicListView != null)
            {
                topicListView.SelectionRequested -= HandleTopicSelectedFromList;
            }

            if (topicDetailsPanel != null)
            {
                topicDetailsPanel.CardSelected -= HandleTopicCardSelected;
            }

            if (activityListPanel != null)
            {
                activityListPanel.PlayRequested -= HandlePlayActivity;
                activityListPanel.SettingsRequested -= HandleLaunchSettings;
            }

            if (filterPanel != null)
            {
                filterPanel.FiltersChanged -= HandleFiltersChanged;
            }
        }

        public void Open()
        {
            EnsureInitialized();
            GlobalUI.ShowPauseMenu(false);
            gameObject.SetActive(true);
            ApplyTopicFilter();
            filterPanel?.FocusSearchField();
        }

        private void EnsureInitialized()
        {
            if (initialized)
                return;

            initialized = true;

            LoadDatabaseSnapshot();

            filterPanel?.Initialize(allTopics);

            ApplyTopicFilter();
        }

        private void LoadDatabaseSnapshot()
        {
            var data = DiscoverDataManager.I;
            if (data == null)
            {
                Debug.LogWarning("DiscoArcade: DiscoverDataManager not available yet.");
                allTopics = new List<TopicData>();
                allActivities = new List<ActivityData>();
                allActivitySettings = new List<ActivitySettingsAbstract>();
                return;
            }

            allTopics = data.Database.All<TopicData>().Where(t => t != null).OrderBy(t => t.Name).ToList();
            allActivities = data.Database.All<ActivityData>().Where(a => a != null).OrderBy(GetActivityDisplayName).ToList();
            allActivitySettings = data.Database.All<ActivitySettingsAbstract>().Where(s => s != null).ToList();

            activityTemplates.Clear();
            foreach (var settings in allActivitySettings)
            {
                if (settings == null)
                    continue;

                if (!activityTemplates.ContainsKey(settings.ActivityCode))
                {
                    activityTemplates[settings.ActivityCode] = settings;
                }
            }
        }

        private string GetActivityDisplayName(ActivityData activity)
        {
            if (activity == null)
                return string.Empty;

            if (activity.Name != null && !activity.Name.IsEmpty)
            {
                try
                {
                    var value = activity.Name.GetLocalizedString();
                    if (!string.IsNullOrEmpty(value))
                        return value;
                }
                catch { }
            }

            return activity.name;
        }

        private void CloseDiscoArcade()
        {
            GlobalUI.ShowPauseMenu(true);
            gameObject.SetActive(false);
        }

        private void ApplyTopicFilter()
        {
            filteredTopics.Clear();

            if (allTopics == null || allTopics.Count == 0)
            {
                UpdateTopicList();
                return;
            }

            var filterState = filterPanel != null ? filterPanel.CurrentState : default;

            foreach (var topic in allTopics)
            {
                if (topic == null)
                    continue;

                if (TopicMatchesFilters(topic, filterState))
                {
                    filteredTopics.Add(topic);
                }
            }

            if (filteredTopics.Count == 0 && allTopics.Count > 0)
            {
                filteredTopics.AddRange(allTopics);
            }

            UpdateTopicList();
            AutoSelectTopic();
        }

        private void UpdateTopicList()
        {
            if (topicListView == null)
                return;

            topicListView.SetTopics(filteredTopics, currentTopic);
        }

        private bool TopicMatchesFilters(TopicData topic, DiscoArcadeFilterPanel.FilterState filters)
        {
            if (!MatchesCountry(topic, filters.SelectedCountries))
                return false;

            if (!MatchesSubject(topic, filters.SelectedSubject))
                return false;

            if (!MatchesSearch(topic, filters.SearchText))
                return false;

            return true;
        }

        private static bool MatchesCountry(TopicData topic, IReadOnlyList<Countries> countries)
        {
            if (countries == null || countries.Count == 0)
                return true;

            for (int i = 0; i < countries.Count; i++)
            {
                if (countries[i] == topic.Country)
                    return true;
            }

            return false;
        }

        private static bool MatchesSubject(TopicData topic, Subject? subject)
        {
            if (!subject.HasValue)
                return true;

            if (topic.Subjects != null && topic.Subjects.Contains(subject.Value))
                return true;

            var cards = topic.GetAllCards();
            foreach (var card in cards)
            {
                if (card?.Subjects != null && card.Subjects.Contains(subject.Value))
                    return true;
            }

            return false;
        }

        private static bool MatchesSearch(TopicData topic, string search)
        {
            if (string.IsNullOrWhiteSpace(search))
                return true;

            var query = search.Trim();

            if (MatchesString(topic.Name, query))
                return true;

            if (MatchesString(topic.Description, query))
                return true;

            var cards = topic.GetAllCards();
            foreach (var card in cards)
            {
                if (card == null)
                    continue;

                if (MatchesString(card.TitleEn, query))
                    return true;

                if (MatchesString(card.DescriptionEn, query))
                    return true;

                if (card.Title != null)
                {
                    try
                    {
                        var localized = card.Title.GetLocalizedString();
                        if (MatchesString(localized, query))
                            return true;
                    }
                    catch { }
                }

                if (card.Description != null)
                {
                    try
                    {
                        var localized = card.Description.GetLocalizedString();
                        if (MatchesString(localized, query))
                            return true;
                    }
                    catch { }
                }
            }

            return false;
        }

        private static bool MatchesString(string value, string query)
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(query))
                return false;

            return value.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private void HandleFiltersChanged(DiscoArcadeFilterPanel.FilterState state)
        {
            ApplyTopicFilter();
        }

        private void AutoSelectTopic()
        {
            if (filteredTopics.Count == 0)
            {
                SelectTopic(null);
                return;
            }

            if (currentTopic != null && filteredTopics.Contains(currentTopic))
            {
                SelectTopic(currentTopic);
                return;
            }

            SelectTopic(filteredTopics[0]);
        }

        private void HandleTopicSelectedFromList(TopicData topic)
        {
            SelectTopic(topic);
        }

        private void SelectTopic(TopicData topic)
        {
            currentTopic = topic;

            if (topicDetailsPanel != null)
            {
                topicDetailsPanel.Show(topic);
            }

            if (activityListPanel != null)
            {
                var factory = topic != null ? new Func<ActivityData, ActivitySettingsAbstract>(CreateRuntimeSettingsForActivity) : null;
                activityListPanel.ShowActivities(topic, allActivities, allActivitySettings, factory);
            }

            if (topicListView != null)
            {
                topicListView.Highlight(topic);
            }
        }

        private ActivitySettingsAbstract CreateRuntimeSettingsForActivity(ActivityData activity)
        {
            if (activity == null || currentTopic == null)
                return null;

            ActivitySettingsAbstract prototype = null;
            if (!activityTemplates.TryGetValue(activity.Code, out prototype))
            {
                prototype = allActivitySettings.FirstOrDefault(s => s.ActivityCode == activity.Code);
            }

            ActivitySettingsAbstract instance;
            if (prototype != null)
            {
                instance = Instantiate(prototype);
            }
            else
            {
                instance = ScriptableObject.CreateInstance<RuntimeActivitySettings>();
                ((RuntimeActivitySettings)instance).InitializeDefaults(activity.Code);
            }

            instance.hideFlags = HideFlags.HideAndDontSave;
            instance.MainTopic = currentTopic;
            instance.SelectionMode = SelectionMode.RandomFromTopic;
            instance.ActivityCode = activity.Code;
            instance.Id = IdentifiedData.PrefixOnce("runtime", IdentifiedData.SanitizeId($"{activity.Code}_{currentTopic.Id}"));
            instance.name = instance.Id;
            return instance;
        }

        private void HandleTopicCardSelected(CardData card)
        {
            if (card == null || cardDetailsPanel == null)
                return;

            CardState state = null;
            var manager = DiscoverAppManager.I;
            if (manager != null && manager.CurrentProfile != null && manager.CurrentProfile.cards != null)
            {
                manager.CurrentProfile.cards.TryGetValue(card.Id, out state);
            }

            cardDetailsPanel.Show(card, state);
        }

        private void HandlePlayActivity(ActivityData activity)
        {
            var settings = CreateRuntimeSettingsForActivity(activity);
            if (settings == null)
            {
                Debug.LogWarning("DiscoArcade: could not create runtime settings for activity.");
                return;
            }

            ActivityManager.I?.Launch(settings, string.Empty);
        }

        private void HandleLaunchSettings(ActivitySettingsAbstract settings)
        {
            if (settings == null)
                return;

            var clone = Instantiate(settings);
            clone.hideFlags = HideFlags.HideAndDontSave;
            clone.MainTopic = currentTopic ?? settings.MainTopic;
            clone.ActivityCode = settings.ActivityCode;
            if (!string.IsNullOrEmpty(settings.Id))
            {
                clone.Id = settings.Id;
            }

            ActivityManager.I?.Launch(clone, string.Empty);
        }
    }
}
