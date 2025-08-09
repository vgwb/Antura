using Antura.Discover.Achievements;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

using Homer;

namespace Antura.Discover
{
    public enum Countries
    {
        None = 0,
        Tutorial = 1,
        France = 2,
        Poland = 3,
        Italy = 4,
        Spain = 5,
    }

    [Serializable]
    public class CountrySpriteMapping
    {
        public Countries country;
        public SpriteRenderer spriteRenderer;
    }

    public enum Difficulty
    {
        Tutorial = 1,
        Easy = 2,
        Normal = 3,
        Difficult = 4
    }

    [Serializable]
    public enum CardCategory
    {
        None = 0,
        Achievement = 10,
        Art = 3,
        Fauna = 6,
        Flora = 5,
        Food = 4,
        Humanity = 9,
        Natural = 2,
        Object = 8,
        People = 7,
        Place = 1,
    }

    public enum Topic
    {
        None = 0,
        Anthropology = 18,
        Architecture = 25,
        Art = 5,
        Community = 38,
        CulinaryArts = 31,
        Culture = 3,
        Dance = 27,
        Design = 30,
        Diversity = 36,
        Economy = 11,
        Education = 20,
        Environment = 9,
        Ethics = 15,
        Fashion = 29,
        Film = 26,
        Geography = 2,
        Health = 10,
        Heritage = 39,
        History = 1,
        Inclusion = 37,
        Language = 6,
        Law = 19,
        Literature = 21,
        Music = 22,
        PerformingArts = 24,
        Philosophy = 13,
        Photography = 28,
        Politics = 12,
        Religion = 14,
        Science = 4,
        Society = 7,
        Sociology = 17,
        Sports = 32,
        Technology = 8,
        Tradition = 34,
        Travel = 33,
        VisualArts = 23,
    }

    public enum GameplayType
    {
        Orientation,
        Seek,
        Parkour,
        Puzzles,
        Deduction,
        Journey,
        Arcade,
        Memory,
        Story
    }

    public enum QuestStatus
    {
        Inactive = 0,
        Development = 1,
        Testing = 2,
        Ready = 3,
        Validated = 4,
    }

    [CreateAssetMenu(menuName = "Antura/Discover/Quest Data")]
    public class QuestData : ScriptableObject
    {
        [Header("Identity")]
        public HomerFlowSlugs.FlowSlug FlowSlug;

        [Tooltip("Unique, stable ID. lowercase")]
        public string Code;
        [Tooltip("Just for display")]
        public string NumberCode;
        public QuestStatus Status;

        [Header("Description")]
        public LocalizedString Title;
        public Countries Country;
        public LocationDefinition Location;
        public LocalizedString Description;
        public Sprite Thumbnail;

        [Header("Gameplay")]
        public Difficulty Difficulty;
        public List<GameplayType> Gameplay;
        [Tooltip("In minutes.. approximately how long it takes to complete the quest.")]
        public int Duration;

        [Header("Content")]
        public Topic MainTopic;
        public List<CardDefinition> Cards;
        public List<QuestData> Dependencies;
        public string LanguageRef;
        public string manualPage;

        [Header("Credits")]
        public List<AuthorDefinition> CreditsContent;
        public List<AuthorDefinition> CreditsDesign;
        public List<AuthorDefinition> CreditsDevelopment;

        [Header("Unity References and Prefabs")]
        public string assetsFolder;
        public string scene;
        public GameObject WorldPrefab;
        public GameObject QuestPrefab;

        /// <summary>
        /// Gets the score (completeness) for this quest.
        /// </summary>
        /// <returns>
        /// An integer representing the number of stars.. 0 -> 3.
        /// </returns>
        public int GetScore()
        {
            return 0;
        }

    }
}
