using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;
using Yarn.Unity;
using UnityEngine.Serialization;

namespace Antura.Discover
{
    // SubjectCount moved to QuestSubjectsUtility to keep this class lean.

    [CreateAssetMenu(fileName = "QuestData", menuName = "Antura/Discover/Quest Data")]
    public class QuestData : IdentifiedData
    {
        public YarnProject YarnProject;
        public TextAsset YarnScript;

        [Tooltip("Just for display in the UI. Not used in the game logic.")]
        public string IdDisplay;

        [Tooltip("For docs and website, 100 = 1.00")]
        public int Version = 100;

        [Tooltip("Development status of this quest. Needed for internal testing and validation.")]
        public Status Status;

        [Header("Description")]
        [Tooltip("English title, for reference only. Not used in the game logic.")]
        public string TitleEn;
        public LocalizedString Title;
        public LocalizedString Description;
        public Countries Country;
        public LocationData Location;
        public AssetData Thumbnail;

        [Header("Content")]
        [Tooltip("Top subjects aggregated from Topics (Core 2x + Connections 1x)")]
        public List<SubjectCount> Subjects;

        public Subject Subject;
        public List<TopicData> Topics;
        public List<CardData> Cards;
        public List<WordData> Words;

        [Tooltip("Target age range for this quest.")]
        public AgeRange targetAge = AgeRange.Ages6to10;

        public Difficulty Difficulty;
        [Tooltip("In minutes.. approximately how long it takes to complete the quest.")]
        public int Duration;

        [Tooltip("Does this quest require any other quest to be completed first?")]
        public List<QuestData> Dependencies;

        [Header("Gameplay")]
        public List<GameplayType> Gameplay;

        [Header("Rewards")]
        [Range(0, 20)]
        public int cookies = 0;

        [Header("Public website docs")]
        public bool IsPublic;
        public bool IsScriptPublic;
        [Tooltip("Forum URL for discussion and feedback")]
        public string ForumUrl;
        [Tooltip("The URL for the translations of this quest")]
        public string GoogleSheetUrl;

        [Tooltip("for the Website. THis is a Markdown file that contains additional resources for the quest, such as links to videos, articles, etc.")]
        public TextAsset TeacherResources;

        [Tooltip("Game Design Notes, published with the script")]
        public TextAsset AdditionalResources;

        public List<AuthorCredit> Credits;

        [Header("WorldSetup")]
        public GameObject QuestPrefab;
        public WorldController WorldControllerPrefab;

        [Tooltip("Optionan override for the world setup")]
        public WorldSetupData WorldSetup;

        [Header("Unity internal")]
        public string assetsFolder;
        public string scene;
        public LocalizedStringTable QuestStringsTable;

        public int GetBestStars()
        {
            try
            {
                var mgr = DiscoverAppManager.I;
                var prof = mgr != null ? mgr.CurrentProfile : null;
                var qs = prof != null ? prof.stats?.quests : null;
                if (qs != null && !string.IsNullOrEmpty(this.Id))
                {
                    if (qs.TryGetValue(this.Id, out var s))
                        return Mathf.Clamp(s.bestStars, 0, 3);
                }
            }
            catch { }
            return 0;
        }
        public int KnowledgeValue
        {
            get
            {
                int value = 0;
                foreach (var card in Cards)
                {
                    if (card != null)
                        value += card.Points;
                }
                return value;
            }
        }

        public string TitleText
        {
            get
            {
                try
                { return Title.GetLocalizedString(); }
                catch { return TitleEn; }
            }
        }

        public string DescriptionText
        {
            get
            {
                try
                { return Description.GetLocalizedString(); }
                catch { return string.Empty; }
            }
        }

        public string VersionText
        {
            get
            {
                // Format as X.YY where Version is stored as integer (e.g., 100 => 1.00)
                int v = Mathf.Max(0, Version);
                int major = v / 100;
                int minor = v % 100;
                return $"{major}.{minor:00}";
            }
        }

        public List<SubjectCount> GetSubjectsBreakdown() => QuestSubjectsUtility.ComputeSubjectsBreakdown(this);
        // Optional non-serialized convenience property (uses live computation)
        public string SubjectsSummaryText => QuestSubjectsUtility.BuildSummaryText(GetSubjectsBreakdown());

        public string SubjectsListText => QuestSubjectsUtility.BuildSummaryTextSimple(GetSubjectsBreakdown());

#if UNITY_EDITOR
        [ContextMenu("Refresh Top Subjects")]
        private void RefreshTopSubjects()
        {
            var list = GetSubjectsBreakdown();
            Subjects = list != null ? list.Take(4).ToList() : new List<SubjectCount>();
        }

        [ContextMenu("Log Subjects Breakdown")]
        private void LogSubjectsBreakdown()
        {
            var list = GetSubjectsBreakdown();
            var lines = list?.Select(sc => $"- {sc.Subject}: {sc.Count}") ?? Enumerable.Empty<string>();
            var body = lines.Any() ? string.Join("\n", lines) : "(none)";
            Debug.Log($"[QuestData] {name} subjects (desc):\n" + body, this);
        }
#endif

    }
}
