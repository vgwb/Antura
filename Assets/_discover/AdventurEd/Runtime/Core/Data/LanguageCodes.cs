using System.Collections.Generic;

namespace AdventurEd
{
    public static class LanguageCodes
    {
        // ISO 639-1 codes mapped to display names
        public static readonly Dictionary<string, string> Codes = new()
        {
            { "ar", "Arabic" },
            { "de", "German" },
            { "en", "English" },
            { "es", "Spanish" },
            { "fa", "Persian (Dari)" },
            { "fr", "French" },
            { "hu", "Hungarian" },
            { "it", "Italian" },
            { "pl", "Polish" },
            { "ro", "Romanian" },
            { "ru", "Russian" },
            { "uk", "Ukrainian" },
        };

        public static string GetName(string code) =>
            Codes.TryGetValue(code, out var name) ? name : code;
    }
}
