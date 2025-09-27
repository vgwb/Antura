using Antura.Core;
using Antura.Audio;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Debug = UnityEngine.Debug;
using UnityEngine.Localization.Settings;

namespace Antura.Language
{
    public class LanguageManager
    {
        public static LanguageManager I
        {
            get
            {
                if (AppManager.I == null)
                    return null;
                return AppManager.I.LanguageManager;
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

        public LanguageManager()
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

        private IEnumerator LoadLanguage(LanguageUse use, LanguageCode language)
        {
            useMapping[use] = language;
            yield return LoadLanguageData(language);
        }

        public IEnumerator ReloadNativeLanguage()
        {
            Debug.Log("Reloading Native Language: " + AppManager.I.AppSettings.NativeLanguage);

            yield return LoadLanguage(LanguageUse.Native, AppManager.I.AppSettings.NativeLanguage);
            var iso2 = LanguageUtilities.GetIso2Direct(AppManager.I.AppSettings.NativeLanguage);
            yield return SetLocalizationLanguage(iso2);
        }

        public IEnumerator LoadAllLanguageData()
        {
            var languagesToLoad = new HashSet<LanguageCode>();

            // We also need to load data for all languages, as they are needed for the selection menu
            foreach (var nativeLanguage in AppManager.I.AppEdition.SupportedNativeLanguages)
            {
                languagesToLoad.Add(nativeLanguage);
            }

            foreach (var contentEdition in AppManager.I.AppEdition.ContentConfigs)
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

        private IEnumerator SetLocalizationLanguage(string iso2Code)
        {
            // Only run if Localization has already been initialized elsewhere;
            // don't trigger or wait here.
            var initOp = LocalizationSettings.InitializationOperation;
            if (!initOp.IsDone)
            {
                yield break;
            }

            // Get all available locales
            var locales = LocalizationSettings.AvailableLocales?.Locales;
            if (locales == null || locales.Count == 0)
                yield break;

            // Try exact code match (e.g., en, fr, it, pl, en-GB)
            var targetLocale = locales.FirstOrDefault(locale =>
                string.Equals(locale.Identifier.Code, iso2Code, System.StringComparison.OrdinalIgnoreCase));

            if (targetLocale == null)
            {
                Debug.LogWarning($"[Language] No Unity Locale matches ISO2 '{iso2Code}'. Keeping current locale.");
                yield break;
            }

            if (LocalizationSettings.SelectedLocale != targetLocale)
            {
                LocalizationSettings.SelectedLocale = targetLocale;
                Debug.Log($"[Language] Unity Localization switched to: {targetLocale.Identifier.Code}");
            }
            yield break;
        }

        IEnumerator LoadLanguageData(LanguageCode language)
        {
            if (loadedLanguageData.ContainsKey(language))
                yield break;
            var languageData = new LanguageData();

            // var stopwatch = new Stopwatch();
            // stopwatch.Start();
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
            // stopwatch.Stop();
            // Debug.LogError(language + "LangConfig: " + stopwatch.ElapsedMilliseconds.ToString());
        }

        public IEnumerator PreloadLocalizedDataCO()
        {
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
