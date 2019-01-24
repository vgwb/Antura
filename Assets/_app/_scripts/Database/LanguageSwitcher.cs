using Antura.Core;
using System.Collections.Generic;
using Antura.Database;
using UnityEngine;

namespace Antura.Language
{
    public class LanguageSwitcher
    {
        public static LanguageSwitcher I
        { get
            {
                if (AppManager.I == null) return null;
                return AppManager.I.LanguageSwitcher;
            }
        }

        public class LanguageData
        {
            public LangConfig config;
            public DatabaseManager dbManager;
            public ILanguageHelper helper;
        }

        private Dictionary<LanguageUse, LanguageCode> useMapping;
        private Dictionary<LanguageCode, LanguageData> loadedLanguageData;

        public LanguageSwitcher()
        {
            useMapping = new Dictionary<LanguageUse, LanguageCode>();
            loadedLanguageData = new Dictionary<LanguageCode, LanguageData>();

            LoadLanguage(LanguageUse.Learning, SAppConfig.I.LearningLanguage);
            LoadLanguage(LanguageUse.Instructions, SAppConfig.I.InstructionsLanguage);
            LoadLanguage(LanguageUse.Tutor, SAppConfig.I.TutorLanguage);
        }

        private void LoadLanguage(LanguageUse use, LanguageCode language)
        {
            useMapping[use] = language;
            if (loadedLanguageData.ContainsKey(language))
            {
                // Nothing to do
                return;
            }
            var languageData = new LanguageData();
            languageData.dbManager = new DatabaseManager(language);
            languageData.config = Resources.Load<LangConfig>(language + "/" + "LangConfig");
            languageData.helper = Resources.Load<AbstractLanguageHelper>(language + "/" + "LanguageHelper");
            loadedLanguageData[language] = languageData;
        }

        public ILanguageHelper GetHelper(LanguageUse use)
        {
            return loadedLanguageData[useMapping[use]].helper;
        }

        public DatabaseManager GetManager(LanguageUse use)
        {
            return loadedLanguageData[useMapping[use]].dbManager;
        }

        public LangConfig GetLangConfig(LanguageUse use)
        {
            return loadedLanguageData[useMapping[use]].config;
        }

    }
}