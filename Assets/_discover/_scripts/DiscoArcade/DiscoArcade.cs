using Antura.UI;
using Antura.Discover.UI;
using Antura.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Antura.Discover
{
    /// <summary>
    /// Displays all cards with dropdown filters for Country and Category (TextMeshPro).
    /// Click a tile to open the details panel.
    /// </summary>
    public class DiscoArcade : SingletonMonoBehaviour<DiscoArcade>
    {
        [Header("Refs")]
        public Button BtnClose;
        public Transform gridParent;             // parent with Grid/Vertical Layout
        public CardTile tilePrefab;
        public CardDetailsPanel detailsPanel;
        [Tooltip("Master database of all cards (assign the CardDatabase asset)")]
        public CardDatabaseData Database;

        private readonly List<CardTile> spawned = new();

        protected override void Init()
        {
            BtnClose.onClick.AddListener(CloseDiscoArcade);
        }

        void OnDestroy()
        {
            BtnClose.onClick.RemoveListener(CloseDiscoArcade);
        }

        void CloseDiscoArcade()
        {
            gameObject.SetActive(false);
            GlobalUI.ShowPauseMenu(true);
        }

        public void Open()
        {
            GlobalUI.ShowPauseMenu(false);
            gameObject.SetActive(true);
            Refresh();
        }

        void Start()
        {
            Refresh();
        }

        public void OnFilterChanged()
        {
            Refresh();
        }

        private void Refresh()
        {
            var manager = DiscoverAppManager.I;

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
