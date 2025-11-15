using Antura;
using Antura.Audio;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Antura.Discover.Activities
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class CanvasBug : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler
    {
        [Header("Runtime wiring")]
        public RectTransform BoundsParent; // Area to move within
        public float Speed = 120f;         // px/sec
        public Vector2 ScaleRange = new Vector2(0.8f, 1.2f);
        public System.Func<float> SpeedMultiplierProvider; // Optional external multiplier (e.g., based on progress)

        private RectTransform _rt;
        private Vector2 _dir;
        private float _minX, _maxX, _minY, _maxY;
        private System.Action _onTouched;
        private bool _alreadyTriggered;
        private float _lastBounceSfxTime;
        private Camera _uiCam;

        void Awake()
        {
            _rt = GetComponent<RectTransform>();
            var img = GetComponent<Image>();
            img.raycastTarget = true;

            // Ensure collider matches rect size
            var box = GetComponent<BoxCollider2D>();
            box.isTrigger = true;
            box.size = _rt.rect.size;

            // Cache UI camera for screen point conversions
            var canvas = GetComponentInParent<Canvas>();
            if (canvas != null)
            {
                _uiCam = canvas.rootCanvas != null ? canvas.rootCanvas.worldCamera : canvas.worldCamera;
            }

            // Subtle UI shadow for depth
            var shadow = GetComponent<Shadow>();
            if (shadow == null)
            {
                shadow = gameObject.AddComponent<Shadow>();
                shadow.effectColor = new Color(0f, 0f, 0f, 0.35f);
                shadow.effectDistance = new Vector2(2f, -2f);
                shadow.useGraphicAlpha = true;
            }
        }

        public void Init(RectTransform boundsParent, float speed, Vector2? startPos, Vector2? startDir, Vector2 scaleRange, System.Action onTouched)
        {
            BoundsParent = boundsParent;
            Speed = speed;
            ScaleRange = scaleRange;
            _onTouched = onTouched;

            // Random scale
            float scl = Random.Range(ScaleRange.x, ScaleRange.y);
            _rt.localScale = Vector3.one * scl;

            // Compute inner bounds with margin equal to half-size to keep sprite fully visible
            var parentRect = BoundsParent.rect;
            float marginX = _rt.rect.width * 0.5f;
            float marginY = _rt.rect.height * 0.5f;
            _minX = parentRect.xMin + marginX;
            _maxX = parentRect.xMax - marginX;
            _minY = parentRect.yMin + marginY;
            _maxY = parentRect.yMax - marginY;

            // Position
            Vector2 pos = startPos ?? new Vector2(
                Random.Range(_minX, _maxX),
                Random.Range(_minY, _maxY));
            _rt.anchoredPosition = pos;

            // Direction
            if (startDir.HasValue && startDir.Value.sqrMagnitude > 0.001f)
                _dir = startDir.Value.normalized;
            else
                _dir = Random.insideUnitCircle.normalized;

            // Small initial wiggle for bouncy feel
            _rt.DOKill();
            _rt.DOPunchScale(Vector3.one * 0.08f, 0.35f, 6, 0.9f).SetUpdate(true);
        }

        void Update()
        {
            if (BoundsParent == null)
                return;

            float dt = Time.unscaledDeltaTime;
            float mul = SpeedMultiplierProvider != null ? Mathf.Max(0.1f, SpeedMultiplierProvider()) : 1f;
            Vector2 pos = _rt.anchoredPosition + _dir * (Speed * mul * dt);

            bool bounced = false;
            if (pos.x < _minX)
            {
                pos.x = _minX;
                _dir.x = -_dir.x;
                bounced = true;
            }
            else if (pos.x > _maxX)
            {
                pos.x = _maxX;
                _dir.x = -_dir.x;
                bounced = true;
            }
            if (pos.y < _minY)
            {
                pos.y = _minY;
                _dir.y = -_dir.y;
                bounced = true;
            }
            else if (pos.y > _maxY)
            {
                pos.y = _maxY;
                _dir.y = -_dir.y;
                bounced = true;
            }

            _rt.anchoredPosition = pos;

            // Face movement direction (z rotation for UI)
            if (_dir.sqrMagnitude > 0.0001f)
            {
                float angle = Mathf.Atan2(_dir.y, _dir.x) * Mathf.Rad2Deg - 90f; // make sprite "up" face forward
                var e = _rt.localEulerAngles;
                e.z = Mathf.LerpAngle(e.z, angle, 10f * dt);
                _rt.localEulerAngles = e;
            }

            // Tiny bounce punch when reflecting to sell the impact
            if (bounced)
            {
                // Impact punch and optional bounce SFX with cooldown
                _rt.DOKill(false);
                _rt.DOPunchPosition(_dir * 6f, 0.15f, 6, 1f).SetUpdate(true);
                if (Time.unscaledTime - _lastBounceSfxTime > 0.3f)
                {
                    _lastBounceSfxTime = Time.unscaledTime;
                    // Occasionally a harder hit
                    if (Random.value < 0.50f)
                        AudioManager.I?.PlaySound(Sfx.DogBarking);
                    else
                        AudioManager.I?.PlaySound(Sfx.DogBarking);
                }
            }

            // While pressing/dragging, trigger if pointer moves over the bug
            if (!_alreadyTriggered)
            {
                bool pressed = Input.GetMouseButton(0) || Input.touchCount > 0;
                if (pressed)
                {
                    if (Input.touchCount > 0)
                    {
                        for (int i = 0; i < Input.touchCount; i++)
                        {
                            var t = Input.GetTouch(i);
                            if (t.phase == TouchPhase.Moved || t.phase == TouchPhase.Stationary || t.phase == TouchPhase.Began)
                            {
                                if (RectTransformUtility.RectangleContainsScreenPoint(_rt, t.position, _uiCam))
                                {
                                    TriggerTouched();
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (RectTransformUtility.RectangleContainsScreenPoint(_rt, (Vector2)Input.mousePosition, _uiCam))
                        {
                            TriggerTouched();
                        }
                    }
                }
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            TriggerTouched();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            // If user is dragging across, treat as touch
            if (eventData != null && (eventData.dragging || eventData.eligibleForClick))
            {
                TriggerTouched();
            }
        }

        private void TriggerTouched()
        {
            if (_alreadyTriggered)
                return;
            _alreadyTriggered = true;
            AudioManager.I?.PlaySound(Sfx.Splat);
            _onTouched?.Invoke();
        }

        void OnRectTransformDimensionsChange()
        {
            // Keep collider in sync with graphic size changes
            var box = GetComponent<BoxCollider2D>();
            if (box != null && _rt != null)
                box.size = _rt.rect.size;
        }
    }
}
