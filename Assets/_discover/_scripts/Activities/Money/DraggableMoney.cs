using UnityEngine;
using UnityEngine.EventSystems;

namespace Antura.Discover.Activities
{
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
            // Convert pointer to the local space of the current drag parent so the center stays under the cursor
            RectTransform parentRT = (dragRoot != null ? dragRoot : canvas.transform as RectTransform);
            if (parentRT && RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRT, eventData.position, eventData.pressEventCamera, out var localPoint))
            {
                // With pivot already set to center (0.5,0.5) this places the token center at the pointer.
                rt.anchoredPosition = localPoint;
            }
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
            // Not on a different drop zone: freely reposition within original tray instead of snapping back
            if (OriginalParent is RectTransform parentRT)
            {
                transform.SetParent(OriginalParent, true);
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRT, eventData.position, eventData.pressEventCamera, out var lpTray))
                {
                    // Optionally clamp within tray bounds
                    var r = parentRT.rect;
                    float halfW = r.width * 0.5f;
                    float halfH = r.height * 0.5f;
                    lpTray.x = Mathf.Clamp(lpTray.x, -halfW, halfW);
                    lpTray.y = Mathf.Clamp(lpTray.y, -halfH, halfH);
                    rt.anchoredPosition = lpTray;
                }
                else
                {
                    // Fallback: keep last dragged position (already in canvas space). Convert through canvas if needed.
                    rt.anchoredPosition = startPos;
                }
            }
        }
    }
}
