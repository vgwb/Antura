using UnityEngine;
using UnityEngine.EventSystems;

namespace Antura.Discover.Activities
{
    /// <summary>
    /// The tray that holds available tokens; dropping here removes them from the mat total.
    /// </summary>
    public class TrayDropZone : MoneyDropZone
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
            rt.anchoredPosition = Vector2.zero;

            // If it was on mat, remove its value
            if (drag.IsOnMat)
            {
                drag.IsOnMat = false;
                game.OnItemRemoved(view.Model, drag);
            }
        }
    }
}
