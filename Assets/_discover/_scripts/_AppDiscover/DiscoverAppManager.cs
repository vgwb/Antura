using Antura.Core;
using Antura.Profile;
using Antura.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Discover
{
    //    [DefaultExecutionOrder(-300)]
    public class DiscoverAppManager : MonoBehaviour
    {

        public static DiscoverAppManager I { get; private set; }


        private string storageSubdir = "discover_profiles";

        // Store for JSON files + tiny PlayerPrefs pointer
        private DiscoverProfileManager profilesManager;

        // Currently loaded Discover profile (null until Initialize... is called)
        public DiscoverPlayerProfile CurrentProfile { get; private set; }

        // Auto-save delay after marking profile dirty (seconds).
        private float saveDebounceSeconds = 60f;

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

        private void Awake()
        {
            if (I != null && I != this)
            { Destroy(gameObject); return; }
            I = this;
            DontDestroyOnLoad(gameObject);
            profilesManager = new DiscoverProfileManager(storageSubdir);
            PlayerProfileManager.OnProfileChanged += OldProfilePlayerChanged;
        }

        void OnDestroy()
        {
            PlayerProfileManager.OnProfileChanged -= OldProfilePlayerChanged;
        }

        private void Start()
        {
            // Initialize with the current profile (if any)
            if (AppManager.I.PlayerProfileManager.CurrentPlayer != null)
            {
                InitializeFromLegacyUuid(AppManager.I.PlayerProfileManager.CurrentPlayer.Uuid, AppManager.I.PlayerProfileManager.CurrentPlayer);
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
            //            Debug.Log($"DiscoverAppManager.OldProfilePlayerChanged: {AppManager.I.PlayerProfileManager.CurrentPlayer?.Uuid})");
            if (AppManager.I.PlayerProfileManager.CurrentPlayer != null)
            {
                InitializeFromLegacyUuid(AppManager.I.PlayerProfileManager.CurrentPlayer.Uuid, AppManager.I.PlayerProfileManager.CurrentPlayer);
            }
            else
            {
                CurrentProfile = null;
            }
        }
        public void InitializeFromLegacyUuid(string legacyUuid, PlayerProfile legacy = null)
        {
            Debug.Log($"DiscoverAppManager.InitializeFromLegacyUuid: {legacyUuid})");
            var platform = IsMobile() ? "mobile" : "desktop";
            CurrentProfile = profilesManager.LoadOrCreateByLegacyUuid(legacyUuid, legacy, platform, Application.version);
            profilesManager.SetCurrent(CurrentProfile.profile.id);
            OnProfileLoaded?.Invoke(CurrentProfile);
        }

        public bool SwitchProfile(string id)
        {
            var p = profilesManager.LoadById(id);
            if (p == null)
                return false;
            CurrentProfile = p;
            profilesManager.SetCurrent(id);
            OnProfileLoaded?.Invoke(CurrentProfile);
            return true;
        }

        public IReadOnlyList<DiscoverProfileHeader> ListProfiles() => profilesManager.ListProfiles();

        // =========================================================
        //   QUEST FLOW
        // =========================================================

        /// <summary>
        /// Data payload for a finished quest. Call <see cref="RecordQuestEnd"/> at the end of a quest.
        /// Keep aligned with what your minigames can report.
        /// </summary>
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

        /// <summary>
        /// update the current profile after a quest concludes
        /// </summary>
        public void RecordQuestEnd(QuestEnd end)
        {
            if (CurrentProfile == null)
            { Debug.LogWarning("DiscoverAppManager.RecordQuestEnd called with no profile loaded."); return; }
            var p = CurrentProfile;
            var now = DateTime.UtcNow.ToString("o");

            // --- Update quest stats ---
            if (!p.stats.quests.TryGetValue(end.questId, out var qs))
            {
                qs = new QuestStats { firstPlayedUtc = now };
                p.stats.quests[end.questId] = qs;
            }
            qs.plays++;
            qs.completions++;
            qs.lastScore = end.score;
            if (end.score > qs.bestScore)
                qs.bestScore = end.score;
            qs.sumScore += end.score;
            qs.timeSec += end.durationSec;
            qs.lastStars = Mathf.Clamp(end.stars, 0, 3);
            if (qs.lastStars > qs.bestStars)
                qs.bestStars = qs.lastStars;
            qs.lastPlayedUtc = now;

            // --- Update activities (aggregated) ---
            if (end.activities != null)
            {
                foreach (var a in end.activities)
                {
                    if (!p.stats.activities.TryGetValue(a.activityId, out var asx))
                    {
                        asx = new ActivityStats();
                        p.stats.activities[a.activityId] = asx;
                    }
                    asx.plays++;
                    if (a.score >= 50)
                        asx.wins++;
                    asx.lastScore = a.score;
                    if (a.score > asx.bestScore)
                        asx.bestScore = a.score;
                    asx.sumScore += a.score;
                    asx.timeSec += a.durationSec;

                    if (!string.IsNullOrEmpty(a.topic))
                        asx.didactic.topic = a.topic;
                    asx.didactic.attempts += a.attempts;
                    asx.didactic.correct += a.correct;

                    if (a.wrongItems != null)
                    {
                        foreach (var kv in a.wrongItems)
                        {
                            asx.didactic.wrongItems.TryGetValue(kv.Key, out var cur);
                            asx.didactic.wrongItems[kv.Key] = cur + kv.Value;
                        }
                    }
                }
            }

            MarkDirty();
        }

        // =========================================================
        //   CARD INTERACTIONS
        // =========================================================

        /// <summary>
        /// Record a single interaction with a card
        /// </summary>
        public void RecordCardInteraction(string cardId, bool unlocked, bool answeredCorrect = true)
        {
            if (CurrentProfile == null)
                return;
            var now = DateTime.UtcNow.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'");

            if (!CurrentProfile.cards.TryGetValue(cardId, out var cardState))
            {
                cardState = new CardState { firstSeenUtc = now, unlocked = unlocked };
                CurrentProfile.cards[cardId] = cardState;
            }

            cardState.unlocked = cardState.unlocked || unlocked;
            cardState.lastSeenUtc = now;
            cardState.interactions++;
            if (answeredCorrect)
                cardState.answered.correct++;
            else
                cardState.answered.wrong++;

            // simple mastery heuristic: update towards 1 on correct, towards 0 on wrong
            var a = 0.15f;
            var target = answeredCorrect ? 1f : 0f;
            cardState.mastery01 = Mathf.Clamp01(cardState.mastery01 + a * (target - cardState.mastery01));

            // streak logic
            cardState.streakCorrect = answeredCorrect ? cardState.streakCorrect + 1 : 0;

            CurrentProfile.cards[cardId] = cardState;
            MarkDirty();
        }

        // =========================================================
        //   ACHIEVEMENTS
        // =========================================================

        /// <summary>
        /// Increment an achievement's progress counter. If it's already unlocked, this is a no-op.
        /// (Your code decides when to call this and what thresholds mean.)
        /// </summary>
        public void AddAchievementProgress(string achievementId, int delta)
        {
            if (CurrentProfile == null || delta == 0)
                return;

            if (!CurrentProfile.achievements.TryGetValue(achievementId, out var st))
                st = new AchievementState();

            if (st.unlocked)
                return; // already granted

            st.progress += delta;
            CurrentProfile.achievements[achievementId] = st;
            MarkDirty();
        }

        /// <summary>
        /// Unlocks an achievement (idempotent). Optionally grant currency here if desired.
        /// Call this when your rule is satisfied (e.g., progress >= goal).
        /// </summary>
        public void UnlockAchievementOnce(string achievementId, int rewardPoints = 0, int rewardGems = 0, int rewardCookies = 0)
        {
            if (CurrentProfile == null)
                return;

            if (!CurrentProfile.achievements.TryGetValue(achievementId, out var st))
                st = new AchievementState();

            if (st.unlocked)
                return;

            st.unlocked = true;
            st.unlockedUtc = DateTime.UtcNow.ToString("o");
            CurrentProfile.achievements[achievementId] = st;

            // currency rewards (optional, driven by your code/definitions)
            if (rewardPoints != 0 || rewardGems != 0 || rewardCookies != 0)
            {
                CurrentProfile.currency.points += rewardPoints;
                CurrentProfile.currency.gems += rewardGems;
                CurrentProfile.currency.cookies += rewardCookies;
                OnCurrencyChanged?.Invoke();
            }

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
