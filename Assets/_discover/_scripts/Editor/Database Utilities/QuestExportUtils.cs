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

namespace Antura.Discover
{
    public static class QuestExportUtils
    {
        public static string GetQuestCode(QuestData q)
        {
            string code = string.IsNullOrEmpty(q.Id) ? q.name : q.Id;
            foreach (var ch in System.IO.Path.GetInvalidFileNameChars())
                code = code.Replace(ch, '-');
            return code;
        }

        public static string GetQuestExportFileName(QuestData q, DateTime? date = null)
        {
            string code = GetQuestCode(q);
            var dateStr = (date ?? DateTime.Now).ToString("yyyy-MM-dd");
            return $"Antura Quest - {code}/{q.VersionText} - {dateStr}.md";
        }

        public static string GetQuestPublishFileName(QuestData q)
        {
            string code = GetQuestCode(q);
            return code + ".md";
        }

        public static string GetQuestPublishFileNameForLocale(QuestData q, Locale locale)
        {
            string code = GetQuestCode(q);
            string lang = GetLanguageCode(locale);
            if (!IsEnglish(lang) && !string.IsNullOrEmpty(lang))
                return $"{code}.{lang}.md"; // e.g., fr_01.fr.md
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

        // Returns the human-facing title with robust fallbacks (IdDisplay -> Id/name)
        public static string GetHumanTitle(QuestData q)
        {
            if (q == null)
                return string.Empty;
            string fallback = !string.IsNullOrEmpty(q.IdDisplay) ? q.IdDisplay : (!string.IsNullOrEmpty(q.Id) ? q.Id : q.name);
            string t = SafeLocalized(q.Title, fallback);
            return string.IsNullOrWhiteSpace(t) ? fallback : t;
        }

        // Builds a localized Markdown for a quest using the current LocalizationSettings.SelectedLocale.
        // includeLanguageMenu: if true, adds the language switch menu (intended for web publish only)
        // scriptPageFileName: if provided and q.IsScriptPublic, the main page will link to this script page instead of embedding
        public static string BuildQuestMarkdown(QuestData q, bool includeLanguageMenu = false, string scriptPageFileName = null, Locale locale = null)
        {
            var sb = new StringBuilder();

            // Resolve human title with robust fallbacks
            string title = GetHumanTitle(q);
            string desc = SafeLocalized(q.Description, fallback: q.DescriptionText);
            string code = GetQuestCode(q);

            // YAML front matter with title
            sb.AppendLine("---");
            sb.AppendLine("title: " + title + " (" + code + ")");
            // Hide navigation in site theme
            sb.AppendLine("hide:");
            sb.AppendLine("  - navigation");
            sb.AppendLine("---");
            sb.AppendLine();

            // Heading and meta
            sb.AppendLine("# " + title + " (" + code + ")");

            // Optional language switch menu (web publish only), after H1
            if (includeLanguageMenu)
            {
                // Web publish: Quest Index (language-specific) + language options; do not link current language
                var lang = GetLanguageCode(locale);
                var indexLink = string.IsNullOrEmpty(lang) || IsEnglish(lang) ? "./index.md" : $"./index.{lang}.md";

                string en = (string.IsNullOrEmpty(lang) || IsEnglish(lang)) ? "english" : $"[english](./{code}.md)";
                string fr = (!string.IsNullOrEmpty(lang) && lang.StartsWith("fr")) ? "french" : $"[french](./{code}.fr.md)";
                string pl = (!string.IsNullOrEmpty(lang) && lang.StartsWith("pl")) ? "polish" : $"[polish](./{code}.pl.md)";
                string it = (!string.IsNullOrEmpty(lang) && lang.StartsWith("it")) ? "italian" : $"[italian](./{code}.it.md)";

                sb.AppendLine($"[Quest Index]({indexLink}) - Language: {en} - {fr} - {pl} - {it}");
                sb.AppendLine();
            }
            sb.AppendLine("Version: " + q.VersionText);
            sb.AppendLine("Status: " + q.DevStatus);
            sb.AppendLine();

            if (!string.IsNullOrEmpty(desc))
            {
                sb.AppendLine(desc);
                sb.AppendLine();
            }

            // Data section
            sb.AppendLine("## Informations");
            sb.AppendLine();
            sb.AppendLine("- Title: " + title);
            sb.AppendLine("- Description: " + (string.IsNullOrEmpty(desc) ? q.DescriptionText : desc));
            string locId = q.Location != null ? (!string.IsNullOrEmpty(q.Location.Id) ? q.Location.Id : q.Location.name) : string.Empty;
            sb.AppendLine("- Location: " + q.Country + " - " + locId);

            sb.AppendLine("## Content");
            sb.AppendLine("- Category: " + q.MainTopic);
            sb.AppendLine("- Knowledge points: " + q.KnowledgeValue);

            // Topics (unique from cards)
            if (q.Cards != null && q.Cards.Count > 0)
            {
                var set = new HashSet<string>();
                foreach (var cc in q.Cards)
                    if (cc != null && cc.Topics != null)
                        foreach (var t in cc.Topics)
                            set.Add(t.ToString());
                if (set.Count > 0)
                {
                    sb.AppendLine("- Topics:");
                    foreach (var t in set.OrderBy(s => s))
                        sb.AppendLine("  - " + t);
                }
            }

            // Linked cards (expanded)
            if (q.Cards != null && q.Cards.Count > 0)
            {
                sb.AppendLine();
                sb.AppendLine("## Linked Cards");
                foreach (var card in q.Cards.Where(c => c != null))
                {
                    string cTitle = SafeLocalized(GetLocalizedString(card, "Title"), fallback: string.IsNullOrEmpty(card.Id) ? card.name : card.Id);
                    string cDesc = SafeLocalized(GetLocalizedString(card, "Description"), fallback: string.Empty);
                    string cCategory = GetCardCategoryString(card);
                    string cYear = GetCardYearString(card);
                    string cCountry = TryToString(() => card.Country.ToString());
                    string cKV = TryToString(() => card.Points.ToString());
                    string cImagePath = GetCardImageAssetPath(card);

                    sb.AppendLine($"### {cTitle}");
                    sb.AppendLine($"Description: {cDesc}");
                    sb.AppendLine($"Category: {cCategory}");
                    sb.AppendLine($"Year: {cYear}");
                    sb.AppendLine($"Country: {cCountry}");
                    sb.AppendLine($"KnowledgeValue: {cKV}");
                    sb.AppendLine($"Image: {cImagePath}");
                    sb.AppendLine();
                }
            }

            // Words Used
            if (q.Words != null && q.Words.Count > 0)
            {
                sb.AppendLine("- Words Used: " + string.Join(", ", q.Words.Where(w => w != null).Select(w => string.IsNullOrEmpty(w.Id) ? w.name : w.Id)));
            }

            sb.AppendLine("## Gameplay");
            sb.AppendLine("- Difficulty: " + q.Difficulty);
            sb.AppendLine("- Duration (min): " + q.Duration);
            if (q.Gameplay != null && q.Gameplay.Count > 0)
            {
                sb.AppendLine("- Kind:");
                foreach (var g in q.Gameplay)
                    sb.AppendLine("  - " + g);
            }

            // Credits
            sb.AppendLine("## Credits");
            // Build Author -> roles map (lowercase)
            var authorRoles = new Dictionary<AuthorData, HashSet<string>>();
            if (q.Credits != null && q.Credits.Count > 0)
            {
                foreach (var qc in q.Credits.Where(c => c != null && c.Author != null))
                {
                    if (!authorRoles.TryGetValue(qc.Author, out var set))
                    { set = new HashSet<string>(StringComparer.OrdinalIgnoreCase); authorRoles[qc.Author] = set; }
                    if (qc.Content)
                        set.Add("content");
                    if (qc.Design)
                        set.Add("design");
                    if (qc.Development)
                        set.Add("development");
                    if (qc.Validation)
                        set.Add("validation");
                }
            }
            if (authorRoles.Count > 0)
            {
                foreach (var kv in authorRoles.OrderBy(k => GetAuthorName(k.Key), StringComparer.OrdinalIgnoreCase))
                {
                    var roles = kv.Value.OrderBy(r => r, StringComparer.OrdinalIgnoreCase).ToList();
                    var suffix = roles.Count > 0 ? " (" + string.Join(", ", roles) + ")" : string.Empty;
                    sb.AppendLine("- " + FormatAuthor(kv.Key) + suffix);
                }
            }

            // Optional Additional Resources section between Data and Script
            if (q.AdditionalResources != null && !string.IsNullOrEmpty(q.AdditionalResources.text))
            {
                sb.AppendLine();
                sb.AppendLine("## Additional Resources");
                sb.AppendLine();
                sb.AppendLine(q.AdditionalResources.text);
            }

            sb.AppendLine();
            sb.AppendLine("---");
            sb.AppendLine();

            // Script section (published only if IsScriptPublic)
            if (q.IsScriptPublic)
            {
                sb.AppendLine("## Quest Script");
                sb.AppendLine();
                if (!string.IsNullOrEmpty(scriptPageFileName))
                {
                    sb.AppendLine($"[See the full script here](./{scriptPageFileName})");
                }
                else if (q.YarnScript != null)
                {
                    sb.AppendLine("```yarn");
                    sb.AppendLine(q.YarnScript.text);
                    sb.AppendLine("```");
                }
                else
                {
                    sb.AppendLine("(No YarnScript attached)");
                }
            }

            return sb.ToString();
        }

        // Build the separate script page for web publish
        public static string BuildQuestScriptMarkdown(QuestData q, bool includeLanguageMenu = true, Locale locale = null)
        {
            var sb = new StringBuilder();
            string code = GetQuestCode(q);
            string title = GetHumanTitle(q);


            sb.AppendLine("---");
            sb.AppendLine("title: " + title + " (" + code + ") - Script");
            sb.AppendLine("hide:");
            sb.AppendLine("  - navigation");
            sb.AppendLine("---");
            sb.AppendLine();

            sb.AppendLine("# " + title + " (" + code + ") - Script");
            if (includeLanguageMenu)
            {
                var lang = GetLanguageCode(locale);
                var indexLink = string.IsNullOrEmpty(lang) || IsEnglish(lang) ? "./index.md" : $"./index.{lang}.md";
                string en = (string.IsNullOrEmpty(lang) || IsEnglish(lang)) ? "english" : $"[english](./{code}-script.md)";
                string fr = (!string.IsNullOrEmpty(lang) && lang.StartsWith("fr")) ? "french" : $"[french](./{code}-script.fr.md)";
                string pl = (!string.IsNullOrEmpty(lang) && lang.StartsWith("pl")) ? "polish" : $"[polish](./{code}-script.pl.md)";
                string it = (!string.IsNullOrEmpty(lang) && lang.StartsWith("it")) ? "italian" : $"[italian](./{code}-script.it.md)";
                sb.AppendLine($"[Quest Index]({indexLink}) - Language: {en} - {fr} - {pl} - {it}");
                sb.AppendLine();
            }

            if (q.YarnScript != null)
            {
                sb.AppendLine(RenderYarnAsHtml(q.YarnScript.text));
            }
            else
            {
                sb.AppendLine("(No YarnScript attached)");
            }

            return sb.ToString();
        }

        // Render Yarn script like the JS viewer: H2 per node + tokenized code inside a styled <pre>
        static string RenderYarnAsHtml(string script)
        {
            if (string.IsNullOrEmpty(script))
                return string.Empty;
            var parts = script.Split(new[] { "===" }, StringSplitOptions.None);
            var sb = new StringBuilder();
            foreach (var rawPart in parts)
            {
                if (string.IsNullOrWhiteSpace(rawPart))
                    continue;

                // Extract title/color from header
                string title = MatchLineValue(rawPart, @"^\s*title:\s*(.+)$");
                string nodeColor = MatchLineValue(rawPart, @"^\s*color:\s*([^\r\n]+)$");
                string nodeId = !string.IsNullOrEmpty(title) ? ($"ys-node-{Slugify(title)}") : string.Empty;

                var lines = rawPart.Replace("\r\n", "\n").Replace("\r", "\n").Split('\n');
                var code = new StringBuilder();

                bool inHeader = true;
                bool seenTitle = false;
                foreach (var lineRaw in lines)
                {
                    string line = HtmlEscape(lineRaw);
                    string t = line.Trim();
                    if (inHeader)
                    {
                        if (!seenTitle && t.Length == 0)
                            continue; // trim empty lines before title
                        if (t == "---")
                        { code.AppendLine("<span class=\"yarn-header-dim\">---</span>"); inHeader = false; continue; }
                        if (t.StartsWith("title:"))
                        { seenTitle = true; continue; } // omit title line from code
                        if (t.StartsWith("position:"))
                        { continue; } // do not render position lines
                        // other header lines dimmed (position:, tags:, color:, blank)
                        if (t.Length == 0)
                        { code.AppendLine(""); continue; }
                        code.AppendLine($"<span class=\"yarn-header-dim\">{line}</span>");
                        // header line written
                    }
                    else
                    {
                        // Body: process tokens on escaped line
                        string processed = line;
                        // commands: <<...>> are escaped already as &lt;&lt;...&gt;&gt;
                        processed = Regex.Replace(processed, "&lt;&lt;[^&]*?&gt;&gt;", m => $"<span class=\"yarn-cmd\">{m.Value}</span>");
                        // #line: meta tags
                        processed = Regex.Replace(processed, "#line:[^\\n]*", m => $"<span class=\"yarn-meta\">{m.Value}</span>");
                        // choices -> ...
                        if (Regex.IsMatch(t, @"^[-\\s]?>"))
                        {
                            processed = $"<span class=\"yarn-choice\">{processed}</span>";
                        }
                        // comments starting with //
                        if (t.StartsWith("//"))
                        {
                            processed = $"<span class=\"yarn-comment\">{processed}</span>";
                        }
                        // Bold only spoken lines (not commands, not choices, not comments, not meta)
                        bool isSpoken = t.Length > 0
                                         && !t.StartsWith("//")
                                         && !Regex.IsMatch(t, @"^[-\\s]?>")
                                         && !t.StartsWith("#line:")
                                         && !t.StartsWith("&lt;&lt;");
                        if (isSpoken)
                            code.AppendLine($"<span class=\"yarn-line\">{processed}</span>");
                        else
                            code.AppendLine(processed);
                    }
                }

                if (!string.IsNullOrEmpty(title))
                {
                    sb.AppendLine($"<a id=\"{nodeId}\"></a>");
                    sb.AppendLine($"## {HtmlEscape(title)}");
                    sb.AppendLine();
                }
                // Pass node color via CSS variable for very light overlay in CSS
                string style = !string.IsNullOrEmpty(nodeColor) ? $" style=\"--node-color:{HtmlAttributeEscape(nodeColor)}\"" : string.Empty;
                sb.Append("<div class=\"yarn-node\"");
                if (!string.IsNullOrEmpty(title))
                    sb.Append($" data-title=\"{HtmlAttributeEscape(title)}\"");
                sb.Append(">");
                sb.Append($"<pre class=\"yarn-code\"{style}><code>");
                sb.Append(code.ToString());
                sb.Append("</code></pre></div>\n\n");
            }

            return sb.ToString();
        }

        static string MatchLineValue(string text, string pattern)
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

        static string HtmlEscape(string s)
        {
            if (string.IsNullOrEmpty(s))
                return string.Empty;
            return s.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");
        }

        static string HtmlAttributeEscape(string s)
        {
            if (string.IsNullOrEmpty(s))
                return string.Empty;
            return HtmlEscape(s).Replace("\"", "&quot;");
        }

        static string Slugify(string s)
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
            // collapse multiple dashes
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
            {
                displayName = $"[{name}]({url})";
            }
            if (!string.IsNullOrEmpty(country))
            {
                return $"{displayName} ({country})";
            }
            return displayName;
        }

