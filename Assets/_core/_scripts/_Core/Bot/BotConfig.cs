using System;
using System.Collections.Generic;
using Antura.Language;
using UnityEngine;

namespace Antura.Core
{
    [Serializable]
    public struct EditionCombo
    {
        public LearningContentID LearningContent;
        public LanguageCode NativeLanguage;
        public override string ToString() => $"Learn {LearningContent} with Native {NativeLanguage}";
    }

    public class BotConfig : ScriptableObject
    {
        public bool BotEnabled;
        public bool AutoStart;
        public float Delay;
        public float GameSpeed;

        [Header("Mini Games")]
        public float MinigamePlayDelay;

        [Header("Profile")]
        public bool DeleteExistingProfiles;
        public bool CreateNewProfile;
        public bool UseDemoProfile;

        [Header("Steps")]
        public bool StartTeacherTester;
        public bool CheckMissingAudio;
        public bool PlayAllGamesInBook;
        public bool PlayJourney;
        public bool EnableStopBeforeJP;
        public JourneyPosition StopBeforeJP;

        public bool TestEditionCombos;
        public List<EditionCombo> Combos = new List<EditionCombo>();

        [Header("Debug")]
        public bool DebugMode;

    }
}
