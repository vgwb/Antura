using System;
using UnityEngine;

namespace Antura.Discover.DEV110
{
    /// <summary>
    /// The player "soul / heart" used during the bullet-hell dodge phase.
    /// Moves with the old Input axes (Horizontal/Vertical, i.e. arrows + WASD) clamped inside
    /// the dodge box, and exposes hit-testing helpers for <see cref="FightBullet"/>.
    /// Must be a child of the dodge box RectTransform (anchoredPosition is relative to the box centre).
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class FightSoul : MonoBehaviour
    {
        [Tooltip("Movement speed in canvas units per second.")]
        public float Speed = 260f;

        [Tooltip("Dodge box the soul is constrained to. Defaults to the parent RectTransform.")]
        public RectTransform Bounds;

        [Tooltip("Inner margin kept from the box edges so the soul stays fully visible.")]
        public float EdgeMargin = 6f;

        [Tooltip("Soul collision radius (canvas units, screen space).")]
        public float HitRadius = 10f;

        [Tooltip("Invulnerability window after taking a hit, in seconds.")]
        public float InvulnTime = 0.6f;

        /// <summary>Raised when a bullet connects. Argument is the damage amount.</summary>
        public event Action<int> OnHit;

        public RectTransform Rect { get; private set; }
        public bool IsVulnerable => invulnTimer <= 0f;
        public Vector3 WorldCenter => Rect != null ? Rect.position : transform.position;

        private float invulnTimer;
        private CanvasGroup flashGroup;

        private void Awake()
        {
            Rect = GetComponent<RectTransform>();
            if (Bounds == null && transform.parent != null)
                Bounds = transform.parent as RectTransform;
            flashGroup = GetComponent<CanvasGroup>();
        }

        private void OnEnable()
        {
            invulnTimer = 0f;
            if (flashGroup != null)
                flashGroup.alpha = 1f;
        }

        private void Update()
        {
            if (invulnTimer > 0f)
            {
                invulnTimer -= Time.deltaTime;
                if (flashGroup != null)
                    flashGroup.alpha = Mathf.PingPong(Time.unscaledTime * 8f, 1f) * 0.6f + 0.4f;
                if (invulnTimer <= 0f && flashGroup != null)
                    flashGroup.alpha = 1f;
            }

            Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if (input.sqrMagnitude > 1f)
                input.Normalize();

            Vector2 pos = Rect.anchoredPosition + input * Speed * Time.deltaTime;
            Rect.anchoredPosition = ClampToBounds(pos);
        }

        private Vector2 ClampToBounds(Vector2 pos)
        {
            if (Bounds == null)
                return pos;
            Vector2 half = Bounds.rect.size * 0.5f - Vector2.one * (EdgeMargin + HitRadius);
            half.x = Mathf.Max(0f, half.x);
            half.y = Mathf.Max(0f, half.y);
            pos.x = Mathf.Clamp(pos.x, -half.x, half.x);
            pos.y = Mathf.Clamp(pos.y, -half.y, half.y);
            return pos;
        }

        /// <summary>Called by a bullet on contact. Respects the invulnerability window.</summary>
        public bool Hit(int damage)
        {
            if (!IsVulnerable)
                return false;
            invulnTimer = InvulnTime;
            OnHit?.Invoke(damage);
            return true;
        }

        /// <summary>Recenter the soul, e.g. at the start of a dodge wave.</summary>
        public void ResetToCenter()
        {
            if (Rect != null)
                Rect.anchoredPosition = Vector2.zero;
            invulnTimer = 0f;
        }
    }
}
