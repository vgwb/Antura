using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
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
        public Sprite Thumbnail;

        [Header("Gameplay")]
        public Difficulty Difficulty;
        public List<GameplayType> Gameplay;
        [Tooltip("In minutes.. approximately how long it takes to complete the quest.")]
        public int Duration;

        [Header("Content")]
        public KnowledgeTopic MainTopic;
        public List<CardData> Cards;
        public List<QuestData> Dependencies;
        public List<WordData> WordsUsed;

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
                        value += card.KnowledgeValue;
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

        [Header("Credits")]
        public List<AuthorData> CreditsContent;
        public List<AuthorData> CreditsDesign;
        public List<AuthorData> CreditsDevelopment;

        [Header("Unity References and Prefabs")]
        public string assetsFolder;
        public string scene;
        public GameObject WorldPrefab;
        public GameObject QuestPrefab;

        // TODO
        public int GetScore()
        {
            return 0;
        }

    }
}
