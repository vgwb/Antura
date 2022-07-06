using Antura.Core;
using Antura.Database;
using Antura.Helpers;
using Antura.LivingLetters;
using Antura.Profile;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Antura.Teacher
{
    /// <summary>
    /// Handles logic that represent the Teacher's expert system:
    /// - selects minigames according to a given progression flow
    /// - selects question packs according to the given profression flow
    /// - selects minigame difficulty according to the player's status
    /// </summary>
    public class TeacherAI
    {
        public static TeacherAI I;

        // References
        private DatabaseManager dbManager;
        private PlayerProfile playerProfile;

        // Inner engines
        public LogAI logAI;
        public VocabularySelectionAI VocabularyAi;
        private MiniGameSelectionAI minigameSelectionAI;
        private DifficultySelectionAI difficultySelectionAI;

        // Helpers
        // TODO refactor: these helpers should be separated from the TeacherAI.
        private VocabularyHelper vocabularyHelper;
        private ScoreHelper scoreHelper;

        #region Setup

        public TeacherAI(DatabaseManager _dbManager, VocabularyHelper _vocabularyHelper, ScoreHelper _scoreHelper)
        {
            I = this;
            dbManager = _dbManager;

            vocabularyHelper = _vocabularyHelper;
            scoreHelper = _scoreHelper;

            logAI = new LogAI(_dbManager);
            minigameSelectionAI = new MiniGameSelectionAI(dbManager);
            VocabularyAi = new VocabularySelectionAI(dbManager);
            difficultySelectionAI = new DifficultySelectionAI(dbManager);

            buildMinimumMiniGameJourneyPositions();
        }

        public void SetPlayerProfile(PlayerProfile _playerProfile)
        {
            playerProfile = _playerProfile;
            difficultySelectionAI.SetPlayerProfile(_playerProfile);
        }

        private void ResetPlaySession()
        {
            var currentPlaySessionId = playerProfile.CurrentJourneyPosition.Id;
            minigameSelectionAI.InitNewPlaySession();
            VocabularyAi.LoadCurrentPlaySessionData(currentPlaySessionId);
        }

        #endregion

        #region MiniGames

        public void InitNewPlaySession()
        {
            ResetPlaySession();
        }

        /// <summary>
        /// Selects the mini games forthe current PlaySession
        /// </summary>
        /// <returns>The mini games List</returns>
        public List<MiniGameData> SelectMiniGames()
        {
            // Check the number of minigames for the current play session
            var currentPlaySessionId = playerProfile.CurrentJourneyPosition.Id;
            var playSessionData = dbManager.GetPlaySessionDataById(currentPlaySessionId);
            int nMinigamesToSelect = playSessionData.NumberOfMinigames;
            if (nMinigamesToSelect == 0)
            {
                // Force at least one minigame (needed for assessment, since we always need one)
                nMinigamesToSelect = 1;
            }

            return selectMiniGames(nMinigamesToSelect);
        }

        public List<MiniGameData> SelectMiniGamesForPlaySession(string playSessionId, int numberToSelect)
        {
            return minigameSelectionAI.PerformSelection(playSessionId, numberToSelect);
        }

        private List<MiniGameData> selectMiniGames(int nMinigamesToSelect)
        {
            List<MiniGameData> newPlaySessionMiniGames = selectMiniGamesForCurrentPlaySession(nMinigamesToSelect);

            if (ConfigAI.VerboseTeacher)
            {
                var debugString = "";
                debugString += ConfigAI.FormatTeacherReportHeader("Minigames Selected");
                foreach (var minigame in newPlaySessionMiniGames)
                {
                    debugString += "\n" + minigame.Code;
                }
                Debug.Log(debugString);
            }

            return newPlaySessionMiniGames;
        }

        private List<MiniGameData> selectMiniGamesForCurrentPlaySession(int nMinigamesToSelect)
        {
            var currentPlaySessionId = playerProfile.CurrentJourneyPosition.Id;
            return SelectMiniGamesForPlaySession(currentPlaySessionId, nMinigamesToSelect);
        }

        #region MiniGame Validity

        private Dictionary<MiniGameCode, JourneyPosition> minMiniGameJourneyPositions = new Dictionary<MiniGameCode, JourneyPosition>();

        private void buildMinimumMiniGameJourneyPositions()
        {
            var allPsData = dbManager.GetAllPlaySessionData();
            foreach (var mgcode in GenericHelper.SortEnums<MiniGameCode>())
            {
                minMiniGameJourneyPositions[mgcode] = null;
                foreach (var psData in allPsData)
                {
                    if (CanMiniGameBePlayedAtPlaySession(psData, mgcode))
                    {
                        minMiniGameJourneyPositions[mgcode] = psData.GetJourneyPosition();
                        //Debug.Log(mgcode + " min at " + psData.GetJourneyPosition());
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Can the given minigame be played at the given play session?
        /// Strong check: it must be the actual play session
        /// </summary>
        public bool CanMiniGameBePlayedAtPlaySession(JourneyPosition journeyPos, MiniGameCode code)
        {
            var psData = dbManager.GetPlaySessionDataById(journeyPos.Id);
            return CanMiniGameBePlayedAtPlaySession(psData, code);
        }

        public bool CanMiniGameBePlayedAtPlaySession(PlaySessionData psData, MiniGameCode code)
        {
            if (psData != null)
            {
                var mgIndex = psData.Minigames.ToList().FindIndex(x => x.MiniGameCode == code);
                if (mgIndex >= 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Can minigame be played at the given play session?
        /// Weak check: it just requires that the minimum has been reached.
        /// </summary>
        public bool CanMiniGameBePlayedAfterMinPlaySession(JourneyPosition jp, MiniGameCode code)
        {
            if (minMiniGameJourneyPositions[code] == null)
            {
                return false;
            }
            return minMiniGameJourneyPositions[code].IsMinor(jp)
                 || minMiniGameJourneyPositions[code].Equals(jp);
        }

        /// <summary>
        /// Can minigame be played at ANY play session at all?
        /// </summary>
        public bool CanMiniGameBePlayedAtAnyPlaySession(MiniGameCode code)
        {
            return minMiniGameJourneyPositions[code] != null;
        }

        #endregion

        #endregion

        #region Difficulty & Minigame Configuration

        public float GetCurrentDifficulty(MiniGameCode miniGameCode)
        {
            return difficultySelectionAI.SelectDifficulty(miniGameCode);
        }

        public int GetCurrentNumberOfRounds(MiniGameCode miniGameCode)
        {
            var currentPos = AppManager.I.Player.CurrentJourneyPosition;
            var psData = dbManager.GetPlaySessionDataById(currentPos.Id);
            return psData.NumberOfRoundsPerMinigame;
        }

        public bool GetTutorialEnabled(MiniGameCode miniGameCode)
        {
            // TODO: define the logic for when the tutorial should not be shown anymore!
            return AppConfig.MinigameTutorialsEnabled;
        }

        #endregion

        #region Learning Blocks

        // TODO refactor: Not used.
        public float GetLearningBlockScore(LearningBlockData lb)
        {
            var allScores = scoreHelper.GetCurrentScoreForPlaySessionsOfLearningBlock(lb.Stage, lb.LearningBlock);
            return scoreHelper.GetAverageScore(allScores);
        }

        #endregion

        #region Assessment

        // TODO refactor: Only for tests purpose. Remove from the teacher.
        public List<LetterData> GetFailedAssessmentLetters(MiniGameCode assessmentCode) // also play session
        {
            // @note: this code shows how to work on the dynamic and static db together
            string query =
                string.Format(
                    "SELECT * FROM " + nameof(LogVocabularyScoreData) + " WHERE TableName = 'LetterData' AND Score < 0 and MiniGame = {0}",
                    (int)assessmentCode);
            List<LogVocabularyScoreData> logLearnData_list = dbManager.FindLogVocabularyScoreDataByQuery(query);
            List<string> letter_ids_list = logLearnData_list.ConvertAll(x => x.ElementId);
            List<LetterData> letters = dbManager.FindLetterData(x => letter_ids_list.Contains(x.Id));
            return letters;
        }

        // TODO refactor: Only for tests purpose. Remove from the teacher.
        public List<WordData> GetFailedAssessmentWords(MiniGameCode assessmentCode)
        {
            string query =
                string.Format(
                    "SELECT * FROM " + nameof(LogVocabularyScoreData) + " WHERE TableName = 'WordData' AND Score < 0 and MiniGame = {0}",
                    (int)assessmentCode);
            List<LogVocabularyScoreData> logLearnData_list = dbManager.FindLogVocabularyScoreDataByQuery(query);
            List<string> words_ids_list = logLearnData_list.ConvertAll(x => x.ElementId);
            List<WordData> words = dbManager.FindWordData(x => words_ids_list.Contains(x.Id));
            return words;
        }

        #endregion

        #region Journeymap

        // TODO refactor: Only for tests purpose. Remove from the teacher.
        public List<LogPlayData> GetScoreHistoryForCurrentJourneyPosition()
        {
            // @note: shows how to work with playerprofile as well as the database
            JourneyPosition currentJourneyPosition = playerProfile.CurrentJourneyPosition;
            string query = string.Format("SELECT * FROM " + nameof(LogPlayData) + " WHERE PlayEvent = {0} AND PlaySession = '{1}'",
                (int)PlayEvent.Skill, currentJourneyPosition.ToString());
            List<LogPlayData> list = dbManager.FindLogPlayDataByQuery(query);
            return list;
        }

        #endregion

        #region Mood

        // TODO refactor: Refactor access to the data through an AnalyticsManager, instead of passing through the TeacherAI.
        public List<LogMoodData> GetLastMoodData(int number)
        {
            string query = string.Format("SELECT * FROM " + nameof(LogMoodData) + " ORDER BY Timestamp LIMIT {0}", number);
            var list = dbManager.FindLogMoodDataByQuery(query);
            return list;
        }

        #endregion


        #region Fake data for question providers

        // TODO refactor: Refactor access to test data throught a TestDataManager, instead of passing through the TeacherAI.
        private static bool giveWarningOnFake = false;

        public List<LL_LetterData> GetAllTestLetterDataLL(LetterFilters filters = null, bool useMaxJourneyData = false)
        {
            if (filters == null)
            { filters = new LetterFilters(); }

            if (useMaxJourneyData)
            {
                VocabularyAi.LoadCurrentPlaySessionData(AppManager.I.Player.MaxJourneyPosition.ToString());
            }

            var availableLetters = VocabularyAi.SelectData(
              () => vocabularyHelper.GetAllLetters(filters),
                new SelectionParameters(SelectionSeverity.AsManyAsPossible, getMaxData: true, useJourney: useMaxJourneyData)
                , true
            );

            var output_list = new List<LL_LetterData>();
            foreach (var letterData in availableLetters)
            {
                output_list.Add(BuildLetterData_LL(letterData));
            }
            /*if (ConfigAI.verboseTeacher)
            {
                Debug.Log("All test letter data requested to teacher.");
            }*/

            return output_list;
        }

        public LL_LetterData GetRandomTestLetterLL(LetterFilters filters = null, bool useMaxJourneyData = false)
        {
            if (filters == null)
            { filters = new LetterFilters(); }

            List<LetterData> availableLetters = null;

            if (AppManager.I.Player == null)
            {
                availableLetters = vocabularyHelper.GetAllLetters(filters);
            }
            else
            {
                if (useMaxJourneyData)
                {
                    VocabularyAi.LoadCurrentPlaySessionData(AppManager.I.Player.MaxJourneyPosition.ToString());
                }

                availableLetters = VocabularyAi.SelectData(
                  () => vocabularyHelper.GetAllLetters(filters),
                    new SelectionParameters(SelectionSeverity.AsManyAsPossible, getMaxData: true, useJourney: useMaxJourneyData)
                  , true
                );
            }

            if (giveWarningOnFake)
            {
                Debug.LogWarning("You are using fake data for testing. Make sure to test with real data too.");
                giveWarningOnFake = false;
            }

            var data = availableLetters.RandomSelectOne();

            /*if (ConfigAI.verboseTeacher)
            {
                Debug.Log("Random test Letter requested to teacher: " + data.ToString());
            }*/

            return BuildLetterData_LL(data);
        }

        public LL_WordData GetRandomTestWordDataLL(WordFilters filters = null, bool useMaxJourneyData = false)
        {
            if (filters == null)
            { filters = new WordFilters(); }

            if (useMaxJourneyData)
            {
                VocabularyAi.LoadCurrentPlaySessionData(AppManager.I.Player.MaxJourneyPosition.ToString());
            }

            if (giveWarningOnFake)
            {
                Debug.LogWarning("You are using fake data for testing. Make sure to test with real data too.");
                giveWarningOnFake = false;
            }

            var availableWords = VocabularyAi.SelectData(
              () => vocabularyHelper.GetAllWords(filters),
                new SelectionParameters(SelectionSeverity.AsManyAsPossible, getMaxData: true, useJourney: useMaxJourneyData)
               , true
              );

            var data = availableWords.RandomSelectOne();

            /*if (ConfigAI.verboseTeacher)
            {
                Debug.Log("Random test Word requested to teacher: " + data.ToString());
            }*/

            return BuildWordData_LL(data);
        }

        private LL_LetterData BuildLetterData_LL(LetterData data)
        {
            return new LL_LetterData(data);
        }

        private List<ILivingLetterData> BuildLetterData_LL_Set(List<LetterData> data_list)
        {
            return data_list.ConvertAll<ILivingLetterData>(x => BuildLetterData_LL(x));
        }

        private LL_WordData BuildWordData_LL(WordData data)
        {
            return new LL_WordData(data.GetId(), data);
        }

        private List<ILivingLetterData> BuildWordData_LL_Set(List<WordData> data_list)
        {
            return data_list.ConvertAll<ILivingLetterData>(x => BuildWordData_LL(x));
        }

        #endregion
    }
}
