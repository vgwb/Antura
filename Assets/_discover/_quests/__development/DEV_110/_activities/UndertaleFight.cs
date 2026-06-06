using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

namespace Antura.Discover.DEV110
{
    /// <summary>
    /// Standalone, Yarn-driven Undertale-style battle for the DEV_110 quest prototype.
    ///
    /// The script owns the battle UI, the ACT/ITEM/MERCY menu, player HP and the bullet-hell dodge
    /// phase. All content (which acts exist, what they say, which one is "correct", when the player
    /// may spare) is authored in the Yarn script and pushed to the fight through the <c>fight_*</c>
    /// commands below.
    ///
    /// Typical Yarn turn (one ACT branch):
    /// <code>
    /// title: Act_Reason
    /// ---
    /// Dante tries to reason with the beast... #line:xxxx
    /// &lt;&lt;fight_dodge "lynx_pounce"&gt;&gt;
    /// &lt;&lt;if fight_hp() &lt;= 0&gt;&gt;
    ///     &lt;&lt;fight_end "lose"&gt;&gt;
    /// &lt;&lt;else&gt;&gt;
    ///     The beast hesitates. #line:yyyy
    ///     &lt;&lt;fight_spare_enable&gt;&gt;
    ///     &lt;&lt;fight_menu&gt;&gt;
    /// &lt;&lt;endif&gt;&gt;
    /// ===
    /// </code>
    /// </summary>
    public class UndertaleFight : MonoBehaviour
    {
        /// <summary>The fight currently targeted by the Yarn <c>fight_*</c> commands.</summary>
        public static UndertaleFight Current { get; private set; }

        public enum FightState { Inactive, Menu, ActSelect, Dialogue, Dodge, LoseChoice, Ended }

        [Header("Player")]
        public int MaxHP = 20;

        [Header("ACT options (each maps to a Yarn node)")]
        public List<ActOption> ActOptions = new List<ActOption>();

        [Header("Dodge waves")]
        public List<DodgeWave> DodgeWaves = new List<DodgeWave>();

        [Header("Item")]
        [Tooltip("HP restored by the ITEM button.")]
        public int ItemHealAmount = 5;
        [Tooltip("How many times ITEM can be used. -1 = unlimited.")]
        public int ItemUses = 1;

        [Header("End nodes (optional Yarn nodes)")]
        [Tooltip("Played when the player spares / wins. Leave empty to just return to the world.")]
        public string WinNode = "";
        [Tooltip("Played when the player's HP reaches 0. Leave empty to just return to the world.")]
        public string LoseNode = "";

        [Header("Behaviour")]
        [Tooltip("Lock world movement (enter Dialogue gameplay state) while the fight is on screen.")]
        public bool LockWorldInputDuringFight = true;

        [Header("UI - Root & HP")]
        public GameObject BattleRoot;
        public Image HPFill;
        public TextMeshProUGUI HPLabel;

        [Header("UI - Main menu")]
        public GameObject MainMenu;
        public Button ActButton;
        public Button ItemButton;
        public Button MercyButton;

        [Header("UI - ACT submenu")]
        public GameObject ActMenu;
        public Transform ActOptionsContainer;
        [Tooltip("Disabled button used as a template; cloned once per ACT option.")]
        public Button ActOptionTemplate;
        public Button ActBackButton;

        [Header("UI - Lose menu")]
        [Tooltip("Panel shown when the player loses, offering Try Again / Exit Battle. If unassigned, losing falls back to the immediate lose flow.")]
        public GameObject LoseMenu;
        public Button TryAgainButton;
        public Button ExitBattleButton;

        [Header("UI - Dodge")]
        public RectTransform DodgeBox;
        public FightSoul Soul;
        [Tooltip("Optional container for spawned bullets. Defaults to the dodge box.")]
        public RectTransform BulletContainer;

        private FightState state = FightState.Inactive;
        private int currentHP;
        private bool spareEnabled;
        private int itemUsesLeft;
        private readonly List<Button> spawnedActButtons = new List<Button>();
        private readonly List<FightBullet> liveBullets = new List<FightBullet>();

