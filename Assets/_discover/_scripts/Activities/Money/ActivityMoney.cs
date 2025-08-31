using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Antura.Discover.Activities
{
    public class ActivityMoney : ActivityBase
    {
        [Header("Activity Money Settings")]
        public MoneySettingsData Settings;

        [Header("References")]
        public Canvas canvas;
        public Transform trayParent;
        public Transform matParent;
        [SerializeField] private RectTransform dragLayer;                 // optional overlay for drags

        public TextMeshProUGUI targetText;
        public TextMeshProUGUI currentText;
        public TextMeshProUGUI timerText;
        public RectTransform hintListParent;        // vertical layout of hint rows
        public GameObject hintRowPrefab;            // shows denomination sprite + value text

        [Header("Prefabs")]
        public GameObject moneyTokenPrefab;         // contains: Image + TMP + MoneyItemView + DraggableMoney

        [Header("Audio")]
        public AudioSource sfxSource;
        public AudioClip sfxPlace;
        public AudioClip sfxRemove;
        public AudioClip sfxWin;
        public AudioClip sfxError;

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
        private bool ended; // prevent multiple close calls

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

            // 2) Choose a solvable target: sum of 2-4 random items (with replacement)
            data.TargetAmount = Settings.UseFixedTarget
                ? Settings.FixedTargetAmount
                : BuildSolvableTarget(all, 2, 4);

            if (targetText)
                targetText.text = $"{Settings.MoneySet.CurrencySymbol} {data.TargetAmount:0.00}";

            // 3) Build a rich tray ensuring solvability with many tokens
            var baseCombo = lastComboForTarget; // from BuildSolvableTarget
            var tray = new List<MoneySet.MoneyItem>();

            // Helper to add copies but respect a soft cap
            void AddCopies(MoneySet.MoneyItem item, int copies)
            {
                int cap = Mathf.Max(1, Settings.MaxTrayTokens);
                for (int i = 0; i < copies && tray.Count < cap; i++)
                    tray.Add(item);
            }

            // Duplicate each base item several times (minimum 2)
            int copiesPerBase = Mathf.Max(2, Settings.CopiesPerBaseItem);
            foreach (var bi in baseCombo)
                AddCopies(bi, copiesPerBase);

            // Add extra copies of the smallest denomination available by difficulty
            var smallest = all.FirstOrDefault();
            if (smallest != null)
                AddCopies(smallest, Mathf.Max(0, Settings.ExtraCopiesSmallest));

            // Add a spread of distractors (other denominations) up to cap
            foreach (var d in all)
            {
                if (tray.Count >= Settings.MaxTrayTokens)
                    break;
                // Add 1-2 copies depending on difficulty
                int extra = Mathf.Clamp(difficulty, 0, 2);
                AddCopies(d, extra);
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
            Play(sfxPlace);
        }

        public void OnItemRemoved(MoneySet.MoneyItem item, DraggableMoney drag)
        {
            data.CurrentAmount -= item.Value;
            placed.Remove(drag);
            UpdateCurrentAmount(data.CurrentAmount);
            Play(sfxRemove);
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
            Play(sfxError);
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
            Play(sfxWin);
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
            Play(sfxError);
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

        private void Play(AudioClip clip)
        {
            if (sfxSource && clip)
                sfxSource.PlayOneShot(clip);
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

