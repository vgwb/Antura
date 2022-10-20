using System;
using System.Collections.Generic;
using Antura.Language;
using UnityEngine;
using UnityEngine.Serialization;

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
        [Tooltip("Make the Bot take control of the game. Can be toggled at runtime.")]
        public bool BotEnabled;
        [Tooltip("If true, the bot will be enabled when the app starts.")]
        public bool AutoStart;

        [Header("Mini Games")]

        [Header("Profile")]
        public bool DeleteExistingProfiles;
        public bool CreateNewProfile;
        public bool UseDemoProfile;

        [Header("Steps")]
        [Tooltip("Enter the DB manage area and start the teacher tester.")]
        public bool StartTeacherTester;
        [Tooltip("Enter the DB manage area and trigger the Check Missing Audio script")]
        public bool CheckLearningMissingAudio;
        public bool PlayAllGamesInBook;
        public bool PlayJourney;
        public bool EnableStopBeforeJP;
        public JourneyPosition StopBeforeJP;

        public bool TestEditionCombos;
        public List<EditionCombo> Combos = new List<EditionCombo>();

        [Header("Debug")]
        [Tooltip("Seconds of play inside a minigame before skipping it")]
        public float MinigamePlayDelay;
        [Tooltip("Delay between actions for the bot to perform. Usually, clicks.")]
        public float Delay;
        [Tooltip("Speed of the game. Changes time scale (normal play is 1)")]
        public float GameSpeed;
        [Tooltip("Enable to see more debug information in the bot logs")]
        public bool DebugMode;

    }
}
