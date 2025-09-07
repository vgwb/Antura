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

            sb.AppendLine($"Total found: **{topics.Count}**\n");

            var info = "!!! note \"NOTE: connections type can be:\"\n";
            info += "    **Created By** (agent (person/org) who made/discovered/commissioned)  \n";
            info += "    **Located In**       (where it is/was)  \n";
            info += "    **Is A**             (type/category relation (Baguette is a Bread))  \n";
            info += "    **Part Of**         (whole/part relation (Crust is part of Baguette))  \n";
            info += "    **Made Of**          (physical composition)  \n";
            info += "    **Time Context**     (period/event/date)  \n";
            info += "    **Cultural Context** (origin/tradition/symbolism)  \n";
            info += "    **Causal**           (clear cause→effect)  \n";
            info += "    **Purpose**          (used for…)  \n";
            info += "    **Compare**          (compare/Kind similar/contrast/analogy)  \n";
            info += "    **Related To**     (whatever else, last choice...)  \n";
            sb.AppendLine(info + "\n");

            foreach (var country in countriesToEmit)
            {
                var list = grouped[country];
                if (list == null || list.Count == 0)
                    continue;

                sb.AppendLine($"## {country}");
                sb.AppendLine();

                var orderedTopics = list
                    .OrderBy(x => string.IsNullOrEmpty(x?.Name) ? (string.IsNullOrEmpty(x?.Id) ? x?.name : x.Id) : x.Name, StringComparer.OrdinalIgnoreCase)
                    .ToList();

                for (int ti = 0; ti < orderedTopics.Count; ti++)
                {
                    var k = orderedTopics[ti];
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

                    // Core card (expanded with description and image)
                    if (k.CoreCard != null)
                    {
                        var core = k.CoreCard;
                        var cid = string.IsNullOrEmpty(core.Id) ? core.name : core.Id;
                        var cDisplay = GetCardDisplayTitle(core, locale);
                        if (string.IsNullOrEmpty(cDisplay))
                            cDisplay = cid;
                        sb.AppendLine("- Core card:");
                        sb.AppendLine($"    - **[{EscapeInline(cDisplay)}](../cards/index.md#{Slug(cid)})**");
                        string cDesc = PublishUtils.SafeLocalized(core.Description, string.Empty);
                        if (!string.IsNullOrEmpty(cDesc))
                            sb.AppendLine($"    {cDesc}");
                        sb.AppendLine();
                        if (core.ImageAsset != null)
                            sb.AppendLine($"    ![preview {cid}](../../assets/img/discover/cards/{cid}.jpg){{ width=\"200\" }}");
                    }

                    // Connected cards (expanded)
                    if (k.Connections != null && k.Connections.Count > 0)
                    {
                        var validConns = k.Connections.Where(c => c != null && c.ConnectedCard != null).ToList();
                        if (validConns.Count > 0)
                        {
                            sb.AppendLine("- Connected cards:");
                            foreach (var c in validConns)
                            {
                                var card = c.ConnectedCard;
                                var cid = string.IsNullOrEmpty(card.Id) ? card.name : card.Id;
                                var display = GetCardDisplayTitle(card, locale);
                                if (string.IsNullOrEmpty(display))
                                    display = cid;
                                sb.AppendLine($"    - **[{EscapeInline(display)}](../cards/index.md#{Slug(cid)})** ({c.ConnectionType})");
                                string desc = PublishUtils.SafeLocalized(card.Description, string.Empty);
                                if (!string.IsNullOrEmpty(desc))
                                    sb.AppendLine($"    {desc}");
                                sb.AppendLine();
                                if (card.ImageAsset != null)
                                {
                                    sb.AppendLine($"    ![preview {cid}](../../assets/img/discover/cards/{cid}.jpg){{ width=\"200\" }}");
                                    sb.AppendLine();
                                }
                            }
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

                    // Separator between topics as requested
                    if (ti < orderedTopics.Count - 1)
                    {
                        sb.AppendLine();
                        sb.AppendLine("---");
                        sb.AppendLine();
                    }
                    else
                    {
                        sb.AppendLine();
                    }
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

        private static string GetCardDisplayTitle(CardData card, Locale locale)
        {
            if (card == null)
                return string.Empty;
            string title = null;
#if UNITY_EDITOR
            try
            {
                if (card.Title != null)
                {
                    // Ensure locale is active so GetLocalizedString pulls the right value
                    if (locale != null && LocalizationSettings.SelectedLocale != locale)
                    {
                        LocalizationSettings.SelectedLocale = locale;
                    }
                    title = card.Title.GetLocalizedString();
                }
            }
            catch { }
#endif
            if (string.IsNullOrEmpty(title))
                title = card.TitleEn;
            if (string.IsNullOrEmpty(title))
                title = string.IsNullOrEmpty(card.Id) ? card.name : card.Id;
            return title;
        }
    }
}
#endif
