using Antura.Profile;
using UnityEngine;

namespace Antura.Core
{
    public class ApplicationConfig : ScriptableObject
    {
        public static ApplicationConfig I => AppManager.I.ApplicationConfig;

        public EditionConfig LoadedConfig;

        [Header("Debug")]
        /// <summary>
        /// Enabled the Advanced Debug Panel.
        /// Set to FALSE for production.
        /// </summary>
        public bool DebugPanelEnabledAtStartup = false;

        /// <summary>
        /// Switches on all Debug.Log calls for performance.
        /// Set to FALSE for production.
        /// </summary>
        public bool DebugLogEnabled = false;

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
    }
}