        public FightState State => state;
        public int CurrentHP => currentHP;

        // =====================================================================
        // Yarn bridge — static commands delegate to the active fight instance.
        // (Auto-registered by the Yarn source generator, like YarnAnturaManager.)
        // =====================================================================

        [YarnCommand("fight_start")]
        public static void Cmd_Start() => Current?.Begin();

        [YarnCommand("fight_menu")]
        public static void Cmd_Menu() => Current?.ShowMenu();

        [YarnCommand("fight_dodge")]
        public static IEnumerator Cmd_Dodge(string waveId)
        {
            if (Current == null)
                yield break;
            yield return Current.RunDodge(waveId);
        }

        [YarnCommand("fight_damage")]
        public static void Cmd_Damage(int amount) => Current?.ApplyDamage(amount);

        [YarnCommand("fight_heal")]
        public static void Cmd_Heal(int amount) => Current?.Heal(amount);

        [YarnCommand("fight_spare_enable")]
        public static void Cmd_SpareEnable() => Current?.EnableSpare();

        [YarnCommand("fight_end")]
        public static void Cmd_End(string result = "win") => Current?.EndFight(result != "lose");

        [YarnFunction("fight_hp")]
        public static int Fn_Hp() => Current != null ? Current.currentHP : 0;

        [YarnFunction("fight_can_spare")]
        public static bool Fn_CanSpare() => Current != null && Current.spareEnabled;

        // =====================================================================
        // Lifecycle
        // =====================================================================

        private void Awake()
        {
            Current = this;
            WireButtons();
            HideAll();
        }

        private void OnEnable()
        {
            Current = this;
            if (Soul != null)
                Soul.OnHit += OnSoulHit;
        }

        private void OnDisable()
        {
            if (Soul != null)
                Soul.OnHit -= OnSoulHit;
            if (Current == this)
                Current = null;
        }

        private void Update()
        {
            // The Discover game manager reverts to Play3D whenever a dialogue node ends
            // (DiscoverGameManager.OnYarnDialogueComplete). While the battle is on screen we keep
            // re-asserting the Dialogue state so the player can't walk around behind the menu.
            // ChangeState is a no-op once already in Dialogue, so this is cheap.
            if (!LockWorldInputDuringFight)
                return;
            if (state == FightState.Inactive || state == FightState.Ended)
                return;
            DiscoverGameManager.I?.ChangeState(GameplayState.Dialogue, true);
        }

        private void WireButtons()
        {
            if (ActButton != null)
            { ActButton.onClick.RemoveListener(OpenActMenu); ActButton.onClick.AddListener(OpenActMenu); }
            if (ItemButton != null)
            { ItemButton.onClick.RemoveListener(UseItem); ItemButton.onClick.AddListener(UseItem); }
            if (MercyButton != null)
            { MercyButton.onClick.RemoveListener(TryMercy); MercyButton.onClick.AddListener(TryMercy); }
            if (ActBackButton != null)
            { ActBackButton.onClick.RemoveListener(ShowMenu); ActBackButton.onClick.AddListener(ShowMenu); }
            if (TryAgainButton != null)
            { TryAgainButton.onClick.RemoveListener(OnTryAgain); TryAgainButton.onClick.AddListener(OnTryAgain); }
            if (ExitBattleButton != null)
            { ExitBattleButton.onClick.RemoveListener(OnExitBattle); ExitBattleButton.onClick.AddListener(OnExitBattle); }
            if (ActOptionTemplate != null)
                ActOptionTemplate.gameObject.SetActive(false);
        }

        // =====================================================================
        // Fight flow
        // =====================================================================

        /// <summary>Start the battle: reset state and show the main menu.</summary>
        public void Begin()
        {
            currentHP = MaxHP;
            spareEnabled = false;
            itemUsesLeft = ItemUses;
            foreach (var opt in ActOptions)
                if (opt != null)
                    opt.Used = false;

            if (LockWorldInputDuringFight)
                DiscoverGameManager.I?.ChangeState(GameplayState.Dialogue, true);

            if (BattleRoot != null)
                BattleRoot.SetActive(true);

            UpdateHP();
            ShowMenu();
        }

