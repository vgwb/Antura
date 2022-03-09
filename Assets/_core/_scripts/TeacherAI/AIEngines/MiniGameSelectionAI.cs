using System.Collections.Generic;
using Antura.Database;
using Antura.Helpers;

namespace Antura.Teacher
{
    /// <summary>
    /// Handles the selection of what minigames to play during a playsession
    /// </summary>
    public class MiniGameSelectionAI
    {
        // References
        private DatabaseManager dbManager;

        public MiniGameSelectionAI(DatabaseManager _dbManager)
        {
            dbManager = _dbManager;
        }

        public void InitNewPlaySession()
        {
            // Nothing to be done here
        }

        public List<MiniGameData> PerformSelection(string playSessionId, int numberToSelect)
        {
            PlaySessionData playSessionData = dbManager.GetPlaySessionDataById(playSessionId);

            List<MiniGameData> selectedMiniGameData = null;
            switch (playSessionData.Order)
            {
                case PlaySessionDataOrder.Sequence:
                    selectedMiniGameData = PerformSelection_Sequence(playSessionData, numberToSelect);
                    break;
                case PlaySessionDataOrder.Random:
                    selectedMiniGameData = PerformSelection_Random(playSessionData, numberToSelect);
                    break;
            }

            return selectedMiniGameData;
        }

        private List<MiniGameData> PerformSelection_Sequence(PlaySessionData playSessionData, int numberToSelect)
        {
            // Get all minigame codes for the given playsession
            // ... also, use the weights to determine insertion order (used to determine the sequential order)
            var ordered_minigamecodes = new SortedDictionary<float, MiniGameCode>();
            int fakeNumber = 1000;
            foreach (var minigameInPlaySession in playSessionData.Minigames)
            {
                if (ordered_minigamecodes.ContainsKey(minigameInPlaySession.Weight))
                {
                    ordered_minigamecodes[fakeNumber] = minigameInPlaySession.MiniGameCode;
                    fakeNumber++;
                }
                else
                {
                    ordered_minigamecodes[minigameInPlaySession.Weight] = minigameInPlaySession.MiniGameCode;
                }
            }

            // Get, in order, each minigame data, filter by availability (from the static DB)
            var minigame_data_list = new List<MiniGameData>();
            foreach (var orderedPair in ordered_minigamecodes)
            {
                var data = dbManager.GetMiniGameDataByCode(orderedPair.Value);
                if (data.CanBeSelected)
                {
                    minigame_data_list.Add(data);
                }
            }

            // Number checks
            int actualNumberToSelect = UnityEngine.Mathf.Min(numberToSelect, minigame_data_list.Count);
            if (minigame_data_list.Count == 0)
            {
                throw new System.Exception("Cannot find even a single minigame for play session " + playSessionData.Id);
            }
            if (numberToSelect > minigame_data_list.Count)
            {
                UnityEngine.Debug.LogWarning("Could not select the requested number of " + numberToSelect + " minigames for play session " + playSessionData.Id + " (only " + minigame_data_list.Count + " are available)");
            }

            // Choose the first N minigames in the ordered list
            var selectedMiniGameData = minigame_data_list.GetRange(0, actualNumberToSelect);
            return selectedMiniGameData;
        }

