using System.Linq;
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
            if (config == null) {
                Debug.LogError($"Could not find ApplicationConfig at path '{configPath}'");
                return null;
            }
            return config;
        }

#endif

        public static ApplicationConfig I => AppManager.I.ApplicationConfig;

        public EditionConfig LoadedAppEdition;
        public LearningConfig LearningEdition
        {
            get
            {
                var index = Mathf.Min(AppManager.I.AppSettings.LearningEditionIndex, LoadedAppEdition.LearningEditions.Length - 1);
                var config = LoadedAppEdition.LearningEditions[index];
                return config;
            }
        }
    }
}