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
        Place = 1,
        Natural = 2,
        Art = 3,
        Food = 4,
        Flora = 5,
        Fauna = 6,
        People = 7,
        Object = 8,
        Humanity = 9
    }

    public enum Topic
    {
        None = 0,
        History = 1,
        Geography = 2,
        Culture = 3,
        Science = 4,
        Art = 5,
        Language = 6,
        Society = 7,
        Technology = 8,
        Environment = 9,
        Health = 10,
        Economy = 11,
        Politics = 12,
        Philosophy = 13,
        Religion = 14,
        Ethics = 15,
        Psychology = 16,
        Sociology = 17,
        Anthropology = 18,
        Law = 19,
        Education = 20,
        Literature = 21,
        Music = 22,
        VisualArts = 23,
        PerformingArts = 24,
        Architecture = 25,
        Film = 26,
        Dance = 27,
        Photography = 28,
        Fashion = 29,
        Design = 30,
        CulinaryArts = 31,
        Sports = 32,
        Travel = 33,
        Tradition = 34,
        Sustainability = 35,
        Diversity = 36,
        Inclusion = 37,
        Community = 38,
        Heritage = 39,
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

        [Header("Authors")]
        public List<AuthorDefinition> Content;
        public List<AuthorDefinition> Design;
        public List<AuthorDefinition> Development;

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