        /// <summary>Show the ACT / ITEM / MERCY menu (the player's turn).</summary>
        public void ShowMenu()
        {
            if (state == FightState.Ended)
                return;

            state = FightState.Menu;
            SetDodgeActive(false);
            if (ActMenu != null)
                ActMenu.SetActive(false);
            if (MainMenu != null)
                MainMenu.SetActive(true);

            if (MercyButton != null)
                MercyButton.interactable = spareEnabled;
            if (ItemButton != null)
                ItemButton.interactable = ItemUses < 0 || itemUsesLeft > 0;
        }

        private void OpenActMenu()
        {
            if (state != FightState.Menu)
                return;

            state = FightState.ActSelect;
            if (MainMenu != null)
                MainMenu.SetActive(false);
            if (ActMenu != null)
                ActMenu.SetActive(true);

            BuildActButtons();
        }

        private void BuildActButtons()
        {
            // Clear previously spawned buttons
            foreach (var b in spawnedActButtons)
                if (b != null)
                    Destroy(b.gameObject);
            spawnedActButtons.Clear();

            if (ActOptionTemplate == null || ActOptionsContainer == null)
            {
                Debug.LogWarning("UndertaleFight: ActOptionTemplate / ActOptionsContainer not assigned.");
                return;
            }

            foreach (var option in ActOptions)
            {
                if (option == null)
                    continue;
                if (option.DisabledAfterUse && option.Used)
                    continue;

                var btn = Instantiate(ActOptionTemplate, ActOptionsContainer);
                btn.gameObject.SetActive(true);
                SetButtonLabel(btn, option.Label);
                var captured = option;
                btn.onClick.AddListener(() => OnActChosen(captured));
                spawnedActButtons.Add(btn);
            }
        }

        private void OnActChosen(ActOption option)
        {
            if (state != FightState.ActSelect)
                return;
            if (string.IsNullOrEmpty(option.YarnNode))
            {
                Debug.LogWarning($"UndertaleFight: ACT option '{option.Label}' has no Yarn node assigned.");
                return;
            }

            option.Used = true;
            state = FightState.Dialogue;
            if (ActMenu != null)
                ActMenu.SetActive(false);
            if (MainMenu != null)
                MainMenu.SetActive(false);

            // The Yarn node now drives the turn; it must call <<fight_menu>> or <<fight_end ...>> when done.
            YarnAnturaManager.I?.StartDialogue(option.YarnNode);
        }

        private void UseItem()
        {
            if (state != FightState.Menu)
                return;
            if (ItemUses >= 0 && itemUsesLeft <= 0)
                return;

            if (ItemUses >= 0)
                itemUsesLeft--;
            Heal(ItemHealAmount);
            ShowMenu(); // refresh button states
        }

        private void TryMercy()
        {
            if (state != FightState.Menu)
                return;
            if (!spareEnabled)
                return;
            EndFight(true);
        }

        public void EnableSpare()
        {
            spareEnabled = true;
            if (MercyButton != null && state == FightState.Menu)
                MercyButton.interactable = true;
        }

        // =====================================================================
        // Dodge phase
        // =====================================================================

