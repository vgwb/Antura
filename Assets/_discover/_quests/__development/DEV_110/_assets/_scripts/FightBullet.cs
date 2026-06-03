using UnityEngine;

namespace Antura.Discover.DEV110
{
    /// <summary>
    /// A single projectile in the dodge phase. Moves by an anchored-space velocity, checks overlap
    /// against the <see cref="FightSoul"/>, applies damage once, and despawns when it leaves the box
    /// or outlives its lifetime. Configured by the wave runner in <see cref="UndertaleFight"/>.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class FightBullet : MonoBehaviour
    {
        [Tooltip("Collision radius of this bullet (canvas units, screen space).")]
        public float HitRadius = 8f;

        // Configured at spawn time by the wave runner.
        [HideInInspector] public Vector2 Velocity;   // anchored units / sec
        [HideInInspector] public float Lifetime = 6f;
        [HideInInspector] public int Damage = 1;
        [HideInInspector] public RectTransform Bounds;
        [HideInInspector] public FightSoul Soul;

        private RectTransform rect;
        private float age;
        private bool consumed;

        private void Awake()
        {
            rect = GetComponent<RectTransform>();
        }

        public void Launch(Vector2 anchoredStart, Vector2 velocity, FightSoul soul, RectTransform bounds, int damage, float lifetime)
        {
            rect = rect != null ? rect : GetComponent<RectTransform>();
            rect.anchoredPosition = anchoredStart;
            Velocity = velocity;
            Soul = soul;
            Bounds = bounds;
            Damage = damage;
            Lifetime = lifetime;
            age = 0f;
            consumed = false;
        }

        private void Update()
        {
            age += Time.deltaTime;
            rect.anchoredPosition += Velocity * Time.deltaTime;

            if (!consumed && Soul != null && Soul.isActiveAndEnabled)
            {
                float dist = Vector2.Distance(rect.position, Soul.WorldCenter);
                if (dist <= HitRadius + Soul.HitRadius)
                {
                    if (Soul.Hit(Damage))
                    {
                        consumed = true;
                        Despawn();
                        return;
                    }
                }
            }

            if (age >= Lifetime || IsOutOfBounds())
                Despawn();
        }

        private bool IsOutOfBounds()
        {
            if (Bounds == null)
                return false;
            Vector2 half = Bounds.rect.size * 0.5f + Vector2.one * (HitRadius + 24f);
            Vector2 p = rect.anchoredPosition;
            return Mathf.Abs(p.x) > half.x || Mathf.Abs(p.y) > half.y;
        }

        private void Despawn()
        {
            Destroy(gameObject);
        }
    }
}
