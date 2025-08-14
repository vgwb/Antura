using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace Antura.Discover
{
    // this is the "Knowledge Atom" of the Discover module.
    [CreateAssetMenu(fileName = "CardData", menuName = "Antura/Discover/Card")]
    public class CardData : IdentifiedData
    {
        [Header("Content")]
        public LocalizedString Title;
        public LocalizedString Description;

        [Header("Media")]
        public Sprite Image;
        [Tooltip("Optional audio clip shown with a sound icon in UI.")]
        public AudioClip Audio;

        [Header("Metadata")]
        public CardCategory Category;
        public List<KnowledgeTopic> Topics;
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
