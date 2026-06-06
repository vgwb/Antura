using UnityEngine;

namespace Antura.Discover.DEV110
{
    /// <summary>
    /// A single projectile in the dodge phase. Travels along its spawn direction (optionally weaving via
    /// <see cref="BulletMovement"/>), checks overlap against the <see cref="FightSoul"/>, applies damage
    /// once, and despawns when it leaves the box or outlives its lifetime. Configured at spawn time by
    /// the wave runner in <see cref="UndertaleFight"/> or by <see cref="ProjectilePatternRunner"/>.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class FightBullet : MonoBehaviour
    {
        [Tooltip("Collision radius of this bullet (canvas units, screen space).")]
        public float HitRadius = 8f;

        // Configured at spawn time by the wave runner.
        [HideInInspector] public float Lifetime = 6f;
        [HideInInspector] public int Damage = 1;
        [HideInInspector] public RectTransform Bounds;
        [HideInInspector] public FightSoul Soul;

        // Motion (analytic, so weaving paths match the editor preview exactly).
        [HideInInspector] public BulletMovement Movement = BulletMovement.Straight;
        [HideInInspector] public float WaveAmplitude;
        [HideInInspector] public float WaveFrequency;

        private RectTransform rect;
        private Vector2 spawnPos;
        private Vector2 dir;
        private float speed;
        private float age;
        private bool consumed;

        private void Awake()
        {
            rect = GetComponent<RectTransform>();
        }

        /// <summary>Legacy straight-line launch: velocity = direction * speed.</summary>
        public void Launch(Vector2 anchoredStart, Vector2 velocity, FightSoul soul, RectTransform bounds, int damage, float lifetime)
        {
            float spd = velocity.magnitude;
            Vector2 d = spd > 0.0001f ? velocity / spd : Vector2.down;
            Launch(anchoredStart, d, spd, soul, bounds, damage, lifetime, BulletMovement.Straight, 0f, 0f);
        }

        /// <summary>Full launch with an optional weave.</summary>
        public void Launch(Vector2 anchoredStart, Vector2 direction, float bulletSpeed, FightSoul soul,
                           RectTransform bounds, int damage, float lifetime,
                           BulletMovement movement, float waveAmplitude, float waveFrequency)
        {
            rect = rect != null ? rect : GetComponent<RectTransform>();
            spawnPos = anchoredStart;
            dir = direction.sqrMagnitude > 0.0001f ? direction.normalized : Vector2.down;
            speed = bulletSpeed;
            Soul = soul;
            Bounds = bounds;
            Damage = damage;
            Lifetime = lifetime;
            Movement = movement;
            WaveAmplitude = waveAmplitude;
            WaveFrequency = waveFrequency;
            age = 0f;
            consumed = false;
            rect.anchoredPosition = spawnPos;
        }

        private void Update()
        {
            age += Time.deltaTime;
            var spawn = new Spawn(spawnPos, dir);
            rect.anchoredPosition = ProjectilePatternMath.PositionAt(spawn, speed, Movement, WaveAmplitude, WaveFrequency, age);

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
