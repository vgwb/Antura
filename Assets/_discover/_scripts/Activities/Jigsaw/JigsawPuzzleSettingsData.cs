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
            image = ResolveTexture(PuzzleCard);
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
