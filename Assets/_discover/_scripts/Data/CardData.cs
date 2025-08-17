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
        public DevStatus DevStatus = DevStatus.Development;

        [Header("Knowledge Content")]
        public LocalizedString Title;
        public LocalizedString Description;
        public CardCategory Category;
        public List<KnowledgeTopic> Topics;
        [Tooltip("Optional, Year of origin, for historical context.")]
        public int Year;
        public Countries Country;

        [Header("Mastery")]
        [Tooltip("Mastery points needed to unlock this card.")]
        [Min(1)]
        public int MasteryPointsToUnlock = 1;

        [Header("Rewards")]
        [Range(0, 20)]
        public int Cookies = 0;
        [Range(0, 3)]
        [Tooltip("Number of gems given by this card, ONCE")]
        public int Gems = 0;
        [Range(0, 10)]
        [Tooltip("Points rewarded with positive interactions. cumulative")]
        public int Points = 1;

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
        [Tooltip("If true, multiple copies of this item can stack in one slot. If false, only one can be held.")]
        public bool IsStackable = false;
        [Tooltip("Maximum quantity per stack for this item (ignored if not stackable). Use 0 or negative for unlimited.")]
        public int MaxStack = 99;
        public ItemData ItemIcon;
        public ItemTag Tag;
        [Tooltip("Optional tag if set to Custom.")]
        public string CustomTag;

        [Header("Quests")]
        [Tooltip("Quests that can unlock this card. A card can be rewarded by multiple quests.")]
        public List<QuestData> Quests;


    }
}
