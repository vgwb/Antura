#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization;

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

        [MenuItem("Antura/Discover/Card Management")]
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
            if (GUILayout.Button("Validate Cards", GUILayout.Height(26)))
            {
                ValidateCards();
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

        private void ValidateCards()
        {
            RunValidationAndShowDialog();
        }

        private static void RunValidationAndShowDialog()
        {
            var cards = LoadAllCards();
            var idSet = new HashSet<string>(cards.Where(c => !string.IsNullOrEmpty(c?.Id)).Select(c => c.Id), StringComparer.OrdinalIgnoreCase);

            int missingId = 0;
            int missingLocTitle = 0;
            int missingTopics = 0;
            int missingImage = 0;
            int emptyTitleEn = 0;
            int emptyDescEn = 0;

            foreach (var c in cards)
            {
                if (c == null)
                    continue;

                if (string.IsNullOrWhiteSpace(c.Id))
                {
                    missingId++;
                    Debug.LogWarning($"[Card Validate] Missing ID: {AssetDatabase.GetAssetPath(c)}", c);
                }

                // Localized Title (if present)
                try
                {
                    LocalizedString title = null;
                    var prop = c.GetType().GetProperty("Title");
                    if (prop != null)
                        title = prop.GetValue(c) as LocalizedString;
                    if (title == null || title.IsEmpty)
                    {
                        missingLocTitle++;
                        Debug.LogWarning($"[Card Validate] Missing localized Title: {c.name}", c);
                    }
                }
                catch { }

                if (c.Topics == null || c.Topics.Count == 0)
                { missingTopics++; Debug.LogWarning($"[Card Validate] Missing topics: {c.name}", c); }

                try
                {
                    var img = c.GetType().GetProperty("ImageAsset")?.GetValue(c);
                    if (img == null)
                    { missingImage++; Debug.LogWarning($"[Card Validate] Missing image: {c.name}", c); }
                }
                catch { }

                if (string.IsNullOrWhiteSpace(c.TitleEn))
                { emptyTitleEn++; Debug.LogWarning($"[Card Validate] '{c.Id}' has empty TitleEn", c); }
                if (string.IsNullOrWhiteSpace(c.DescriptionEn))
                { emptyDescEn++; Debug.LogWarning($"[Card Validate] '{c.Id}' has empty DescriptionEn", c); }
            }

            EditorUtility.DisplayDialog(
                "Validate Cards",
                $"Total: {cards.Count}\n" +
                $"Missing Id: {missingId}\n" +
                $"Missing Localized Title: {missingLocTitle}\n" +
                $"Missing Topics: {missingTopics}\n" +
                $"Missing Image: {missingImage}\n" +
                $"TitleEn empty: {emptyTitleEn}\n" +
                $"DescriptionEn empty: {emptyDescEn}\n",
                "OK");
        }
    }
}
#endif
