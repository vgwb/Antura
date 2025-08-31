using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Antura.Discover.Activities
{
    /// <summary>
    /// Independent drop handler for ActivityMatch. Attach this to the dropSlotPrefab.
    /// Provides simple highlight helpers used by ActivityMatch.
    /// </summary>
    [DisallowMultipleComponent]
    public class MatchDropSlot : MonoBehaviour, IDropHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [Tooltip("Assigned by ActivityMatch at runtime")]
        public ActivityMatch manager;
        [Tooltip("Assigned by ActivityMatch at runtime")]
        public int slotIndex = -1;
        [Tooltip("When true, acts as a pool drop area instead of a question slot.")]
        public bool IsPoolArea = false;

        // Drag state for moving the whole question group
        private RectTransform dragRoot;
        private bool draggingQuestion;
        private Vector2 startRootPos;
        private Vector2 startPointerPos;

        private void Awake()
        {
            // If used under a RectTransform, stretch to cover parent by default
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
            if (IsPoolArea)
            {
                // If dropping over another detached tile, do nothing (keep position)
                var raycaster = GetComponentInParent<GraphicRaycaster>();
                if (raycaster != null)
                {
                    var results = new List<RaycastResult>();
                    raycaster.Raycast(eventData, results);
                    foreach (var r in results)
                    {
                        var otherTile = r.gameObject ? r.gameObject.GetComponentInParent<DraggableTile>() : null;
                        if (otherTile != null && otherTile != tile)
                        {
                            // If the tile under pointer is not attached, keep current position and just update memory
                            if (manager != null && !manager.IsTileAttached(otherTile))
                            {
                                manager.OnTileDroppedInPoolKeepPosition(tile);
                                return;
                            }
                            break; // hit a tile, but it's attached; fall back to normal drop handling
                        }
                    }
                }
                manager.OnTileDroppedInPool(tile, eventData);
            }
            else
                manager.PlaceTileAt(tile, slotIndex);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (IsPoolArea)
                return; // don't drag the pool
            // Begin dragging the question group when dragging starts on this overlay
            draggingQuestion = true;
            dragRoot = transform.parent as RectTransform;
            if (dragRoot == null)
                return;
            // Bring to front while dragging
            dragRoot.SetAsLastSibling();
            var parentRT = dragRoot.parent as RectTransform;
            if (parentRT == null)
                return;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRT, eventData.position, eventData.pressEventCamera, out startPointerPos);
            startRootPos = dragRoot.anchoredPosition;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (IsPoolArea)
                return;
            if (!draggingQuestion || dragRoot == null)
                return;
            var parentRT = dragRoot.parent as RectTransform;
            if (parentRT == null)
                return;
            Vector2 curr;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRT, eventData.position, eventData.pressEventCamera, out curr);
            var delta = curr - startPointerPos;
            var next = startRootPos + delta;
            // Clamp inside board area if available
            RectTransform boardArea = null;
            if (manager != null && manager.BoardArea != null)
                boardArea = manager.BoardArea as RectTransform;
            if (boardArea != null)
            {
                var rt = dragRoot;
                float halfPW = boardArea.rect.width * 0.5f;
                float halfPH = boardArea.rect.height * 0.5f;
                float halfCW = rt.rect.width * 0.5f;
                float halfCH = rt.rect.height * 0.5f;
                float minX = -halfPW + halfCW;
                float maxX = halfPW - halfCW;
                float minY = -halfPH + halfCH;
                float maxY = halfPH - halfCH;
                next.x = Mathf.Clamp(next.x, minX, maxX);
                next.y = Mathf.Clamp(next.y, minY, maxY);
            }
            dragRoot.anchoredPosition = next;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (IsPoolArea)
                return;
            draggingQuestion = false;
        }

    }
}
