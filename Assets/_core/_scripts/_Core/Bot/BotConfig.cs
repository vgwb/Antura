using Antura.Language;
using UnityEngine;

namespace Antura.Core
{
    public class BotConfig : ScriptableObject
    {
        public bool AutoStart;
        public float Delay;

        public float GameSpeed;
        public float MinigamePlayDelay;

        public bool EnableStopBeforeJP;
        public JourneyPosition StopBeforeJP;

        public LanguageCode NativeLanguage;
        public LanguageCode LearningLanguage;
    }
}
