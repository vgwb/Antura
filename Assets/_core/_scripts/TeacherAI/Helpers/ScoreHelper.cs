using System.Collections.Generic;
using System.Linq;
using Antura.Core;
using Antura.Database;
using Antura.Helpers;

namespace Antura.Teacher
{
    /// <summary>
    /// Utilities that help in retrieving and updating score values for learning and progression data.
    /// </summary>
    public class ScoreHelper
    {
        DatabaseManager dbManager;

        public ScoreHelper(DatabaseManager _dbManager)
        {
            dbManager = _dbManager;
        }


        #region Info getters

        public List<MiniGameInfo> GetAllMiniGameInfo()
        {
            return GetAllMiniGameDataInfo<MiniGameData, MiniGameInfo>(DbTables.MiniGames);
        }

        public List<PlaySessionInfo> GetAllPlaySessionInfo()
        {
            return GetAllJourneyDataInfo<PlaySessionData, PlaySessionInfo>(DbTables.PlaySessions, JourneyDataType.PlaySession);
        }

        public List<LearningBlockInfo> GetAllLearningBlockInfo()
        {
            return GetAllJourneyDataInfo<LearningBlockData, LearningBlockInfo>(DbTables.LearningBlocks, JourneyDataType.LearningBlock);
        }

        public List<LetterInfo> GetAllLetterInfo()
        {
            return GetAllVocabularyDataInfo<LetterData, LetterInfo>(DbTables.Letters, VocabularyDataType.Letter);
        }

        public List<WordInfo> GetAllWordInfo()
        {
            return GetAllVocabularyDataInfo<WordData, WordInfo>(DbTables.Words, VocabularyDataType.Word);
        }

        public List<PhraseInfo> GetAllPhraseInfo()
        {
            return GetAllVocabularyDataInfo<PhraseData, PhraseInfo>(DbTables.Phrases, VocabularyDataType.Phrase);
        }

        /*public List<I> GetAllInfo<D,I>(DbTables table) where I : DataInfo<D>, new() where D : IData
        {
            // Retrieve all data
            List<D> data_list = dbManager.GetAllData<D>(table);
            return GetAllInfo<D,I>(data_list, table);
        }*/

        public List<I> GetAllMiniGameDataInfo<D, I>(DbTables table) where I : DataInfo<D>, new()
            where D : MiniGameData
        {
            List<D> data_list = dbManager.GetAllData<D>(table);
            var info_list = new List<I>();

            // Build info instances for the given data
            foreach (var data in data_list)
            {
                var info = new I();
                info.data = data;
                info_list.Add(info);
            }

            // Find available scores
            string query = string.Format("SELECT * FROM " + nameof(MiniGameScoreData));
            var scoredata_list = dbManager.Query<MiniGameScoreData>(query);
            for (int i = 0; i < info_list.Count; i++)
            {
                var info = info_list[i];
                var scoredata = scoredata_list.Find(x => x.MiniGameCode == info.data.Code);
                if (scoredata != null)
                {
                    info.score = scoredata.GetScore();
                    info.unlocked = true;
                }
                else
                {
                    info.score = 0; // 0 until unlocked
                    info.unlocked = false;
                }
            }

            return info_list;
        }

        public List<I> GetAllJourneyDataInfo<D, I>(DbTables table, JourneyDataType dataType) where I : DataInfo<D>, new()
            where D : IData
        {
            List<D> data_list = dbManager.GetAllData<D>(table);
            var info_list = new List<I>();

            // Build info instances for the given data
            foreach (var data in data_list)
            {
                var info = new I();
                info.data = data;
                info_list.Add(info);
            }

            // Find available scores
            string query = string.Format("SELECT * FROM " + nameof(JourneyScoreData) + " WHERE JourneyDataType = '" + (int)dataType +
                                         "' ORDER BY ElementId ");
            var scoredata_list = dbManager.Query<JourneyScoreData>(query);
            for (int i = 0; i < info_list.Count; i++)
            {
                var info = info_list[i];
                var scoredata = scoredata_list.Find(x => x.ElementId == info.data.GetId());
                if (scoredata != null)
                {
                    info.score = scoredata.GetScore();
                    info.unlocked = true;
                }
                else
                {
                    info.score = 0; // 0 until unlocked
                    info.unlocked = false;
                }
            }

            return info_list;
        }

