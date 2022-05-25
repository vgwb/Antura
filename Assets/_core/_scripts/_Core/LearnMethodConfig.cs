using Antura.Database;
using Antura.Keeper;
using UnityEngine;
using UnityEngine.Serialization;

namespace Antura.Core
{
    public enum LearnMethodID
    {
        LearnToRead,
        LearnLanguage
    }

    [CreateAssetMenu(menuName = "Antura/Config Learn Method")]
    public class LearnMethodConfig : ScriptableObject
    {
        public LearnMethodID ID;
        public string TitleLocID;

        [Header("Settings - Language")]

        [Tooltip("If set, the Learning language can be the same as the Native language (e.g. learn english with english)")]
        public bool CanUseLearningAsNative;

        [Header("Settings - Scenes")]
        public bool ShowEndSceneBigText;
        public string EndSceneLocID;

        [Header("Settings - Subtitles")]
        [Tooltip("The mode in which the keeper will show text, by default")]
        public KeeperMode DefaultKeeperMode;

        public bool CanUseSubtitles =>
            DefaultKeeperMode == KeeperMode.LearningAndSubtitles
            || DefaultKeeperMode == KeeperMode.SubtitlesOnly
            || DefaultKeeperMode == KeeperMode.NativeAndSubtitles
            || DefaultKeeperMode == KeeperMode.LearningThenNativeAndSubtitles;

        [Tooltip("If true, subtitles can be skipped by clicking on them")]
        public bool AllowSubtitleSkip;

        [Tooltip("Allows the user to toggle subtitles on/off in the Options panel")]
        public bool EnableSubtitlesToggle;

        [Header("Settings - Help Text")]
        [Tooltip("Show various text around the application based on the Help language? (Book, GamesSelector, PromptPanel)")]
        public bool ShowHelpText;

        [Tooltip("Show a translation of the Keeper widget based on the Help language?")]
        public bool ShowKeeperTranslation;

        [Header("Settings - Book")]
        [Tooltip("Will letters show linked words in the book, or diacritic combos?")]
        public bool ShowLinkedWordsInBook = false;

        // If we can skip subtitles and this is
        //  - true: clicking once skips only one of the two dialogues that are read for the two languages
        //  - false: clicking once skips all languages at once
        public bool SkipSingleLanguage => false;
    }
}
