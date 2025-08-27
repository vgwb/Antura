using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
#if UNITY_EDITOR
using UnityEditor.Localization;
#endif

namespace Antura.Discover.Editor
{

    [System.Serializable]
    public class CardDataExport
    {
        public string version = "1.0";
        public string exportDate;
        public int totalCards;
        public List<CardDataJson> cards = new List<CardDataJson>();
        public ExportStats stats = new ExportStats();
    }

    [System.Serializable]
    public class CardDataJson
    {
        public string id;
        public string fileName;
        public string devStatus;

        public LocalizedStringJson title;
        public LocalizedStringJson description;
        public string category;
        public List<string> topics = new List<string>();
        public int year;
        public string country;

        public LocationJson location;

        public List<string> wordIds = new List<string>();  // Reference by ID

        public int masteryPointsToUnlock;

    }

    [System.Serializable]
    public class LocalizedStringJson
    {
        public string tableReference;
        public string entryReference;
        public string fallbackText;  // For when localization isn't available
    }

    [System.Serializable]
    public class LocationJson
    {
        public string locationId;
        public string locationName;
        public float latitude;
        public float longitude;
        // Add other LocationData properties as needed
    }

    [System.Serializable]
    public class AssetJson
    {
        public string assetId;
        public string assetPath;
        public string assetName;
        public string assetType;  // "Image", "Audio", etc.
    }

    [System.Serializable]
    public class ExportStats
    {
        public Dictionary<string, int> categoryCounts = new Dictionary<string, int>();
        public Dictionary<string, int> topicCounts = new Dictionary<string, int>();
        public Dictionary<string, int> countryCounts = new Dictionary<string, int>();
        public int collectibleCards;
        public int cardsWithAudio;
        public int cardsWithLocation;
        public int totalWords;
    }

    // === EXPORTER CLASS ===

    public static class CardDataJsonExporter
    {
        [MenuItem("Antura/Discover/Knowledge/Export CardData to JSON")]
        public static void ExportAllCardData()
        {
            ExportCardDataToJson("_production/Data/all_cards_export.json");
        }

        public static void ExportCardDataToJson(string filePath)
        {
            // Find all CardData assets
            string[] guids = AssetDatabase.FindAssets("t:CardData");
            Debug.Log($"Found {guids.Length} CardData assets");

            var export = new CardDataExport
            {
                exportDate = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                totalCards = guids.Length
            };

            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                CardData card = AssetDatabase.LoadAssetAtPath<CardData>(assetPath);

                if (card != null)
                {
                    var cardJson = ConvertCardToJson(card, assetPath);
                    export.cards.Add(cardJson);

                    // Update stats
                    UpdateExportStats(export.stats, card, cardJson);
                }
            }

            // Write to JSON file
            string json = JsonUtility.ToJson(export, true);

            // Create directory if it doesn't exist
            string directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(filePath, json);

            Debug.Log($"Exported {export.totalCards} cards to: {filePath}");
            Debug.Log($"File size: {new FileInfo(filePath).Length / 1024} KB");

            // Log some stats
            Debug.Log($"Categories: {string.Join(", ", export.stats.categoryCounts.Keys)}");
            Debug.Log($"Most common topics: {GetTopEntries(export.stats.topicCounts, 5)}");

            AssetDatabase.Refresh();
        }

        private static CardDataJson ConvertCardToJson(CardData card, string assetPath)
        {
            var cardJson = new CardDataJson
            {
                id = card.Id,
                fileName = Path.GetFileNameWithoutExtension(assetPath),
                devStatus = card.DevStatus.ToString(),

                // Knowledge Content
                title = ConvertLocalizedString(card.Title),
                description = ConvertLocalizedString(card.Description),
                category = card.Category.ToString(),
                year = card.Year,
                country = card.Country.ToString(),

                // Mastery & Rewards
                masteryPointsToUnlock = card.MasteryPointsToUnlock,
            };

            // Topics
            if (card.Topics != null)
            {
                foreach (var topic in card.Topics)
                {
                    cardJson.topics.Add(topic.ToString());
                }
            }

            // Location
            if (card.Location != null)
            {
                cardJson.location = ConvertLocationData(card.Location);
            }

            // Words
            if (card.Words != null)
            {
                foreach (var word in card.Words)
                {
                    if (word != null)
                        cardJson.wordIds.Add(word.Id);
                }
            }
            return cardJson;
        }

