#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using System.Text;

namespace Antura.Discover.Editor
{
    /// Editor window to manage Topics: export/import JSON for external editing.
    public class TopicManagementWindow : EditorWindow
    {
        [MenuItem("Antura/Topic Management", priority = 23)]
        public static void Open()
        {
            // Open as a regular, dockable window (utility=false)
            var win = GetWindow<TopicManagementWindow>(false, "Topic Management", true);
            win.minSize = new Vector2(520, 320);
            win.Show();
        }

        // Import options
        private bool importConnectionsOnly = true;
        private bool validateAfterImport = true;
        private Vector2 scroll;
        private string lastLog;

        void OnGUI()
        {
            EditorGUILayout.LabelField("Topic Management", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox("Export Topics to JSON, edit externally (connections, etc.), then import changes. More validations and utilities will be added here soon.", MessageType.Info);

            scroll = EditorGUILayout.BeginScrollView(scroll);

            DrawExportSection();
            EditorGUILayout.Space(8);
            DrawImportSection();
            EditorGUILayout.Space(8);
            DrawUtilitiesSection();
            EditorGUILayout.Space(8);
            DrawValidationSection();

            EditorGUILayout.EndScrollView();

            if (!string.IsNullOrEmpty(lastLog))
            {
                EditorGUILayout.Space(8);
                EditorGUILayout.LabelField("Last operation", EditorStyles.boldLabel);
                EditorGUILayout.TextArea(lastLog, GUILayout.MinHeight(80));
            }
        }

        private void DrawUtilitiesSection()
        {
            EditorGUILayout.LabelField("Utilities", EditorStyles.boldLabel);
            using (new EditorGUILayout.VerticalScope("box"))
            {
                EditorGUILayout.LabelField("One-click fixes for common authoring issues:");
                if (GUILayout.Button("Repair All Topics (recreate missing lists)", GUILayout.Height(24)))
                {
                    RepairAllTopics();
                }
            }
        }

        private void DrawValidationSection()
        {
            EditorGUILayout.LabelField("Validation", EditorStyles.boldLabel);
            using (new EditorGUILayout.VerticalScope("box"))
            {
                EditorGUILayout.LabelField("Run built-in topic validations:");
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Validate ALL Topics", GUILayout.Height(24)))
                {
                    ValidateAllTopics();
                }
                if (GUILayout.Button("Validate SELECTED Topic(s)", GUILayout.Height(24)))
                {
                    ValidateSelectedTopics();
                }
                EditorGUILayout.EndHorizontal();
            }
        }

        private void DrawExportSection()
        {
            EditorGUILayout.LabelField("Export", EditorStyles.boldLabel);
            using (new EditorGUILayout.VerticalScope("box"))
            {
                EditorGUILayout.LabelField("Choose what to export:");
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Export ALL Topics…", GUILayout.Height(28)))
                {
                    ExportAllTopics();
                }
                if (GUILayout.Button("Export SELECTED Topic(s)…", GUILayout.Height(28)))
                {
                    ExportSelectedTopics();
                }
                EditorGUILayout.EndHorizontal();
            }
        }

        private void DrawImportSection()
        {
            EditorGUILayout.LabelField("Import", EditorStyles.boldLabel);
            using (new EditorGUILayout.VerticalScope("box"))
            {
                importConnectionsOnly = EditorGUILayout.ToggleLeft("Connections only (don’t overwrite name/description/country etc.)", importConnectionsOnly);
                validateAfterImport = EditorGUILayout.ToggleLeft("Validate topics after import", validateAfterImport);

                if (GUILayout.Button("Import from JSON…", GUILayout.Height(28)))
                {
                    ImportFromJson();
                }
            }
        }

        // ==== Export ====
        private void ExportAllTopics()
        {
            try
            {
                var guids = AssetDatabase.FindAssets("t:TopicData");
                var topics = new List<TopicData>(guids.Length);
                foreach (var guid in guids)
                {
                    var path = AssetDatabase.GUIDToAssetPath(guid);
                    var t = AssetDatabase.LoadAssetAtPath<TopicData>(path);
                    if (t != null)
                        topics.Add(t);
                }
                ExportTopics(topics);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Topic export failed: {ex}");
                lastLog = ex.ToString();
            }
        }

        private void ExportSelectedTopics()
        {
            var sel = Selection.objects;
            var topics = new List<TopicData>();
            foreach (var o in sel)
            {
                if (o is TopicData td)
                    topics.Add(td);
            }
            if (topics.Count == 0)
            {
                EditorUtility.DisplayDialog("Export Selected", "Select one or more TopicData assets in the Project view.", "OK");
                return;
            }
            ExportTopics(topics);
        }

        private void ExportTopics(List<TopicData> topics)
        {
            if (topics == null || topics.Count == 0)
            {
                EditorUtility.DisplayDialog("Export", "No topics to export.", "OK");
                return;
            }

            string path = EditorUtility.SaveFilePanel("Export Topics JSON", Application.dataPath, "topics_export.json", "json");
            if (string.IsNullOrEmpty(path))
                return;

            var bundle = new TopicJsonBundle
            {
                version = 1,
                exportedAt = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"),
                topics = topics.Select(TopicJson.FromAsset).OrderBy(t => t.id, StringComparer.OrdinalIgnoreCase).ToList()
            };

            string json = EditorJsonUtility.ToJson(bundle, true);
            File.WriteAllText(path, json);
            EditorUtility.RevealInFinder(path);
            lastLog = $"Exported {bundle.topics.Count} topic(s) to\n{path}";
        }

        // ==== Import ====
        private void ImportFromJson()
        {
            string path = EditorUtility.OpenFilePanel("Import Topics JSON", Application.dataPath, "json");
            if (string.IsNullOrEmpty(path))
                return;
            try
            {
                string json = File.ReadAllText(path);
                var bundle = new TopicJsonBundle();
                EditorJsonUtility.FromJsonOverwrite(json, bundle);
                if (bundle.topics == null || bundle.topics.Count == 0)
                {
                    EditorUtility.DisplayDialog("Import", "No topics found in JSON.", "OK");
                    return;
                }

                // Build lookup dictionaries for assets
                var topicsById = FindAllTopicsById();
                var cardsById = FindAllCardsById();

                int updated = 0;
                int skipped = 0;
                int errors = 0;
                try
                { EditorUtility.DisplayProgressBar("Import Topics", "Applying changes…", 0f); }
                catch { }

                for (int i = 0; i < bundle.topics.Count; i++)
                {
                    var tj = bundle.topics[i];
                    float p = (i + 1f) / bundle.topics.Count;
                    try
                    { EditorUtility.DisplayProgressBar("Import Topics", tj.id, p); }
                    catch { }

                    if (string.IsNullOrEmpty(tj.id) || !topicsById.TryGetValue(tj.id, out var tasset))
                    {
                        Debug.LogWarning($"[Topic Import] Topic id '{tj.id}' not found. Skipping.");
                        skipped++;
                        continue;
                    }

                    Undo.RecordObject(tasset, "Import Topic JSON");

                    if (!importConnectionsOnly)
                    {
                        // Basic fields
                        tasset.Name = tj.name;
                        tasset.Description = tj.description;
                        tasset.Importance = ParseEnumSafe<Importance>(tj.importance, tasset.Importance);
                        tasset.Country = ParseEnumSafe<Countries>(tj.country, tasset.Country);
                        tasset.CohesionStrength = tj.cohesionStrength > 0 ? Mathf.Clamp01(tj.cohesionStrength) : tasset.CohesionStrength;
                        tasset.TargetAge = ParseEnumSafe<AgeRange>(tj.targetAge, tasset.TargetAge);
                        // Subjects
                        tasset.Subjects = (tj.subjects != null && tj.subjects.Count > 0)
                            ? tj.subjects.Select(s => ParseEnumSafe<Subject>(s, Subject.None)).Distinct().ToList()
                            : tasset.Subjects;

                        // Core card
                        if (!string.IsNullOrEmpty(tj.coreCardId) && cardsById.TryGetValue(tj.coreCardId, out var core))
                            tasset.CoreCard = core;
                        else if (!string.IsNullOrEmpty(tj.coreCardId))
                            Debug.LogWarning($"[Topic Import] Core card '{tj.coreCardId}' not found for topic '{tj.id}'. Keeping previous.");
                    }

                    // Connections
                    if (tj.connections != null)
                    {
                        var newConns = new List<CardConnection>(tj.connections.Count);
                        foreach (var cj in tj.connections)
                        {
                            if (string.IsNullOrEmpty(cj.connectedCardId))
                                continue;
                            if (!cardsById.TryGetValue(cj.connectedCardId, out var casset))
                            {
                                Debug.LogWarning($"[Topic Import] Card '{cj.connectedCardId}' not found. Skipping connection for topic '{tj.id}'.");
                                continue;
                            }
                            var cc = new CardConnection
                            {
                                ConnectedCard = casset,
                                ConnectionType = ParseEnumSafe<ConnectionType>(cj.connectionType, ConnectionType.RelatedTo),
                                ConnectionStrength = Mathf.Clamp(cj.connectionStrength, 0.1f, 1.0f),
                                ConnectionReason = cj.connectionReason,
                                LinkingKeywords = cj.linkingKeywords != null ? new List<string>(cj.linkingKeywords) : new List<string>(),
                                LearningValue = cj.learningValue,
                                TimeKind = ParseEnumSafe<TimeContextKind>(cj.timeKind, TimeContextKind.Period),
                                CompareKind = ParseEnumSafe<CompareKind>(cj.compareKind, CompareKind.Similar),
                                CulturalRole = ParseEnumSafe<CulturalRole>(cj.culturalRole, CulturalRole.Origin)
                            };
                            newConns.Add(cc);
                        }
                        tasset.Connections = newConns;
                    }

                    // DiscoveryPath removed

                    EditorUtility.SetDirty(tasset);
                    updated++;

                    if (validateAfterImport)
                    {
                        try
                        { tasset.ValidateTopic(); }
                        catch { }
                    }
                }

                AssetDatabase.SaveAssets();
                lastLog = $"Import complete. Updated: {updated}, Skipped: {skipped}, Errors: {errors}";
            }
            catch (Exception ex)
            {
                Debug.LogError($"Topic import failed: {ex}");
                lastLog = ex.ToString();
            }
            finally
            {
                try
                { EditorUtility.ClearProgressBar(); }
                catch { }
            }
        }

        // ==== Helpers ====
        private static Dictionary<string, TopicData> FindAllTopicsById()
        {
            var dict = new Dictionary<string, TopicData>(StringComparer.OrdinalIgnoreCase);
            var guids = AssetDatabase.FindAssets("t:TopicData");
            foreach (var g in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(g);
                var t = AssetDatabase.LoadAssetAtPath<TopicData>(path);
                if (t == null)
                    continue;
                var id = string.IsNullOrEmpty(t.Id) ? t.name : t.Id;
                if (!string.IsNullOrEmpty(id) && !dict.ContainsKey(id))
                    dict.Add(id, t);
            }
            return dict;
        }

        private static Dictionary<string, CardData> FindAllCardsById()
        {
            var dict = new Dictionary<string, CardData>(StringComparer.OrdinalIgnoreCase);
            var guids = AssetDatabase.FindAssets("t:CardData");
            foreach (var g in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(g);
                var c = AssetDatabase.LoadAssetAtPath<CardData>(path);
                if (c == null)
                    continue;
                var id = string.IsNullOrEmpty(c.Id) ? c.name : c.Id;
                if (!string.IsNullOrEmpty(id) && !dict.ContainsKey(id))
                    dict.Add(id, c);
            }
            return dict;
        }

        private static TEnum ParseEnumSafe<TEnum>(string value, TEnum fallback) where TEnum : struct
        {
            if (!string.IsNullOrEmpty(value) && Enum.TryParse<TEnum>(value, true, out var e))
                return e;
            return fallback;
        }

        private void RepairAllTopics()
        {
            try
            {
                var guids = AssetDatabase.FindAssets("t:TopicData");
                int repaired = 0;
                for (int i = 0; i < guids.Length; i++)
                {
                    var path = AssetDatabase.GUIDToAssetPath(guids[i]);
                    var t = AssetDatabase.LoadAssetAtPath<TopicData>(path);
                    if (t == null)
                        continue;
                    try
                    { EditorUtility.DisplayProgressBar("Repair Topics", t.name, (i + 1f) / guids.Length); }
                    catch { }

                    bool changed = false;
                    if (t.Connections == null)
                    {
                        Undo.RecordObject(t, "Repair Topic (Connections)");
                        t.Connections = new List<CardConnection>();
                        changed = true;
                    }
                    // DiscoveryPath removed
                    if (changed)
                    {
                        EditorUtility.SetDirty(t);
                        repaired++;
                    }
                }
                AssetDatabase.SaveAssets();
                lastLog = $"Repair complete. Updated {repaired} topic(s).";
            }
            catch (Exception ex)
            {
                Debug.LogError($"Repair failed: {ex}");
                lastLog = ex.ToString();
            }
            finally
            {
                try
                { EditorUtility.ClearProgressBar(); }
                catch { }
            }
        }

        // ===== Validation =====
        private void ValidateAllTopics()
        {
            try
            {
                var guids = AssetDatabase.FindAssets("t:TopicData");
                var topics = new List<TopicData>(guids.Length);
                foreach (var g in guids)
                {
                    var path = AssetDatabase.GUIDToAssetPath(g);
                    var t = AssetDatabase.LoadAssetAtPath<TopicData>(path);
                    if (t != null)
                        topics.Add(t);
                }
                RunValidation(topics, "All Topics");
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
                lastLog = ex.ToString();
            }
        }

        private void ValidateSelectedTopics()
        {
            var sel = Selection.objects;
            var topics = new List<TopicData>();
            foreach (var o in sel)
            {
                if (o is TopicData td)
                    topics.Add(td);
            }
            if (topics.Count == 0)
            {
                EditorUtility.DisplayDialog("Validate Selected", "Select one or more TopicData assets in the Project view.", "OK");
                return;
            }
            RunValidation(topics, "Selected Topics");
        }

        private void RunValidation(List<TopicData> topics, string label)
        {
            int ok = 0, warn = 0;
            var sb = new StringBuilder();
            sb.AppendLine($"Validation report: {label}");
            try
            { EditorUtility.DisplayProgressBar("Validate Topics", label, 0f); }
            catch { }
            for (int i = 0; i < topics.Count; i++)
            {
                var t = topics[i];
                float p = (i + 1f) / topics.Count;
                try
                { EditorUtility.DisplayProgressBar("Validate Topics", t != null ? t.name : "(null)", p); }
                catch { }
                if (t == null)
                { continue; }
                var issues = ComputeTopicIssues(t);
                if (issues.Count == 0)
                {
                    ok++;
                }
                else
                {
                    warn++;
                    sb.AppendLine($"- {t.name}:");
                    foreach (var isx in issues)
                        sb.AppendLine($"    • {isx}");
                }
            }
            lastLog = sb.AppendLine($"\nPassed: {ok}, With issues: {warn}, Total: {topics.Count}").ToString();
            try
            { EditorUtility.ClearProgressBar(); }
            catch { }
        }

        private static List<string> ComputeTopicIssues(TopicData t)
        {
            var issues = new List<string>();
            if (t == null)
            { issues.Add("Topic is null"); return issues; }

            if (string.IsNullOrEmpty(t.Name))
                issues.Add("Topic name is empty");
            if (t.CoreCard == null)
                issues.Add("No core card assigned");

            if (t.Connections == null || t.Connections.Count == 0)
                issues.Add("No connections defined");
            else
            {
                var set = new HashSet<CardData>();
                foreach (var c in t.Connections)
                {
                    var card = c != null ? c.ConnectedCard : null;
                    if (card == null)
                        continue;
                    if (!set.Add(card))
                        issues.Add($"Duplicate connection to {card.name}");
                }
            }

            // DiscoveryPath removed

            return issues;
        }
    }

