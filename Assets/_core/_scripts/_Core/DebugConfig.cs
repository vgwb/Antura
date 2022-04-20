using Antura.Profile;
using UnityEngine;

namespace Antura.Core
{
    public class DebugConfig : ScriptableObject
    {
        public static DebugConfig I => AppManager.I.DebugConfig;

        [Header("Dev Mode (set all to FALSE for release)")]
        /// <summary>
        /// generic settings (for analytics environment, for example)
        /// Set to FALSE for release.
        /// </summary>
        public bool DeveloperMode = false;

        /// <summary>
        /// Enables the Advanced Debug Panel (click bottom right corner of the screen to activate)
        /// Set to FALSE for production.
        /// </summary>
        public bool DebugPanelEnabledAtStartup = false;

        /// <summary>
        /// Bypass boring and slow dialogs to fast game test
        /// Set to FALSE for production.
        /// </summary>
        public bool BypassDialogs = false;

        /// <summary>
        /// Switches on all Debug.Log calls for performance.
        /// Set to FALSE for production.
        /// </summary>
        public bool DebugLogEnabled = true;

        [Header("Tutorial")]
        public bool SimulateFirstContact;
        public FirstContactPhase SimulateFirstContactPhase;

        [Header("Verbose")]
        public bool VerboseBook;
        public bool VerboseAudio;

        [Header("AI")]
        public bool VerboseTeacher;
        public bool VerboseMinigameSelection;
        public bool VerboseDifficultySelection;
        public bool VerboseQuestionPacks;
        public bool VerboseDataFiltering;
        public bool VerboseDataSelection;
        public bool VerbosePlaySessionInitialisation;
        public bool TeacherSafetyFallbackEnabled = true;

    }
}