        private static LocalizedStringJson ConvertLocalizedString(LocalizedString localizedString)
        {
            if (localizedString == null || localizedString.IsEmpty)
                return null;

            // Resolve text using the project (default) locale to ensure a deterministic value in exports.
            string resolved = string.Empty;
            try
            {
                var projectLocale = LocalizationSettings.ProjectLocale ?? LocalizationSettings.SelectedLocale;
                if (projectLocale != null)
                {
                    resolved = LocalizationSettings.StringDatabase.GetLocalizedString(
                        localizedString.TableReference,
                        localizedString.TableEntryReference,
                        projectLocale
                    );
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogWarning($"[CardDataJsonExporter] Failed to resolve localized text for {localizedString.TableReference}:{localizedString.TableEntryReference} -> {ex.Message}");
            }

            // Fallback to direct GetLocalizedString (may be empty in editor if not initialized)
            if (string.IsNullOrEmpty(resolved))
            {
                try
                { resolved = localizedString.GetLocalizedString(); }
                catch { /* ignore */ }
            }

            return new LocalizedStringJson
            {
                tableReference = localizedString.TableReference.ToString() ?? "",
                entryReference = localizedString.TableEntryReference.ToString() ?? "",
                fallbackText = resolved
            };
        }

        // === IMPORTER ===

        [MenuItem("Antura/Discover/Knowledge/Import CardData from JSON...")]
        public static void ImportCardDataFromJsonMenu()
        {
            string path = EditorUtility.OpenFilePanel("Import CardData JSON", Application.dataPath, "json");
            if (!string.IsNullOrEmpty(path))
            {
                ImportCardDataFromJson(path, updateLocalization: true);
            }
        }

        public static void ImportCardDataFromJson(string filePath, bool updateLocalization)
        {
            if (!File.Exists(filePath))
            {
                Debug.LogError($"JSON file not found: {filePath}");
                return;
            }

            string json = File.ReadAllText(filePath);
            CardDataExport export = null;
            try
            {
                export = JsonUtility.FromJson<CardDataExport>(json);
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to parse JSON: {e.Message}");
                return;
            }

            if (export == null || export.cards == null)
            {
                Debug.LogWarning("No cards found in JSON.");
                return;
            }

            // Build index of existing CardData by Id
            var existing = new Dictionary<string, (CardData card, string path)>();
            string[] guids = AssetDatabase.FindAssets("t:CardData");
            foreach (var guid in guids)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                var cd = AssetDatabase.LoadAssetAtPath<CardData>(assetPath);
                if (cd != null && !string.IsNullOrEmpty(cd.Id))
                {
                    existing[cd.Id] = (cd, assetPath);
                }
            }

            string createDir = "Assets/_discover/_data/_dev";
            if (!AssetDatabase.IsValidFolder(createDir))
            {
                var parent = "Assets/_discover/_data";
                if (!AssetDatabase.IsValidFolder(parent))
                {
                    AssetDatabase.CreateFolder("Assets/_discover", "_data");
                }
                AssetDatabase.CreateFolder(parent, "_dev");
            }

            int created = 0, updated = 0;
            foreach (var cj in export.cards)
            {
                if (cj == null)
                    continue;

                CardData card = null;
                string assetPath = null;
                string id = string.IsNullOrEmpty(cj.id) ? cj.fileName : cj.id;
                if (string.IsNullOrEmpty(id))
                    id = Guid.NewGuid().ToString("N");

                if (existing.TryGetValue(id, out var tuple))
                {
                    card = tuple.card;
                    assetPath = tuple.path;
                }
                else
                {
                    // Create new asset
                    card = ScriptableObject.CreateInstance<CardData>();
                    card.name = string.IsNullOrEmpty(cj.fileName) ? id : cj.fileName;
                    // Ensure Id set via editor API
                    card.Editor_SetId(id);

                    assetPath = $"{createDir}/{card.name}.asset";
                    AssetDatabase.CreateAsset(card, assetPath);
                    created++;
                    existing[id] = (card, assetPath);
                }

                // Update core fields (category, topics, year, country)
                if (!string.IsNullOrEmpty(cj.category) && Enum.TryParse<CardCategory>(cj.category, out var cat))
                    card.Category = cat;
                card.Year = cj.year;
                if (!string.IsNullOrEmpty(cj.country) && Enum.TryParse<Countries>(cj.country, out var cnt))
                    card.Country = cnt;

                // Topics
                if (card.Topics == null)
                    card.Topics = new List<KnowledgeTopic>();
                else
                    card.Topics.Clear();
                if (cj.topics != null)
                {
                    foreach (var t in cj.topics)
                    {
                        if (!string.IsNullOrEmpty(t) && Enum.TryParse<KnowledgeTopic>(t, out var tp))
                            card.Topics.Add(tp);
                    }
                }

                // Localized Title / Description references and optional table updates
                if (cj.title != null)
                {
                    SetLocalizedStringRef(card, id, cj.title, isTitle: true);
#if UNITY_EDITOR
                    if (updateLocalization && !string.IsNullOrEmpty(cj.title.fallbackText))
                        TryUpdateLocalizationTable(cj.title, cj.title.fallbackText);
#endif
                }
                if (cj.description != null)
                {
                    SetLocalizedStringRef(card, id, cj.description, isTitle: false);
#if UNITY_EDITOR
                    if (updateLocalization && !string.IsNullOrEmpty(cj.description.fallbackText))
                        TryUpdateLocalizationTable(cj.description, cj.description.fallbackText);
#endif
                }

                EditorUtility.SetDirty(card);
                updated++;
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"Import complete. Updated {updated} card(s), created {created} new card(s).");
        }

