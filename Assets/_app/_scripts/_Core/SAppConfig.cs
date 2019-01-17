using UnityEngine;

namespace Antura.Core
{

    [CreateAssetMenu]
    public class SAppConfig : ScriptableObject
    {
        public static SAppConfig I => AppManager.I.AppConfig;

        public LanguageCode LearningLanguage;
        public LanguageCode InstructionsLanguage;
        public LanguageCode TutorLanguage;
    }
}