using Antura.Discover;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Antura.Discover.Activities
{
    [CreateAssetMenu(fileName = "CanvasSettingsData", menuName = "Antura/Activity/Canvas Settings")]
    public class CanvasSettingsData : ActivitySettingsAbstract
    {
        [Header("Activity Canvas Settings")]
        [Tooltip("Background asset used as the canvas image")]
        public AssetData PuzzleImageAsset;

        // Backward-compatibility with older assets using a raw Texture2D
        [SerializeField, FormerlySerializedAs("PuzzleImage"), HideInInspector]
        private Texture2D LegacyPuzzleImage;

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
            image = (PuzzleImageAsset != null && PuzzleImageAsset.Image != null)
                ? PuzzleImageAsset.Image.texture
                : LegacyPuzzleImage;
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
    }
}
