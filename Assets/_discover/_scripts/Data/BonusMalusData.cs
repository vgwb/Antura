using UnityEngine;
using UnityEngine.Localization;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Antura.Discover
{
    public enum BonusMalusType
    {
        Bonus = 1,
        Malus = -1
    }

    /// <summary>
    /// a Bonus/Malus action that adds or removes progress points
    /// </summary>
    [CreateAssetMenu(fileName = "BonusMalusData", menuName = "Antura/Discover/Bonus-Malus")]
    public class BonusMalusData : IdentifiedData
    {
        [Tooltip("Just for debugging, not used in gameplay")]
        public BonusMalusType Type = BonusMalusType.Bonus;

        [Tooltip("Localized title for the book")]
        public LocalizedString Title;

        public LocalizedString Description;

        [Tooltip("Localized title shown to the player when this action triggers")]
        public LocalizedString Feedback;

        public Image Icon;

        [Tooltip("Progress points to apply (can be negative)")]
        [Range(-10, 10)]
        public int Points = 1;
    }
}
