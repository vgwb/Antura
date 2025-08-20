#if UNITY_EDITOR
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Antura.Discover
{
    public class DataHealthWindow : EditorWindow
    {
        private Vector2 _scroll;
        private readonly List<string> _log = new List<string>();
        private bool _verbose = true;
        private bool _dryRun = false;

        [MenuItem("Antura/Discover/Data Health")]
        public static void Open() => GetWindow<DataHealthWindow>("Data Health");

        void OnGUI()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Antura • Data Health", EditorStyles.boldLabel);
            _verbose = EditorGUILayout.ToggleLeft("Verbose logging", _verbose);
            _dryRun = EditorGUILayout.ToggleLeft("Dry run (no changes)", _dryRun);

            EditorGUILayout.Space(6);
            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Scan", GUILayout.Height(28)))
                    Run(scanOnly: true);
                if (GUILayout.Button("Fix All", GUILayout.Height(28)))
                    Run(scanOnly: false);
                if (GUILayout.Button("Check World Prefab Ids", GUILayout.Height(28)))
                {
                    _log.Clear();
                    int changed = DataHealthUtility.CheckWorldPrefabIds(applyChanges: !_dryRun, logs: _log, verbose: _verbose);
                    _log.Add($"— Done. WorldPrefabData ID check. Modified prefabs: {changed}");
                    Repaint();
                }
            }

            // New chapter: Data Maintenance
            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("Data Maintenance", EditorStyles.boldLabel);
            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Sync Card ↔ Quest links", GUILayout.Height(24)))
                {
                    _log.Clear();
                    int changed = InvokeSyncCardQuestLinks(applyChanges: !_dryRun, logs: _log, verbose: _verbose);
                    _log.Add($"— Done. Synced Card↔Quest links. Modified assets: {changed}");
                    Repaint();
                }
                if (GUILayout.Button("Show Card↔Quest relationships", GUILayout.Height(24)))
                {
                    _log.Clear();
                    InvokePrintCardQuestRelationships(_log, includeOrphans: true);
                    Repaint();
                }
                if (GUILayout.Button("Unlink non-reciprocal + clean", GUILayout.Height(24)))
                {
                    _log.Clear();
                    int changed = InvokeUnlinkNonReciprocalCardQuestLinks(applyChanges: !_dryRun, logs: _log, verbose: _verbose);
                    _log.Add($"— Done. Unlinked/cleaned. Modified assets: {changed}");
                    Repaint();
                }
            }

            EditorGUILayout.Space(8);
            EditorGUILayout.LabelField("Log", EditorStyles.boldLabel);
            _scroll = EditorGUILayout.BeginScrollView(_scroll);
            foreach (var line in _log)
                EditorGUILayout.LabelField(line, EditorStyles.wordWrappedLabel);
            EditorGUILayout.EndScrollView();
        }

        void Run(bool scanOnly)
        {
            _log.Clear();
            var changed = DataHealthUtility.ScanAndFixAll(applyChanges: !scanOnly, logs: _log, verbose: _verbose);
            _log.Add($"— Done. {(scanOnly ? "Scanned" : "Fixed")} changes: {changed}");
            Repaint();
        }

        // Reflection-based wrappers to avoid compile-order issues
        private static int InvokeSyncCardQuestLinks(bool applyChanges, List<string> logs, bool verbose)
        {
            var t = typeof(DataHealthUtility);
            var mi = t.GetMethod("SyncCardQuestLinks", BindingFlags.Public | BindingFlags.Static);
            if (mi != null)
            {
                object result = mi.Invoke(null, new object[] { applyChanges, logs, verbose });
                return result is int i ? i : 0;
            }
            logs?.Add("[DataHealth] SyncCardQuestLinks() not found.");
            return 0;
        }

        private static void InvokePrintCardQuestRelationships(List<string> logs, bool includeOrphans)
        {
            var t = typeof(DataHealthUtility);
            var mi = t.GetMethod("PrintCardQuestRelationships", BindingFlags.Public | BindingFlags.Static);
            if (mi != null)
            {
                mi.Invoke(null, new object[] { logs, includeOrphans });
                return;
            }
            logs?.Add("[DataHealth] PrintCardQuestRelationships() not found.");
        }

        private static int InvokeUnlinkNonReciprocalCardQuestLinks(bool applyChanges, List<string> logs, bool verbose)
        {
            var t = typeof(DataHealthUtility);
            var mi = t.GetMethod("UnlinkNonReciprocalCardQuestLinks", BindingFlags.Public | BindingFlags.Static);
            if (mi != null)
            {
                object result = mi.Invoke(null, new object[] { applyChanges, logs, verbose });
                return result is int i ? i : 0;
            }
            logs?.Add("[DataHealth] UnlinkNonReciprocalCardQuestLinks() not found.");
            return 0;
        }
    }
}
#endif
