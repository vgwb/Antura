using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using Antura.Core;
using Antura.Language;

namespace Antura.Database
{
    /// <summary>
    /// Main entry point for learning data access.
    /// Holds all static input data on learning.
    /// </summary>
    public class DatabaseObject
    {
        public StageDatabase stageDb;
        public PlaySessionDatabase playsessionDb;
        public LearningBlockDatabase learningblockDb;
        public MiniGameDatabase minigameDb;
        public LetterDatabase letterDb;
        public WordDatabase wordDb;
        public PhraseDatabase phraseDb;
        public LocalizationDatabase localizationDb;
        public RewardDatabase rewardDb;

        #region Creation

        public static DatabaseObject LoadDB(ContentEditionConfig fromEdition, LanguageCode language, string staticDbNameToLoad)
        {
            var db = new DatabaseObject();
            if (fromEdition != null)
            {
                db.LoadTablesFromEdition(fromEdition);
            }
            else
            {
                db.LoadTablesFromPath(language, staticDbNameToLoad);
            }
            return db;
        }

        public bool HasTables()
        {
            if (stageDb == null)
            {
                return false;
            }
            if (playsessionDb == null)
            {
                return false;
            }
            if (learningblockDb == null)
            {
                return false;
            }
            if (minigameDb == null)
            {
                return false;
            }
            if (letterDb == null)
            {
                return false;
            }
            if (wordDb == null)
            {
                return false;
            }
            if (phraseDb == null)
            {
                return false;
            }
            if (localizationDb == null)
            {
                return false;
            }
            if (rewardDb == null)
            {
                return false;
            }
            return true;
        }

        public void LoadTablesFromEdition(ContentEditionConfig edition)
        {
            Debug.Log("LoadTablesFromEdition " + edition.ContentID.ToString());
            stageDb = edition.StageDB;
            learningblockDb = edition.LearningBlockDB;
            playsessionDb = edition.PlaySessionDB;
            minigameDb = edition.MiniGameDB;
            localizationDb = edition.LocalizationDB;
            letterDb = edition.LetterDB;
            wordDb = edition.WordDB;
            phraseDb = edition.PhraseDB;
            rewardDb = edition.RewardDB;
        }

        // TODO: Deprecate this
        public void LoadTablesFromPath(LanguageCode language, string dbName)
        {
            var path = language + "/" + dbName + "/" + dbName + "_";

            Debug.LogWarning("Loading database at path " + path);

            stageDb = Resources.Load<StageDatabase>(path + "Stage");
            if (!stageDb)
            {
                // Debug.LogError("Could not load StageDatabase db");
            }

            playsessionDb = Resources.Load<PlaySessionDatabase>(path + "PlaySession");
            if (!playsessionDb)
            {
                // Debug.LogError("Could not load PlaySessionDatabase db");
            }

            learningblockDb = Resources.Load<LearningBlockDatabase>(path + "LearningBlock");
            if (!learningblockDb)
            {
                // Debug.LogError("Could not load LearningBlockDatabase db");
            }

            minigameDb = Resources.Load<MiniGameDatabase>(path + "MiniGame");
            if (!minigameDb)
            {
                // Debug.LogError("Could not load MiniGameDatabase db");
            }

            letterDb = Resources.Load<LetterDatabase>(path + "Letter");
            if (!letterDb)
            {
                // Debug.LogError("Could not load LetterDatabase db");
            }

            wordDb = Resources.Load<WordDatabase>(path + "Word");
            if (!wordDb)
            {
                // Debug.LogError("Could not load WordDatabase db");
            }

            phraseDb = Resources.Load<PhraseDatabase>(path + "Phrase");
            if (!phraseDb)
            {
                // Debug.LogError("Could not load PhraseDatabase db");
            }

            localizationDb = Resources.Load<LocalizationDatabase>(path + "Localization");
            if (!localizationDb)
            {
                // Debug.LogError("Could not load LocalizationDatabase db");
            }

            rewardDb = Resources.Load<RewardDatabase>(path + "Reward");
            if (!rewardDb)
            {
                // Debug.LogError("Could not load RewardDatabase db");
            }
        }

