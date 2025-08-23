using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;
using Yarn.Unity;

namespace Antura.Discover
{
    [CreateAssetMenu(fileName = "QuestData", menuName = "Antura/Discover/Quest Data")]
    public class QuestData : IdentifiedData
    {
        public YarnProject YarnProject;
        public TextAsset YarnScript;

        [Tooltip("Just for display in the UI. Not used in the game logic.")]
        public string IdDisplay;

        [Tooltip("Development status of this quest. Needed for internal testing and validation.")]
        public DevStatus DevStatus;

        [Header("Description")]
        public LocalizedString Title;
        public Countries Country;
        public LocationData Location;
        public LocalizedString Description;
        public AssetData Thumbnail;

        [Header("Content")]
        public KnowledgeTopic MainTopic;
        public List<CardData> Cards;
        public List<WordData> Words;

        [Header("Gameplay")]
        public Difficulty Difficulty;
        public List<GameplayType> Gameplay;
        [Tooltip("In minutes.. approximately how long it takes to complete the quest.")]
        public int Duration;

        [Tooltip("Does this quest require any other quest to be completed first?")]
        public List<QuestData> Dependencies;

        [Header("Rewards")]
        [Range(0, 20)]
        public int cookies = 0;

        [Header("Public website docs")]
        public bool IsPublic;
        public bool IsScriptPublic;
        public string manualPage;

        [Tooltip("for the Website. THis is a Markdown file that contains additional resources for the quest, such as links to videos, articles, etc.")]
        public TextAsset AdditionalResources;

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

        // Convenience localized fields
        public string TitleText
        {
            get
            {
                try
                { return Title.GetLocalizedString(); }
                catch { return string.Empty; }
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

        public List<AuthorCredit> Credits;

        [Header("Unity References and Prefabs")]
        public GameObject WorldPrefab;
        public GameObject QuestPrefab;

        public WorldController WorldControllerPrefab;

        public string assetsFolder;
        public string scene;

        [Header("Localization")]
        [Tooltip("String Table Collection used by Yarn's Localized Line Provider for this quest.")]
        public TableReference YarnStringTable;

        // Returns the player's best stars for this quest (0..3) using DiscoverAppManager's current profile.
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

    }
}