        private IEnumerator RunDodge(string waveId)
        {
            var wave = DodgeWaves.Find(w => w != null && w.Id == waveId);
            if (wave == null)
            {
                Debug.LogWarning($"UndertaleFight: dodge wave '{waveId}' not found.");
                yield break;
            }
            if (DodgeBox == null || Soul == null)
            {
                Debug.LogWarning("UndertaleFight: dodge wave missing DodgeBox / Soul.");
                yield break;
            }
            if (wave.Pattern == null && wave.BulletPrefab == null)
            {
                Debug.LogWarning($"UndertaleFight: dodge wave '{waveId}' has neither a Pattern nor a BulletPrefab.");
                yield break;
            }

            state = FightState.Dodge;
            SetDodgeActive(true);
            Soul.ResetToCenter();

            var container = BulletContainer != null ? BulletContainer : DodgeBox;

            if (wave.Pattern != null)
            {
                // Authored pattern: shares maths with the editor preview; needs no bullet prefab.
                yield return ProjectilePatternRunner.Play(
                    wave.Pattern, DodgeBox, container, Soul,
                    b => liveBullets.Add(b),
                    () => currentHP > 0);
            }
            else
            {
                // Legacy simple spawner.
                float elapsed = 0f;
                float sinceSpawn = wave.SpawnInterval; // spawn immediately on first frame
                while (elapsed < wave.Duration && currentHP > 0)
                {
                    elapsed += Time.deltaTime;
                    sinceSpawn += Time.deltaTime;
                    if (sinceSpawn >= wave.SpawnInterval)
                    {
                        sinceSpawn = 0f;
                        for (int i = 0; i < Mathf.Max(1, wave.BulletsPerSpawn); i++)
                            SpawnBullet(wave, container);
                    }
                    yield return null;
                }
            }

            // Let remaining bullets fly out (briefly), then clear.
            float clearTimeout = 2f;
            while (clearTimeout > 0f && currentHP > 0 && HasLiveBullets())
            {
                clearTimeout -= Time.deltaTime;
                yield return null;
            }
            ClearBullets();
            SetDodgeActive(false);

            if (currentHP <= 0)
            {
                EndFight(false);
                yield break;
            }

            // Back to dialogue; the Yarn node continues and decides what happens next.
            if (state == FightState.Dodge)
                state = FightState.Dialogue;
        }

        private void SpawnBullet(DodgeWave wave, RectTransform container)
        {
            Vector2 half = DodgeBox.rect.size * 0.5f;
            Vector2 start;
            Vector2 dir;
            ComputeSpawn(wave.Shape, half, out start, out dir);

            var bullet = Instantiate(wave.BulletPrefab, container);
            bullet.gameObject.SetActive(true);
            float lifetime = (half.magnitude * 2f) / Mathf.Max(1f, wave.Speed) + 1.5f;
            bullet.Launch(start, dir * wave.Speed, Soul, DodgeBox, wave.DamagePerHit, lifetime);
            liveBullets.Add(bullet);
        }

        private void ComputeSpawn(SpawnShape shape, Vector2 half, out Vector2 start, out Vector2 dir)
        {
            switch (shape)
            {
                case SpawnShape.TopDown:
                    start = new Vector2(Random.Range(-half.x, half.x), half.y);
                    dir = Vector2.down;
                    break;
                case SpawnShape.Sides:
                    bool left = Random.value < 0.5f;
                    start = new Vector2(left ? -half.x : half.x, Random.Range(-half.y, half.y));
                    dir = left ? Vector2.right : Vector2.left;
                    break;
                default: // RandomEdges: spawn on a random edge aimed roughly at the soul
                    int edge = Random.Range(0, 4);
                    switch (edge)
                    {
                        case 0: start = new Vector2(Random.Range(-half.x, half.x), half.y); break;   // top
                        case 1: start = new Vector2(Random.Range(-half.x, half.x), -half.y); break;  // bottom
                        case 2: start = new Vector2(-half.x, Random.Range(-half.y, half.y)); break;   // left
                        default: start = new Vector2(half.x, Random.Range(-half.y, half.y)); break;   // right
                    }
                    Vector2 target = Soul != null ? Soul.Rect.anchoredPosition : Vector2.zero;
                    dir = (target - start).sqrMagnitude > 0.01f ? (target - start).normalized : -start.normalized;
                    break;
            }
        }

        private bool HasLiveBullets()
        {
            liveBullets.RemoveAll(b => b == null);
            return liveBullets.Count > 0;
        }

        private void ClearBullets()
        {
            foreach (var b in liveBullets)
                if (b != null)
                    Destroy(b.gameObject);
            liveBullets.Clear();
        }

        private void SetDodgeActive(bool active)
        {
            if (DodgeBox != null)
                DodgeBox.gameObject.SetActive(active);
            if (Soul != null)
                Soul.gameObject.SetActive(active);
        }

