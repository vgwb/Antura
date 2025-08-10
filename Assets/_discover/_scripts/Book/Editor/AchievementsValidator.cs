#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Antura.Discover.EditorTools
{
    public static class AchievementsValidator
    {
        [MenuItem("Antura/Discover/Validate Card Database")]
        public static void ValidateDatabase()
        {
            var db = Selection.activeObject as CardDatabaseData;
            if (db == null)
            {
                // Try find one in project
                var guids = AssetDatabase.FindAssets("t:CardDatabase");
                if (guids.Length == 0)
                { EditorUtility.DisplayDialog("Validator", "No CardDatabase found.", "OK"); return; }
                db = AssetDatabase.LoadAssetAtPath<CardDatabaseData>(AssetDatabase.GUIDToAssetPath(guids[0]));
            }

            if (db == null)
            { EditorUtility.DisplayDialog("Validator", "Select or create a CardDatabase.", "OK"); return; }

            var problems = new List<string>();
            var seenIds = new HashSet<string>();

            if (db.Collections == null || db.Collections.Length == 0)
                problems.Add("Database has no collections.");

            foreach (var col in db.Collections)
            {
                if (col == null)
                { problems.Add("Null collection reference in database."); continue; }
                if (col.Cards == null || col.Cards.Length == 0)
                    problems.Add($"Collection '{col.name}' has no cards.");

                foreach (var card in col.Cards)
                {
                    if (card == null)
                    { problems.Add($"Null card in collection '{col.name}'."); continue; }

                    if (string.IsNullOrWhiteSpace(card.Id))
                        problems.Add($"Card '{card.name}' has empty Id.");

                    if (!seenIds.Add(card.Id))
                        problems.Add($"Duplicate card Id: {card.Id} (found in collection '{col.name}')");

                    if (card.Country != col.Country)
                        problems.Add($"Card '{card.Id}' country ({card.Country}) != collection country ({col.Country}).");

                    if (card.UnlockQuests != null)
                    {
                        foreach (var q in card.UnlockQuests)
                        {
                            if (q == null)
                                problems.Add($"Card '{card.Id}' has a null Quest reference.");
                            else if (q.Country != card.Country)
                                problems.Add($"Card '{card.Id}' quest '{q.name}' country mismatch: {q.Country} vs card {card.Country}.");
                        }
                    }
                }
            }

            if (problems.Count == 0)
            {
                EditorUtility.DisplayDialog("Validator", "No issues found. All good! ✅", "OK");
            }
            else
            {
                var msg = string.Join("\n• ", problems.Prepend("Issues found:"));
                EditorUtility.DisplayDialog("Validator", msg, "OK");
                Debug.LogWarning(msg);
            }
        }
    }
}
#endif
