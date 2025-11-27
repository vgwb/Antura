using Antura.Core;
using Antura.Language;
using Antura.Utilities;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace Antura.Discover
{
    public class LocalizationSystem : SingletonMonoBehaviour<LocalizationSystem>
    {
        private Locale currentLearningLocale;
        private Locale currentNativeLocale;

        private string currentLearningIso2;
        private LanguageCode currentNativeLanguageCode;

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
                localizedString.LocaleOverride = GetNativeLocale();
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
                localizedString.LocaleOverride = GetNativeLocale();
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

        public Locale GetNativeLocale()
        {
            var desiredLanguage = LanguageCode.italian;
            if (currentNativeLocale == null || desiredLanguage != currentNativeLanguageCode)
            {
                currentNativeLanguageCode = desiredLanguage;
                var desiredIso2 = LanguageUtilities.GetIso2Direct(desiredLanguage);
                currentNativeLocale = ResolveLocale(desiredIso2) ?? LocalizationSettings.SelectedLocale;
                if (currentNativeLocale == null)
                {
                    Debug.LogWarning($"[LocalizationSystem] Unable to resolve native locale for {desiredLanguage} ({desiredIso2}).");
                }
            }

            return currentNativeLocale;
        }

        public static Locale GetLocaleFromCode(string languageCode)
        {
            // e.g. "en", "en-US", "it", "ar"
            var locale = LocalizationSettings.AvailableLocales.GetLocale(languageCode);
            return locale ?? ResolveLocale(languageCode);
        }

        #endregion

        private static Locale ResolveLocale(string isoCode)
        {
            if (string.IsNullOrEmpty(isoCode))
                return null;

            var locales = LocalizationSettings.AvailableLocales?.Locales;
            if (locales == null || locales.Count == 0)
                return null;

            return locales.FirstOrDefault(locale =>
                string.Equals(locale.Identifier.Code, isoCode, System.StringComparison.OrdinalIgnoreCase) ||
                (!string.IsNullOrEmpty(locale.Identifier.Code) && locale.Identifier.Code.StartsWith(isoCode + "-", System.StringComparison.OrdinalIgnoreCase)));
        }

    }
}
