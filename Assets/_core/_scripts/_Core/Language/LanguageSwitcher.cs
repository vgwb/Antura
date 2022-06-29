using Antura.Core;
using Antura.Audio;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Debug = UnityEngine.Debug;

namespace Antura.Language
{
    public class LanguageSwitcher
    {
        public static LanguageSwitcher I
        {
            get
            {
                if (AppManager.I == null)
                    return null;
                return AppManager.I.LanguageSwitcher;
            }
        }

        public class LanguageData
        {
            public LangConfig config;
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

        public IEnumerator LoadEditionData()
        {
            yield return LoadLanguage(LanguageUse.Learning, AppManager.I.ContentEdition.LearningLanguage);
            yield return ReloadNativeLanguage();
            yield return LoadLanguage(LanguageUse.Help, AppManager.I.ContentEdition.HelpLanguage);
        }

        public IEnumerator LoadAllLanguageData()
        {
            var languagesToLoad = new HashSet<LanguageCode>();

            // We also need to load data for all languages, as they are needed for the selection menu
            foreach (var nativeLanguage in AppManager.I.AppEdition.SupportedNativeLanguages)
            {
                languagesToLoad.Add(nativeLanguage);
            }

            foreach (var contentEdition in AppManager.I.AppEdition.ContentEditions)
            {
                languagesToLoad.Add(contentEdition.LearningLanguage);
                languagesToLoad.Add(contentEdition.HelpLanguage);
                foreach (LanguageCode nativeLanguage in contentEdition.OverridenNativeLanguages)
                {
                    languagesToLoad.Add(nativeLanguage);
                }
            }

            foreach (LanguageCode languageCode in languagesToLoad)
            {
                yield return LoadLanguageData(languageCode);
            }
        }

        public IEnumerator ReloadNativeLanguage()
        {
            yield return LoadLanguage(LanguageUse.Native, AppManager.I.AppSettings.NativeLanguage);
        }

        private IEnumerator LoadLanguage(LanguageUse use, LanguageCode language)
        {
            useMapping[use] = language;
            yield return LoadLanguageData(language);
        }

        IEnumerator LoadLanguageData(LanguageCode language)
        {
            if (loadedLanguageData.ContainsKey(language))
                yield break;
            var languageData = new LanguageData();

            //var stopwatch = new Stopwatch();
            //stopwatch.Start();
            yield return AssetLoader.Load<LangConfig>($"languages/{language}/LangConfig_{language}", r => languageData.config = r, DebugConfig.I.AddressablesBlockingLoad, fromResources: true);
            if (languageData.config == null)
            {
                throw new FileNotFoundException($"Could not find the LangConfig file for {language} in the language resources! Did you setup it correctly?");
            }

            languageData.helper = languageData.config.LanguageHelper;
            if (languageData.helper == null)
            {
                throw new FileNotFoundException($"Could not find the LanguageHelper file in the language resources! Did you setup the {language} language correctly?");
            }
            loadedLanguageData[language] = languageData;

            languageData.diacriticsComboData = languageData.config.DiacriticsComboData;
            //stopwatch.Stop();
            //Debug.LogError(language + "LangConfig: " + stopwatch.ElapsedMilliseconds.ToString());
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

        public ILanguageHelper GetHelper(LanguageCode code)
        {
            return loadedLanguageData[code].helper;
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
