#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace Antura.Discover.Editor
{
    public static class QuestExportUtils
    {
        public static string BuildQuestMarkdown(QuestData q, bool includeLanguageMenu = false, string scriptPageFileName = null, Locale locale = null)
        {
            var sb = new StringBuilder();

            string title = PublishUtils.GetHumanTitle(q);
            string desc = PublishUtils.SafeLocalized(q.Description, fallback: q.DescriptionText);
            string code = PublishUtils.GetQuestCode(q);

            sb.AppendLine("---");
            sb.AppendLine("title: " + title + " (" + code + ")");
            sb.AppendLine("hide:");
            sb.AppendLine("---");
            sb.AppendLine();

            sb.AppendLine("# " + title + " (" + code + ")");

            if (includeLanguageMenu)
            {
                var lang = PublishUtils.GetLanguageCode(locale);
                var indexLink = string.IsNullOrEmpty(lang) || PublishUtils.IsEnglish(lang) ? "./index.md" : $"./index.{lang}.md";

                string en = (string.IsNullOrEmpty(lang) || PublishUtils.IsEnglish(lang)) ? "english" : $"[english](./{code}.md)";
                string fr = (!string.IsNullOrEmpty(lang) && lang.StartsWith("fr")) ? "french" : $"[french](./{code}.fr.md)";
                string pl = (!string.IsNullOrEmpty(lang) && lang.StartsWith("pl")) ? "polish" : $"[polish](./{code}.pl.md)";
                string it = (!string.IsNullOrEmpty(lang) && lang.StartsWith("it")) ? "italian" : $"[italian](./{code}.it.md)";

                sb.AppendLine($"[Quest Index]({indexLink}) - Language: {en} - {fr} - {pl} - {it}");
                sb.AppendLine();
            }

            sb.AppendLine(GetEditInfoSection(q));

            sb.AppendLine("Version: " + q.VersionText + "  ");
            sb.AppendLine("Status: " + q.Status + "  ");
            string locName = string.Empty;
            if (q.Location != null)
            {
                var fallback = !string.IsNullOrEmpty(q.Location.Id) ? q.Location.Id : q.Location.name;
                // Use localized Location.Name when available
                var localized = PublishUtils.SafeLocalized(q.Location.Name, fallback: fallback);
                locName = localized;
            }
            sb.AppendLine("Location: " + q.Country + (string.IsNullOrEmpty(locName) ? string.Empty : (" - " + locName)));
            sb.AppendLine();

            if (!string.IsNullOrEmpty(desc))
            {
                sb.AppendLine(desc);
                sb.AppendLine();
            }

            sb.AppendLine("## Content");
            sb.AppendLine("Subjects: ");
            sb.AppendLine();

            if (q.Subjects != null)
            {
                foreach (var cc in q.Subjects)
                {
                    sb.AppendLine("  - " + cc.Subject + " (x" + cc.Count + ")");
                }
                sb.AppendLine();
            }

            // Topics list (links to topics index anchors)
            if (q.Topics != null && q.Topics.Count > 0)
            {
                sb.AppendLine("## Topics");
                foreach (var topic in q.Topics.Where(t => t != null))
                {
                    string tId = !string.IsNullOrEmpty(topic.Id) ? topic.Id : topic.name;
                    string tName = !string.IsNullOrEmpty(topic.Name) ? topic.Name : tId;
                    sb.AppendLine($"- [{tName}](../topics/index.md#{tId})");
                }
                sb.AppendLine();
            }

            // Linked cards (expanded)
            if (q.Cards != null && q.Cards.Count > 0)
            {
                sb.AppendLine();
                sb.AppendLine("## Cards");
                foreach (var card in q.Cards.Where(c => c != null))
                {
                    string cTitle = PublishUtils.SafeLocalized(PublishUtils.GetLocalizedString(card, "Title"), fallback: string.IsNullOrEmpty(card.Id) ? card.name : card.Id);
                    string cDesc = PublishUtils.SafeLocalized(PublishUtils.GetLocalizedString(card, "Description"), fallback: string.Empty);
                    string cCategory = PublishUtils.GetCardCategoryString(card);
                    string cYear = PublishUtils.GetCardYearString(card);
                    string cCountry = PublishUtils.TryToString(() => card.Country.ToString());
                    string cKV = PublishUtils.TryToString(() => card.Points.ToString());
                    string cImagePath = PublishUtils.GetCardImageAssetPath(card);
                    string cId = !string.IsNullOrEmpty(card.Id) ? card.Id : card.name;

                    sb.AppendLine($"**[{cTitle}](../cards/index.md#{cId})**  ");
                    sb.AppendLine($"{cDesc}  ");
                    sb.AppendLine();
                }
            }

            sb.AppendLine("## Words");
            if (q.Words != null && q.Words.Count > 0)
            {
                sb.AppendLine("- " + string.Join(", ", q.Words.Where(w => w != null).Select(w => string.IsNullOrEmpty(w.Id) ? w.name : w.Id)));
            }

            sb.AppendLine("## Activities");
            try
            {
                var prefab = q != null ? q.QuestPrefab : null;
                var qmType = FindTypeByName("QuestManager");
                if (prefab != null && qmType != null)
                {
                    var qm = prefab.GetComponent(qmType);
                    if (qm != null)
                    {
                        var acMember = qmType.GetField("ActivityConfigs", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                                       ?? (MemberInfo)qmType.GetProperty("ActivityConfigs", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                        var acValue = GetMemberValue(qm, acMember);
                        int count = 0;
                        foreach (var item in AsEnumerable(acValue))
                        {
                            string codeStr = GetActivityCode(item);
                            if (!string.IsNullOrEmpty(codeStr))
                                sb.AppendLine($"- [{codeStr}](../activities/index.md#{codeStr})");
                            else
                            {
                                string label = GetActivityLabel(item);
                                if (!string.IsNullOrEmpty(label))
                                    sb.AppendLine("- " + label);
                            }
                            count++;
                        }
                        if (count == 0)
                            sb.AppendLine("- (none)");

                        sb.AppendLine();
                        sb.AppendLine("## Tasks");
                        var qtMember = qmType.GetField("QuestTasks", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                                      ?? (MemberInfo)qmType.GetProperty("QuestTasks", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                        var qtValue = GetMemberValue(qm, qtMember);
                        int tcount = 0;
                        foreach (var item in AsEnumerable(qtValue))
                        {
                            string label = GetTaskLabel(item);
                            if (!string.IsNullOrEmpty(label))
                                sb.AppendLine("- " + label);
                            tcount++;
                        }
                        if (tcount == 0)
                            sb.AppendLine("- (none)");
                    }
                    else
                    {
                        sb.AppendLine("- (no QuestManager on prefab)");
                        sb.AppendLine();
                        sb.AppendLine("## Tasks");
                        sb.AppendLine("- (no QuestManager on prefab)");
                    }
                }
                else
                {
                    sb.AppendLine("- (none)");
                    sb.AppendLine();
                    sb.AppendLine("## Tasks");
                    sb.AppendLine("- (none)");
                }
            }
            catch
            {
                sb.AppendLine("- (error reading activities)");
                sb.AppendLine();
                sb.AppendLine("## Tasks");
                sb.AppendLine("- (error reading tasks)");
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
                foreach (var kv in authorRoles.OrderBy(k => PublishUtils.GetAuthorName(k.Key), StringComparer.OrdinalIgnoreCase))
                {
                    var roles = kv.Value.OrderBy(r => r, StringComparer.OrdinalIgnoreCase).ToList();
                    var suffix = roles.Count > 0 ? " (" + string.Join(", ", roles) + ")" : string.Empty;
                    sb.AppendLine("- " + PublishUtils.FormatAuthor(kv.Key) + suffix);
                }
            }

            if (q.AdditionalResources != null && !string.IsNullOrEmpty(q.AdditionalResources.text))
            {
                // sb.AppendLine();
                // sb.AppendLine("## Additional Resources");
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
            string code = PublishUtils.GetQuestCode(q);
            string title = PublishUtils.GetHumanTitle(q);

            sb.AppendLine("---");
            sb.AppendLine("title: " + title + " (" + code + ") - Script");
            sb.AppendLine("hide:");
            // sb.AppendLine("  - navigation");
            sb.AppendLine("---");
            sb.AppendLine();

            sb.AppendLine("# " + title + " (" + code + ") - Script");
            if (includeLanguageMenu)
            {
                var lang = PublishUtils.GetLanguageCode(locale);
                var indexLink = string.IsNullOrEmpty(lang) || PublishUtils.IsEnglish(lang) ? "./index.md" : $"./index.{lang}.md";
                string en = (string.IsNullOrEmpty(lang) || PublishUtils.IsEnglish(lang)) ? "english" : $"[english](./{code}-script.md)";
                string fr = (!string.IsNullOrEmpty(lang) && lang.StartsWith("fr")) ? "french" : $"[french](./{code}-script.fr.md)";
                string pl = (!string.IsNullOrEmpty(lang) && lang.StartsWith("pl")) ? "polish" : $"[polish](./{code}-script.pl.md)";
                string it = (!string.IsNullOrEmpty(lang) && lang.StartsWith("it")) ? "italian" : $"[italian](./{code}-script.it.md)";
                sb.AppendLine($"[Quest Index]({indexLink}) - Language: {en} - {fr} - {pl} - {it}");
                sb.AppendLine();
            }

            sb.AppendLine(GetEditInfoSection(q));

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

                string title = PublishUtils.MatchLineValue(rawPart, @"^\s*title:\s*(.+)$");
                string nodeColor = PublishUtils.MatchLineValue(rawPart, @"^\s*color:\s*([^\r\n]+)$");
                string nodeId = !string.IsNullOrEmpty(title) ? ($"ys-node-{PublishUtils.Slugify(title)}") : string.Empty;

                var lines = rawPart.Replace("\r\n", "\n").Replace("\r", "\n").Split('\n');
                var code = new StringBuilder();

                bool inHeader = true;
                bool seenTitle = false;
                foreach (var lineRaw in lines)
                {
                    string line = PublishUtils.HtmlEscape(lineRaw);
                    string t = line.Trim();
                    if (inHeader)
                    {
                        if (!seenTitle && t.Length == 0)
                            continue;
                        if (t == "---")
                        { code.AppendLine("<span class=\"yarn-header-dim\">---</span>"); inHeader = false; continue; }
                        if (t.StartsWith("title:"))
                        { seenTitle = true; continue; }
                        if (t.StartsWith("position:"))
                        { continue; }
                        if (t.Length == 0)
                        { code.AppendLine(""); continue; }
                        code.AppendLine($"<span class=\"yarn-header-dim\">{line}</span>");
                    }
                    else
                    {
                        string processed = line;
                        processed = Regex.Replace(processed, "&lt;&lt;[^&]*?&gt;&gt;", m => $"<span class=\"yarn-cmd\">{m.Value}</span>");
                        processed = Regex.Replace(processed, "#line:[^\\n]*", m => $"<span class=\"yarn-meta\">{m.Value}</span>");
                        if (Regex.IsMatch(t, @"^[-\\s]?>"))
                        { processed = $"<span class=\"yarn-choice\">{processed}</span>"; }
                        if (t.StartsWith("//"))
                        { processed = $"<span class=\"yarn-comment\">{processed}</span>"; }
                        bool isSpoken = t.Length > 0 && !t.StartsWith("//") && !Regex.IsMatch(t, @"^[-\\s]?>") && !t.StartsWith("#line:") && !t.StartsWith("&lt;&lt;");
                        if (isSpoken)
                            code.AppendLine($"<span class=\"yarn-line\">{processed}</span>");
                        else
                            code.AppendLine(processed);
                    }
                }

                if (!string.IsNullOrEmpty(title))
                {
                    sb.AppendLine($"<a id=\"{nodeId}\"></a>");
                    sb.AppendLine($"## {PublishUtils.HtmlEscape(title)}");
                    sb.AppendLine();
                }
                string style = !string.IsNullOrEmpty(nodeColor) ? $" style=\"--node-color:{PublishUtils.HtmlAttributeEscape(nodeColor)}\"" : string.Empty;
                sb.Append("<div class=\"yarn-node\"");
                if (!string.IsNullOrEmpty(title))
                    sb.Append($" data-title=\"{PublishUtils.HtmlAttributeEscape(title)}\"");
                sb.Append(">");
                sb.Append($"<pre class=\"yarn-code\"{style}><code>");
                sb.Append(code.ToString());
                sb.Append("</code></pre></div>\n\n");
            }

            return sb.ToString();
        }

        // Local helpers (kept private to this file)
        static Type FindTypeByName(string name)
        {
            try
            {
                foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
                {
                    try
                    {
                        var t = asm.GetTypes().FirstOrDefault(x => x.Name == name || x.FullName.EndsWith("." + name, StringComparison.Ordinal));
                        if (t != null)
                            return t;
                    }
                    catch { }
                }
            }
            catch { }
            return null;
        }

        static string GetEditInfoSection(QuestData q)
        {
            var scriptPath = q != null && q.YarnScript != null ? AssetDatabase.GetAssetPath(q.YarnScript) : "NO_SCRIPT_ATTACHED.yarn";
            var githublink = "https://github.com/vgwb/Antura/blob/main/" + PublishUtils.EncodeUriString(scriptPath);
            var googlelink = q.GoogleSheetUrl;
            var editInfo = "!!! note \"Educators & Designers: help improving this quest!\"" + "\n";
            editInfo += $"    **Comments and feedback**: [discuss in the Forum]({q.ForumUrl})  " + "\n";
            editInfo += $"    **Improve translations**: [comment the Google Sheet]({googlelink})  " + "\n";
            editInfo += $"    **Improve the script**: [propose an edit here]({githublink})  " + "\n";
            return editInfo;
        }

        static System.Collections.IEnumerable AsEnumerable(object obj)
        {
            if (obj is System.Collections.IEnumerable en)
                return en;
            return Array.Empty<object>();
        }

        static object GetMemberValue(object instance, MemberInfo mi)
        {
            if (instance == null || mi == null)
                return null;
            try
            {
                if (mi is PropertyInfo pi)
                    return pi.GetValue(instance);
                if (mi is FieldInfo fi)
                    return fi.GetValue(instance);
            }
            catch { }
            return null;
        }

        static string GetActivityLabel(object cfg)
        {
            if (cfg == null)
                return string.Empty;
            try
            {
                var t = cfg.GetType();
                object val =
                    (t.GetProperty("Code", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(cfg)) ??
                    (t.GetField("Code", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(cfg)) ??
                    (t.GetProperty("ActivityCode", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(cfg)) ??
                    (t.GetField("ActivityCode", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(cfg)) ??
                    (t.GetProperty("Activity", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(cfg)) ??
                    (t.GetField("Activity", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(cfg)) ??
                    (t.GetProperty("Name", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(cfg)) ??
                    (t.GetField("Name", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(cfg));
                string s = val != null ? val.ToString() : cfg.ToString();
                return string.IsNullOrWhiteSpace(s) ? t.Name : s;
            }
            catch { return cfg.ToString(); }
        }

        static string GetTaskLabel(object task)
        {
            if (task == null)
                return string.Empty;
            try
            {
                var t = task.GetType();
                object typeVal =
                    (t.GetProperty("Type", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(task)) ??
                    (t.GetField("Type", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(task));
                object codeVal =
                    (t.GetProperty("Code", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(task)) ??
                    (t.GetField("Code", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(task));
                string typeS = typeVal != null ? typeVal.ToString() : null;
                string codeS = codeVal != null ? codeVal.ToString() : null;
                if (!string.IsNullOrEmpty(typeS) && !string.IsNullOrEmpty(codeS))
                    return $"[{typeS}] {codeS}";
                if (!string.IsNullOrEmpty(typeS))
                    return typeS;
                if (!string.IsNullOrEmpty(codeS))
                    return codeS;
                return task.ToString();
            }
            catch { return task.ToString(); }
        }

        static string GetActivityCode(object cfg)
        {
            if (cfg == null)
                return string.Empty;
            try
            {
                var t = cfg.GetType();
                // Preferred: ActivityConfig.ActivitySettings.ActivityCode
                try
                {
                    var settingsObj = t.GetProperty("ActivitySettings", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(cfg)
                                     ?? t.GetField("ActivitySettings", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(cfg);
                    if (settingsObj != null)
                    {
                        var st = settingsObj.GetType();
                        var acVal = st.GetProperty("ActivityCode", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(settingsObj)
                                   ?? st.GetField("ActivityCode", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(settingsObj);
                        if (acVal != null)
                            return acVal.ToString();
                    }
                }
                catch { }

                // Fallbacks: ActivityConfig.Code or ActivityConfig.ActivityCode
                var pi = t.GetProperty("Code", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                         ?? t.GetProperty("ActivityCode", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (pi != null)
                {
                    var v = pi.GetValue(cfg);
                    return v != null ? v.ToString() : string.Empty;
                }
                var fi = t.GetField("Code", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                        ?? t.GetField("ActivityCode", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (fi != null)
                {
                    var v = fi.GetValue(cfg);
                    return v != null ? v.ToString() : string.Empty;
                }
            }
            catch { }
            return string.Empty;
        }


    }
}
#endif
