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

    [CreateAssetMenu(menuName = "Antura/Config App Edition")]
    public class AppEditionConfig : ScriptableObject
    {
        [Header("Edition")]
        public AppEditionID editionID;
        public string EditionTitle;

        public ContentEditionConfig[] ContentEditions;
        public bool HasMultipleContentEditions => ContentEditions != null && ContentEditions.Length > 1;

        [Header("Settings")]
        [Space(30)]
        public bool ShowAccents = false;

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

        // If true, the reserved area panel will appear with two languages
        public bool HelpTextInReservedArea;

        // Show a translation of the Keeper widget based on the "Help" language?
        public bool ShowKeeperTranslation;

        // If we can skip subtitles and this is
        //  - true: clicking once skips only one of the two dialogues that are read for the two languages
        //  - false: clicking once skips all languages at once
        public bool SkipSingleLanguage => false;

        [Header("Player Data")]
        public bool RequireGender;
        public bool RequireAge;

        [Header("MiniGames")]
        public bool PlayTitleAtMiniGameStart;
        public bool PlayIntroAtMiniGameStart;
        public bool AutomaticDifficulty;

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
        [Header("Player Settings")]
        public bool ChangePlayerSettings = false;

#if UNITY_EDITOR
        [DeMethodButton("Configure as Active Edition")]
        public void ConfigureForBuild()
        {
            var config = ApplicationConfig.FindMainConfig();
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

            // Move folders based on language...
            var languagesToUse = new HashSet<LanguageCode>();
            foreach (var edition in learningConfigsToUse)
            {
                languagesToUse.Add(edition.NativeLanguage);
                languagesToUse.UnionWith(edition.SupportedNativeLanguages);
                languagesToUse.Add(edition.LearningLanguage);
                languagesToUse.Add(edition.HelpLanguage);
            }

            for (int iLang = 0; iLang < (int)LanguageCode.COUNT; iLang++)
            {
                var langCode = (LanguageCode)iLang;
                if (langCode == LanguageCode.NONE)
                    continue;

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

#endif
    }
}
