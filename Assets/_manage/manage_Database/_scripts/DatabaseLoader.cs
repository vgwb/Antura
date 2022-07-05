#if UNITY_EDITOR
using System.Linq;
using UnityEngine;
using UnityEditor;
using Antura.Core;
using Antura.GoogleSheets;
using Antura.Language;
using DG.DeInspektor.Attributes;
using System;

namespace Antura.Database.Management
{
    /// <summary>
    /// Management class that converts JSON data to static database custom assets.
    /// </summary>
    public class DatabaseLoader : MonoBehaviour
    {
        private DatabaseObject _databaseObject;
        public GoogleSheetRef CommonSheet;

        public bool ImportLocalizations;
        public bool ImportLetters;
        public bool ImportWords;
        public bool ImportPhrases;
        public bool ImportJourney;

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

        #region Loading

        /// <summary>
        /// Load the received data and place it inside the database.
        /// </summary>
        /// <param name="DBInputData">Generated json from Google sheet</param>
        /// <param name="fileName">Json fileName selected to import</param>
        /// <param name="edition">The current loaded version. (Except for Localization, for the rest importTypes must be selected the same that you're trying to load to the db)</param>
        /// <param name="importType">Indicates if the coming json to load into de db is Vocabulary, PlaySession or Localization</param>
        /// <param name="dataType">Not mandatory: this param indicates the sheet name from which the json was generated (only necessary for Vocabulary)</param>
        public void DirectLoadData(string DBInputData, string fileName, ContentEditionConfig edition, ContentType importType, string dataType = null, string currentLangCode = null)
        {
            if(currentLangCode != null && Enum.IsDefined(typeof(LanguageCode), currentLangCode.ToLower()))
                langCode = (LanguageCode)Enum.Parse(typeof(LanguageCode), currentLangCode.ToLower());

            this._databaseObject = DatabaseObject.LoadDB(edition, langCode, DatabaseManager.STATIC_DATABASE_NAME);

            if (importType == ContentType.Vocabulary && ImportLetters && dataType.Equals("LetterData"))
            {
                {
                    Debug.Log("Loading Letters from temp JSON: " + fileName + " for lang: " + langCode);
                    var parser = new LetterParser();
                    parser.Parse(DBInputData, _databaseObject, _databaseObject.GetLetterTable(), langCode);
                }
                EditorUtility.SetDirty(_databaseObject.letterDb);
            }

            if (importType == ContentType.Vocabulary && ImportWords && dataType.Equals("WordData"))
            {
                {
                    // @note: depends on Letter
                    Debug.Log("Loading Words...");
                    var parser = new WordParser();
                    parser.Parse(DBInputData, _databaseObject, _databaseObject.GetWordTable(), langCode);
                }
                EditorUtility.SetDirty(_databaseObject.wordDb);
            }

            if (importType == ContentType.Vocabulary && ImportPhrases && dataType.Equals("PhraseData"))
            {
                {
                    // @note: depends on Word
                    Debug.Log("Loading Phrases...");
                    var parser = new PhraseParser();
                    parser.Parse(DBInputData, _databaseObject, _databaseObject.GetPhraseTable(), langCode);
                }
                EditorUtility.SetDirty(_databaseObject.phraseDb);
            }

            if (importType == ContentType.Localization && ImportLocalizations)
            {
                {
                    Debug.Log("Loading Localization...");
                    var parser = new LocalizationParser();
                    parser.Parse(DBInputData, _databaseObject, _databaseObject.GetLocalizationTable(), langCode);
                }
                EditorUtility.SetDirty(_databaseObject.localizationDb);
            }

            if (importType == ContentType.Journey && ImportJourney)
            {
                {
                    Debug.Log("Loading PlaySessions...");
                    var parser = new PlaySessionParser();
                    parser.Parse(DBInputData, _databaseObject, _databaseObject.GetPlaySessionTable(), langCode);
                }
                EditorUtility.SetDirty(_databaseObject.playsessionDb);
            }

            if (importType == ContentType.Journey && ImportJourney)
            {
                {
                    // @note: depends on Letter, Word, Phrase, PlaySession
                    Debug.Log("Loading LearningBlocks...");
                    var parser = new LearningBlockParser();
                    parser.Parse(DBInputData, _databaseObject, _databaseObject.GetLearningBlockTable(), langCode);
                }
                EditorUtility.SetDirty(_databaseObject.learningblockDb);
            }

            AssetDatabase.SaveAssets();
            Debug.Log("Import complete.");
        }
        #endregion

#if UNITY_EDITOR

        [DeMethodButton("Import from Google Sheets")]
        public void Import()
        {
            if (ImportLocalizations)
            {
                Debug.Log("Importing sheet ref: " + CommonSheet.name);
                CommonSheet.ImportJSON();
            }

            var ech = GetComponent<EditorContentHolder>();
            if (ImportJourney)
            {
                var sheetRef = ech.InputContent.GoogleSheets != null ? ech.InputContent.GoogleSheets.FirstOrDefault(x => x != null && x.ContentType == ContentType.Journey) : null;
                if (sheetRef == null)
                {
                    Debug.LogError("Could not find any sheet ref to import Journey data for this Content Edition");
                }
                else
                {
                    Debug.Log("Importing sheet ref: " + sheetRef.name);
                    sheetRef.ImportJSON();
                }
            }

            if (ImportLetters || ImportWords || ImportPhrases)
            {
                var sheetRef = ech.InputContent.GoogleSheets != null ? ech.InputContent.GoogleSheets.FirstOrDefault(x => x != null && x.ContentType == ContentType.Vocabulary) : null;
                if (sheetRef == null)
                {
                    Debug.LogError("Could not find any sheet ref to import Vocabulary data for this Content Edition");
                }
                else
                {
                    Debug.Log("Importing sheet ref: " + sheetRef.name);
                    sheetRef.ImportJSON();
                }
            }
        }

#endif
    }
}
#endif