        public List<I> GetAllVocabularyDataInfo<D, I>(DbTables table, VocabularyDataType dataType) where I : DataInfo<D>, new()
            where D : IData
        {
            List<D> data_list = dbManager.GetAllData<D>(table);
            var info_list = new List<I>();

            // Build info instances for the given data
            foreach (var data in data_list)
            {
                var info = new I();
                info.data = data;
                info_list.Add(info);
            }

            // Find available scores
            string query = string.Format("SELECT * FROM " + nameof(VocabularyScoreData) + " WHERE VocabularyDataType = '" +
                                         (int)dataType + "' ORDER BY ElementId ");
            var scoredata_list = dbManager.Query<VocabularyScoreData>(query);
            for (int i = 0; i < info_list.Count; i++)
            {
                var info = info_list[i];
                var scoredata = scoredata_list.Find(x => x.ElementId == info.data.GetId());
                if (scoredata != null)
                {
                    info.score = scoredata.Score;
                    info.unlocked = scoredata.Unlocked;
                }
                else
                {
                    info.score = 0; // 0 until unlocked
                    info.unlocked = false;
                }
            }

            return info_list;
        }

        #endregion

        #region Latest Info getters

        public LetterInfo GetLastLearnedLetterInfo()
        {
            return GetLastLearnedDataInfo<LetterData, LetterInfo>(VocabularyDataType.Letter, AppManager.I.ScoreHelper.GetAllLetterInfo());
        }

        public WordInfo GetLastLearnedWordInfo()
        {
            return GetLastLearnedDataInfo<WordData, WordInfo>(VocabularyDataType.Word, AppManager.I.ScoreHelper.GetAllWordInfo());
        }

        public PhraseInfo GetLastLearnedPhraseInfo()
        {
            return GetLastLearnedDataInfo<PhraseData, PhraseInfo>(VocabularyDataType.Phrase, AppManager.I.ScoreHelper.GetAllPhraseInfo());
        }

        private IT GetLastLearnedDataInfo<T, IT>(VocabularyDataType dataType, List<IT> allInfos)
            where T : IVocabularyData where IT : DataInfo<T>
        {
            string query = "select * from \"" + nameof(LogVocabularyScoreData) + "\"" + " where VocabularyDataType = '" +
                           (int)dataType + "' " + " order by Timestamp limit 1";
            var list = AppManager.I.DB.Query<LogVocabularyScoreData>(query);
            if (list.Count > 0 && list[0] != null)
            {
                return allInfos.Find(x => x.data.GetId() == list[0].ElementId);
            }
            return null;
        }

        #endregion


        #region Score getters

        public List<float> GetLatestScoresForMiniGame(MiniGameCode minigameCode, int nLastDays)
        {
            int fromTimestamp = GenericHelper.GetRelativeTimestampFromNow(-nLastDays);
            string query = string.Format(
                "SELECT * FROM  " + nameof(LogMiniGameScoreData) + " WHERE MiniGameCode = '{0}' AND Timestamp < {1}",
                (int)minigameCode, fromTimestamp);
            var list = dbManager.FindLogPlayDataByQuery(query);
            var scores = list.ConvertAll(x => x.Score);
            return scores;
        }

        public float GetCurrentScoreForJourneyPosition(JourneyPosition jp)
        {
            var allPlaySessionInfo = GetAllPlaySessionInfo();
            var psInfo = allPlaySessionInfo.FirstOrDefault(x => x.data.GetJourneyPosition().Equals(jp));
            if (psInfo == null)
                return 0;
            return psInfo.score;
        }

        public List<JourneyScoreData> GetCurrentScoreForAllPlaySessions()
        {
            string query =
                string.Format("SELECT * FROM " + nameof(JourneyScoreData) + " WHERE JourneyDataType = '{0}'  ORDER BY ElementId",
                   (int)JourneyDataType.PlaySession);
            var list = dbManager.Query<JourneyScoreData>(query);
            return list;
        }

