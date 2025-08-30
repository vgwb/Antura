#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Antura.Discover
{
    /// <summary>
    /// Centralized CSV export/import for CardData.
    /// </summary>
    public static class CardDataExchangeUtility
    {
        private static readonly string[] Header = new[]
        {
            "id","status","importance","type","topics","TitleEn","DescriptionEn","year","Country","wikipediaUrl","Notes","Rationale","LinkedCards"
        };

        public static string BuildCardsCsv(IEnumerable<CardData> cards, bool includeDevRow = true)
        {
            var sb = new StringBuilder();
            sb.AppendLine(string.Join(",", Header));
            if (includeDevRow)
            {
                // Dev row: enumerate enum values for guidance; importer will skip id "-"
                var devRow = new[]
                {
                    "-", // id
                    Escape(string.Join(", ", Enum.GetNames(typeof(Status)))),
                    Escape(string.Join(", ", Enum.GetNames(typeof(CardImportance)))),
                    // Exclude CardType values <= 0 (e.g., None)
                    Escape(string.Join(", ", Enum.GetValues(typeof(CardType)).Cast<Enum>().Where(e => Convert.ToInt32(e) > 0).Select(e => e.ToString()))),
                    // Exclude KnowledgeTopic values <= 0 (e.g., None)
                    Escape(string.Join(", ", Enum.GetValues(typeof(KnowledgeTopic)).Cast<Enum>().Where(e => Convert.ToInt32(e) > 0).Select(e => e.ToString()))),
                    "", // TitleEn
                    "", // DescriptionEn
                    "", // year
                    "", // Country
                    "", // wikipediaUrl
                    "", // Notes
                    "", // Rationale
                    ""  // LinkedCards
                };
                sb.AppendLine(string.Join(",", devRow));
            }
            foreach (var c in cards ?? Enumerable.Empty<CardData>())
            {
                var topics = c.Topics != null ? string.Join(",", c.Topics.Select(t => t.ToString())) : string.Empty;
                var row = new[]
                {
                    Escape(c.Id),
                    Escape(c.Status.ToString()),
                    Escape(c.Importance.ToString()),
                    Escape(c.Type.ToString()),
                    Escape(topics),
                    Escape(c.TitleEn),
                    Escape(c.DescriptionEn),
                    Escape(c.Year.ToString()),
                    Escape(c.Country.ToString()),
                    Escape(c.WikipediaUrl),
                    Escape(c.Notes),
                    Escape(c.Rationale),
                    Escape(c.LinkedCards)
                };
                sb.AppendLine(string.Join(",", row));
            }
            return sb.ToString();
        }

        public static void ExportCardsCsvToPath(IEnumerable<CardData> cards, string path, bool includeDevRow = true)
        {
            if (string.IsNullOrEmpty(path))
                return;
            var csv = BuildCardsCsv(cards, includeDevRow);
            try
            { File.WriteAllText(path, csv, new UTF8Encoding(false)); }
            catch (Exception ex) { Debug.LogError($"[CardDataExchange] Failed to write CSV: {ex.Message}"); }
        }

        /// <summary>
        /// Import CardData from a CSV file built by BuildCardsCsv. Returns number of rows applied.
        /// Updates existing assets matched by Id. Does not create missing by default.
        /// </summary>
        public static int ImportCardsCsvFromPath(string path, bool createIfMissing = false, string createFolder = null, bool dryRun = false)
        {
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
            { Debug.LogWarning("[CardDataExchange] CSV file not found."); return 0; }

            Debug.Log($"[CardDataExchange] Import start: {path} | createIfMissing={createIfMissing} | dryRun={dryRun} | targetFolder={(string.IsNullOrEmpty(createFolder) ? "<default>" : createFolder)}");

            // Index existing CardData by Id
            var map = new Dictionary<string, CardData>(StringComparer.OrdinalIgnoreCase);
            foreach (var guid in AssetDatabase.FindAssets("t:CardData"))
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                var cd = AssetDatabase.LoadAssetAtPath<CardData>(assetPath);
                if (cd != null && !string.IsNullOrEmpty(cd.Id))
                {
                    if (!map.ContainsKey(cd.Id))
                        map.Add(cd.Id, cd);
                }
            }

            int applied = 0;
            var createdIds = new List<string>();
            var modifiedIds = new List<string>();
            string[] lines;
            try
            { lines = File.ReadAllLines(path); }
            catch (Exception ex) { Debug.LogError($"[CardDataExchange] Failed to read CSV: {ex.Message}"); return 0; }
            if (lines.Length == 0)
                return 0;

            // Read header
            var header = ParseCsvLine(lines[0]);
            var col = header
                .Select((h, i) => new { h = (h ?? string.Empty).Trim(), i })
                .ToDictionary(x => x.h.ToLowerInvariant(), x => x.i);

            for (int li = 1; li < lines.Length; li++)
            {
                var line = lines[li];
                if (string.IsNullOrWhiteSpace(line))
                    continue;
                var cells = ParseCsvLine(line);
                string Get(string name)
                {
                    if (name == null)
                        return string.Empty;
                    var key = name.ToLowerInvariant();
                    if (col.TryGetValue(key, out int idx) && idx >= 0 && idx < cells.Count)
                        return cells[idx];
                    return string.Empty;
                }

                var id = Get("id").Trim();
                if (string.IsNullOrEmpty(id))
                    continue;
                if (id == "-")
                    continue; // skip dev row
                if (!map.TryGetValue(id, out var card))
                {
                    if (!createIfMissing)
                    {
                        Debug.Log($"[CardDataExchange] Skip unknown id '{id}' (creation disabled)");
                        continue;
                    }
                    // Create new asset when requested
                    if (dryRun)
                    {
                        Debug.Log($"[CardDataExchange] Would create CardData '{id}' (dry run)");
                        continue; // don't create, and skip applying fields in dry run
                    }
                    else
                    {
                        var newCard = ScriptableObject.CreateInstance<CardData>();
                        newCard.Editor_SetId(IdentifiedData.SanitizeId(id));
                        var folder = string.IsNullOrEmpty(createFolder) ? "Assets/_discover/_Data/Cards" : createFolder;
                        if (!AssetDatabase.IsValidFolder(folder))
                        {
                            var parts = folder.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                            string acc = parts[0];
                            for (int pi = 1; pi < parts.Length; pi++)
                            {
                                var next = acc + "/" + parts[pi];
                                if (!AssetDatabase.IsValidFolder(next))
                                    AssetDatabase.CreateFolder(acc, parts[pi]);
                                acc = next;
                            }
                        }
                        var assetPath = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(folder, newCard.Id + ".asset"));
                        AssetDatabase.CreateAsset(newCard, assetPath);
                        AssetDatabase.SaveAssets();
                        card = newCard;
                        map[id] = card;
                        Debug.Log($"[CardDataExchange] Created CardData '{id}' at {assetPath}", card);
                        createdIds.Add(id);
                    }
                }

                if (!dryRun)
                    Undo.RecordObject(card, "Import CardData CSV");
                var changedFields = new List<string>();
                // Status
                TryParseEnum(Get("status"), out Status status);
                if (card.Status != status)
                { if (!dryRun) card.Status = status; changedFields.Add(nameof(card.Status)); }
                // Importance
                TryParseEnum(Get("importance"), out CardImportance importance);
                if (card.Importance != importance)
                { if (!dryRun) card.Importance = importance; changedFields.Add(nameof(card.Importance)); }
                // Type
                TryParseEnum(Get("type"), out CardType type);
                if (card.Type != type)
                { if (!dryRun) card.Type = type; changedFields.Add(nameof(card.Type)); }
                // Topics
                var newTopics = ParseTopics(Get("topics"));
                bool topicsChanged = (card.Topics == null && newTopics.Count > 0)
                                     || (card.Topics != null && (card.Topics.Count != newTopics.Count || !card.Topics.SequenceEqual(newTopics)));
                if (topicsChanged)
                { if (!dryRun) card.Topics = newTopics; changedFields.Add(nameof(card.Topics)); }
                // Title/Description
                var titleEn = Get("TitleEn");
                if (!string.Equals(card.TitleEn, titleEn, StringComparison.Ordinal))
                { if (!dryRun) card.TitleEn = titleEn; changedFields.Add(nameof(card.TitleEn)); }
                var descEn = Get("DescriptionEn");
                if (!string.Equals(card.DescriptionEn, descEn, StringComparison.Ordinal))
                { if (!dryRun) card.DescriptionEn = descEn; changedFields.Add(nameof(card.DescriptionEn)); }
                // Year
                if (int.TryParse(Get("year"), out int year))
                { if (card.Year != year) { if (!dryRun) card.Year = year; changedFields.Add(nameof(card.Year)); } }
                else
                { if (card.Year != 0) { if (!dryRun) card.Year = 0; changedFields.Add(nameof(card.Year)); } }
                // Country
                TryParseEnum(Get("Country"), out Countries country);
                if (!Equals(card.Country, country))
                { if (!dryRun) card.Country = country; changedFields.Add(nameof(card.Country)); }
                // Wikipedia/Notes/Rationale/LinkedCards
                var wiki = Get("wikipediaUrl");
                if (!string.Equals(card.WikipediaUrl, wiki, StringComparison.Ordinal))
                { if (!dryRun) card.WikipediaUrl = wiki; changedFields.Add(nameof(card.WikipediaUrl)); }
                var notes = Get("Notes");
                if (!string.Equals(card.Notes, notes, StringComparison.Ordinal))
                { if (!dryRun) card.Notes = notes; changedFields.Add(nameof(card.Notes)); }
                var rationale = Get("Rationale");
                if (!string.Equals(card.Rationale, rationale, StringComparison.Ordinal))
                { if (!dryRun) card.Rationale = rationale; changedFields.Add(nameof(card.Rationale)); }
                var linked = Get("LinkedCards");
                if (!string.Equals(card.LinkedCards, linked, StringComparison.Ordinal))
                { if (!dryRun) card.LinkedCards = linked; changedFields.Add(nameof(card.LinkedCards)); }
                // Optional metadata
                var lastReviewed = Get("LastReviewed");
                if (!string.IsNullOrWhiteSpace(lastReviewed))
                { if (!dryRun) card.LastReviewed = lastReviewed; }
                var needsLocUpdate = Get("NeedsLocalizationUpdate");
                if (!string.IsNullOrWhiteSpace(needsLocUpdate))
                { if (!dryRun) card.NeedsLocalizationUpdate = needsLocUpdate.Equals("true", StringComparison.OrdinalIgnoreCase); }
                if (!dryRun)
                    EditorUtility.SetDirty(card);
                if (changedFields.Count > 0)
                {
                    Debug.Log($"[CardDataExchange] {(dryRun ? "Would update" : "Updated")} '{id}': {string.Join(", ", changedFields)}", card);
                    modifiedIds.Add(id);
                }
                applied++;
            }

            if (!dryRun)
                AssetDatabase.SaveAssets();
            // if (createdIds.Count > 0)
            //     Debug.Log($"[CardDataExchange] Created {createdIds.Count} cards: {string.Join(", ", createdIds)}");
            // if (modifiedIds.Count > 0)
            //     Debug.Log($"[CardDataExchange] Modified {modifiedIds.Count} cards: {string.Join(", ", modifiedIds)}");
            return applied;
        }

        private static List<KnowledgeTopic> ParseTopics(string s)
        {
            var list = new List<KnowledgeTopic>();
            if (string.IsNullOrWhiteSpace(s))
                return list;
            var parts = s.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var p in parts)
            {
                var token = p.Trim();
                if (TryParseEnum(token, out KnowledgeTopic k))
                    list.Add(k);
            }
            return list;
        }

        private static bool TryParseEnum<T>(string s, out T value) where T : struct
        {
            value = default;
            if (string.IsNullOrWhiteSpace(s))
                return false;
            // Try exact, then ignore case, then remove spaces
            if (Enum.TryParse<T>(s, out value))
                return true;
            if (Enum.TryParse<T>(s, true, out value))
                return true;
            var compact = new string(s.Where(ch => !char.IsWhiteSpace(ch)).ToArray());
            if (Enum.TryParse<T>(compact, true, out value))
                return true;
            return false;
        }

        private static List<string> ParseCsvLine(string line)
        {
            var result = new List<string>();
            if (line == null)
                return result;
            var sb = new StringBuilder();
            bool inQuotes = false;
            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];
                if (inQuotes)
                {
                    if (c == '"')
                    {
                        // Escaped quote
                        if (i + 1 < line.Length && line[i + 1] == '"')
                        { sb.Append('"'); i++; }
                        else
                        { inQuotes = false; }
                    }
                    else
                    { sb.Append(c); }
                }
                else
                {
                    if (c == ',')
                    { result.Add(sb.ToString()); sb.Length = 0; }
                    else if (c == '"')
                    { inQuotes = true; }
                    else
                    { sb.Append(c); }
                }
            }
            result.Add(sb.ToString());
            return result;
        }

        private static string Escape(string s)
        {
            if (string.IsNullOrEmpty(s))
                return "";
            s = s.Replace("\r", " ").Replace("\n", " ");
            if (s.Contains(",") || s.Contains("\""))
            {
                s = s.Replace("\"", "\"\"");
                return "\"" + s + "\"";
            }
            return s;
        }

        // Menu items removed; use CardManagementWindow instead.
    }
}
#endif
