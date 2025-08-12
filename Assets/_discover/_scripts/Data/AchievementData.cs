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

    }
}
