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

        public EditionConfig LoadedEdition;
        public EditionConfig SpecificEdition
        {
            get {
                if (LoadedEdition.IsMultiEdition) {
                    var wantedEdition = AppManager.I.AppSettings.SpecificEdition;
                    var editionConfig = LoadedEdition.ChildEditions.FirstOrDefault(ed => ed.Edition == wantedEdition);
                    if (editionConfig == null) {
                        AppManager.I.AppSettingsManager.SetSpecificEdition(LoadedEdition.ChildEditions[0].Edition);
                        editionConfig = LoadedEdition.ChildEditions[0];
                    }
                    return editionConfig;
                }
                return LoadedEdition;
            }
        }
    }
}