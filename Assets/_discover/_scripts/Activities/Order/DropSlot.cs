using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Antura.Discover
{
    public class DropSlot : MonoBehaviour, IDropHandler
    {
        public int slotIndex;
        public ActivityOrder puzzleManager;

        [Header("Optional UI")]
        public Image highlightImage; // assign an Image on the slot prefab if you want coloring

        public void OnDrop(PointerEventData eventData)
        {
            var tile = eventData.pointerDrag ? eventData.pointerDrag.GetComponent<DraggableTile>() : null;
            if (tile == null)
                return;

            puzzleManager.PlaceTile(tile, slotIndex);
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
