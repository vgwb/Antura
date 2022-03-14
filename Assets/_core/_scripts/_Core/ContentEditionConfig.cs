using Antura.Database;
using Antura.Language;
using UnityEngine;

namespace Antura.Core
{
    [CreateAssetMenu(menuName = "Antura/Config Content Edition")]
    public class ContentEditionConfig : ScriptableObject
    {
        public LearningContentID ContentID;
        public Sprite Icon;
        public string Title;

        [Header("Language")]
        public LanguageCode LearningLanguage;
        public LanguageCode NativeLanguage;
        public LanguageCode HelpLanguage;
        public LanguageCode[] SupportedNativeLanguages;
        [Tooltip("try to set the native language to the device language, otherwise use NativeLanguage")]
        public bool DetectSystemLanguage;

        [Header("Teacher Options")]
        // @note: this also makes all LetterPhoneme games use Diacritics ONLY, as only those have phonemes
        [Tooltip("If set, diacritics will appear only on isolated letters, and not any forms. Affects LetterAny and LetterPhoneme variations.")]
        public bool DiacriticsOnlyOnIsolated;
        [Tooltip("If set, LetterForm variations and assessments will use name sounds instead of phoneme sounds.")]
        public bool PlayNameSoundWithForms;

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
