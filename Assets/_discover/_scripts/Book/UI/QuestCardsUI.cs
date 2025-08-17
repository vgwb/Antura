using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Antura.Discover.UI
{
    /// <summary>
    /// Shows cards associated with a specific quest
    /// </summary>
    public class QuestCardsUI : MonoBehaviour
    {
        [Header("Refs")]
        public Transform gridParent;             // parent with Grid/Vertical Layout
        public CardTile tilePrefab;
        public CardDetailsPanel detailsPanel;
        [Tooltip("Master database of all cards (assign the CardDatabase asset)")]
        public CardDatabaseData Database;
        private readonly List<CardTile> spawned = new();

        public void Init(QuestData questData)
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


            // Spawn grid
            ClearGrid();
            foreach (var def in questData.Cards)
            {
                var tile = Instantiate(tilePrefab, gridParent);
                spawned.Add(tile);
                CardState state = null;
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
