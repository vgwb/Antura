using Antura.Language;
using Antura.Database;
using System;
using System.Linq;
using UnityEngine;
using DG.DeInspektor.Attributes;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
#endif

namespace Antura.Core
{
    public enum EditionResourceID
    {
        Flag,
    }

    [CreateAssetMenu(menuName = "Antura/Config App Edition")]
    public class AppEditionConfig : ScriptableObject
    {
        [Header("Edition")]
        public AppEditionID editionID;
        public ContentEditionConfig[] ContentEditions;
        public LanguageCode[] SupportedNativeLanguages;
        public bool HasMultipleContentEditions => ContentEditions != null && ContentEditions.Length > 1;

        [Header("Settings - Application")]
        /// <summary>
        /// Version of the application. Displayed in the Home scene.
        /// Major.Minor.Patch.Build
        /// </summary>
        [Tooltip("The version of the application. (Major.Minor.Patch.Build)")]
        public string AppVersion = "0.0.0.0";

        public string GetAppVersionString()
        {
            var VersionArray = AppVersion.Split('.');
            string v = string.Format("{0}.{1}.{2} ({3})", VersionArray[0], VersionArray[1], VersionArray[2], VersionArray[3]);
            return v;
        }

        [Tooltip("Are local notifications enabled? (add compilation symbol: MODULE_NOTIFICATIONS)")]
        public bool EnableNotifications;

        [Tooltip("Are Online Analytics enabled?")]
        public bool OnlineAnalyticsEnabled = false;

        [Tooltip("The text asset that holds all credits for this version")]
        public TextAsset CreditsText;

        [Tooltip("Try to set the native language to the device language, otherwise use the default NativeLanguage")]
        public bool DetectSystemLanguage;

        [Header("Settings - Reserved Area")]

        [Tooltip("Use a forced sequence to access the reserved area, instead of a randomized one?")]
        public bool ReservedAreaForcedSeq;

        [Tooltip("Show the Donate button in the reserved area?")]
        public bool ShowDonate;

        [Tooltip("Show the Teacher Guide button in the reserved area?")]
        public bool ShowTeacherGuide;

        [Tooltip("If true, the reserved area panel will appear with two languages")]
        public bool HelpTextInReservedArea;

        [Tooltip("If true, the hidden debug button will be used to open the report UI")]
        public bool OpenBugReportOnHiddenButton = false;

        [Header("Player Profile")]
        [Tooltip("Require the Gender of the user when a profile is created")]
        public bool RequireGender;

        [Tooltip("Require the Age of the user when a profile is created")]
        public bool RequireAge;

        [Header("MiniGames")]
        public bool PlayTitleAtMiniGameStart;
        public bool PlayIntroAtMiniGameStart;
        public bool AutomaticDifficulty;

