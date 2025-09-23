using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.Discover
{
    /// <summary>
    /// Displays cookies count with a small scale animation when the value changes.
    /// Can animate a pickup icon from a world position to the counter icon.
    /// </summary>
    public class CookiesDisplay : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TextMeshProUGUI label;
        [SerializeField] private RectTransform icon;
        [SerializeField] private Canvas canvas;
        [SerializeField] private Sprite defaultCookieSprite;

        [Header("Animation")]
        [SerializeField] private float unitInterval = 0.08f;
        public float punchScale = 0.15f;
        public float punchDuration = 0.3f;
        public float flyDuration = 1f;
        public Ease flyEase = Ease.InOutQuad;

        private Coroutine animCo;

        private Tween punchTween;
        private InventoryManager inventoryManager;
        private bool initialized;

        void Awake()
        {
            if (canvas == null)
                canvas = GetComponentInParent<Canvas>();
        }

        void OnEnable() { }

        void OnDisable()
        {
            if (inventoryManager != null)
                inventoryManager.OnCookiesChanged -= OnCookiesChanged;
        }

        /// <summary>
        /// Explicit initialization, called by UIManager when systems are ready.
        /// Binds to the given InventoryManager and optional Canvas override.
        /// </summary>
        public void Initialize(Canvas canvasOverride = null)
        {
            inventoryManager = QuestManager.I.Inventory;
            if (canvasOverride != null)
                canvas = canvasOverride;

            if (inventoryManager != null)
            {
                inventoryManager.OnCookiesChanged -= OnCookiesChanged;
                inventoryManager.OnCookiesChanged += OnCookiesChanged;
                SetValue(inventoryManager.GetCookies());
            }
            initialized = true;
        }

        void OnCookiesChanged(int value, bool animate)
        {
            SetValue(value);
            PlayPunch();
        }

        void SetValue(int value)
        {
            if (label != null)
                label.text = value.ToString();
        }

        void PlayPunch()
        {
            if (icon == null)
                return;
            punchTween?.Kill();
            punchTween = icon.DOPunchScale(Vector3.one * punchScale, punchDuration);
        }

        private IEnumerator PlayPopSequence(int units)
        {
            for (int i = 0; i < units; i++)
            {
                PlayPunch();
                yield return new WaitForSeconds(unitInterval);
            }
            animCo = null;
        }

        /// <summary>
        /// Plays a pickup animation from a world position to this counter's icon.
        /// </summary>
        public void PlayPickupFromWorld(Vector3 worldPos, Sprite spriteOverride = null)
        {
            if (icon == null)
                return;

            // Resolve the target space: use the icon's parent RectTransform
            var parentRT = icon.parent as RectTransform;
            if (parentRT == null)
                return;

            // Determine cameras: use 3D camera for world->screen, UI camera (or null for overlay) for screen->local
            var iconCanvas = icon.GetComponentInParent<Canvas>();
            var cam3D = Camera.main != null ? Camera.main : Camera.current;
            if (cam3D == null)
                return;
            Camera uiCam = null;
            if (iconCanvas != null && iconCanvas.renderMode != RenderMode.ScreenSpaceOverlay)
                uiCam = iconCanvas.worldCamera != null ? iconCanvas.worldCamera : cam3D;

            // Create temp UI image under the same parent as the icon (same coordinate space)
            var go = new GameObject("CookiePickupFx", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            var rt = go.GetComponent<RectTransform>();
            var img = go.GetComponent<Image>();
            img.sprite = spriteOverride != null ? spriteOverride : defaultCookieSprite;
            img.raycastTarget = false;
            go.transform.SetParent(parentRT, false);

            // Convert world position to parentRT local point
            Vector3 screen3D = cam3D.WorldToScreenPoint(worldPos);
            // If behind camera, fall back to starting near the icon
            if (screen3D.z < 0)
            {
                screen3D = RectTransformUtility.WorldToScreenPoint(uiCam, icon.position);
            }
            // Clamp to screen bounds to avoid off-screen artifacts
            screen3D.x = Mathf.Clamp(screen3D.x, 0, Screen.width);
            screen3D.y = Mathf.Clamp(screen3D.y, 0, Screen.height);
            Vector2 screen = new Vector2(screen3D.x, screen3D.y);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRT, screen, uiCam, out var localStart);
            rt.anchoredPosition = localStart;
            rt.sizeDelta = icon.sizeDelta;
            rt.localScale = Vector3.one;

            // Target is the icon's anchored position in the same parent space
            var localEnd = icon.anchoredPosition;

            // Animate position and scale
            rt.DOAnchorPos(localEnd, flyDuration).SetEase(flyEase).OnComplete(() =>
            {
                PlayPunch();
                Destroy(go);
            });
            rt.DOScale(0.8f, flyDuration).SetEase(Ease.OutQuad);
        }
    }
}
