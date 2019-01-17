#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using Antura.Core;

namespace Antura.Database.Management
{
    /// <summary>
    /// Management class that converts JSON data to static database custom assets.
    /// </summary>
    public class DatabaseLoader : MonoBehaviour
    {
        public DatabaseInputData inputData;
        private DatabaseObject _databaseObject;
        public bool verbose;
        [HideInInspector]
        public LanguageCode langCode;

        public void RecreateDatabase()
        {
            CreateDatabaseAsset.CreateAssets("Assets/Resources/" + DatabaseManager.STATIC_DATABASE_NAME+"/", DatabaseManager.STATIC_DATABASE_NAME);
            this._databaseObject = DatabaseObject.LoadDB(langCode, DatabaseManager.STATIC_DATABASE_NAME);
        }

        public void CopyCurrentDatabaseForTesting()
        {
            this._databaseObject = DatabaseObject.LoadDB(langCode, DatabaseManager.STATIC_DATABASE_NAME);

            var test_db = DatabaseObject.LoadDB(langCode, DatabaseManager.STATIC_DATABASE_NAME_TEST);
            if (!test_db.HasTables())
            {
                CreateDatabaseAsset.CreateAssets("Assets/Resources/" + DatabaseManager.STATIC_DATABASE_NAME_TEST+"/", DatabaseManager.STATIC_DATABASE_NAME_TEST);
                test_db = DatabaseObject.LoadDB(langCode, DatabaseManager.STATIC_DATABASE_NAME_TEST);
            }

            {
                var table = test_db.GetLetterTable();
                table.Clear();
                table.AddRange(this._databaseObject.GetLetterTable().GetValuesTyped());
            }

            {
                var table = test_db.GetWordTable();
                table.Clear();
                table.AddRange(this._databaseObject.GetWordTable().GetValuesTyped());
            }

            {
                var table = test_db.GetPhraseTable();
                table.Clear();
                table.AddRange(this._databaseObject.GetPhraseTable().GetValuesTyped());
            }

            {
                var table = test_db.GetLocalizationTable();
                table.Clear();
                table.AddRange(this._databaseObject.GetLocalizationTable().GetValuesTyped());
            }

            {
                var table = test_db.GetMiniGameTable();
                table.Clear();
                table.AddRange(this._databaseObject.GetMiniGameTable().GetValuesTyped());
            }

            {
                var table = test_db.GetPlaySessionTable();
                table.Clear();
                table.AddRange(this._databaseObject.GetPlaySessionTable().GetValuesTyped());
            }

            {
                var table = test_db.GetLearningBlockTable();
                table.Clear();
                table.AddRange(this._databaseObject.GetLearningBlockTable().GetValuesTyped()); 
            }

            {
                var table = test_db.GetStageTable();
                table.Clear();
                table.AddRange(this._databaseObject.GetStageTable().GetValuesTyped());
            }

            {
                var table = test_db.GetRewardTable();
                table.Clear();
                table.AddRange(this._databaseObject.GetRewardTable().GetValuesTyped());
            }

            Debug.Log("Database copied");
            AssetDatabase.SaveAssets();
        }

        public void RegenerateEnums()
        {
            if (verbose) Debug.Log("Regenerating enums from JSON files...");

            RegenerateEnumsFrom(inputData);

            if (verbose) Debug.Log("Finished regenerating enums!");
        }

        private void RegenerateEnumsFrom(DatabaseInputData DBInputData)
        {
            {
                Debug.Log("Generating Letters enums...");
                var parser = new LetterParser();
                parser.RegenerateEnums(DBInputData.letterDataAsset.text);
            }

            {
                Debug.Log("Generating Words enums...");
                var parser = new WordParser();
                parser.RegenerateEnums(DBInputData.wordDataAsset.text);
            }

            {
                Debug.Log("Generating Phrases enums...");
                var parser = new PhraseParser();
                parser.RegenerateEnums(DBInputData.phraseDataAsset.text);
            }

            {
                Debug.Log("Generating MiniGames enums...");
                var parser = new MiniGameParser();
                parser.RegenerateEnums(DBInputData.minigameDataAsset.text);
            }

            {
                Debug.Log("Generating PlaySessions enums...");
                var parser = new PlaySessionParser();
                parser.RegenerateEnums(DBInputData.playSessionDataAsset.text);
            }

            {
                Debug.Log("Generating LearningBlocks enums...");
                var parser = new LearningBlockParser();
                parser.RegenerateEnums(DBInputData.playSessionDataAsset.text);  // @note: LearningBlockParser works on the same table of playSessionData
            }

            {
                Debug.Log("Generating Localization enums...");
                var parser = new LocalizationParser();
                parser.RegenerateEnums(DBInputData.localizationDataAsset.text);
            }

            {
                Debug.Log("Generating Stages enums...");
                var parser = new StageParser();
                parser.RegenerateEnums(DBInputData.stageDataAsset.text);
            }

            {
                Debug.Log("Generating Rewards enums...");
                var parser = new RewardParser();
                parser.RegenerateEnums(DBInputData.rewardDataAsset.text);
            }
        }