    // ===== JSON DTOs =====
    [Serializable]
    public class TopicJsonBundle
    {
        public int version = 1;
        public string exportedAt;
        public List<TopicJson> topics = new List<TopicJson>();
    }

    [Serializable]
    public class TopicJson
    {
        public string id;
        public string name;
        public string description;
        public string importance;   // enum name
        public string country;      // enum name
        public string coreCardId;
        public float cohesionStrength;
        public string targetAge;    // enum name
        public List<string> subjects; // enum names
        public List<ConnectionJson> connections = new List<ConnectionJson>();

        public static TopicJson FromAsset(TopicData t)
        {
            var tj = new TopicJson
            {
                id = string.IsNullOrEmpty(t.Id) ? t.name : t.Id,
                name = t.Name,
                description = t.Description,
                importance = t.Importance.ToString(),
                country = t.Country.ToString(),
                coreCardId = t.CoreCard ? (string.IsNullOrEmpty(t.CoreCard.Id) ? t.CoreCard.name : t.CoreCard.Id) : null,
                cohesionStrength = t.CohesionStrength,
                targetAge = t.TargetAge.ToString(),
                subjects = (t.Subjects != null && t.Subjects.Count > 0) ? t.Subjects.Select(s => s.ToString()).ToList() : new List<string>(),
                connections = new List<ConnectionJson>()
            };
            // discoveryPath removed

            if (t.Connections != null)
            {
                foreach (var c in t.Connections)
                {
                    if (c == null || c.ConnectedCard == null)
                        continue;
                    var cid = string.IsNullOrEmpty(c.ConnectedCard.Id) ? c.ConnectedCard.name : c.ConnectedCard.Id;
                    tj.connections.Add(new ConnectionJson
                    {
                        connectedCardId = cid,
                        connectionType = c.ConnectionType.ToString(),
                        connectionStrength = c.ConnectionStrength,
                        connectionReason = c.ConnectionReason,
                        linkingKeywords = c.LinkingKeywords != null ? new List<string>(c.LinkingKeywords) : new List<string>(),
                        learningValue = c.LearningValue,
                        timeKind = c.TimeKind.ToString(),
                        compareKind = c.CompareKind.ToString(),
                        culturalRole = c.CulturalRole.ToString()
                    });
                }
            }

            return tj;
        }
    }

    [Serializable]
    public class ConnectionJson
    {
        public string connectedCardId;
        public string connectionType; // enum name
        public float connectionStrength;
        public string connectionReason;
        public List<string> linkingKeywords;
        public string learningValue;
        public string timeKind;     // enum name
        public string compareKind;  // enum name
        public string culturalRole; // enum name
    }
}
#endif
