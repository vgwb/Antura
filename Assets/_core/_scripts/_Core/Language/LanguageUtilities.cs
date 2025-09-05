using System;
using UnityEngine;

namespace Antura.Language
{
    public static class LanguageUtilities
    {
        /*
        return ISO 639-3 code
        see https://en.wikipedia.org/wiki/List_of_ISO_639-1_codes
        */
        public static string GetISO2Code(LanguageCode lang)
        {
            var langConfig = LanguageManager.I.GetLangConfig(lang);
            return langConfig.Iso2;
        }

        public static string GetIso2Direct(LanguageCode lang)
        {
            switch (lang)
            {
                case LanguageCode.english:
                    return "en";
                case LanguageCode.arabic:
                    return "ar";
                case LanguageCode.french:
                    return "fr";
                case LanguageCode.german:
                    return "de";
                case LanguageCode.italian:
                    return "it";
                case LanguageCode.polish:
                    return "pl";
                case LanguageCode.russian:
                    return "ru";
                case LanguageCode.spanish:
                    return "es";
                case LanguageCode.arabic_legacy:
                    return "ar";
                case LanguageCode.ukrainian:
                    return "uk";
                case LanguageCode.romanian:
                    return "ro";
                case LanguageCode.hungarian:
                    return "hu";
                default:
                    Debug.LogWarning($"GetIso2Direct: No ISO2 code found for language {lang}, returning 'en' as fallback");
                    return "en"; // fallback
            }
        }


    }
}
