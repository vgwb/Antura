using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Antura.Discover
{
    public static class DatabaseProvider
    {
        // Change if you place the DB somewhere else under Resources
        private const string ResourcesPath = "Database/Database";

        private static DatabaseManager _instance;
        public static DatabaseManager I
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.Load<DatabaseManager>(ResourcesPath);
#if UNITY_EDITOR
                    if (_instance == null)
                    {
                        // Editor fallback: try find any DatabaseManager in the project
                        var guids = AssetDatabase.FindAssets("t:Antura.Discover.DatabaseManager");
                        if (guids.Length > 0)
                        {
                            var path = AssetDatabase.GUIDToAssetPath(guids[0]);
                            _instance = AssetDatabase.LoadAssetAtPath<DatabaseManager>(path);
                        }
                    }
#endif
                    if (_instance != null)
                        _instance.BuildIndex();
                    else
                        Debug.LogError($"[DatabaseProvider] Could not locate Database at Resources/{ResourcesPath}");
                }
                return _instance;
            }
        }

        /// <summary>Manually set the active database (useful for testing or Addressables).</summary>
        public static void Set(DatabaseManager db)
        {
            _instance = db;
            _instance?.BuildIndex();
        }

        // Sugar
        public static T Get<T>(string id) where T : IdentifiedData => I.Get<T>(id);
        public static bool TryGet<T>(string id, out T data) where T : IdentifiedData => I.TryGet(id, out data);
    }
}
