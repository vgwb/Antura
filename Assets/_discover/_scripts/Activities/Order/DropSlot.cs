using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Antura.Discover.Activities
{
    public class DropSlot : MonoBehaviour, IDropHandler
    {
        public int slotIndex;
        public ActivityOrder activityManager;

        [Header("Optional UI")]
        public Image highlightImage;

        public void OnDrop(PointerEventData eventData)
        {
            var tile = eventData.pointerDrag ? eventData.pointerDrag.GetComponent<DraggableTile>() : null;
            if (tile == null)
                return;

            activityManager.PlaceTile(tile, slotIndex);
        }

        public void SetHighlight(Color c, float alpha = 0.35f)
        {
            if (!highlightImage)
                return;
            c.a = alpha;
            highlightImage.color = c;
        }

        public void ClearHighlight()
        {
            if (!highlightImage)
                return;
            var c = highlightImage.color;
            c.a = 0f;
            highlightImage.color = c;
        }
    }
}
