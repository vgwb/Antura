using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Homer;

namespace Antura.Minigames.DiscoverCountry
{
    public enum Countries
    {
        Tutorial = 1,
        France = 2,
        Poland = 3
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

    [CreateAssetMenu(menuName = "Antura/Quest Data")]
    public class QuestData : ScriptableObject
    {
        public bool Active;
        public HomerFlowSlugs.FlowSlug QuestId;
        public Countries Country;
        public string Code;

        public string Title;
        public string Location;
        public string Categories;
        public int Duration;

        [TextArea(3, 10)]
        public string Description;

        public string Gameplay;

        [TextArea(3, 10)]
        public string Content;

        public string LanguageRef;
        public string manualPage;

        public Sprite Thumbnail;

        [Header("Scene")]
        public string scene;

        [Header("Level Prefabs")]
        public GameObject Town;
        public GameObject GameLevel;

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
