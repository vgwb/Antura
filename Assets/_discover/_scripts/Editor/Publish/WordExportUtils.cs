#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace Antura.Discover.Editor
{
    public static class WordExportUtils
    {
        public static string BuildWordsIndexMarkdown(Locale locale)
        {
            var sb = new StringBuilder();
            sb.AppendLine("---");
            sb.AppendLine("title: Words");
            sb.AppendLine("hide:");
            sb.AppendLine("---\n");
            sb.AppendLine("# Words\n");

            var guids = AssetDatabase.FindAssets("t:WordData");
            var words = new List<WordData>();
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var w = AssetDatabase.LoadAssetAtPath<WordData>(path);
                if (w != null)
                    words.Add(w);
            }

            // Only include active words
            words = words.Where(w => w != null && w.Active).ToList();

            if (words.Count == 0)
            {
                sb.AppendLine("(No words found)");
                return sb.ToString();
            }

            // Resolve target locales
            var enLoc = FindLocaleByCode("en");
            var frLoc = FindLocaleByCode("fr");
            var plLoc = FindLocaleByCode("pl");
            var itLoc = FindLocaleByCode("it");

            // Group by category and render a table per category
            var groups = words
                .GroupBy(w => w.Category)
                .OrderBy(g => g.Key.ToString(), StringComparer.OrdinalIgnoreCase);

            foreach (var group in groups)
            {
                var category = group.Key;
                var list = group
                    .OrderBy(x => string.IsNullOrEmpty(x.TextEn) ? x.Id : x.TextEn, StringComparer.OrdinalIgnoreCase)
                    .ToList();
                if (list.Count == 0)
                    continue;

                sb.AppendLine($"## {category}");
                sb.AppendLine();
                sb.AppendLine("| id | en | fr | pl | it |");
                sb.AppendLine("|---|---|---|---|---|");

                foreach (var w in list)
                {
                    string fallback = !string.IsNullOrEmpty(w.TextEn) ? w.TextEn : (!string.IsNullOrEmpty(w.name) ? w.name : w.Id);

                    string en = LocalizeWord(w, enLoc, fallback);
                    string fr = LocalizeWord(w, frLoc, fallback);
                    string pl = LocalizeWord(w, plLoc, fallback);
                    string it = LocalizeWord(w, itLoc, fallback);

                    sb.AppendLine($"| {EscapeMd(w.Id)} | {EscapeMd(en)} | {EscapeMd(fr)} | {EscapeMd(pl)} | {EscapeMd(it)} |");
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }

        private static string LocalizeWord(WordData w, Locale locale, string fallback)
        {
            string result = fallback;
            try
            {
                PublishUtils.WithLocale(locale, () =>
                {
                    if (w.TextLocalized != null)
                        result = PublishUtils.SafeLocalized(w.TextLocalized, fallback);
                    else
                        result = fallback;
                });
            }
            catch { result = fallback; }
            return result;
        }

        private static string EscapeMd(string s)
        {
            if (string.IsNullOrEmpty(s))
                return "";
            // Escape pipes to avoid breaking tables
            return s.Replace("|", "\\|");
        }

        private static Locale FindLocaleByCode(string code)
        {
            try
            {
                var locales = LocalizationSettings.AvailableLocales?.Locales;
                if (locales == null || locales.Count == 0)
                    return null;
                var exact = locales.FirstOrDefault(l => string.Equals(l?.Identifier.Code, code, StringComparison.OrdinalIgnoreCase));
                if (exact != null)
                    return exact;
                return locales.FirstOrDefault(l => l?.Identifier.Code != null && l.Identifier.Code.StartsWith(code, StringComparison.OrdinalIgnoreCase));
            }
            catch { return null; }
        }
    }
}
#endif
