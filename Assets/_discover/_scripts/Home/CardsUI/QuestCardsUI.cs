using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.Discover.UI
{
    /// <summary>
    /// Shows cards associated with a specific quest
    /// </summary>
    public class QuestCardsUI : MonoBehaviour
    {
        [Header("References")]
        public Transform gridParent;
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

            if (detailsPanel != null)
            {
                detailsPanel.SetOwner(this);
                detailsPanel.SetNavData(GetCardDefs(), GetState);
            }

        }

        private void OnTileClicked(CardData def)
        {
            var manager = DiscoverAppManager.I;
            CardState st = null;
            if (manager != null && manager.CurrentProfile != null && manager.CurrentProfile.cards != null)
            {
                manager.CurrentProfile.cards.TryGetValue(def.Id, out st);
            }
            if (detailsPanel != null)
            {
                detailsPanel.SetNavData(GetCardDefs(), GetState);
                detailsPanel.Show(def, st);
            }
        }

        // Navigation helpers used by CardDetailsPanel
        public bool TryGetNext(CardData current, out CardData next, out CardState state)
        {
            next = null;
            state = null;
            if (current == null || spawned.Count == 0)
                return false;
            var list = GetCardDefs();
            int i = list.FindIndex(cd => cd == current);
            if (i < 0)
                i = list.FindIndex(cd => cd != null && cd.Id == current.Id);
            if (i < 0 || i + 1 >= list.Count)
                return false;
            next = list[i + 1];
            state = GetState(next);
            return true;
        }

        public bool TryGetPrev(CardData current, out CardData prev, out CardState state)
        {
            prev = null;
            state = null;
            if (current == null || spawned.Count == 0)
                return false;
            var list = GetCardDefs();
            int i = list.FindIndex(cd => cd == current);
            if (i < 0)
                i = list.FindIndex(cd => cd != null && cd.Id == current.Id);
            if (i <= 0)
                return false;
            prev = list[i - 1];
            state = GetState(prev);
            return true;
        }

        public bool HasNext(CardData current) => TryGetNext(current, out _, out _);
        public bool HasPrev(CardData current) => TryGetPrev(current, out _, out _);

        private List<CardData> GetCardDefs()
        {
            var list = new List<CardData>(spawned.Count);
            foreach (var tile in spawned)
            {
                if (tile != null && tile.Data != null)
                    list.Add(tile.Data);
            }
            return list;
        }

        private CardState GetState(CardData def)
        {
            if (def == null)
                return null;
            // Use the state from the existing tile if possible to avoid recomputing
            var tile = spawned.FirstOrDefault(t => t != null && t.Data == def);
            if (tile != null)
                return tile.State;

            var manager = DiscoverAppManager.I;
            CardState st = null;
            if (manager != null && manager.CurrentProfile != null && manager.CurrentProfile.cards != null)
            {
                manager.CurrentProfile.cards.TryGetValue(def.Id, out st);
            }
            return st;
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
