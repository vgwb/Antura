using Antura.Language;
using Antura.Keeper;
using UnityEngine;
using System;
using Antura.Database;

namespace Antura.Core
{
    [CreateAssetMenu]
    public class EditionConfig : ScriptableObject
    {
        public static EditionConfig I => AppManager.I.EditionConfig;

        [Header("Edition")]
        public AppEditions Edition;
        public string EditionTitle;

        public string ProductName;
        public string BundleIdentifier;

        /// <summary>
        /// Version of the application. Displayed in the Home scene.
        /// Major.Minor.Patch.Build
        /// </summary>
        [Tooltip("Major.Minor.Patch.Build")]
        public string AppVersion = "0.0.0.0";

        [Header("Settings")]
        public bool ReservedAreaForcedSeq = false;

        [Header("Sprites")]
        public Sprite HomeLogo;
        public Sprite TransitionLogo;

        [Header("Language")]
        public LanguageCode LearningLanguage;
        public LanguageCode NativeLanguage;
        public LanguageCode SubtitlesLanguage;

        public LanguageCode[] SupportedNativeLanguages;
        [Tooltip("try to set the native language to the device language, otherwise use NativeLanguage")]
        public bool DetectSystemLanguage;

        public bool ShowSubtitles;
        public KeeperMode DefaultKeeperMode;

        // If true, subtitles can be skipped by clicking on them 
        public bool AllowSubtitleSkip;

        // If we can skip subtitles and this is
        //  - true: clicking once skips only one of the two dialogues that are read for the two languages
        //  - false: clicking once skips all languages at once
        public bool SkipSingleLanguage => false;

        public readonly bool ForceALLCAPSTextRendering = true;

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

        public string GetLearningLangResourcePrefix()
        {
            return LearningLanguage.ToString() + "/";
        }

        public string GetAppVersionString()
        {
            var VersionArray = AppVersion.Split('.');
            string v = string.Format("{0}.{1}.{2} ({3})", VersionArray[0], VersionArray[1], VersionArray[2], VersionArray[3]);
            if (EditionTitle != "") {
                v += " " + EditionTitle;
            }

            return v;
        }

        [Header("Data - Vocabulary")]
        public LetterDatabase LetterDB;
        public WordDatabase WordDB;
        public PhraseDatabase PhraseDB;
        public LocalizationDatabase LocalizationDB;

        [Header("Data - Journey")]
        public StageDatabase StageDB;
        public LearningBlockDatabase LearningBlockDB;
        public PlaySessionDatabase PlaySessionDB;
        public MiniGameDatabase MiniGameDB;
        public RewardDatabase RewardDB;

    }
}