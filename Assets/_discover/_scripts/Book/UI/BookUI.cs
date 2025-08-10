using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Antura.Discover.Achievements.UI
{
    /// <summary>
    /// Displays all cards with dropdown filters for Country and Category (TextMeshPro).
    /// Click a tile to open the details panel.
    /// </summary>
    public class BookUI : MonoBehaviour
    {
        [Header("Refs")]
        public AchievementsManager manager;
        public Transform gridParent;             // parent with Grid/Vertical Layout
        public CardTile tilePrefab;
        public CardDetailsPanel detailsPanel;

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
            foreach (var t in spawned)
                if (t)
                    Destroy(t.gameObject);
            spawned.Clear();

            if (manager == null || manager.Database == null)
                return;

            var country = (Antura.Discover.Countries)countryDropdown.value;
            var category = (Antura.Discover.CardCategory)categoryDropdown.value;

            IEnumerable<CardDefinition> pool =
                country == Antura.Discover.Countries.None
                ? (IEnumerable<CardDefinition>)manager.Database.ById?.Values
                : manager.GetCardsByCountry(country);

            if (pool == null)
                return;

            foreach (var def in pool)
            {
                if (def == null)
                    continue;
                if (category != Antura.Discover.CardCategory.None && def.Category != category)
                    continue;

                var st = manager.GetState(def.Id);
                var tile = Instantiate(tilePrefab, gridParent);
                tile.Bind(def, st, OnTileClicked);
                spawned.Add(tile);
            }
        }

        private void OnTileClicked(CardDefinition def)
        {
            var st = manager.GetState(def.Id);
            detailsPanel?.Show(def, st);
        }
    }
}
