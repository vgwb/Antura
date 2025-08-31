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
    public static class KnowledgeExportUtils
    {
        public static string BuildKnowledgeIndexMarkdown(Locale locale)
        {
            var sb = new StringBuilder();
            sb.AppendLine("---");
            sb.AppendLine("title: Knowledge");
            sb.AppendLine("hide:");
            sb.AppendLine("---\n");
            sb.AppendLine("# Knowledge\n");

            var guids = AssetDatabase.FindAssets("t:KnowledgeData");
            var knowledges = new List<KnowledgeData>();
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var k = AssetDatabase.LoadAssetAtPath<KnowledgeData>(path);
                if (k != null)
                    knowledges.Add(k);
            }

            // Sort by Id (fallback to name)
            knowledges = knowledges
                .OrderBy(k => string.IsNullOrEmpty(k?.Id) ? k?.name : k.Id)
                .ToList();

            foreach (var k in knowledges)
            {
                if (k == null)
                    continue;
                string kid = string.IsNullOrEmpty(k.Id) ? k.name : k.Id;
                sb.AppendLine($"## {kid}");

                // Description


                // Inline meta
                var meta = new List<string>();

                if (!string.IsNullOrEmpty(k.Name))
                    meta.Add($"Name: {EscapeInline(k.Name)}");

                if (!string.IsNullOrEmpty(k.Description))
                {
                    meta.Add($"Description: {EscapeParagraph(k.Description)}");
                }

                meta.Add($"Importance: {k.Importance}");

                if (k.Topics != null && k.Topics.Count > 0)
                {
                    var tops = string.Join(", ", k.Topics.Select(t => t.ToString()));
                    meta.Add($"Topics: {tops}");
                }
                meta.Add($"TargetAge: {k.targetAge}");
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
                        .Where(c => c != null && c.connectedCard != null)
                        .Select(c =>
                        {
                            var cid = string.IsNullOrEmpty(c.connectedCard.Id) ? c.connectedCard.name : c.connectedCard.Id;
                            return $"[{cid}](../cards/index.md#{Slug(cid)}) ({c.connectionType})";
                        })
                        .ToList();
                    if (conns.Count > 0)
                    {
                        sb.AppendLine("- Connections:");
                        foreach (var line in conns)
                            sb.AppendLine("    - " + line);
                    }
                }



                sb.AppendLine();
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

        private static string EscapeParagraph(string s)
        {
            if (string.IsNullOrEmpty(s))
                return string.Empty;
            return s.Replace("\r\n", "\n").Replace("\r", "\n");
        }
    }
}
#endif