        #endregion


        #region Access

        public List<T> FindAll<T>(SerializableDataTable<T> table, Predicate<T> predicate) where T : IData
        {
            var allValues = new List<T>(table.GetValuesTyped());
            var filtered = allValues.FindAll(predicate);
            return filtered;
        }

        public IEnumerable<T> FindAllOptimized<T>(SerializableDataTable<T> table, Predicate<T> predicate) where T : IData
        {
            return table.GetValuesTyped().Where(x => predicate(x));
        }


        public List<T> GetAll<T>(SerializableDataTable<T> table) where T : IData
        {
            var allValues = new List<T>(table.GetValuesTyped());
            return allValues;
        }

        public T GetById<T>(SerializableDataTable<T> table, string id) where T : IData
        {
            var value = (T)table.GetValue(id);
            if (value == null)
            {
                Debug.LogWarning("Cannot find id '" + id + "' in table " + table.GetType().Name);
                return default(T);
            }
            return value;
        }

        public bool HasById<T>(SerializableDataTable<T> table, string id) where T : IData
        {
            var value = (T)table.GetValue(id);
            if (value == null)
            {
                return false;
            }
            return true;
        }

        public IEnumerable<List<IData>> GetAllData()
        {
            foreach (var table in GetAllTables())
            {
                yield return table.GetList();
            }
        }

        public LetterTable GetLetterTable()
        {
            return this.letterDb.table;
        }

        public WordTable GetWordTable()
        {
            return this.wordDb.table;
        }

        public PhraseTable GetPhraseTable()
        {
            return this.phraseDb.table;
        }

        public MiniGameTable GetMiniGameTable()
        {
            return this.minigameDb.table;
        }

        public StageTable GetStageTable()
        {
            return this.stageDb.table;
        }

        public PlaySessionTable GetPlaySessionTable()
        {
            return this.playsessionDb.table;
        }

        public LearningBlockTable GetLearningBlockTable()
        {
            return this.learningblockDb.table;
        }

        public RewardTable GetRewardTable()
        {
            return this.rewardDb.table;
        }

        public LocalizationTable GetLocalizationTable()
        {
            return this.localizationDb.table;
        }

        // @note: interface for common use using categories
        public IData GetData(DbTables tables, string id)
        {
            var table = GetTable(tables);
            return table.GetValue(id);
        }

        public IEnumerable<IDataTable> GetAllTables()
        {
            yield return GetTable(DbTables.Letters);
            yield return GetTable(DbTables.Words);
            yield return GetTable(DbTables.Phrases);
            yield return GetTable(DbTables.MiniGames);
            yield return GetTable(DbTables.Stages);
            yield return GetTable(DbTables.PlaySessions);
            yield return GetTable(DbTables.Rewards);
            yield return GetTable(DbTables.Localizations);
        }

        public IDataTable GetTable(DbTables tables)
        {
            IDataTable table = null;
            switch (tables)
            {
                case DbTables.Letters:
                    table = GetLetterTable();
                    break;
                case DbTables.Words:
                    table = GetWordTable();
                    break;
                case DbTables.Phrases:
                    table = GetPhraseTable();
                    break;
                case DbTables.MiniGames:
                    table = GetMiniGameTable();
                    break;
                case DbTables.Stages:
                    table = GetStageTable();
                    break;
                case DbTables.PlaySessions:
                    table = GetPlaySessionTable();
                    break;
                case DbTables.LearningBlocks:
                    table = GetLearningBlockTable();
                    break;
                case DbTables.Rewards:
                    table = GetRewardTable();
                    break;
                case DbTables.Localizations:
                    table = GetLocalizationTable();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("tables", tables, null);
            }
            return table;
        }

        #endregion
    }
}
