using Antura.Core;
using Antura.Utilities;
using System.Collections;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace Antura.Discover
{
    public class LocalizationSystem : SingletonMonoBehaviour<LocalizationSystem>
    {
        private Locale currentLearningLocale;
        private string currentLearningIso2;

        /// <summary>
        /// Initialize the localization system.
        /// as early as possible.
        /// </summary>
        /// <returns></returns>
        IEnumerator Start()
        {
            yield return LocalizationSettings.InitializationOperation;
        }

        public void SetupWithLearningLocale(string iso2)
        {
            currentLearningIso2 = iso2;
            currentLearningLocale = LocalizationSettings.AvailableLocales.GetLocale(iso2);
        }

        public string GetLocalizedString(string id, string table = "Common", bool useLearning = false)
        {
            var localizedString = new LocalizedString(table, id);
            return GetLocalizedStringByLangType(localizedString, useLearning);
        }

        public string GetLocalizedStringByLangType(LocalizedString localizedString, bool useLearning = false)
        {
            if (useLearning)
            {
                localizedString.LocaleOverride = GetLearningLocale();
            }
            else
            {
                localizedString.LocaleOverride = null;
            }
            return localizedString.GetLocalizedString();
        }

        public string GetLocalizedString(LocalizedString localizedString, bool respectClassroomMode = false)
        {
            if (respectClassroomMode && AppManager.I.AppSettings.isClassroomMode)
            {
                localizedString.LocaleOverride = GetLearningLocale();
            }
            else
            {
                localizedString.LocaleOverride = null;
            }
            return localizedString.GetLocalizedString();
        }

        #region utilities

        public Locale GetLearningLocale()
        {
            if (AppManager.I.ContentEdition.LearningLanguageConfig.Iso2 != currentLearningIso2)
            {
                SetupWithLearningLocale(AppManager.I.ContentEdition.LearningLanguageConfig.Iso2);
            }

            return currentLearningLocale;
        }

        public static Locale GetLocaleFromCode(string languageCode)
        {
            // e.g. "en", "en-US", "it", "ar"
            var locale = LocalizationSettings.AvailableLocales.GetLocale(languageCode);
            return locale;
        }

        #endregion

    }
}
