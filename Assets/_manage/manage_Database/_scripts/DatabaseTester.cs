#if UNITY_EDITOR

using Antura.Core;
using Antura.Helpers;
using Antura.Profile;
using Antura.Rewards;
using Antura.Language;
using Antura.Teacher;
using Antura.UI;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

// refactor: standardize random use across the codebase
using RND = UnityEngine.Random;

namespace Antura.Database.Management
{
    /// <summary>
    /// Helps in managing and testing database contents.
    /// </summary>
    [RequireComponent(typeof(EditorContentHolder))]
    public class DatabaseTester : MonoBehaviour
    {
        private ContentEditionConfig inputContent;

        [Header("Config")]
        public Text OutputText;
        public TextRender OutputTextArabic;

        private DatabaseLoader dbLoader;
        [HideInInspector]
        public DatabaseManager dbManager;
        private TeacherAI teacherAI;
        private PlayerProfile playerProfile;
        private VocabularyHelper vocabularyHelper;
        private ScoreHelper scoreHelper;
        public static string DEBUG_PLAYER_UUID = "TEST";

        [Header("Test")]
        public string TestCustomProfile;

        void Awake()
        {
            inputContent = FindObjectOfType<EditorContentHolder>().InputContent;
            var langCode = inputContent.LearningLanguage;

            dbLoader = GetComponent<DatabaseLoader>();
            dbLoader.langCode = langCode;
            dbLoader.InputContent = inputContent;

            dbManager = new DatabaseManager(inputContent, langCode);
            vocabularyHelper = new VocabularyHelper(dbManager);
            scoreHelper = new ScoreHelper(dbManager);
            teacherAI = new TeacherAI(dbManager, vocabularyHelper, scoreHelper);

            if (TestCustomProfile != string.Empty)
            {
                LoadCustomProfile(TestCustomProfile);
            }
            else
            {
                LoadProfile(DEBUG_PLAYER_UUID);
            }
        }

        private void Start()
        {
            GlobalUI.I.gameObject.SetActive(false);
            ToDatabaseManager();
        }

        private void LoadCustomProfile(string profileID)
        {
            dbManager.LoadDatabaseForPlayer(profileID);
            PlayerProfileData playerProfile = dbManager.GetPlayerProfileData();
            Debug.LogWarning(playerProfile.ToString());
            Debug.LogWarning(playerProfile.AppVersion);
            Debug.LogWarning(playerProfile.EditionID);
            Debug.LogWarning(playerProfile.ContentID);
            Debug.LogWarning(playerProfile.Tint);
            Debug.LogWarning(playerProfile.Gender);
            Debug.LogWarning(dbManager.GetAllRewardPackUnlockData().ToJoinedString());

            // Fake wrong JP
            /*var pp = AppManager.I.PlayerProfileManager.SetPlayerAsCurrentByUUID(profileID);
            pp.SetMaxJourneyPosition(new JourneyPosition(1,2,2), _forced:true);
            pp.SetCurrentJourneyPosition(new JourneyPosition(1,2,2));
            */
        }

        #region Main Actions

        public ContentEditionConfig GetInputContent()
        {
            return inputContent;
        }

        public void RecreateDatabase()
        {
            dbLoader.RecreateDatabase();
        }

        public void CopyCurrentDatabaseForTesting()
        {
            dbLoader.CopyCurrentDatabaseForTesting();
        }

        #endregion

        #region Specific Logs

        public void DumpAllDataCounts()
        {
            var output = "";
            output += ("N letters: " + dbManager.GetAllLetterData().Count) + "\n";
            output += ("N words: " + dbManager.GetAllWordData().Count) + "\n";
            output += ("N phrases: " + dbManager.GetAllPhraseData().Count) + "\n";
            output += ("N minigames: " + dbManager.GetAllMiniGameData().Count) + "\n";
            output += ("N stages: " + dbManager.GetAllStageData().Count) + "\n";
            output += ("N learningblocks: " + dbManager.GetAllLearningBlockData().Count) + "\n";
            output += ("N playsessions: " + dbManager.GetAllPlaySessionData().Count) + "\n";
            output += ("N localizations: " + dbManager.GetAllLocalizationData().Count) + "\n";
            output += ("N rewards: " + dbManager.GetAllRewardData().Count) + "\n";
            PrintOutput(output);
        }

