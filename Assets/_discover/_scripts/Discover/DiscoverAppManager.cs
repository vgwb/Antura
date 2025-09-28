using Antura.Core;
using Antura.Profile;
using Antura.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using System.Threading;
using System.Threading.Tasks;

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
        public Dictionary<string, int> wrongItems;
    }

    public struct QuestEnd
    {
        public string questId;
        public int score;
        public int durationSec;
        public int stars;
        public List<ActivityEnd> activities;
    }

    [RequireComponent(typeof(DiscoverDataManager))]
    public class DiscoverAppManager : SingletonMonoBehaviour<DiscoverAppManager>
    {

        [SerializeField]
        private DiscoverDataManager dataManager;
        public string LearningLanguageIso2 = "fr";

        // Cached learning locale & localized strings (table:entry:locale -> value)
        private Locale _cachedLearningLocale;
        private string _cachedLearningIso2;
        private readonly Dictionary<string, string> _learningStringCache = new Dictionary<string, string>();

        [SerializeField]
        private EconomySettings economySettings;
        private const string storageSubdir = "discover_profiles";
        private DiscoverProfileManager profilesManager;
        public DiscoverPlayerProfile CurrentProfile { get; private set; }
        public DiscoverDataManager Data => dataManager;
        private ProfileService profileService;
        private ProfileService ProfileSvc
        {
            get
            {
                if (profileService == null && CurrentProfile != null)
                    profileService = new ProfileService(CurrentProfile, economySettings);
                return profileService;
            }
        }

        // Auto-save debounce (seconds)
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

        /// <summary>Raised when gems are awarded through the ledger (delta, newly added tokens).</summary>
        public event Action<int, GemTokenClaim[]> OnGemsAwarded;

        protected override void Init()
        {
            if (dataManager == null)
                dataManager = GetComponent<DiscoverDataManager>();
            if (dataManager != null)
                dataManager.Initialize(this);

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
            if (AppManager.I.PlayerProfileManager.CurrentPlayer != null)
            {
                InitializeFromLegacyUuid(AppManager.I.PlayerProfileManager.CurrentPlayer.Uuid,
                                         AppManager.I.PlayerProfileManager.CurrentPlayer);
            }

            LearningLanguageIso2 = AppManager.I.ContentEdition.LearningLanguageConfig.Iso2;
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
        // Navigation
        // ------------------------------
        public void OpenQuest(QuestData questData)
        {
            AppManager.I.NavigationManager.GoToDiscoverQuest(questData.scene);
        }

        public void GoToQuestMenu()
        {
            AppManager.I.NavigationManager.ExitToMainMenu();
        }


        // ------------------------------
        // Profile load / switch
        // ------------------------------
        public void InitializeFromLegacyUuid(string legacyUuid, PlayerProfile legacy = null)
        {
            // Debug.Log($"DiscoverAppManager.InitializeFromLegacyUuid: {legacyUuid}");
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
            profileService = (p != null) ? new ProfileService(p, economySettings) : null;
            OnProfileLoaded?.Invoke(CurrentProfile);
            // Profile switch may impact language choice; clear cached localization
            ClearLearningLocalizationCache();
        }


        public CardData GetCardById(string cardId)
        {
            return dataManager != null ? dataManager.GetCard(cardId) : null;
        }

        public TopicData GetTopicById(string topicId)
        {
            return dataManager != null ? dataManager.GetTopic(topicId) : null;
        }

        public BonusMalusData GetBonusMalusById(string id)
        {
            return dataManager != null ? dataManager.GetBonusMalus(id) : null;
        }

        public QuestData GetQuestById(string questId)
        {
            return dataManager != null ? dataManager.GetQuest(questId) : null;
        }

        // =========================================================
        //   QUEST FLOW
        // =========================================================

        public void RecordActivityEnd(ActivityEnd activityEnd)
        {
            if (CurrentProfile == null)
            { Debug.LogWarning("DiscoverAppManager.RecordActivityEnd called with no profile loaded."); return; }

            ProfileSvc.RecordActivityRun(
                activityId: activityEnd.activityId,
                win: activityEnd.score >= 50,
                score: activityEnd.score,
                timeSec: activityEnd.durationSec,
                topic: activityEnd.topic,
                attempts: activityEnd.attempts,
                correct: activityEnd.correct,
                wrongItems: activityEnd.wrongItems
            );

            MarkDirty();
        }

        /// <summary>Update the current profile after a quest concludes</summary>
        public void RecordQuestEnd(QuestEnd end, int currentCookies)
        {
            if (CurrentProfile == null)
            { Debug.LogWarning("DiscoverAppManager.RecordQuestEnd called with no profile loaded."); return; }

            // Record quest run (updates stats + awards star→gem delta if improved)
            var award = ProfileSvc.RecordQuestRun(end.questId, end.score, end.stars, end.durationSec);

            // Aggregate activities
            if (end.activities != null)
            {
                foreach (var a in end.activities)
                {
                    ProfileSvc.RecordActivityRun(
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

            CurrentProfile.wallet.cookies = currentCookies;

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

            ProfileSvc.RecordCardSeen(card.Id);

            // KP per interaction (can trigger XP milestone gems)
            var kpRes = ProfileSvc.RecordCardAnswerAndPoints(card.Id, answeredCorrect);

            // MP per interaction + unlock gem once ready (no overrides)
            var mpRes = ProfileSvc.ApplyCardMasteryAndUnlock(card, answeredCorrect);

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
                var xpAward = ProfileSvc.AddPoints(rewardPoints);
                if (xpAward.Any)
                {
                    OnGemsAwarded?.Invoke(xpAward.gemsAdded, xpAward.newClaims);
                    anyCurrencyChanged = true;
                }
            }

            // Gems via ledger (idempotent per achievement)
            if (rewardGems > 0)
            {
                var gemAward = ProfileSvc.ClaimAchievementGem(achievementId, rewardGems);
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

        // =========================================================
        //   LEARNING LANGUAGE LOCALIZATION (CACHE)
        // =========================================================

        /// <summary>Clears the cached learning locale and localized string cache.</summary>
        public void ClearLearningLocalizationCache()
        {
            _cachedLearningLocale = null;
            _cachedLearningIso2 = null;
            _learningStringCache.Clear();
        }

        /// <summary>
        /// Resolve current learning Locale based on <see cref="LearningLanguageIso2"/>.
        /// Prefers exact match, then first locale whose code starts with the ISO2.
        /// Cached until the ISO2 changes.
        /// </summary>
        public Locale GetLearningLocale()
        {
            LearningLanguageIso2 = AppManager.I.ContentEdition.LearningLanguageConfig.Iso2;
            var iso2 = LearningLanguageIso2;
            if (string.IsNullOrEmpty(iso2) || LocalizationSettings.AvailableLocales == null)
                return null;

            if (_cachedLearningLocale != null && string.Equals(_cachedLearningIso2, iso2, StringComparison.OrdinalIgnoreCase))
                return _cachedLearningLocale;

            Locale locale = LocalizationSettings.AvailableLocales.GetLocale(iso2);
            if (locale == null)
            {
                foreach (var loc in LocalizationSettings.AvailableLocales.Locales)
                {
                    if (loc == null)
                        continue;
                    var code = loc.Identifier.Code;
                    if (!string.IsNullOrEmpty(code) && code.StartsWith(iso2, StringComparison.OrdinalIgnoreCase))
                    { locale = loc; break; }
                }
            }

            _cachedLearningLocale = locale;
            _cachedLearningIso2 = iso2;
            return locale;
        }

        /// <summary>
        /// Get the localized string for the given entry (or shadow) from the specified table, using the learning language locale.
        /// Results are cached per (table, entry, locale).
        /// </summary>
        /// <param name="tableReference">The string table reference.</param>
        /// <param name="entryId">The primary line/entry ID.</param>
        /// <param name="shadowEntryId">Optional shadow entry ID to prefer.</param>
        /// <param name="fallback">Localization fallback behavior.</param>
        /// <param name="ct">Cancellation token.</param>
        public async Task<string> GetLearningLocalizedStringAsync(TableReference tableReference, string entryId, string shadowEntryId = null, FallbackBehavior fallback = FallbackBehavior.UseFallback, CancellationToken ct = default)
        {
            try
            {
                var locale = GetLearningLocale();
                if (locale == null)
                    return string.Empty;

                var keyEntry = string.IsNullOrEmpty(shadowEntryId) ? entryId : shadowEntryId;
                var cacheKey = $"{tableReference}:{keyEntry}:{locale.Identifier.Code}";
                if (_learningStringCache.TryGetValue(cacheKey, out var cached))
                    return cached;

                var handle = LocalizationSettings.StringDatabase.GetTableEntryAsync(tableReference, keyEntry, locale, fallback);
                // Await the async operation's task; observe cancellation if supported
                var result = await handle.Task.ConfigureAwait(false);
                ct.ThrowIfCancellationRequested();
                var value = result.Entry?.LocalizedValue ?? string.Empty;
                _learningStringCache[cacheKey] = value;
                return value;
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[DiscoverAppManager] GetLearningLocalizedStringAsync failed for entry '{shadowEntryId ?? entryId}': {ex.Message}");
                return string.Empty;
            }
        }
    }
}
