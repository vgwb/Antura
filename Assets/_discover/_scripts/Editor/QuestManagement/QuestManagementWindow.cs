#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Antura.Discover;
using UnityEditor.IMGUI.Controls;
using Antura.Discover.Activities; // ActivitySettingsAbstract

namespace Antura.Discover.EditorTools
{
    public class QuestManagementWindow : EditorWindow
    {
        private Vector2 _scroll;
        private string _search = string.Empty;
        private List<QuestData> _quests = new List<QuestData>();
        private int _selectedQuestIndex = 0; // Direct index into _quests (single quest mode)
        private List<CardData> _allCards = new List<CardData>();
        private List<AssetData> _allAssets = new List<AssetData>();
        private SearchField _searchField;

        // Cache last parse results
        private Dictionary<QuestData, QuestScriptAnalysis> _analysisCache = new Dictionary<QuestData, QuestScriptAnalysis>();

        private class QuestScriptAnalysis
        {
            public HashSet<string> CardsMentioned = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            public HashSet<string> InventoryMentions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            public HashSet<string> AssetMentions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            public HashSet<string> TaskStartMentions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            public HashSet<string> TaskEndMentions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            public HashSet<string> ActivityMentions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            public HashSet<string> ActionMentions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            public string ScriptPath; // for info
            public string QuestId;
        }

        [MenuItem("Antura/Quest Management", priority = 21)]
        public static void ShowWindow()
        {
            var wnd = GetWindow<QuestManagementWindow>(false, "Quest Management", true);
            wnd.minSize = new Vector2(800, 400);
            wnd.RefreshData();
            wnd.Show();
        }

        private void OnEnable()
        {
            RefreshData();
            if (_searchField == null)
                _searchField = new SearchField();
        }

        private void RefreshData()
        {
            _quests = LoadAll<QuestData>().OrderBy(q => q.Id ?? q.name).ToList();
            _allCards = LoadAll<CardData>().OrderBy(c => c.Id ?? c.name).ToList();
            _allAssets = LoadAll<AssetData>().OrderBy(a => a.Id ?? a.name).ToList();
            _analysisCache.Clear();
            Repaint();
        }

        private static List<T> LoadAll<T>() where T : UnityEngine.Object
        {
            var list = new List<T>();
            try
            {
                var guids = AssetDatabase.FindAssets("t:" + typeof(T).Name);
                foreach (var g in guids)
                {
                    var path = AssetDatabase.GUIDToAssetPath(g);
                    var obj = AssetDatabase.LoadAssetAtPath<T>(path);
                    if (obj != null)
                        list.Add(obj);
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[QuestManagement] Failed to load {typeof(T).Name}: {ex.Message}");
            }
            return list;
        }

        private void OnGUI()
        {
            DrawToolbar();
            GUILayout.Space(6);
            DrawBody();
        }

        private void DrawToolbar()
        {
            using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                // Quest selector: single quest only (removed 'All')
                if (_quests.Count == 0)
                {
                    EditorGUILayout.LabelField("No Quests Found", EditorStyles.toolbarPopup, GUILayout.Width(200));
                }
                else
                {
                    // Clamp selected index
                    if (_selectedQuestIndex < 0 || _selectedQuestIndex >= _quests.Count)
                        _selectedQuestIndex = 0;
                    var labels = _quests.Select(q => GetQuestLabel(q)).ToArray();
                    int newIdx = EditorGUILayout.Popup(_selectedQuestIndex, labels, EditorStyles.toolbarPopup, GUILayout.Width(250));
                    if (newIdx != _selectedQuestIndex)
                    { _selectedQuestIndex = newIdx; Repaint(); }
                }

                GUILayout.Space(8);
                if (GUILayout.Button("Refresh", EditorStyles.toolbarButton, GUILayout.Width(70)))
                { RefreshData(); }

                if (GUILayout.Button("Analyze Scripts", EditorStyles.toolbarButton, GUILayout.Width(120)))
                { AnalyzeSelected(); }

                GUILayout.FlexibleSpace();
                if (_searchField == null)
                    _searchField = new SearchField();
                var newSearch = _searchField.OnToolbarGUI(_search, GUILayout.MinWidth(180));
                if (!string.Equals(newSearch, _search, StringComparison.Ordinal))
                { _search = newSearch; Repaint(); }
            }
        }

        private void DrawBody()
        {
            var targets = GetTargetQuests();
            _scroll = EditorGUILayout.BeginScrollView(_scroll);

            // Scene-wide ActableAbstract ID uniqueness check
            DrawActableIdCheck();
            GUILayout.Space(10);

            foreach (var q in targets)
            {
                DrawQuestSection(q);
                GUILayout.Space(8);
            }

            EditorGUILayout.EndScrollView();
        }

