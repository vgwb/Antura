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

        [MenuItem("Antura/Database/Data Health", priority = 151)]
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
            // Mirror log to Console for quick navigation; detailed item warnings include object context
            if (_log.Count > 0)
                Debug.Log(string.Join("\n", _log));
            Repaint();
        }
    }
}
#endif
