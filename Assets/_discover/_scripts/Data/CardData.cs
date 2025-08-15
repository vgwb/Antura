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
        [Header("Knowledge Content")]
        public LocalizedString Title;
        public LocalizedString Description;
        public CardCategory Category;
        public List<KnowledgeTopic> Topics;
        [Tooltip("Optional, Year of origin, for historical context.")]
        public int Year;
        public Countries Country;
        [Range(1, 10)]
        public int KnowledgeValue = 10;

        [Header("Media")]
        public AssetType MediaType = AssetType.Image;
        [Tooltip("Let' put an image always, even if media type is different")]
        public AssetData ImageAsset;

        [Tooltip("Optional image shown in the card, if different from the main image.")]
        public Sprite Image;

        [Tooltip("Optional audio clip shown with a sound icon in UI.")]
        public AssetData AudioAsset;

        [Header("Gameplay and Inventory Settings")]
        [Tooltip("Can the card be collected by the player?")]
        public bool IsCollectible = false;
        public ItemData ItemIcon;
        public ItemTag Tag;
        [Tooltip("Optional tag if set to Custom.")]
        public string CustomTag;

        [Header("Linked Sources")]
        [Tooltip("Quests that can unlock this card. A card can be rewarded by multiple quests.")]
        public List<QuestData> LinkedQuests;


    }
}
