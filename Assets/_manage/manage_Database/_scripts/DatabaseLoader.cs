#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using Antura.Core;
using Antura.Language;

namespace Antura.Database.Management
{
    /// <summary>
    /// Management class that converts JSON data to static database custom assets.
    /// </summary>
    public class DatabaseLoader : MonoBehaviour
    {
        private DatabaseObject _databaseObject;

        public bool ImportLocalizations;
        public bool ImportLetters;
        public bool ImportWords;
        public bool ImportPhrases;
        public bool ImportPlaySessions;
        public bool ImportLearningBlocks;
        public bool ImportMiniGames;
        public bool ImportStages;
        public bool ImportRewards;

        [HideInInspector]
        public LanguageCode langCode;
        [HideInInspector]
        public ContentEditionConfig InputContent;

        public void RecreateDatabase()
        {
            CreateDatabaseAsset.CreateAssets("Assets/Resources/" + DatabaseManager.STATIC_DATABASE_NAME + "/", DatabaseManager.STATIC_DATABASE_NAME);
            this._databaseObject = DatabaseObject.LoadDB(AppManager.I.ContentEdition, langCode, DatabaseManager.STATIC_DATABASE_NAME);
        }

        public void CopyCurrentDatabaseForTesting()
        {
            this._databaseObject = DatabaseObject.LoadDB(AppManager.I.ContentEdition, langCode, DatabaseManager.STATIC_DATABASE_NAME);

            var test_db = DatabaseObject.LoadDB(AppManager.I.ContentEdition, langCode, DatabaseManager.STATIC_DATABASE_NAME_TEST);
            if (!test_db.HasTables())
            {
                CreateDatabaseAsset.CreateAssets("Assets/Resources/" + DatabaseManager.STATIC_DATABASE_NAME_TEST + "/", DatabaseManager.STATIC_DATABASE_NAME_TEST);
                test_db = DatabaseObject.LoadDB(AppManager.I.ContentEdition, langCode, DatabaseManager.STATIC_DATABASE_NAME_TEST);
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
            Debug.Log("Regenerating enums from JSON files...");

            RegenerateEnumsFrom(InputContent.DBImportDataFiles);

            Debug.Log("Finished regenerating enums!");
        }

        private void RegenerateEnumsFrom(DatabaseInputData DBInputData)
        {
            if (ImportLetters)
            {
                {
                    Debug.Log("Generating Letters enums...");
                    var parser = new LetterParser();
                    parser.RegenerateEnums(DBInputData.letterDataAsset.text);
                }
            }

            if (ImportWords)
            {
                {
                    Debug.Log("Generating Words enums...");
                    var parser = new WordParser();
                    parser.RegenerateEnums(DBInputData.wordDataAsset.text);
                }
            }

            if (ImportPhrases)
            {
                {
                    Debug.Log("Generating Phrases enums...");
                    var parser = new PhraseParser();
                    parser.RegenerateEnums(DBInputData.phraseDataAsset.text);
                }
            }

            if (ImportMiniGames)
            {
                {
                    Debug.Log("Generating MiniGames enums...");
                    var parser = new MiniGameParser();
                    parser.RegenerateEnums(DBInputData.minigameDataAsset.text);
                }
            }

            if (ImportPlaySessions)
            {
                {
                    Debug.Log("Generating PlaySessions enums...");
                    var parser = new PlaySessionParser();
                    parser.RegenerateEnums(DBInputData.playSessionDataAsset.text);
                }
            }

            if (ImportLearningBlocks)
            {
                {
                    Debug.Log("Generating LearningBlocks enums...");
                    var parser = new LearningBlockParser();
                    parser.RegenerateEnums(DBInputData.playSessionDataAsset.text);  // @note: LearningBlockParser works on the same table of playSessionData
                }
            }

            if (ImportLocalizations)
            {
                {
                    Debug.Log("Generating Localization enums...");
                    var parser = new LocalizationParser();
                    parser.RegenerateEnums(DBInputData.localizationDataAsset.text);
                }
            }

            if (ImportStages)
            {
                {
                    Debug.Log("Generating Stages enums...");
                    var parser = new StageParser();
                    parser.RegenerateEnums(DBInputData.stageDataAsset.text);
                }
            }

            if (ImportRewards)
            {
                {
                    Debug.Log("Generating Rewards enums...");
                    var parser = new RewardParser();
                    parser.RegenerateEnums(DBInputData.rewardDataAsset.text);
                }
            }
        }

        #region Loading

        /// <summary>
        /// Load all database values from scriptable objects
        /// </summary>
        public void LoadDatabase(ContentEditionConfig edition)
        {
            // Debug.Log("Loading data from JSON files...");

            this._databaseObject = DatabaseObject.LoadDB(edition, langCode, DatabaseManager.STATIC_DATABASE_NAME);
            LoadDataFrom(InputContent.DBImportDataFiles);

            Debug.Log("Finished loading!");
        }

        /// <summary>
        /// Load input data and place it inside the database.
        /// </summary>
        /// <param name="DBInputData"></param>
        private void LoadDataFrom(DatabaseInputData DBInputData)
        {
            if (ImportLetters)
            {
                {
                    Debug.Log("Loading Letters from JSON: " + DBInputData.letterDataAsset.name + " for lang: " + langCode);
                    var parser = new LetterParser();
                    parser.Parse(DBInputData.letterDataAsset.text, _databaseObject, _databaseObject.GetLetterTable(), langCode);
                }
                EditorUtility.SetDirty(_databaseObject.letterDb);
            }

            if (ImportWords)
            {
                {
                    // @note: depends on Letter
                    Debug.Log("Loading Words...");
                    var parser = new WordParser();
                    parser.Parse(DBInputData.wordDataAsset.text, _databaseObject, _databaseObject.GetWordTable(), langCode);
                }
                EditorUtility.SetDirty(_databaseObject.wordDb);
            }

            if (ImportPhrases)
            {
                {
                    // @note: depends on Word
                    Debug.Log("Loading Phrases...");
                    var parser = new PhraseParser();
                    parser.Parse(DBInputData.phraseDataAsset.text, _databaseObject, _databaseObject.GetPhraseTable(), langCode);
                }
                EditorUtility.SetDirty(_databaseObject.phraseDb);
            }

            if (ImportLocalizations)
            {
                {
                    Debug.Log("Loading Localization...");
                    var parser = new LocalizationParser();
                    parser.Parse(DBInputData.localizationDataAsset.text, _databaseObject, _databaseObject.GetLocalizationTable(), langCode);
                }
                EditorUtility.SetDirty(_databaseObject.localizationDb);
            }


            if (ImportMiniGames)
            {
                {
                    Debug.Log("Loading MiniGames...");
                    var parser = new MiniGameParser();
                    parser.Parse(DBInputData.minigameDataAsset.text, _databaseObject, _databaseObject.GetMiniGameTable(), langCode);
                }
                EditorUtility.SetDirty(_databaseObject.minigameDb);
            }

            if (ImportPlaySessions)
            {
                {
                    // @note: depends on Minigame
                    Debug.Log("Loading PlaySessions...");
                    var parser = new PlaySessionParser();
                    parser.Parse(DBInputData.playSessionDataAsset.text, _databaseObject, _databaseObject.GetPlaySessionTable(), langCode);
                }
                EditorUtility.SetDirty(_databaseObject.playsessionDb);
            }

            if (ImportLearningBlocks)
            {
                {
                    // @note: depends on Letter, Word, Phrase, PlaySession
                    Debug.Log("Loading LearningBlocks...");
                    var parser = new LearningBlockParser();
                    parser.Parse(DBInputData.playSessionDataAsset.text, _databaseObject, _databaseObject.GetLearningBlockTable(), langCode);
                }
                EditorUtility.SetDirty(_databaseObject.learningblockDb);
            }

            if (ImportStages)
            {
                {
                    Debug.Log("Loading Stages...");
                    var parser = new StageParser();
                    parser.Parse(DBInputData.stageDataAsset.text, _databaseObject, _databaseObject.GetStageTable(), langCode);
                }
                EditorUtility.SetDirty(_databaseObject.stageDb);
            }

            if (ImportRewards)
            {
                {
                    Debug.Log("Loading Rewards...");
                    var parser = new RewardParser();
                    parser.Parse(DBInputData.rewardDataAsset.text, _databaseObject, _databaseObject.GetRewardTable(), langCode);
                }
                EditorUtility.SetDirty(_databaseObject.rewardDb);
            }

            AssetDatabase.SaveAssets();
        }
        #endregion

    }
}
#endif
