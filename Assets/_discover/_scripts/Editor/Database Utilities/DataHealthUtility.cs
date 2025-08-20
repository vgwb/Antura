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
                }
            }

            if (applyChanges && changes > 0)
            {
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

            return changes;
        }

        /// <summary>
        /// Ensure bidirectional consistency between CardData.Quests and QuestData.Cards.
        /// Adds missing back-links on both sides. Returns number of modified assets.
        /// </summary>
        public static int SyncCardQuestLinks(bool applyChanges, List<string> logs, bool verbose = true)
        {
            int changes = 0;
            var cards = FindAll<CardData>();
            var quests = FindAll<QuestData>();

            // Build fast lookup by Id/name
            var cardById = cards.Where(c => c != null)
                .ToDictionary(c => string.IsNullOrEmpty(c.Id) ? c.name : c.Id, c => c, StringComparer.OrdinalIgnoreCase);
            var questById = quests.Where(q => q != null)
                .ToDictionary(q => string.IsNullOrEmpty(q.Id) ? q.name : q.Id, q => q, StringComparer.OrdinalIgnoreCase);

            // Normalize null lists
            foreach (var c in cards.Where(x => x != null))
                if (c.Quests == null)
                    c.Quests = new List<QuestData>();
            foreach (var q in quests.Where(x => x != null))
                if (q.Cards == null)
                    q.Cards = new List<CardData>();

            // Pass 1: For each CardData.Quests ensure QuestData.Cards contains the card
            foreach (var c in cards.Where(x => x != null))
            {
                foreach (var q in c.Quests.Where(x => x != null).ToList())
                {
                    if (!q.Cards.Contains(c))
                    {
                        logs?.Add($"[Sync] Add back-link: Quest '{q.Id ?? q.name}' ← Card '{c.Id ?? c.name}'");
                        if (applyChanges)
                        {
                            q.Cards.Add(c);
                            EditorUtility.SetDirty(q);
                            changes++;
                        }
                    }
                }
            }

            // Pass 2: For each QuestData.Cards ensure CardData.Quests contains the quest
            foreach (var q in quests.Where(x => x != null))
            {
                foreach (var c in q.Cards.Where(x => x != null).ToList())
                {
                    if (!c.Quests.Contains(q))
                    {
                        logs?.Add($"[Sync] Add forward-link: Card '{c.Id ?? c.name}' ← Quest '{q.Id ?? q.name}'");
                        if (applyChanges)
                        {
                            c.Quests.Add(q);
                            EditorUtility.SetDirty(c);
                            changes++;
                        }
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

        /// <summary>
        /// Print current Card↔Quest relationships, optionally listing orphans (cards without quests, quests without cards).
        /// </summary>
        public static void PrintCardQuestRelationships(List<string> logs, bool includeOrphans)
        {
            var cards = FindAll<CardData>();
            var quests = FindAll<QuestData>();

            logs?.Add($"Cards: {cards.Count}, Quests: {quests.Count}");

            foreach (var c in cards.Where(x => x != null))
            {
                var qList = (c.Quests ?? new List<QuestData>())
                    .Where(q => q != null)
                    .Select(q => string.IsNullOrEmpty(q.Id) ? q.name : q.Id)
                    .ToList();
                string cId = string.IsNullOrEmpty(c.Id) ? c.name : c.Id;
                logs?.Add($"Card {cId} → Quests: {(qList.Count == 0 ? "-" : string.Join(", ", qList))}");
            }

            foreach (var q in quests.Where(x => x != null))
            {
                var cList = (q.Cards ?? new List<CardData>())
                    .Where(cd => cd != null)
                    .Select(cd => string.IsNullOrEmpty(cd.Id) ? cd.name : cd.Id)
                    .ToList();
                string qId = string.IsNullOrEmpty(q.Id) ? q.name : q.Id;
                logs?.Add($"Quest {qId} → Cards: {(cList.Count == 0 ? "-" : string.Join(", ", cList))}");
            }

            if (includeOrphans)
            {
                var orphanCards = cards.Where(c => c != null && (c.Quests == null || c.Quests.All(x => x == null))).ToList();
                var orphanQuests = quests.Where(q => q != null && (q.Cards == null || q.Cards.All(x => x == null))).ToList();
                if (orphanCards.Count > 0)
                    logs?.Add($"Orphan Cards (no quests): {string.Join(", ", orphanCards.Select(c => string.IsNullOrEmpty(c.Id) ? c.name : c.Id))}");
                if (orphanQuests.Count > 0)
                    logs?.Add($"Orphan Quests (no cards): {string.Join(", ", orphanQuests.Select(q => string.IsNullOrEmpty(q.Id) ? q.name : q.Id))}");
            }
        }

        /// <summary>
        /// Remove non-reciprocal Card↔Quest links and cleanup nulls/duplicates on both sides.
        /// Returns number of modified assets.
        /// </summary>
        public static int UnlinkNonReciprocalCardQuestLinks(bool applyChanges, List<string> logs, bool verbose = true)
        {
            int changes = 0;
            var cards = FindAll<CardData>();
            var quests = FindAll<QuestData>();

            foreach (var c in cards.Where(x => x != null))
            {
                bool dirty = false;
                if (c.Quests == null)
                    c.Quests = new List<QuestData>();
                // Remove nulls
                int before = c.Quests.Count;
                c.Quests = c.Quests.Where(q => q != null).Distinct().ToList();
                if (c.Quests.Count != before)
                { dirty = true; logs?.Add($"[Clean] Card '{c.Id ?? c.name}' removed null/dup quests"); }
                // Remove quests that don't reference this card
                var toRemove = new List<QuestData>();
                foreach (var q in c.Quests)
                {
                    if (q == null)
                        continue;
                    if (q.Cards == null || !q.Cards.Contains(c))
                    {
                        logs?.Add($"[Unlink] Card '{c.Id ?? c.name}' -x Quest '{q?.Id ?? q?.name}' (non-reciprocal)");
                        toRemove.Add(q);
                    }
                }
                if (toRemove.Count > 0)
                {
                    if (applyChanges)
                    {
                        foreach (var q in toRemove)
                            c.Quests.Remove(q);
                        EditorUtility.SetDirty(c);
                        changes++;
                    }
                }
                else if (applyChanges && dirty)
                {
                    EditorUtility.SetDirty(c);
                    changes++;
                }
            }

            foreach (var q in quests.Where(x => x != null))
            {
                bool dirty = false;
                if (q.Cards == null)
                    q.Cards = new List<CardData>();
                int before = q.Cards.Count;
                q.Cards = q.Cards.Where(cd => cd != null).Distinct().ToList();
                if (q.Cards.Count != before)
                { dirty = true; logs?.Add($"[Clean] Quest '{q.Id ?? q.name}' removed null/dup cards"); }
                var toRemove = new List<CardData>();
                foreach (var c in q.Cards)
                {
                    if (c == null)
                        continue;
                    if (c.Quests == null || !c.Quests.Contains(q))
                    {
                        logs?.Add($"[Unlink] Quest '{q.Id ?? q.name}' -x Card '{c?.Id ?? c?.name}' (non-reciprocal)");
                        toRemove.Add(c);
                    }
                }
                if (toRemove.Count > 0)
                {
                    if (applyChanges)
                    {
                        foreach (var c in toRemove)
                            q.Cards.Remove(c);
                        EditorUtility.SetDirty(q);
                        changes++;
                    }
                }
                else if (applyChanges && dirty)
                {
                    EditorUtility.SetDirty(q);
                    changes++;
                }
            }

            if (applyChanges && changes > 0)
            {
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            return changes;
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
