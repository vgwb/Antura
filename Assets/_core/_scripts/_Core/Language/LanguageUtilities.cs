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
            var langConfig = LanguageSwitcher.I.GetLangConfig(lang);
            return langConfig.Iso3;
        }
    }
}
