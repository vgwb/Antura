using Antura.UI;
using Antura.Database;
using Antura.Language;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using System;
using DG.Tweening;
using Antura.Discover;

namespace Antura.Discover.Activities
{
    public enum Difficulty
    {
        Default = 0,
        Tutorial = 1,
        Easy = 2,
        Normal = 3,
        Expert = 4
    }

    public class ActivityBase : MonoBehaviour
    {

        [Header("Common properties")]
        public string ActivityCode;

        [SerializeField] GameObject HelpPanel;

        [Tooltip("Optional permalink of the Node with the mission")]
        public LocalizedString Description;

        [Tooltip("Optional permalink of the Node when success")]
        public LocalizedString SuccessMessage;

        [Tooltip("Optional permalink of the Node when fail")]
        public LocalizedString FailMessage;

        [Header("Rounds & Scoring")]
        [Tooltip("Target rounds to play for this activity. Defaults to Settings.MinRounds if available.")]
        [SerializeField] private int roundsTarget = 1;
        [Tooltip("Time threshold in seconds to consider a perfect 'master' round (doubles points on success). Set 0 to disable.")]
        [SerializeField] private float masterTimeThreshold = 0f;

        private bool initialized;
        private bool currHasTimer;

        public bool HasTimer => GetSettings()?.HasTimer ?? false;
        public int TimerSeconds => GetSettings()?.TimerSeconds ?? 60;
        private ActivityOverlay overlay;
        private int currentRound = 0;
        private float roundStartTime;
        private bool validateEnabled;
        private bool descriptionShown;
        private int startingRoundsTarget; // keep original target to decide replay prompt

        public enum ActivityPlayState { Idle, Playing, Paused, Finished, Exiting }
        private ActivityPlayState state = ActivityPlayState.Idle;
        public ActivityPlayState State => state;


        // Settings configured by the launcher (manager); concrete activities can override GetSettings to customize
        private ActivitySettingsAbstract _configuredSettings;

        /// <summary>
        /// Configure this activity with the provided settings. Call before opening.
        /// </summary>
        public virtual void ConfigureSettings(ActivitySettingsAbstract settings)
        {
            _configuredSettings = settings;
        }

        public void Open()
        {
            InitOverlayUI();
            SetupRoundsFromSettings();
            startingRoundsTarget = roundsTarget;
            if (!descriptionShown && Description != null)
            {
                ShowMessage(Description);
                descriptionShown = true;
            }
            // Show panel with settings-derived timer
            InitActivity();
            ShowPanel(HasTimer, TimerSeconds);
            BeginRound();
        }

        /// <summary>
        /// Reset the activity to a pristine state so it can be relaunched cleanly.
        /// Derived classes should override <see cref="OnResetActivity"/> to clear custom state.
        /// </summary>
        public void ResetActivity()
        {
            StopAllCoroutines();
            state = ActivityPlayState.Idle;
            currentRound = 0;
            roundStartTime = 0f;
            validateEnabled = false;
            descriptionShown = false;
            // Reset DOTween on children to avoid stuck tweens
            try
            { DOTween.Kill(this, complete: false); }
            catch { }
            OnResetActivity();
        }

        /// <summary>
        /// Hook for derived classes to reset their own state.
        /// </summary>
        protected virtual void OnResetActivity() { }

        public virtual void InitActivity()
        {
            Debug.Log("ActivityBase: Init() called");
        }


        // =============================
        // Overlay/Panel management
        // =============================
        private void InitOverlayUI()
        {
            if (initialized)
                return;
            initialized = true;

            overlay = this.GetComponentInChildren<ActivityOverlay>(true);
            if (overlay == null)
            {
                Debug.LogError("ActivityBase: couldn't find ActivityOverlay child");
                return;
            }

            // Wire buttons
            overlay.BtClose.onClick.AddListener(ExitWithoutPoints);
            overlay.BtHelp.onClick.AddListener(ToggleHelp);
            overlay.BtValidate.onClick.AddListener(Validate);

            // Subscriptions
            try
            { GlobalUI.PauseMenu.OnPauseToggled.Subscribe(OnGlobalPauseToggled); }
            catch { }
            overlay.Timer.OnTimerElapsed.Subscribe(OnTimerElapsed);
        }

