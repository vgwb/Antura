using System.Collections;
using System.Collections.Generic;
using Antura.Discover.Audio;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.Discover.Activities
{
    public class ActivityMemory : ActivityBase
    {
        [Header("Activity Memory Settings")]
        public MemorySettingData Settings;

        [Header("References")]

        [Tooltip("Common sprite used for the back of all cards.")]
        public Sprite commonBack;

        [Header("Difficulty & Grid")]
        public Difficulty difficulty = Difficulty.Easy;
        public RectTransform gridParent;
        public GameObject cardPrefab;
        public Vector2 cellSize = new Vector2(180, 220);
        public Vector2 spacing = new Vector2(18, 18);

        [Header("Hints")]
        [Tooltip("In tutorial mode, show hint after X seconds of inactivity.")]
        public float tutorialIdleSeconds = 5f;
        [Tooltip("In easy mode, shake 1-2 random pairs every X seconds.")]
        public float easyShakeInterval = 6f;

        [Header("SFX (optional)")]
        public AudioSource audioSource;
        public AudioClip flipSfx;
        public AudioClip matchSfx;
        public AudioClip mismatchSfx;

        private GridLayoutGroup grid;
        private readonly List<MemoryCard> cards = new();
        private MemoryCard first, second;
        private bool resolving;
        private int matchedPairs;
        private int totalPairs;
        private float lastActionTime;

        void Awake()
        {
            if (gridParent != null)
                grid = gridParent.GetComponent<GridLayoutGroup>();
        }

        public override void InitActivity()
        {
            if (Settings != null)
                difficulty = Settings.Difficulty;

            BuildBoard();
            if (difficulty == Difficulty.Tutorial)
                StartCoroutine(TutorialIdleHints());
            else if (difficulty == Difficulty.Easy)
                StartCoroutine(EasyPeriodicShakes());
        }

        private void OnWin()
        {
            Debug.Log("🏆 Memory: all pairs found!");
            EnableValidateButton(true);
        }

        public override bool DoValidate()
        {
            return true;
        }

        protected override void Update()
        {
            base.Update();
        }

        public override void ConfigureSettings(ActivitySettingsAbstract settings)
        {
            base.ConfigureSettings(settings);
            if (settings is MemorySettingData csd)
                Settings = csd;
        }

        /// <summary>
        /// Builds the board using CardData only.
        /// </summary>
        public void BuildBoard()
        {
            if (Settings == null || Settings.CardsData == null || Settings.CardsData.Count == 0)
            { Debug.LogError("Memory: missing CardsData"); return; }

            // Clear previous grid
            foreach (Transform c in gridParent)
                Destroy(c.gameObject);
            cards.Clear();
            matchedPairs = 0;
            resolving = false;
            first = second = null;
            lastActionTime = Time.unscaledTime;

            // Set grid size based on difficulty (avoid switch-expression to satisfy analyzers)
            int rows, cols;
            if (difficulty == Difficulty.Tutorial)
            { rows = 2; cols = 2; }
            else if (difficulty == Difficulty.Easy)
            { rows = 2; cols = 4; }
            else if (difficulty == Difficulty.Normal)
            { rows = 3; cols = 4; }
            else
            { rows = 3; cols = 6; }
            int totalCards = rows * cols;
            totalPairs = totalCards / 2;

            // Configure GridLayoutGroup
            grid.cellSize = cellSize;
            grid.spacing = spacing;
            grid.startAxis = GridLayoutGroup.Axis.Horizontal;
            grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            grid.constraintCount = cols;
            grid.childAlignment = TextAnchor.MiddleCenter;

            var autoSize = CalculateAutoCellSize(rows, cols);
            grid.cellSize = autoSize;

            // Pick unique faces from CardData
            var selectedFaces = PickUniqueFaces(totalPairs);
            var pool = new List<(int id, Sprite face)>();
            for (int i = 0; i < selectedFaces.Count; i++)
            {
                pool.Add((i, selectedFaces[i]));
                pool.Add((i, selectedFaces[i]));
            }
            Shuffle(pool);

            // Instantiate cards
            foreach (var entry in pool)
            {
                var go = Instantiate(cardPrefab, gridParent);
                var card = go.GetComponent<MemoryCard>();
                card.Init(this, entry.id, entry.face, commonBack);
                cards.Add(card);
            }

            if (gridParent != null)
                LayoutRebuilder.ForceRebuildLayoutImmediate(gridParent);
        }

        /// <summary>
        /// Picks a number of unique card faces from CardsData.
        /// Loops if there are not enough unique items.
        /// </summary>
        private List<Sprite> PickUniqueFaces(int count)
        {
            var result = new List<Sprite>(count);
            var cd = Settings.CardsData;
            if (cd == null || cd.Count == 0)
            {
                Debug.LogError("Memory: no CardsData provided");
                return result;
            }

            var bag = new List<Sprite>(cd.Count);
            foreach (var data in cd)
            {
                if (data == null)
                    continue;
                var sprite = ResolveSprite(data);
                if (sprite != null)
                    bag.Add(sprite);
            }

            Shuffle(bag);
            for (int i = 0; i < count; i++)
                result.Add(bag[i % bag.Count]);

            return result;
        }

        private static Sprite ResolveSprite(Antura.Discover.CardData data)
        {
            if (data == null)
                return null;
            if (data.ImageAsset != null)
                return data.ImageAsset.GetImage();
            return null;
        }

        /// <summary>
        /// Called by a MemoryCard when the player clicks it.
        /// </summary>
        public void TryReveal(MemoryCard card)
        {
            if (resolving || card.IsLocked || card.IsFaceUp)
                return;
            lastActionTime = Time.unscaledTime;

            card.RevealUp();
            DiscoverAudioManager.I.PlaySfx(DiscoverSfx.ActivityClick);

            if (first == null)
            { first = card; return; }
            if (card == first)
                return;
            second = card;
            StartCoroutine(ResolvePair());
        }

        /// <summary>
        /// Checks if two revealed cards match, locks them if correct, or hides them again.
        /// </summary>
        private IEnumerator ResolvePair()
        {
            resolving = true;
            yield return new WaitForSecondsRealtime(0.25f);

            if (first.pairId == second.pairId)
            {
                first.Lock();
                second.Lock();
                DiscoverAudioManager.I.PlaySfx(DiscoverSfx.ActivityGoodMove);
                matchedPairs++;
                if (matchedPairs >= totalPairs)
                    OnWin();
                // brief settle delay
                yield return new WaitForSecondsRealtime(0.05f);
            }
            else
            {
                // mismatch: flip both back down and play SFX, keep input locked during flip
                DiscoverAudioManager.I.PlaySfx(DiscoverSfx.ActivityBadMove);
                float wait = 1f;
                yield return new WaitForSecondsRealtime(wait);
                first.HideDown();
                second.HideDown();
                // wait approximately the duration of the flip animation to avoid early input
            }

            first = null;
            second = null;
            resolving = false;
        }


        /// <summary>
        /// Tutorial mode: wiggle one correct pair after X seconds of inactivity.
        /// </summary>
        private IEnumerator TutorialIdleHints()
        {
            while (true)
            {
                yield return new WaitForSecondsRealtime(0.25f);
                if (Time.unscaledTime - lastActionTime >= tutorialIdleSeconds && !resolving)
                {
                    var pair = PickRandomUnmatchedPair();
                    if (pair != null)
                    {
                        // Vibrate();
                        pair.Value.a.Wiggle();
                        pair.Value.b.Wiggle();
                    }
                    lastActionTime = Time.unscaledTime;
                }
            }
        }

        /// <summary>
        /// Easy mode: periodically wiggle a couple of random pairs.
        /// </summary>
        private IEnumerator EasyPeriodicShakes()
        {
            while (true)
            {
                yield return new WaitForSecondsRealtime(easyShakeInterval);
                if (resolving)
                    continue;
                int pairs = Random.Range(1, 3);
                for (int i = 0; i < pairs; i++)
                {
                    var p = PickRandomUnmatchedPair();
                    if (p != null)
                    { p.Value.a.Wiggle(); p.Value.b.Wiggle(); }
                }
            }
        }

        /// <summary>
        /// Returns a random unmatched pair, or null if none left.
        /// </summary>
        private (MemoryCard a, MemoryCard b)? PickRandomUnmatchedPair()
        {
            var map = new Dictionary<int, List<MemoryCard>>();
            foreach (var c in cards)
            {
                if (c.IsLocked)
                    continue;
                if (!map.ContainsKey(c.pairId))
                    map[c.pairId] = new List<MemoryCard>();
                map[c.pairId].Add(c);
            }

            var candidates = new List<(MemoryCard, MemoryCard)>();
            foreach (var kv in map)
            {
                if (kv.Value.Count >= 2)
                {
                    MemoryCard a = null, b = null;
                    foreach (var c in kv.Value)
                        if (!c.IsFaceUp)
                        { if (a == null) a = c; else if (b == null) b = c; }
                    if (a != null && b != null)
                        candidates.Add((a, b));
                }
            }

            if (candidates.Count == 0)
                return null;
            return candidates[Random.Range(0, candidates.Count)];
        }

        /// <summary>
        /// Shuffles a list in place.
        /// </summary>
        private void Shuffle<T>(IList<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }

        private Vector2 CalculateAutoCellSize(int rows, int cols)
        {
            if (gridParent == null || rows <= 0 || cols <= 0)
                return cellSize;

            var rect = gridParent.rect;
            if (rect.height <= 0f || rect.width <= 0f)
                return cellSize;

            var padding = grid != null ? grid.padding : new RectOffset();
            var spacingVec = grid != null ? grid.spacing : spacing;

            float availableHeight = rect.height - padding.vertical - spacingVec.y * Mathf.Max(0, rows - 1);
            float availableWidth = rect.width - padding.horizontal - spacingVec.x * Mathf.Max(0, cols - 1);
            if (availableHeight <= 0f || availableWidth <= 0f)
                return cellSize;

            float aspect = cellSize.y > 0.01f ? cellSize.x / cellSize.y : 1f;
            float targetHeight = availableHeight / rows;
            float targetWidth = targetHeight * aspect;

            float maxWidth = availableWidth / cols;
            if (targetWidth > maxWidth)
            {
                targetWidth = maxWidth;
                targetHeight = aspect > 0f ? targetWidth / aspect : targetHeight;
            }

            targetWidth = Mathf.Max(10f, targetWidth);
            targetHeight = Mathf.Max(10f, targetHeight);

            return new Vector2(targetWidth, targetHeight);
        }
    }
}
