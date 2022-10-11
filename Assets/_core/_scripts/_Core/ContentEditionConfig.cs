using Antura.Database;
using Antura.Language;
using Antura.Database.Management;
using Antura.GoogleSheets;
using UnityEngine;
using UnityEngine.Serialization;
using System.Linq;
using System.Collections.Generic;

namespace Antura.Core
{
    [CreateAssetMenu(menuName = "Antura/Config Content Edition")]
    public class ContentEditionConfig : ScriptableObject
    {
        public LearningContentID ContentID;
        public string Title;
        public LearnMethodConfig LearnMethod;

        [Header("Language")]
        public LanguageCode LearningLanguage;
        public LangConfig LearningLanguageConfig;

        [FormerlySerializedAs("SupportedNativeLanguages")] public LanguageCode[] OverridenNativeLanguages;

        public bool SupportsLanguage(LanguageCode code)
        {
            bool supportsAnyLanguage = OverridenNativeLanguages.Length == 0;
            if (supportsAnyLanguage)
            {
                var langConfig = AppManager.I.LanguageSwitcher.GetLangConfig(code);
                if (langConfig.ExtraLanguage)
                {
                    return false;
                }

                if (!LearnMethod.CanUseLearningAsNative)
                {
                    return code != LearningLanguage;
                }
                return true;
            }
            else
            {
                return OverridenNativeLanguages.Contains(code);
            }
        }

        public LanguageCode HelpLanguage;

        [Header("Teacher Options")]
        // @note: this also makes all LetterPhoneme games use Diacritics ONLY, as only those have phonemes
        [Tooltip("If set, diacritics will appear only on isolated letters, and not any forms. Affects LetterAny and LetterPhoneme variations.")]
        public bool DiacriticsOnlyOnIsolated;
        [Tooltip("If set, LetterForm variations and assessments will use name sounds instead of phoneme sounds.")]
        public bool PlayNameSoundWithForms;
        [Tooltip("Will accented letters show their accent when placed on living letters? If not, they will ignore accents.")]
        public bool ShowAccentsOnSeparatedLivingLetters;

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

        [Header("In-Game Resources")]
        public Sprite HomeLogo;
        public Sprite TransitionLogo;
        public GameObject Flag3D;

        [Header("Database Data Files to Import")]
        public List<GoogleSheetRef> GoogleSheets;

        public GameObject GetResource(EditionResourceID id)
        {
            switch (id)
            {
                case EditionResourceID.Flag:
                    return Flag3D;
            }
            return null;
        }
    }
}
