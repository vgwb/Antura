using Antura.Core;
using Antura.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Antura.Minigames
{
    public static class MiniGamesUtilities
    {
        public enum MiniGameSortLogic
        {
            Appearance,
            Alphanumeric
        }

        public static List<MainMiniGame> GetMainMiniGameList(bool skipAssessments = true, MiniGameSortLogic sortLogic = MiniGameSortLogic.Appearance)
        {
            var dictionary = new Dictionary<string, MainMiniGame>();
            var minigameInfoList = AppManager.I.ScoreHelper.GetAllMiniGameInfo();
            foreach (var minigameInfo in minigameInfoList)
            {
                if (minigameInfo.data.CanBeSelected)
                {
                    if (!dictionary.ContainsKey(minigameInfo.data.Main))
                    {
                        dictionary[minigameInfo.data.Main] = new MainMiniGame
                        {
                            MainId = minigameInfo.data.Main,
                            variations = new List<MiniGameInfo>()
                        };
                    }
                    dictionary[minigameInfo.data.Main].variations.Add(minigameInfo);
                }
            }

            var outputMainMiniGamesList = new List<MainMiniGame>();
            foreach (var k in dictionary.Keys)
            {
                if (dictionary[k].MainId == "Assessment" && skipAssessments)
                {
                    continue;
                }
                outputMainMiniGamesList.Add(dictionary[k]);
            }

            // Sort minigames and variations based on their minimum journey position
            var minimumJourneyPositions = new Dictionary<MiniGameCode, JourneyPosition>();
            foreach (var mainMiniGame in outputMainMiniGamesList)
            {
                foreach (var miniGameInfo in mainMiniGame.variations)
                {
                    var miniGameCode = miniGameInfo.data.Code;

                    // Minimum journey position. Set to the max if not found.
                    var minJP = AppManager.I.JourneyHelper.GetMinimumJourneyPositionForMiniGame(miniGameCode);
                    if (minJP == null)
                    {
                        //UnityEngine.Debug.LogWarning("MiniGameCode " + miniGameCode + " has no minimum play session. Forcing to the final one.");
                        minJP = AppManager.I.JourneyHelper.GetFinalJourneyPosition();
                    }

                    minimumJourneyPositions[miniGameCode] = minJP;
                }
            }

            // First sort variations (so the first variation is in front)
            foreach (var mainMiniGame in outputMainMiniGamesList)
            {
                mainMiniGame.variations.Sort((g1, g2) => minimumJourneyPositions[g1.data.Code].IsMinor(
                    minimumJourneyPositions[g2.data.Code])
                    ? -1
                    : 1);
            }


            switch (sortLogic)
            {
                case MiniGameSortLogic.Alphanumeric:
                    outputMainMiniGamesList.Sort((g1, g2) => string.Compare(g1.MainId, g2.MainId, StringComparison.Ordinal));
                    break;

                case MiniGameSortLogic.Appearance:
                    // Then sort minigames by the first variation that appears in Play Sessions
                    outputMainMiniGamesList.Sort((g1, g2) => SortMiniGamesByAppearance(minimumJourneyPositions, g1, g2));
                    break;
            }

            return outputMainMiniGamesList;
        }

        public static int SortMiniGamesByAppearance(Dictionary<MiniGameCode, JourneyPosition> minimumJourneyPositions,
            MainMiniGame g1, MainMiniGame g2)
        {
            // MiniGames are sorted based on minimum play session
            var minPos1 = minimumJourneyPositions[g1.GetFirstVariationMiniGameCode()];
            var minPos2 = minimumJourneyPositions[g2.GetFirstVariationMiniGameCode()];

            if (minPos1.IsMinor(minPos2))
            {
                return -1;
            }
            if (minPos2.IsMinor(minPos1))
            {
                return 1;
            }

            // Check play session order
            var sharedPlaySessionData = AppManager.I.DB.GetPlaySessionDataById(minPos1.Id);
            int ret = 0;
            switch (sharedPlaySessionData.Order)
            {
                case PlaySessionDataOrder.Random:
                    // No specific sorting
                    ret = 0;
                    break;
                case PlaySessionDataOrder.Sequence:
                    // In case of a Sequence PS, two minigames with the same minimum play session are sorted based on the sequence order
                    var miniGameInPlaySession1 =
                        sharedPlaySessionData.Minigames.ToList()
                            .Find(x => x.MiniGameCode == g1.GetFirstVariationMiniGameCode());
                    var miniGameInPlaySession2 =
                        sharedPlaySessionData.Minigames.ToList()
                            .Find(x => x.MiniGameCode == g2.GetFirstVariationMiniGameCode());
                    ret = miniGameInPlaySession1.Weight - miniGameInPlaySession2.Weight;
                    break;
            }
            return ret;
        }

        public static Dictionary<MiniGameCode, float[]> GetMiniGameDifficultiesForTesting()
        {
            var difficultiesForTest = new Dictionary<MiniGameCode, float[]>();
            foreach (MiniGameCode gamecode in Enum.GetValues(typeof(MiniGameCode)))
            {
                difficultiesForTest.Add(gamecode, new[] { 0.0f });
            }
            return difficultiesForTest;
        }
    }
}
