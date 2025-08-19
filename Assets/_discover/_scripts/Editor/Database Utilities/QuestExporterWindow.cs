#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Antura.Discover;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace Antura.Discover.EditorUI
{
    public class QuestExporterWindow : EditorWindow
    {
        private Locale selectedLocale;
        private bool includeOnlyPublic = true;
        private Countries filterCountry = Countries.Global; // Default to Global as hint; 'All' not available, so use toggle below
        private bool filterByCountry = false;

        [MenuItem("Antura/Discover/Quest Exporter", priority = 290)]
        public static void Open()
        {
            GetWindow<QuestExporterWindow>(title: "Quest Exporter").Show();
        }

        private void OnEnable()
        {
            // Try default selected locale
            try
            { selectedLocale = LocalizationSettings.SelectedLocale; }
            catch { }
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Quest Exporter", EditorStyles.boldLabel);
            EditorGUILayout.Space(4);

            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("Locale", GUILayout.Width(80));
                EditorGUI.BeginChangeCheck();
                selectedLocale = LocalePopup(selectedLocale);
                if (EditorGUI.EndChangeCheck())
                {
                    Repaint();
                }
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
                if (GUILayout.Button("Export to Folder"))
                {
                    ExportFilteredToFolder();
                }
                if (GUILayout.Button("Publish All to Docs"))
                {
                    PublishAllToDocs();
                }
            }

            EditorGUILayout.HelpBox("Select one or more QuestData assets in the Project window, choose a Locale, then Export. 'Publish All' writes markdown to docs/manual/quests.", MessageType.Info);
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
            // Export all quests matching current filters to a chosen folder
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

            Antura.Discover.QuestExportUtils.WithLocale(selectedLocale, () =>
            {
                foreach (var q in quests)
                {
                    if (includeOnlyPublic && !q.IsPublic)
                        continue;
                    if (filterByCountry && q.Country != filterCountry)
                        continue;
                    try
                    {
                        var md = Antura.Discover.QuestExportUtils.BuildQuestMarkdown(q); // single-file export: no language menu
                        var fileName = Antura.Discover.QuestExportUtils.GetQuestPublishFileNameForLocale(q, selectedLocale);
                        var path = Path.Combine(folder, fileName);
                        File.WriteAllText(path, md);
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"[QuestExporter] Failed to export '{q?.Id}': {ex.Message}");
                    }
                }
            });

            EditorUtility.RevealInFinder(folder);
        }

        [MenuItem("Antura/Discover/Publish Quests Website", priority = 300)]
        public static void PublishAllToDocs()
        {
            // Always publish all quests in four languages, ignoring UI filters
            string projectRoot = Directory.GetParent(Application.dataPath).FullName;
            string folder = Path.Combine(projectRoot, "docs", "quest");
            Directory.CreateDirectory(folder);

            // Clean existing .md files
            try
            {
                foreach (var file in Directory.GetFiles(folder, "*.md", SearchOption.TopDirectoryOnly))
                {
                    File.Delete(file);
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[QuestExporter] Could not clean folder: {ex.Message}");
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

            // Languages to publish
            var publishLangs = new[] { "en", "fr", "pl", "it" };
            int ok = 0, fail = 0;

            // Country order and label for indexes
            Countries[] order = new[] {
                Countries.France, Countries.Poland, Countries.Germany, Countries.Greece,
                Countries.Italy, Countries.Portugal, Countries.Spain, Countries.UnitedKingdom,
                Countries.Global
            };
            Func<Countries, string> countryLabel = c =>
            {
                var s = c.ToString();
                if (s == nameof(Countries.UnitedKingdom))
                    return "United Kingdom";
                return s;
            };

            // For each language, publish all quest files and one localized index
            foreach (var lang in publishLangs)
            {
                var locale = FindLocaleByCode(lang);
                var grouped = new Dictionary<Countries, List<KeyValuePair<string, string>>>();

                Antura.Discover.QuestExportUtils.WithLocale(locale, () =>
                {
                    foreach (var q in quests)
                    {
                        if (!q.IsPublic)
                            continue; // web publish: only public quests
                        try
                        {
                            string fileName = Antura.Discover.QuestExportUtils.GetQuestPublishFileNameForLocale(q, locale);
                            string outPath = Path.Combine(folder, fileName);

                            // Generate separate script page if script is public
                            string scriptFileName = null;
                            if (q.IsScriptPublic)
                            {
                                scriptFileName = Antura.Discover.QuestExportUtils.GetQuestScriptPublishFileNameForLocale(q, locale);
                                string scriptPath = Path.Combine(folder, scriptFileName);
                                var scriptMd = Antura.Discover.QuestExportUtils.BuildQuestScriptMarkdown(q, includeLanguageMenu: true, locale: locale);
                                File.WriteAllText(scriptPath, scriptMd);
                            }

                            var md = Antura.Discover.QuestExportUtils.BuildQuestMarkdown(q, includeLanguageMenu: true, scriptPageFileName: scriptFileName, locale: locale); // include language menu and link to script page
                            File.WriteAllText(outPath, md);

                            var title = q.TitleText;
                            if (string.IsNullOrEmpty(title) || string.IsNullOrWhiteSpace(title))
                            {
                                title = string.IsNullOrEmpty(q.Id) ? q.name : q.Id;
                            }
                            title = title?.Replace("\r", " ").Replace("\n", " ");
                            var code = Antura.Discover.QuestExportUtils.GetQuestCode(q);
                            var linkText = string.IsNullOrEmpty(title) ? code : ($"{code} - {title}");
                            if (!grouped.TryGetValue(q.Country, out var list))
                            { list = new List<KeyValuePair<string, string>>(); grouped[q.Country] = list; }
                            list.Add(new KeyValuePair<string, string>(linkText, fileName));
                            ok++;
                        }
                        catch (Exception ex)
                        {
                            Debug.LogError($"[QuestExporter] Failed publishing '{q?.Id}' ({lang}): {ex.Message}");
                            fail++;
                        }
                    }
                });

                // Write localized index for this language
                try
                {
                    var indexSb = new System.Text.StringBuilder();
                    indexSb.AppendLine("---");
                    indexSb.AppendLine("title: Antura Discover Quests");
                    indexSb.AppendLine("hide:");
                    indexSb.AppendLine("  - navigation");
                    indexSb.AppendLine("---");
                    indexSb.AppendLine();

                    // H1 and language menu on index
                    indexSb.AppendLine("# Antura Discover Quests");
                    // Disable link for the current language
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
                        {
                            indexSb.AppendLine($"- [{kv.Key}](./{kv.Value})");
                        }
                        indexSb.AppendLine();
                    }

                    string indexName = lang.StartsWith("en") ? "index.md" : $"index.{lang}.md";
                    string indexPath = Path.Combine(folder, indexName);
                    File.WriteAllText(indexPath, indexSb.ToString());
                }
                catch (Exception ex)
                {
                    Debug.LogError($"[QuestExporter] Failed to write index for {lang}: {ex.Message}");
                }
            }

            Debug.Log($"[QuestExporter] Published {ok} quest files to {folder}. Failures: {fail}");
            EditorUtility.RevealInFinder(folder);
        }

        private static Locale FindLocaleByCode(string code)
        {
            try
            {
                var locales = LocalizationSettings.AvailableLocales?.Locales;
                if (locales == null || locales.Count == 0)
                    return null;
                // Prefer exact code match, then startswith (e.g., en matches en-GB)
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
            public void Dispose()
            {
                GUI.enabled = prev;
            }
        }
    }

}
#endif
