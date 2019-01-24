using UnityEngine;
using Antura.Language;

namespace Antura.Core
{

    [CreateAssetMenu]
    public class SAppConfig : ScriptableObject
    {
        public static SAppConfig I => AppManager.I.AppConfig;

        public LanguageCode LearningLanguage;
        public LanguageCode InstructionsLanguage;
        public LanguageCode TutorLanguage;

        [Header("Debug - AI")]
        public bool VerboseTeacher;
        public bool VerboseMinigameSelection;
        public bool VerboseDifficultySelection;
        public bool VerboseQuestionPacks;
        public bool VerboseDataFiltering;
        public bool VerboseDataSelection;
        public bool VerbosePlaySessionInitialisation;
    }
}