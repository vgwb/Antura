using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization;

namespace Antura.Discover.Editor
{
    public struct CardValidationReport
    {
        public int TotalCards;
        public int DuplicateIdsGroups;
        public int MissingId;
        public int MissingLocalizedTitle;
        public int MissingTopics;
        public int MissingImage;
        public int EmptyTitleEn;
        public int EmptyDescriptionEn;
        public int BrokenLinkedCards;

        public override string ToString()
        {
            return $"Total: {TotalCards}\n" +
                   $"Duplicate Id groups: {DuplicateIdsGroups}\n" +
                   $"Missing Id: {MissingId}\n" +
                   $"Missing Localized Title: {MissingLocalizedTitle}\n" +
                   $"Missing Topics: {MissingTopics}\n" +
                   $"Missing Image: {MissingImage}\n" +
                   $"TitleEn empty: {EmptyTitleEn}\n" +
                   $"DescriptionEn empty: {EmptyDescriptionEn}\n" +
                   $"Broken LinkedCards: {BrokenLinkedCards}";
        }
    }

    public static class CardValidationUtility
    {
        // Local finder mirroring DataHealthUtility's behavior
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

        public static CardValidationReport ValidateAllCards(bool logEachIssue = true)
        {
            var guids = AssetDatabase.FindAssets("t:CardData");
            var cards = new List<CardData>(guids.Length);
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var c = AssetDatabase.LoadAssetAtPath<CardData>(path);
                if (c != null)
                    cards.Add(c);
            }

            // Detect duplicate CardId across assets (case-insensitive)
            var dupGroups = cards
                .Where(c => c != null && !string.IsNullOrWhiteSpace(c.Id))
                .GroupBy(c => c.Id, StringComparer.OrdinalIgnoreCase)
                .Where(g => g.Count() > 1)
                .ToList();
            if (dupGroups.Count > 0)
            {
                foreach (var g in dupGroups)
                {
                    Debug.LogError($"[Card Validate] Duplicate CardId '{g.Key}' used by {g.Count()} assets:");
                    foreach (var c in g)
                    {
                        Debug.LogError($" - {c.name} ({AssetDatabase.GetAssetPath(c)})", c);
                    }
                }
            }

            var idSet = new HashSet<string>(cards.Where(c => !string.IsNullOrEmpty(c?.Id)).Select(c => c.Id), StringComparer.OrdinalIgnoreCase);

            var report = new CardValidationReport { TotalCards = cards.Count, DuplicateIdsGroups = dupGroups.Count };
            foreach (var c in cards)
            {
                if (c == null)
                    continue;
                ValidateCard(c, idSet, logEachIssue, ref report);
            }

            return report;
        }

        private static void ValidateCard(CardData card, HashSet<string> allIds, bool log, ref CardValidationReport rpt)
        {
            if (string.IsNullOrWhiteSpace(card.Id))
            {
                rpt.MissingId++;
                if (log)
                    Debug.LogWarning($"[Card Validate] Missing ID: {AssetDatabase.GetAssetPath(card)}", card);
            }

            // Localized Title
            try
            {
                var titleLoc = card.Title;
                if (titleLoc == null || titleLoc.IsEmpty)
                {
                    rpt.MissingLocalizedTitle++;
                    if (log)
                        Debug.LogWarning($"[Card Validate] Missing localized Title: {card.name}", card);
                }
            }
            catch { }

            // Topics
            if (card.Topics == null || card.Topics.Count == 0)
            {
                rpt.MissingTopics++;
                if (log)
                    Debug.LogWarning($"[Card Validate] Missing topics: {card.name}", card);
            }

            // Image
            try
            {
                if (card.ImageAsset == null)
                {
                    rpt.MissingImage++;
                    if (log)
                        Debug.LogWarning($"[Card Validate] Missing image: {card.name}", card);
                }
            }
            catch { }

            try
            {
                if (string.IsNullOrWhiteSpace(card.TitleEn))
                {
                    rpt.EmptyTitleEn++;
                    if (log)
                        Debug.LogWarning($"[Card Validate] '{card.Id}' has empty TitleEn", card);
                }
            }
            catch { }

            try
            {
                if (string.IsNullOrWhiteSpace(card.DescriptionEn))
                {
                    rpt.EmptyDescriptionEn++;
                    if (log)
                        Debug.LogWarning($"[Card Validate] '{card.Id}' has empty DescriptionEn", card);
                }
            }
            catch { }

        }

        // Ensure bidirectional consistency between CardData.Quests and QuestData.Cards.
        // Adds missing back-links on both sides. Returns number of modified assets.
        public static int SyncCardQuestLinks(bool applyChanges, List<string> logs, bool verbose = true)
        {
            int changes = 0;
            var cards = FindAll<CardData>();
            var quests = FindAll<QuestData>();

            // Normalize null lists
            foreach (var c in cards)
                if (c != null && c.Quests == null)
                    c.Quests = new List<QuestData>();
            foreach (var q in quests)
                if (q != null && q.Cards == null)
                    q.Cards = new List<CardData>();

            // Deduplicate: ensure a Quest does not link the same Card multiple times
            foreach (var q in quests.Where(x => x != null))
            {
                if (q.Cards == null)
                    q.Cards = new List<CardData>();
                int before = q.Cards.Count;
                // Remove nulls in this pass as well, keeps logs simpler
                var dedup = q.Cards.Where(cd => cd != null).Distinct().ToList();
                if (dedup.Count != before)
                {
                    int removed = before - dedup.Count;
                    logs?.Add($"[Sync] Dedup Quest '{(string.IsNullOrEmpty(q.Id) ? q.name : q.Id)}': removed {removed} duplicate card reference(s)");
                    if (applyChanges)
                    {
                        q.Cards = dedup;
                        EditorUtility.SetDirty(q);
                        changes++;
                    }
                }
            }

            // Pass 1: Card -> ensure Quest has Card
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

            // Pass 2: Quest -> ensure Card has Quest
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

        // Remove non-reciprocal links and cleanup nulls/duplicates on both sides.
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
                int before = c.Quests.Count;
                c.Quests = c.Quests.Where(q => q != null).Distinct().ToList();
                if (c.Quests.Count != before)
                { dirty = true; logs?.Add($"[Clean] Card '{c.Id ?? c.name}' removed null/dup quests"); }
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
    }
}