        #region Loading

        /// <summary>
        /// Load all database values from scriptable objects
        /// </summary>
        public void LoadDatabase()
        {
            if (verbose) Debug.Log("Loading data from JSON files...");

            this._databaseObject = DatabaseObject.LoadDB(langCode, DatabaseManager.STATIC_DATABASE_NAME);
            LoadDataFrom(inputData);

            if (verbose) Debug.Log("Finished loading!");
        }

        /// <summary>
        /// Load input data and place it inside the database.
        /// </summary>
        /// <param name="DBInputData"></param>
        private void LoadDataFrom(DatabaseInputData DBInputData)
        {
            {
                Debug.Log("Loading Letters...");
                var parser = new LetterParser();
                parser.Parse(DBInputData.letterDataAsset.text, _databaseObject, _databaseObject.GetLetterTable());
            }

            {
                // @note: depends on Letter
                Debug.Log("Loading Words...");
                var parser = new WordParser();
                parser.Parse(DBInputData.wordDataAsset.text, _databaseObject, _databaseObject.GetWordTable());
            }

            {
                // @note: depends on Word
                Debug.Log("Loading Phrases...");
                var parser = new PhraseParser();
                parser.Parse(DBInputData.phraseDataAsset.text, _databaseObject, _databaseObject.GetPhraseTable());
            }

            {
                Debug.Log("Loading MiniGames...");
                var parser = new MiniGameParser();
                parser.Parse(DBInputData.minigameDataAsset.text, _databaseObject, _databaseObject.GetMiniGameTable());
            }

            {
                // @note: depends on Minigame
                Debug.Log("Loading PlaySessions...");
                var parser = new PlaySessionParser();
                parser.Parse(DBInputData.playSessionDataAsset.text, _databaseObject, _databaseObject.GetPlaySessionTable());
            }

            {
                // @note: depends on Letter, Word, Phrase, PlaySession
                Debug.Log("Loading LearningBlocks...");
                var parser = new LearningBlockParser();
                parser.Parse(DBInputData.playSessionDataAsset.text, _databaseObject, _databaseObject.GetLearningBlockTable());
            }

            {
                Debug.Log("Loading Localization...");
                var parser = new LocalizationParser();
                parser.Parse(DBInputData.localizationDataAsset.text, _databaseObject, _databaseObject.GetLocalizationTable());
            }

            {
                Debug.Log("Loading Stages...");
                var parser = new StageParser();
                parser.Parse(DBInputData.stageDataAsset.text, _databaseObject, _databaseObject.GetStageTable());
            }

            {
                Debug.Log("Loading Rewards...");
                var parser = new RewardParser();
                parser.Parse(DBInputData.rewardDataAsset.text, _databaseObject, _databaseObject.GetRewardTable());
            }

            // Save database modifications
            EditorUtility.SetDirty(_databaseObject.stageDb);
            EditorUtility.SetDirty(_databaseObject.minigameDb);
            EditorUtility.SetDirty(_databaseObject.rewardDb);
            EditorUtility.SetDirty(_databaseObject.letterDb);
            EditorUtility.SetDirty(_databaseObject.wordDb);
            EditorUtility.SetDirty(_databaseObject.phraseDb);
            EditorUtility.SetDirty(_databaseObject.localizationDb);
            EditorUtility.SetDirty(_databaseObject.learningblockDb);
            EditorUtility.SetDirty(_databaseObject.playsessionDb);
            AssetDatabase.SaveAssets();
        }
        #endregion

    }
}
#endif