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
    public static class WordExportUtils
    {
        public static string BuildWordsIndexMarkdown(Locale locale)
        {
            var sb = new StringBuilder();
            sb.AppendLine("---");
            sb.AppendLine("title: Words");
            sb.AppendLine("hide:");
            sb.AppendLine("---\n");
            sb.AppendLine("# Words\n");

            var guids = AssetDatabase.FindAssets("t:WordData");
            var words = new List<WordData>();
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var w = AssetDatabase.LoadAssetAtPath<WordData>(path);
                if (w != null)
                    words.Add(w);
            }

            if (words.Count == 0)
            {
                sb.AppendLine("(No words found)");
                return sb.ToString();
            }

            foreach (var w in words.OrderBy(x => x.TextEn, StringComparer.OrdinalIgnoreCase))
            {
                string text = !string.IsNullOrEmpty(w.TextEn) ? w.TextEn : w.name;
                sb.AppendLine($"## {text}");
                sb.AppendLine("- Active: " + w.Active);
                sb.AppendLine("- DevStatus: " + w.DevStatus);
                sb.AppendLine("- Kind: " + w.Kind);
                sb.AppendLine("- Category: " + w.Category);
                sb.AppendLine("- Form: " + w.Form);
                if (!string.IsNullOrEmpty(w.Value))
                    sb.AppendLine("- Value: " + w.Value);
                if (!string.IsNullOrEmpty(w.SortValue))
                    sb.AppendLine("- SortValue: " + w.SortValue);
                if (!string.IsNullOrEmpty(w.DrawingUnicode))
                    sb.AppendLine("- DrawingUnicode: " + w.DrawingUnicode);
                if (!string.IsNullOrEmpty(w.DrawingValue))
                    sb.AppendLine("- DrawingValue: " + w.DrawingValue);
                if (!string.IsNullOrEmpty(w.DrawingAtlas))
                    sb.AppendLine("- DrawingAtlas: " + w.DrawingAtlas);
                if (w.Drawing)
                    sb.AppendLine("- Drawing: " + AssetDatabase.GetAssetPath(w.Drawing));
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}
#endif
