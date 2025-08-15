using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using System;
using Antura.UI;
using Antura.Database;
using Antura.Language;

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

        private ActivityPanel activityPanel;
        private int currentRound = 0;
        private float roundStartTime;
        private bool validateEnabled;
        private bool descriptionShown;

        public enum ActivityPlayState { Idle, Playing, Paused, Finished, Exiting }
        private ActivityPlayState state = ActivityPlayState.Idle;
        public ActivityPlayState State => state;

        // Timer settings bridged from concrete settings
        public bool HasTimer => GetSettings()?.HasTimer ?? false;
        public int TimerSeconds => GetSettings()?.TimerSeconds ?? 60;

        public void Open()
        {
            activityPanel = GetComponentInParent<ActivityPanel>();
            Init();
            SetupRoundsFromSettings();
            if (!descriptionShown && Description != null)
            {
                ShowMessage(Description);
                descriptionShown = true;
            }
            BeginRound();
        }

        public virtual void Init()
        {

        }


        #region Public Methods

        /// <summary>
        /// Called by <see cref="ActivityOverlay"/> when the Help buttons is clicked,
        /// should toggle (show/hide) help panel
        /// </summary>
        public void ToggleHelp()
        {

        }

        /// <summary>
        /// Called by <see cref="ActivityOverlay"/> when the eventual timer has elapsed
        /// </summary>
        public void TimerElapsed()
        {
            // Default behavior: treat as fail and end activity (no points or fail points based on PointsFail)
            Debug.LogWarning("ActivityBase: timer elapsed -> FAIL round");
            if (state == ActivityPlayState.Playing)
                EndRound(false, 0f, true);
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
        /// Override to expose your concrete settings to base scoring logic.
        /// </summary>
        protected virtual ActivitySettingsAbstract GetSettings() => null;

        /// <summary>
        /// Enable/disable the Validate button in the overlay.
        /// </summary>
        protected void SetValidateEnabled(bool enable)
        {
            validateEnabled = enable;
            activityPanel?.EnableValidateButton(enable);
        }

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
                activityPanel?.PauseTimer();
            RequestExitConfirmation(
                onYes: () =>
                {
                    // Finish immediately with 0 points, mark as fail for flow messaging
                    OnActivityFinished(false, 0, Time.realtimeSinceStartup - roundStartTime, false);
                    ShowEndMessage(false);
                    try
                    { activityPanel?.PauseTimer(); }
                    catch { }
                    activityPanel?.Hide();
                    state = ActivityPlayState.Finished;
                },
                onNo: () =>
                {
                    // Resume if was playing
                    if (HasTimer)
                        activityPanel?.ResumeTimer();
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

            // Decide next
            if (currentRound < Mathf.Max(1, roundsTarget))
            {
                // Prepare next round
                BeginRound();
                OnRoundAdvanced(success, points, elapsed, dueToTimeout);
            }
            else
            {
                // Finished activity
                OnActivityFinished(success, points, elapsed, dueToTimeout);
                ShowEndMessage(success);
                activityPanel?.PauseTimer();
                activityPanel?.Hide();
                state = ActivityPlayState.Finished;
            }
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
    }
}