        protected virtual void Update()
        {
            if (!(QuestManager.I?.DebugQuest ?? false))
                return;
            if (state != ActivityPlayState.Playing)
                return;
            // Debug: 1=fail (<0), 2=quit (=0), 3=success (>0)
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Debug.Log("Force Exit Fail");
                // Force close as fail
                SetRoundsTarget(currentRound);
                EndRound(false, 0f, false);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Debug.Log("Force Exit 0 points");
                // Quit with 0 points
                ExitWithoutPoints();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                Debug.Log("Force Exit Success");
                // Force close as success
                SetRoundsTarget(currentRound);
                EndRound(true, 1f, false);
            }
        }


        #region Public Methods

        /// <summary>
        /// Called by <see cref="ActivityOverlay"/> when the Help buttons is clicked,
        /// should toggle (show/hide) help panel
        /// </summary>
        public void ToggleHelp()
        {
            if (HelpPanel == null)
                return;
            HelpPanel.SetActive(!HelpPanel.activeSelf);
        }

        private void OnTimerElapsed()
        {
            // Default behavior: treat as fail and end activity (no points or fail points based on PointsFail)
            Debug.LogWarning("ActivityBase: timer elapsed -> FAIL round");
            if (state == ActivityPlayState.Playing)
                EndRound(false, 0f, true);
        }

        /// <summary>
        /// Enable/disable the Validate button in the overlay.
        /// </summary>
        protected void SetValidateEnabled(bool enable)
        {
            validateEnabled = enable;
            EnableValidateButton(enable);
        }

        /// <summary>
        /// Called by <see cref="ActivityOverlay"/> when the validate button is clicked
        /// </summary>
        public void Validate()
        {
            if (state != ActivityPlayState.Playing)
                return;

            bool result = DoValidate();
            float score01 = result ? Mathf.Clamp01(GetRoundScore01()) : 0f;
            EndRound(result, score01, false);
        }

        public virtual bool DoValidate()
        {
            // Default implementation, should be overridden by derived classes
            return false;
        }

        /// <summary>
        /// Override to return a normalized score [0..1] for the current round.
        /// Default: 1 when DoValidate returns true, 0 otherwise.
        /// </summary>
        protected virtual float GetRoundScore01()
        {
            return 1f;
        }

        /// <summary>
        /// Override to expose concrete settings to base scoring logic.
        /// </summary>
        protected virtual ActivitySettingsAbstract GetSettings() => _configuredSettings;



        /// <summary>
        /// Call at the start of each round to initialize timers and UI.
        /// </summary>
        protected void BeginRound()
        {
            currentRound++;
            roundStartTime = Time.realtimeSinceStartup;
            SetValidateEnabled(false);
            state = ActivityPlayState.Playing;
        }

        /// <summary>
        /// Allow user to exit the activity with zero points. Shows a confirmation before exiting.
        /// </summary>
        public void ExitWithoutPoints()
        {
            if (state == ActivityPlayState.Finished || state == ActivityPlayState.Exiting)
                return;

            state = ActivityPlayState.Exiting;
            // Pause timer while prompting
            if (HasTimer)
                PauseTimer();
            RequestExitConfirmation(
                onYes: () =>
                {
                    // Finish immediately with 0 points, mark as fail for flow messaging
                    OnActivityFinished(false, 0, Time.realtimeSinceStartup - roundStartTime, false);
                    ShowEndMessage(false);
                    try
                    { PauseTimer(); }
                    catch { }
                    HidePanel();
                    state = ActivityPlayState.Finished;
                },
                onNo: () =>
                {
                    // Resume if was playing
                    if (HasTimer)
                        ResumeTimer();
                    state = ActivityPlayState.Playing;
                }
            );
        }

        /// <summary>
        /// Override to show a confirmation UI before exiting. Default immediately confirms.
        /// </summary>
        protected virtual void RequestExitConfirmation(Action onYes, Action onNo)
        {
            // Common prompt using existing GlobalUI prompt system
            GlobalUI.ShowPrompt(LocalizationDataId.UI_AreYouSure, onYes, onNo, LanguageUse.Native);
        }

