using UnityEngine;
using UnityEngine.UI;
using Antura; // for Sfx enum
using Antura.Audio; // for AudioManager
using TMPro;
using DG.Tweening;
using System.Collections.Generic;

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
        [Tooltip("Optional parent for treasure icons (defaults to BackgroundImageUI)")]
        public RectTransform TreasuresLayer;

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
        private bool treasuresSpawned;

        private RectTransform coverRect;
        private Camera uiCamera;   // if using Screen Space - Camera; left null for Overlay
        private readonly List<RectTransform> bugsList = new List<RectTransform>();
        public RectTransform bugsLayer; // optional separate layer for bugs over the cover
        public float bugBaseSpeed = 120f; // px/sec at Normal
        public Vector2 bugScaleRange = new Vector2(0.8f, 1.2f);

        private readonly List<(CardData card, RectTransform rt, bool collected)> treasures = new();
        private bool _didInit;

        void Start()
        {
            // If launched outside ActivityManager flow, do a best-effort init
            if (!_didInit)
            {
                ResolveDifficulty();
                Setup();
                _didInit = true;
            }
        }

        public override void ConfigureSettings(ActivitySettingsAbstract settings)
        {
            base.ConfigureSettings(settings);
            if (settings is CanvasSettingsData csd)
                Settings = csd;
        }

        public override void InitActivity()
        {
            Settings = base._configuredSettings as CanvasSettingsData;
            // Called by ActivityBase.Open
            ResolveDifficulty();
            Setup();
            _didInit = true;
        }

        protected override void OnResetActivity()
        {
            // Destroy textures and runtime objects from previous runs
            if (coverTex != null)
            {
                try
                { Destroy(coverTex); }
                catch { }
                coverTex = null;
            }
            if (CoverImageUI)
                CoverImageUI.texture = null;
            for (int i = 0; i < bugsList.Count; i++)
            {
                var rt = bugsList[i];
                if (rt != null)
                    Destroy(rt.gameObject);
            }
            bugsList.Clear();
            for (int i = 0; i < treasures.Count; i++)
            {
                var t = treasures[i];
                if (t.rt != null)
                    Destroy(t.rt.gameObject);
            }
            treasures.Clear();
            _didInit = false;
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
            SpawnTreasures();
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
            treasuresSpawned = false; // will respawn on Setup
        }

        protected override void Update()
        {
            base.Update();
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
                RevealTreasuresUnderArea(cx, cy, r);
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
            // Auto-reveal any remaining uncollected treasures visually
            foreach (var t in treasures)
            {
                if (!t.collected && t.rt != null)
                {
                    var img = t.rt.GetComponent<UnityEngine.UI.Image>();
                    if (img)
                        img.DOFade(1f, 0.3f).SetUpdate(true);
                }
            }

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

        #region Treasures

        private void SpawnTreasures()
        {
            treasures.Clear();
            var list = Settings != null ? Settings.HiddenTreasures : null;
            if (list == null || list.Count == 0)
                return;

            var parent = TreasuresLayer != null ? TreasuresLayer : (BackgroundImageUI != null ? BackgroundImageUI.rectTransform : null);
            if (parent == null)
                return;

            // Place treasures at random non-overlapping positions
            var placed = new List<Rect>();
            foreach (var card in list)
            {
                if (card == null || card.ItemIcon == null || card.ItemIcon.Icon == null)
                    continue;

                var go = new GameObject($"Treasure_{card.Id}", typeof(RectTransform), typeof(UnityEngine.UI.Image), typeof(UnityEngine.UI.Button));
                var rt = go.GetComponent<RectTransform>();
                rt.SetParent(parent, false);
                rt.sizeDelta = new Vector2(80, 80);
                rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
                rt.pivot = new Vector2(0.5f, 0.5f);

                var img = go.GetComponent<UnityEngine.UI.Image>();
                img.sprite = card.ItemIcon.Icon;
                img.color = new Color(1, 1, 1, 0); // start hidden under cover
                img.raycastTarget = true;

                // Random position with simple rejection to avoid heavy overlap
                Rect pr = parent.rect;
                Vector2 pos;
                int tries = 0;
                Rect rect;
                do
                {
                    pos = new Vector2(
                        Random.Range(pr.xMin + 40, pr.xMax - 40),
                        Random.Range(pr.yMin + 40, pr.yMax - 40));
                    rect = new Rect(pos - rt.sizeDelta * 0.5f, rt.sizeDelta);
                    tries++;
                } while (tries < 20 && Overlaps(rect, placed));
                placed.Add(rect);
                rt.anchoredPosition = pos;

                var btn = go.GetComponent<UnityEngine.UI.Button>();
                btn.onClick.AddListener(() => OnTreasureClicked(card, rt));

                treasures.Add((card, rt, false));
            }

            treasuresSpawned = true;
        }

        private bool Overlaps(Rect r, List<Rect> others)
        {
            for (int i = 0; i < others.Count; i++)
                if (others[i].Overlaps(r))
                    return true;
            return false;
        }

        private void RevealTreasuresUnderArea(int cx, int cy, int radius)
        {
            if (!treasuresSpawned || treasures.Count == 0 || coverRect == null)
                return;

            // Convert cover pixel circle to coverRect local space box for rough test
            float ux = cx / (float)(coverTextureWidth - 1);
            float uy = cy / (float)(coverTextureHeight - 1);
            Vector2 size = coverRect.rect.size;
            Vector2 center = new Vector2(ux * size.x - size.x * 0.5f, uy * size.y - size.y * 0.5f);
            float radPixels = radius;
            float radLocal = radPixels / (coverTextureWidth - 1) * size.x; // approximate using width scale

            for (int i = 0; i < treasures.Count; i++)
            {
                var t = treasures[i];
                if (t.collected || t.rt == null)
                    continue;
                Vector2 pos = t.rt.anchoredPosition;
                if ((pos - center).sqrMagnitude <= radLocal * radLocal)
                {
                    var img = t.rt.GetComponent<UnityEngine.UI.Image>();
                    if (img && img.color.a < 1f)
                        img.DOFade(1f, 0.2f).SetUpdate(true);
                }
            }
        }

        private void OnTreasureClicked(CardData card, RectTransform rt)
        {
            // Mark collected and play SFX
            for (int i = 0; i < treasures.Count; i++)
            {
                if (treasures[i].card == card && treasures[i].rt == rt)
                {
                    treasures[i] = (card, rt, true);
                    break;
                }
            }

            // Feedback
            AudioManager.I?.PlaySound(Sfx.Win);
            rt.DOKill();
            rt.DOPunchScale(Vector3.one * 0.2f, 0.25f, 6, 0.9f).SetUpdate(true);

            // TODO: hook into AchievementsManager to mark unlocked if desired
            // var ach = FindObjectOfType<AchievementsManager>();
            // if (ach != null) ach.MarkFound(card);
        }

        #endregion
    }
}

