using Antura.Profile;
using UnityEngine;

namespace Antura.Core
{
    public class DebugConfig : ScriptableObject
    {
        /// <summary>
        /// Enables the Advanced Debug Panel (click bottom right corner of the screen to activate)
        /// Set to FALSE for production.
        /// </summary>
        public bool DebugPanelEnabledAtStartup = false;

        /// <summary>
        /// Bypass boring and slow dialogs to fast game test
        /// Set to FALSE for production.
        /// </summary>
        public bool DebugBypassDialogs = false;

        /// <summary>
        /// Switches on all Debug.Log calls for performance.
        /// Set to FALSE for production.
        /// </summary>
        public bool DebugLogEnabled = true;

        [Header("Debug - Verbose")]
        public bool VerboseBook;

        public bool VerboseAudio;

        [Header("Debug - Tutorial")]
        public bool SimulateFirstContact;

        public FirstContactPhase SimulateFirstContactPhase;

        [Header("Debug - AI")]
        public bool VerboseTeacher;

        public bool VerboseMinigameSelection;
        public bool VerboseDifficultySelection;
        public bool VerboseQuestionPacks;
        public bool VerboseDataFiltering;
        public bool VerboseDataSelection;
        public bool VerbosePlaySessionInitialisation;
        public bool TeacherSafetyFallbackEnabled = true;

        public static DebugConfig I => AppManager.I.DebugConfig;


    }
}