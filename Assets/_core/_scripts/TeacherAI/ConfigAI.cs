using Antura.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Teacher
{
    /// <summary>
    /// Constants used to configure the Teacher System.
    /// </summary>
    public static class ConfigAI
    {
        // Reporting
        public static bool VerboseTeacher => DebugConfig.I.VerboseTeacher;

        // these depends on main VerboseTeacher bool
        public static bool VerboseMinigameSelection => DebugConfig.I.VerboseMinigameSelection;
        public static bool VerboseDifficultySelection => DebugConfig.I.VerboseDifficultySelection;
        public static bool VerboseQuestionPacks => DebugConfig.I.VerboseQuestionPacks;
        public static bool VerboseDataFiltering => DebugConfig.I.VerboseDataFiltering;
        public static bool VerboseDataSelection => DebugConfig.I.VerboseDataSelection;
        public static bool VerbosePlaySessionInitialisation => DebugConfig.I.VerbosePlaySessionInitialisation;

        // If true, the Teacher will keep retrying if it encounters a selection error, to avoid blocking the game
        // @note: this may HANG the game if an error keeps appearing, so use it only for extreme cases!
        public static bool TeacherSafetyFallbackEnabled => DebugConfig.I.TeacherSafetyFallbackEnabled;

        // If true, the journey progression logic is turned off, so that all data is usable
        public static bool ForceJourneyIgnore = false;

        // General configuration
        // Days at which we get the maximum malus for a recent play weight
        public const int DaysForMaximumRecentPlayMalus = 4;

        // MiniGame selection weights
        public const float MiniGame_PlaySession_Weight = 1f;
        public const float MiniGame_RecentPlay_Weight = 1f;

        // Vocabulary data selection weights
        public const float Vocabulary_Score_Weight = 1f;
        public const float Vocabulary_RecentPlay_Weight = 1f;
        public const float Vocabulary_CurrentPlaySession_Weight = 10f;
        // the minimal value for this Weight
        public const float Vocabulary_MinTotal_Weight = 0.1f;

        // Difficulty selection weights
        public const float Difficulty_Age_Weight = 0f;
        public const float Difficulty_Performance_Weight = 1f;
        public const float StartingDifficultyForNewMiniGame = 0f;

        public const int LastScoresForPerformanceWindow = 10;
        public const float ScoreStarsToDifficultyContribution = 0.15f;

        // Logging
        public const int ScoreMovingAverageWindow = 5;


        private static string teacherReportString;

        public static void StartTeacherReport()
        {
            teacherReportString = "";
        }

        public static string FormatTeacherReportHeader(string s)
        {
            return "[Teacher] - " + s + " --------";
        }

        public static void AppendToTeacherReport(string s)
        {
            if (VerboseTeacher)
            {
                teacherReportString += "\n\n" + s;
            }
        }

        public static void PrintTeacherReport(bool logOnly = false)
        {
            if (VerboseTeacher)
            {
                teacherReportString = "----- TEACHER REPORT " + DateTime.Now + "----" + teacherReportString;
                Debug.Log(teacherReportString);
#if UNITY_EDITOR
                if (!logOnly)
                {
                    System.IO.File.WriteAllText(Application.persistentDataPath + "/teacher_report.txt", teacherReportString);
                }
#endif
            }
        }

        public static void ReportPacks(List<QuestionPackData> packs)
        {
            if (VerboseQuestionPacks)
            {
                string packsString = FormatTeacherReportHeader("Generated Packs");
                for (int i = 0; i < packs.Count; i++)
                {
                    packsString += "\n" + (i + 1) + ": " + packs[i];
                }
                AppendToTeacherReport(packsString);
            }
        }
    }
}