        public void DumpAllLetterData()
        {
            DumpAllData(dbManager.GetAllLetterData());
        }

        public void DumpAllWordData()
        {
            DumpAllData(dbManager.GetAllWordData());
        }

        public void DumpAllPhraseData()
        {
            DumpAllData(dbManager.GetAllPhraseData());
        }

        public void DumpAllPlaySessionData()
        {
            DumpAllData(dbManager.GetAllPlaySessionData());
        }

        public void DumpAllLearningBlockData()
        {
            DumpAllData(dbManager.GetAllLearningBlockData());
        }

        public void DumpAllStageData()
        {
            DumpAllData(dbManager.GetAllStageData());
        }

        public void DumpAllLocalizationData()
        {
            DumpAllData(dbManager.GetAllLocalizationData());
        }

        public void DumpAllMiniGameData()
        {
            DumpAllData(dbManager.GetAllMiniGameData());
        }

        public void DumpAllLogInfoData()
        {
            DumpAllData(dbManager.GetAllLogInfoData());
        }

        public void DumpAllLogLearnData()
        {
            DumpAllData(dbManager.GetAllVocabularyScoreData());
        }

        public void DumpAllLogMoodData()
        {
            DumpAllData(dbManager.GetAllLogMoodData());
        }

        public void DumpAllLogPlayData()
        {
            DumpAllData(dbManager.GetAllLogPlayData());
        }

        public void DumpAllVocabularyScoreData()
        {
            DumpAllData(dbManager.GetAllDynamicData<VocabularyScoreData>());
        }

        public void DumpAllJourneyScoreData()
        {
            DumpAllData(dbManager.GetAllDynamicData<JourneyScoreData>());
        }

        public void DumpAllMinigameScoreData()
        {
            DumpAllData(dbManager.GetAllDynamicData<MiniGameScoreData>());
        }

        public void DumpLetterById(string id)
        {
            IData data = dbManager.GetLetterDataById(id);
            DumpDataById(id, data);
        }

        public void DumpWordById(string id)
        {
            var data = dbManager.GetWordDataById(id);
            var arabic_text = data.Text;
            PrintArabicOutput(arabic_text);
            DumpDataById(id, data);
        }

        public void DumpPhraseById(string id)
        {
            IData data = dbManager.GetPhraseDataById(id);
            DumpDataById(id, data);
        }

        public void DumpMiniGameByCode(MiniGameCode code)
        {
            IData data = dbManager.GetMiniGameDataByCode(code);
            DumpDataById(data.GetId(), data);
        }

        public void DumpStageById(string id)
        {
            IData data = dbManager.GetStageDataById(id);
            DumpDataById(id, data);
        }

        public void DumpPlaySessionById(string id)
        {
            IData data = dbManager.GetPlaySessionDataById(id);
            DumpDataById(id, data);
        }

        public void DumpLocalizationById(string id)
        {
            IData data = dbManager.GetLocalizationDataById(id);
            DumpDataById(id, data);
        }

        public void DumpRewardById(string id)
        {
            IData data = dbManager.GetRewardDataById(id);
            DumpDataById(id, data);
        }

        public void DumpLogDataById(string id)
        {
            IData data = dbManager.GetLogInfoDataById(id);
            DumpDataById(id, data);
        }


        public void DumpArabicWord(string id)
        {
            var data = dbManager.GetWordDataById(id);
            var arabic_text = data.Text;
            PrintArabicOutput(arabic_text);
        }

