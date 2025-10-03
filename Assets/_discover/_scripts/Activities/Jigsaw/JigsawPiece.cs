using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Antura.Discover.Audio;

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
        public JigsawSlot PreviousSlot { get; private set; }
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

            PreviousSlot = parentSlot;
            if (parentSlot != null)
            {
                parentSlot.currentPiece = null;
                parentSlot = null;
            }

            DiscoverAudioManager.I?.PlaySfx(DiscoverSfx.ActivityClick);
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
            // Pulse on placement
            var ab = manager as ActivityBase;
            if (ab != null)
                ab.Pulse(transform);
            DiscoverAudioManager.I?.PlaySfx(DiscoverSfx.ActivityDrop);
        }

        public void ReturnToPool()
        {
            RectTransform.SetParent(poolParent, false);
            RectTransform.anchoredPosition = originalLocalPos;
            parentSlot = null;
            DiscoverAudioManager.I?.PlaySfx(DiscoverSfx.ActivityDrop);
        }

        public void ReturnToPoolScatter(Vector2 scatterPos)
        {
            RectTransform.SetParent(poolParent, false);
            RectTransform.anchoredPosition = scatterPos;
            originalLocalPos = scatterPos;
            parentSlot = null;
            DiscoverAudioManager.I?.PlaySfx(DiscoverSfx.ActivityDrop);
        }

        public void DetachFromSlot()
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
