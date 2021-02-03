using System.Collections;
using Antura.Core;
using Antura.Database;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Antura
{
}

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
        }

        public IEnumerator LoadData()
        {
            yield return LoadLanguage(LanguageUse.Learning, AppManager.I.SpecificEdition.LearningLanguage);
            yield return ReloadNativeLanguage();
            yield return LoadLanguage(LanguageUse.Help, AppManager.I.SpecificEdition.HelpLanguage);
        }

        public IEnumerator ReloadNativeLanguage()
        {
            yield return LoadLanguage(LanguageUse.Native, AppManager.I.SpecificEdition.NativeLanguage);
        }

        private IEnumerator LoadLanguage(LanguageUse use, LanguageCode language)
        {
            useMapping[use] = language;
            yield return LoadLanguageData(language);
        }


        IEnumerator LoadLanguageData(LanguageCode language)
        {
            if (loadedLanguageData.ContainsKey(language)) yield break;
            var languageData = new LanguageData();

            yield return AssetLoader.Load<LangConfig>($"{language}/LangConfig", r => languageData.config = r);



            languageData.config = Resources.Load<LangConfig>($"{language}/LangConfig");
            if (languageData.config == null)
            {
                throw new FileNotFoundException($"Could not find the LangConfig file for {language} in the language resources! Did you setup it correctly?");
            }

            languageData.helper = Resources.Load<AbstractLanguageHelper>($"{language}/LanguageHelper");
            if (languageData.config == null)
            {
                throw new FileNotFoundException($"Could not find the LanguageHelper file in the language resources! Did you setup the {language} language correctly?");
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