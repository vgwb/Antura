using System.Collections;
using Antura.Core;
using System.Collections.Generic;
using System.IO;
using Antura.Audio;

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
            public DiacriticsComboData diacriticsComboData;
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

            yield return AssetLoader.Load<LangConfig>($"{language}/LangConfig", r => languageData.config = r, AppManager.BlockingLoad);
            if (languageData.config == null)
            {
                throw new FileNotFoundException($"Could not find the LangConfig file for {language} in the language resources! Did you setup it correctly?");
            }

            yield return AssetLoader.Load<AbstractLanguageHelper>($"{language}/LanguageHelper", r => languageData.helper = r, AppManager.BlockingLoad);
            if (languageData.helper == null)
            {
                throw new FileNotFoundException($"Could not find the LanguageHelper file in the language resources! Did you setup the {language} language correctly?");
            }
            loadedLanguageData[language] = languageData;

            yield return AssetLoader.Load<DiacriticsComboData>($"{language}/DiacriticsComboData", r => languageData.diacriticsComboData = r, AppManager.BlockingLoad);
            /*if (languageData.diacriticsComboData == null)
            {
                throw new FileNotFoundException($"Could not find the DiacriticsComboData file for {language} in the language resources! Did you setup it correctly?");
            }*/
        }

        public IEnumerator PreloadLocalizedDataCO()
        {
            yield return AudioManager.I.PreloadDataCO();
            yield return AppManager.I.AssetManager.PreloadDataCO();
        }

        public ILanguageHelper GetHelper(LanguageUse use)
        {
            return loadedLanguageData[useMapping[use]].helper;
        }

        /*public DatabaseManager GetDBManager(LanguageUse use)
        {
            return loadedLanguageData[useMapping[use]].dbManager;
        }*/

        public DiacriticsComboData GetDiacriticsComboData(LanguageUse use)
        {
            return loadedLanguageData[useMapping[use]].diacriticsComboData;
        }

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