        private void DrawActableIdCheck()
        {
            using (new EditorGUILayout.VerticalScope("box"))
            {
                EditorGUILayout.LabelField("Scene Actable IDs", EditorStyles.boldLabel);
                var actables = GetSceneActables();
                var dupes = actables
                    .Where(a => a != null && !string.IsNullOrEmpty(a.Id))
                    .GroupBy(a => a.Id, System.StringComparer.OrdinalIgnoreCase)
                    .Where(g => g.Count() > 1)
                    .OrderBy(g => g.Key, System.StringComparer.OrdinalIgnoreCase)
                    .ToList();

                if (actables.Count == 0)
                {
                    EditorGUILayout.HelpBox("No ActableAbstract components found in the open scene(s).", MessageType.Info);
                    return;
                }

                if (dupes.Count == 0)
                {
                    EditorGUILayout.HelpBox($"OK: {actables.Count} actables, all IDs unique (non-empty only).", MessageType.Info);
                }
                else
                {
                    EditorGUILayout.HelpBox($"Found {dupes.Count} duplicate ID group(s). Click buttons to ping objects.", MessageType.Warning);
                    foreach (var group in dupes)
                    {
                        using (new EditorGUILayout.VerticalScope("box"))
                        {
                            EditorGUILayout.LabelField($"ID: {group.Key}  (x{group.Count()})", EditorStyles.boldLabel);
                            using (new EditorGUILayout.HorizontalScope())
                            {
                                int shown = 0;
                                foreach (var a in group)
                                {
                                    if (a == null)
                                        continue;
                                    if (GUILayout.Button(a.gameObject.name, GUILayout.MinWidth(120)))
                                    {
                                        Selection.activeObject = a.gameObject;
                                        EditorGUIUtility.PingObject(a.gameObject);
                                    }
                                    shown++;
                                    if (shown % 4 == 0)
                                    {
                                        GUILayout.EndHorizontal();
                                        GUILayout.BeginHorizontal();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private List<ActableAbstract> GetSceneActables()
        {
            var list = new List<ActableAbstract>();
            try
            {
#if UNITY_2022_2_OR_NEWER
                var found = UnityEngine.Object.FindObjectsByType<ActableAbstract>(FindObjectsSortMode.None);
#else
                var found = UnityEngine.Object.FindObjectsOfType<ActableAbstract>(true);
#endif
                foreach (var a in found)
                {
                    if (a == null)
                        continue;
                    // Exclude prefabs / assets, keep only scene instances
                    if (EditorUtility.IsPersistent(a))
                        continue;
                    if (!a.gameObject.scene.IsValid())
                        continue;
                    list.Add(a);
                }
            }
            catch { }
            return list;
        }

        private IEnumerable<QuestData> GetTargetQuests()
        {
            if (_quests.Count == 0)
                return Enumerable.Empty<QuestData>();

            var quest = _quests[Mathf.Clamp(_selectedQuestIndex, 0, _quests.Count - 1)];
            if (string.IsNullOrEmpty(_search))
                return new[] { quest };

            string term = _search.Trim();
            bool matches = (!string.IsNullOrEmpty(quest.Id) && quest.Id.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0)
                        || (!string.IsNullOrEmpty(quest.TitleEn) && quest.TitleEn.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0)
                        || (quest.name.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0);
            return matches ? new[] { quest } : Enumerable.Empty<QuestData>();
        }

        private void DrawQuestSection(QuestData q)
        {
            using (new EditorGUILayout.VerticalScope("box"))
            {
                EditorGUILayout.LabelField($"Quest: {q.Id ?? q.name}", EditorStyles.boldLabel);
                var analysis = AnalyzeQuest(q);
                using (new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("Ping", GUILayout.Width(60)))
                    { EditorGUIUtility.PingObject(q); }
                    if (GUILayout.Button("Open QuestData", GUILayout.Width(130)))
                    { Selection.activeObject = q; }
                    if (GUILayout.Button("Open Prefab", GUILayout.Width(110)))
                    { OpenQuestPrefab(q); }
                    if (GUILayout.Button("Open Script", GUILayout.Width(110)))
                    {
                        var ta = q.YarnScript != null ? q.YarnScript : null;
                        if (ta != null)
                        { Selection.activeObject = ta; EditorGUIUtility.PingObject(ta); }
                        else if (!string.IsNullOrEmpty(analysis.ScriptPath))
                        { EditorUtility.RevealInFinder(analysis.ScriptPath); }
                    }
                    if (GUILayout.Button("Analyze", GUILayout.Width(80)))
                    { AnalyzeQuest(q, force: true); }
                    if (GUILayout.Button("Inject Scene Data", GUILayout.Width(140)))
                    { InjectAutomaticSceneData(q); }
                    GUILayout.FlexibleSpace();
                }

                // Build collections
                var cardMentions = analysis.CardsMentioned.OrderBy(s => s).ToList();
                var invMentions = analysis.InventoryMentions.OrderBy(s => s).ToList();
                var assetMentions = analysis.AssetMentions.OrderBy(s => s).ToList();
                var allMentions = new HashSet<string>(cardMentions, StringComparer.OrdinalIgnoreCase);
                foreach (var m in invMentions)
                    allMentions.Add(m);

                var allCardIds = new HashSet<string>(_allCards.Select(c => c.Id ?? c.name), StringComparer.OrdinalIgnoreCase);
                var allAssetIds = new HashSet<string>(_allAssets.Select(a => a.Id ?? a.name), StringComparer.OrdinalIgnoreCase);

                // Script -> Unity mismatches (Left)
                var missingInDb = allMentions.Where(m => !allCardIds.Contains(m)).OrderBy(s => s).ToList();
                var missingAssets = assetMentions.Where(m => !allAssetIds.Contains(m)).OrderBy(s => s).ToList();

                var prefabTaskCodes = GetQuestTaskCodes(q);
                var tasksMentioned = new HashSet<string>(analysis.TaskStartMentions, StringComparer.OrdinalIgnoreCase);
                foreach (var t in analysis.TaskEndMentions)
                    tasksMentioned.Add(t);
                var missingTasks = tasksMentioned.Where(t => !prefabTaskCodes.Contains(t)).OrderBy(s => s).ToList();

                var prefabActivityCodes = GetQuestActivityCodes(q);
                var missingActivities = analysis.ActivityMentions.Where(a => !prefabActivityCodes.Contains(a)).OrderBy(s => s).ToList();

                var prefabActionCodes = GetQuestActionCodes(q);
                // Ignore reserved action called by QuestManager
                analysis.ActionMentions.RemoveWhere(s => string.Equals(s, "init", StringComparison.OrdinalIgnoreCase));
                var missingActions = analysis.ActionMentions.Where(a => !prefabActionCodes.Contains(a)).OrderBy(s => s).ToList();

                var questCardIds = new HashSet<string>((q.Cards ?? new List<CardData>()).Where(c => c != null).Select(c => c.Id ?? c.name), StringComparer.OrdinalIgnoreCase);
                var missingInQuest = allMentions.Where(m => !questCardIds.Contains(m)).OrderBy(s => s).ToList();

                // Unity -> Script mismatches (Right)
                var unusedTasks = prefabTaskCodes.Where(code => !tasksMentioned.Contains(code)).OrderBy(s => s).ToList();
                var unusedActivities = prefabActivityCodes.Where(code => !analysis.ActivityMentions.Contains(code)).OrderBy(s => s).ToList();
                var unusedActions = prefabActionCodes
                    .Where(code => !string.Equals(code, "init", StringComparison.OrdinalIgnoreCase))
                    .Where(code => !analysis.ActionMentions.Contains(code))
                    .OrderBy(s => s)
                    .ToList();
                var unusedInScript = questCardIds.Where(id => !allMentions.Contains(id)).OrderBy(s => s).ToList();

                // Two columns layout
                using (new EditorGUILayout.HorizontalScope())
                {
                    // Left column: In Script but missing in Unity
                    using (new EditorGUILayout.VerticalScope("box"))
                    {
                        EditorGUILayout.LabelField("In Script → Missing in Unity", EditorStyles.boldLabel);
                        // CARDS
                        if (missingInDb.Count > 0 || missingInQuest.Count > 0)
                        {
                            GUILayout.Space(4);
                            EditorGUILayout.LabelField("CARDS", EditorStyles.boldLabel);
                            if (missingInDb.Count > 0)
                            { EditorGUILayout.LabelField("Missing in CardData DB:", EditorStyles.miniBoldLabel); DrawTagList(missingInDb); }
                            if (missingInQuest.Count > 0)
                            { DrawCopyableList($"Mentioned but not in QuestData.Cards ({missingInQuest.Count})", missingInQuest); }
                        }
                        // ASSETS
                        if (missingAssets.Count > 0)
                        {
                            GUILayout.Space(6);
                            EditorGUILayout.LabelField("ASSETS", EditorStyles.boldLabel);
                            EditorGUILayout.LabelField("Missing Assets in AssetData DB:", EditorStyles.miniBoldLabel);
                            DrawTagList(missingAssets);
                        }
                        // TASKS
                        if (missingTasks.Count > 0)
                        {
                            GUILayout.Space(6);
                            EditorGUILayout.LabelField("TASKS", EditorStyles.boldLabel);
                            EditorGUILayout.LabelField("Missing Tasks (QuestManager.QuestTasks):", EditorStyles.miniBoldLabel);
                            DrawTagList(missingTasks);
                        }
                        // ACTIVITIES
                        if (missingActivities.Count > 0)
                        {
                            GUILayout.Space(6);
                            EditorGUILayout.LabelField("ACTIVITIES", EditorStyles.boldLabel);
                            EditorGUILayout.LabelField("Missing Activities (ActivityConfigs by Settings.Id):", EditorStyles.miniBoldLabel);
                            DrawActivityList(missingActivities);
                        }
                        // ACTIONS
                        if (missingActions.Count > 0)
                        {
                            GUILayout.Space(6);
                            EditorGUILayout.LabelField("ACTIONS", EditorStyles.boldLabel);
                            EditorGUILayout.LabelField("Missing Actions (ActionManager.QuestActions):", EditorStyles.miniBoldLabel);
                            DrawTagList(missingActions);
                        }
                    }

                    // Right column: In Unity but not referenced in Script
                    using (new EditorGUILayout.VerticalScope("box"))
                    {
                        EditorGUILayout.LabelField("In Unity → Not used in Script", EditorStyles.boldLabel);
                        // TASKS
                        if (unusedTasks.Count > 0)
                        {
                            GUILayout.Space(4);
                            EditorGUILayout.LabelField("TASKS", EditorStyles.boldLabel);
                            EditorGUILayout.LabelField("Tasks configured but NOT used:", EditorStyles.miniBoldLabel);
                            DrawTagList(unusedTasks);
                        }
                        // ACTIVITIES
                        if (unusedActivities.Count > 0)
                        {
                            GUILayout.Space(6);
                            EditorGUILayout.LabelField("ACTIVITIES", EditorStyles.boldLabel);
                            EditorGUILayout.LabelField("Activities configured but NOT used:", EditorStyles.miniBoldLabel);
                            DrawActivityList(unusedActivities);
                        }
                        // ACTIONS
                        if (unusedActions.Count > 0)
                        {
                            GUILayout.Space(6);
                            EditorGUILayout.LabelField("ACTIONS", EditorStyles.boldLabel);
                            EditorGUILayout.LabelField("Actions configured but NOT used:", EditorStyles.miniBoldLabel);
                            DrawTagList(unusedActions);
                        }
                        // CARDS
                        if (unusedInScript.Count > 0)
                        {
                            GUILayout.Space(6);
                            EditorGUILayout.LabelField("CARDS", EditorStyles.boldLabel);
                            DrawCopyableList($"Cards in QuestData.Cards but NOT used ({unusedInScript.Count})", unusedInScript);
                        }
                    }
                }
            }
        }

        private static void OpenQuestPrefab(QuestData q)
        {
            if (q == null || q.QuestPrefab == null)
            {
                EditorUtility.DisplayDialog("Open Prefab", "No QuestPrefab assigned on this Quest.", "OK");
                return;
            }

            var go = q.QuestPrefab;
            string path = null;
            try
            { path = AssetDatabase.GetAssetPath(go); }
            catch { path = null; }
            if (string.IsNullOrEmpty(path))
            {
                try
                { path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(go); }
                catch { path = null; }
            }

            if (!string.IsNullOrEmpty(path) && path.EndsWith(".prefab", StringComparison.OrdinalIgnoreCase))
            {
                // Open in Prefab Stage
                PrefabStageUtility.OpenPrefab(path);
            }
            else
            {
                // Fallback: select and ping the object
                Selection.activeObject = go;
                EditorGUIUtility.PingObject(go);
            }
        }

        private static string GetQuestLabel(QuestData q)
        {
            var idPart = string.IsNullOrEmpty(q.Id) ? q.name : q.Id;
            var title = q.TitleEn;
            return string.IsNullOrEmpty(title) ? idPart : (idPart + " | " + title);
        }

        private void DrawTagList(List<string> items)
        {
            using (new EditorGUILayout.VerticalScope("box"))
            {
                int cols = 4;
                int rows = Mathf.CeilToInt(items.Count / (float)cols);
                int idx = 0;
                for (int r = 0; r < rows; r++)
                {
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        for (int c = 0; c < cols && idx < items.Count; c++)
                        {
                            var id = items[idx++];
                            if (GUILayout.Button(id, GUILayout.MinWidth(120)))
                            {
                                // Ping card if exists
                                var cd = _allCards.FirstOrDefault(x => string.Equals(x.Id ?? x.name, id, StringComparison.OrdinalIgnoreCase));
                                if (cd != null)
                                { Selection.activeObject = cd; EditorGUIUtility.PingObject(cd); }
                                else
                                {
                                    var ad = _allAssets.FirstOrDefault(x => string.Equals(x.Id ?? x.name, id, StringComparison.OrdinalIgnoreCase));
                                    if (ad != null)
                                    { Selection.activeObject = ad; EditorGUIUtility.PingObject(ad); }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void DrawCopyableList(string title, List<string> items)
        {
            using (new EditorGUILayout.VerticalScope("box"))
            {
                EditorGUILayout.LabelField(title + ":", EditorStyles.miniBoldLabel);
                var text = string.Join("\n", items);
                // Make the text selectable/copyable
                var style = new GUIStyle(EditorStyles.textArea) { wordWrap = false }; // keep IDs one per line
                EditorGUILayout.TextArea(text, style, GUILayout.MinHeight(Mathf.Min(200, 20 + items.Count * 18)));
                using (new EditorGUILayout.HorizontalScope())
                {
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("Copy All", GUILayout.Width(90)))
                    {
                        EditorGUIUtility.systemCopyBuffer = text;
                    }
                }
            }
        }

        // Render a clickable grid of activity settings IDs; clicking selects the ActivitySettings asset in the Project
        private void DrawActivityList(List<string> items)
        {
            using (new EditorGUILayout.VerticalScope("box"))
            {
                int cols = 4;
                int rows = Mathf.CeilToInt(items.Count / (float)cols);
                int idx = 0;
                for (int r = 0; r < rows; r++)
                {
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        for (int c = 0; c < cols && idx < items.Count; c++)
                        {
                            var id = items[idx++];
                            if (GUILayout.Button(id, GUILayout.MinWidth(120)))
                            {
                                var settings = FindActivitySettingsById(id);
                                if (settings != null)
                                {
                                    Selection.activeObject = settings;
                                    EditorGUIUtility.PingObject(settings);
                                }
                            }
                        }
                    }
                }
            }
        }

        private static ActivitySettingsAbstract FindActivitySettingsById(string id)
        {
            if (string.IsNullOrEmpty(id))
                return null;
            try
            {
                var guids = AssetDatabase.FindAssets("t:" + nameof(ActivitySettingsAbstract));
                foreach (var g in guids)
                {
                    var path = AssetDatabase.GUIDToAssetPath(g);
                    var obj = AssetDatabase.LoadAssetAtPath<ActivitySettingsAbstract>(path);
                    if (obj != null)
                    {
                        var oid = obj.Id;
                        if (!string.IsNullOrEmpty(oid) && string.Equals(oid, id, StringComparison.OrdinalIgnoreCase))
                            return obj;
                    }
                }
            }
            catch { }
            return null;
        }

        private void AnalyzeSelected()
        {
            var targets = GetTargetQuests().ToList();
            foreach (var q in targets)
                AnalyzeQuest(q, force: true);
            Repaint();
        }

        private QuestScriptAnalysis AnalyzeQuest(QuestData q, bool force = false)
        {
            if (!force && _analysisCache.TryGetValue(q, out var cached))
                return cached;

            var result = new QuestScriptAnalysis { QuestId = q.Id ?? q.name };
            // Determine script content: prefer QuestData.YarnScript, otherwise try YarnProject text assets; fallback to same-folder .yarn
            string text = null;
            string pathInfo = null;
            try
            {
                if (q.YarnScript != null)
                {
                    text = q.YarnScript.text;
                    pathInfo = AssetDatabase.GetAssetPath(q.YarnScript);
                }
                else if (q.YarnProject != null)
                {
                    // Try to read all .yarn files referenced by YarnProject (Editor only)
                    var ypPath = AssetDatabase.GetAssetPath(q.YarnProject);
                    if (!string.IsNullOrEmpty(ypPath))
                    {
                        var dir = System.IO.Path.GetDirectoryName(ypPath);
                        var yarnFiles = Directory.GetFiles(dir, "*.yarn", SearchOption.AllDirectories);
                        text = string.Join("\n\n", yarnFiles.Select(f => SafeReadAllText(f)));
                        pathInfo = dir;
                    }
                }
                else
                {
                    // Fallback: try to locate a .yarn next to the quest asset
                    var qPath = AssetDatabase.GetAssetPath(q);
                    var qDir = System.IO.Path.GetDirectoryName(qPath);
                    var yarnFiles = Directory.GetFiles(qDir, "*.yarn", SearchOption.AllDirectories);
                    if (yarnFiles.Length > 0)
                    {
                        text = string.Join("\n\n", yarnFiles.Select(f => SafeReadAllText(f)));
                        pathInfo = qDir;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[QuestManagement] Error reading yarn for {q.Id ?? q.name}: {ex.Message}");
            }

            result.ScriptPath = pathInfo ?? "(unknown)";
            ParseMentions(text ?? string.Empty, result);

            _analysisCache[q] = result;
            return result;
        }

        private static string SafeReadAllText(string path)
        {
            try
            { return File.ReadAllText(path); }
            catch { return string.Empty; }
        }

        // Parse <<card id>>, <<inventory id ...>>, <<asset id ...>>, <<activity code ...>>, <<action code ...>>, and <<task_start|task_end id ...>> tokens
        private static readonly Regex CardRegex = new Regex(@"<<\s*card\s+([A-Za-z0-9_\-]+)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex InventoryRegex = new Regex(@"<<\s*inventory\s+([A-Za-z0-9_\-]+)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex AssetRegex = new Regex(@"<<\s*asset\s+([A-Za-z0-9_\-]+)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex ActivityRegex = new Regex(@"<<\s*activity\s+([A-Za-z0-9_\-]+)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex ActionRegex = new Regex(@"<<\s*action\s+([A-Za-z0-9_\-]+)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex TaskStartRegex = new Regex(@"<<\s*task_start\s+([A-Za-z0-9_\-]+)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex TaskEndRegex = new Regex(@"<<\s*task_end\s+([A-Za-z0-9_\-]+)", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private void ParseMentions(string text, QuestScriptAnalysis into)
        {
            into.CardsMentioned.Clear();
            into.InventoryMentions.Clear();
            into.AssetMentions.Clear();
            into.TaskStartMentions.Clear();
            into.TaskEndMentions.Clear();
            into.ActivityMentions.Clear();
            into.ActionMentions.Clear();
            if (string.IsNullOrEmpty(text))
                return;

            foreach (Match m in CardRegex.Matches(text))
            {
                if (m.Groups.Count > 1)
                {
                    var id = m.Groups[1].Value.Trim();
                    if (!string.IsNullOrEmpty(id))
                        into.CardsMentioned.Add(id);
                }
            }
            foreach (Match m in InventoryRegex.Matches(text))
            {
                if (m.Groups.Count > 1)
                {
                    var id = m.Groups[1].Value.Trim();
                    if (!string.IsNullOrEmpty(id))
                        into.InventoryMentions.Add(id);
                }
            }
            foreach (Match m in AssetRegex.Matches(text))
            {
                if (m.Groups.Count > 1)
                {
                    var id = m.Groups[1].Value.Trim();
                    if (!string.IsNullOrEmpty(id))
                        into.AssetMentions.Add(id);
                }
            }
            foreach (Match m in TaskStartRegex.Matches(text))
            {
                if (m.Groups.Count > 1)
                {
                    var id = m.Groups[1].Value.Trim();
                    if (!string.IsNullOrEmpty(id))
                        into.TaskStartMentions.Add(id);
                }
            }
            foreach (Match m in TaskEndRegex.Matches(text))
            {
                if (m.Groups.Count > 1)
                {
                    var id = m.Groups[1].Value.Trim();
                    if (!string.IsNullOrEmpty(id))
                        into.TaskEndMentions.Add(id);
                }
            }
            foreach (Match m in ActivityRegex.Matches(text))
            {
                if (m.Groups.Count > 1)
                {
                    var id = m.Groups[1].Value.Trim();
                    if (!string.IsNullOrEmpty(id))
                        into.ActivityMentions.Add(id);
                }
            }
            foreach (Match m in ActionRegex.Matches(text))
            {
                if (m.Groups.Count > 1)
                {
                    var id = m.Groups[1].Value.Trim();
                    if (!string.IsNullOrEmpty(id))
                        into.ActionMentions.Add(id);
                }
            }
        }

        // Inspect the quest prefab to retrieve QuestManager.QuestTasks[*].Code values without hard depending on QuestManager type
        private static HashSet<string> GetQuestTaskCodes(QuestData q)
        {
            var result = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            if (q == null || q.QuestPrefab == null)
                return result;

            try
            {
                var root = q.QuestPrefab;
                var comps = root.GetComponentsInChildren<Component>(true);
                foreach (var comp in comps)
                {
                    if (comp == null)
                        continue;
                    SerializedObject so = null;
                    try
                    { so = new SerializedObject(comp); }
                    catch { so = null; }
                    if (so == null)
                        continue;

                    var tasksProp = so.FindProperty("QuestTasks");
                    if (tasksProp == null || !tasksProp.isArray)
                        continue;

                    for (int i = 0; i < tasksProp.arraySize; i++)
                    {
                        var elem = tasksProp.GetArrayElementAtIndex(i);
                        if (elem == null)
                            continue;
                        var codeProp = elem.FindPropertyRelative("Code");
                        if (codeProp != null)
                        {
                            var code = codeProp.stringValue;
                            if (!string.IsNullOrEmpty(code))
                                result.Add(code);
                        }
                    }

                    // Found a component with QuestTasks; we can stop if desired
                    if (result.Count > 0)
                        break;
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[QuestManagement] Could not read QuestTasks from prefab for {q.Id ?? q.name}: {ex.Message}");
            }
            return result;
        }

        private static HashSet<string> GetQuestActivityCodes(QuestData q)
        {
            var result = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            if (q == null || q.QuestPrefab == null)
                return result;
            try
            {
                var comps = q.QuestPrefab.GetComponentsInChildren<Component>(true);
                foreach (var comp in comps)
                {
                    if (comp == null)
                        continue;
                    SerializedObject so = null;
                    try
                    { so = new SerializedObject(comp); }
                    catch { so = null; }
                    if (so == null)
                        continue;

                    var activityConfigs = so.FindProperty("ActivityConfigs");
                    if (activityConfigs == null || !activityConfigs.isArray)
                        continue;

                    for (int i = 0; i < activityConfigs.arraySize; i++)
                    {
                        var elem = activityConfigs.GetArrayElementAtIndex(i);
                        if (elem == null)
                            continue;
                        var settingsProp = elem.FindPropertyRelative("ActivitySettings");
                        if (settingsProp != null && settingsProp.objectReferenceValue != null)
                        {
                            var settings = settingsProp.objectReferenceValue as ActivitySettingsAbstract;
                            if (settings != null && !string.IsNullOrEmpty(settings.Id))
                                result.Add(settings.Id);
                        }
                    }

                    if (result.Count > 0)
                        break;
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[QuestManagement] Could not read ActivityConfigs from prefab for {q.Id ?? q.name}: {ex.Message}");
            }
            return result;
        }

        private static HashSet<string> GetQuestActionCodes(QuestData q)
        {
            var result = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            if (q == null || q.QuestPrefab == null)
                return result;
            try
            {
                var comps = q.QuestPrefab.GetComponentsInChildren<Component>(true);
                foreach (var comp in comps)
                {
                    if (comp == null)
                        continue;
                    SerializedObject so = null;
                    try
                    { so = new SerializedObject(comp); }
                    catch { so = null; }
                    if (so == null)
                        continue;

                    var questActions = so.FindProperty("QuestActions");
                    if (questActions == null || !questActions.isArray)
                        continue;

                    for (int i = 0; i < questActions.arraySize; i++)
                    {
                        var elem = questActions.GetArrayElementAtIndex(i);
                        if (elem == null)
                            continue;
                        var codeProp = elem.FindPropertyRelative("ActionCode");
                        if (codeProp != null)
                        {
                            var code = codeProp.stringValue;
                            if (!string.IsNullOrEmpty(code))
                                result.Add(code);
                        }
                    }

                    if (result.Count > 0)
                        break;
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[QuestManagement] Could not read QuestActions from prefab for {q.Id ?? q.name}: {ex.Message}");
            }
            return result;
        }

        // -------------------------------- Automatic Scene Data Injection -------------------------------
        private void InjectAutomaticSceneData(QuestData quest)
        {
            if (quest == null)
            {
                Debug.LogWarning("[QuestManagement] No quest selected for scene data injection.");
                return;
            }
            if (quest.YarnScript == null)
            {
                Debug.LogWarning($"[QuestManagement] Quest '{quest.Id}' has no YarnScript assigned.");
                return;
            }
            string path = AssetDatabase.GetAssetPath(quest.YarnScript);
            if (string.IsNullOrEmpty(path))
            {
                Debug.LogWarning($"[QuestManagement] Could not resolve path for YarnScript of quest '{quest.Id}'.");
                return;
            }
            string originalText;
            try
            { originalText = File.ReadAllText(path); }
            catch (Exception ex)
            {
                Debug.LogError($"[QuestManagement] Failed reading Yarn script: {ex.Message}");
                return;
            }

            // Ensure we have // <scene_data> ... // </scene_data> section (commented). Backwards compatible with raw tags.
            const string openTagRaw = "<scene_data>";
            const string closeTagRaw = "</scene_data>";
            const string openTagCommented = "// <scene_data>";
            const string closeTagCommented = "// </scene_data>";
            string actualOpenTag = openTagCommented;
            string actualCloseTag = closeTagCommented;
            int openIdx = originalText.IndexOf(openTagCommented, StringComparison.OrdinalIgnoreCase);
            int closeIdx = originalText.IndexOf(closeTagCommented, StringComparison.OrdinalIgnoreCase);
            if (openIdx < 0)
            {
                openIdx = originalText.IndexOf(openTagRaw, StringComparison.OrdinalIgnoreCase);
                if (openIdx >= 0)
                    actualOpenTag = openTagRaw;
            }
            if (closeIdx < 0)
            {
                closeIdx = originalText.IndexOf(closeTagRaw, StringComparison.OrdinalIgnoreCase);
                if (closeIdx >= 0)
                    actualCloseTag = closeTagRaw;
            }
            if (openIdx < 0 || closeIdx < 0 || closeIdx < openIdx)
            {
                int firstLineEnd = originalText.IndexOf('\n');
                if (firstLineEnd < 0)
                    firstLineEnd = 0;
                else
                    firstLineEnd += 1;
                string firstPart = originalText.Substring(0, firstLineEnd);
                string rest = originalText.Substring(firstLineEnd);
                string insertion = openTagCommented + "\n" + closeTagCommented + "\n";
                originalText = firstPart + insertion + rest;
                actualOpenTag = openTagCommented;
                actualCloseTag = closeTagCommented;
                openIdx = originalText.IndexOf(actualOpenTag, StringComparison.OrdinalIgnoreCase);
                closeIdx = originalText.IndexOf(actualCloseTag, StringComparison.OrdinalIgnoreCase);
            }

            // Ensure first line is quest header: // Quest: code | TitleEn
            string questCode = quest.Id ?? quest.name;
            string questTitle = string.IsNullOrEmpty(quest.TitleEn) ? questCode : quest.TitleEn;
            string desiredHeader = $"// Quest: {questCode} | {questTitle}";
            // Split once to inspect first line
            int firstNewline = originalText.IndexOf('\n');
            if (firstNewline < 0)
                firstNewline = originalText.Length; // single line file edge case
            string firstLine = originalText.Substring(0, firstNewline).TrimEnd('\r');
            if (!firstLine.StartsWith("// Quest:"))
            {
                // Prepend header
                originalText = desiredHeader + "\n" + originalText;
                // Recompute indices
                openIdx = originalText.IndexOf(actualOpenTag, StringComparison.OrdinalIgnoreCase);
                closeIdx = originalText.IndexOf(actualCloseTag, StringComparison.OrdinalIgnoreCase);
            }
            else if (!string.Equals(firstLine, desiredHeader, StringComparison.Ordinal))
            {
                // Replace header line
                originalText = desiredHeader + originalText.Substring(firstNewline);
                // Indices unchanged positions shift maybe same length? safer to recompute
                openIdx = originalText.IndexOf(actualOpenTag, StringComparison.OrdinalIgnoreCase);
                closeIdx = originalText.IndexOf(actualCloseTag, StringComparison.OrdinalIgnoreCase);
            }

            // Rebuild entire scene_data inner content (we fully replace between tags)
            string block = BuildAutomaticSceneDataBlock(quest);
            int innerStart = openIdx + actualOpenTag.Length;
            string before = originalText.Substring(0, innerStart);
            string after = originalText.Substring(closeIdx);
            string newText = before + "\n" + block + "\n" + after; // ensure surrounding newlines

            // Normalize spacing after closing tag: exactly one empty line after the // </scene_data> line
            const string closeTagCommented = "// </scene_data>";
            int closeTagLineIdx = newText.IndexOf(closeTagCommented, StringComparison.OrdinalIgnoreCase);
            if (closeTagLineIdx >= 0)
            {
                int endOfLine = newText.IndexOf('\n', closeTagLineIdx);
                if (endOfLine < 0)
                {
                    // File ended right after close tag: add two newlines (one for EOL, one blank line)
                    newText += "\n\n";
                }
                else
                {
                    int scan = endOfLine + 1;
                    while (scan < newText.Length && (newText[scan] == '\n' || newText[scan] == '\r'))
                        scan++;
                    string rest = newText.Substring(scan);
                    newText = newText.Substring(0, endOfLine + 1) + "\n" + rest; // one extra blank line only
                }
            }

            try
            {
                File.WriteAllText(path, newText);
                AssetDatabase.ImportAsset(path);
                EditorGUIUtility.PingObject(quest.YarnScript);
                Debug.Log($"[QuestManagement] Injected automatic scene data into '{quest.YarnScript.name}'.");
            }
            catch (Exception ex)
            {
                Debug.LogError($"[QuestManagement] Failed writing Yarn script: {ex.Message}");
            }
        }

        // Build the content that goes INSIDE <scene_data> ... </scene_data> (without the tags themselves)
        private string BuildAutomaticSceneDataBlock(QuestData quest)
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("// DO NOT EDIT THIS SECTION. It is auto-generated by the editor.");
            sb.AppendLine("// in the Unity scene these elements are ready to be used in this script:");

            // TOPICS
            var topicIds = (quest.Topics ?? new List<TopicData>())
                .Where(t => t != null)
                .Select(t => t.Id ?? t.name)
                .Where(id => !string.IsNullOrEmpty(id))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(id => id, StringComparer.OrdinalIgnoreCase)
                .ToList();
            sb.AppendLine("// TOPICS");
            foreach (var id in topicIds)
                sb.AppendLine("// - " + id);
            if (topicIds.Count == 0)
                sb.AppendLine("// - (none)");

            // CARDS
            var cardIds = (quest.Cards ?? new List<CardData>())
                .Where(c => c != null)
                .Select(c => c.Id ?? c.name)
                .Where(id => !string.IsNullOrEmpty(id))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(id => id, StringComparer.OrdinalIgnoreCase)
                .ToList();
            sb.AppendLine("// CARDS");
            foreach (var id in cardIds)
                sb.AppendLine("// - " + id);
            if (cardIds.Count == 0)
                sb.AppendLine("// - (none)");

            // TASKS
            var taskCodes = GetQuestTaskCodes(quest).OrderBy(s => s, StringComparer.OrdinalIgnoreCase).ToList();
            sb.AppendLine("// TASKS");
            foreach (var t in taskCodes)
                sb.AppendLine("// - " + t);
            if (taskCodes.Count == 0)
                sb.AppendLine("// - (none)");

            // ACTIVITIES
            var activityCodes = GetQuestActivityCodes(quest).OrderBy(s => s, StringComparer.OrdinalIgnoreCase).ToList();
            sb.AppendLine("// ACTIVITIES");
            foreach (var a in activityCodes)
                sb.AppendLine("// - " + a);
            if (activityCodes.Count == 0)
                sb.AppendLine("// - (none)");

            // ACTABLE
            var actables = GetSceneActables()
                .Where(a => a != null && !string.IsNullOrEmpty(a.Id))
                .GroupBy(a => a.Id, StringComparer.OrdinalIgnoreCase)
                .Select(g => g.Key)
                .OrderBy(id => id, StringComparer.OrdinalIgnoreCase)
                .ToList();
            sb.AppendLine("// ACTABLE");
            foreach (var id in actables)
                sb.AppendLine("// - " + id);
            if (actables.Count == 0)
                sb.AppendLine("// - (none)");

            // WORDS single line
            var wordList = (quest.Words ?? new List<WordData>())
                .Where(w => w != null)
                .Select(w => string.IsNullOrEmpty(w.TextEn) ? (w.name ?? w.Id) : w.TextEn)
                .Where(t => !string.IsNullOrEmpty(t))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(t => t, StringComparer.OrdinalIgnoreCase)
                .ToList();
            string wordsJoined = wordList.Count == 0 ? "(none)" : string.Join(", ", wordList);
            sb.AppendLine("// WORDS: " + wordsJoined);
            return sb.ToString().TrimEnd();
        }
    }
}
#endif
