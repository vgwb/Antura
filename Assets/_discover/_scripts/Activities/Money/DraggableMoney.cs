using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Antura.Discover.Activities
{
    /// <summary>
    /// Drag & drop behaviour for money tokens
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public class DraggableMoney : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [Header("Wiring")]
        public Canvas canvas;
        public RectTransform dragRoot;

        [Header("State")]
        public Transform OriginalParent;
        public bool IsOnMat;

        private CanvasGroup cg;
        private RectTransform rt;
        private Vector2 startPos;
        private PointerEventData lastEvent;

        void Awake()
        {
            cg = GetComponent<CanvasGroup>();
            rt = GetComponent<RectTransform>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            lastEvent = eventData;
            OriginalParent = transform.parent;
            startPos = rt.anchoredPosition;
            cg.blocksRaycasts = false;
            cg.alpha = 0.9f;

            if (dragRoot != null)
                transform.SetParent(dragRoot, true);
            else
                transform.SetParent(canvas.transform, true); // fallback
        }

        public void OnDrag(PointerEventData eventData)
        {
            lastEvent = eventData;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform, eventData.position, eventData.pressEventCamera, out var localPoint);
            rt.anchoredPosition = localPoint;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            lastEvent = eventData;
            cg.blocksRaycasts = true;
            cg.alpha = 1f;

            // If dropped onto a zone convert pointer pos to that zone's local space
            if (transform.parent != dragRoot && transform.parent != canvas.transform && transform.parent != OriginalParent)
            {
                var zoneRT = transform.parent as RectTransform;
                if (zoneRT &&
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(zoneRT, eventData.position, eventData.pressEventCamera, out var lp))
                {
                    rt.anchoredPosition = lp;
                }
                return;
            }

            // Not on valid zone -> return
            transform.SetParent(OriginalParent, true);
            rt.anchoredPosition = startPos;
        }
    }
}