        static string GetAuthorName(AuthorData a)
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
            catch
            {
                return fallback;
            }
        }

        static LocalizedString GetLocalizedString(object obj, string propertyName)
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

        static string GetCardCategoryString(object card)
        {
            if (card == null)
                return string.Empty;
            // Try common names: Category, CardCategory
            object val = GetMember(card, "Category") ?? GetMember(card, "CardCategory");
            return val != null ? val.ToString() : string.Empty;
        }

        static string GetCardYearString(object card)
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

        static string TryToString(Func<string> getter)
        {
            try
            { return getter(); }
            catch { return string.Empty; }
        }

        static string GetLanguageCode(Locale locale)
        {
            try
            {
                if (locale == null)
                    return string.Empty;
                var id = locale.Identifier; // struct
                var code = id.Code; // string
                return string.IsNullOrEmpty(code) ? string.Empty : code.ToLowerInvariant();
            }
            catch { return string.Empty; }
        }

        static bool IsEnglish(string lang)
        {
            if (string.IsNullOrEmpty(lang))
                return true;
            return lang.StartsWith("en");
        }

        static string GetCardImageAssetPath(CardData card)
        {
            try
            {
                UnityEngine.Object asset = null;
                var imgAsset = GetMember(card, "ImageAsset");
                if (imgAsset != null)
                {
                    var imageProp = imgAsset.GetType().GetProperty("Image", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    if (imageProp != null)
                    {
                        asset = imageProp.GetValue(imgAsset) as UnityEngine.Object;
                    }
                }
                if (asset == null)
                {
                    // Fallback to legacy Image (Sprite)
                    asset = GetMember(card, "Image") as UnityEngine.Object;
                }
                if (asset != null)
                {
                    return AssetDatabase.GetAssetPath(asset);
                }
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
                {
                    LocalizationSettings.SelectedLocale = locale;
                }
                action?.Invoke();
            }
            finally
            {
                LocalizationSettings.SelectedLocale = prev;
            }
        }
    }
}
#endif
