using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Antura.Discover.Activities
{
    public class DraggableTile : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
    {
        public Image TileImage;
        public TextMeshProUGUI Label;
        public Antura.Discover.CardData ItemData { get; private set; }
        public int OriginalParentSlotIndex { get; set; } = -1;
        public RectTransform Rect => rectTransform;

        private object manager;
        private System.Action<int> onLiftedFromSlot;
        private System.Action onReturnedToPool;
        private System.Action<AudioClip> onPlayItemSound;
        private System.Action<Antura.Discover.CardData, DraggableTile> onFlashCorrectSlot;
        private System.Func<Transform> getPoolParent;
        private CanvasGroup canvasGroup;
        private RectTransform rectTransform;
        private Transform originalParent;

        private Transform activityTranform;
        private Vector3 originalPosition;
        private bool dragging;

        [Header("Audio Settings")]
        public float soundCooldown = 0.5f;
        private float lastSoundTime = -999f;

        public void Init(ActivityOrder mgr, Antura.Discover.CardData data, Transform activityRoot)
        {
            manager = mgr;
            ItemData = data;
            if (Label != null)
                Label.text = data.Title != null ? data.Title.GetLocalizedString() : data.name;

            var sprite = ResolveSprite(data);
            if (sprite != null)
                TileImage.sprite = sprite;

            // Set initial position
            rectTransform.anchoredPosition = Vector2.zero;
            activityTranform = activityRoot;

            // Bind callbacks for ActivityOrder
            onLiftedFromSlot = mgr.NotifyTileLiftedFromSlot;
            onReturnedToPool = mgr.NotifyTileReturnedToPool;
            onPlayItemSound = mgr.PlayItemSound;
            onFlashCorrectSlot = (ci, dt) => mgr.FlashCorrectSlot(ci, dt);
            getPoolParent = () => mgr.tilesPoolParent;
        }

        // Generic initializer for other activities (e.g., match)
        public void InitGeneric(Antura.Discover.CardData data, Transform activityRoot,
            System.Func<Transform> poolGetter,
            System.Action<int> onLift,
            System.Action onReturn,
            System.Action<AudioClip> onPlay,
            System.Action<Antura.Discover.CardData, DraggableTile> onHint = null,
            object owner = null)
        {
            manager = owner;
            ItemData = data;
            if (Label != null)
                Label.text = data.Title != null ? data.Title.GetLocalizedString() : data.name;
            var sprite = ResolveSprite(data);
            if (sprite != null)
                TileImage.sprite = sprite;
            rectTransform.anchoredPosition = Vector2.zero;
            activityTranform = activityRoot;

            getPoolParent = poolGetter;
            onLiftedFromSlot = onLift;
            onReturnedToPool = onReturn;
            onPlayItemSound = onPlay;
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

            // If it wasn't reparented by a DropSlot, it's still under the temp activity root
            if (transform.parent == activityTranform)
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

            var clip = ResolveAudio(ItemData);
            if (clip != null)
            {
                onPlayItemSound?.Invoke(clip);
                lastSoundTime = Time.unscaledTime;
            }

            // Tutorial hint
            onFlashCorrectSlot?.Invoke(ItemData, this);
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

        private static Sprite ResolveSprite(Antura.Discover.CardData data)
        {
            if (data == null)
                return null;
            if (data.Image != null)
                return data.Image;
            if (data.ImageAsset != null)
                return data.ImageAsset.Image;
            return null;
        }

        private static AudioClip ResolveAudio(Antura.Discover.CardData data)
        {
            if (data == null)
                return null;
            if (data.AudioAsset != null)
                return data.AudioAsset.Audio;
            return null;
        }
    }
}
