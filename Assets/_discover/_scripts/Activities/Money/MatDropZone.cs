using UnityEngine;
using UnityEngine.EventSystems;

namespace Antura.Discover.Activities
{
    /// <summary>
    /// The mat where tokens are counted towards the total.
    /// </summary>
    public class MatDropZone : MoneyDropZone
    {
        public ActivityMoney game;

        public override void OnDrop(PointerEventData eventData)
        {
            var drag = GetDraggable(eventData);
            if (drag == null)
                return;

            var view = drag.GetComponent<MoneyItemView>();
            if (view == null)
                return;

            // Reparent into this zone
            drag.transform.SetParent(transform, true);
            var rt = drag.GetComponent<RectTransform>();
            rt.anchoredPosition = Vector2.zero; // center snap; adjust if you want grid

            // Update game state (only if it wasn’t already on mat)
            if (!drag.IsOnMat)
            {
                drag.IsOnMat = true;
                game.OnItemPlaced(view.Model, drag);
            }
        }
    }
}
