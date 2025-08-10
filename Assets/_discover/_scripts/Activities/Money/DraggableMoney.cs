using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Antura.Discover.Activities
{
    /// <summary>
    /// Drag & drop behaviour for money tokens.
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public class DraggableMoney : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [Header("Wiring")]
        public Canvas canvas;                    // Assign your UI canvas
        public RectTransform dragRoot;           // Optional: a top-level "DragLayer" under the Canvas

        [Header("State")]
        public Transform OriginalParent;
        public bool IsOnMat;

        private CanvasGroup cg;
        private RectTransform rt;
        private Vector2 startPos;

        void Awake()
        {
            cg = GetComponent<CanvasGroup>();
            rt = GetComponent<RectTransform>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
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
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform, eventData.position, eventData.pressEventCamera, out var localPoint);
            rt.anchoredPosition = localPoint;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            cg.blocksRaycasts = true;
            cg.alpha = 1f;

            // If not dropped on a valid DropZone, return home
            if (transform.parent == dragRoot || transform.parent == canvas.transform)
            {
                transform.SetParent(OriginalParent, true);
                rt.anchoredPosition = startPos;
            }
        }
    }
}
