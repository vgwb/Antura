using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Homer;

namespace Antura.Minigames.DiscoverCountry
{
    public enum Countries
    {
        None = 0,
        Tutorial = 1,
        France = 2,
        Poland = 3
    }
    [Serializable]
    public class CountrySpriteMapping
    {
        public Countries country;
        public SpriteRenderer spriteRenderer;
    }

    [System.Flags]
    public enum Categories
    {
        None = 0,
        Place,
        Character,
        NaturalSite,
        Food,
        Art,
        CultureElement
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
        Ready = 3
    }

    [CreateAssetMenu(menuName = "Antura/Quest Data")]
    public class QuestData : ScriptableObject
    {
        public QuestStatus Status;
        public HomerFlowSlugs.FlowSlug QuestId;
        public Countries Country;
        public string Code;
        public string NumberCode;

        public string Title;
        public string Location;
        public string Categories;
        public int Duration;

        [Tooltip("how many nodes have to be visited for 100% score")]
        public int TotalProgress;

        [TextArea(3, 10)]
        public string Description;

        public string Gameplay;

        public string Credits;

        [TextArea(3, 10)]
        public string Content;

        public List<QuestData> Dependencies;

        public string LanguageRef;
        public string manualPage;
        public string assetsFolder;

        public Sprite Thumbnail;

        [Header("Quest Prefabs")]
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
