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
    public static class TopicExportUtils
    {
        public static string BuildTopicIndexMarkdown(Locale locale)
        {
            var sb = new StringBuilder();
            sb.AppendLine("---");
            sb.AppendLine("title: Topics");
            sb.AppendLine("hide:");
            sb.AppendLine("---\n");
            sb.AppendLine("# Topics\n");

            // Load topics
            var guids = AssetDatabase.FindAssets("t:TopicData");
            var topics = new List<TopicData>();
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var k = AssetDatabase.LoadAssetAtPath<TopicData>(path);
                if (k != null)
                    topics.Add(k);
            }

            // Preload all quests once for cross-referencing
            var questGuids = AssetDatabase.FindAssets("t:QuestData");
            var quests = new List<QuestData>(questGuids.Length);
            foreach (var qg in questGuids)
            {
                var qpath = AssetDatabase.GUIDToAssetPath(qg);
                var qd = AssetDatabase.LoadAssetAtPath<QuestData>(qpath);
                if (qd != null)
                    quests.Add(qd);
            }

            // Group by country like Cards index
            Countries[] order = new[] { Countries.International, Countries.France, Countries.Poland };
            var grouped = topics.GroupBy(t => t.Country).ToDictionary(g => g.Key, g => g.ToList());

            var countriesToEmit = new List<Countries>();
            countriesToEmit.AddRange(order.Where(c => grouped.ContainsKey(c)));
            countriesToEmit.AddRange(grouped.Keys.Where(c => !countriesToEmit.Contains(c)));

            foreach (var country in countriesToEmit)
            {
                var list = grouped[country];
                if (list == null || list.Count == 0)
                    continue;

                sb.AppendLine($"## {country}");
                sb.AppendLine();

                foreach (var k in list.OrderBy(x => string.IsNullOrEmpty(x?.Name) ? (string.IsNullOrEmpty(x?.Id) ? x?.name : x.Id) : x.Name, StringComparer.OrdinalIgnoreCase))
                {
                    if (k == null)
                        continue;
                    string kid = string.IsNullOrEmpty(k.Id) ? k.name : k.Id;
                    if (!string.IsNullOrEmpty(kid))
                        sb.AppendLine($"<a id=\"{kid}\"></a>");
                    string heading = !string.IsNullOrEmpty(k.Name) ? EscapeInline(k.Name) : kid;
                    sb.AppendLine($"### {heading}");

                    // Inline meta
                    var meta = new List<string>();
                    if (!string.IsNullOrEmpty(k.Description))
                        meta.Add($"Description: {PublishUtils.EscapeParagraph(k.Description)}");
                    meta.Add($"Importance: {k.Importance}");
                    if (k.Subjects != null && k.Subjects.Count > 0)
                    {
                        var tops = string.Join(", ", k.Subjects.Select(t => t.ToString()));
                        meta.Add($"Subjects: {tops}");
                    }
                    meta.Add($"Target Age: {k.TargetAge}");
                    if (meta.Count > 0)
                        sb.AppendLine("- " + string.Join("  \n- ", meta));

                    // Core card link
                    if (k.CoreCard != null)
                    {
                        var cid = string.IsNullOrEmpty(k.CoreCard.Id) ? k.CoreCard.name : k.CoreCard.Id;
                        sb.AppendLine($"- Core card: [{cid}](../cards/index.md#{Slug(cid)})");
                    }

                    // Connections (sub-list)
                    if (k.Connections != null && k.Connections.Count > 0)
                    {
                        var conns = k.Connections
                            .Where(c => c != null && c.ConnectedCard != null)
                            .Select(c =>
                            {
                                var cid = string.IsNullOrEmpty(c.ConnectedCard.Id) ? c.ConnectedCard.name : c.ConnectedCard.Id;
                                return $"[{cid}](../cards/index.md#{Slug(cid)}) ({c.ConnectionType})";
                            })
                            .ToList();
                        if (conns.Count > 0)
                        {
                            sb.AppendLine("- Connected cards:");
                            foreach (var line in conns)
                                sb.AppendLine("    - " + line);
                        }
                    }

                    // Quests that link to this topic (Title (Code) -> localized quest page)
                    var questLinks = quests
                        .Where(q => q != null && q.Topics != null && q.Topics.Contains(k))
                        .Distinct()
                        .Select(q => new
                        {
                            Title = PublishUtils.GetHumanTitle(q),
                            Code = PublishUtils.GetQuestCode(q),
                            File = PublishUtils.GetQuestPublishFileNameForLocale(q, locale)
                        })
                        .Where(x => !string.IsNullOrEmpty(x.Title) && !string.IsNullOrEmpty(x.Code) && !string.IsNullOrEmpty(x.File))
                        .OrderBy(x => x.Title, StringComparer.OrdinalIgnoreCase)
                        .Select(x => $"[{x.Title} ({x.Code})](../quest/{x.File})")
                        .ToList();
                    if (questLinks.Count > 0)
                        sb.AppendLine("- Quests: " + string.Join(", ", questLinks));

                    if (k.Credits != null && k.Credits.Count > 0)
                    {
                        sb.AppendLine("\nCredits:");
                        foreach (var c in k.Credits.Where(c => c != null && c.Author != null))
                            sb.AppendLine("  - " + PublishUtils.FormatAuthor(c.Author));
                    }

                    sb.AppendLine();
                }
            }

            return sb.ToString();
        }

        private static string Slug(string s)
        {
            if (string.IsNullOrEmpty(s))
                return string.Empty;
            return s.Trim().ToLowerInvariant().Replace(' ', '-');
        }

        private static string EscapeInline(string s)
        {
            if (string.IsNullOrEmpty(s))
                return string.Empty;
            return s.Replace("|", "\\|").Replace("*", "\\*");
        }
    }
}
#endif