        // =====================================================================
        // HP & end
        // =====================================================================

        private void OnSoulHit(int damage) => ApplyDamage(damage);

        public void ApplyDamage(int amount)
        {
            if (state == FightState.Ended)
                return;
            currentHP = Mathf.Max(0, currentHP - Mathf.Abs(amount));
            UpdateHP();
            // Death during the dodge loop is handled there; death from a scripted <<fight_damage>>
            // outside a wave ends the fight immediately.
            if (currentHP <= 0 && state != FightState.Dodge)
                EndFight(false);
        }

        public void Heal(int amount)
        {
            if (state == FightState.Ended)
                return;
            currentHP = Mathf.Min(MaxHP, currentHP + Mathf.Abs(amount));
            UpdateHP();
        }

        private void UpdateHP()
        {
            if (HPFill != null)
                HPFill.fillAmount = MaxHP > 0 ? (float)currentHP / MaxHP : 0f;
            if (HPLabel != null)
                HPLabel.text = $"{currentHP}/{MaxHP}";
        }

        /// <summary>
        /// Finish or, on a loss, offer the Try Again / Exit Battle choice.
        /// <paramref name="win"/> true = spared/won, false = lost.
        /// </summary>
        public void EndFight(bool win)
        {
            if (state == FightState.Ended)
                return;
            // On a loss, give the player the retry/exit choice instead of ending immediately.
            // Falls back to the old behaviour when no lose menu is wired up.
            if (!win && LoseMenu != null)
            {
                ShowLoseMenu();
                return;
            }
            FinishFight(win);
        }

        /// <summary>Show the lose menu (Try Again / Exit Battle). Keeps the world locked.</summary>
        private void ShowLoseMenu()
        {
            StopAllCoroutines();
            ClearBullets();
            SetDodgeActive(false);
            if (MainMenu != null)
                MainMenu.SetActive(false);
            if (ActMenu != null)
                ActMenu.SetActive(false);
            if (BattleRoot != null)
                BattleRoot.SetActive(true);
            if (LoseMenu != null)
                LoseMenu.SetActive(true);

            state = FightState.LoseChoice;
        }

        private void OnTryAgain()
        {
            if (state != FightState.LoseChoice)
                return;
            if (LoseMenu != null)
                LoseMenu.SetActive(false);
            Begin();
        }

        private void OnExitBattle()
        {
            if (state != FightState.LoseChoice)
                return;
            if (LoseMenu != null)
                LoseMenu.SetActive(false);
            FinishFight(false);
        }

        /// <summary>Tear down the fight and hand control back to Yarn / the world.</summary>
        private void FinishFight(bool win)
        {
            if (state == FightState.Ended)
                return;
            state = FightState.Ended;

            StopAllCoroutines();
            ClearBullets();
            HideAll();

            string endNode = win ? WinNode : LoseNode;
            if (!string.IsNullOrEmpty(endNode))
            {
                // StartDialogueAgain stops any running dialogue first, then plays the end node.
                YarnAnturaManager.I?.StartDialogueAgain(endNode);
            }
            else if (LockWorldInputDuringFight)
            {
                DiscoverGameManager.I?.ChangeState(GameplayState.Play3D, true);
            }
        }

        private void HideAll()
        {
            SetDodgeActive(false);
            if (ActMenu != null)
                ActMenu.SetActive(false);
            if (MainMenu != null)
                MainMenu.SetActive(false);
            if (LoseMenu != null)
                LoseMenu.SetActive(false);
            if (BattleRoot != null)
                BattleRoot.SetActive(false);
        }

        // =====================================================================
        // Helpers
        // =====================================================================

        private static void SetButtonLabel(Button button, string label)
        {
            if (button == null)
                return;
            var tmp = button.GetComponentInChildren<TextMeshProUGUI>(true);
            if (tmp != null)
            {
                tmp.text = label;
                return;
            }
            var legacy = button.GetComponentInChildren<Text>(true);
            if (legacy != null)
                legacy.text = label;
        }
    }
}
