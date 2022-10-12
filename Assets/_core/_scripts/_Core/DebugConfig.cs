using Antura.Profile;
using UnityEngine;

namespace Antura.Core
{
    public class DebugConfig : ScriptableObject
    {
        public static DebugConfig I => AppManager.I.RootConfig.DebugConfig;

        /// <summary>
        /// generic settings (for analytics environment, for example)
        /// Set to FALSE for release.
        /// </summary>
        public bool DeveloperMode = false;

        [Header("Dev Mode (set all to FALSE for release)")]
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
        /// Speed-up some animations to fast game test
        /// Set to FALSE for production.
        /// </summary>
        public bool SpeedUpAnimations = false;

        /// <summary>
        /// Switches on all Debug.Log calls for performance.
        /// Set to FALSE for production.
        /// </summary>
        public bool DebugLogEnabled = true;

        /// <summary>
        /// If true, the initial load will be blocking. Used for editor use when not loading from bootstrap
        /// </summary>
        public bool AddressablesBlockingLoad => Application.isEditor;  // @note: always forced to false when out of the editor, or it hides slowdowns, but true in editor so play is fast

        [Header("Tutorial")]
        public bool SimulateFirstContact;
        public FirstContactPhase SimulateFirstContactPhase;

        [Header("Verbose")]
        public bool VerboseBook;
        public bool VerboseAudio;
        public bool VerboseAssetsManager;

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
