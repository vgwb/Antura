using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// this is the main database of the Discover system.
// every identified data (ActivityData, CardData, ItemData, etc) must be registered here.
// the editor script DatabaseManagerEditor has a "Rebuild" command that scans all assets of
// type IdentifiedData and populates the AllData list.
// it also checks for common problems (missing ids, duplicate ids, missing references, etc).

/*
// Fetch anywhere
// var db = DatabaseProvider.Instance;
var sword = db.Get<ItemData>("steel_sword");
if (db.TryGet<AssetData>("baguette", out var art)) {
    // use art.Image / art.Audio
}
*/

namespace Antura.Discover
{
    [CreateAssetMenu(fileName = "Database", menuName = "Antura/Discover/Database")]
    public class DatabaseManager : ScriptableObject
    {
        [Tooltip("Drop all IdentifiedData here. The Editor 'Rebuild' can auto-populate this.")]
        public List<IdentifiedData> AllData = new List<IdentifiedData>();

        // Runtime index
        [NonSerialized] private Dictionary<Type, Dictionary<string, IdentifiedData>> _index;

        void OnEnable() => BuildIndex();

        public void BuildIndex()
        {
            _index = new Dictionary<Type, Dictionary<string, IdentifiedData>>();
            foreach (var d in AllData)
            {
                if (d == null || string.IsNullOrEmpty(d.Id))
                    continue;
                var t = d.GetType();
                if (!_index.TryGetValue(t, out var map))
                {
                    map = new Dictionary<string, IdentifiedData>(StringComparer.OrdinalIgnoreCase);
                    _index[t] = map;
                }
                if (!map.ContainsKey(d.Id))
                    map[d.Id] = d;
                else
                    Debug.LogError($"[Database] Duplicate id for type {t.Name}: '{d.Id}'. Keeping first, ignoring '{d.name}'.");
            }
        }

        public void RebuildIndexIfNeeded()
        {
            if (_index == null)
                BuildIndex();
        }

        public T Get<T>(string id) where T : IdentifiedData
        {
            if (TryGet(id, out T result))
                return result;
            throw new KeyNotFoundException($"[Database] Not found: {typeof(T).Name} id='{id}'");
        }

        public bool TryGet<T>(string id, out T result) where T : IdentifiedData
        {
            result = null;
            if (string.IsNullOrEmpty(id))
                return false;
            if (_index != null && _index.TryGetValue(typeof(T), out var map) &&
                map.TryGetValue(id, out var obj))
            {
                result = obj as T;
                return result != null;
            }
            return false;
        }

        public IEnumerable<T> All<T>() where T : IdentifiedData
        {
            if (_index != null && _index.TryGetValue(typeof(T), out var map))
                return map.Values.Cast<T>();
            return Array.Empty<T>();
        }
    }
}
