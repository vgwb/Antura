#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

namespace Antura.Discover
{
    public class CardManagementWindow : EditorWindow
    {
        private const string PrefKey_TargetFolder = "Antura.Discover.CardManagement.TargetFolder";
        private const string DefaultTargetFolder = "Assets/_discover/_data/Cards";

        private string targetFolder;
        private bool createMissing = true;
        private bool dryRun = false;
        private bool includeDevRow = true;

        [MenuItem("Antura/Card Management", priority = 22)]
        public static void ShowWindow()
        {
            var wnd = GetWindow<CardManagementWindow>(false, "Card Management", true);
            wnd.minSize = new Vector2(420, 220);
            wnd.Show();
        }

        private void OnEnable()
        {
            targetFolder = EditorPrefs.GetString(PrefKey_TargetFolder, DefaultTargetFolder);
            if (string.IsNullOrEmpty(targetFolder))
                targetFolder = DefaultTargetFolder;
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("CSV Exchange", EditorStyles.boldLabel);
            EditorGUILayout.Space(2);

            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("Target folder for new cards", GUILayout.Width(190));
                targetFolder = EditorGUILayout.TextField(targetFolder);
                if (GUILayout.Button("…", GUILayout.Width(28)))
                {
                    var abs = EditorUtility.OpenFolderPanel("Select target folder", Application.dataPath, "");
                    if (!string.IsNullOrEmpty(abs))
                    {
                        if (abs.StartsWith(Application.dataPath))
                        { targetFolder = "Assets" + abs.Substring(Application.dataPath.Length); }
                        else
                        { Debug.LogWarning("[CardMgmt] Selected folder outside project. Keeping current."); }
                    }
                }
            }

            using (new EditorGUILayout.HorizontalScope())
            {
                createMissing = EditorGUILayout.ToggleLeft("Create missing cards", createMissing, GUILayout.Width(170));
                dryRun = EditorGUILayout.ToggleLeft("Dry run (log only)", dryRun);
            }

