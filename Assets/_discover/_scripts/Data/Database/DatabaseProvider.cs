using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Antura.Discover
{
    public static class DatabaseProvider
    {
        // Change if you place the DB somewhere else under Resources
        private static readonly string[] ResourcePaths =
        {
            "Database/Database",   // Legacy location
            "DiscoverDatabase"     // Current shipping asset
        };

        private static DatabaseManager _instance;
        private static bool _missingLogged;
        private static string _lastLoadedPath;
        public static DatabaseManager I
        {
            get
            {
                if (_instance == null)
                {
                    Debug.Log("[DatabaseProvider] Instance null. Beginning load sequence.");
                    _instance = LoadFromResources();
#if UNITY_EDITOR
                    if (_instance == null)
                    {
                        Debug.Log("[DatabaseProvider] Resources load failed. Attempting editor asset search.");
                        // Editor fallback: try find any DatabaseManager in the project
                        var guids = AssetDatabase.FindAssets("t:Antura.Discover.DatabaseManager");
                        if (guids.Length > 0)
                        {
                            var path = AssetDatabase.GUIDToAssetPath(guids[0]);
                            Debug.Log($"[DatabaseProvider] Editor fallback found asset at {path}");
                            _instance = AssetDatabase.LoadAssetAtPath<DatabaseManager>(path);
                        }
                        else
                        {
                            Debug.LogWarning("[DatabaseProvider] Editor fallback did not find any DatabaseManager asset.");
                        }
                    }
#endif
                    if (_instance != null)
                    {
                        Debug.Log("[DatabaseProvider] Database instance acquired. Building index...");
                        _instance.BuildIndex();
                        LogLoaded("Resources", _lastLoadedPath, _instance);
                    }
                    else if (!_missingLogged)
                    {
                        _missingLogged = true;
                        Debug.LogError($"[DatabaseProvider] Could not locate a Discover Database asset in Resources. Checked: {string.Join(", ", ResourcePaths)}");
                    }
                }
                return _instance;
            }
        }

        /// <summary>Manually set the active database (useful for testing or Addressables).</summary>
        public static void Set(DatabaseManager db)
        {
            _instance = db;
            _missingLogged = false;
            _instance?.BuildIndex();
            if (_instance != null)
            {
                LogLoaded("Manual Set", "<runtime>", _instance);
            }
        }

        public static T Get<T>(string id) where T : IdentifiedData
        {
            //            Debug.Log($"[DatabaseProvider] Get<{typeof(T).Name}> requested for id '{id}'");
            var db = I;
            if (db == null)
                throw new InvalidOperationException("[DatabaseProvider] Database not available. Ensure a DatabaseManager asset is under Resources.");
            return db.Get<T>(id);
        }

        public static bool TryGet<T>(string id, out T data) where T : IdentifiedData
        {
            //            Debug.Log($"[DatabaseProvider] TryGet<{typeof(T).Name}> requested for id '{id}'");
            data = null;
            var db = I;
            if (db == null)
            {
                Debug.LogWarning("[DatabaseProvider] TryGet aborted: database instance is null.");
                return false;
            }
            return db != null && db.TryGet(id, out data);
        }

        private static DatabaseManager LoadFromResources()
        {
            foreach (var path in ResourcePaths)
            {
                if (string.IsNullOrWhiteSpace(path))
                    continue;
                Debug.Log($"[DatabaseProvider] Attempting to load Discover Database from Resources/{path}");
                var db = Resources.Load<DatabaseManager>(path);
                if (db != null)
                {
                    _missingLogged = false;
                    _lastLoadedPath = path;
                    return db;
                }
                else
                {
                    Debug.LogWarning($"[DatabaseProvider] No asset found at Resources/{path}");
                }
            }
            Debug.LogError("[DatabaseProvider] LoadFromResources exhausted all paths without success.");
            return null;
        }

        private static void LogLoaded(string origin, string path, DatabaseManager db)
        {
            if (db == null)
                return;
            var entries = db.AllData != null ? db.AllData.Count : 0;
            Debug.Log($"[DatabaseProvider] Loaded Discover Database via {origin} ({path}) with {entries} entries.");
        }
    }
}
