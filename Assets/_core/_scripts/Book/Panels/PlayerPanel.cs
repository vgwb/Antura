using Antura.Core;
using Antura.Database;
using Antura.Helpers;
using Antura.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Antura.Book
{
    public class PlayerPanel : MonoBehaviour
    {
        public UIButton BtnMiniGames;
        public UIButton BtnVocabulary;
        public UIButton BtnGallery;

        public CompletionSlider StarsSlider;
        public CompletionSlider RewardsSlider;
        public CompletionSlider JourneySlider;
        public TextRender BonesText;
        public TextRender PlayTimeText;

        private const int REWARDS_UNLOCK_AT_START = 2;

        void Start()
        {
            BtnMiniGames.Bt.onClick.AddListener(() => Book.I.BtnOpenMinigGamesWithBack());
            BtnVocabulary.Bt.onClick.AddListener(() => Book.I.BtnOpenVocabularyWithBack());
            BtnGallery.Bt.onClick.AddListener(() => BtnOpenPhotoGallery());
            //InfoTable.Reset();

            // Level reached
            //InfoTable.AddRow(LocalizationDataId.UI_Stage_and_Level, AppManager.I.Player.MaxJourneyPosition.GetShortTitle());

            // Unlocked / total PlaySessions
            var totalPlaySessions = AppManager.I.ScoreHelper.GetAllPlaySessionInfo();
            var totalPlaySessionsUnlocked = totalPlaySessions.FindAll(x => x.unlocked);
            JourneySlider.SetValue(totalPlaySessionsUnlocked.Count, totalPlaySessions.Count);

            // Total elapsed time
            //var totalTimespan = GetTotalApplicationTime();
            //InfoTable.AddRow(LocalizationDataId.UI_Journey_duration,totalTimespan.Days + "d " + totalTimespan.Hours + "h " + totalTimespan.Minutes + "m");

            // Played Games
            //InfoTable.AddRow(LocalizationDataId.UI_Games_played, GetTotalMiniGamePlayInstances().ToString());

            // Total stars
            var playerStars = GetObtainedPlaySessionStars();
            StarsSlider.SetValue(playerStars, GetTotalPlaySessionStars());
            //InfoTable.AddRow(LocalizationDataId.UI_Stars, totalStars.ToString());

            // unlocked / total REWARDS
            var totalRewards = AppManager.I.RewardSystemManager.GetTotalRewardPacksCount(true);
            var totalRewardsUnlocked = AppManager.I.RewardSystemManager.GetUnlockedRewardPacksCount(true);
            RewardsSlider.SetValue(totalRewardsUnlocked - REWARDS_UNLOCK_AT_START, totalRewards - REWARDS_UNLOCK_AT_START);
            //InfoTable.AddRow("Antura Rewards", "", totalRewardsUnlocked.ToString() + " / " + totalRewards);
            //InfoTable.AddSliderRow(LocalizationDataId.UI_Antura_Rewards, totalRewards, totalRewardsUnlocked);


            // Total bones
            var totalBones = AppManager.I.Player.GetTotalNumberOfBones();
            BonesText.SetText(totalBones.ToString());

            // total play time
            var totalPlayTime = GetTotalMiniGamePlayTime();
            PlayTimeText.SetText(totalPlayTime.Days + "d " + totalPlayTime.Hours + "h " + totalPlayTime.Minutes + "m");
            //InfoTable.AddRow(LocalizationDataId.UI_Playing_time,                totalPlayTime.Days + "d " + totalPlayTime.Hours + "h " + totalPlayTime.Minutes + "m");


            //// unlocked / total Letters
            //var totalLetters = GetTotalVocabularyData(VocabularyDataType.Letter);
            //var totalLettersUnlocked = GetTotalVocabularyDataUnlocked(VocabularyDataType.Letter);
            ////InfoTable.AddRow("Unlocked Letters", "", totalLettersUnlocked.ToString() + " / " + totalLetters);
            //InfoTable.AddSliderRow(LocalizationDataId.UI_Unlocked_Letters, totalLettersUnlocked, totalLetters);

            //// unlocked / total Words
            //var totalWords = GetTotalVocabularyData(VocabularyDataType.Word);
            //var totalWordsUnlocked = GetTotalVocabularyDataUnlocked(VocabularyDataType.Word);
            ////InfoTable.AddRow("Unlocked Words", "", totalWordsUnlocked.ToString() + " / " + totalWords);
            //InfoTable.AddSliderRow(LocalizationDataId.UI_Unlocked_Words, totalWordsUnlocked, totalWords);

            //// unlocked / total Phrases
            //var totalPhrases = GetTotalVocabularyData(VocabularyDataType.Phrase);
            //var totalPhrasesUnlocked = GetTotalVocabularyDataUnlocked(VocabularyDataType.Phrase);
            ////InfoTable.AddRow("Unlocked Phrases", "", totalPhrasesUnlocked.ToString() + " / " + totalPhrases);
            //InfoTable.AddSliderRow(LocalizationDataId.UI_Unlocked_Phrases, totalPhrasesUnlocked, totalPhrases);

            // player UUID
            //InfoTable.AddRow("Player Code", "", AppManager.I.Player.GetShortUuid());

            ////////////////////////////////////
            ///
            // last lettert learnd
            //var lastLetterLearned = AppManager.I.ScoreHelper.GetLastLearnedLetterInfo();
            //InfoTable.AddRow("Last Letter", "", (lastLetterLearned != null ? lastLetterLearned.data.ToString() : ""));

            //if (AppManager.I.Player.Precision != 0f) { str += "Precision " + AppManager.I.Player.Precision + "\n"; }
            //if (AppManager.I.Player.Reaction != 0f) { str += "Reaction " + AppManager.I.Player.Reaction + "\n"; }
            //if (AppManager.I.Player.Memory != 0f) { str += "Memory " + AppManager.I.Player.Memory + "\n"; }
            //if (AppManager.I.Player.Logic != 0f) { str += "Logic " + AppManager.I.Player.Logic + "\n"; }
            //if (AppManager.I.Player.Rhythm != 0f) { str += "Rhythm " + AppManager.I.Player.Rhythm + "\n"; }
            //if (AppManager.I.Player.Musicality != 0f) { str += "Musicality " + AppManager.I.Player.Musicality + "\n"; }
            //if (AppManager.I.Player.Sight != 0f) { str += "Sight " + AppManager.I.Player.Sight + "\n"; }


            //Debug.Log("LAST LETTER: " + AppManager.I.ScoreHelper.GetLastLearnedLetterInfo().data);
            //Debug.Log("Total play times: " + GetMiniGamesTotalPlayTime().ToDebugString());
            //Debug.Log("Number of plays: " + GetMiniGamesNumberOfPlays().ToDebugString());

            // GRAPH
            //journeyGraph.Show(allPlaySessionInfos, unlockedPlaySessionInfos);
        }

        public void BtnOpenPhotoGallery()
        {
            GlobalUI.ShowPrompt(LocalizationDataId.AnturaSpace_Photo_Gallery);
        }

        #region Player Stats Queries

        private TimeSpan GetTotalApplicationTime()
        {
            string query = "select * from \"" + nameof(LogInfoData) + "\"";
            var list = AppManager.I.DB.Query<LogInfoData>(query);

            System.TimeSpan totalTimespan = new System.TimeSpan(0);
            bool foundStart = false;
            int startTimestamp = 0;
            foreach (var infoData in list)
            {
                if (!foundStart && infoData.Event == InfoEvent.AppSessionStart)
                {
                    startTimestamp = infoData.Timestamp;
                    //Debug.Log("START: " + infoData.Timestamp);
                    foundStart = true;
                }
                else if (foundStart && infoData.Event == InfoEvent.AppSessionEnd)
                {
                    var endTimestamp = infoData.Timestamp;
                    foundStart = false;
                    //Debug.Log("END: " + infoData.Timestamp);

                    var deltaTimespan = GenericHelper.GetTimeSpanBetween(startTimestamp, endTimestamp);
                    totalTimespan += deltaTimespan;
                    //Debug.Log("TIME FOUND:"  + deltaTimespan.Days + " days " + deltaTimespan.Hours + " hours " + deltaTimespan.Minutes + " minutes " + deltaTimespan.Seconds + " seconds");
                }
            }

            // Time up to now
            if (foundStart)
            {
                var deltaTimespan = GenericHelper.GetTimeSpanBetween(startTimestamp, GenericHelper.GetTimestampForNow());
                totalTimespan += deltaTimespan;
                //Debug.Log("TIME UP TO NOW:" + deltaTimespan.Days + " days " + deltaTimespan.Hours + " hours " + deltaTimespan.Minutes + " minutes " + deltaTimespan.Seconds + " seconds");
            }
            return totalTimespan;
        }

        private TimeSpan GetTotalMiniGamePlayTime()
        {
            float totalSeconds = 0f;
            string query = "select * from " + nameof(MiniGameScoreData);
            var list = AppManager.I.DB.Query<MiniGameScoreData>(query);

            foreach (var data in list)
            {
                totalSeconds += data.TotalPlayTime;
            }
            TimeSpan t = TimeSpan.FromSeconds(totalSeconds);
            return t;
        }

        private Dictionary<MiniGameCode, float> GetMiniGamesTotalPlayTime()
        {
            Dictionary<MiniGameCode, float> dict = new Dictionary<MiniGameCode, float>();
            string query = "select * from " + nameof(MiniGameScoreData);
            var list = AppManager.I.DB.Query<MiniGameScoreData>(query);

            foreach (var data in list)
            {
                dict[data.MiniGameCode] = data.TotalPlayTime;
            }
            return dict;
        }

        private int GetTotalMiniGamePlayInstances()
        {
            int total = 0;
            string query = "select * from " + nameof(LogMiniGameScoreData);
            var list = AppManager.I.DB.Query<LogMiniGameScoreData>(query);

            foreach (var data in list)
            {
                total++;
            }
            return total;
        }

        private int GetObtainedPlaySessionStars()
        {
            string query = "select * from " + nameof(JourneyScoreData);
            var list = AppManager.I.DB.Query<JourneyScoreData>(query);
            var totalStars = list.Sum(data => data.IsMiniGamePS ? data.Stars : 0);
            return totalStars;
        }

        private int GetTotalPlaySessionStars()
        {
            int nMinigamePS = AppManager.I.DB.GetAllPlaySessionData().Count(data => data.IsMiniGamePS);
            return nMinigamePS * AppConfig.MaxMiniGameScore;
        }

        private int GetTotalVocabularyData(VocabularyDataType dataType)
        {
            int count = 0;
            switch (dataType)
            {
                case VocabularyDataType.Letter:
                    count = AppManager.I.DB.GetAllLetterData().Count;
                    break;
                case VocabularyDataType.Word:
                    count = AppManager.I.DB.GetAllWordData().Count;
                    break;
                case VocabularyDataType.Phrase:
                    count = AppManager.I.DB.GetAllPhraseData().Count;
                    break;
            }
            return count;
        }

        private int GetTotalVocabularyDataUnlocked(VocabularyDataType dataType)
        {
            if (AppManager.I.Player.IsDemoUser)
            { return GetTotalVocabularyData(dataType); }
            string query = "select * from " + nameof(VocabularyScoreData) + " where VocabularyDataType='" + (int)dataType + "'";
            var list = AppManager.I.DB.Query<VocabularyScoreData>(query);
            return list.Count(data => data.Unlocked);
        }

        private Dictionary<MiniGameCode, int> GetNumberOfPlaysByMiniGame()
        {
            Dictionary<MiniGameCode, int> dict = new Dictionary<MiniGameCode, int>();
            string query = "select * from " + nameof(LogMiniGameScoreData);
            var list = AppManager.I.DB.Query<LogMiniGameScoreData>(query);

            foreach (var data in list)
            {
                if (!dict.ContainsKey(data.MiniGameCode))
                {
                    dict[data.MiniGameCode] = 0;
                }
                dict[data.MiniGameCode]++;
            }
            return dict;
        }

        #endregion
    }
}
