using Antura.Core;
using Antura.Profile;
using Antura.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Discover
{
    public struct ActivityEnd
    {
        public string activityId;
        public int score;
        public int durationSec;
        public string topic;
        public int attempts;
        public int correct;
        public Dictionary<string, int> wrongItems; // optional/sparse
    }

    public struct QuestEnd
    {
        public string questId;
        public int score;
        public int durationSec;
        public int stars; // 0..3
        public List<ActivityEnd> activities; // optional
    }

    public class DiscoverAppManager : SingletonMonoBehaviour<DiscoverAppManager>
    {
        [Header("Economy (milestones & quest star caps)")]
        [SerializeField] private EconomySettings economySettings;

        [Header("Storage")]
        [SerializeField] private string storageSubdir = "discover_profiles";

        // Store for JSON files + tiny PlayerPrefs pointer
        private DiscoverProfileManager profilesManager;

        // Currently loaded Discover profile (null until Initialize... is called)
        public DiscoverPlayerProfile CurrentProfile { get; private set; }

        // Profile service (v7 helpers)
        private ProfileService _profileSvc;
        private ProfileService Svc
        {
            get
            {
                if (_profileSvc == null && CurrentProfile != null)
                    _profileSvc = new ProfileService(CurrentProfile, economySettings);
                return _profileSvc;
            }
        }

        // Auto-save debounce (seconds)
        [SerializeField] private float saveDebounceSeconds = 60f;
        private bool dirty;
        private Coroutine saveCo;

        // ------------------------------
        // Events for UI / systems
        // ------------------------------
        /// <summary>Raised after a profile is loaded/created/switches.</summary>
        public event Action<DiscoverPlayerProfile> OnProfileLoaded;

        /// <summary>Raised when an achievement becomes unlocked (id, at UTC ISO time).</summary>
        public event Action<string, string> OnAchievementUnlocked;

        /// <summary>Raised when cookies/gems/points change (for HUD badges, etc.).</summary>
        public event Action OnCurrencyChanged;

        /// <summary>Raised when gems are awarded through the ledger (delta, newly added tokens).</summary>
        public event Action<int, GemTokenClaim[]> OnGemsAwarded;

        // ------------------------------
        // Lifecycle
        // ------------------------------
        protected override void Init()
        {
            DontDestroyOnLoad(this);
            profilesManager = new DiscoverProfileManager(storageSubdir);
            PlayerProfileManager.OnProfileChanged += OldProfilePlayerChanged;
        }

        private void OnDestroy()
        {
            PlayerProfileManager.OnProfileChanged -= OldProfilePlayerChanged;
        }

        private void Start()
        {
            // Initialize with the current legacy profile (if any)
            if (AppManager.I.PlayerProfileManager.CurrentPlayer != null)
            {
                InitializeFromLegacyUuid(AppManager.I.PlayerProfileManager.CurrentPlayer.Uuid,
                                         AppManager.I.PlayerProfileManager.CurrentPlayer);
            }
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus && CurrentProfile != null)
                SaveNow();
        }

        private void OnApplicationQuit()
        {
            if (CurrentProfile != null)
                SaveNow();
        }

        private void OldProfilePlayerChanged()
        {
            var legacy = AppManager.I.PlayerProfileManager.CurrentPlayer;
            if (legacy != null)
            {
                InitializeFromLegacyUuid(legacy.Uuid, legacy);
            }
            else
            {
                SetCurrentProfile(null);
            }
        }

        // ------------------------------
        // Profile load / switch
        // ------------------------------
        public void InitializeFromLegacyUuid(string legacyUuid, PlayerProfile legacy = null)
        {
            Debug.Log($"DiscoverAppManager.InitializeFromLegacyUuid: {legacyUuid}");
            var platform = IsMobile() ? "mobile" : "desktop";
            var p = profilesManager.LoadOrCreateByLegacyUuid(legacyUuid, legacy, platform, Application.version);
            profilesManager.SetCurrent(p.profile.id);
            SetCurrentProfile(p);
        }

        public bool SwitchProfile(string id)
        {
            var p = profilesManager.LoadById(id);
            if (p == null)
                return false;
            profilesManager.SetCurrent(id);
            SetCurrentProfile(p);
            return true;
        }

        public IReadOnlyList<DiscoverProfileHeader> ListProfiles() => profilesManager.ListProfiles();

        private void SetCurrentProfile(DiscoverPlayerProfile p)
        {
            CurrentProfile = p;
            _profileSvc = (p != null) ? new ProfileService(p, economySettings) : null;
            OnProfileLoaded?.Invoke(CurrentProfile);
        }

        // =========================================================
        //   QUEST FLOW
        // =========================================================

        /// <summary>Update the current profile after a quest concludes</summary>
        public void RecordQuestEnd(QuestEnd end)
        {
            if (CurrentProfile == null)
            { Debug.LogWarning("DiscoverAppManager.RecordQuestEnd called with no profile loaded."); return; }

            // Record quest run (updates stats + awards star→gem delta if improved)
            var award = Svc.RecordQuestRun(end.questId, end.score, end.stars, end.durationSec);

            // Aggregate activities
            if (end.activities != null)
            {
                foreach (var a in end.activities)
                {
                    Svc.RecordActivityRun(
                        activityId: a.activityId,
                        win: a.score >= 50,
                        score: a.score,
                        timeSec: a.durationSec,
                        topic: a.topic,
                        attempts: a.attempts,
                        correct: a.correct,
                        wrongItems: a.wrongItems
                    );
                }
            }

            if (award.Any)
            {
                OnGemsAwarded?.Invoke(award.gemsAdded, award.newClaims);
                OnCurrencyChanged?.Invoke();
            }

            MarkDirty();
        }

        // =========================================================
        //   CARD INTERACTIONS
        // =========================================================

        /// <summary>Record a single interaction with a card.</summary>
        public void RecordCardInteraction(CardData card, bool answeredCorrect = true)
        {
            if (CurrentProfile == null || card == null)
                return;

            Svc.RecordCardSeen(card.Id);

            // KP per interaction (can trigger XP milestone gems)
            var kpRes = Svc.RecordCardAnswerAndPoints(card.Id, answeredCorrect);

            // MP per interaction + unlock gem once ready (no overrides)
            var mpRes = Svc.ApplyCardMasteryAndUnlock(card, answeredCorrect);

            OnCurrencyChanged?.Invoke();
            if (kpRes.Any)
                OnGemsAwarded?.Invoke(kpRes.gemsAdded, kpRes.newClaims);
            if (mpRes.Any)
                OnGemsAwarded?.Invoke(mpRes.gemsAdded, mpRes.newClaims);

            MarkDirty();
        }

        // =========================================================
        //   ACHIEVEMENTS
        // =========================================================

        /// <summary>Increment an achievement's progress counter. If already unlocked, no-op.</summary>
        public void AddAchievementProgress(string achievementId, int delta)
        {
            if (CurrentProfile == null || string.IsNullOrEmpty(achievementId) || delta == 0)
                return;

            if (!CurrentProfile.achievements.TryGetValue(achievementId, out var st) || st == null)
                st = new AchievementState();

            if (st.unlocked)
                return;

            st.progress += delta;
            CurrentProfile.achievements[achievementId] = st;
            MarkDirty();
        }

        /// <summary>
        /// Unlocks an achievement (idempotent). Optionally grant points (XP), gems (ledger), and cookies.
        /// </summary>
        public void UnlockAchievementOnce(string achievementId, int rewardPoints = 0, int rewardGems = 0, int rewardCookies = 0)
        {
            if (CurrentProfile == null || string.IsNullOrEmpty(achievementId))
                return;

            if (!CurrentProfile.achievements.TryGetValue(achievementId, out var st) || st == null)
                st = new AchievementState();

            if (st.unlocked)
                return;

            st.unlocked = true;
            st.unlockedUtc = DatetimeUtilities.GetNowUtcString();
            CurrentProfile.achievements[achievementId] = st;

            bool anyCurrencyChanged = false;

            // Points (XP) — may trigger XP milestone gem claims via EconomySettings
            if (rewardPoints != 0)
            {
                var xpAward = Svc.AddPoints(rewardPoints);
                if (xpAward.Any)
                {
                    OnGemsAwarded?.Invoke(xpAward.gemsAdded, xpAward.newClaims);
                    anyCurrencyChanged = true;
                }
            }

            // Gems via ledger (idempotent per achievement)
            if (rewardGems > 0)
            {
                var gemAward = Svc.ClaimAchievementGem(achievementId, rewardGems);
                if (gemAward.Any)
                {
                    OnGemsAwarded?.Invoke(gemAward.gemsAdded, gemAward.newClaims);
                    anyCurrencyChanged = true;
                }
            }

            // Cookies (spendable)
            if (rewardCookies != 0)
            {
                CurrentProfile.wallet.cookies = Mathf.Max(0, CurrentProfile.wallet.cookies + rewardCookies);
                anyCurrencyChanged = true;
            }

            if (anyCurrencyChanged)
                OnCurrencyChanged?.Invoke();
            OnAchievementUnlocked?.Invoke(achievementId, st.unlockedUtc);

            MarkDirty();
        }

        // =========================================================
        //   SAVE PROFILE
        // =========================================================

        /// <summary>Mark the profile as modified, so it will be saved.</summary>
        public void MarkDirty()
        {
            dirty = true;
            if (saveCo != null)
                StopCoroutine(saveCo);
            saveCo = StartCoroutine(SaveAfterDelay());
        }

        /// <summary>Immediate save to disk (no debounce).</summary>
        public void SaveNow()
        {
            if (CurrentProfile == null)
                return;
            dirty = false;
            if (saveCo != null)
            { StopCoroutine(saveCo); saveCo = null; }
            profilesManager.Save(CurrentProfile);
        }

        private IEnumerator SaveAfterDelay()
        {
            yield return new WaitForSeconds(saveDebounceSeconds);
            if (dirty)
                SaveNow();
        }

        private bool IsMobile()
        {
            var p = Application.platform;
            return p == RuntimePlatform.IPhonePlayer || p == RuntimePlatform.Android;
        }
    }
}