        public void DumpActiveMinigames()
        {
            var all_minigames = dbManager.GetAllMiniGameData();
            var active_minigames = dbManager.GetActiveMinigames();
            PrintOutput(active_minigames.Count + " active minigames out of " + all_minigames.Count);
        }

        #endregion

        #region Letters Words

        public void TestLettersData()
        {
            foreach (var l in dbManager.StaticDatabase.GetLetterTable().GetValuesTyped())
            {
                if (l.Initial_Unicode == l.Isolated_Unicode)
                    Debug.LogError("Letter " + l + " has same initial and isolated unicodes");

                if (l.Medial_Unicode == l.Isolated_Unicode)
                    Debug.LogError("Letter " + l + " has same medial and isolated unicodes");

                if (l.Final_Unicode == l.Isolated_Unicode)
                    Debug.LogError("Letter " + l + " has same final and isolated unicodes");
            }

            foreach (var w in dbManager.StaticDatabase.GetWordTable().GetValuesTyped())
            {
                LanguageSwitcher.I.GetHelper(LanguageUse.Learning).SplitWord(dbManager, w);
            }


            foreach (var w in dbManager.StaticDatabase.GetPhraseTable().GetValuesTyped())
            {
                LanguageSwitcher.I.GetHelper(LanguageUse.Learning).SplitPhrase(dbManager, w);
            }

            /*
            List<Database.PhraseData> phrases = new List<Database.PhraseData>(AppManager.I.DB.StaticDatabase.GetPhraseTable().GetValuesTyped());

            int idx;
            foreach (var word in phrases)
            {
                if ((idx = word.Arabic.IndexOf((char)int.Parse("0623", System.Globalization.NumberStyles.HexNumber))) >= 0 &&
                    (idx = word.Arabic.IndexOf((char)int.Parse("0644", System.Globalization.NumberStyles.HexNumber))) >= 0)
                    Debug.Log("FOUND! " + word);
            }

            List<Database.WordData> words = new List<Database.WordData>(AppManager.I.DB.StaticDatabase.GetWordTable().GetValuesTyped());

            foreach (var word in words)
            {
                if ((idx = word.Arabic.IndexOf((char)int.Parse("0623", System.Globalization.NumberStyles.HexNumber))) >= 0 &&
                    (idx = word.Arabic.IndexOf((char)int.Parse("0644", System.Globalization.NumberStyles.HexNumber))) >= 0)
                    Debug.Log("FOUND! " + word);
            }

            //LL_WordData newWordData = new LL_WordData(AppManager.I.DB.GetWordDataById("wolf"));
            */

        }

        public void TestVocabularyHelper()
        {
            var allWords = vocabularyHelper.GetAllWords(new WordFilters());
            var testWord = allWords[2];
            Debug.Log("N words: " + allWords.Count);
            Debug.Log("TEST Word: " + testWord);
            var lettersInWord = vocabularyHelper.GetLettersInWord(testWord);
            Debug.Log("Letters in that word: " + lettersInWord.ToDebugString());

            var testLetter = lettersInWord[0];
            var wordsWithLetters = vocabularyHelper.GetWordsWithLetter(new WordFilters(), testLetter, LetterEqualityStrictness.Letter);
            Debug.Log("Words with unstrict letter " + testLetter + ": \n" + wordsWithLetters.ToDebugStringNewline());

            wordsWithLetters = vocabularyHelper.GetWordsWithLetter(new WordFilters(), testLetter, LetterEqualityStrictness.WithActualForm);
            Debug.Log("Words with strict letter " + testLetter + ": \n" + wordsWithLetters.ToDebugStringNewline());

        }

        #endregion

        #region Test Insert Log Data

