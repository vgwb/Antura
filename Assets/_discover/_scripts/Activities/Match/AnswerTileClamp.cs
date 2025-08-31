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

        private void Awake()
        {
            rt = transform as RectTransform;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (manager == null || manager.BoardArea == null || rt == null)
                return;
            board = manager.BoardArea as RectTransform;
            var parentRT = rt.parent as RectTransform;
            if (parentRT == null)
                return;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRT, eventData.position, eventData.pressEventCamera, out startPointer);
            startPos = rt.anchoredPosition;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (board == null || rt == null)
                return;
            var parentRT = rt.parent as RectTransform;
            if (parentRT == null)
                return;
            Vector2 curr;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRT, eventData.position, eventData.pressEventCamera, out curr);
            var desired = startPos + (curr - startPointer);
            rt.anchoredPosition = ClampAnchoredToParent(board, rt, desired);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
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
