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
        public static string GetISO3Code(LanguageCode lang)
        {
            switch (lang)
            {
                case LanguageCode.italian:
                    return ("ita");
                case LanguageCode.english:
                    return ("eng");
                case LanguageCode.arabic:
                case LanguageCode.arabic_legacy:
                    return ("ara");
                case LanguageCode.spanish:
                    return ("spa");
                case LanguageCode.french:
                    return ("fra");
                case LanguageCode.persian_farsi:
                    return ("fas");
                case LanguageCode.persian_dari:
                    return ("prs");
                case LanguageCode.pashto:
                    return ("pus");
                case LanguageCode.polish:
                    return ("pol");
                case LanguageCode.ukrainian:
                    return ("ukr");
                case LanguageCode.russian:
                    return ("rus");
                case LanguageCode.NONE:
                case LanguageCode.COUNT:
                    return "";
                default:
                    Debug.LogError("ISO3 code not defined");
                    return "";
            }

        }
    }
}