        /// <summary>
        /// Ends the round, computes points, records profile data, and progresses rounds or closes the panel.
        /// </summary>
        protected void EndRound(bool success, float score01, bool dueToTimeout)
        {
            float elapsed = Time.realtimeSinceStartup - roundStartTime;

            int points = 0;
            if (success)
            {
                points = PointsFromSettings();
                if (masterTimeThreshold > 0 && elapsed <= masterTimeThreshold)
                    points *= 2; // Master bonus
            }
            else
            {
                var s = GetSettings();
                points = s != null ? s.PointsFail : 0;
            }

            try
            { QuestManager.I.AddProgressPoints(points); }
            catch { }

            // Show result banner briefly if present
            TryShowResultBanner(success);

            // Decide next
            if (currentRound < Mathf.Max(1, roundsTarget))
            {
                StartCoroutine(ProceedNextRoundCO(success, points, elapsed, dueToTimeout));
            }
            else
            {
                StartCoroutine(CloseWithBannerCO(success, points, elapsed, dueToTimeout));
            }
        }

        private IEnumerator ProceedNextRoundCO(bool success, int points, float elapsed, bool dueToTimeout)
        {
            yield return new WaitForSecondsRealtime(0.65f);
            BeginRound();
            OnRoundAdvanced(success, points, elapsed, dueToTimeout);
        }

        private IEnumerator CloseWithBannerCO(bool success, int points, float elapsed, bool dueToTimeout)
        {
            yield return new WaitForSecondsRealtime(0.85f);
            OnActivityFinished(success, points, elapsed, dueToTimeout);
            ShowEndMessage(success);
            PauseTimer();

            // If finished before reaching MaxRounds, offer a replay prompt
            var s = GetSettings();
            bool canReplay = (s != null && startingRoundsTarget < s.MaxRounds);
            if (canReplay)
            {
                bool userChoseReplay = false;
                bool decided = false;
                RequestReplayConfirmation(
                    onYes: () => { userChoseReplay = true; decided = true; },
                    onNo: () => { userChoseReplay = false; decided = true; }
                );
                while (!decided)
                    yield return null;

                if (userChoseReplay)
                {
                    roundsTarget = Mathf.Clamp(startingRoundsTarget + 1, s.MinRounds, s.MaxRounds);
                    ResetActivity();
                    ShowPanel(HasTimer, TimerSeconds);
                    BeginRound();
                    yield break;
                }
            }

            HidePanel();
            state = ActivityPlayState.Finished;

            // Notify ActivityManager to persist and possibly jump back to Yarn
            try
            {
                // Map points to the "score" concept expected by Yarn: >0 success, =0 quit, <0 fail
                int scoreOut = success ? Mathf.Max(1, points) : (dueToTimeout ? -1 : Mathf.Min(-1, points));
                ActivityManager.I?.OnActivityClosed(ActivityCode, scoreOut, Mathf.RoundToInt(elapsed));
            }
            catch { }
        }

        private void TryShowResultBanner(bool success)
        {
            try
            {
                var ov = overlay;
                if (ov != null)
                {
                    var color = success ? new Color(0.1f, 0.7f, 0.2f) : new Color(0.85f, 0.25f, 0.25f);
                    var text = success ? "Success" : "Try again";
                    StartCoroutine(ov.ShowResultBanner(text, color, 0.6f));
                }
            }
            catch { }
        }

        // Simple pulse helper usable by derived classes
        protected void Pulse(Transform target, float scale = 1.08f, float duration = 0.12f)
        {
            if (target == null)
                return;
            var s = target.localScale;
            target.DOKill();
            DOTween.Sequence()
                .Append(target.DOScale(s * scale, duration).SetUpdate(true))
                .Append(target.DOScale(s, duration).SetUpdate(true));
        }

        /// <summary>
        /// Hook called when moving to next round. Derived classes can refresh content.
        /// </summary>
        protected virtual void OnRoundAdvanced(bool lastRoundSuccess, int lastRoundPoints, float lastRoundSeconds, bool dueToTimeout) { }

        /// <summary>
        /// Hook called when the activity ends (after the last round).
        /// </summary>
        protected virtual void OnActivityFinished(bool lastRoundSuccess, int lastRoundPoints, float lastRoundSeconds, bool dueToTimeout) { }