        public void PopulateDatabaseRandomly()
        {
            for (int i = 0; i < RND.Range(10, 20); i++)
                TestInsertLogInfoData();
            for (int i = 0; i < RND.Range(10, 20); i++)
                TestInsertLogLearnData();
            for (int i = 0; i < RND.Range(10, 20); i++)
                TestInsertLogMoodData();
            for (int i = 0; i < RND.Range(10, 20); i++)
                TestInsertLogPlayData();

            for (int i = 0; i < RND.Range(20, 30); i++)
                TestInsertVocabularyScoreData();
            for (int i = 0; i < RND.Range(20, 30); i++)
                TestInsertJourneyScoreData();
            for (int i = 0; i < RND.Range(20, 30); i++)
                TestInsertMinigameScoreData();

        }

        public void TestInsertLogInfoData()
        {
            var newData = new LogInfoData();
            newData.AppSession = GenericHelper.GetTimestampForNow();
            newData.Timestamp = GenericHelper.GetTimestampForNow();

            newData.Event = InfoEvent.Book;
            newData.AdditionalData = "test:1";

            this.dbManager.Insert(newData);
            PrintOutput("Inserted new LogInfoData: " + newData.ToString());
        }

        public void TestInsertLogLearnData()
        {
            var newData = new LogVocabularyScoreData();
            newData.AppSession = GenericHelper.GetTimestampForNow();
            newData.Timestamp = GenericHelper.GetTimestampForNow();
            newData.Stage = 1;
            newData.LearningBlock = 1;
            newData.PlaySession = 1;
            newData.MiniGameCode = MiniGameCode.Assessment_LetterAny;

            newData.VocabularyDataType = RandomHelper.GetRandomEnum<VocabularyDataType>();

            switch (newData.VocabularyDataType)
            {
                case VocabularyDataType.Letter:
                    newData.ElementId = RandomHelper.GetRandom(dbManager.GetAllLetterData()).GetId();
                    break;
                case VocabularyDataType.Word:
                    newData.ElementId = RandomHelper.GetRandom(dbManager.GetAllWordData()).GetId();
                    break;
                case VocabularyDataType.Phrase:
                    newData.ElementId = RandomHelper.GetRandom(dbManager.GetAllPhraseData()).GetId();
                    break;
            }

            newData.Score = RND.Range(-1f, 1f);

            this.dbManager.Insert(newData);
            PrintOutput("Inserted new LogVocabularyScoreData: " + newData.ToString());
        }

        public void TestInsertLogMoodData()
        {
            var newData = new LogMoodData();
            newData.AppSession = GenericHelper.GetTimestampForNow();
            newData.Timestamp = GenericHelper.GetTimestampForNow();

            newData.MoodValue = RND.Range(0, 20);

            this.dbManager.Insert(newData);
            PrintOutput("Inserted new LogMoodData: " + newData.ToString());
        }

        public void TestInsertLogPlayData()
        {
            var newData = new LogPlayData();
            newData.AppSession = GenericHelper.GetTimestampForNow();
            newData.Timestamp = GenericHelper.GetRelativeTimestampFromNow(-RND.Range(0, 5));

            newData.Stage = 1;
            newData.LearningBlock = 1;
            newData.PlaySession = 1;
            newData.MiniGameCode = MiniGameCode.Balloons_counting;
            newData.Score = RND.Range(0, 1f);
            newData.PlayEvent = PlayEvent.Skill;
            newData.PlaySkill = PlaySkill.Logic;
            newData.AdditionalData = "TEST";

            this.dbManager.Insert(newData);
            PrintOutput("Inserted new LogPlayData: " + newData.ToString());
        }

