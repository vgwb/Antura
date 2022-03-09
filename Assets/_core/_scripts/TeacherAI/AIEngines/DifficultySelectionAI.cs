using Antura.Core;
using Antura.Database;
using Antura.Profile;
using UnityEngine;

namespace Antura.Teacher
{
    /// <summary>
    /// Handles the selection of the difficulty to use for a given minigame
    /// </summary>
    public class DifficultySelectionAI
    {

        // References
        //private DatabaseManager dbManager;
        //private PlayerProfile playerProfile;

        // Weights
        //private float ageWeightContribution = ConfigAI.Difficulty_Age_Weight;                   // Higher age -> higher difficulty
        //private float performanceWeightContribution = ConfigAI.Difficulty_Performance_Weight;  // Higher performance -> higher difficulty

        public DifficultySelectionAI(DatabaseManager _dbManager)
        {
            //dbManager = _dbManager;
        }

        public void SetPlayerProfile(PlayerProfile _playerProfile)
        {
            // playerProfile = _playerProfile;
        }

        public float SelectDifficulty(MiniGameCode miniGameCode)
        {
            return 0;   // Force difficulty, always zero

            /*
            float totalWeight = ageWeightContribution + performanceWeightContribution;

            // Age
            var playerAge = playerProfile.Age;
            float ageDifficulty = Mathf.Clamp01(Mathf.InverseLerp(AppConfig.MinPlayerAge, AppConfig.MaxPlayerAge, playerAge));
            float weightedAgeDifficulty = ageDifficulty * ageWeightContribution / totalWeight;

            // Performance
            float playerPerformance;
            string query = string.Format("SELECT * FROM " + typeof(MiniGameScoreData).Name + " WHERE MiniGameCode = '{0}'", (int)miniGameCode);
            var minigame_scoreData_list = dbManager.Query<MiniGameScoreData>(query);
            if (minigame_scoreData_list.Count == 0) {
                playerPerformance = ConfigAI.StartingDifficultyForNewMiniGame;
                //Debug.Log("No previous scores");
            } else {
                // We use a custom logic to define the difficulty:
                // - start from performance = 0
                // - get last N scores for the minigame
                // - a score of 0 diminishes the performance
                // - a score of 1 does not change it
                // - a score of 2 or 3 increases it

                // Query on last X minigame logged scores
                string query2 = "SELECT * FROM " + typeof(LogMiniGameScoreData).Name + " WHERE MiniGameCode = " + (int)miniGameCode + " ORDER BY Timestamp LIMIT " + ConfigAI.LastScoresForPerformanceWindow;
                var logMinigameScoreDataList = dbManager.Query<LogMiniGameScoreData>(query2);
                var scores = logMinigameScoreDataList.ConvertAll(x => x.Stars);
                //Debug.Log("Found " + (scores.Count) + " previous scores");

                // Diminish to create the weights [-1, 0, 1, 2]
                for (var i = 0; i < scores.Count; i++) {
                    scores[i] -= 1;
                }

                // Compute the performance for these minigames starting from zero and adding values
                playerPerformance = 0f;
                for (var i = 0; i < scores.Count; i++) {
                    playerPerformance += scores[i] * ConfigAI.ScoreStarsToDifficultyContribution;
                    //Debug.LogWarning("Score " + i + " was " + (scores[i] + 1) + " contrib: " + scores[i] * scorePointsContribution + " current " + playerPerformance);
                }
                playerPerformance = Mathf.Clamp01(playerPerformance);
            }
            float performanceDifficulty = playerPerformance;
            float weightedPerformanceDifficulty = performanceDifficulty * performanceWeightContribution / totalWeight;

            // Total
            float totalDifficulty = weightedAgeDifficulty + weightedPerformanceDifficulty;

            // Debug log
            if (ConfigAI.VerboseDifficultySelection) {
                var debugString = ConfigAI.FormatTeacherReportHeader("Selected Difficulty : " + totalDifficulty);
                debugString += "\n From Age (C " + ageWeightContribution + "): " + ageDifficulty + " w(" + weightedAgeDifficulty + ")";
                debugString += "\n From Performance (C " + performanceWeightContribution + "): " + performanceDifficulty + " w(" + weightedPerformanceDifficulty + ")";
                ConfigAI.AppendToTeacherReport(debugString);
            }

            return totalDifficulty;*/
        }

    }
}
