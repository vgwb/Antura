using System;
using Antura.Database;
using Antura.Language;
using Antura.Profile;
using UnityEngine.Localization;

namespace Antura.Core
{
    /// <summary>
    /// Static class that helps in localizing strings.
    /// </summary>
    public class LocalizationManager
    {

        public static string GetNewLocalized(string id)
        {
            var str = new LocalizedString("Common", id);
            return str.GetLocalizedString();
        }

        public static PlayerGender CurrentPlayerGender
        {
            get
            {
                if (AppManager.I.Player == null)
                {
                    return AppManager.I.PlayerProfileManager.TemporaryPlayerGender;
                }
                return AppManager.I.Player.Gender;
            }
        }

        public static string GetNative(LocalizationDataId id)
        {
            return GetLocalizationData(id).GetNativeText();
        }

        public static string GetLearning(LocalizationDataId id)
        {
            return GetLocalizationData(id).GetLearningText(CurrentPlayerGender);
        }

        public static string GetHelp(LocalizationDataId id)
        {
            return GetLocalizationData(id).HelpText;
        }

        public static string GetLearning(string id)
        {
            return GetLocalizationData(id).GetLearningText(CurrentPlayerGender);
        }
        public static string GetNative(string id)
        {
            return GetLocalizationData(id).GetNativeText();
        }

        public static string GetLocalizedAudioFileName(string id)
        {
            return GetLocalizationData(id).GetLocalizedAudioFileName(CurrentPlayerGender);
        }

        public static string GetLocalizedAudioFileName(string id, PlayerGender forcedGender)
        {
            return GetLocalizationData(id).GetLocalizedAudioFileName(forcedGender);
        }

        public static LocalizationData GetLocalizationData(LocalizationDataId id)
        {
            return AppManager.I.DB.GetLocalizationDataById(id.ToString());
        }

        public static LocalizationData GetLocalizationData(string id)
        {
            return AppManager.I.DB.GetLocalizationDataById(id);
        }

        public static LocalizationData GetWordCategoryData(WordDataCategory cat)
        {
            var locId = (LocalizationDataId)Enum.Parse(typeof(LocalizationDataId), $"UI_WordCat_{cat}");
            return GetLocalizationData(locId);
        }

        public static LocalizationData GetPhraseCategoryData(PhraseDataCategory cat)
        {
            var locId = (LocalizationDataId)Enum.Parse(typeof(LocalizationDataId), $"UI_Phrases_{cat}");
            return GetLocalizationData(locId);
        }

        public static string PrefixHomerNodeWithLangCode(string node_id, LanguageCode langCode)
        {
            string prefix = IsoLangFromLangCode(langCode);
            return prefix + "_" + node_id;
        }

        public static string IsoLangFromLangCode(LanguageCode langCode)
        {
            string iso2lang = "";
            switch (langCode)
            {
                case LanguageCode.english:
                    iso2lang = "EN";
                    break;
                case LanguageCode.arabic:
                    iso2lang = "AR";
                    break;
                case LanguageCode.spanish:
                    iso2lang = "ES";
                    break;
                case LanguageCode.italian:
                    iso2lang = "IT";
                    break;
                case LanguageCode.french:
                    iso2lang = "FR";
                    break;
                case LanguageCode.polish:
                    iso2lang = "PL";
                    break;
                case LanguageCode.ukrainian:
                    iso2lang = "UK";
                    break;
                case LanguageCode.russian:
                    iso2lang = "RU";
                    break;
                case LanguageCode.romanian:
                    iso2lang = "RO";
                    break;
                case LanguageCode.hungarian:
                    iso2lang = "HU";
                    break;
                case LanguageCode.german:
                    iso2lang = "DE";
                    break;
            }
            return iso2lang;
        }
    }
}
