#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEditor;

namespace Antura.Discover.Editor
{
    public static class PublishUtils
    {
        public static string GetQuestCode(QuestData q)
        {
            string code = string.IsNullOrEmpty(q.Id) ? q.name : q.Id;
            foreach (var ch in System.IO.Path.GetInvalidFileNameChars())
                code = code.Replace(ch, '-');
            return code;
        }

        public static string GetQuestPublishFileNameForLocale(QuestData q, Locale locale)
        {
            string code = GetQuestCode(q);
            string lang = GetLanguageCode(locale);
            if (!IsEnglish(lang) && !string.IsNullOrEmpty(lang))
                return $"{code}.{lang}.md";
            return code + ".md";
        }

        public static string GetQuestScriptPublishFileNameForLocale(QuestData q, Locale locale)
        {
            string code = GetQuestCode(q);
            string lang = GetLanguageCode(locale);
            if (!IsEnglish(lang) && !string.IsNullOrEmpty(lang))
                return $"{code}-script.{lang}.md";
            return code + "-script.md";
        }

        public static string GetHumanTitle(QuestData q)
        {
            if (q == null)
                return string.Empty;
            string fallback = !string.IsNullOrEmpty(q.IdDisplay) ? q.IdDisplay : (!string.IsNullOrEmpty(q.Id) ? q.Id : q.name);
            string t = SafeLocalized(q.Title, fallback);
            return string.IsNullOrWhiteSpace(t) ? fallback : t;
        }

        public static string MatchLineValue(string text, string pattern)
        {
            try
            {
                var rx = new Regex(pattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);
                var m = rx.Match(text);
                if (m.Success && m.Groups.Count > 1)
                    return m.Groups[1].Value.Trim();
            }
            catch { }
            return string.Empty;
        }

        public static string HtmlEscape(string s)
        {
            if (string.IsNullOrEmpty(s))
                return string.Empty;
            return s.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");
        }

        public static string HtmlAttributeEscape(string s)
        {
            if (string.IsNullOrEmpty(s))
                return string.Empty;
            return HtmlEscape(s).Replace("\"", "&quot;");
        }

        public static string Slugify(string s)
        {
            if (string.IsNullOrEmpty(s))
                return string.Empty;
            var sb = new StringBuilder();
            foreach (var ch in s.ToLowerInvariant())
            {
                if ((ch >= 'a' && ch <= 'z') || (ch >= '0' && ch <= '9'))
                    sb.Append(ch);
                else
                    sb.Append('-');
            }
            var slug = sb.ToString();
            slug = Regex.Replace(slug, "-+", "-");
            return slug.Trim('-');
        }

