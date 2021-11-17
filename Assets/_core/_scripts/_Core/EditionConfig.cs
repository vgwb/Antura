using Antura.Language;
using Antura.Keeper;
using UnityEngine;
using System;
using Antura.Database;
using DG.DeInspektor.Attributes;
#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;

#endif

namespace Antura.Core
{
    public enum EditionResourceID
    {
        Flag,
    }

    [CreateAssetMenu]
    public class EditionConfig : ScriptableObject
    {
        [Header("Edition")]
        public AppEditions Edition;
        public string EditionTitle;

        public LearningConfig[] LearningEditions;
        public bool HasMultipleLearningEditions => LearningEditions != null && LearningEditions.Length > 1;

        [Header("Settings")]
        [Space(30)]
        public bool ShowAccents = false;
        public TextAsset CreditsText;

        [Header("Settings - Book")]
        public bool BookShowRelatedWords = false;

        [Header("Settings - Reserved Area")]
        public bool ReservedAreaForcedSeq = false;
        public bool ShowDonate = false;
        public bool ShowTeacherGuide = false;

        [Header("Settings - Subtitles")]
        public KeeperMode DefaultKeeperMode;

        public bool CanUseSubtitles =>
            DefaultKeeperMode == KeeperMode.LearningAndSubtitles
                                       || DefaultKeeperMode == KeeperMode.SubtitlesOnly
                                       || DefaultKeeperMode == KeeperMode.NativeAndSubtitles
                                       || DefaultKeeperMode == KeeperMode.LearningThenNativeAndSubtitles;

        // If true, subtitles can be skipped by clicking on them
        public bool AllowSubtitleSkip;

        // Allows the user to toggle subtitles on/off in the Options panel
        public bool EnableSubtitlesToggle;

        // Show various text around the application based on the "Help" language? (Book, GamesSelector, PromptPanel)
        public bool ShowHelpText;

        // Show a translation of the Keeper widget based on the "Help" language?
        public bool ShowKeeperTranslation;

        // If we can skip subtitles and this is
        //  - true: clicking once skips only one of the two dialogues that are read for the two languages
        //  - false: clicking once skips all languages at once
        public bool SkipSingleLanguage => false;

        [Header("Player Data")]
        public bool RequireGender;
        public bool RequireAge;

        [Header("Modules")]
        [Tooltip("add compilation symbol: MODULE_NOTIFICATIONS")]
        public bool EnableNotifications;

        /// <summary>
        /// Tracks common events using Unity Analytics.
        /// Set to TRUE for production.
        /// </summary>
        public bool OnlineAnalyticsEnabled = false;

        [Header("MiniGames")]
        public bool PlayTitleAtMiniGameStart;
        public bool PlayIntroAtMiniGameStart;
        public bool AutomaticDifficulty;

        [Header("Build Settings")]

        /// <summary>
        /// Version of the application. Displayed in the Home scene.
        /// Major.Minor.Patch.Build
        /// </summary>
        [Tooltip("Major.Minor.Patch.Build")]
        public string AppVersion = "0.0.0.0";

        public string GetAppVersionString()
        {
            var VersionArray = AppVersion.Split('.');
            string v = string.Format("{0}.{1}.{2} ({3})", VersionArray[0], VersionArray[1], VersionArray[2], VersionArray[3]);
            if (EditionTitle != "") {
                v += " " + EditionTitle;
            }
            return v;
        }

        /// <summary>
        /// Auto-generated at each new Cloud build
        /// </summary>
        [ReadOnly]
        public string CloudManifest = "NONE";

        public string ProductName;
        public string UnityProjectId;
        public string Android_BundleIdentifier;
        public string iOS_BundleIdentifier;
        public string BundleVersion;

        public Texture2D Android_AppIcon;
        public Texture2D iOS_AppIcon;
        public Sprite[] SplashLogos;

#if UNITY_EDITOR
        [DeMethodButton("Configure as Active Edition")]
        public void ConfigureForBuild()
        {
            var config = ApplicationConfig.FindMainConfig();
            if (config == null) return;

            config.LoadedAppEdition = this;
            PlayerSettings.productName = ProductName;
            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, Android_BundleIdentifier);
            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.iOS, iOS_BundleIdentifier);
            PlayerSettings.bundleVersion = BundleVersion;
            PlayerSettings.SetIconsForTargetGroup(BuildTargetGroup.Android, new[] { Android_AppIcon });
            PlayerSettings.SetIconsForTargetGroup(BuildTargetGroup.iOS, new[] { iOS_AppIcon });
            PlayerSettings.SplashScreen.logos = new PlayerSettings.SplashScreenLogo[SplashLogos.Length];
            for (int i = 0; i < SplashLogos.Length; i++) {
                PlayerSettings.SplashScreen.logos[i].logo = SplashLogos[i];
            }

            var learningConfigsToUse = new List<LearningConfig>();
            foreach (var edition in LearningEditions) {
                learningConfigsToUse.Add(edition);
            }

            // Move folders based on language...
            var languagesToUse = new HashSet<LanguageCode>();
            foreach (var edition in learningConfigsToUse) {
                languagesToUse.Add(edition.NativeLanguage);
                languagesToUse.UnionWith(edition.SupportedNativeLanguages);
                languagesToUse.Add(edition.LearningLanguage);
                languagesToUse.Add(edition.HelpLanguage);
            }

            for (int iLang = 0; iLang < (int)LanguageCode.COUNT; iLang++)
            {
                var langCode = (LanguageCode)iLang;
                if (langCode == LanguageCode.NONE) continue;

                string pascalcaseName = langCode.ToString();
                var words = pascalcaseName.Split('_');
                pascalcaseName = "";
                for (var i = 0; i < words.Length; i++)
                {
                    var word = words[i];
                    pascalcaseName += $"{Char.ToUpperInvariant(word[0])}{word.Substring(1)}";
                    if (i < words.Length - 1) pascalcaseName += "_";
                }

                var assetGroupSchemaPath =  $"Assets/AddressableAssetsData/AssetGroups/Schemas/{pascalcaseName}_BundledAssetGroupSchema.asset";
                var schema = AssetDatabase.LoadAssetAtPath<BundledAssetGroupSchema>(assetGroupSchemaPath);

                if (schema == null)
                {
                    Debug.LogWarning($"Language {langCode} is not setup with Addressables. No schema found at '{assetGroupSchemaPath}'");
                    continue;
                }

                if (languagesToUse.Contains(langCode)) {
                    schema.IncludeInBuild = true;
                    Debug.Log($"Enabling language: {langCode}");
                } else {
                    schema.IncludeInBuild = false;
                    Debug.Log($"Disabling language: {langCode}");
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.LogWarning($"Set '{EditionTitle}' as active Edition");
        }

        [DeMethodButton("Configure as Active & Rebuild Addressables")]
        public void ConfigureForBuildAndBuild()
        {
            ConfigureForBuild();

            AddressableAssetSettings.CleanPlayerContent();
            AddressableAssetSettings.BuildPlayerContent();
            Debug.LogWarning($"Rebuilt addressables");
        }


        /*[DeMethodButton("Test set cloud config")]
        public void TestSetCloudConfig()
        {
            var config = ApplicationConfig.FindMainConfig();
            if (config == null) return;
            config.LoadedEdition.CloudManifest = "TEST CLOUD MANIFEST";
            AssetDatabase.SaveAssets();
        }*/

#endif
    }
}