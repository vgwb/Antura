using System.Linq;
using UnityEngine;

namespace Antura.Discover.Achievements.UI
{
    /// <summary>
    /// Shows cards associated with a specific quest (those listed in CardDefinition.UnlockQuests).
    /// </summary>
    public class QuestCardsUI : MonoBehaviour
    {
        [Header("Refs")]
        public AchievementsManager manager;
        public QuestData quest;
        public Transform gridParent;
        public CardTile tilePrefab;
        public CardDetailsPanel detailsPanel;

        void Start() => Refresh();

        public void Refresh()
        {
            foreach (Transform c in gridParent)
                Destroy(c.gameObject);
            if (manager?.Database?.ById == null || quest == null)
                return;

            foreach (var def in manager.Database.ById.Values.Where(d => d != null))
            {
                if (def.UnlockQuests != null && def.UnlockQuests.Contains(quest))
                {
                    var st = manager.GetState(def.Id);
                    var tile = Instantiate(tilePrefab, gridParent);
                    tile.Bind(def, st, OnTileClicked);
                }
            }
        }

        private void OnTileClicked(CardDefinition def)
        {
            var st = manager.GetState(def.Id);
            detailsPanel?.Show(def, st);
        }
    }
}
