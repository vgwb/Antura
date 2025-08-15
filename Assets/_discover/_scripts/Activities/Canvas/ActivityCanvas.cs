using UnityEngine;
using UnityEngine.UI;
using Antura; // for Sfx enum
using Antura.Audio; // for AudioManager
using TMPro;
using DG.Tweening;

namespace Antura.Discover.Activities
{
    public class ActivityCanvas : ActivityBase
    {
        [Header("Activity Canvas Settings")]
        public CanvasSettingsData Settings;
        [Tooltip("Optional bug prefab. If assigned, this prefab will be instantiated for each bug.")]
        public CanvasBug BugPrefab;

        [Header("Override Settings")]
        public Texture2D BackgroundImage;

        [Tooltip("Optional override; if > 0 forces this brush radius instead of difficulty-based.")]
        public int BrushSize = 0;
        public Difficulty ActivityDifficulty = Difficulty.Default;
        public int Bugs = 0;

        [Header("UI Refs")]
        public RawImage BackgroundImageUI;
        public RawImage CoverImageUI;
        public TextMeshProUGUI ProgressLabel;

        [Header("Cover Settings")]
        public Color coverColor = new Color(0, 0, 0, 1f);
        [Tooltip("Lower resolution improves performance when erasing.")]
        public int coverTextureWidth = 1024;
        public int coverTextureHeight = 512;

        [Header("Gameplay")]
        [Tooltip("Stop when fully cleared (100%).")]
        public bool endOnFullClear = true;
        [Tooltip("Percentage (0-100) considered complete. 100 = all.")]
        private float completionThreshold = 99.8f;

        private Texture2D coverTex;
        private int totalMaskPixels;
        private int clearedPixels;
        private bool completed;

        private RectTransform coverRect;
        private Camera uiCamera;   // if using Screen Space - Camera; left null for Overlay
        private readonly System.Collections.Generic.List<RectTransform> bugsList = new System.Collections.Generic.List<RectTransform>();
        public RectTransform bugsLayer; // optional separate layer for bugs over the cover
        public float bugBaseSpeed = 120f; // px/sec at Normal
        public Vector2 bugScaleRange = new Vector2(0.8f, 1.2f);

        void Start()
        {
            ResolveDifficulty();
            Setup();
        }

        private void ResolveDifficulty()
        {
            Settings.Resolve(out var difficulty, out var image, out var bugs, out var brushSize, out var compThreshold);
            if (ActivityDifficulty == Difficulty.Default)
                ActivityDifficulty = difficulty;
            if (BackgroundImage == null)
                BackgroundImage = image;
            if (Bugs == 0)
                Bugs = bugs;
            if (BrushSize == 0)
                BrushSize = brushSize;
            BrushSize = Mathf.Max(2, BrushSize);

            completionThreshold = compThreshold;
        }

        private void Setup()
        {
            if (BackgroundImageUI && BackgroundImage)
                BackgroundImageUI.texture = BackgroundImage;

            CreateCoverTexture();
            UpdateProgressUI();

            SpawnBugs();
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

            int r = BrushSize;
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

        #region Bugs

        private void SpawnBugs()
        {
            if (Settings == null || Settings.BugSprite == null)
                return;
            int count = Mathf.Max(0, Bugs);
            if (count <= 0)
                return;

            var parent = bugsLayer != null ? bugsLayer : (CoverImageUI != null ? CoverImageUI.rectTransform : null);
            if (parent == null)
                return;

            var sprite = Settings.BugSprite;
            for (int i = 0; i < count; i++)
            {
                CanvasBug bug;
                RectTransform rt;
                if (BugPrefab != null)
                {
                    bug = Instantiate(BugPrefab, parent);
                    rt = bug.GetComponent<RectTransform>();
                    rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
                    rt.pivot = new Vector2(0.5f, 0.5f);

                    var img = bug.GetComponent<Image>();
                    if (img && img.sprite == null && sprite)
                        img.sprite = sprite;
                    if (img)
                        img.raycastTarget = true;
                }
                else
                {
                    var go = new GameObject($"Bug_{i}", typeof(RectTransform), typeof(Image), typeof(BoxCollider2D), typeof(CanvasBug));
                    rt = go.GetComponent<RectTransform>();
                    rt.SetParent(parent, false);
                    rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
                    rt.pivot = new Vector2(0.5f, 0.5f);

                    var img = go.GetComponent<Image>();
                    img.sprite = sprite;
                    img.raycastTarget = true;

                    bug = go.GetComponent<CanvasBug>();
                }

                // Provide a slight speed multiplier based on uncovered percentage for rising tension
                System.Func<float> multiplier = () =>
                {
                    if (totalMaskPixels <= 0)
                        return 1f;
                    float pct = Mathf.Clamp01((float)clearedPixels / totalMaskPixels);
                    return 1f + pct * 0.6f; // up to +60% speed when almost cleared
                };
                bug.SpeedMultiplierProvider = multiplier;
                bug.Init(parent, BugSpeed(), null, null, bugScaleRange, OnBugTouched);
                bugsList.Add(rt);
            }
        }

        private float BugSpeed()
        {
            // Map difficulty to a speed multiplier without a switch to satisfy strict analyzers
            float mul = 1f;
            if (ActivityDifficulty == Difficulty.Tutorial)
                mul = 0.6f;
            else if (ActivityDifficulty == Difficulty.Easy)
                mul = 0.8f;
            else if (ActivityDifficulty == Difficulty.Expert)
                mul = 1.4f;
            else /* Default, Normal or any unknown */
                mul = 1f;
            return bugBaseSpeed * mul;
        }

        // Movement handled by CanvasBug component

        private void OnBugTouched()
        {
            // Treat as immediate fail
            if (!completed)
            {
                AudioManager.I?.PlaySound(Sfx.Lose);
                EndRound(false, 0f, false);
            }
        }

        #endregion
    }
}

