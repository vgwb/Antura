#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using System.IO;

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

            // 1) Build duplicate maps per data type
            var dupByType = new Dictionary<string, Dictionary<string, List<IdentifiedData>>>(StringComparer.OrdinalIgnoreCase);
            foreach (var d in identifiedAll.Where(x => x != null))
            {
                var t = d.GetType().Name;
                var id = d.Id ?? string.Empty;
                if (!dupByType.TryGetValue(t, out var map))
                { map = new Dictionary<string, List<IdentifiedData>>(StringComparer.OrdinalIgnoreCase); dupByType[t] = map; }
                if (!map.TryGetValue(id, out var list))
                { list = new List<IdentifiedData>(); map[id] = list; }
                list.Add(d);
            }

            // 2) Report duplicates (per type) and provide a clear original entry
            foreach (var typeEntry in dupByType)
            {
                foreach (var idEntry in typeEntry.Value)
                {
                    if (!string.IsNullOrEmpty(idEntry.Key) && idEntry.Value.Count > 1)
                    {
                        var ordered = idEntry.Value
                            .OrderBy(x => AssetDatabase.GetAssetPath(x), StringComparer.OrdinalIgnoreCase)
                            .ToList();
                        var original = ordered[0];
                        var originalPath = AssetDatabase.GetAssetPath(original);
                        var dupPaths = ordered.Skip(1).Select(x => AssetDatabase.GetAssetPath(x)).ToList();
                        logs?.Add($"[Duplicates] {typeEntry.Key} Id '{idEntry.Key}' → Original: {originalPath}\nDuplicates ({dupPaths.Count}):\n - " + string.Join("\n - ", dupPaths));
                        Debug.LogWarning($"[Data Health] Duplicate Id (original kept) '{idEntry.Key}' for type {typeEntry.Key}", original);
                        foreach (var obj in ordered.Skip(1))
                            Debug.LogWarning($"[Data Health] Duplicate Id (will be renamed) '{idEntry.Key}' for type {typeEntry.Key}", obj);
                    }
                }
            }

            // 3) Per-type ID counts to decide first occurrence vs duplicates
            var idCountsByType = new Dictionary<string, Dictionary<string, int>>(StringComparer.OrdinalIgnoreCase);
            foreach (var d in identifiedAll.Where(x => x != null))
            {
                var t = d.GetType().Name;
                if (!idCountsByType.TryGetValue(t, out var map))
                { map = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase); idCountsByType[t] = map; }
                var id = d.Id ?? string.Empty;
                if (string.IsNullOrEmpty(id))
                    continue;
                map[id] = map.TryGetValue(id, out var c) ? c + 1 : 1;
            }
            var seenPerIdByType = new Dictionary<string, Dictionary<string, int>>(StringComparer.OrdinalIgnoreCase);

            // Maintain used Ids per type as we fix assets (incremental to preserve first wins)
            var usedIdsPerType = new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);

            // 4) Pass 1: Generic rules for all IdentifiedData (sanitize + per-type uniqueness)
            foreach (var data in identifiedAll.Where(x => x != null))
            {
                var assetPath = AssetDatabase.GetAssetPath(data);
                var fileBase = System.IO.Path.GetFileNameWithoutExtension(assetPath);
                bool dirty = false;

                var typeName = data.GetType().Name;
                if (!usedIdsPerType.TryGetValue(typeName, out var typeSet))
                { typeSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase); usedIdsPerType[typeName] = typeSet; }

                // Sanitize desired Id
                var desired = string.IsNullOrEmpty(data.Id) ? fileBase : data.Id;
                desired = IdentifiedData.SanitizeId(desired);
                if (string.IsNullOrEmpty(data.Id))
                {
                    logs?.Add($"[{data.GetType().Name}:{fileBase}] Missing Id → '{desired}'");
                    Debug.LogWarning($"[Data Health] Missing Id → '{desired}'", data);
                }
                if (!string.Equals(data.Id, desired, StringComparison.Ordinal))
                {
                    if (verbose)
                        logs?.Add($"[{data.GetType().Name}:{fileBase}] Id '{data.Id}' → '{desired}'");
                    Debug.LogWarning($"[Data Health] Id sanitize → '{data.Id}' → '{desired}'", data);
                    if (applyChanges)
                    { data.Editor_SetId(desired); dirty = true; }
                }

                // Compute per-type uniqueness, keeping first occurrence of duplicates
                var otherIds = new HashSet<string>(typeSet, StringComparer.OrdinalIgnoreCase);
                var currentId = data.Id ?? string.Empty;
                bool hasDup = !string.IsNullOrEmpty(currentId)
                              && idCountsByType.TryGetValue(typeName, out var countMap)
                              && countMap.TryGetValue(currentId, out var totalForId)
                              && totalForId > 1;
                if (!seenPerIdByType.TryGetValue(typeName, out var seenMap))
                { seenMap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase); seenPerIdByType[typeName] = seenMap; }
                int seen = !string.IsNullOrEmpty(currentId) && seenMap.TryGetValue(currentId, out var s) ? s : 0;

                // Avoid self-collision by default
                otherIds.RemoveWhere(id => string.Equals(id, currentId, StringComparison.OrdinalIgnoreCase));
                // If this is the first occurrence among duplicates, let it keep the id
                if (hasDup && seen == 0)
                {
                    otherIds.RemoveWhere(id => string.Equals(id, currentId, StringComparison.OrdinalIgnoreCase));
                }

                var unique = EnsureUnique(desired, otherIds);
                if (!string.Equals(unique, data.Id, StringComparison.Ordinal))
                {
                    logs?.Add($"[{data.GetType().Name}:{fileBase}] Id duplicate → '{data.Id}' → '{unique}'");
                    Debug.LogWarning($"[Data Health] Duplicate Id resolved → '{data.Id}' → '{unique}'", data);
                    if (applyChanges)
                    { data.Editor_SetId(unique, lockAfter: true); dirty = true; }
                }

                // Update tracking for this type
                typeSet.Add(data.Id);
                if (!string.IsNullOrEmpty(currentId))
                    seenMap[currentId] = seen + 1;

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
                bool hasImg = a.HasImageAsset;
                bool hasAud = a.HasAudioAsset;

                if (hasImg && hasAud)
                {
                    logs?.Add($"[AssetData:{fileBase}] Both Image & Audio set. Keeping '{a.Type}', clearing the other.");
                    Debug.LogWarning($"[Data Health] AssetData has both Image & Audio set", a);
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
                    Debug.LogWarning($"[Data Health] AssetData type mismatch, set Type=Image", a);
                    if (applyChanges)
                    { a.Type = AssetType.Image; dirty = true; }
                }
                else if (hasAud && a.Type != AssetType.Audio)
                {
                    if (verbose)
                        logs?.Add($"[AssetData:{fileBase}] Type mismatch → set Type=Audio.");
                    Debug.LogWarning($"[Data Health] AssetData type mismatch, set Type=Audio", a);
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
                {
                    logs?.Add($"[ItemData:{fileBase}] Icon is missing. (Warning)");
                    Debug.LogWarning($"[Data Health] ItemData icon is missing", it);
                }

                if (applyChanges && dirty)
                { EditorUtility.SetDirty(it); changes++; }
            }

            // Pass 3: CardData-specific checks
            // - Duplicate ImageAsset usage: detect when multiple CardData reference the same AssetData as ImageAsset
            var cards = identifiedAll.OfType<CardData>().Where(c => c != null).ToList();
            if (cards.Count > 0)
            {
                var byImageAsset = cards
                    .Where(c => c.ImageAsset != null)
                    .GroupBy(c => c.ImageAsset)
                    .Where(g => g.Count() > 1);

                foreach (var grp in byImageAsset)
                {
                    var asset = grp.Key;
                    var assetPath = AssetDatabase.GetAssetPath(asset);
                    var cardList = string.Join(", ", grp.Select(c => string.IsNullOrEmpty(c.Id) ? c.name : c.Id));
                    logs?.Add($"[CardData] Duplicate ImageAsset: '{asset?.Id ?? asset?.name}' ({assetPath}) used by {grp.Count()} cards → {cardList}");
                    // Emit per-card clickable warnings
                    foreach (var c in grp)
                    {
                        Debug.LogWarning($"[Data Health] Duplicate ImageAsset '{asset?.Id ?? asset?.name}' referenced here", c);
                    }
                }
            }

            if (applyChanges && changes > 0)
            {
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

            return changes;
        }

        public static int SyncCardQuestLinks(bool applyChanges, List<string> logs, bool verbose = true)
        {
            // Forward to central implementation in CardValidationUtility
            return Antura.Discover.Editor.CardValidationUtility.SyncCardQuestLinks(applyChanges, logs, verbose);
        }

        /// <summary>
        /// Print current Card↔Quest relationships, optionally listing orphans (cards without quests, quests without cards).
        /// </summary>
        public static void PrintCardQuestRelationships(List<string> logs, bool includeOrphans)
        {
            Antura.Discover.Editor.CardValidationUtility.PrintCardQuestRelationships(logs, includeOrphans);
        }

        /// <summary>
        /// Remove non-reciprocal Card↔Quest links and cleanup nulls/duplicates on both sides.
        /// Returns number of modified assets.
        /// </summary>
        public static int UnlinkNonReciprocalCardQuestLinks(bool applyChanges, List<string> logs, bool verbose = true)
        {
            return Antura.Discover.Editor.CardValidationUtility.UnlinkNonReciprocalCardQuestLinks(applyChanges, logs, verbose);
        }

        // ---------- Helpers ----------

        /// <summary>
        /// Scan prefabs under Assets/_discover/Prefabs for root-level WorldPrefabData, report empty or duplicate Ids.
        /// If applyChanges is true, auto-fill missing Ids using a stable rule (parentFolder + fileName) sanitized.
        /// Returns number of modified prefabs.
        /// </summary>
        public static int CheckWorldPrefabIds(bool applyChanges, List<string> logs, bool verbose = true)
        {
            int changes = 0;
            var root = "Assets/_discover/Prefabs";
            var guids = AssetDatabase.FindAssets("t:Prefab", new[] { root });
            var idToPaths = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);

            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var go = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (go == null)
                    continue;
                var comp = go.GetComponent<WorldPrefabData>(); // root only
                if (comp == null)
                    continue;

                string id = comp.Id != null ? comp.Id.Trim() : string.Empty;
                if (string.IsNullOrEmpty(id))
                {
                    string desired = BuildStablePrefabId(path);
                    logs?.Add($"[WorldPrefabData] Missing Id → '{desired}' at {path}");
                    if (applyChanges)
                    {
                        comp.Id = desired;
                        EditorUtility.SetDirty(comp);
                        changes++;
                    }
                }
                else
                {
                    if (!idToPaths.TryGetValue(id, out var list))
                    { list = new List<string>(); idToPaths[id] = list; }
                    list.Add(path);
                }
            }

            foreach (var kv in idToPaths)
            {
                if (kv.Value.Count > 1)
                {
                    logs?.Add($"[WorldPrefabData] Duplicate Id '{kv.Key}':\n - " + string.Join("\n - ", kv.Value));
                }
            }

            if (applyChanges && changes > 0)
            {
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            logs?.Add($"WorldPrefabData checked. Prefabs scanned: {guids.Length}. Duplicates: {idToPaths.Count(kv => kv.Value.Count > 1)}. {(applyChanges ? "Autofilled missing Ids." : "No changes applied.")}");
            return changes;
        }

        private static string BuildStablePrefabId(string assetPath)
        {
            try
            {
                var file = Path.GetFileNameWithoutExtension(assetPath) ?? string.Empty;
                var dir = Path.GetDirectoryName(assetPath) ?? string.Empty;
                var parent = Path.GetFileName(dir) ?? string.Empty;
                var raw = string.IsNullOrEmpty(parent) ? file : parent + "_" + file;
                return IdentifiedData.SanitizeId(raw);
            }
            catch
            {
                return IdentifiedData.SanitizeId(Path.GetFileNameWithoutExtension(assetPath) ?? "prefab");
            }
        }

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