        private List<MiniGameData> PerformSelection_Random(PlaySessionData playSessionData, int numberToSelect)
        {
            // Get all minigames ids for the given playsession (from PlaySessionData)
            // ... also, keep the weights around
            var minigame_id_list = new List<string>();
            var playsession_weights_dict = new Dictionary<MiniGameCode, float>();

            foreach (var minigameInPlaySession in playSessionData.Minigames)
            {
                minigame_id_list.Add(minigameInPlaySession.MiniGameCode.ToString());
                playsession_weights_dict[minigameInPlaySession.MiniGameCode] = minigameInPlaySession.Weight;
            }

            // Get all minigame data, filter by availability (from the static DB)
            var minigame_data_list = dbManager.FindMiniGameData(x => x.CanBeSelected && minigame_id_list.Contains(x.GetId()));

            // Create the weights list too
            var weights_list = new List<float>(minigame_data_list.Count);

            // Retrieve the current score data (state) for each minigame (from the dynamic DB)
            var minigame_score_list = dbManager.Query<MiniGameScoreData>("SELECT * FROM " + nameof(MiniGameScoreData));

            //UnityEngine.Debug.Log("M GAME SCORE LIST: " + minigame_score_list.Count);
            //foreach(var l in minigame_score_list) UnityEngine.Debug.Log(l.ElementId);

            // Determine the final weight for each minigame
            var required_minigames = new List<MiniGameData>();

            string debugString = ConfigAI.FormatTeacherReportHeader("Minigame Selection");

            foreach (var minigame_data in minigame_data_list)
            {
                float cumulativeWeight = 0;
                var minigame_scoredata = minigame_score_list.Find(x => x.MiniGameCode == minigame_data.Code);
                int daysSinceLastScore = 0;
                if (minigame_scoredata != null)
                {
                    var timespanFromLastScoreToNow = GenericHelper.GetTimeSpanBetween(minigame_scoredata.UpdateTimestamp, GenericHelper.GetTimestampForNow());
                    daysSinceLastScore = timespanFromLastScoreToNow.Days;
                }
                debugString += minigame_data.Code + " --- \t";

                // PlaySession Weight [0,1]
                float playSessionWeight = playsession_weights_dict[minigame_data.Code] / 100f; //  [0-100]
                cumulativeWeight += playSessionWeight * ConfigAI.MiniGame_PlaySession_Weight;
                debugString += " PSw: " + playSessionWeight * ConfigAI.MiniGame_PlaySession_Weight + "(" + playSessionWeight + ")";

                // Some minigames are required to appear (weight 100+)
                if (playsession_weights_dict[minigame_data.Code] >= 100)
                {
                    required_minigames.Add(minigame_data);
                    debugString += " REQUIRED!\n";
                    continue;
                }

                // RecentPlay Weight  [1,0]
                const float dayLinerWeightDecrease = 1f / ConfigAI.DaysForMaximumRecentPlayMalus;
                float weightMalus = daysSinceLastScore * dayLinerWeightDecrease;
                float recentPlayWeight = 1f - UnityEngine.Mathf.Min(1, weightMalus);
                cumulativeWeight += recentPlayWeight * ConfigAI.MiniGame_RecentPlay_Weight;
                debugString += " RPw: " + recentPlayWeight * ConfigAI.MiniGame_RecentPlay_Weight + "(" + recentPlayWeight + ")";

                // Save cumulative weight
                weights_list.Add(cumulativeWeight);
                debugString += " TOTw: " + cumulativeWeight + "\n";
            }
            if (ConfigAI.VerboseMinigameSelection)
            {
                ConfigAI.AppendToTeacherReport(debugString);
            }

            // Number checks
            int actualNumberToSelect = UnityEngine.Mathf.Min(numberToSelect, minigame_data_list.Count);

            // Remove the required ones
            actualNumberToSelect -= required_minigames.Count;
            foreach (var requiredMinigame in required_minigames)
            {
                minigame_data_list.Remove(requiredMinigame);
            }

            if (actualNumberToSelect > 0 && minigame_data_list.Count == 0)
            {
                throw new System.Exception("Cannot find even a single minigame for play session " + playSessionData.Id);
            }

            if (actualNumberToSelect > minigame_data_list.Count)
            {
                UnityEngine.Debug.LogWarning("Could not select the requested number of " + numberToSelect + " minigames for play session " + playSessionData.Id + " (only " + minigame_data_list.Count + " are available)");
            }

            // Choose N minigames based on these weights
            var selectedMiniGameData = RandomHelper.RouletteSelectNonRepeating(minigame_data_list, weights_list, actualNumberToSelect);

            // Output
            var finalList = new List<MiniGameData>();
            finalList.AddRange(required_minigames);
            finalList.AddRange(selectedMiniGameData);

            return finalList;
        }

    }
}
