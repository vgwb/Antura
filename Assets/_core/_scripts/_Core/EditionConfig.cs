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

        [Header("Multi-Edition")]
        public EditionConfig[] ChildEditions;
        public bool IsMultiEdition => ChildEditions != null && ChildEditions.Length > 0;

        [Header("Specific - Language")]
        [Space(30)]
        public LanguageCode LearningLanguage;
        public LanguageCode NativeLanguage;
        public LanguageCode SubtitlesLanguage;
        public LanguageCode[] SupportedNativeLanguages;
        [Tooltip("try to set the native language to the device language, otherwise use NativeLanguage")]
        public bool DetectSystemLanguage;

        public string GetLearningLangResourcePrefix()
        {
            return LearningLanguage.ToString() + "/";
        }

        [Header("Specific - Data - Vocabulary")]
        public LetterDatabase LetterDB;
        public WordDatabase WordDB;
        public PhraseDatabase PhraseDB;
        public LocalizationDatabase LocalizationDB;

        [Header("Specific - Data - Journey")]
        public StageDatabase StageDB;
        public LearningBlockDatabase LearningBlockDB;
        public PlaySessionDatabase PlaySessionDB;
        public MiniGameDatabase MiniGameDB;
        public RewardDatabase RewardDB;

        [Header("Settings")]
        [Space(30)]
        public bool ShowAccents = false;
        public TextAsset CreditsText;

        [Header("Settings - Reserved Area")]
        public bool ReservedAreaForcedSeq = false;
        public bool ShowDonate = false;
        public bool ShowTeacherGuide = false;

        [Header("Settings - Subtitles")]
        public KeeperMode DefaultKeeperMode;
        public bool ShowSubtitles;

        // If true, subtitles can be skipped by clicking on them
        public bool AllowSubtitleSkip;

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

        [Header("In-Game Resources")]
        public Sprite EditionIcon;
        public Sprite HomeLogo;
        public Sprite TransitionLogo;
        public GameObject Flag3D;

        public GameObject GetResource(EditionResourceID id)
        {
            switch (id) {
                case EditionResourceID.Flag: return Flag3D;
            }
            return null;
        }


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

            if (IsMultiEdition) {
                v += " - " + AppManager.I.SpecificEdition.EditionTitle;
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

            config.LoadedEdition = this;
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

            List<EditionConfig> editionsToUse = new List<EditionConfig>();
            if (IsMultiEdition) {
                foreach (var edition in ChildEditions) {
                    editionsToUse.Add(edition);
                }
            } else {
                editionsToUse.Add(this);
            }

            // Move folders based on language...
            var languagesToUse = new HashSet<LanguageCode>();
            foreach (var edition in editionsToUse) {
                languagesToUse.Add(edition.NativeLanguage);
                languagesToUse.UnionWith(edition.SupportedNativeLanguages);
                languagesToUse.Add(edition.LearningLanguage);
            }

            for (int iLang = 0; iLang < (int)LanguageCode.COUNT; iLang++) {
                var langCode = (LanguageCode)iLang;

                var usePath = $"{Application.dataPath}/_config/Resources/{langCode}";
                var unusePath = $"{Application.dataPath}/_config/Resources_unused/{langCode}";

                if (languagesToUse.Contains(langCode)) {
                    if (!Directory.Exists(usePath) && Directory.Exists(unusePath)) {
                        Debug.Log("Enabling language: " + langCode);
                        Directory.Move(unusePath, usePath);
                    }
                } else {
                    if (!Directory.Exists(unusePath) && Directory.Exists(usePath)) {
                        Debug.Log("Disabling language: " + langCode);
                        Directory.Move(usePath, unusePath);
                    }
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.LogWarning($"Set '{EditionTitle}' as active Edition");
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