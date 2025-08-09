using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
