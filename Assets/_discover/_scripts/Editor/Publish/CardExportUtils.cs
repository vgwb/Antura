#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization;
using System.Reflection;

namespace Antura.Discover.Editor
{
    public static class CardExportUtils
    {
        public static string BuildCardsIndexMarkdown(Locale locale)
        {
            // Language switcher like other sections
            var lang = PublishUtils.GetLanguageCode(locale);


            var sb = new StringBuilder();
            sb.AppendLine("---");
            sb.AppendLine("title: Cards (" + lang + ")");
            sb.AppendLine("hide:");
            sb.AppendLine("---\n");
            sb.AppendLine("# Cards(" + lang + ")\n");

            var langs = new (string code, string label)[] { ("en", "english"), ("fr", "french"), ("pl", "polish"), ("it", "italian") };
            var parts = new List<string>(langs.Length);
            foreach (var l in langs)
            {
                bool isCurrent = !string.IsNullOrEmpty(lang) ? lang.StartsWith(l.code, StringComparison.OrdinalIgnoreCase) : l.code == "en";
                string file = l.code == "en" ? "index.md" : $"index.{l.code}.md";
                parts.Add(isCurrent ? l.label : $"[{l.label}](./{file})");
            }
            sb.AppendLine($"Language: {string.Join(" - ", parts)}\n");

            var googlelink = "https://docs.google.com/spreadsheets/d/1M3uOeqkbE4uyDs5us5vO-nAFT8Aq0LGBxjjT_CSScWw/edit?gid=415931977#gid=415931977";
            var editInfo = "!!! note \"Educators: help improving these cards!\"" + "\n";
            editInfo += $"    **Improve translations**: [comment here]({googlelink})  " + "\n";
            sb.AppendLine(editInfo);

            var guids = AssetDatabase.FindAssets("t:CardData");
            var cards = new List<CardData>();
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var c = AssetDatabase.LoadAssetAtPath<CardData>(path);
                if (c != null)
                    cards.Add(c);
            }

            if (cards.Count == 0)
            {
                sb.AppendLine("(No cards found)");
                return sb.ToString();
            }

            sb.AppendLine($"Total found:** {cards.Count}**\n");

            // Group by desired countries: International, France, Poland, then others
            Countries[] order = new[] { Countries.International, Countries.France, Countries.Poland };
            var grouped = cards.GroupBy(c => c.Country).ToDictionary(g => g.Key, g => g.ToList());

            foreach (var country in order)
            {
                if (!grouped.TryGetValue(country, out var list) || list == null || list.Count == 0)
                    continue;
                sb.AppendLine($"## {country}");
                sb.AppendLine();
                foreach (var c in list.OrderBy(x => PublishUtils.SafeLocalized(x.Title, x.name), StringComparer.OrdinalIgnoreCase))
                {
                    string title = PublishUtils.SafeLocalized(c.Title, c.name);
                    string desc = PublishUtils.SafeLocalized(c.Description, string.Empty);
                    string cId = !string.IsNullOrEmpty(c.Id) ? c.Id : c.name;
                    if (!string.IsNullOrEmpty(cId))
                        sb.AppendLine($"<a id=\"{cId}\"></a>");
                    sb.AppendLine($"### {title}");

                    if (c.ImageAsset != null)
                        sb.AppendLine("![preview " + cId + "](../../assets/img/discover/cards/" + cId + ".jpg)\n");

                    if (!string.IsNullOrEmpty(desc))
                        sb.AppendLine(desc + "\n");

                    if (!string.IsNullOrEmpty(c.Rationale))
                    {
                        sb.AppendLine($"- Rationale: {PublishUtils.EscapeParagraph(c.Rationale)}");
                    }

                    sb.AppendLine("- Type: " + c.Type);
                    if (c.Subjects != null && c.Subjects.Count > 0)
                        sb.AppendLine("- Subjects: " + string.Join(", ", c.Subjects));
                    if (c.Year != 0)
                        sb.AppendLine("- Year: " + c.Year);
                    sb.AppendLine("- Country: " + c.Country);
                    // if (c.ImageAsset != null)
                    //     sb.AppendLine("- Image: " + AssetDatabase.GetAssetPath(c.ImageAsset.Image));
                    // if (c.AudioAsset != null)
                    //     sb.AppendLine("- Audio: " + AssetDatabase.GetAssetPath(c.AudioAsset.Audio));
                    if (c.Words != null && c.Words.Count > 0)
                    {
                        var wordLinks = c.Words
                            .Where(w => w != null)
                            .Select(w => string.IsNullOrEmpty(w.Id) ? w.name : w.Id)
                            .Where(id => !string.IsNullOrEmpty(id))
                            .Select(id => $"[{id}](../words/{id}.md)");
                        sb.AppendLine("- Words: " + string.Join(", ", wordLinks));
                    }

                    if (c.Quests != null && c.Quests.Count > 0)
                    {
                        var questLinks = c.Quests
                            .Where(q => q != null)
                            .Select(q =>
                            {
                                var qTitle = PublishUtils.GetHumanTitle(q);
                                var qCode = PublishUtils.GetQuestCode(q);
                                var qFile = PublishUtils.GetQuestPublishFileNameForLocale(q, locale);
                                return $"[{qTitle} ({qCode})](../quest/{qFile})";
                            });
                        sb.AppendLine("- Quests: " + string.Join(", ", questLinks));
                    }
                    sb.AppendLine();
                }
            }

            return sb.ToString();
        }

        // Local helpers mirroring QuestExportUtils minimal needs
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
    }
}
#endif
