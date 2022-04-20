using Antura.Core;
using Antura.Helpers;
using Antura.Language;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Antura.Database
{
    /// <summary>
    /// Entry point for the rest of the application to access database entries.
    /// This class is responsible for loading all data and provide access to both static (learning) and dynamic (logging) data.
    /// </summary>
    public class DatabaseManager
    {
        public const string STATIC_DATABASE_NAME = "Database";
        public const string STATIC_DATABASE_NAME_TEST = STATIC_DATABASE_NAME + "_Test";

        // DB references
        private DatabaseObject staticDb;
        private DBService dynamicDb;
        private LanguageCode langCode;

        public List<Type> staticDataTypes = new List<Type>()
        {
            typeof(StageData),
            typeof(PlaySessionData),
            typeof(LearningBlockData),
            typeof(MiniGameData),
            typeof(LetterData),
            typeof(WordData),
            typeof(PhraseData),
            typeof(LocalizationData),
            typeof(RewardData)
        };

        public List<Type> dynamicDataTypes = new List<Type>()
        {
            typeof(VocabularyScoreData),
            typeof(JourneyScoreData),
            typeof(MiniGameScoreData),
            typeof(LogInfoData),
            typeof(LogMoodData),
            typeof(LogPlayData),
            typeof(LogMiniGameScoreData),
            typeof(LogPlaySessionScoreData),
            typeof(LogVocabularyScoreData),
            typeof(DatabaseInfoData),
            typeof(PlayerProfileData),
            typeof(RewardPackUnlockData)
        };

        public DatabaseObject StaticDatabase
        {
            get { return staticDb; }
        }

        public bool HasLoadedPlayerProfile()
        {
            return dynamicDb != null;
        }

        #region Player assignment

        public void CreateDatabaseForPlayer(PlayerProfileData playerProfileData)
        {
            SetPlayerProfile(playerProfileData.Uuid);
            UpdatePlayerProfileData(playerProfileData);
        }

        public PlayerProfileData LoadDatabaseForPlayer(string playerUuid)
        {
            SetPlayerProfile(playerUuid);
            return GetPlayerProfileData();
        }

        #endregion

        public DatabaseManager(ContentEditionConfig fromEdition, LanguageCode langCode = LanguageCode.english)
        {
            this.langCode = langCode;
            // Only the static DB is available until the player profile is also assigned
            LoadStaticDB(fromEdition);
        }

        private void SetPlayerProfile(string playerUuid)
        {
            // SAFE MODE: we need to make sure that the static db has some entries, otherwise there is something wrong
            if (staticDb.GetPlaySessionTable().GetDataCount() == 0)
            {
                throw new System.Exception(
                    "Database is empty, it was probably not setup correctly. Make sure it has been statically loaded by the management scene.");
            }

            // We load the selected player profile
            LoadDynamicDbForPlayerProfile(playerUuid);
        }


        void LoadStaticDB(ContentEditionConfig fromEdition)
        {
            var dbName = STATIC_DATABASE_NAME;
            this.staticDb = DatabaseObject.LoadDB(fromEdition, langCode, dbName);
        }

        #region Profile

        private void LoadDynamicDbForPlayerProfile(string playerUuid)
        {
            dynamicDb = DBService.OpenFromPlayerUUID(true, playerUuid);
        }

        public void UnloadCurrentProfile()
        {
            dynamicDb = null;
        }

        public void DeleteCurrentProfile()
        {
            dynamicDb.ForceFileDeletion();
            dynamicDb = null;
        }

        public void CreateProfile()
        {
            dynamicDb.CreateAllTables();
        }

        public void RecreateProfile()
        {
            dynamicDb.RecreateAllTables();
        }

        public void DropProfile()
        {
            dynamicDb.DropAllTables();
        }

        #endregion

        #region Player Profile Data

        public void UpdatePlayerProfileData(PlayerProfileData playerProfileData)
        {
            //Debug.Log("UPDATING " + playerProfileData.ToString());
            dynamicDb.InsertOrReplace(playerProfileData);
        }

        public PlayerProfileData GetPlayerProfileData()
        {
            var data = dynamicDb.GetPlayerProfileData();
            //Debug.Log("LOADING " + data.ToString());
            return data;
        }

        #endregion

        #region Utilities

        // Utilities
        public string GetTableName<T>()
        {
            return dynamicDb.GetTableName<T>();
        }

        public void Insert<T>(T data) where T : IData, new()
        {
            dynamicDb.Insert(data);
        }

        public void InsertOrReplace<T>(T data) where T : IData, new()
        {
            dynamicDb.InsertOrReplace(data);
        }

        public void InsertAll<T>(IEnumerable<T> objects) where T : IData, new()
        {
            dynamicDb.InsertAll<T>(objects);
        }

        public void InsertOrReplaceAll<T>(IEnumerable<T> objects) where T : IData, new()
        {
            dynamicDb.InsertOrReplaceAll<T>(objects);
        }

        public void DeleteAll<T>() where T : IData, new()
        {
            dynamicDb.DeleteAll<T>();
        }

        #endregion

        #region Letter

        public LetterData GetLetterDataById(string id)
        {
            return staticDb.GetById(staticDb.GetLetterTable(), id);
        }

        public List<LetterData> FindLetterData(Predicate<LetterData> predicate)
        {
            return staticDb.FindAll(staticDb.GetLetterTable(), predicate);
        }

        public List<LetterData> GetAllLetterData()
        {
            return new List<LetterData>(staticDb.GetLetterTable().GetValuesTyped());
        }

        #endregion

        #region Word

        public WordData GetWordDataById(string id)
        {
            return staticDb.GetById(staticDb.GetWordTable(), id);
        }

        public List<WordData> GetAllWordData()
        {
            return new List<WordData>(staticDb.GetWordTable().GetValuesTyped());
        }

        public List<WordData> FindWordData(Predicate<WordData> predicate)
        {
            return staticDb.FindAll(staticDb.GetWordTable(), predicate);
        }

        public IEnumerable<WordData> FindWordDataOptimized(Predicate<WordData> predicate)
        {
            return staticDb.FindAllOptimized(staticDb.GetWordTable(), predicate);
        }

        public List<WordData> FindWordDataByCategory(WordDataCategory wordCategory)
        {
            return staticDb.FindAll(staticDb.GetWordTable(), (x) => (x.Category == wordCategory));
        }

        #endregion

        #region MiniGame

        public MiniGameData GetMiniGameDataByCode(MiniGameCode code)
        {
            return GetMiniGameDataById(code.ToString());
        }

        private MiniGameData GetMiniGameDataById(string id)
        {
            return staticDb.GetById(staticDb.GetMiniGameTable(), id);
        }

        public List<MiniGameData> GetActiveMinigames()
        {
            return FindMiniGameData((x) => (x.Active && x.Type == MiniGameDataType.MiniGame));
        }

        public List<MiniGameData> GetAllMiniGameData()
        {
            return new List<MiniGameData>(staticDb.GetMiniGameTable().GetValuesTyped());
        }

        public List<MiniGameData> FindMiniGameData(Predicate<MiniGameData> predicate)
        {
            return staticDb.FindAll(staticDb.GetMiniGameTable(), predicate);
        }

        #endregion

        #region LearningBlock

        public LearningBlockData GetLearningBlockDataById(string id)
        {
            return staticDb.GetById(staticDb.GetLearningBlockTable(), id);
        }

        public List<LearningBlockData> FindLearningBlockData(Predicate<LearningBlockData> predicate)
        {
            return staticDb.FindAll(staticDb.GetLearningBlockTable(), predicate);
        }

        public List<LearningBlockData> GetAllLearningBlockData()
        {
            return new List<LearningBlockData>(staticDb.GetLearningBlockTable().GetValuesTyped());
        }

        // @note: new generic-only data getter, should be used instead of all the above ones
        public List<T> GetAllData<T>(DbTables table) where T : IData
        {
            return staticDb.GetAll<T>((SerializableDataTable<T>)staticDb.GetTable(table));
        }

        #endregion

        #region PlaySession

        public bool HasPlaySessionDataById(string id)
        {
            return staticDb.HasById(staticDb.GetPlaySessionTable(), id);
        }

        public PlaySessionData GetPlaySessionDataById(string id)
        {
            return staticDb.GetById(staticDb.GetPlaySessionTable(), id);
        }

        public List<PlaySessionData> FindPlaySessionData(Predicate<PlaySessionData> predicate)
        {
            return staticDb.FindAll(staticDb.GetPlaySessionTable(), predicate);
        }

        public List<PlaySessionData> GetPlaySessionsOfLearningBlock(LearningBlockData lb)
        {
            return FindPlaySessionData(x => x.Stage == lb.Stage && x.LearningBlock == lb.LearningBlock);
        }

        public List<PlaySessionData> GetAllPlaySessionData()
        {
            return new List<PlaySessionData>(staticDb.GetPlaySessionTable().GetValuesTyped());
        }

        #endregion

        #region Phrase

        public PhraseData GetPhraseDataById(string id)
        {
            return staticDb.GetById(staticDb.GetPhraseTable(), id);
        }

        public List<PhraseData> FindPhraseData(Predicate<PhraseData> predicate)
        {
            return staticDb.FindAll(staticDb.GetPhraseTable(), predicate);
        }

        public List<PhraseData> GetAllPhraseData()
        {
            return new List<PhraseData>(staticDb.GetPhraseTable().GetValuesTyped());
        }

        #endregion

        #region Localization

        public LocalizationData GetLocalizationDataById(string id)
        {
            var locData = staticDb.GetById(staticDb.GetLocalizationTable(), id);
            if (locData != null)
            {
                return locData;
            }
            return new LocalizationData { Id = id };
        }

        public List<LocalizationData> GetAllLocalizationData()
        {
            return new List<LocalizationData>(staticDb.GetLocalizationTable().GetValuesTyped());
        }

        public List<LocalizationData> FindLocalizationData(Predicate<LocalizationData> predicate)
        {
            return staticDb.FindAll(staticDb.GetLocalizationTable(), predicate);
        }

        #endregion

        #region Stage

        public StageData GetStageDataById(string id)
        {
            return staticDb.GetById(staticDb.GetStageTable(), id);
        }

        public List<StageData> GetAllStageData()
        {
            return new List<StageData>(staticDb.GetStageTable().GetValuesTyped());
        }

        public List<StageData> FindStageData(Predicate<StageData> predicate)
        {
            return staticDb.FindAll(staticDb.GetStageTable(), predicate);
        }

        #endregion

        #region Reward

        public RewardData GetRewardDataById(string id)
        {
            return staticDb.GetById(staticDb.GetRewardTable(), id);
        }

        public List<RewardData> GetAllRewardData()
        {
            return new List<RewardData>(staticDb.GetRewardTable().GetValuesTyped());
        }

        public List<RewardData> FindRewardData(Predicate<RewardData> predicate)
        {
            return staticDb.FindAll(staticDb.GetRewardTable(), predicate);
        }

        #endregion

        #region Log

        public List<LogInfoData> GetAllLogInfoData()
        {
            return dynamicDb.FindAll<LogInfoData>();
        }

        public List<LogMoodData> GetAllLogMoodData()
        {
            return dynamicDb.FindAll<LogMoodData>();
        }

        public List<LogVocabularyScoreData> GetAllVocabularyScoreData()
        {
            return dynamicDb.FindAll<LogVocabularyScoreData>();
        }

        public List<LogPlayData> GetAllLogPlayData()
        {
            return dynamicDb.FindAll<LogPlayData>();
        }

        public List<T> GetAllDynamicData<T>() where T : IData, new()
        {
            return dynamicDb.FindAll<T>();
        }

        // Find all (expression)
        public List<LogInfoData> FindLogInfoData(System.Linq.Expressions.Expression<Func<LogInfoData, bool>> expression)
        {
            return dynamicDb.FindAll(expression);
        }

        // Get by id
        public LogInfoData GetLogInfoDataById(string id)
        {
            return dynamicDb.FindLogInfoDataById(id);
        }

        // Query
        public List<LogInfoData> FindLogInfoDataByQuery(string query)
        {
            return dynamicDb.Query<LogInfoData>(query);
        }

        public List<LogVocabularyScoreData> FindLogVocabularyScoreDataByQuery(string query)
        {
            return dynamicDb.Query<LogVocabularyScoreData>(query);
        }

        public List<LogMoodData> FindLogMoodDataByQuery(string query)
        {
            return dynamicDb.Query<LogMoodData>(query);
        }

        public List<LogPlayData> FindLogPlayDataByQuery(string query)
        {
            return dynamicDb.Query<LogPlayData>(query);
        }

        public List<T> Query<T>(string query) where T : IData, new()
        {
            return dynamicDb.Query<T>(query);
        }

        public List<object> Query(Type t, string query)
        {
            return dynamicDb.Query(t, query);
        }

        public List<object> FindCustomDataByQuery(SQLite.TableMapping mapping, string query)
        {
            return dynamicDb.FindByQueryCustom(mapping, query);
        }

        #endregion


        #region Reward Unlock

        public List<RewardPackUnlockData> GetAllRewardPackUnlockData()
        {
            /*
            Debug.Log("DB getting data list: " + GetAllDynamicData<RewardPackUnlockData>().Count);
            foreach (var rewardPackUnlockData in GetAllDynamicData<RewardPackUnlockData>())
                Debug.Log("LOAD PACK: " + rewardPackUnlockData.ToString());
                */

            return GetAllDynamicData<RewardPackUnlockData>();
        }

        public void UpdateRewardPackUnlockData(RewardPackUnlockData updatedData)
        {
            dynamicDb.InsertOrReplace(updatedData);
        }

        public void UpdateRewardPackUnlockDataAll(List<RewardPackUnlockData> updatedDataList)
        {
            /*Debug.Log("DB updating data list: " + updatedDataList.Count);
            foreach (var rewardPackUnlockData in updatedDataList)
                Debug.Log("INSERT PACK: " + rewardPackUnlockData.ToString());
            */

            dynamicDb.InsertOrReplaceAll(updatedDataList);
        }

        #endregion

        #region Export

        public bool ExportPlayerDb(string playerUuid)
        {
            // Create a new service for the copied database
            // This will copy the current database
            var exportDbService = DBService.ExportFromPlayerUUIDAndReopen(playerUuid);

            InjectUUID(playerUuid, exportDbService);
            InjectStaticData(exportDbService);
            InjectEnums(exportDbService);

            exportDbService.CloseConnection();

            return true;
        }

        /// <summary>
        /// Exports the players joined db reading them from the import directory
        /// </summary>
        /// <returns><c>true</c>, if players joined db was exported, <c>false</c> otherwise.</returns>
        /// <param name="errorString">Error string.</param>
        public bool ExportPlayersJoinedDb(out string errorString)
        {
            // Load all the databases we can find and get the player UUIDs
            var allUUIDs = new List<string>();
            var filePaths = GetImportFilePaths();
            if (filePaths != null)
            {
                foreach (var filePath in filePaths)
                {
                    // Check whether that is a DB and load it
                    if (IsValidDatabasePath(filePath))
                    {
                        var importDbService = DBService.OpenFromFilePath(false, filePath);
                        var playerProfileData = importDbService.GetPlayerProfileData();
                        if (playerProfileData == null)
                        {
                            // skip no-player DBs, they are wrong
                            continue;
                        }
                        allUUIDs.Add(playerProfileData.Uuid);
                        importDbService.CloseConnection();
                    }
                }
            }
            else
            {
                errorString = "Could not find the import folder: " + AppConfig.DbImportFolder;
                Debug.LogError(errorString);
                return false;
            }

            // Create the joined DB
            var joinedDbService = DBService.OpenFromDirectoryAndFilename(true, AppConfig.GetJoinedDatabaseFilename(), AppConfig.DbJoinedFolder);
            InjectStaticData(joinedDbService);
            InjectEnums(joinedDbService);

            // Export and inject all the DBs
            foreach (var uuid in allUUIDs)
            {
                // Export
                var exportDbService = DBService.ExportFromPlayerUUIDAndReopen(uuid, dirName: AppConfig.DbImportFolder);
                InjectUUID(uuid, exportDbService);

                // Inject
                InjectExportedDB(uuid, exportDbService, joinedDbService);
                exportDbService.CloseConnection();
                exportDbService.ForceFileDeletion();
            }

            joinedDbService.CloseConnection();
            errorString = "";
            return true;
        }

        public string[] GetImportFilePaths()
        {
            var importDirectory = DBService.GetDatabaseDirectoryPath(AppConfig.DbImportFolder);
            if (Directory.Exists(importDirectory))
            {
                string[] filePaths = Directory.GetFiles(importDirectory);
                return filePaths;
            }
            return null;
        }

        public bool IsValidDatabasePath(string dbpath)
        {
            return (dbpath.Contains(AppConfig.DbFileExtension));

        }

        public PlayerProfileData ImportDynamicDatabase(string importFilePath)
        {
            if (!File.Exists(importFilePath))
            {
                Debug.LogError("Cannot find database file for import: " + importFilePath);
                return null;
            }

            // Copy the file
            string fileName = Path.GetFileName(importFilePath);
            string newFilePath = DBService.GetDatabaseFilePath(fileName, AppConfig.DbPlayersFolder);
            /* @note: we overwrite it
                if (File.Exists(newFilePath)) {
                Debug.LogError("Database already exists. Cannot import: " + importFilePath);
                return null;
            }*/

            File.Copy(importFilePath, newFilePath, true);

            // Load the new DB and get its player profile data
            var importDbService = DBService.OpenFromFilePath(false, newFilePath);
            var playerProfileData = importDbService.GetPlayerProfileData();
            importDbService.CloseConnection();
            return playerProfileData;
        }

        private void InjectExportedDB(string uuid, DBService exportDbService, DBService joinedDbService)
        {
            foreach (Type dynamicDataType in dynamicDataTypes)
            {
                string query = "SELECT * FROM " + dynamicDataType.Name;
                var objectList = exportDbService.Query(dynamicDataType, query);
                var iDataList = objectList.ConvertAll(x => (IData)x);

                foreach (var element in iDataList)
                {
                    if (element is IDataEditable)
                    {
                        (element as IDataEditable).SetId(element.GetId() + "_" + uuid);
                    }
                }

                joinedDbService.InsertAllObjects(iDataList);
            }
        }

        private static void InjectEnums(DBService exportDbService)
        {
            exportDbService.ExportEnum<AppScene>();
            exportDbService.ExportEnum<JourneyDataType>();
            exportDbService.ExportEnum<LearningBlockDataFocus>();
            exportDbService.ExportEnum<LetterDataKind>();
            exportDbService.ExportEnum<LetterDataSunMoon>();
            exportDbService.ExportEnum<LetterDataType>();
            exportDbService.ExportEnum<LocalizationDataId>();
            exportDbService.ExportEnum<MiniGameDataType>();
            exportDbService.ExportEnum<PhraseDataCategory>();
            exportDbService.ExportEnum<InfoEvent>();
            exportDbService.ExportEnum<PlaySkill>();
            exportDbService.ExportEnum<PlayEvent>();
            exportDbService.ExportEnum<PlaySessionDataOrder>();
            //exportDbService.ExportEnum<RewardDataCategory>();
            exportDbService.ExportEnum<VocabularyDataGender>();
            exportDbService.ExportEnum<VocabularyDataType>();
            exportDbService.ExportEnum<WordDataArticle>();
            exportDbService.ExportEnum<WordDataCategory>();
            exportDbService.ExportEnum<WordDataForm>();
            exportDbService.ExportEnum<WordDataKind>();
        }

        private void InjectStaticData(DBService dbService)
        {
            try
            {
                dbService.GenerateStaticExportTables();
                dbService.InsertAll(StaticDatabase.GetStageTable().GetValuesTyped());
                dbService.InsertAll(StaticDatabase.GetPlaySessionTable().GetValuesTyped());
                dbService.InsertAll(StaticDatabase.GetLearningBlockTable().GetValuesTyped());
                dbService.InsertAll(StaticDatabase.GetMiniGameTable().GetValuesTyped());
                dbService.InsertAll(StaticDatabase.GetLetterTable().GetValuesTyped());
                dbService.InsertAll(StaticDatabase.GetWordTable().GetValuesTyped());
                dbService.InsertAll(StaticDatabase.GetPhraseTable().GetValuesTyped());
                dbService.InsertAll(StaticDatabase.GetLocalizationTable().GetValuesTyped());
                dbService.InsertAll(StaticDatabase.GetRewardTable().GetValuesTyped());
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        private void InjectUUID(string playerUuid, DBService exportDbService)
        {
            foreach (var type in dynamicDataTypes)
            {
                PopulateUUID(type, playerUuid, exportDbService);
            }
        }

        private void PopulateUUID(Type t, string playerUuid, DBService exportDbService)
        {
            string query = "UPDATE " + t.Name + " SET Uuid = \"" + playerUuid + "\"";
            exportDbService.Query(t, query);
        }

        #endregion
    }
}
