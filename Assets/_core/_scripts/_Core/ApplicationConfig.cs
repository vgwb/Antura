using Antura.Profile;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Antura.Core
{
    public class ApplicationConfig : ScriptableObject
    {
#if UNITY_EDITOR

        public static ApplicationConfig FindMainConfig()
        {
            var configPath = $"Assets/_config/ApplicationConfig.asset";
            var config = AssetDatabase.LoadAssetAtPath<ApplicationConfig>(configPath);
            if (config == null)
            {
                Debug.LogError($"Could not find ApplicationConfig at path '{configPath}'");
                return null;
            }
            return config;
        }

#endif

        public static ApplicationConfig I => AppManager.I.ApplicationConfig;

        public EditionConfig LoadedEdition;
        public EditionConfig UsedEdition => LoadedEdition; // TODO: detect MULTI, and use the APPSETTINGS in that case

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
        public bool TeacherSafetyFallbackEnabled = true;

    }
}