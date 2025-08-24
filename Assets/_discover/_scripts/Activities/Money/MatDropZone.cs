using UnityEngine;
using UnityEngine.EventSystems;

namespace Antura.Discover.Activities
{
    /// <summary>
    /// The mat where tokens are counted towards the total
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

            drag.transform.SetParent(transform, true);

            // Position at drop point (no centering)
            var rt = drag.GetComponent<RectTransform>();
            var zoneRT = transform as RectTransform;
            if (zoneRT &&
                RectTransformUtility.ScreenPointToLocalPointInRectangle(zoneRT, eventData.position, eventData.pressEventCamera, out var lp))
            {
                rt.anchoredPosition = lp;
            }

            if (!drag.IsOnMat)
            {
                drag.IsOnMat = true;
                game.OnItemPlaced(view.Model, drag);
                // Pulse feedback
                var mb = game as ActivityBase;
                if (mb != null)
                    mb.Pulse(drag.transform);
            }
        }
    }
}
