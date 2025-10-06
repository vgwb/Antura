using System.Collections.Generic;
using Antura.Discover;
using UnityEngine;
using UnityEngine.Serialization;

namespace Antura.Discover.Activities
{
    [CreateAssetMenu(fileName = "JigsawSettings", menuName = "Antura/Activity/Jigsaw Settings")]
    public class JigsawPuzzleSettingsData : ActivitySettingsAbstract
    {
        private void OnEnable()
        {
            ActivityCode = ActivityCode.JigsawPuzzle;
        }

        [Header("--- Activity Jigsaw Settings")]
        [Tooltip("CardData providing the image for the puzzle")]
        public CardData PuzzleCard;

        [Header("Overrides Difficulty Based Settings")]
        [Tooltip("If > 0 overrides difficulty-based horizontal pieces.")]
        public int HorizontalPieces = 0;
        [Tooltip("If > 0 overrides difficulty-based vertical pieces.")]
        public int VerticalPieces = 0;

        public void Resolve(out Texture2D image, out int cols, out int rows, out Difficulty difficulty, out float underlayAlpha)
        {
            // Default underlay alpha
            underlayAlpha = 0.2f;
            var card = SelectPuzzleCard();
            image = ResolveTexture(card) ?? ResolveFallbackTexture(card);
            difficulty = Difficulty;

            int baseSize = 4;
            switch (Difficulty)
            {
                case Difficulty.Default:
                    baseSize = 4;
                    break;
                case Difficulty.Tutorial:
                    baseSize = 3;
                    underlayAlpha = 0.4f;
                    break;
                case Difficulty.Easy:
                    baseSize = 3;
                    underlayAlpha = 0.2f;
                    break;
                case Difficulty.Normal:
                    baseSize = 4;
                    underlayAlpha = 0.02f;
                    break;
                case Difficulty.Expert:
                    baseSize = 5;
                    underlayAlpha = 0f;
                    break;
                default:
                    baseSize = 4;
                    break;
            }

            cols = HorizontalPieces > 0 ? HorizontalPieces : baseSize;
            rows = VerticalPieces > 0 ? VerticalPieces : baseSize;
        }

        private CardData SelectPuzzleCard()
        {
            switch (SelectionMode)
            {
                case SelectionMode.ManualSet:
                    return PuzzleCard;
                case SelectionMode.RandomFromTopic:
                    var random = ResolveRandomTopicCard();
                    if (random != null)
                        return random;
                    break;
            }

            if (PuzzleCard != null)
                return PuzzleCard;

            return MainTopic?.CoreCard;
        }

        private CardData ResolveRandomTopicCard()
        {
            if (MainTopic == null)
                return null;

            List<CardData> cards = MainTopic.GetAllCards();
            if (cards == null || cards.Count == 0)
                return null;

            // Prefer cards that actually have textures available.
            var textured = new List<CardData>();
            for (int i = 0; i < cards.Count; i++)
            {
                var card = cards[i];
                if (card == null)
                    continue;
                if (HasTexture(card))
                    textured.Add(card);
            }

            var source = textured.Count > 0 ? textured : cards;
            if (source.Count == 0)
                return null;

            int idx = Random.Range(0, source.Count);
            return source[idx];
        }

        private static bool HasTexture(CardData card)
        {
            if (card == null)
                return false;
            return card.ImageAsset != null && card.ImageAsset.Image != null && card.ImageAsset.Image.texture != null;
        }

        private Texture2D ResolveFallbackTexture(CardData card)
        {
            // If the primary card had no texture, try fallback order: manual override -> topic cards -> null.
            if (card != PuzzleCard && PuzzleCard != null)
            {
                var tex = ResolveTexture(PuzzleCard);
                if (tex != null)
                    return tex;
            }

            if (MainTopic != null)
            {
                var cards = MainTopic.GetAllCards();
                if (cards != null)
                {
                    for (int i = 0; i < cards.Count; i++)
                    {
                        var tex = ResolveTexture(cards[i]);
                        if (tex != null)
                            return tex;
                    }
                }
            }

            return null;
        }

        private static Texture2D ResolveTexture(CardData data)
        {
            if (data == null)
                return null;
            if (data.ImageAsset != null && data.ImageAsset.Image != null && data.ImageAsset.Image.texture != null)
                return data.ImageAsset.Image.texture;
            return null;
        }
    }
}
