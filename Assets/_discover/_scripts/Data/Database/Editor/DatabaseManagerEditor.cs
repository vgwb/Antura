#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// alla data in the project is stored in a single ScriptableObject asset of type DatabaseManager.
// and inherits IndentifiedData (which has an Id field).
// we need to keep this database updated, because many commands search for any data here.
// if there are some errores, use the "Rebuild" command to fix them.
// run this before any build.

namespace Antura.Discover
{
    [CustomEditor(typeof(DatabaseManager))]
    public class DatabaseManagerEditor : UnityEditor.Editor
    {
        private static DatabaseManager _cachedInstance;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var db = (DatabaseManager)target;
            _cachedInstance = db;
            EditorGUILayout.Space();

            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Rebuild (scan + fix + index)"))
                {
                    RebuildAndFix(db);
                }
                if (GUILayout.Button("Validate"))
                {
                    Validate(db);
                }
            }
        }

        static void RebuildAndFix(DatabaseManager db)
        {
            // 1) Scan & fix all data in project
            var logs = new List<string>();
            var changed = DataHealthUtility.ScanAndFixAll(applyChanges: true, logs: logs, verbose: false);
            foreach (var l in logs)
                Debug.Log(l);
            Debug.Log($"[Database] DataHealth fixed {changed} assets.");

            // 2) Rebuild the list
            var all = new List<IdentifiedData>();
            foreach (var guid in AssetDatabase.FindAssets("t:Antura.Discover.IdentifiedData"))
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var obj = AssetDatabase.LoadAssetAtPath<IdentifiedData>(path);
                if (obj != null)
                    all.Add(obj);
            }

            Undo.RecordObject(db, "Rebuild Database");
            db.AllData = all;
            EditorUtility.SetDirty(db);
            AssetDatabase.SaveAssets();

            // 3) Re-index
            db.BuildIndex();

            Debug.Log($"[Database] Rebuilt. Items: {db.AllData.Count}");
        }

        static void Validate(DatabaseManager db)
        {
            var dupes = new HashSet<string>(System.StringComparer.OrdinalIgnoreCase);
            var seenByType = new Dictionary<System.Type, HashSet<string>>();

            foreach (var d in db.AllData)
            {
                if (d == null)
                    continue;
                if (string.IsNullOrEmpty(d.Id))
                {
                    Debug.LogWarning($"[Database] Empty ID in '{d.name}' ({d.GetType().Name})");
                    continue;
                }
                var t = d.GetType();
                if (!seenByType.TryGetValue(t, out var set))
                    seenByType[t] = set = new HashSet<string>(System.StringComparer.OrdinalIgnoreCase);

                if (!set.Add(d.Id))
                    dupes.Add($"{t.Name}:{d.Id}");
            }

            if (dupes.Count == 0)
                Debug.Log("[Database] OK: no duplicates.");
            else
                foreach (var d in dupes)
                    Debug.LogError($"[Database] Duplicate: {d}");
        }

        [MenuItem("Antura/Database/Rebuild", priority = 200)]
        private static void RebuildMenu()
        {
            var db = FindOrCreateDatabaseManager();
            if (db == null)
            {
                Debug.LogError("[Database] Could not locate a DatabaseManager in the project.");
                return;
            }
            RebuildAndFix(db);
        }

        [MenuItem("Antura/Database/Validate", priority = 201)]
        private static void ValidateMenu()
        {
            var db = FindOrCreateDatabaseManager();
            if (db == null)
            {
                Debug.LogError("[Database] Could not locate a DatabaseManager in the project.");
                return;
            }
            Validate(db);
        }

        private static DatabaseManager FindOrCreateDatabaseManager()
        {
            if (_cachedInstance != null)
                return _cachedInstance;

            _cachedInstance = FindFirstObjectByType<DatabaseManager>();
            if (_cachedInstance != null)
                return _cachedInstance;

            // Fallback: search assets for a DatabaseManager ScriptableObject or prefab (depends on implementation)
            var guids = AssetDatabase.FindAssets("t:DatabaseManager");
            foreach (var g in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(g);
                var obj = AssetDatabase.LoadAssetAtPath<DatabaseManager>(path);
                if (obj != null)
                {
                    _cachedInstance = obj;
                    return _cachedInstance;
                }
            }

            return _cachedInstance;
        }
    }
}
#endif