        /// <summary>
        /// Randomly insert vocabulary score values
        /// </summary>
        public void TestInsertVocabularyScoreData()
        {
            VocabularyDataType vocabularyDataType = RandomHelper.GetRandomEnum<VocabularyDataType>();
            string rndId = "";
            switch (vocabularyDataType)
            {
                case VocabularyDataType.Letter:
                    rndId = RandomHelper.GetRandom(dbManager.GetAllLetterData()).GetId();
                    break;
                case VocabularyDataType.Word:
                    rndId = RandomHelper.GetRandom(dbManager.GetAllWordData()).GetId();
                    break;
                case VocabularyDataType.Phrase:
                    rndId = RandomHelper.GetRandom(dbManager.GetAllPhraseData()).GetId();
                    break;
            }

            var lastAccessTimestamp = GenericHelper.GetRelativeTimestampFromNow(-RND.Range(0, 5));

            float score = RND.Range(-1f, 1f);
            bool unlocked = RND.value > 0.5f;
            dbManager.InsertOrReplace(new VocabularyScoreData(rndId, vocabularyDataType, score, unlocked, lastAccessTimestamp));

            PrintOutput("Inserted (or replaced) vocabulary score data " + lastAccessTimestamp);
        }

        /// <summary>
        /// Randomly insert journey score values
        /// </summary>
        public void TestInsertJourneyScoreData()
        {
            JourneyDataType journeyDataType = RandomHelper.GetRandomEnum<JourneyDataType>();
            string rndId = "";
            switch (journeyDataType)
            {
                case JourneyDataType.PlaySession:
                    rndId = RandomHelper.GetRandom(dbManager.GetAllPlaySessionData()).GetId();
                    break;

                case JourneyDataType.LearningBlock:
                    rndId = RandomHelper.GetRandom(dbManager.GetAllLearningBlockData()).GetId();
                    break;

                case JourneyDataType.Stage:
                    rndId = RandomHelper.GetRandom(dbManager.GetAllStageData()).GetId();
                    break;
            }

            var lastAccessTimestamp = GenericHelper.GetRelativeTimestampFromNow(-RND.Range(0, 5));

            var score = RND.Range(0, 4);
            dbManager.InsertOrReplace(new JourneyScoreData(rndId, journeyDataType, score, lastAccessTimestamp));

            PrintOutput("Inserted (or replaced) journey score data " + lastAccessTimestamp);
        }

        /// <summary>
        /// Randomly insert minigame score values
        /// </summary>
        public void TestInsertMinigameScoreData()
        {
            var minigameCode = RandomHelper.GetRandomEnum<MiniGameCode>();
            var lastAccessTimestamp = GenericHelper.GetRelativeTimestampFromNow(-RND.Range(0, 5));
            var score = RND.Range(0, 4);
            dbManager.InsertOrReplace(new MiniGameScoreData(minigameCode, score, RND.Range(1, 100f), lastAccessTimestamp));
            PrintOutput("Inserted (or replaced) minigame score data " + lastAccessTimestamp);
        }

        #endregion

        #region Test Query Log Data

        // Test that uses a simple select/where expression on a single table
        public void TestLINQLogData()
        {
            List<LogInfoData> list = this.dbManager.FindLogInfoData(x => x.Timestamp > 1000);
            DumpAllData(list);
        }

        // Test query: get all MoodData, ordered by MoodValue
        public void TestQuery_SingleTable1()
        {
            var tableName = this.dbManager.GetTableName<LogMoodData>();
            string query = "select * from \"" + tableName + "\" order by MoodValue";
            List<LogMoodData> list = this.dbManager.FindLogMoodDataByQuery(query);
            DumpAllData(list);
        }

        // Test query: get number of LogPlayData for a given PlaySession with a high enough score
        public void TestQuery_SingleTable2()
        {
            var tableName = this.dbManager.GetTableName<LogPlayData>();
            string targetPlaySessionId = "\"5\"";
            string query = "select * from \"" + tableName + "\" where Session = " + targetPlaySessionId;
            List<LogPlayData> list = this.dbManager.FindLogPlayDataByQuery(query);
            PrintOutput("Number of play data for PlaySession " + targetPlaySessionId + ": " + list.Count);
        }


        public class TestQueryResult
        {
            public int MoodValue { get; set; }
        }

