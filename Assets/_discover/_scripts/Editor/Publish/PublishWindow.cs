#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace Antura.Discover.Editor
{
    public class PublishWindow : EditorWindow
    {
        private Locale selectedLocale;
        private bool includeOnlyPublic = true;
        private Countries filterCountry = Countries.International;
        private bool filterByCountry = false;

        [MenuItem("Antura/Discover/Export Window ", priority = 290)]
        public static void Open()
        {
            GetWindow<PublishWindow>(title: "Export").Show();
        }

        private void OnEnable()
        {
            try
            { selectedLocale = LocalizationSettings.SelectedLocale; }
            catch { }
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Publish website", EditorStyles.boldLabel);
            EditorGUILayout.Space(4);

            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("Locale", GUILayout.Width(80));
                EditorGUI.BeginChangeCheck();
                selectedLocale = LocalePopup(selectedLocale);
                if (EditorGUI.EndChangeCheck())
                    Repaint();
            }

            includeOnlyPublic = EditorGUILayout.ToggleLeft("Include only public quests", includeOnlyPublic);

            using (new EditorGUILayout.HorizontalScope())
            {
                filterByCountry = EditorGUILayout.ToggleLeft("Filter by Country", filterByCountry, GUILayout.Width(140));
                using (new GUIEnabledScope(filterByCountry))
                {
                    filterCountry = (Countries)EditorGUILayout.EnumPopup(filterCountry);
                }
            }

            EditorGUILayout.Space(8);

            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Export Quests to Folder"))
                {
                    ExportFilteredToFolder();
                }
                if (GUILayout.Button("Publish All to Docs"))
                {
                    PublishAllToDocs();
                }
            }

            EditorGUILayout.HelpBox("Select QuestData assets in Project, choose Locale, then Export. 'Publish All' writes markdown into docs.", MessageType.Info);
        }

        private static Locale LocalePopup(Locale current)
        {
            var locales = LocalizationSettings.AvailableLocales?.Locales;
            if (locales == null || locales.Count == 0)
            {
                EditorGUILayout.LabelField("No Locales configured");
                return current;
            }
            int idx = Mathf.Max(0, locales.IndexOf(current));
            var names = locales.Select(l => l?.Identifier.ToString() ?? l?.name ?? "").ToArray();
            idx = EditorGUILayout.Popup(idx, names);
            return locales[Mathf.Clamp(idx, 0, locales.Count - 1)];
        }

        private void ExportFilteredToFolder()
        {
            var guids = AssetDatabase.FindAssets("t:QuestData");
            var quests = new List<QuestData>();
            foreach (var guid in guids)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                var q = AssetDatabase.LoadAssetAtPath<QuestData>(assetPath);
                if (q != null)
                    quests.Add(q);
            }
            string folder = EditorUtility.SaveFolderPanel("Export Quest Markdown Files", Application.dataPath, "QuestExports");
            if (string.IsNullOrEmpty(folder))
                return;

            PublishUtils.WithLocale(selectedLocale, () =>
            {
                foreach (var q in quests)
                {
                    if (includeOnlyPublic && !q.IsPublic)
                        continue;
                    if (filterByCountry && q.Country != filterCountry)
                        continue;
                    try
                    {
                        var md = QuestExportUtils.BuildQuestMarkdown(q);
                        var fileName = PublishUtils.GetQuestPublishFileNameForLocale(q, selectedLocale);
                        var path = Path.Combine(folder, fileName);
                        File.WriteAllText(path, md);
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"[Publish] Failed to export '{q?.Id}': {ex.Message}");
                    }
                }
            });

            EditorUtility.RevealInFinder(folder);
        }

        [MenuItem("Antura/Discover/Publish Website", priority = 300)]
        public static void PublishAllToDocs()
        {
            string projectRoot = Directory.GetParent(Application.dataPath).FullName;
            string discoverRoot = Path.Combine(projectRoot, "docs", "discover");
            string folder = Path.Combine(discoverRoot, "quest");
            Directory.CreateDirectory(discoverRoot);
            Directory.CreateDirectory(folder);

            try
            {
                foreach (var file in Directory.GetFiles(folder, "*.md", SearchOption.TopDirectoryOnly))
                {
                    // Quest folder is fully generated; safe to clear all .md files
                    File.Delete(file);
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[Publish] Could not clean folder: {ex.Message}");
            }

            var guids = AssetDatabase.FindAssets("t:QuestData");
            var quests = new List<QuestData>();
            foreach (var guid in guids)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                var q = AssetDatabase.LoadAssetAtPath<QuestData>(assetPath);
                if (q != null)
                    quests.Add(q);
            }

            var publishLangs = new[] { "en", "fr", "pl", "it" };
            int ok = 0, fail = 0;

            Countries[] order = new[] {
                Countries.France, Countries.Poland, Countries.Germany, Countries.Greece,
                Countries.Italy, Countries.Portugal, Countries.Spain, Countries.UnitedKingdom,
                Countries.International
            };
            Func<Countries, string> countryLabel = c => c == Countries.UnitedKingdom ? "United Kingdom" : c.ToString();

            foreach (var lang in publishLangs)
            {
                var locale = FindLocaleByCode(lang);
                var grouped = new Dictionary<Countries, List<KeyValuePair<string, string>>>();

                PublishUtils.WithLocale(locale, () =>
                {
                    foreach (var q in quests)
                    {
                        if (!q.IsPublic)
                            continue;
                        try
                        {
                            string fileName = PublishUtils.GetQuestPublishFileNameForLocale(q, locale);
                            string outPath = Path.Combine(folder, fileName);

                            string scriptFileName = null;
                            if (q.IsScriptPublic)
                            {
                                scriptFileName = PublishUtils.GetQuestScriptPublishFileNameForLocale(q, locale);
                                string scriptPath = Path.Combine(folder, scriptFileName);
                                var scriptMd = QuestExportUtils.BuildQuestScriptMarkdown(q, includeLanguageMenu: true, locale: locale);
                                File.WriteAllText(scriptPath, scriptMd);
                            }

                            var md = QuestExportUtils.BuildQuestMarkdown(q, includeLanguageMenu: true, scriptPageFileName: scriptFileName, locale: locale);
                            File.WriteAllText(outPath, md);

                            var title = PublishUtils.GetHumanTitle(q)?.Replace("\r", " ").Replace("\n", " ");
                            var code = PublishUtils.GetQuestCode(q);
                            var linkText = string.IsNullOrEmpty(title) ? code : ($"{title} ({code})");
                            if (!grouped.TryGetValue(q.Country, out var list))
                            { list = new List<KeyValuePair<string, string>>(); grouped[q.Country] = list; }
                            list.Add(new KeyValuePair<string, string>(linkText, fileName));
                            ok++;
                        }
                        catch (Exception ex)
                        {
                            Debug.LogError($"[Publish] Failed publishing '{q?.Id}' ({lang}): {ex.Message}");
                            fail++;
                        }
                    }
                });

                try
                {
                    var indexSb = new System.Text.StringBuilder();
                    indexSb.AppendLine("---");
                    indexSb.AppendLine("title: Quests");
                    indexSb.AppendLine("hide:");
                    // indexSb.AppendLine("  - navigation");
                    indexSb.AppendLine("---");
                    indexSb.AppendLine();
                    indexSb.AppendLine("# Quests");
                    var langs = new (string code, string label)[] { ("en", "english"), ("fr", "french"), ("pl", "polish"), ("it", "italian") };
                    var parts = new List<string>(langs.Length);
                    foreach (var l in langs)
                    {
                        bool isCurrent = lang.StartsWith(l.code, StringComparison.OrdinalIgnoreCase);
                        string file = l.code == "en" ? "index.md" : $"index.{l.code}.md";
                        parts.Add(isCurrent ? l.label : $"[{l.label}](./{file})");
                    }
                    indexSb.AppendLine($"Language: {string.Join(" - ", parts)}");
                    indexSb.AppendLine();

                    foreach (var c in order)
                    {
                        if (!grouped.TryGetValue(c, out var list) || list == null || list.Count == 0)
                            continue;
                        indexSb.AppendLine($"## {countryLabel(c)}");
                        indexSb.AppendLine();
                        foreach (var kv in list.OrderBy(k => k.Key, StringComparer.OrdinalIgnoreCase))
                            indexSb.AppendLine($"- [{kv.Key}](./{kv.Value})");
                        indexSb.AppendLine();
                    }

                    string indexName = lang.StartsWith("en") ? "index.md" : $"index.{lang}.md";
                    string indexPath = Path.Combine(folder, indexName);
                    File.WriteAllText(indexPath, indexSb.ToString());
                }
                catch (Exception ex)
                {
                    Debug.LogError($"[Publish] Failed to write index for {lang}: {ex.Message}");
                }
            }

            Debug.Log($"[Publish] Published {ok} quest files to {folder}. Failures: {fail}");

            try
            {
                var en = FindLocaleByCode("en");
                string activitiesDir = Path.Combine(discoverRoot, "activities");
                string cardsDir = Path.Combine(discoverRoot, "cards");
                string wordsDir = Path.Combine(discoverRoot, "words");
                Directory.CreateDirectory(activitiesDir);
                Directory.CreateDirectory(cardsDir);
                Directory.CreateDirectory(wordsDir);

                PublishUtils.WithLocale(en, () =>
                {
                    var activitiesMd = ActivityExportUtils.BuildActivitiesIndexMarkdown(en);
                    File.WriteAllText(Path.Combine(activitiesDir, "index.md"), activitiesMd);

                    var cardsMd = CardExportUtils.BuildCardsIndexMarkdown(en);
                    File.WriteAllText(Path.Combine(cardsDir, "index.md"), cardsMd);

                    var wordsMd = WordExportUtils.BuildWordsIndexMarkdown(en);
                    File.WriteAllText(Path.Combine(wordsDir, "index.md"), wordsMd);
                });
            }
            catch (Exception ex)
            {
                Debug.LogError($"[Publish] Failed generating Activities/Cards/Words indexes: {ex.Message}");
            }

            EditorUtility.RevealInFinder(folder);
        }

        private static Locale FindLocaleByCode(string code)
        {
            try
            {
                var locales = LocalizationSettings.AvailableLocales?.Locales;
                if (locales == null || locales.Count == 0)
                    return null;
                var exact = locales.FirstOrDefault(l => string.Equals(l?.Identifier.Code, code, StringComparison.OrdinalIgnoreCase));
                if (exact != null)
                    return exact;
                return locales.FirstOrDefault(l => l?.Identifier.Code != null && l.Identifier.Code.StartsWith(code, StringComparison.OrdinalIgnoreCase));
            }
            catch { return null; }
        }

        private readonly struct GUIEnabledScope : IDisposable
        {
            private readonly bool prev;
            public GUIEnabledScope(bool enabled)
            {
                prev = GUI.enabled;
                GUI.enabled = enabled;
            }
            public void Dispose() { GUI.enabled = prev; }
        }
    }
}
#endif
