using System;
using Antura.Database;
using Antura.Language;
using Antura.Profile;

namespace Antura.Core
{
    /// <summary>
    /// Static class that helps in localizing strings.
    /// </summary>
    public class LocalizationManager
    {

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
            string prefix = "";
            switch (langCode)
            {
                case LanguageCode.english:
                    prefix = "EN";
                    break;
                case LanguageCode.arabic:
                    prefix = "AR";
                    break;
                case LanguageCode.spanish:
                    prefix = "ES";
                    break;
                case LanguageCode.italian:
                    prefix = "IT";
                    break;
                case LanguageCode.french:
                    prefix = "FR";
                    break;
                case LanguageCode.polish:
                    prefix = "PL";
                    break;
                case LanguageCode.ukrainian:
                    prefix = "UK";
                    break;
                case LanguageCode.russian:
                    prefix = "RU";
                    break;
                case LanguageCode.romanian:
                    prefix = "RO";
                    break;
                case LanguageCode.hungarian:
                    prefix = "HU";
                    break;
                case LanguageCode.german:
                    prefix = "DE";
                    break;
            }
            return prefix + "_" + node_id;
        }
    }
}
