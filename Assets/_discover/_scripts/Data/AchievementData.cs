using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace Antura.Discover
{
    public enum AchievementType
    {
        None = 0,
        Global = 1,
        QuestSpecific = 2,
    }

    [CreateAssetMenu(fileName = "AchievementData", menuName = "Antura/Discover/Achievement", order = 1)]
    public class AchievementData : IdentifiedData
    {
        [Header("Content")]
        public LocalizedString Title;

        [Tooltip("Description of the achievement, this is internal information only.")]
        public string Description;

        [Header("Rewards")]
        [Range(0, 3)]
        [Tooltip("Number of gems given by this card, ONCE")]
        public int Gems = 0;
        [Range(0, 1000)]
        [Tooltip("Points rewarded with positive interactions. cumulative")]
        public int Points = 0;

    }
}
