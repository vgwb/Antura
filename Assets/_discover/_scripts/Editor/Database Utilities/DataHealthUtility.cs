#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Antura.Discover
{
    public static class DataHealthUtility
    {
        private static readonly string[] AdditionalIdentifiedTypes = new[]
        {
            "ActivityData",
            "ActivitySettingsAbstract",
            "BonusMalusData",
            "AssetData",
            "TaskData",
            "CardData",
            "QuestData",
            "WordData",
        };
        // ---------- Public entry points ----------

        /// <summary>Find all data of the given type in the project.</summary>
        public static List<T> FindAll<T>() where T : ScriptableObject
        {
            var list = new List<T>();
            foreach (var guid in AssetDatabase.FindAssets($"t:{typeof(T).FullName}"))
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var obj = AssetDatabase.LoadAssetAtPath<T>(path);
                if (obj)
                    list.Add(obj);
            }
            if (list.Count == 0)
            {
                foreach (var guid in AssetDatabase.FindAssets($"t:{typeof(T).Name}"))
                {
                    var path = AssetDatabase.GUIDToAssetPath(guid);
                    var obj = AssetDatabase.LoadAssetAtPath<T>(path);
                    if (obj)
                        list.Add(obj);
                }
            }
            return list;
        }

        /// <summary>
        /// Scan/fix all data. Returns number of modified assets. Populates logs.
        /// </summary>
        public static int ScanAndFixAll(bool applyChanges, List<string> logs, bool verbose = true)
        {
            // Gather specific types we know we want to treat specially
            var assets = FindAll<AssetData>();
            var items = FindAll<ItemData>();

            // Gather any IdentifiedData-derived assets by type name (if present in this project)
            var identified = new List<IdentifiedData>();
            var typeCounts = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            foreach (var typeName in AdditionalIdentifiedTypes)
            {
                var list = TryFindAllByTypeName(typeName);
                typeCounts[typeName] = list.Count;
                foreach (var obj in list)
                {
                    if (obj is IdentifiedData id && !identified.Contains(id))
                        identified.Add(id);
                }
            }

            // Merge AssetData into identified list (to ensure same ID rules)
            foreach (var a in assets)
                if (a != null && !identified.Contains(a))
                    identified.Add(a);

            // Report counts
            logs?.Add($"Found AssetData: {assets.Count}, ItemData: {items.Count}");
            foreach (var kv in typeCounts)
                logs?.Add($"Found {kv.Key}: {kv.Value}");

            return Fix(identified, assets, items, applyChanges, logs, verbose);
        }

        /// <summary>
        /// Fix the provided AssetData/ItemData sets. Ensures unique IDs across both.
        /// </summary>
        public static int Fix(List<IdentifiedData> identifiedAll, List<AssetData> assets, List<ItemData> items, bool applyChanges, List<string> logs, bool verbose = true)
        {
            int changes = 0;
            var usedIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            // Seed with current IDs across all IdentifiedData we found (including AssetData & ItemData if they derive)
            foreach (var id in identifiedAll)
                if (!string.IsNullOrEmpty(id?.Id))
                    usedIds.Add(id.Id);
            foreach (var i in items)
                if (i != null && !string.IsNullOrEmpty(i.Id))
                    usedIds.Add(i.Id);

            // Pass 1: Generic rules for all IdentifiedData (sanitize + uniqueness)
            foreach (var data in identifiedAll.Where(x => x != null))
            {
                var path = AssetDatabase.GetAssetPath(data);
                var fileBase = System.IO.Path.GetFileNameWithoutExtension(path);
                bool dirty = false;

                // ID sanitize + uniqueness
                var desired = string.IsNullOrEmpty(data.Id) ? fileBase : data.Id;
                desired = IdentifiedData.SanitizeId(desired);

                if (string.IsNullOrEmpty(data.Id))
                    logs?.Add($"[{data.GetType().Name}:{fileBase}] Missing Id → '{desired}'");

                if (!string.Equals(data.Id, desired, StringComparison.Ordinal))
                {
                    if (verbose)
                        logs?.Add($"[{data.GetType().Name}:{fileBase}] Id '{data.Id}' → '{desired}'");
                    if (applyChanges)
                    { data.Editor_SetId(desired); dirty = true; }
                }

                usedIds.RemoveWhere(id => string.Equals(id, data.Id, StringComparison.OrdinalIgnoreCase));
                var unique = EnsureUnique(desired, usedIds);
                if (!string.Equals(unique, data.Id, StringComparison.Ordinal))
                {
                    logs?.Add($"[{data.GetType().Name}:{fileBase}] Id duplicate → '{data.Id}' → '{unique}'");
                    if (applyChanges)
                    { data.Editor_SetId(unique, lockAfter: true); dirty = true; }
                }
                usedIds.Add(data.Id);

                if (applyChanges && dirty)
                { EditorUtility.SetDirty(data); changes++; }
            }

            // Pass 2: Specific rules for AssetData
            foreach (var a in assets.Where(x => x != null))
            {
                var path = AssetDatabase.GetAssetPath(a);
                var fileBase = System.IO.Path.GetFileNameWithoutExtension(path);
                bool dirty = false;

                // Type vs references
                bool hasImg = a.Image != null;
                bool hasAud = a.Audio != null;

                if (hasImg && hasAud)
                {
                    logs?.Add($"[AssetData:{fileBase}] Both Image & Audio set. Keeping '{a.Type}', clearing the other.");
                    if (applyChanges)
                    {
                        if (a.Type == AssetType.Image)
                            a.Audio = null;
                        else
                            a.Image = null;
                        dirty = true;
                    }
                }
                else if (hasImg && a.Type != AssetType.Image)
                {
                    if (verbose)
                        logs?.Add($"[AssetData:{fileBase}] Type mismatch → set Type=Image.");
                    if (applyChanges)
                    { a.Type = AssetType.Image; dirty = true; }
                }
                else if (hasAud && a.Type != AssetType.Audio)
                {
                    if (verbose)
                        logs?.Add($"[AssetData:{fileBase}] Type mismatch → set Type=Audio.");
                    if (applyChanges)
                    { a.Type = AssetType.Audio; dirty = true; }
                }

                if (applyChanges && dirty)
                { EditorUtility.SetDirty(a); changes++; }
            }

            // Process ItemData
            foreach (var it in items.Where(x => x != null))
            {
                var path = AssetDatabase.GetAssetPath(it);
                var fileBase = System.IO.Path.GetFileNameWithoutExtension(path);
                bool dirty = false;

                if (it.Icon == null)
                    logs?.Add($"[ItemData:{fileBase}] Icon is missing. (Warning)");

                if (applyChanges && dirty)
                { EditorUtility.SetDirty(it); changes++; }
            }

            if (applyChanges && changes > 0)
            {
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

            return changes;
        }

        // ---------- Helpers ----------

        private static string EnsureUnique(string desired, HashSet<string> used)
        {
            if (!used.Contains(desired))
                return desired;
            for (int i = 2; i < 100; i++)
            {
                var candidate = $"{desired}_{i}";
                if (!used.Contains(candidate))
                    return candidate;
            }
            var sg = Guid.NewGuid().ToString("N").Substring(0, 6);
            return $"{desired}_{sg}";
        }

        // Reflection-based finder for types that may not exist in all editions
        private static List<IdentifiedData> TryFindAllByTypeName(string typeName)
        {
            var list = new List<IdentifiedData>();
            if (string.IsNullOrEmpty(typeName))
                return list;
            var guids = AssetDatabase.FindAssets($"t:{typeName}");
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var obj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
                if (obj is IdentifiedData id)
                    list.Add(id);
            }
            return list;
        }
    }
}
#endif