        private static void SetLocalizedStringRef(CardData card, string cardId, LocalizedStringJson json, bool isTitle)
        {
            string table = string.IsNullOrEmpty(json.tableReference) ? "Cards" : json.tableReference;
            string entry = string.IsNullOrEmpty(json.entryReference) ? ($"{cardId}_{(isTitle ? "Title" : "Description")}") : json.entryReference;

            if (isTitle)
            {
                var ls = card.Title;
                ls.TableReference = table;
                ls.TableEntryReference = entry;
                card.Title = ls;
            }
            else
            {
                var ls = card.Description;
                ls.TableReference = table;
                ls.TableEntryReference = entry;
                card.Description = ls;
            }
        }

#if UNITY_EDITOR
        private static bool TryUpdateLocalizationTable(LocalizedStringJson json, string text)
        {
            if (string.IsNullOrEmpty(json.tableReference) || string.IsNullOrEmpty(json.entryReference))
                return false;

            var collection = LocalizationEditorSettings.GetStringTableCollection(json.tableReference);
            if (collection == null)
            {
                Debug.LogWarning($"StringTableCollection '{json.tableReference}' not found. Skipping localization update for '{json.entryReference}'.");
                return false;
            }

            var locale = LocalizationSettings.ProjectLocale ?? LocalizationSettings.SelectedLocale;
            if (locale == null)
            {
                Debug.LogWarning("No ProjectLocale/SelectedLocale found. Skipping localization update.");
                return false;
            }

            var table = collection.GetTable(locale.Identifier) as StringTable;
            if (table == null)
            {
                Debug.LogWarning($"Table for locale '{locale.Identifier}' not found in collection '{json.tableReference}'.\nCreate the table or use CSV import if you need to populate locales in bulk.");
                return false;
            }

            var entry = table.GetEntry(json.entryReference);
            if (entry == null)
            {
                entry = table.AddEntry(json.entryReference, text);
            }
            else
            {
                entry.Value = text;
            }

            EditorUtility.SetDirty(table);
            return true;
        }
#endif

        private static AssetJson ConvertAssetData(AssetData assetData)
        {
            if (assetData == null)
                return null;

            return new AssetJson
            {
                assetId = assetData.Id,
                assetName = assetData.name,
                assetPath = AssetDatabase.GetAssetPath(assetData),
                assetType = assetData.GetType().Name
            };
        }

        private static LocationJson ConvertLocationData(LocationData location)
        {
            if (location == null)
                return null;

            return new LocationJson
            {
                locationId = location.Id,
                locationName = location.name,
                // Add latitude/longitude if your LocationData has them
                // latitude = location.Latitude,
                // longitude = location.Longitude
            };
        }

        private static void UpdateExportStats(ExportStats stats, CardData card, CardDataJson cardJson)
        {
            // Category counts
            string category = card.Category.ToString();
            if (stats.categoryCounts.ContainsKey(category))
                stats.categoryCounts[category]++;
            else
                stats.categoryCounts[category] = 1;

            // Topic counts
            foreach (var topic in cardJson.topics)
            {
                if (stats.topicCounts.ContainsKey(topic))
                    stats.topicCounts[topic]++;
                else
                    stats.topicCounts[topic] = 1;
            }

            // Country counts
            string country = card.Country.ToString();
            if (stats.countryCounts.ContainsKey(country))
                stats.countryCounts[country]++;
            else
                stats.countryCounts[country] = 1;

            // Other stats
            if (card.IsCollectible)
                stats.collectibleCards++;

            if (card.AudioAsset != null)
                stats.cardsWithAudio++;

            if (card.Location != null)
                stats.cardsWithLocation++;

            stats.totalWords += cardJson.wordIds.Count;
        }

        private static string GetTopEntries(Dictionary<string, int> dict, int count)
        {
            var sorted = new List<KeyValuePair<string, int>>(dict);
            sorted.Sort((x, y) => y.Value.CompareTo(x.Value));

            var top = new List<string>();
            for (int i = 0; i < Math.Min(count, sorted.Count); i++)
            {
                top.Add($"{sorted[i].Key}({sorted[i].Value})");
            }

            return string.Join(", ", top);
        }
    }

}
