using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Antura.Discover.Activities
{

    public enum MoneyKind
    {
        Both = 0,
        CoinOnly = 1,
        BanknoteOnly = 2,
    }


    /// <summary>
    /// Main controller for the Count Money activity.
    /// </summary>
    public class ActivityMoney : ActivityBase
    {
        [Header("Definition")]
        public CountMoneyDefinition definition;
        [Tooltip("Currency symbol to show with values (€, zł, etc.).")]
        public string currencySymbol = "€";

        [Header("UI References")]
        public Canvas canvas;
        public Transform trayParent;
        public Transform matParent;
        [SerializeField] private RectTransform dragLayer;                 // optional overlay for drags
        public Button confirmButton;
        public Button skipButton;
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

        private CountMoneyData data;
        private readonly List<DraggableMoney> placed = new();
        private readonly List<GameObject> spawned = new();

        private const float EPS = 0.01f;

        [Header("Layout")]
        [Tooltip("Padding from edges when scattering in tray.")]
        public float spawnScatterPadding = 20f;

        private void Start()
        {
            SetupGame();
        }

        private void Update()
        {
            if (definition.TimeLimit > 0 && data.TimeRemaining > 0)
            {
                data.TimeRemaining -= Time.deltaTime;
                if (data.TimeRemaining < 0)
                    data.TimeRemaining = 0;
                if (timerText)
                    timerText.text = Mathf.CeilToInt(data.TimeRemaining).ToString();

                if (Mathf.Approximately(data.TimeRemaining, 0))
                {
                    Lose();
                }
            }
        }

        private void OnDestroy()
        {
            CleanupSpawned();
        }

        private void SetupGame()
        {
            CleanupSpawned();

            data = new CountMoneyData
            {
                TimeRemaining = definition.TimeLimit
            };

            // 1) Build candidate pool by difficulty gate
            var all = definition.MoneySet.items
                .Where(it => (int)it.Difficulty <= difficulty)
                .OrderBy(it => it.Value)
                .ToList();

            // Fallback if none matched
            if (all.Count == 0)
                all = definition.MoneySet.items.OrderBy(it => it.Value).ToList();

            // 2) Choose a solvable target: sum of 2-4 random items (with replacement)
            data.TargetAmount = definition.UseFixedTarget
                ? definition.FixedTargetAmount
                : BuildSolvableTarget(all, 2, 4);

            if (targetText)
                targetText.text = $"{currencySymbol} {data.TargetAmount:0.00}";

            // 3) Build tray with a base set + distractors
            var baseCombo = lastComboForTarget; // from BuildSolvableTarget
            var tray = new List<MoneySet.MoneyItem>(baseCombo);

            int distractorCount = Mathf.Clamp(difficulty, 0, Mathf.Max(0, all.Count - tray.Count));
            var distractors = all.Except(tray).OrderBy(_ => prng.Next()).Take(distractorCount);
            tray.AddRange(distractors);

            // Shuffle tray
            tray = tray.OrderBy(_ => prng.Next()).ToList();
            data.AvailableItems = tray;

            // 4) Spawn tokens in tray
            foreach (var it in tray)
            {
                var go = Instantiate(moneyTokenPrefab, trayParent);
                spawned.Add(go);
                var view = go.GetComponent<MoneyItemView>();
                var drag = go.GetComponent<DraggableMoney>();
                view.Setup(it, currencySymbol);
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
            if (confirmButton)
            {
                confirmButton.onClick.RemoveAllListeners();
                confirmButton.onClick.AddListener(Confirm);
                confirmButton.interactable = false;
            }
            if (skipButton)
            {
                skipButton.onClick.RemoveAllListeners();
                skipButton.onClick.AddListener(Skip);
            }
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
                currentText.text = $"{currencySymbol} {data.CurrentAmount:0.00}";
            if (confirmButton)
            {
                bool match = Mathf.Abs(data.CurrentAmount - data.TargetAmount) <= EPS;
                bool any = placed.Count > 0;
                confirmButton.interactable = match && any;
            }
        }

        private void Confirm()
        {
            bool match = Mathf.Abs(data.CurrentAmount - data.TargetAmount) <= EPS;
            if (match)
                Win();
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
            DoValidate();

        }

        public override bool DoValidate()
        {
            return true;
        }

        private void Lose()
        {
            Play(sfxError);
            Debug.Log("[CountMoney] LOSE (time)");
            if (confirmButton)
                confirmButton.interactable = false;
        }

        private void Skip()
        {
            // TODO: apply -1 point via your BonusMalus or Score system
            Debug.Log("[CountMoney] SKIP (apply penalty)");
            // Optionally immediately end:
            Lose();
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
                    view.Setup(item, currencySymbol);
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

