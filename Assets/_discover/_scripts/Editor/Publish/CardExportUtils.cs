#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization;

namespace Antura.Discover.Editor
{
    public static class CardExportUtils
    {
        public static string BuildCardsIndexMarkdown(Locale locale)
        {
            var sb = new StringBuilder();
            sb.AppendLine("---");
            sb.AppendLine("title: Cards");
            sb.AppendLine("hide:");
            sb.AppendLine("---\n");
            sb.AppendLine("# Cards\n");

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
                    sb.AppendLine($"### {title}");
                    if (!string.IsNullOrEmpty(desc))
                        sb.AppendLine(desc);
                    sb.AppendLine("- Category: " + c.Category);
                    if (c.Topics != null && c.Topics.Count > 0)
                        sb.AppendLine("- Topics: " + string.Join(", ", c.Topics));
                    if (c.Year != 0)
                        sb.AppendLine("- Year: " + c.Year);
                    sb.AppendLine("- Country: " + c.Country);
                    sb.AppendLine("- Cookies: " + c.Cookies);
                    sb.AppendLine("- Points: " + c.Points);
                    if (c.ImageAsset != null)
                        sb.AppendLine("- Image: " + AssetDatabase.GetAssetPath(c.ImageAsset.Image));
                    if (c.AudioAsset != null)
                        sb.AppendLine("- Audio: " + AssetDatabase.GetAssetPath(c.AudioAsset.Audio));
                    if (c.Words != null && c.Words.Count > 0)
                        sb.AppendLine("- Words: " + string.Join(", ", c.Words.Where(w => w != null).Select(w => w.TextEn)));
                    if (c.Quests != null && c.Quests.Count > 0)
                        sb.AppendLine("- Quests: " + string.Join(", ", c.Quests.Where(q => q != null).Select(q => !string.IsNullOrEmpty(q.IdDisplay) ? q.IdDisplay : (string.IsNullOrEmpty(q.Id) ? q.name : q.Id))));
                    sb.AppendLine();
                }
            }

            return sb.ToString();
        }
    }
}
#endif