        /// <summary>
        /// Optionally set the total rounds to play (clamped to settings Min/Max when available).
        /// </summary>
        public void SetRoundsTarget(int rounds)
        {
            roundsTarget = Mathf.Max(1, rounds);
            ClampRoundsToSettings();
        }

        private void SetupRoundsFromSettings()
        {
            var s = GetSettings();
            if (s != null)
            {
                roundsTarget = Mathf.Max(1, s.MinRounds);
                // derive master threshold from timer if not explicitly set and timerSeconds is short
                if (masterTimeThreshold <= 0f && s.HasTimer)
                    masterTimeThreshold = Mathf.Min(10f, s.TimerSeconds * 0.25f);
            }
            ClampRoundsToSettings();
        }

        private void ClampRoundsToSettings()
        {
            var s = GetSettings();
            if (s != null)
                roundsTarget = Mathf.Clamp(roundsTarget, Mathf.Max(1, s.MinRounds), Mathf.Max(s.MinRounds, s.MaxRounds));
        }

        private int PointsFromSettings()
        {
            var s = GetSettings();
            Difficulty difficulty = Difficulty.Default;
            // Try to infer difficulty from settings if needed
            if (s != null)
                difficulty = s.Difficulty;

            switch (difficulty)
            {
                case Difficulty.Tutorial:
                    return s != null ? s.PointsEasy : 0;
                case Difficulty.Easy:
                    return s != null ? s.PointsEasy : 0;
                case Difficulty.Normal:
                    return s != null ? s.PointsNormal : 0;
                case Difficulty.Expert:
                    return s != null ? s.PointsExpert : 0;
                case Difficulty.Default:
                default:
                    return s != null ? s.PointsNormal : 0;
            }
        }

        protected virtual void ShowMessage(LocalizedString message)
        {
            if (message == null)
                return;
            try
            {
                var handle = message.GetLocalizedStringAsync();
                handle.Completed += op =>
                {
                    if (op.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
                    {
                        var txt = op.Result;
                        if (!string.IsNullOrEmpty(txt))
                            Debug.Log($"[Activity] {txt}", this);
                    }
                };
            }
            catch { }
        }
        private void ShowEndMessage(bool success)
        {
            if (success)
                ShowMessage(SuccessMessage);
            else
                ShowMessage(FailMessage);
        }

        #endregion
        /// <summary>
        /// Ask the user if they want to replay with more rounds when there are still rounds available (< MaxRounds).
        /// Default uses the same GlobalUI prompt system.
        /// </summary>
        protected virtual void RequestReplayConfirmation(Action onYes, Action onNo)
        {
            GlobalUI.ShowPrompt(LocalizationDataId.UI_AreYouSure, onYes, onNo, LanguageUse.Native);
        }


        protected virtual void OnDestroy()
        {
            if (overlay != null)
            {
                overlay.Timer.OnTimerElapsed.Unsubscribe(OnTimerElapsed);
            }
            try
            { GlobalUI.PauseMenu.OnPauseToggled.Unsubscribe(OnGlobalPauseToggled); }
            catch { }
        }

        public void OpenFresh()
        {
            InitOverlayUI();
            ResetActivity();
            Open();
        }

        public void ShowPanel(bool pHasTimer, int pTimerSeconds)
        {
            InitOverlayUI();
            currHasTimer = pHasTimer;
            this.gameObject.SetActive(true);
            EnableValidateButton(false);
            overlay.SetTimer(pHasTimer, pTimerSeconds);
        }

        public void HidePanel()
        {
            InitOverlayUI();
            overlay.Timer.CancelTimer();
            this.gameObject.SetActive(false);

        }

        public void EnableValidateButton(bool enable)
        {
            InitOverlayUI();
            overlay.BtValidate.interactable = enable;
        }

        public void PauseTimer()
        {
            InitOverlayUI();
            overlay.Timer.PauseTimer();
        }

        public void ResumeTimer()
        {
            InitOverlayUI();
            overlay.Timer.ResumeTimer();
        }

        private void OnGlobalPauseToggled(bool paused)
        {
            if (!currHasTimer)
                return;
            if (paused)
                PauseTimer();
            else
                ResumeTimer();
        }
    }
}
