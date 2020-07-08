using Antura.Core;
using Antura.Database;
using System.Collections.Generic;
using System.IO;
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

            LoadLanguage(LanguageUse.Learning, AppManager.I.SpecificEdition.LearningLanguage);
            ReloadNativeLanguage();
            LoadLanguage(LanguageUse.Subtitle, AppManager.I.SpecificEdition.SubtitlesLanguage);
        }

        public void ReloadNativeLanguage()
        {
            LoadLanguage(LanguageUse.Native, AppManager.I.SpecificEdition.NativeLanguage);
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

            languageData.config = Resources.Load<LangConfig>($"{language}/LangConfig");
            if (languageData.config == null)
            {
                Debug.LogError($"Could not find the LangConfig file for {language} in the language resources! Did you setup it correctly?");
                throw new FileNotFoundException();
            }

            languageData.helper = Resources.Load<AbstractLanguageHelper>($"{language}/LanguageHelper");
            if (languageData.config == null)
            {
                Debug.LogError($"Could not find the LanguageHelper file in the language resources! Did you setup the {language} language correctly?");
                throw new FileNotFoundException();
            }
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