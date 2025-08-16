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
        public QuestData quest;
        public Transform gridParent;
        public CardTile tilePrefab;
        public CardDetailsPanel detailsPanel;

        void Start() => Refresh();

        public void Refresh()
        {

        }

        private void OnTileClicked(CardData def)
        {
        }
    }
}
