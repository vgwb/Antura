using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Antura.Discover; // for AchievementsManager, CardData

namespace Antura.Discover.UI
{
    /// <summary>
    /// Displays all cards with dropdown filters for Country and Category (TextMeshPro).
    /// Click a tile to open the details panel.
    /// </summary>
    public class BookUI : MonoBehaviour
    {
        [Header("Refs")]
        public Transform gridParent;             // parent with Grid/Vertical Layout
        public CardTile tilePrefab;
        public CardDetailsPanel detailsPanel;
        [Tooltip("Master database of all cards (assign the CardDatabase asset)")]
        public CardDatabaseData Database;

        [Header("Filters")]
        public TMP_Dropdown countryDropdown;     // populate with Countries enum
        public TMP_Dropdown categoryDropdown;    // populate with CardCategory enum

        private readonly List<CardTile> spawned = new();

        void Start()
        {
            BuildDropdowns();
            Refresh();
        }

        public void OnFilterChanged()
        {
            Refresh();
        }

        private void BuildDropdowns()
        {
            if (countryDropdown != null)
            {
                countryDropdown.ClearOptions();
                var names = Enum.GetNames(typeof(Antura.Discover.Countries));
                countryDropdown.AddOptions(new List<string>(names));
                // Default selection:
                var defaultIndex = Array.IndexOf(names, Antura.Discover.Countries.France.ToString());
                countryDropdown.value = Mathf.Clamp(defaultIndex, 0, names.Length - 1);
                countryDropdown.RefreshShownValue();
            }

            if (categoryDropdown != null)
            {
                categoryDropdown.ClearOptions();
                var names = Enum.GetNames(typeof(Antura.Discover.CardCategory));
                categoryDropdown.AddOptions(new List<string>(names));
                var defaultIndex = Array.IndexOf(names, Antura.Discover.CardCategory.None.ToString());
                categoryDropdown.value = Mathf.Clamp(defaultIndex, 0, names.Length - 1);
                categoryDropdown.RefreshShownValue();
            }
        }

        private void Refresh()
        {
            var manager = DiscoverAppManager.I;
            if (manager == null || manager.CurrentProfile == null)
            {
                Debug.LogWarning("BookUI: DiscoverAppManager or CurrentProfile is missing. Cannot compute lock state.");
            }

            if (Database == null)
            {
                Debug.LogWarning("BookUI: CardDatabaseData asset not assigned.");
                return;
            }

            // Collect cards according to filters
            var cards = new List<CardData>();
            IEnumerable<CardData> all;
            if (Database.ById != null && Database.ById.Count > 0)
            {
                all = Database.ById.Values;
            }
            else if (Database.Collections != null)
            {
                var list = new List<CardData>();
                foreach (var col in Database.Collections)
                {
                    if (col == null || col.Cards == null)
                        continue;
                    list.AddRange(col.Cards);
                }
                all = list;
            }
            else
            {
                all = Array.Empty<CardData>();
            }

            Antura.Discover.Countries? country = null;
            if (countryDropdown)
            {
                var names = Enum.GetNames(typeof(Antura.Discover.Countries));
                if (countryDropdown.value >= 0 && countryDropdown.value < names.Length)
                {
                    if (Enum.TryParse<Antura.Discover.Countries>(names[countryDropdown.value], out var c))
                        country = c;
                }
            }

            Antura.Discover.CardCategory? category = null;
            if (categoryDropdown)
            {
                var names = Enum.GetNames(typeof(Antura.Discover.CardCategory));
                if (categoryDropdown.value >= 0 && categoryDropdown.value < names.Length)
                {
                    if (Enum.TryParse<Antura.Discover.CardCategory>(names[categoryDropdown.value], out var cat))
                    {
                        // Treat None as no category filter
                        if (cat != Antura.Discover.CardCategory.None)
                            category = cat;
                    }
                }
            }

            foreach (var c in all)
            {
                if (c == null)
                    continue;
                if (country.HasValue && c.Country != country.Value)
                    continue;
                if (category.HasValue && c.Category != category.Value)
                    continue;
                cards.Add(c);
            }

            // Spawn grid
            ClearGrid();
            foreach (var def in cards)
            {
                var tile = Instantiate(tilePrefab, gridParent);
                spawned.Add(tile);
                Antura.Discover.CardState state = null;
                if (manager != null && manager.CurrentProfile != null && manager.CurrentProfile.cards != null)
                {
                    manager.CurrentProfile.cards.TryGetValue(def.Id, out state);
                }
                tile.Init(def, state, OnTileClicked);
            }
        }


        private void OnTileClicked(CardData def)
        {
            var manager = DiscoverAppManager.I;
            Antura.Discover.CardState st = null;
            if (manager != null && manager.CurrentProfile != null && manager.CurrentProfile.cards != null)
            {
                manager.CurrentProfile.cards.TryGetValue(def.Id, out st);
            }
            if (detailsPanel != null)
                detailsPanel.Show(def, st);
        }

        private void ClearGrid()
        {
            for (int i = 0; i < spawned.Count; i++)
            {
                if (spawned[i] != null)
                    Destroy(spawned[i].gameObject);
            }
            spawned.Clear();
            if (gridParent != null)
            {
                for (int i = gridParent.childCount - 1; i >= 0; i--)
                    Destroy(gridParent.GetChild(i).gameObject);
            }
        }
    }
}
