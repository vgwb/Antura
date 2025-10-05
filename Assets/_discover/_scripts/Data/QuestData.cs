using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;
using Yarn.Unity;
using UnityEngine.Serialization;
using UnityEngine.ResourceManagement.AsyncOperations;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Antura.Discover
{
    [CreateAssetMenu(fileName = "QuestData", menuName = "Antura/Discover Data/Quest Data")]
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

        public AssetReferenceGameObject QuestPrefabReference => questPrefab;
        public bool HasQuestPrefabReference => questPrefab != null && questPrefab.RuntimeKeyIsValid();

        public GameObject GetQuestPrefab(bool forceReload = false)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                return GetQuestPrefabEditorAsset();
#endif

            if (!forceReload && cachedQuestPrefab != null)
                return cachedQuestPrefab;

            if (!forceReload && legacyQuestPrefab != null)
            {
                cachedQuestPrefab = legacyQuestPrefab;
                return cachedQuestPrefab;
            }

            if (!HasQuestPrefabReference)
                return null;

            if (forceReload || !questPrefabHandle.IsValid())
                questPrefabHandle = questPrefab.LoadAssetAsync<GameObject>();

            cachedQuestPrefab = questPrefabHandle.WaitForCompletion();
            return cachedQuestPrefab;
        }

        public void ReleaseQuestPrefab()
        {
            if (questPrefabHandle.IsValid())
            {
                Addressables.Release(questPrefabHandle);
                questPrefabHandle = default;
            }
            cachedQuestPrefab = null;
        }

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
        [SerializeField] private AssetReferenceGameObject questPrefab;
        [SerializeField, HideInInspector, FormerlySerializedAs("QuestPrefab")] private GameObject legacyQuestPrefab;
        [System.NonSerialized] private GameObject cachedQuestPrefab;
        [System.NonSerialized] private AsyncOperationHandle<GameObject> questPrefabHandle;

        [Tooltip("Optionan override for the world setup")]
        public WorldSetupData WorldSetup;

        [Header("Unity internal")]
        public string assetsFolder;
        public string scene;
        public LocalizedStringTable QuestStringsTable;
        public LocalizedAssetTable QuestAssetsTable;

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

        /// Returns ALL unique cards related to this quest.
        /// (all cards in Topics + all cards explicitly listed)
        public List<CardData> GetAllCards()
        {
            var set = new HashSet<CardData>();

            if (Topics != null)
            {
                foreach (var topic in Topics)
                {
                    if (topic == null)
                        continue;
                    try
                    {
                        var tCards = topic.GetAllCards();
                        if (tCards != null)
                        {
                            foreach (var c in tCards)
                                if (c != null)
                                    set.Add(c);
                        }
                    }
                    catch { }
                }
            }

            if (Cards != null)
            {
                foreach (var c in Cards)
                    if (c != null)
                        set.Add(c);
            }

            return set.ToList();
        }

        /// Returns ALL unique words related to the quest by aggregating:
        ///  Words from every card + words explicitly listed
        public List<WordData> GetAllWords()
        {
            var set = new HashSet<WordData>();

            // From cards
            foreach (var card in GetAllCards())
            {
                if (card == null || card.Words == null)
                    continue;
                foreach (var w in card.Words)
                    if (w != null)
                        set.Add(w);
            }

            // Quest-level words (if any)
            if (Words != null)
            {
                foreach (var w in Words)
                    if (w != null)
                        set.Add(w);
            }

            return set.ToList();
        }

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

        public GameObject GetQuestPrefabEditorAsset()
        {
            if (questPrefab != null && questPrefab.editorAsset != null)
                return questPrefab.editorAsset as GameObject;
            return legacyQuestPrefab;
        }

        private void OnValidate()
        {
            if (legacyQuestPrefab != null && !HasQuestPrefabReference)
            {
                questPrefab = CreateQuestPrefabReference(legacyQuestPrefab);
                legacyQuestPrefab = null;
            }
        }

        private static AssetReferenceGameObject CreateQuestPrefabReference(GameObject prefab)
        {
            if (prefab == null)
                return new AssetReferenceGameObject(string.Empty);
            var path = AssetDatabase.GetAssetPath(prefab);
            var guid = AssetDatabase.AssetPathToGUID(path);
            return new AssetReferenceGameObject(guid);
        }
#endif

    }
}
