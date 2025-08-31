using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Antura.Discover.Activities
{
    /// <summary>
    /// Pool/board drop area: when a DraggableTile is dropped here, it stays where dropped.
    /// Auto-adds a transparent raycastable Image and stretches to its RectTransform area.
    /// </summary>
    [DisallowMultipleComponent]
    public class MatchPoolDropArea : MonoBehaviour, IDropHandler
    {
        [Tooltip("Assigned by ActivityMatch at runtime")]
        public ActivityMatch manager;

        private void Awake()
        {
            var img = GetComponent<Image>();
            if (img == null)
                img = gameObject.AddComponent<Image>();
            // Hide visually, keep raycast to capture drops
            var c = img.color;
            c.a = 0f;
            img.color = c;
            img.raycastTarget = true;

            var rt = transform as RectTransform;
            if (rt != null)
            {
                rt.anchorMin = Vector2.zero;
                rt.anchorMax = Vector2.one;
                rt.offsetMin = Vector2.zero;
                rt.offsetMax = Vector2.zero;
                rt.pivot = new Vector2(0.5f, 0.5f);
            }
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (manager == null)
                return;
            var tile = eventData.pointerDrag ? eventData.pointerDrag.GetComponent<DraggableTile>() : null;
            if (tile == null)
                return;
            // If dropping on top of another tile, ignore and keep current position
            var raycaster = GetComponentInParent<GraphicRaycaster>();
            if (raycaster != null)
            {
                var results = new System.Collections.Generic.List<RaycastResult>();
                raycaster.Raycast(eventData, results);
                foreach (var r in results)
                {
                    var otherTile = r.gameObject ? r.gameObject.GetComponentInParent<DraggableTile>() : null;
                    if (otherTile != null && otherTile != tile)
                    {
                        manager.OnTileDroppedInPoolKeepPosition(tile);
                        return;
                    }
                }
            }
            manager.OnTileDroppedInPool(tile, eventData);
        }
    }
}
