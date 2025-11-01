using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Antura.Discover.Audio;

namespace Antura.Discover.Activities
{
    public class DraggableTile : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
    {
        public Image TileImage;
        public TextMeshProUGUI Label;
        public CardData CardData { get; private set; }
        public int OriginalParentSlotIndex { get; set; } = -1;
        public RectTransform Rect => rectTransform;

        private object activityManager;
        private System.Action<int> onLiftedFromSlot;
        private System.Action onReturnedToPool;
        private System.Action<AudioClip> onPlayItemSound;
        private System.Action<CardData, DraggableTile> onFlashCorrectSlot;
        private System.Func<Transform> getPoolParent;
        private CanvasGroup canvasGroup;
        private RectTransform rectTransform;
        private Transform originalParent;

        private Transform activityTransform;
        private Vector3 originalPosition;
        private bool dragging;

        [Header("Audio Settings")]
        public float soundCooldown = 0.5f;
        private float lastSoundTime = -999f;

        public void Init(ActivityOrder activityOrder, CardData data, Transform activityRoot)
        {
            activityManager = activityOrder;
            CardData = data;
            if (Label != null)
                Label.text = data.Title != null ? data.Title.GetLocalizedString() : data.name;

            var sprite = ResolveSprite(data);
            if (sprite != null)
                TileImage.sprite = sprite;

            // Set initial position
            rectTransform.anchoredPosition = Vector2.zero;
            activityTransform = activityRoot;

            // Bind callbacks for ActivityOrder
            onLiftedFromSlot = activityOrder.NotifyTileLiftedFromSlot;
            onReturnedToPool = activityOrder.NotifyTileReturnedToPool;
            onPlayItemSound = activityOrder.PlayItemSound;
            onFlashCorrectSlot = (cardInfo, draggableTile) => activityOrder.FlashCorrectSlot(cardInfo, draggableTile);
            getPoolParent = () => activityOrder.tilesPoolParent;
        }

        // Generic initializer for other activities (e.g., match)
        public void InitGeneric(CardData data, Transform activityRoot,
            System.Func<Transform> poolGetter,
            System.Action<int> onLift,
            System.Action onReturn,
            System.Action<CardData, DraggableTile> onHint = null,
            object owner = null)
        {
            activityManager = owner;
            CardData = data;
            if (Label != null)
                Label.text = data.Title != null ? data.Title.GetLocalizedString() : data.name;
            var sprite = ResolveSprite(data);
            if (sprite != null)
                TileImage.sprite = sprite;
            rectTransform.anchoredPosition = Vector2.zero;
            activityTransform = activityRoot;

            getPoolParent = poolGetter;
            onLiftedFromSlot = onLift;
            onReturnedToPool = onReturn;
            // onPlayItemSound = onPlay;
            onFlashCorrectSlot = onHint;
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
                onLiftedFromSlot?.Invoke(OriginalParentSlotIndex);
            }

            canvasGroup.blocksRaycasts = false;
            transform.SetParent(activityTransform);

            DiscoverAudioManager.I?.PlaySfx(DiscoverSfx.ActivityClick);
        }

        public void OnDrag(PointerEventData eventData)
        {
            rectTransform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            canvasGroup.blocksRaycasts = true;
            dragging = false;

            // If it wasn't reparented by a DropSlot, it's still under the temp activity root
            if (transform.parent == activityTransform)
            {
                if (OriginalParentSlotIndex >= 0)
                {
                    MoveToSlot(originalParent, OriginalParentSlotIndex);
                }
                else
                {
                    var pool = getPoolParent != null ? getPoolParent() : null;
                    if (pool != null)
                        MoveToPool(pool);
                    DiscoverAudioManager.I?.PlaySfx(DiscoverSfx.ActivityDrop);
                    onReturnedToPool?.Invoke();
                }
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (dragging)
                return;

            if (Time.unscaledTime - lastSoundTime < soundCooldown)
                return;

            var clip = ResolveAudio(CardData);
            if (clip != null)
            {
                onPlayItemSound?.Invoke(clip);
                lastSoundTime = Time.unscaledTime;
            }

            // Tutorial hint
            onFlashCorrectSlot?.Invoke(CardData, this);
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

        private static Sprite ResolveSprite(CardData data)
        {
            if (data == null)
                return null;
            if (data.ImageAsset != null)
                return data.ImageAsset.Image;
            return null;
        }

        private static AudioClip ResolveAudio(CardData data)
        {
            if (data == null)
                return null;
            if (data.AudioAsset != null)
                return data.AudioAsset.Audio;
            return null;
        }
    }
}
