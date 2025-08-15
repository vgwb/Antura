using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Antura.Discover
{
    /// <summary>
    /// - Builds lookup dictionaries from CardDatabase
    /// - Loads/saves player state (JSON)
    /// - Unlocks cards from specific quests and tracks per-quest history
    /// - Exposes queries for Diary/menus (by country, by category, by quest)
    /// </summary>
    public class AchievementsManager : MonoBehaviour
    {
        [Header("Content")]
        public CardDatabaseData Database;
        [Tooltip("Optional: all QuestData assets to help resolve and validate slugs.")]
        public QuestListData AllQuests;

        [Header("Save")]
        public string SaveFileName = "player_cards.json";

        private Dictionary<string, CardState> stateById = new();

        public event Action<CardData, CardState> OnCardUnlocked;
        public event Action<CardData, CardState> OnProgressChanged;

        void Awake()
        {
            BuildIndexes();
            LoadState();
        }

        // ---------------- Indexing ----------------

        void BuildIndexes()
        {
            if (Database == null || Database.Collections == null)
            {
                Debug.LogError("AchievementsManager: No CardDatabase assigned.");
                return;
            }

            Database.ById = new();
            Database.ByCountry = new();

            foreach (var col in Database.Collections)
            {
                if (col == null)
                    continue;
                if (!Database.ByCountry.ContainsKey(col.Country))
                    Database.ByCountry[col.Country] = new List<CardData>();

                if (col.Cards == null)
                    continue;
                foreach (var c in col.Cards)
                {
                    if (c == null)
                        continue;
                    if (string.IsNullOrEmpty(c.Id))
                    {
                        Debug.LogWarning($"CardDefinition with empty Id in collection {col.name}");
                        continue;
                    }
                    if (Database.ById.ContainsKey(c.Id))
                    {
                        Debug.LogWarning($"Duplicate card Id detected: {c.Id} (collection {col.name})");
                        continue;
                    }
                    Database.ById[c.Id] = c;
                    Database.ByCountry[col.Country].Add(c);
                }
            }
        }

        // ---------------- Persistence ----------------

        void LoadState()
        {
            stateById.Clear();
            string path = Path.Combine(Application.persistentDataPath, SaveFileName);
            if (File.Exists(path))
            {
                var json = File.ReadAllText(path);
                var save = JsonUtility.FromJson<PlayerCollectionSave>(json);
                if (save?.Cards != null)
                    foreach (var s in save.Cards)
                        stateById[s.Id] = s;
            }

            // Ensure every known card has a state entry
            if (Database?.ById != null)
            {
                foreach (var kv in Database.ById)
                    if (!stateById.ContainsKey(kv.Key))
                        stateById[kv.Key] = new CardState { Id = kv.Key };
            }
        }

        public void SaveState()
        {
            var save = new PlayerCollectionSave { Cards = new List<CardState>(stateById.Values) };
            var json = JsonUtility.ToJson(save, prettyPrint: true);
            var path = Path.Combine(Application.persistentDataPath, SaveFileName);
            File.WriteAllText(path, json);
        }

        // ---------------- Accessors ----------------

        public CardData GetCard(string id) =>
            Database != null && Database.ById != null && Database.ById.TryGetValue(id, out var c) ? c : null;

        public CardState GetState(string id) =>
            stateById.TryGetValue(id, out var s) ? s : null;

        public IEnumerable<CardData> GetCardsByCountry(Antura.Discover.Countries c) =>
            Database != null && Database.ByCountry != null && Database.ByCountry.TryGetValue(c, out var list) ? list : Array.Empty<CardData>();

        public IEnumerable<CardData> GetCardsByCategory(Antura.Discover.CardCategory cat)
        {
            var result = new List<CardData>();
            if (Database?.ById == null)
                return result;
            foreach (var kv in Database.ById)
                if (kv.Value.Category == cat)
                    result.Add(kv.Value);
            return result;
        }

        // ---------------- Unlocking & progress ----------------

        public void UnlockFromQuest(CardData card, Antura.Discover.QuestData quest, DateTime whenUTC, bool countRepeat = true)
        {
            if (card == null || quest == null)
                return;
            var st = GetState(card.Id);
            if (st == null)
                return;

            string questSlug = QuestSlug(quest);
            long now = new DateTimeOffset(whenUTC).ToUnixTimeSeconds();

            bool firstEver = !st.Unlocked;
            if (firstEver)
            {
                st.Unlocked = true;
                st.UnlockedUnixTime = now;
                st.UnlockCount = 1;
            }
            else if (countRepeat)
            {
                st.UnlockCount++;
            }

            var pq = st.PerQuest.Find(x => x.QuestId == questSlug);
            if (pq == null)
            {
                pq = new CardUnlockByQuest
                {
                    QuestId = questSlug,
                    Count = 1,
                    FirstUnixTime = now,
                    LastUnixTime = now
                };
                st.PerQuest.Add(pq);
            }
            else
            {
                pq.Count++;
                pq.LastUnixTime = now;
            }

            OnCardUnlocked?.Invoke(card, st);
            SaveState();
        }

        public void AddProgress(CardData card, int delta)
        {
            if (card == null)
                return;
            var st = GetState(card.Id);
            if (st == null)
                return;

            int max = Mathf.Max(1, card.KnowledgeValue);
            int before = st.ProgressPoints;
            st.ProgressPoints = Mathf.Clamp(st.ProgressPoints + delta, 0, max);
            if (st.ProgressPoints != before)
            {
                OnProgressChanged?.Invoke(card, st);
                SaveState();
            }
        }

        public static string QuestSlug(QuestData quest)
        {
            // Prefer explicit code if provided; fallback to QuestId.ToString().
            return quest != null
                ? quest.Id
                : string.Empty;
        }
    }
}
