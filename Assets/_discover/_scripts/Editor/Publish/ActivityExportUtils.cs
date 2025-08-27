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
    public static class ActivityExportUtils
    {
        public static string BuildActivitiesIndexMarkdown(Locale locale)
        {
            var sb = new StringBuilder();
            sb.AppendLine("---");
            sb.AppendLine("title: Activities");
            sb.AppendLine("hide:");
            sb.AppendLine("---\n");
            sb.AppendLine("# Activities\n");

            var guids = AssetDatabase.FindAssets("t:ActivityData");
            var acts = new List<ActivityData>();
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var a = AssetDatabase.LoadAssetAtPath<ActivityData>(path);
                if (a != null)
                    acts.Add(a);
            }

            if (acts.Count == 0)
            {
                sb.AppendLine("(No activities found)");
                return sb.ToString();
            }

            foreach (var a in acts.OrderBy(x => x.Code.ToString(), StringComparer.OrdinalIgnoreCase))
            {
                string name = PublishUtils.SafeLocalized(a.Name, a.name);
                sb.AppendLine($"## {name} ({a.Code})");
                sb.AppendLine();
                sb.AppendLine("- Code: " + a.Code);
                sb.AppendLine("- Image: " + (a.Image ? AssetDatabase.GetAssetPath(a.Image) : ""));
                if (a.Category != null && a.Category.Count > 0)
                    sb.AppendLine("- Categories: " + string.Join(", ", a.Category));
                if (a.Skills != null && a.Skills.Count > 0)
                    sb.AppendLine("- Skills: " + string.Join(", ", a.Skills));
                sb.AppendLine("- Prefab: " + (a.ActivityPrefab ? AssetDatabase.GetAssetPath(a.ActivityPrefab) : ""));
                if (a.Credits != null && a.Credits.Count > 0)
                {
                    sb.AppendLine("- Credits:");
                    foreach (var c in a.Credits.Where(c => c != null && c.Author != null))
                        sb.AppendLine("  - " + PublishUtils.FormatAuthor(c.Author));
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}
#endif