        [Header("Data")]
        public DrawingsData DrawingsData;
        /*
                [Header("Build Settings")]

                /// <summary>
                /// Auto-generated at each new Cloud build
                /// </summary>
                [ReadOnly]
                public string CloudManifest = "NONE";

                public string ProductName;
                public string UnityProjectId;
                public string Desktop_BundleIdentifier;
                public string Android_BundleIdentifier;
                public string iOS_BundleIdentifier;
                public string BundleVersion;

                public Texture2D Android_AppIcon;
                public Texture2D Android_AppIcon_Bg;
                public Texture2D Android_AppIcon_Fg;
                public Texture2D iOS_AppIcon;
                public Sprite[] SplashLogos;
        */
        //[Header("Player Settings")]
        //public bool ChangePlayerSettings = false;

#if UNITY_EDITOR
        [DeMethodButton("Configure as Active Edition")]
        public void ConfigureForBuild()
        {
            var config = RootConfig.FindMainConfig();
            if (config == null)
                return;

            config.LoadedAppEdition = this;
            // if (ChangePlayerSettings)
            // {
            //     PlayerSettings.productName = ProductName;
            //     PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Standalone, Desktop_BundleIdentifier);
            //     PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, Android_BundleIdentifier);
            //     PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.iOS, iOS_BundleIdentifier);
            //     PlayerSettings.bundleVersion = BundleVersion;
            //     PlayerSettings.SetIconsForTargetGroup(BuildTargetGroup.Android, new[] { Android_AppIcon });
            //     PlayerSettings.SetIconsForTargetGroup(BuildTargetGroup.iOS, new[] { iOS_AppIcon });
            //     PlayerSettings.SplashScreen.logos = new PlayerSettings.SplashScreenLogo[SplashLogos.Length];
            //     for (int i = 0; i < SplashLogos.Length; i++)
            //     {
            //         PlayerSettings.SplashScreen.logos[i].logo = SplashLogos[i];
            //     }
            // }
            var learningConfigsToUse = new List<ContentEditionConfig>();

            foreach (var edition in ContentEditions)
            {
                learningConfigsToUse.Add(edition);
            }

            var languagesToUse = new HashSet<LanguageCode>();
            languagesToUse.UnionWith(SupportedNativeLanguages);
            foreach (var edition in learningConfigsToUse)
            {
                languagesToUse.UnionWith(edition.OverridenNativeLanguages);
                languagesToUse.Add(edition.LearningLanguage);
                languagesToUse.Add(edition.HelpLanguage);
            }

            for (int iLang = 0; iLang < (int)LanguageCode.COUNT; iLang++)
            {
                var langCode = (LanguageCode)iLang;
                if (langCode == LanguageCode.NONE)
                    continue;

                FixAddressables(langCode.ToString());

                string pascalcaseName = langCode.ToString();
                var words = pascalcaseName.Split('_');
                pascalcaseName = "";
                for (var i = 0; i < words.Length; i++)
                {
                    var word = words[i];
                    pascalcaseName += $"{Char.ToUpperInvariant(word[0])}{word.Substring(1)}";
                    if (i < words.Length - 1)
                    {
                        pascalcaseName += "_";
                    }
                }

                var assetGroupSchemaPath = $"Assets/AddressableAssetsData/AssetGroups/Schemas/{pascalcaseName}_BundledAssetGroupSchema.asset";
                var schema = AssetDatabase.LoadAssetAtPath<BundledAssetGroupSchema>(assetGroupSchemaPath);

                if (schema == null)
                {
                    Debug.LogWarning($"Language {langCode} is not setup with Addressables. No schema found at '{assetGroupSchemaPath}'");
                    continue;
                }

                if (languagesToUse.Contains(langCode))
                {
                    schema.IncludeInBuild = true;
                    Debug.Log($"Enabling language: {langCode}");
                }
                else
                {
                    schema.IncludeInBuild = false;
                    Debug.Log($"Disabling language: {langCode}");
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.LogWarning($"Set '{editionID}' as active Edition");
        }

        [DeMethodButton("Configure as Active & Rebuild Addressables")]
        public void ConfigureForBuildAndBuild()
        {
            ConfigureForBuild();

            AddressableAssetSettings.CleanPlayerContent();
            AddressableAssetSettings.BuildPlayerContent();
            Debug.LogWarning($"Rebuilt addressables");
        }

        public static void FixAddressables(string lang)
        {
            // TODO: Get all assets in the lang paths for the current edition
            var guids = AssetDatabase.FindAssets("", new[] { "Assets/_lang_bundles/" + lang });
            Debug.Log("Fixing addressable for lang: " + lang);
            var group = AddressableAssetSettingsDefaultObject.Settings.groups.FirstOrDefault(x => x.name.ToLower().Contains(lang));
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                if (path.Contains("Resources/")) continue;
                var entry = AddressableAssetSettingsDefaultObject.Settings.CreateOrMoveEntry(guid, group);
                var removedPathLength = "Assets/_lang_bundles/".Length;
                var splits = path.Split('.');
                if (splits.Length > 1)
                {
                    var extension = splits[splits.Length - 1];
                    path = path.Substring(0, path.Length - (extension.Length + 1));
                }
                entry.address = path.Substring(removedPathLength, path.Length - removedPathLength);
            }
            Debug.Log("FINISHED Fixing addressable for lang: " + lang);
        }

#endif


    }
}