        public List<JourneyScoreData> GetCurrentScoreForPlaySessionsOfStage(int stage)
        {
            // First, get all data given a stage
            var eligiblePlaySessionData_list = this.dbManager.FindPlaySessionData(x => x.Stage == stage);
            var eligiblePlaySessionData_id_list = eligiblePlaySessionData_list.ConvertAll(x => x.Id);

            // Then, get all scores
            string query = string.Format("SELECT * FROM " + nameof(JourneyScoreData) + "  WHERE JourneyDataType = '{0}'", (int)JourneyDataType.PlaySession);
            var all_score_list = dbManager.Query<JourneyScoreData>(query);

            // At last, filter by the given stage
            var filtered_score_list = all_score_list.FindAll(x => eligiblePlaySessionData_id_list.Contains(x.ElementId));
            return filtered_score_list;
        }

        public List<JourneyScoreData> GetCurrentScoreForPlaySessionsOfLearningBlock(int stage, int learningBlock)
        {
            // First, get all data given a stage
            // TODO: make this readily available!
            var eligiblePlaySessionData_list = dbManager.FindPlaySessionData(x => x.Stage == stage && x.LearningBlock == learningBlock);
            var eligiblePlaySessionData_id_list = eligiblePlaySessionData_list.ConvertAll(x => x.Id);

            // Then, get all scores
            string query = string.Format("SELECT * FROM " + nameof(JourneyScoreData) + "  WHERE JourneyDataType = '{0}'",
                JourneyDataType.PlaySession);
            var all_score_list = dbManager.Query<JourneyScoreData>(query);

            // At last, filter
            var filtered_score_list = all_score_list.FindAll(x => eligiblePlaySessionData_id_list.Contains(x.ElementId));
            return filtered_score_list;
        }

        public List<JourneyScoreData> GetCurrentScoreForLearningBlocksOfStage(int stage)
        {
            // First, get all data given a stage
            var eligibleLearningBlockData_list = this.dbManager.FindLearningBlockData(x => x.Stage == stage);
            var eligibleLearningBlockData_id_list = eligibleLearningBlockData_list.ConvertAll(x => x.Id);

            // Then, get all scores
            string query = string.Format("SELECT * FROM " + nameof(JourneyScoreData) + "  WHERE JourneyDataType= '{0}'",
                JourneyDataType.LearningBlock);
            var all_score_list = dbManager.Query<JourneyScoreData>(query);

            // At last, filter by the given stage
            var filtered_score_list = all_score_list.FindAll(x => eligibleLearningBlockData_id_list.Contains(x.ElementId));
            return filtered_score_list;
        }

        #endregion


        #region List Helper

        /// <summary>
        /// TODO take count of the numberOfItems variable
        /// </summary>
        /// <returns>The average score.</returns>
        /// <param name="_scoreList">Score list.</param>
        /// <param name="numberOfItems">Number of items.</param>
        public float GetAverageScore(List<JourneyScoreData> _scoreList, int numberOfItems = -1)
        {
            var average = 0f;

            foreach (var item in _scoreList)
            {
                average += item.GetScore();
            }

            return (average / _scoreList.Count);
        }

        #endregion

        public bool HasFinishedTheGameWithAllStars()
        {
            bool hasFinishedTheGame = AppManager.I.JourneyHelper.HasFinishedTheGame();
            if (!hasFinishedTheGame)
                return false;

            var allMiniGameInfo = GetAllMiniGameInfo();
            foreach (var miniGameInfo in allMiniGameInfo)
            {
                if (System.Math.Abs(miniGameInfo.score - AppConfig.MaxMiniGameScore) > AppConfig.EPSILON)
                {
                    return false;
                }
            }
            return true;
        }

        public bool HasEarnedMaxStarsInCurrentPlaySessions()
        {
            // If we finished the game, just check all stars
            if (AppManager.I.Player.HasFinishedTheGame)
            {
                return AppManager.I.Player.HasFinishedTheGameWithAllStars;
            }

            // If we did not finish the game, we check all play sessions up to the max reached
            // We however ignore the max reached (still to be played!)
            var maxJP = AppManager.I.Player.MaxJourneyPosition;
            var allPlaySessionInfo = GetAllPlaySessionInfo();
            foreach (var playSessionInfo in allPlaySessionInfo)
            {
                if (playSessionInfo.data.GetJourneyPosition().IsMinor(maxJP))
                {
                    if (playSessionInfo.score < AppConfig.MaxMiniGameScore)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

    }
}
