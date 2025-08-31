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
        public Status Status = Status.Draft;

        public KnowledgeImportance Importance = KnowledgeImportance.Medium;

        [Tooltip("What role does this card play in the game?")]
        public CardType Type;

        [Tooltip("What domain of knowledge does it belong to")]
        public List<KnowledgeTopic> Topics;

        [Tooltip("Optional, Year of origin, for historical context.")]
        public int Year;
        public Countries Country;
        [Tooltip("If the card has a specific place...")]
        public LocationData Location;
        public string WikipediaUrl;

        [Header("Content: internal English only")]
        public string TitleEn;
        [TextArea]
        public string DescriptionEn;
        [Header("Content: Localized")]
        public LocalizedString Title;
        public LocalizedString Description;

        [Header("References")]
        [Tooltip("Quests that can unlock this card. A card can be rewarded by multiple quests.")]
        public List<QuestData> Quests;
        [Tooltip("Words related to this card, for vocabulary learning and Living Letters spawned")]
        public List<WordData> Words;

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
        public Sprite PreviewImage;

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
        public ItemTag ItemTag;
        [Tooltip("Optional tag if set to Custom.")]
        public string CustomTag;

        [Header("Authoring Metadata")]
        [Tooltip("Why is this card important?")]
        [TextArea]
        public string Rationale;

        [Tooltip("Notes about this card for the authoring team.")]
        [TextArea]
        public string Notes;
        [Tooltip("Last review date in yyyy-MM-dd format. Auto-updated when TitleEn or DescriptionEn change.")]
        public string LastReviewed;
        [Tooltip("Set to true when TitleEn/DescriptionEn change we know we must update Localizatazions!.")]
        public bool NeedsLocalizationUpdate = false;
        [Tooltip("Set to true when TitleEn/DescriptionEn change we know we must update Localizatazions!.")]


        // Track these to detect changes in editor
        [SerializeField, HideInInspector] private string _lastTitleEn;
        [SerializeField, HideInInspector] private string _lastDescriptionEn;

#if UNITY_EDITOR
        private void OnValidate()
        {
            try
            {
                bool titleChanged = !string.Equals(TitleEn, _lastTitleEn, StringComparison.Ordinal);
                bool descChanged = !string.Equals(DescriptionEn, _lastDescriptionEn, StringComparison.Ordinal);
                if (titleChanged || descChanged)
                {
                    LastReviewed = DateTime.Now.ToString("yyyy-MM-dd");
                    NeedsLocalizationUpdate = true;
                    _lastTitleEn = TitleEn;
                    _lastDescriptionEn = DescriptionEn;
                }
            }
            catch { }
        }
#endif

    }
}
