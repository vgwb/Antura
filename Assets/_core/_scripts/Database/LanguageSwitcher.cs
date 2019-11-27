using Antura.Core;
using Antura.Database;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Language
{
    public class LanguageSwitcher
    {
        public static LanguageSwitcher I
        {
            get {
                if (AppManager.I == null) return null;
                return AppManager.I.LanguageSwitcher;
            }
        }

        public class LanguageData
        {
            public LangConfig config;
            //public DatabaseManager dbManager;
            public ILanguageHelper helper;
        }

        private Dictionary<LanguageUse, LanguageCode> useMapping;
        private Dictionary<LanguageCode, LanguageData> loadedLanguageData;

        public LanguageSwitcher()
        {
            useMapping = new Dictionary<LanguageUse, LanguageCode>();
            loadedLanguageData = new Dictionary<LanguageCode, LanguageData>();

            LoadLanguage(LanguageUse.Learning, EditionConfig.I.LearningLanguage);
            ReloadNativeLanguage();
            LoadLanguage(LanguageUse.Subtitle, EditionConfig.I.SubtitlesLanguage);
        }

        public void ReloadNativeLanguage()
        {
            LoadLanguage(LanguageUse.Native, EditionConfig.I.NativeLanguage);
        }

        private void LoadLanguage(LanguageUse use, LanguageCode language)
        {
            useMapping[use] = language;
            LoadLanguageData(language);
        }

        void LoadLanguageData(LanguageCode language)
        {
            if (loadedLanguageData.ContainsKey(language)) return;
            var languageData = new LanguageData();
            //languageData.dbManager = new DatabaseManager(false, language);
            languageData.config = Resources.Load<LangConfig>(language + "/" + "LangConfig");
            languageData.helper = Resources.Load<AbstractLanguageHelper>(language + "/" + "LanguageHelper");
            loadedLanguageData[language] = languageData;
        }

        public ILanguageHelper GetHelper(LanguageUse use)
        {
            return loadedLanguageData[useMapping[use]].helper;
        }

        /*public DatabaseManager GetDBManager(LanguageUse use)
        {
            return loadedLanguageData[useMapping[use]].dbManager;
        }*/

        public LangConfig GetLangConfig(LanguageUse use)
        {
            return loadedLanguageData[useMapping[use]].config;
        }
        
        public LangConfig GetLangConfig(LanguageCode code)
        {
            LoadLanguageData(code);
            return loadedLanguageData[code].config;
        }

        #region Shortcuts

        public bool IsLearningLanguageRTL()
        {
            return GetLangConfig(LanguageUse.Learning).IsRightToLeft();
        }

        public static bool LearningRTL => LearningConfig.IsRightToLeft();
        public static LangConfig LearningConfig => I.GetLangConfig(LanguageUse.Learning);
        public static ILanguageHelper LearningHelper => I.GetHelper(LanguageUse.Learning);

        #endregion

    }
}