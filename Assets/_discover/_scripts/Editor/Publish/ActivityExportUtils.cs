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
                if (a != null && a.Status != Status.Standby && a.Status != Status.Draft)
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
                sb.AppendLine($"<a id=\"{a.Code}\"></a>");
                sb.AppendLine($"## {name}");
                sb.AppendLine();

                sb.AppendLine($"![preview](../../../assets/img/content/activities/activity_{a.Code}.jpg)");

                string description = PublishUtils.SafeLocalized(a.Description, string.Empty);
                if (!string.IsNullOrEmpty(description))
                    sb.AppendLine(description + "\n");

                if (a.Category != null && a.Category.Count > 0)
                {
                    var cats = a.Category.Where(c => c != 0).ToArray();
                    if (cats.Length > 0)
                    {
                        sb.AppendLine("Categories:");
                        sb.AppendLine();
                        foreach (var c in cats)
                        {
                            var title = LocalizeCategoryTitle(c, locale);
                            var desc = LocalizeCategoryDesc(c, locale);
                            if (!string.IsNullOrEmpty(desc))
                                sb.AppendLine($"  - **{title}**: {desc}");
                            else
                                sb.AppendLine($"  - **{title}**");
                        }
                    }
                }
                if (a.Skills != null && a.Skills.Count > 0)
                {
                    var skills = a.Skills
                        .Where(s => s != 0)
                        .ToArray();
                    if (skills.Length > 0)
                    {
                        sb.AppendLine();
                        sb.AppendLine("Skills:");
                        sb.AppendLine();
                        foreach (var sk in skills)
                        {
                            var title = LocalizeSkillTitle(sk, locale);
                            var desc = LocalizeSkillDesc(sk, locale);
                            if (!string.IsNullOrEmpty(desc))
                                sb.AppendLine($"  - **{title}**: {desc}");
                            else
                                sb.AppendLine($"  - **{title}**");
                        }
                    }
                }

                if (a.AdditionalResources != null && !string.IsNullOrEmpty(a.AdditionalResources.text))
                {
                    sb.AppendLine();
                    sb.AppendLine(a.AdditionalResources.text);
                }


                if (a.Credits != null && a.Credits.Count > 0)
                {
                    sb.AppendLine("\nCredits:");
                    foreach (var c in a.Credits.Where(c => c != null && c.Author != null))
                        sb.AppendLine("  - " + PublishUtils.FormatAuthor(c.Author));
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }

        private static string LocalizeActivities(string key, Locale locale, string fallback)
        {
            try
            {
                if (LocalizationSettings.Instance == null || LocalizationSettings.StringDatabase == null)
                    return fallback ?? key;
                var value = LocalizationSettings.StringDatabase.GetLocalizedString("Activities", key, locale);
                if (string.IsNullOrEmpty(value))
                    return fallback ?? key;
                return value;
            }
            catch
            {
                return fallback ?? key;
            }
        }

        private static string LocalizeCategoryTitle(ActivityCategory c, Locale locale)
            => LocalizeActivities($"ActivityCategory_{c}", locale, c.ToString());

        private static string LocalizeSkillTitle(ActivitySkill s, Locale locale)
            => LocalizeActivities($"ActivitySkills_{s}", locale, s.ToString());

        private static string LocalizeCategoryDesc(ActivityCategory c, Locale locale)
            => LocalizeActivities($"ActivityCategory_{c}.Desc", locale, null);

        private static string LocalizeSkillDesc(ActivitySkill s, Locale locale)
            => LocalizeActivities($"ActivitySkills_{s}.Desc", locale, null);
    }
}
#endif