        // Test query: get just the MoodValues (with a custom result class) from all LogMoodData entries
        public void TestQuery_SingleTable3()
        {
            SQLite.TableMapping resultMapping = new SQLite.TableMapping(typeof(TestQueryResult));

            string targetPlaySessionId = "\"5\"";
            string query = "select LogMoodData.MoodValue from LogMoodData where LogMoodData.Session = " + targetPlaySessionId;
            List<object> list = this.dbManager.FindCustomDataByQuery(resultMapping, query);

            string output = "Test values N: " + list.Count + "\n";
            foreach (var obj in list)
            {
                output += ("Test value: " + (obj as TestQueryResult).MoodValue) + "\n";
            }
            PrintOutput(output);
        }


        // Test query: join LogMoodData and LogPlayData by PlayerId (fake), match where they have the same PlayerId, return MoodData
        public void TestQuery_JoinTables()
        {
            SQLite.TableMapping resultMapping = new SQLite.TableMapping(typeof(TestQueryResult));

            string query = "select LogMoodData.MoodValue from LogMoodData inner join LogPlayData on LogMoodData.Session = LogPlayData.Session";
            List<object> list = this.dbManager.FindCustomDataByQuery(resultMapping, query);

            string output = "Test values N: " + list.Count + "\n";
            foreach (var obj in list)
            {
                output += ("Test value: " + (obj as TestQueryResult).MoodValue) + "\n";
            }
            PrintOutput(output);
        }
        #endregion

        #region Teacher

        public void Teacher_LastNMoods()
        {
            var list = teacherAI.GetLastMoodData(10);

            string output = "Latest 10 moods:\n";
            foreach (var data in list)
                output += GenericHelper.FromTimestamp(data.Timestamp) + ": " + data.ToString() + "\n";
            PrintOutput(output);
        }

        public void Teacher_LatestScores()
        {
            var scores = scoreHelper.GetLatestScoresForMiniGame(MiniGameCode.Balloons_counting, 3);

            string output = "Scores:\n";
            foreach (var score in scores)
            { output += score.ToString() + "\n"; }
            PrintOutput(output);
        }

        public void Teacher_AllPlaySessionScores()
        {
            var list = scoreHelper.GetCurrentScoreForAllPlaySessions();

            string output = "All play session scores:\n";
            foreach (var data in list)
            { output += data.ElementId + ": " + data.GetScore() + "\n"; }
            PrintOutput(output);
        }

        public void Teacher_FailedAssessmentLetters()
        {
            var list = teacherAI.GetFailedAssessmentLetters(MiniGameCode.Assessment_LetterAny);

            string output = "Failed letters for assessment 'Letters':\n";
            foreach (var data in list)
                output += data.ToString() + "\n";
            PrintOutput(output);
        }

        public void Teacher_FailedAssessmentWords()
        {
            var list = teacherAI.GetFailedAssessmentWords(MiniGameCode.Assessment_LetterAny);

            string output = "Failed words for assessment 'Letters':\n";
            foreach (var data in list)
                output += data.ToString() + "\n";
            PrintOutput(output);
        }

        public void Teacher_ScoreHistoryCurrentJourneyPosition()
        {
            var list = teacherAI.GetScoreHistoryForCurrentJourneyPosition();

            string output = "Score history for the current journey position (" + playerProfile.CurrentJourneyPosition.ToString() + ") in the PlayerProfile:\n";
            foreach (var data in list)
                output += GenericHelper.FromTimestamp(data.Timestamp) + ": " + data.Score + "\n";
            PrintOutput(output);
        }

        public void Teacher_PerformMiniGameSelection()
        {
            var currentJourneyPositionId = playerProfile.CurrentJourneyPosition.ToString();
            var list = teacherAI.SelectMiniGamesForPlaySession(currentJourneyPositionId, 2);

            string output = "Minigames selected (" + currentJourneyPositionId + "):\n";
            foreach (var data in list)
                output += data.Code + "\n";
            PrintOutput(output);
        }

