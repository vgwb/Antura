using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Antura.Discover.Activities
{
    public class ActivityCanvas : ActivityBase
    {
        [Header("Activity Canvas Settings")]
        public Texture2D BackgroundImage;

        [Header("UI Refs")]
        public RawImage BackgroundImageUI;      // shows BackgroundImage
        public RawImage CoverImageUI;           // the opaque layer we scratch
        public TextMeshProUGUI ProgressLabel;   // shows e.g. "73%"

        [Header("Cover Settings")]
        public Color coverColor = new Color(0, 0, 0, 1f);
        [Tooltip("Lower resolution improves performance when erasing.")]
        public int coverTextureWidth = 1024;
        public int coverTextureHeight = 512;

        [Header("Brush Sizes (radius in pixels)")]
        public int brushSmall = 20;
        public int brushMedium = 34;
        public int brushLarge = 50;

        [Tooltip("Optional override; if > 0 forces this brush radius instead of difficulty-based.")]
        public int overrideBrushRadius = 0;

        [Header("Gameplay")]
        [Tooltip("Stop when fully cleared (100%).")]
        public bool endOnFullClear = true;
        [Tooltip("Percentage (0-100) considered complete. 100 = all.")]
        public float completionThreshold = 99.8f;

        private Texture2D coverTex;
        private int totalMaskPixels;
        private int clearedPixels;
        private bool completed;
        private int brushRadius;

        private RectTransform coverRect;
        private Camera uiCamera;   // if using Screen Space - Camera; left null for Overlay

        void Start()
        {
            Setup();
        }

        private void Setup()
        {
            if (BackgroundImageUI && BackgroundImage)
                BackgroundImageUI.texture = BackgroundImage;

            CreateCoverTexture();
            DetermineBrushRadius();
            UpdateProgressUI();
        }

        private void CreateCoverTexture()
        {
            if (coverTextureWidth <= 0 || coverTextureHeight <= 0)
            {
                coverTextureWidth = 512;
                coverTextureHeight = 512;
            }

            coverTex = new Texture2D(coverTextureWidth, coverTextureHeight, TextureFormat.RGBA32, false, true);
            var cols = new Color32[coverTextureWidth * coverTextureHeight];
            Color32 cc = coverColor;
            for (int i = 0; i < cols.Length; i++)
                cols[i] = cc;
            coverTex.SetPixels32(cols);
            coverTex.Apply();

            if (CoverImageUI)
            {
                CoverImageUI.texture = coverTex;
                coverRect = CoverImageUI.rectTransform;
            }

            totalMaskPixels = coverTextureWidth * coverTextureHeight;
            clearedPixels = 0;
            completed = false;
        }

        private void DetermineBrushRadius()
        {
            if (overrideBrushRadius > 0)
            {
                brushRadius = overrideBrushRadius;
                return;
            }

            // Map (example) activity difficulty (if ActivityBase exposes something). Adjust as needed.
            // Fallback: Normal -> Medium.
            // Replace with your real difficulty retrieval.
            var diffName = "Normal";
            int r;
            switch (diffName)
            {
                case "Tutorial":
                case "Easy":
                    r = brushSmall;
                    break;
                case "Hard":
                    r = brushLarge;
                    break;
                default:
                    r = brushMedium;
                    break;
            }
            brushRadius = Mathf.Max(2, r);
        }

        void Update()
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            if (Input.GetMouseButton(0))
                ScratchAt(Input.mousePosition);
#endif
            if (Input.touchCount > 0)
            {
                for (int i = 0; i < Input.touchCount; i++)
                {
                    var t = Input.GetTouch(i);
                    if (t.phase == TouchPhase.Moved || t.phase == TouchPhase.Stationary || t.phase == TouchPhase.Began)
                        ScratchAt(t.position);
                }
            }
        }

        private void ScratchAt(Vector2 screenPos)
        {
            if (completed || coverTex == null || coverRect == null)
                return;

            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    coverRect, screenPos, uiCamera, out var local))
                return;

            Vector2 size = coverRect.rect.size;
            // local origin is center => convert to 0..1
            float u = (local.x + size.x * 0.5f) / size.x;
            float v = (local.y + size.y * 0.5f) / size.y;
            if (u < 0 || u > 1 || v < 0 || v > 1)
                return;

            int cx = Mathf.RoundToInt(u * (coverTextureWidth - 1));
            int cy = Mathf.RoundToInt(v * (coverTextureHeight - 1));

            int r = brushRadius;
            int r2 = r * r;

            bool anyCleared = false;

            for (int y = cy - r; y <= cy + r; y++)
            {
                if (y < 0 || y >= coverTextureHeight)
                    continue;
                int dy = y - cy;
                int dy2 = dy * dy;
                int rowIndex = y * coverTextureWidth;
                for (int x = cx - r; x <= cx + r; x++)
                {
                    if (x < 0 || x >= coverTextureWidth)
                        continue;
                    int dx = x - cx;
                    if (dx * dx + dy2 > r2)
                        continue;

                    int idx = rowIndex + x;
                    Color32 c = coverTex.GetPixel(x, y);
                    if (c.a != 0)
                    {
                        c.a = 0;  // make transparent
                        coverTex.SetPixel(x, y, c);
                        clearedPixels++;
                        anyCleared = true;
                    }
                }
            }

            if (anyCleared)
            {
                coverTex.Apply(false);
                UpdateProgressUI();
            }
        }

        private void UpdateProgressUI()
        {
            float pct = Mathf.Clamp01((float)clearedPixels / totalMaskPixels) * 100f;
            if (ProgressLabel)
                ProgressLabel.text = $"{pct:0}%";

            if (!completed && pct >= completionThreshold)
            {
                completed = true;
                OnCanvasCleared();
            }
        }

        private void OnCanvasCleared()
        {
            Debug.Log("DONE");

        }
    }
}

