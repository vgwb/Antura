using Antura.Language;
using UnityEngine;

namespace Antura.Core
{
    public class BotConfig : ScriptableObject
    {
        public bool AutoStart;
        public bool BotEnabled;
        public float Delay;

        public float GameSpeed;
        public float MinigamePlayDelay;

        public bool CreateNewProfile;

        [Header("Actions")]
        public bool PlayJourney;
        public bool PlayAllGamesInBook;

        public bool EnableStopBeforeJP;
        public JourneyPosition StopBeforeJP;

        //public bool SelectEdition;
        //public LanguageCode NativeLanguage;
        //public LearningContentID LearningContent;
    }
}