            EditorGUILayout.Space(6);
            includeDevRow = EditorGUILayout.ToggleLeft("Include dev row (enum values)", includeDevRow);
            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Export Cards CSV", GUILayout.Height(28)))
                {
                    ExportCards();
                }
                if (GUILayout.Button("Import Cards CSV", GUILayout.Height(28)))
                {
                    ImportCards();
                }
            }

            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("Validation", EditorStyles.boldLabel);
            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Validate Cards Data", GUILayout.Height(26)))
                {
                    ValidateCardsData();
                }
                if (GUILayout.Button("Validate Cards Localization", GUILayout.Height(26)))
                {
                    ValidateCardsLocalization();
                }
            }
            if (GUILayout.Button("Purge Obsolete Translations", GUILayout.Height(26)))
            {
                PurgeObsoleteTranslations();
            }

            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("Card ↔ Quest Links", EditorStyles.boldLabel);
            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Sync Links (bidirectional)", GUILayout.Height(22)))
                {
                    var logs = new List<string>();
                    int changes = Antura.Discover.Editor.CardValidationUtility.SyncCardQuestLinks(true, logs);
                    Debug.Log(string.Join("\n", logs));
                    EditorUtility.DisplayDialog("Sync Links", $"Changes applied: {changes}", "OK");
                }
                if (GUILayout.Button("Unlink Non-Reciprocal", GUILayout.Height(22)))
                {
                    var logs = new List<string>();
                    int changes = Antura.Discover.Editor.CardValidationUtility.UnlinkNonReciprocalCardQuestLinks(true, logs);
                    Debug.Log(string.Join("\n", logs));
                    EditorUtility.DisplayDialog("Unlink Non-Reciprocal", $"Changes applied: {changes}", "OK");
                }
            }
            if (GUILayout.Button("Print Relationships to Console", GUILayout.Height(22)))
            {
                var logs = new List<string>();
                Antura.Discover.Editor.CardValidationUtility.PrintCardQuestRelationships(logs, includeOrphans: true);
                Debug.Log(string.Join("\n", logs));
            }

            // Persist prefs
            if (GUI.changed)
            {
                EditorPrefs.SetString(PrefKey_TargetFolder, targetFolder);
            }
        }

        private void ExportCards()
        {
            var cards = LoadAllCards();
            string dateStr = DateTime.Now.ToString("yyyy-MM-dd");
            string defName = $"Antura Cards - {dateStr}.csv";
            var path = EditorUtility.SaveFilePanel("Export Cards CSV", Application.dataPath, defName, "csv");
            if (string.IsNullOrEmpty(path))
                return;
            CardDataExchangeUtility.ExportCardsCsvToPath(cards, path, includeDevRow: includeDevRow);
            EditorUtility.RevealInFinder(path);
        }

        private void ImportCards()
        {
            var path = EditorUtility.OpenFilePanel("Import Cards CSV", Application.dataPath, "csv");
            if (string.IsNullOrEmpty(path))
                return;

            // Ensure target folder is a valid project path
            string folder = targetFolder;
            if (string.IsNullOrEmpty(folder))
                folder = DefaultTargetFolder;
            if (!folder.StartsWith("Assets"))
            {
                Debug.LogWarning($"[CardMgmt] Target folder must be within Assets. Using default {DefaultTargetFolder}");
                folder = DefaultTargetFolder;
            }

            int n = CardDataExchangeUtility.ImportCardsCsvFromPath(path, createIfMissing: createMissing, createFolder: folder, dryRun: dryRun);
            EditorUtility.DisplayDialog("Import Cards CSV", $"{(dryRun ? "Dry run: would apply" : "Applied")} {n} rows. New assets in: {folder}", "OK");
        }

        private static List<CardData> LoadAllCards()
        {
            var result = new List<CardData>();
            foreach (var guid in AssetDatabase.FindAssets("t:CardData"))
            {
                var p = AssetDatabase.GUIDToAssetPath(guid);
                var c = AssetDatabase.LoadAssetAtPath<CardData>(p);
                if (c != null)
                    result.Add(c);
            }
            result.Sort((a, b) => string.Compare(a?.Id, b?.Id, StringComparison.Ordinal));
            return result;
        }

        private void ValidateCardsData()
        {
            var rpt = Editor.CardValidationUtility.ValidateAllCards(logEachIssue: true);
            EditorUtility.DisplayDialog("Validate Cards Data", rpt.ToString(), "OK");
        }

        private void ValidateCardsLocalization()
        {
            RunLocalizationValidationAndShowDialog();
        }

        private void PurgeObsoleteTranslations()
        {
            var cards = LoadAllCards();
            if (cards == null)
            {
                EditorUtility.DisplayDialog("Purge Obsolete Translations", "No card assets found.", "OK");
                return;
            }

            var expectedKeys = new HashSet<string>(StringComparer.Ordinal);
            foreach (var card in cards)
            {
                if (card == null)
                    continue;

                if (!string.IsNullOrEmpty(card.Id))
                {
                    expectedKeys.Add(card.Id);
                    expectedKeys.Add(card.Id + ".desc");
                }

                if (card.Title != null && !card.Title.IsEmpty)
                {
                    var key = card.Title.TableEntryReference.Key;
                    if (!string.IsNullOrEmpty(key))
                        expectedKeys.Add(key);
                }

                if (card.Description != null && !card.Description.IsEmpty)
                {
                    var key = card.Description.TableEntryReference.Key;
                    if (!string.IsNullOrEmpty(key))
                        expectedKeys.Add(key);
                }
            }

            var locales = LocalizationSettings.AvailableLocales?.Locales;
            if (locales == null || locales.Count == 0)
            {
                EditorUtility.DisplayDialog("Purge Obsolete Translations", "No locales configured in Localization Settings.", "OK");
                return;
            }

            var stringTables = new List<StringTable>();
            foreach (var locale in locales)
            {
                if (locale == null)
                    continue;
                var table = LocalizationSettings.StringDatabase.GetTable("Cards", locale);
                if (table != null && !stringTables.Contains(table))
                    stringTables.Add(table);
            }

            if (stringTables.Count == 0)
            {
                EditorUtility.DisplayDialog("Purge Obsolete Translations", "No 'Cards' string tables found for the available locales.", "OK");
                return;
            }

            var sharedData = stringTables[0].SharedData;
            if (sharedData == null)
            {
                EditorUtility.DisplayDialog("Purge Obsolete Translations", "Shared data for the 'Cards' tables could not be found.", "OK");
                return;
            }

            var entriesToRemove = sharedData.Entries
                .Where(entry => entry != null && !string.IsNullOrEmpty(entry.Key) && !expectedKeys.Contains(entry.Key))
                .ToList();

            if (entriesToRemove.Count == 0)
            {
                EditorUtility.DisplayDialog("Purge Obsolete Translations", "No obsolete localization entries detected.", "OK");
                return;
            }

            int removedCount = 0;
            var dirtyTables = new HashSet<StringTable>();

            foreach (var sharedEntry in entriesToRemove)
            {
                long entryId = sharedEntry.Id;
                foreach (var table in stringTables)
                {
                    if (table.GetEntry(entryId) != null)
                    {
                        table.RemoveEntry(entryId);
                        dirtyTables.Add(table);
                    }
                }
                sharedData.RemoveKey(entryId);
                removedCount++;
            }

            foreach (var table in dirtyTables)
            {
                EditorUtility.SetDirty(table);
            }

            EditorUtility.SetDirty(sharedData);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            EditorUtility.DisplayDialog(
                "Purge Obsolete Translations",
                $"Removed {removedCount} obsolete entr{(removedCount == 1 ? "y" : "ies")} from the 'Cards' tables across {dirtyTables.Count} locale(s).",
                "OK");
        }

        private static void RunLocalizationValidationAndShowDialog()
        {
            var cards = LoadAllCards();

            // Counters
            int createdTitleEntriesEn = 0;
            int createdDescEntriesEn = 0;
            int fixedTitleRefsNew = 0;    // were empty
            int fixedDescRefsNew = 0;     // were empty
            int fixedTitleRefsWrong = 0;  // wrong table/key corrected
            int fixedDescRefsWrong = 0;   // wrong table/key corrected
            int updatedTitleValuesEn = 0; // seeded from TitleEn
            int updatedDescValuesEn = 0;  // seeded from DescriptionEn
            int missingCardsTableLocales = 0; // across checks
            int missingTitleEntriesOtherLocales = 0;
            int missingDescEntriesOtherLocales = 0;

            var locales = LocalizationSettings.AvailableLocales?.Locales ?? new List<UnityEngine.Localization.Locale>();
            int localeCount = locales.Count;

            // Find English locale (en or en-*)
            var enLocale = locales.FirstOrDefault(l => l != null && l.Identifier.Code.ToLowerInvariant().StartsWith("en"));

            foreach (var c in cards)
            {
                if (c == null)
                    continue;
                if (string.IsNullOrEmpty(c.Id))
                {
                    Debug.LogWarning($"[Card Loc Validate] Skipping card without Id: {c.name}", c);
                    continue;
                }

                string titleKey = c.Id;
                string descKey = c.Id + ".desc";

                // 1) Ensure LocalizedString references exist and point to Cards/{expectedKey}
                // Title
                if (c.Title == null || c.Title.IsEmpty)
                {
                    c.Title = new LocalizedString("Cards", titleKey);
                    fixedTitleRefsNew++;
                    EditorUtility.SetDirty(c);
                }
                else
                {
                    bool needsFix = false;
                    try
                    {
                        var tblName = c.Title.TableReference.TableCollectionName;
                        if (!string.Equals(tblName, "Cards", StringComparison.OrdinalIgnoreCase))
                            needsFix = true;
                        // If we can read a key name, ensure it matches expected; if not available, we still rewrite for safety
                        var entryName = c.Title.TableEntryReference.Key;
                        if (!string.Equals(entryName, titleKey, StringComparison.Ordinal))
                            needsFix = true;
                    }
                    catch { needsFix = true; }
                    if (needsFix)
                    {
                        c.Title = new LocalizedString("Cards", titleKey);
                        fixedTitleRefsWrong++;
                        EditorUtility.SetDirty(c);
                    }
                }

                // Description
                if (c.Description == null || c.Description.IsEmpty)
                {
                    c.Description = new LocalizedString("Cards", descKey);
                    fixedDescRefsNew++;
                    EditorUtility.SetDirty(c);
                }
                else
                {
                    bool needsFix = false;
                    try
                    {
                        var tblName = c.Description.TableReference.TableCollectionName;
                        if (!string.Equals(tblName, "Cards", StringComparison.OrdinalIgnoreCase))
                            needsFix = true;
                        var entryName = c.Description.TableEntryReference.Key;
                        if (!string.Equals(entryName, descKey, StringComparison.Ordinal))
                            needsFix = true;
                    }
                    catch { needsFix = true; }
                    if (needsFix)
                    {
                        c.Description = new LocalizedString("Cards", descKey);
                        fixedDescRefsWrong++;
                        EditorUtility.SetDirty(c);
                    }
                }

                // 2) Ensure entries exist in the Cards table(s)
                // English: create/seed entries
                if (enLocale == null)
                {
                    Debug.LogWarning($"[Card Loc Validate] No English locale found. Cannot seed Title/Description for '{c.Id}'.", c);
                }
                else
                {
                    var enTable = LocalizationSettings.StringDatabase.GetTable("Cards", enLocale);
                    if (enTable == null)
                    {
                        Debug.LogWarning($"[Card Loc Validate] Could not find 'Cards' table for locale '{enLocale.Identifier.Code}'.", c);
                        missingCardsTableLocales++;
                        // Fallback: ensure keys exist in any available 'Cards' table so SharedData is updated and persisted
                        var anyTable = locales
                            .Select(loc => LocalizationSettings.StringDatabase.GetTable("Cards", loc))
                            .FirstOrDefault(t => t != null);
                        if (anyTable != null)
                        {
                            if (anyTable.GetEntry(titleKey) == null)
                                anyTable.AddEntry(titleKey, string.Empty);
                            if (anyTable.GetEntry(descKey) == null)
                                anyTable.AddEntry(descKey, string.Empty);
                            EditorUtility.SetDirty(anyTable);
                            if (anyTable.SharedData != null)
                                EditorUtility.SetDirty(anyTable.SharedData);
                        }
                    }
                    else
                    {
                        // Title entry
                        var tEntry = enTable.GetEntry(titleKey);
                        if (tEntry == null)
                        {
                            enTable.AddEntry(titleKey, string.IsNullOrWhiteSpace(c.TitleEn) ? c.name : c.TitleEn);
                            createdTitleEntriesEn++;
                            EditorUtility.SetDirty(enTable);
                            if (enTable.SharedData != null)
                                EditorUtility.SetDirty(enTable.SharedData);
                        }
                        else if (string.IsNullOrEmpty(tEntry.Value) && !string.IsNullOrWhiteSpace(c.TitleEn))
                        {
                            tEntry.Value = c.TitleEn;
                            updatedTitleValuesEn++;
                            EditorUtility.SetDirty(enTable);
                            if (enTable.SharedData != null)
                                EditorUtility.SetDirty(enTable.SharedData);
                        }

                        // Description entry
                        var dEntry = enTable.GetEntry(descKey);
                        if (dEntry == null)
                        {
                            enTable.AddEntry(descKey, string.IsNullOrWhiteSpace(c.DescriptionEn) ? string.Empty : c.DescriptionEn);
                            createdDescEntriesEn++;
                            EditorUtility.SetDirty(enTable);
                            if (enTable.SharedData != null)
                                EditorUtility.SetDirty(enTable.SharedData);
                        }
                        else if (string.IsNullOrEmpty(dEntry.Value) && !string.IsNullOrWhiteSpace(c.DescriptionEn))
                        {
                            dEntry.Value = c.DescriptionEn;
                            updatedDescValuesEn++;
                            EditorUtility.SetDirty(enTable);
                            if (enTable.SharedData != null)
                                EditorUtility.SetDirty(enTable.SharedData);
                        }
                    }
                }

                // Other locales: ensure keys exist in existing tables (create empty entries), and report missing tables
                foreach (var loc in locales)
                {
                    if (loc == null || (enLocale != null && loc.Identifier == enLocale.Identifier))
                        continue;

                    var table = LocalizationSettings.StringDatabase.GetTable("Cards", loc);
                    if (table == null)
                    {
                        missingCardsTableLocales++;
                        continue;
                    }
                    if (table.GetEntry(titleKey) == null)
                    {
                        table.AddEntry(titleKey, string.Empty);
                        missingTitleEntriesOtherLocales++;
                        EditorUtility.SetDirty(table);
                        if (table.SharedData != null)
                            EditorUtility.SetDirty(table.SharedData);
                    }
                    if (table.GetEntry(descKey) == null)
                    {
                        table.AddEntry(descKey, string.Empty);
                        missingDescEntriesOtherLocales++;
                        EditorUtility.SetDirty(table);
                        if (table.SharedData != null)
                            EditorUtility.SetDirty(table.SharedData);
                    }
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            EditorUtility.DisplayDialog(
                "Validate Cards Localization",
                $"Total cards: {cards.Count}\n" +
                $"Locales available: {localeCount}\n" +
                $"Fixed Title refs: {fixedTitleRefsNew + fixedTitleRefsWrong} (created: {fixedTitleRefsNew}, corrected: {fixedTitleRefsWrong})\n" +
                $"Fixed Description refs: {fixedDescRefsNew + fixedDescRefsWrong} (created: {fixedDescRefsNew}, corrected: {fixedDescRefsWrong})\n" +
                $"English entries created — Title: {createdTitleEntriesEn}, Description: {createdDescEntriesEn}\n" +
                $"English entries updated — Title: {updatedTitleValuesEn}, Description: {updatedDescValuesEn}\n" +
                $"Missing 'Cards' table occurrences (all locales): {missingCardsTableLocales}\n" +
                $"Missing entries in other locales — Title: {missingTitleEntriesOtherLocales}, Description: {missingDescEntriesOtherLocales}",
                "OK");
        }
    }
}
#endif