        public void DifficultySelectionTest()
        {
            var minigameCode = RandomHelper.GetRandomEnum<MiniGameCode>();

            for (int i = 0; i < 10; i++)
            {
                var score = RND.Range(0, 4);
                var data = new LogMiniGameScoreData(0, JourneyPosition.InitialJourneyPosition, minigameCode, score, RND.Range(1, 15f));
                dbManager.Insert(data);
            }

            var difficulty = teacherAI.GetCurrentDifficulty(minigameCode);

            string output = "Minigame " + minigameCode + " selected difficulty " + difficulty;
            PrintOutput(output);
        }

        #endregion

        #region Profiles

        public void LoadProfile(string playerUuid)
        {
            dbManager.LoadDatabaseForPlayer(playerUuid);
            playerProfile = new PlayerProfile();
            playerProfile.SetCurrentJourneyPosition(new JourneyPosition(1, 2, 2), _save: false);    // test
            teacherAI.SetPlayerProfile(playerProfile);
            PrintOutput("Loading profile " + playerUuid);
        }

        public void CreateCurrentProfile()
        {
            this.dbManager.CreateProfile();
            PrintOutput("Creating tables for selected profile");
        }

        public void DeleteCurrentProfile()
        {
            this.dbManager.DropProfile();
            PrintOutput("Deleting tables for current selected profile");
        }

        public void TestDynamicProfileData()
        {
            dbManager.UpdatePlayerProfileData(
                new PlayerProfileData(DEBUG_PLAYER_UUID, 1, PlayerGender.M, PlayerTint.Blue, Color.yellow, Color.red, Color.magenta, 4, false, false, false, false,
                                      8, 0, "", 0, new AnturaSpace.ShopState(), new FirstContactState(), AppManager.I.AppEdition.editionID, AppManager.I.AppSettings.ContentID, AppManager.I.AppEdition.AppVersion)
            );
            var playerProfileData = dbManager.GetPlayerProfileData();
            PrintOutput(playerProfileData.ToString());
        }

        #endregion

        #region Rewards

        public void TestRewardUnlocks()
        {
            var jp = new JourneyPosition(1, 1, 2);
            dbManager.UpdateRewardPackUnlockData(new RewardPackUnlockData(0, "aaa_black", jp));
            dbManager.UpdateRewardPackUnlockData(new RewardPackUnlockData(0, "bbb_black", jp));
            dbManager.UpdateRewardPackUnlockData(new RewardPackUnlockData(0, "ccc_black", jp));
            var rewardPackUnlockDatas = dbManager.GetAllRewardPackUnlockData();
            DumpAllData(rewardPackUnlockDatas);
        }

        #endregion

        #region Inner Dumps

        public void DumpAllData<T>(List<T> list) where T : IData
        {
            string output = "";
            foreach (var data in list)
            {
                output += (data.GetId() + ": " + data.ToString()) + "\n";
            }
            PrintOutput(output);
        }

        public void DumpDataById(string id, IData data)
        {
            string output = "";
            if (data != null)
            {
                output += (data.GetId() + ": " + data.ToString());
            }
            else
            {
                output += "No data with ID " + id;
            }
            PrintOutput(output);
        }

        #endregion

        #region Utilities

        public void PrintOutput(string output)
        {
            Debug.Log(output);
            OutputText.text = output.Substring(0, Mathf.Min(1000, output.Length));
        }

        void PrintArabicOutput(string output)
        {
            OutputTextArabic.text = output;//ArabicAlphabetHelper.PrepareStringForDisplay(output);
        }

        #endregion

        #region Section Switch

        public Canvas DatabaseManagerCanvas;
        public Canvas TeacherTesterCanvas;

        public void ToTeacherTester()
        {
            DatabaseManagerCanvas.enabled = false;
            TeacherTesterCanvas.enabled = true;
        }

        public void ToDatabaseManager()
        {
            DatabaseManagerCanvas.enabled = true;
            TeacherTesterCanvas.enabled = false;
        }

        #endregion
    }
}
#endif
