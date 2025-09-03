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
    public static class LocationsExportUtils
    {
        public static string BuildLocationsIndexMarkdown(Locale locale)
        {
            var sb = new StringBuilder();
            sb.AppendLine("---");
            sb.AppendLine("title: Locations");
            sb.AppendLine("hide:");
            sb.AppendLine("---\n");
            sb.AppendLine("# Locations\n");

            var guids = AssetDatabase.FindAssets("t:WorldData");
            var worlds = new List<WorldData>();
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var k = AssetDatabase.LoadAssetAtPath<WorldData>(path);
                if (k != null)
                    worlds.Add(k);
            }

            // Sort by Id (fallback to name)
            worlds = worlds
                .OrderBy(k => string.IsNullOrEmpty(k?.Id) ? k?.name : k.Id)
                .ToList();

            foreach (var k in worlds)
            {
                if (k == null)
                    continue;
                string kid = string.IsNullOrEmpty(k.Id) ? k.name : k.Id;
                sb.AppendLine($"## {kid}");


                // Inline meta
                var meta = new List<string>();

                if (!string.IsNullOrEmpty(k.Title))
                    meta.Add($"Title: {EscapeInline(k.Title)}");

                if (!string.IsNullOrEmpty(k.Description))
                {
                    meta.Add($"Description: {PublishUtils.EscapeParagraph(k.Description)}");
                }

                if (k.Credits != null && k.Credits.Count > 0)
                {
                    sb.AppendLine("\nCredits:");
                    foreach (var c in k.Credits.Where(c => c != null && c.Author != null))
                        sb.AppendLine("  - " + PublishUtils.FormatAuthor(c.Author));
                }

                sb.AppendLine();
            }

            return sb.ToString();
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