        public static string FormatAuthor(AuthorData a)
        {
            if (a == null)
                return string.Empty;
            string name = a.name;
            string country = string.Empty;
            string url = string.Empty;
            try
            {
                var t = a.GetType();
                var nameFi = t.GetField("Name", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                var namePi = t.GetProperty("Name", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (nameFi != null && nameFi.FieldType == typeof(string))
                    name = nameFi.GetValue(a) as string ?? name;
                else if (namePi != null && namePi.PropertyType == typeof(string) && namePi.CanRead)
                    name = namePi.GetValue(a, null) as string ?? name;

                var countryFi = t.GetField("Country", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                var countryPi = t.GetProperty("Country", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (countryFi != null && countryFi.FieldType == typeof(Countries))
                    country = ((Countries)countryFi.GetValue(a)).ToString();
                else if (countryPi != null && countryPi.PropertyType == typeof(Countries) && countryPi.CanRead)
                    country = ((Countries)countryPi.GetValue(a, null)).ToString();

                var urlFi = t.GetField("Url", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                var urlPi = t.GetProperty("Url", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (urlFi != null && urlFi.FieldType == typeof(string))
                    url = urlFi.GetValue(a) as string ?? string.Empty;
                else if (urlPi != null && urlPi.PropertyType == typeof(string) && urlPi.CanRead)
                    url = urlPi.GetValue(a, null) as string ?? string.Empty;
            }
            catch { }

            string displayName = name;
            if (!string.IsNullOrEmpty(url))
                displayName = $"[{name}]({url})";
            if (!string.IsNullOrEmpty(country))
                return $"{displayName} ({country})";
            return displayName;
        }

        public static string GetAuthorName(AuthorData a)
        {
            if (a == null)
                return string.Empty;
            string name = a.name;
            try
            {
                var t = a.GetType();
                var nameFi = t.GetField("Name", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                var namePi = t.GetProperty("Name", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (nameFi != null && nameFi.FieldType == typeof(string))
                    name = nameFi.GetValue(a) as string ?? name;
                else if (namePi != null && namePi.PropertyType == typeof(string) && namePi.CanRead)
                    name = namePi.GetValue(a, null) as string ?? name;
            }
            catch { }
            return name ?? string.Empty;
        }

        public static string SafeLocalized(LocalizedString ls, string fallback)
        {
            if (ls == null)
                return fallback;
            try
            {
                var s = ls.GetLocalizedString();
                return string.IsNullOrWhiteSpace(s) ? fallback : s;
            }
            catch { return fallback; }
        }

        public static LocalizedString GetLocalizedString(object obj, string propertyName)
        {
            if (obj == null || string.IsNullOrEmpty(propertyName))
                return null;
            try
            {
                var t = obj.GetType();
                var pi = t.GetProperty(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (pi != null && typeof(LocalizedString).IsAssignableFrom(pi.PropertyType))
                    return pi.GetValue(obj) as LocalizedString;
                var fi = t.GetField(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (fi != null && typeof(LocalizedString).IsAssignableFrom(fi.FieldType))
                    return fi.GetValue(obj) as LocalizedString;
            }
            catch { }
            return null;
        }

        public static string GetCardCategoryString(object card)
        {
            if (card == null)
                return string.Empty;
            object val = GetMember(card, "Category") ?? GetMember(card, "CardCategory");
            return val != null ? val.ToString() : string.Empty;
        }

        public static string GetCardYearString(object card)
        {
            if (card == null)
                return string.Empty;
            object val = GetMember(card, "Year");
            return val != null ? val.ToString() : string.Empty;
        }

        static object GetMember(object obj, string member)
        {
            try
            {
                var t = obj.GetType();
                var pi = t.GetProperty(member, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (pi != null && pi.CanRead)
                    return pi.GetValue(obj);
                var fi = t.GetField(member, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (fi != null)
                    return fi.GetValue(obj);
            }
            catch { }
            return null;
        }

        public static string TryToString(Func<string> getter)
        {
            try
            { return getter(); }
            catch { return string.Empty; }
        }

        public static string GetLanguageCode(Locale locale)
        {
            try
            {
                if (locale == null)
                    return string.Empty;
                var id = locale.Identifier;
                var code = id.Code;
                return string.IsNullOrEmpty(code) ? string.Empty : code.ToLowerInvariant();
            }
            catch { return string.Empty; }
        }

        public static bool IsEnglish(string lang)
        {
            if (string.IsNullOrEmpty(lang))
                return true;
            return lang.StartsWith("en");
        }

        public static string GetCardImageAssetPath(CardData card)
        {
            try
            {
                UnityEngine.Object asset = null;
                var imgAsset = GetMember(card, "ImageAsset");
                if (imgAsset != null)
                {
                    var imageProp = imgAsset.GetType().GetProperty("Image", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    if (imageProp != null)
                        asset = imageProp.GetValue(imgAsset) as UnityEngine.Object;
                }
                if (asset == null)
                    asset = GetMember(card, "Image") as UnityEngine.Object; // fallback legacy
                if (asset != null)
                    return AssetDatabase.GetAssetPath(asset);
            }
            catch { }
            return string.Empty;
        }

        // Temporarily set locale, execute action, then restore previous
        public static void WithLocale(Locale locale, Action action)
        {
            var prev = LocalizationSettings.SelectedLocale;
            try
            {
                if (locale != null)
                    LocalizationSettings.SelectedLocale = locale;
                action?.Invoke();
            }
            finally
            {
                LocalizationSettings.SelectedLocale = prev;
            }
        }

        public static string EncodeUriString(string path)
        {
            if (string.IsNullOrEmpty(path))
                return string.Empty;
            return Uri.EscapeUriString(path).Replace("%2F", "/").Replace("%5C", "/");
            //            var encodedScriptPath = string.Join("/", scriptPath.Replace("\\", "/").Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries).Select(seg => Uri.EscapeDataString(seg)));
        }

        public static string EscapeParagraph(string s)
        {
            if (string.IsNullOrEmpty(s))
                return string.Empty;
            return s.Replace("\r\n", "\n").Replace("\r", "\n");
        }

    }
}
#endif
