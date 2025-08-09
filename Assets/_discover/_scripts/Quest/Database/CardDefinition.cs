using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace Antura.Discover.Achievements
{
    /// <summary>
    /// Immutable design-time data for a collectible card.
    /// Runtime state (unlocked/progress/history) is saved separately.
    /// </summary>
    [CreateAssetMenu(fileName = "CardDefinition", menuName = "Antura/Discover/Card Definition")]
    public class CardDefinition : ScriptableObject
    {
        [Header("Identity")]
        [Tooltip("Unique, stable ID. lowercase, Never change after shipping.")]
        public string Id;

        [Header("Content")]
        public LocalizedString Title;
        public LocalizedString Description;

        [Header("Media")]
        public Sprite Image;
        [Tooltip("Optional audio clip shown with a sound icon in UI.")]
        public AudioClip Audio;

        [Header("Metadata")]
        public CardCategory Category;
        public List<Topic> Topics;
        [Tooltip("Year of origin, for historical context.")]
        public int Year;
        public Countries Country;

        [Header("Unlock Sources")]
        [Tooltip("Quests that can unlock this card. A card can be rewarded by multiple quests.")]
        public List<QuestData> UnlockQuests = new();

        [Header("Progress")]
        [Range(1, 10)] public int MaxProgress = 10;
    }
}
