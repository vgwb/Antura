using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.UI;

namespace Antura.Discover
{
    public class DraggableTile : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
    {
        public Image TileImage;
        public TextMeshProUGUI Label;
        public CardItem ItemData { get; private set; }
        public int OriginalParentSlotIndex { get; set; } = -1;
        public RectTransform Rect => rectTransform;

        private ActivityOrder manager;
        private CanvasGroup canvasGroup;
        private RectTransform rectTransform;
        private Transform originalParent;

        private Transform activityTranform;
        private Vector3 originalPosition;
        private bool dragging;

        [Header("Audio Settings")]
        public float soundCooldown = 0.5f;
        private float lastSoundTime = -999f;

        public void Init(ActivityOrder mgr, CardItem data, Transform activityRoot)
        {
            manager = mgr;
            ItemData = data;
            if (Label != null)
                Label.text = data.Name;

            if (data.Image != null)
            {
                TileImage.sprite = data.Image;
            }

            // Set initial position
            rectTransform.anchoredPosition = Vector2.zero;
            activityTranform = activityRoot;
        }

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            rectTransform = GetComponent<RectTransform>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            dragging = true;

            originalParent = transform.parent;
            originalPosition = transform.position;

            OriginalParentSlotIndex = -1;
            var slot = originalParent.GetComponent<DropSlot>();
            if (slot != null)
            {
                OriginalParentSlotIndex = slot.slotIndex;
                manager.NotifyTileLiftedFromSlot(OriginalParentSlotIndex);
            }

            canvasGroup.blocksRaycasts = false;
            transform.SetParent(activityTranform);
        }

        public void OnDrag(PointerEventData eventData)
        {
            rectTransform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            canvasGroup.blocksRaycasts = true;
            dragging = false;

            if (transform.parent == transform.root)
            {
                if (OriginalParentSlotIndex >= 0)
                {
                    MoveToSlot(originalParent, OriginalParentSlotIndex);
                }
                else
                {
                    MoveToPool(manager.tilesPoolParent);
                    manager.NotifyTileReturnedToPool();
                }
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (dragging)
                return;

            if (Time.unscaledTime - lastSoundTime < soundCooldown)
                return;

            if (ItemData.AudioClip != null)
            {
                manager.PlayItemSound(ItemData.AudioClip);
                lastSoundTime = Time.unscaledTime;
            }

            // Tutorial hint
            manager.FlashCorrectSlot(ItemData, this);
        }

        public void MoveToSlot(Transform slotParent, int slotIndex)
        {
            SnapCentered(slotParent);
            OriginalParentSlotIndex = slotIndex;
        }

        public void MoveToPool(Transform poolParent)
        {
            SnapCentered(poolParent);
            OriginalParentSlotIndex = -1;
        }

        public void SnapTo(Transform dropTarget)
        {
            SnapCentered(dropTarget);
        }

        private void SnapCentered(Transform newParent)
        {
            // Reparent without keeping world position, so local is reset
            rectTransform.SetParent(newParent, false);

            // Ensure tile anchors/pivot are centered so (0,0) is the slot center
            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            rectTransform.pivot = new Vector2(0.5f, 0.5f);

            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.localRotation = Quaternion.identity;
            rectTransform.localScale = Vector3.one;
        }
    }
}
