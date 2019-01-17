using Antura.Core;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Database
{
    public class LanguageSwitcher
    {
        private Dictionary<LanguageUse, LanguageCode> useMapping;
        private Dictionary<LanguageCode, DatabaseManager> loadedManagers;

        public DatabaseManager GetManager(LanguageUse use)
        {
            return loadedManagers[useMapping[use]];
        }

        public LanguageSwitcher()
        {
            useMapping = new Dictionary<LanguageUse, LanguageCode>();
            loadedManagers = new Dictionary<LanguageCode, DatabaseManager>();
            loadedConfigs = new Dictionary<LanguageCode, LangConfig>();

            LoadLanguage(LanguageUse.Learning, SAppConfig.I.LearningLanguage);
            LoadLanguage(LanguageUse.Instructions, SAppConfig.I.InstructionsLanguage);
            LoadLanguage(LanguageUse.Tutor, SAppConfig.I.TutorLanguage);
        }

        private void LoadLanguage(LanguageUse use, LanguageCode language)
        {
            useMapping[use] = language;
            if (loadedManagers.ContainsKey(language))
            {
                // Nothing to do
                return;
            }
            var db = new DatabaseManager(language);
            loadedManagers[language] = db;
            loadedConfigs[language] = Resources.Load<LangConfig>(language +"/" + "LangConfig");
        }

        #region Language Configs

        private Dictionary<LanguageCode, LangConfig> loadedConfigs;

        public LangConfig GetLangConfig(LanguageUse use)
        {
            return loadedConfigs[useMapping[use]];
        }

        #endregion
    }
}