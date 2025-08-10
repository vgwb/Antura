using UnityEngine;

namespace Antura.Discover.Activities
{
    [CreateAssetMenu(fileName = "JigsawPuzzleSettings", menuName = "Antura/Activity/Jigsaw Settings")]
    public class JigsawPuzzleSettingsData : ActivitySettingsAbstract
    {
        [Header("JigsawPuzzle Settings")]
        public Texture2D PuzzleImage;

        [Header("Overrides Difficulty Based Settings")]
        [Tooltip("If > 0 overrides difficulty-based horizontal pieces.")]
        public int HorizontalPieces = 0;
        [Tooltip("If > 0 overrides difficulty-based vertical pieces.")]
        public int VerticalPieces = 0;

        public void Resolve(out Texture2D image, out int cols, out int rows, out Difficulty difficulty, out float underlayAlpha)
        {
            // Default underlay alpha
            underlayAlpha = 0.2f;
            image = PuzzleImage;
            difficulty = Difficulty;

            int baseSize;
            switch (Difficulty)
            {
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
    }
}
