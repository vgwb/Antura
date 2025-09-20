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

            // Language menu removed: global locale switch handles language selection now.

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

            // Topics list (links + their cards)
            var topicCardsSet = new HashSet<CardData>();
            if (q.Topics != null && q.Topics.Count > 0)
            {
                sb.AppendLine("## Topics");
                foreach (var topic in q.Topics.Where(t => t != null))
                {
                    string topicId = !string.IsNullOrEmpty(topic.Id) ? topic.Id : topic.name;
                    string tName = !string.IsNullOrEmpty(topic.Name) ? topic.Name : topicId;
                    sb.AppendLine($"### [{tName}](../../topics/index.md#{topicId})");
                    sb.AppendLine();

                    // List all cards of the topic (core + connections + discovery path) with description
                    try
                    {
                        var allCards = topic.GetAllCards();
                        foreach (var c in allCards.Where(ca => ca != null))
                        {
                            topicCardsSet.Add(c);
                            string cardId = !string.IsNullOrEmpty(c.Id) ? c.Id : c.name;
                            string cTitle = PublishUtils.SafeLocalized(PublishUtils.GetLocalizedString(c, "Title"), fallback: cardId);
                            string cDesc = PublishUtils.SafeLocalized(PublishUtils.GetLocalizedString(c, "Description"), fallback: string.Empty);
                            sb.AppendLine($"  - **[{cTitle}](../../cards/index.md#{cardId})**  ");
                            if (!string.IsNullOrEmpty(cDesc))
                                sb.AppendLine($"    {cDesc}  ");
                        }
                    }
                    catch { }
                }
                sb.AppendLine();
            }

            // Additional quest cards (those not already covered by topics)
            if (q.Cards != null && q.Cards.Count > 0)
            {
                var additional = q.Cards.Where(c => c != null && !topicCardsSet.Contains(c)).ToList();
                if (additional.Count > 0)
                {
                    sb.AppendLine("## Additional Cards");
                    foreach (var card in additional)
                    {
                        string cTitle = PublishUtils.SafeLocalized(PublishUtils.GetLocalizedString(card, "Title"), fallback: string.IsNullOrEmpty(card.Id) ? card.name : card.Id);
                        string cDesc = PublishUtils.SafeLocalized(PublishUtils.GetLocalizedString(card, "Description"), fallback: string.Empty);
                        string cId = !string.IsNullOrEmpty(card.Id) ? card.Id : card.name;
                        sb.AppendLine($"**[{cTitle}](../../cards/index.md#{cId})**  ");
                        if (!string.IsNullOrEmpty(cDesc))
                            sb.AppendLine($"{cDesc}  ");
                        sb.AppendLine();
                    }
                }
            }

            // Quest Script placed early (before other metadata)
            sb.AppendLine("## Quest Script");
            sb.AppendLine();
            if (q.IsScriptPublic)
            {
                if (!string.IsNullOrEmpty(scriptPageFileName))
                {
                    sb.AppendLine($"[See the full script here](./{scriptPageFileName})");
                }
                else if (q.YarnScript != null)
                {
                    sb.AppendLine("```yarn");
                    var strippedEarly = RemoveSceneDataChunk(q.YarnScript.text);
                    sb.AppendLine(strippedEarly);
                    sb.AppendLine("```");
                }
                else
                {
                    sb.AppendLine("(No YarnScript attached)");
                }
            }
            else
            {
                sb.AppendLine("(Script not public)");
            }

            // Separator before remaining sections
            sb.AppendLine();
            sb.AppendLine("---");
            sb.AppendLine();

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
                                sb.AppendLine($"- [{codeStr}](../../activities/index.md#{codeStr})");
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

            return sb.ToString();
        }

        // Build the separate script page for web publish
        public static string BuildQuestScriptMarkdown(QuestData q, bool includeLanguageMenu = false, Locale locale = null)
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
            // Language menu removed: global locale switch handles language selection now.

            sb.AppendLine(GetEditInfoSection(q));

            if (q.YarnScript != null)
            {
                var raw = RemoveSceneDataChunk(q.YarnScript.text);
                sb.AppendLine(RenderYarnAsHtml(raw, q, locale));
            }
            else
            {
                sb.AppendLine("(No YarnScript attached)");
            }

            return sb.ToString();
        }

        // Render Yarn script like the JS viewer: H2 per node + tokenized code inside a styled <pre>
        // Enhanced renderer with optional translation lookup from QuestData.QuestStringsTable.
        static string RenderYarnAsHtml(string script, QuestData quest = null, Locale locale = null)
        {
            if (string.IsNullOrEmpty(script))
                return string.Empty;
            script = RemoveSceneDataChunk(script);

            // Prepare translation table if available.
            UnityEngine.Localization.Tables.StringTable translationTable = null;
            if (quest != null && quest.QuestStringsTable != null)
            {
                try
                {
                    if (locale != null)
                    {
                        translationTable = UnityEngine.Localization.Settings.LocalizationSettings.StringDatabase.GetTable(quest.QuestStringsTable.TableReference, locale);
                    }
                    if (translationTable == null)
                    {
                        // Fallback to currently selected locale or default
                        translationTable = quest.QuestStringsTable.GetTable();
                    }
                }
                catch { }
            }

            string LookupTranslation(string lineKey)
            {
                if (translationTable == null || string.IsNullOrEmpty(lineKey))
                    return string.Empty;
                try
                {
                    var entry = translationTable.GetEntry(lineKey);
                    if (entry != null)
                    {
                        // Prefer direct value to avoid re-resolving locale context.
                        var val = entry.Value;
                        if (!string.IsNullOrEmpty(val))
                            return val.Trim();
                    }
                }
                catch { }
                return string.Empty;
            }

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
                        // We now output ONLY translations (no original English) for spoken/choice lines.
                        string originalEscaped = line; // HTML-escaped original line
                        string lineKey = string.Empty;
                        var mTag = Regex.Match(lineRaw, @"#line:([A-Za-z0-9_]+)");
                        if (mTag.Success && mTag.Groups.Count > 1)
                        { lineKey = "line:" + mTag.Groups[1].Value; }

                        bool isChoice = Regex.IsMatch(t, @"^[-\\s]?>");
                        bool isComment = t.StartsWith("//");
                        bool isCommandOnly = t.StartsWith("&lt;&lt;") || Regex.IsMatch(t, "&lt;&lt;[^&]*?&gt;&gt;");
                        bool hasLineTag = t.Contains("#line:");
                        bool isSpoken = !isComment && !isChoice && !isCommandOnly && !string.IsNullOrEmpty(t) && hasLineTag;

                        // Capture leading indentation from original raw line (spaces/tabs) to reapply before translated text.
                        string indent = System.Text.RegularExpressions.Regex.Match(lineRaw, @"^\s*").Value;
                        string indentHtml = PublishUtils.HtmlEscape(indent);

                        // Extract meta tag text (e.g., #line:abc123) without wrapping it yet
                        string metaText = string.Empty;
                        var metaMatch = Regex.Match(lineRaw, @"#line:[^\r\n]*");
                        if (metaMatch.Success)
                            metaText = metaMatch.Value;

                        // Determine translation
                        string translation = string.IsNullOrEmpty(lineKey) ? string.Empty : LookupTranslation(lineKey);

                        if (isComment)
                        {
                            // Output comment content without meta as one span and meta as a sibling span (no nested spans)
                            string commentContent = Regex.Replace(lineRaw, @"#line:[^\r\n]*", "");
                            commentContent = PublishUtils.HtmlEscape(commentContent.TrimEnd());
                            var metaSuffix = string.IsNullOrEmpty(metaText) ? string.Empty : (" <span class=\"yarn-meta\">" + PublishUtils.HtmlEscape(metaText) + "</span>\n");
                            code.AppendLine($"<span class=\"yarn-comment\">{commentContent}</span>{metaSuffix}");
                            continue;
                        }

                        if (isCommandOnly)
                        {
                            // Highlight commands, append meta as a sibling span (no nesting)
                            string cmdProcessed = Regex.Replace(originalEscaped, "&lt;&lt;[^&]*?&gt;&gt;", m => $"<span class=\"yarn-cmd\">{m.Value}</span>");
                            cmdProcessed = Regex.Replace(cmdProcessed, @"#line:[^\n]*", "");
                            var metaSuffix = string.IsNullOrEmpty(metaText) ? string.Empty : (" <span class=\"yarn-meta\">" + PublishUtils.HtmlEscape(metaText) + "</span>");
                            code.AppendLine(cmdProcessed + metaSuffix);
                            continue;
                        }

                        // Build choice prefix if present
                        string choicePrefix = string.Empty;
                        if (isChoice)
                        {
                            var mChoice = Regex.Match(lineRaw, @"^\s*[-\s]*>");
                            if (mChoice.Success)
                                choicePrefix = PublishUtils.HtmlEscape(mChoice.Value.TrimEnd()) + " ";
                        }

                        string finalText;
                        if (!string.IsNullOrEmpty(translation))
                        {
                            finalText = PublishUtils.HtmlEscape(translation);
                        }
                        else
                        {
                            // Missing translation fallback depends on locale, but if the original has no visible text (only a tag) leave it blank.
                            string originalPlain = lineRaw;
                            originalPlain = Regex.Replace(originalPlain, @"#line:[^\n]*", "").TrimEnd();
                            string langCode = locale != null ? PublishUtils.GetLanguageCode(locale) : string.Empty;

                            if (string.IsNullOrWhiteSpace(originalPlain))
                            {
                                // Truly blank line (or only metadata): render as blank without placeholder.
                                finalText = string.Empty;
                            }
                            else if (PublishUtils.IsEnglish(langCode) || string.IsNullOrEmpty(langCode))
                            {
                                finalText = PublishUtils.HtmlEscape(originalPlain);
                            }
                            else
                            {
                                finalText = "[MISSING TRANSLATION: " + PublishUtils.HtmlEscape(originalPlain) + "]";
                            }
                        }

                        string contentHtml = indentHtml + choicePrefix + finalText;
                        string metaSuffix2 = string.IsNullOrEmpty(metaText) ? string.Empty : (" <span class=\"yarn-meta\">" + PublishUtils.HtmlEscape(metaText) + "</span>");

                        if (isChoice)
                        {
                            code.AppendLine($"<span class=\"yarn-choice\">{contentHtml}</span>{metaSuffix2}");
                        }
                        else if (isSpoken)
                        {
                            code.AppendLine($"<span class=\"yarn-line\">{contentHtml}</span>{metaSuffix2}");
                        }
                        else
                        {
                            // Lines that don't fit above categories: emit content followed by optional meta span
                            code.AppendLine(contentHtml + metaSuffix2);
                        }
                    }
                }

                if (!string.IsNullOrEmpty(title))
                {
                    sb.AppendLine($"<a id=\"{nodeId}\"></a>\n");
                    sb.AppendLine($"## {PublishUtils.HtmlEscape(title)}");
                    sb.AppendLine();
                }
                string style = !string.IsNullOrEmpty(nodeColor) ? $" style=\"--node-color:{PublishUtils.HtmlAttributeEscape(nodeColor)}\"" : string.Empty;
                sb.Append("<div class=\"yarn-node\"");
                if (!string.IsNullOrEmpty(title))
                    sb.Append($" data-title=\"{PublishUtils.HtmlAttributeEscape(title)}\"");
                sb.Append(">\n");
                sb.Append($"<pre class=\"yarn-code\"{style}><code>\n");
                sb.Append(code.ToString());
                sb.Append("</code>\n</pre>\n</div>\n\n");
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
            var editInfo = "> [!note] Educators & Designers: help improving this quest!" + "\n";
            editInfo += $"> **Comments and feedback**: [discuss in the Forum]({q.ForumUrl})  " + "\n";
            editInfo += $"> **Improve translations**: [comment the Google Sheet]({googlelink})  " + "\n";
            editInfo += $"> **Improve the script**: [propose an edit here]({githublink})  " + "\n";
            return editInfo;
        }

        // Remove the scene_data metadata block (commented or raw tags) from a Yarn script text.
        static string RemoveSceneDataChunk(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;
            try
            {
                // Support commented or raw tags with optional leading whitespace.
                // Use tempered dot with lazy quantifier to first closing tag occurrence after open.
                var pattern = @"(?is)^[ \t]*//?[ \t]*<scene_data>[ \t]*\r?\n.*?<\/scene_data>[ \t]*\r?\n?";
                var cleaned = Regex.Replace(text, pattern, string.Empty);
                if (cleaned.IndexOf("<scene_data>", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    // Fallback manual removal (single block) if regex missed.
                    int open = cleaned.IndexOf("<scene_data>", StringComparison.OrdinalIgnoreCase);
                    if (open >= 0)
                    {
                        int close = cleaned.IndexOf("</scene_data>", open, StringComparison.OrdinalIgnoreCase);
                        if (close > open)
                        {
                            int after = close + "</scene_data>".Length;
                            cleaned = cleaned.Remove(open, after - open);
                        }
                    }
                }
                return cleaned;
            }
            catch { return text; }
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
