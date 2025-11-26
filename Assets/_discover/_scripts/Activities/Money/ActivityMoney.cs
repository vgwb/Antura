using Antura.Discover.Audio;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Antura.Discover.Activities
{
    public class ActivityMoney : ActivityBase, IActivity
    {
        [Header("Activity Money Settings")]
        public MoneySettingsData Settings;

        [Header("References")]
        public Canvas canvas;
        public Transform trayParent;
        public Transform matParent;
        [SerializeField] private RectTransform dragLayer;                 // optional overlay for drags

        public TextMeshProUGUI currentText;
        public RectTransform hintListParent;        // vertical layout of hint rows
        public GameObject hintRowPrefab;            // shows denomination sprite + value text

        [Header("Prefabs")]
        public GameObject moneyTokenPrefab;         // contains: Image + TMP + MoneyItemView + DraggableMoney

        [Header("Feedback")]
        public RectTransform shakeTarget;           // e.g., the mat or the whole board
        public float shakeDuration = 0.25f;
        public float shakeStrength = 12f;

        [Header("Debug/Info")]
        public int difficulty = 1;                 // 0..definition.MaxDifficulty
        public System.Random prng = new System.Random();

        private MoneyData data;
        private readonly List<DraggableMoney> placed = new();
        private readonly List<GameObject> spawned = new();

        private const float EPS = 0.01f;
        private bool ended;

        [Header("Layout")]
        [Tooltip("Padding from edges when scattering in tray.")]
        public float spawnScatterPadding = 20f;

        [Tooltip("Global scale")]
        public float globalScale = 0.6f;

        public override void ConfigureSettings(ActivitySettingsAbstract settings)
        {
            base.ConfigureSettings(settings);
            if (settings is MoneySettingsData csd)
                Settings = csd;
        }

        public override void InitActivity()
        {
            SetupGame();
        }

        protected override void Update()
        {
            base.Update();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            CleanupSpawned();
        }

        private void SetupGame()
        {
            CleanupSpawned();
            ended = false;

            data = new MoneyData();

            // 1) Build candidate pool by difficulty gate
            var all = Settings.MoneySet.items
                .Where(it => (int)it.Difficulty <= difficulty)
                .OrderBy(it => it.Value)
                .ToList();

            // Fallback if none matched
            if (all.Count == 0)
                all = Settings.MoneySet.items.OrderBy(it => it.Value).ToList();

            // 2) Choose a solvable target
            if (Settings.UseFixedTarget)
            {
                data.TargetAmount = Settings.FixedTargetAmount;
                // We need to find a combination that sums to this target to ensure solvability
                lastComboForTarget = GetCombinationForTarget(data.TargetAmount, all);
            }
            else
            {
                data.TargetAmount = BuildSolvableTarget(all, 2, 4);
            }

            base.DisplayFeedback($"{Settings.MoneySet.CurrencySymbol} {data.TargetAmount:0.00}");

            // 3) Build tray based on difficulty
            var tray = new List<MoneySet.MoneyItem>();
            var solution = lastComboForTarget; // The coins needed to solve it

            // Helper to add items respecting cap
            void AddToTray(IEnumerable<MoneySet.MoneyItem> items)
            {
                foreach (var item in items)
                {
                    if (tray.Count >= Settings.GenMaxTokens)
                        break;
                    tray.Add(item);
                }
            }

            if (Settings.GenType == MoneyGenerationType.Manual)
            {
                if (Settings.ManualComposition != null)
                {
                    foreach (var entry in Settings.ManualComposition)
                    {
                        var match = Settings.MoneySet.items.FirstOrDefault(x => Mathf.Abs(x.Value - entry.Value) < 0.001f && x.Type == entry.Type);
                        if (match != null)
                        {
                            for (int i = 0; i < entry.Count; i++)
                            {
                                AddToTray(new[] { match });
                            }
                        }
                    }
                }
            }
            else
            {
                // Base: always add the solution first (so it's solvable)
                AddToTray(solution);

                var smallest = all.OrderBy(x => x.Value).FirstOrDefault();

                switch (difficulty)
                {
                    case 0: // Tutorial: Exact amount
                        // Already added solution
                        break;

                    case 1: // Easy: Exact + few smallest
                        if (smallest != null)
                        {
                            for (int i = 0; i < 3; i++)
                                AddToTray(new[] { smallest });
                        }
                        break;

                    case 2: // Normal: Double solution + minor coins
                        AddToTray(solution); // Double the solution coins
                        if (smallest != null)
                        {
                            for (int i = 0; i < 3; i++)
                                AddToTray(new[] { smallest });
                        }
                        // Add some random distractors
                        for (int i = 0; i < 2; i++)
                        {
                            var randomItem = all[prng.Next(all.Count)];
                            AddToTray(new[] { randomItem });
                        }
                        break;

                    case 3: // Expert: Triple solution + more distractors
                    default:
                        AddToTray(solution);
                        AddToTray(solution);
                        // Add more random distractors
                        for (int i = 0; i < 5; i++)
                        {
                            var randomItem = all[prng.Next(all.Count)];
                            AddToTray(new[] { randomItem });
                        }
                        break;
                }
            }

            // Shuffle tray and assign
            tray = tray.OrderBy(_ => prng.Next()).ToList();
            data.AvailableItems = tray;

            // 4) Spawn tokens in tray
            foreach (var it in tray)
            {
                var go = Instantiate(moneyTokenPrefab, trayParent);
                spawned.Add(go);
                var view = go.GetComponent<MoneyItemView>();
                var drag = go.GetComponent<DraggableMoney>();
                view.Setup(it, Settings.MoneySet.CurrencySymbol, globalScale);
                if (drag != null)
                {
                    drag.canvas = canvas;
                    drag.dragRoot = dragLayer ? dragLayer : null;
                }
                // Scatter initial position in tray
                var rt = go.GetComponent<RectTransform>();
                ScatterInRect(rt, (RectTransform)trayParent);
            }

            // 5) Hint table: sorted unique denominations ascending
            BuildHintTable(all);

            // 6) UI wiring
            UpdateCurrentAmount(0f);
        }

        private void CleanupSpawned()
        {
            foreach (var g in spawned)
                if (g)
                    Destroy(g);
            spawned.Clear();
            placed.Clear();
        }

        // ---------- Amount & Win/Lose ----------

        public void OnItemPlaced(MoneySet.MoneyItem item, DraggableMoney drag)
        {
            data.CurrentAmount += item.Value;
            placed.Add(drag);
            UpdateCurrentAmount(data.CurrentAmount);
            DiscoverAudioManager.I.PlaySfx(DiscoverSfx.ActivityAttach);
        }

        public void OnItemRemoved(MoneySet.MoneyItem item, DraggableMoney drag)
        {
            data.CurrentAmount -= item.Value;
            placed.Remove(drag);
            UpdateCurrentAmount(data.CurrentAmount);
            DiscoverAudioManager.I.PlaySfx(DiscoverSfx.ActivityDetach);
        }

        private void UpdateCurrentAmount(float amount)
        {
            if (currentText)
                currentText.text = $"{Settings.MoneySet.CurrencySymbol} {data.CurrentAmount:0.00}";

            bool match = Mathf.Abs(data.CurrentAmount - data.TargetAmount) <= EPS;
            bool any = placed.Count > 0;
            EnableValidateButton(match && any);

        }

        private void Confirm()
        {
            bool match = Mathf.Abs(data.CurrentAmount - data.TargetAmount) <= EPS;
            if (match)
            {
                if (!ended)
                {
                    ended = true;
                    EndRound(true, 1f, false);
                }
            }
            else
                StartCoroutine(ShakeAndError());
        }

        private IEnumerator ShakeAndError()
        {
            DiscoverAudioManager.I.PlaySfx(DiscoverSfx.ActivityBadMove);
            if (shakeTarget == null)
                yield break;

            Vector3 origin = shakeTarget.anchoredPosition;
            float t = 0f;
            while (t < shakeDuration)
            {
                t += Time.deltaTime;
                float dx = Mathf.Sin(t * 60f) * shakeStrength * (1f - t / shakeDuration);
                shakeTarget.anchoredPosition = origin + new Vector3(dx, 0f, 0f);
                yield return null;
            }
            shakeTarget.anchoredPosition = origin;
        }

        private void Win()
        {
            DiscoverAudioManager.I.PlaySfx(DiscoverSfx.ActivitySuccess);
            // TODO: fire event to AchievementsManager / progression
            Debug.Log("[CountMoney] WIN");
            // Lock input if you want:
            if (!ended)
            {
                ended = true;
                EndRound(true, 1f, false);
            }

        }

        public override bool DoValidate()
        {
            // Success when the current amount exactly matches the target (within EPS)
            return Mathf.Abs(data.CurrentAmount - data.TargetAmount) <= EPS;
        }

        private void Lose()
        {
            DiscoverAudioManager.I.PlaySfx(DiscoverSfx.ActivityFail);
            Debug.Log("[CountMoney] LOSE (time)");
            ended = true;
            EndRound(false, 0f, true);
        }

        private void Skip()
        {
            // TODO: apply -1 point via BonusMalus or Score system
            Debug.Log("[CountMoney] SKIP (apply penalty)");
            // End the round as a fail via base flow
            if (!ended)
            {
                ended = true;
                EndRound(false, 0f, false);
            }
        }

        // ---------- Hints ----------

        private void BuildHintTable(List<MoneySet.MoneyItem> all)
        {
            if (hintListParent == null || hintRowPrefab == null)
                return;

            // Clear
            for (int i = hintListParent.childCount - 1; i >= 0; i--)
                Destroy(hintListParent.GetChild(i).gameObject);

            // Unique denominations asc
            foreach (var g in all
                .GroupBy(x => x.Value)
                .OrderBy(gp => gp.Key))
            {
                var item = g.First();
                var row = Instantiate(hintRowPrefab, hintListParent);
                var view = row.GetComponent<MoneyItemView>();
                if (view)
                    view.Setup(item, Settings.MoneySet.CurrencySymbol, globalScale);
            }
        }

        // ---------- Target generation (solvable) ----------

        private List<MoneySet.MoneyItem> lastComboForTarget = new();

        private List<MoneySet.MoneyItem> GetCombinationForTarget(float target, List<MoneySet.MoneyItem> pool)
        {
            // 1. Filter by difficulty
            var validItems = pool
                .Where(it => (int)it.Difficulty <= difficulty)
                .OrderByDescending(x => x.Value)
                .ToList();

            if (validItems.Count == 0)
                validItems = pool.OrderByDescending(x => x.Value).ToList();

            // 2. Try to find a combination (with retries for randomness)
            // We try multiple times to find a valid combination because the random choices might lead to a dead end
            // or we just want to ensure we can solve it.
            for (int attempt = 0; attempt < 20; attempt++)
            {
                var currentCombo = new List<MoneySet.MoneyItem>();
                float remaining = target;
                bool failed = false;

                // Safety break
                int safety = 0;
                while (remaining > 0.001f && safety++ < 100)
                {
                    // Find candidates that fit
                    var candidates = validItems.Where(x => x.Value <= remaining + 0.001f).ToList();
                    if (candidates.Count == 0)
                    {
                        failed = true;
                        break;
                    }

                    // Pick one.
                    // To ensure we don't just pick 1s, prefer larger coins.
                    // Strategy: Pick randomly from the top 3 candidates (largest values).
                    int topN = Mathf.Min(candidates.Count, 3);
                    // candidates are already sorted descending because validItems is sorted descending
                    var pick = candidates[prng.Next(0, topN)];

                    currentCombo.Add(pick);
                    remaining -= pick.Value;
                }

                if (!failed && Mathf.Abs(remaining) <= 0.001f)
                {
                    return currentCombo;
                }
            }

            // Fallback to strict greedy if random attempts fail
            return GetGreedyCombination(target, validItems);
        }

        private List<MoneySet.MoneyItem> GetGreedyCombination(float target, List<MoneySet.MoneyItem> pool)
        {
            var result = new List<MoneySet.MoneyItem>();
            // Pool is assumed to be sorted descending
            float current = 0f;

            foreach (var item in pool)
            {
                while (current + item.Value <= target + 0.001f)
                {
                    current += item.Value;
                    result.Add(item);
                }
            }

            // Fill with smallest if needed
            if (Mathf.Abs(target - current) > 0.001f)
            {
                var smallest = pool.LastOrDefault(); // validItems is sorted descending, so last is smallest
                if (smallest != null)
                {
                    while (current + smallest.Value <= target + 0.001f)
                    {
                        current += smallest.Value;
                        result.Add(smallest);
                    }
                }
            }

            return result;
        }
        private float BuildSolvableTarget(List<MoneySet.MoneyItem> pool, int minCount, int maxCount)
        {
            // Try a few times to assemble a simple sum
            for (int attempt = 0; attempt < 50; attempt++)
            {
                int count = prng.Next(minCount, maxCount + 1);
                lastComboForTarget.Clear();
                float sum = 0f;
                for (int i = 0; i < count; i++)
                {
                    var pick = pool[prng.Next(0, pool.Count)];
                    lastComboForTarget.Add(pick);
                    sum += pick.Value;
                }
                sum = Mathf.Round(sum * 100f) / 100f;
                if (sum > 0.001f)
                    return sum;
            }

            // Fallback: minimal denomination
            lastComboForTarget.Clear();
            var min = pool.OrderBy(p => p.Value).First();
            lastComboForTarget.Add(min);
            return Mathf.Round(min.Value * 100f) / 100f;
        }

        private void ScatterInRect(RectTransform item, RectTransform area)
        {
            if (!item || !area)
                return;
            // Ensure center pivot for consistent positioning
            item.anchorMin = item.anchorMax = new Vector2(0.5f, 0.5f);
            item.pivot = new Vector2(0.5f, 0.5f);

            var size = area.rect.size;
            float pad = Mathf.Max(0f, spawnScatterPadding);
            float x = Random.Range(-size.x * 0.5f + pad, size.x * 0.5f - pad);
            float y = Random.Range(-size.y * 0.5f + pad, size.y * 0.5f - pad);
            item.anchoredPosition = new Vector2(x, y);
        }
    }
}

