using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Antura.Discover.Activities
{
    [RequireComponent(typeof(CanvasGroup))]
    public class JigsawPiece : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public int targetRow;
        public int targetCol;
        public ActivityJigsawPuzzle manager;
        public RectTransform RectTransform { get; private set; }
        public RectTransform poolParent;
        public RectTransform dragLayer;
        public float snapDistance = 60f;
        public Vector2 originalLocalPos;

        private CanvasGroup cg;
        private JigsawSlot parentSlot;
        public JigsawSlot PreviousSlot { get; private set; }   // NEW
        private Vector3 dragOffset;

        private void Awake()
        {
            RectTransform = GetComponent<RectTransform>();
            cg = GetComponent<CanvasGroup>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            cg.blocksRaycasts = false;
            transform.SetParent(dragLayer ? dragLayer : RectTransform.parent);

            PreviousSlot = parentSlot;         // remember where it was
            if (parentSlot != null)
            {
                parentSlot.currentPiece = null;
                parentSlot = null;
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                dragLayer ? dragLayer : (RectTransform)transform.parent,
                eventData.position, eventData.pressEventCamera, out pos);
            RectTransform.anchoredPosition = pos;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            cg.blocksRaycasts = true;
            manager.TryPlaceAtSlot(this);
        }

        public void SetParentSlot(JigsawSlot slot)
        {
            parentSlot = slot;
            slot.currentPiece = this;
            RectTransform.SetParent(slot.transform, false);
            RectTransform.anchoredPosition = Vector2.zero;
        }

        public void ReturnToPool()
        {
            RectTransform.SetParent(poolParent, false);
            RectTransform.anchoredPosition = originalLocalPos;
            parentSlot = null;
        }

        public void ReturnToPoolScatter(Vector2 scatterPos)   // NEW
        {
            RectTransform.SetParent(poolParent, false);
            RectTransform.anchoredPosition = scatterPos;
            originalLocalPos = scatterPos;
            parentSlot = null;
        }

        public void DetachFromSlot()  // NEW
        {
            if (parentSlot != null)
            {
                if (parentSlot.currentPiece == this)
                    parentSlot.currentPiece = null;
                parentSlot = null;
            }
        }
    }
}
