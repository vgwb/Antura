using System.Collections.Generic;
using Antura.Discover;
using UnityEngine;
using UnityEngine.Serialization;

namespace Antura.Discover.Activities
{
    [CreateAssetMenu(fileName = "CanvasSettingsData", menuName = "Antura/Activity/Canvas Settings")]
    public class CanvasSettingsData : ActivitySettingsAbstract
    {
        private void OnEnable()
        {
            ActivityCode = ActivityCode.CleanCanvas;
        }

        [Header("--- Activity Canvas Settings")]
        [Tooltip("Specific card to use when Selection Mode is Manual Set.")]
        public CardData CanvasCard;

        [Tooltip("Background asset used as the canvas image")]
        public AssetData PuzzleImageAsset;

        public Sprite BugSprite;

        [Header("Hidden Treasures")]
        [Tooltip("Cards to hide under the canvas. Their ItemIcon will be shown when revealed.")]
        public List<CardData> HiddenTreasures = new();

        [Header("Overrides Difficulty Based Settings")]

        [Tooltip("If > 0 overrides difficulty-based bugs.")]
        public int Bugs = 0;

        [Tooltip("If > 0 overrides difficulty-based BrushSize.")]
        public int BrushSize = 0;

        [Tooltip("If > 0 overrides Percentage (0-100) considered complete. 100 = all.")]
        public float CompletionThreshold = 0f;

        public void Resolve(out Difficulty difficulty, out Texture2D image, out int bugs, out int brushSize, out float threshold)
        {
            image = ResolveTexture();
            difficulty = Difficulty;

            switch (Difficulty)
            {
                case Difficulty.Tutorial:
                    bugs = 0;
                    brushSize = 50;
                    threshold = 95f;
                    break;
                case Difficulty.Easy:
                    bugs = 1;
                    brushSize = 40;
                    threshold = 96f;
                    break;
                case Difficulty.Normal:
                    bugs = 2;
                    brushSize = 30;
                    threshold = 99f;
                    break;
                case Difficulty.Expert:
                    bugs = 4;
                    brushSize = 20;
                    threshold = 99.9f;
                    break;
                case Difficulty.Default:
                default:
                    brushSize = 50;
                    bugs = 0;
                    threshold = 99.8f;
                    break;
            }
            if (Bugs > 0)
                bugs = Bugs;

            if (BrushSize > 0)
                brushSize = BrushSize;

            if (CompletionThreshold > 0)
                threshold = CompletionThreshold;
        }

        private Texture2D ResolveTexture()
        {
            var card = SelectPuzzleCard();
            if (card != null)
            {
                var tex = TryGetCardTexture(card);
                if (tex != null)
                    return tex;
            }

            if (PuzzleImageAsset != null && PuzzleImageAsset.Image != null)
            {
                return PuzzleImageAsset.Image.texture;
            }

            return ResolveFallbackTextureFromTopic(card);
        }

        private CardData SelectPuzzleCard()
        {
            switch (SelectionMode)
            {
                case SelectionMode.ManualSet:
                    return CanvasCard;
                case SelectionMode.RandomFromTopic:
                    return GetRandomTopicCard();
                default:
                    return null;
            }
        }

        private CardData GetRandomTopicCard()
        {
            if (MainTopic == null)
                return null;

            List<CardData> cards = MainTopic.GetAllCards();
            if (cards == null || cards.Count == 0)
                return null;

            var textured = new List<CardData>();
            for (int i = 0; i < cards.Count; i++)
            {
                var card = cards[i];
                if (card != null && TryGetCardTexture(card) != null)
                {
                    textured.Add(card);
                }
            }

            var pool = textured.Count > 0 ? textured : cards;
            if (pool.Count == 0)
                return null;

            int idx = Random.Range(0, pool.Count);
            return pool[idx];
        }

        private Texture2D TryGetCardTexture(CardData card)
        {
            if (card == null)
                return null;
            if (card.ImageAsset != null && card.ImageAsset.Image != null && card.ImageAsset.Image.texture != null)
                return card.ImageAsset.Image.texture;
            return null;
        }

        private Texture2D ResolveFallbackTextureFromTopic(CardData alreadyTried)
        {
            if (CanvasCard != null && CanvasCard != alreadyTried)
            {
                var tex = TryGetCardTexture(CanvasCard);
                if (tex != null)
                    return tex;
            }

            if (MainTopic == null)
                return null;

            var cards = MainTopic.GetAllCards();
            if (cards == null)
                return null;

            for (int i = 0; i < cards.Count; i++)
            {
                var tex = TryGetCardTexture(cards[i]);
                if (tex != null)
                    return tex;
            }

            if (MainTopic.CoreCard != null)
            {
                var tex = TryGetCardTexture(MainTopic.CoreCard);
                if (tex != null)
                    return tex;
            }

            if (PuzzleImageAsset != null && PuzzleImageAsset.Image != null)
            {
                return PuzzleImageAsset.Image.texture;
            }

            return null;
        }
    }
}
