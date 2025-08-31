using UnityEngine;
using UnityEngine.EventSystems;

namespace Antura.Discover.Activities
{
    [DisallowMultipleComponent]
    public class AnswerTileClamp : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public ActivityMatch manager;
        private RectTransform rt;
        private RectTransform board;
        private Vector2 startPos;
        private Vector2 startPointer;
        private bool reparentedOnBegin;

        private void Awake()
        {
            rt = transform as RectTransform;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (manager == null || manager.BoardArea == null || rt == null)
                return;
            board = manager.BoardArea as RectTransform;
            if (board == null)
                return;
            // Ensure we drag in BoardArea space to clamp correctly
            if (rt.parent != board)
            {
                var world = rt.position;
                rt.SetParent(board, worldPositionStays: true);
                // Center anchors/pivot for consistent anchoredPosition semantics
                rt.anchorMin = new Vector2(0.5f, 0.5f);
                rt.anchorMax = new Vector2(0.5f, 0.5f);
                rt.pivot = new Vector2(0.5f, 0.5f);
                reparentedOnBegin = true;
                // Recompute anchored position from world
                var local = (Vector2)board.InverseTransformPoint(world);
                rt.anchoredPosition = local;
            }
            RectTransformUtility.ScreenPointToLocalPointInRectangle(board, eventData.position, eventData.pressEventCamera, out startPointer);
            startPos = rt.anchoredPosition;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (board == null || rt == null)
                return;
            Vector2 curr;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(board, eventData.position, eventData.pressEventCamera, out curr);
            var desired = startPos + (curr - startPointer);
            rt.anchoredPosition = ClampAnchoredToParent(board, rt, desired);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            // Keep parent as BoardArea; drop logic will reattach if needed
            reparentedOnBegin = false;
        }

        private static Vector2 ClampAnchoredToParent(RectTransform parent, RectTransform child, Vector2 desired)
        {
            var pRect = parent.rect;
            var cRect = child.rect;
            float halfPW = pRect.width * 0.5f;
            float halfPH = pRect.height * 0.5f;
            float halfCW = cRect.width * 0.5f;
            float halfCH = cRect.height * 0.5f;
            float minX = -halfPW + halfCW;
            float maxX = halfPW - halfCW;
            float minY = -halfPH + halfCH;
            float maxY = halfPH - halfCH;
            return new Vector2(Mathf.Clamp(desired.x, minX, maxX), Mathf.Clamp(desired.y, minY, maxY));
        }
    }
}
