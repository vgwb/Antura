using Antura.Language;
using Antura.Profile;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Antura.Core
{
    // Entry point for configurations
    // @note: this is needed as we need to load it from the project when switching versions, regardless of which App Edition is used
    public class RootConfig : ScriptableObject
    {
#if UNITY_EDITOR

        public static RootConfig FindMainConfig()
        {
            var configPath = $"Assets/_config/Config Root.asset";
            var config = AssetDatabase.LoadAssetAtPath<RootConfig>(configPath);
            if (config == null)
            {
                Debug.LogError($"Could not find RootConfig at path '{configPath}'");
                return null;
            }
            return config;
        }

#endif

        public AppEditionConfig LoadedAppEdition;
        public DebugConfig DebugConfig;
        public ContentEditionConfig ContentEdition
        {
            get
            {
                var loadedContentID = AppManager.I.AppSettings.ContentID;
                if (loadedContentID == LearningContentID.None)
                {
                    Debug.Log("No saved content ID. Choosing the first available.");
                    AppManager.I.AppSettingsManager.SetLearningContentID(LoadedAppEdition.ContentEditions[0].ContentID);
                    return LoadedAppEdition.ContentEditions[0];
                }
                var config = LoadedAppEdition.ContentEditions.FirstOrDefault(x => x.ContentID == loadedContentID);
                if (config == null)
                {
                    Debug.Log($"No content with ID {loadedContentID} could be found. Reverting to the first available.");
                    AppManager.I.AppSettingsManager.SetLearningContentID(LoadedAppEdition.ContentEditions[0].ContentID);
                    return LoadedAppEdition.ContentEditions[0];
                }
                return config;
            }
        }


        public List<LanguageCode> LanguageSorting;
    }
}
