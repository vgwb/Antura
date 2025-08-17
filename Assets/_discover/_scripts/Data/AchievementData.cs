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
    public class AchievementData : ScriptableObject
    {
        [Header("Identity")]
        [Tooltip("Unique, stable ID. lowercase, Never change after shipping.")]
        public string Id;

        [Header("Content")]
        public LocalizedString Title;

        [Header("Rewards")]
        [Range(0, 3)]
        [Tooltip("Number of gems given by this card, ONCE")]
        public int Gems = 0;
        [Range(0, 1000)]
        [Tooltip("Points rewarded with positive interactions. cumulative")]
        public int Points = 0;

    }
}
