using System;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Discover.DEV110
{
    /// <summary>How an <see cref="Emitter"/> places its bullets and which way they initially travel.</summary>
    public enum EmitterShape
    {
        Aimed,  // fired from the top toward the player's soul (fanned across Spread if Count > 1)
        Fan,    // a spread of Count bullets across SpreadAngle, centred on BaseAngle
        Ring,   // Count bullets evenly around 360 degrees, from the box centre
        Stream, // bullets along one edge moving inward (Edge = Top is the classic "rain")
        Spiral  // like Ring, but rotate BaseAngle by SpinPerBurst each burst for a spiral
    }

    /// <summary>Which edge a <see cref="EmitterShape.Stream"/> spawns along.</summary>
    public enum StreamEdge { Top, Bottom, Left, Right, Random }

    /// <summary>Per-bullet travel style layered on top of the emission direction.</summary>
    public enum BulletMovement
    {
        Straight, // travels in a straight line
        ZigZag,   // sharp triangle-wave weave perpendicular to travel
        Wave      // smooth sine weave perpendicular to travel
    }

    /// <summary>
    /// Named spawn origin for <see cref="EmitterShape.Aimed"/> and <see cref="EmitterShape.Fan"/> emitters.
    /// Ring/Spiral always originate from the centre; Stream uses its own <see cref="StreamEdge"/> field.
    /// </summary>
    public enum SpawnOrigin { Top, Bottom, Left, Right, Center }

    /// <summary>One resolved bullet to spawn: where it starts and the unit direction it travels.</summary>
    public struct Spawn
    {
        public Vector2 Pos;  // anchored position relative to the dodge-box centre
        public Vector2 Dir;  // unit travel direction
        public Spawn(Vector2 pos, Vector2 dir) { Pos = pos; Dir = dir; }
    }

    /// <summary>
    /// One "enemy attack" within a <see cref="ProjectilePattern"/>: a burst (or repeated bursts) of
    /// bullets sharing a shape, timing, appearance and movement. Authored entirely in the inspector /
    /// Projectile Pattern Designer; the maths live in <see cref="ProjectilePatternMath"/> so the editor
    /// preview and the runtime spawner stay identical.
    /// </summary>
    [Serializable]
    public class Emitter
    {
        public string Name = "Emitter";

        [Header("Timing")]
        [Tooltip("Seconds after the pattern starts before this emitter first fires.")]
        public float StartTime = 0f;
        [Tooltip("How many times this emitter fires.")]
        public int Bursts = 1;
        [Tooltip("Seconds between bursts.")]
        public float BurstInterval = 0.4f;

        [Header("Emission")]
        public EmitterShape Shape = EmitterShape.Aimed;
        [Tooltip("Bullets fired per burst.")]
        public int Count = 1;
        [Tooltip("Fan: total arc in degrees the bullets spread across.")]
        public float SpreadAngle = 60f;
        [Tooltip("Facing in degrees. 0 = down, 90 = right, 180 = up, 270 = left (clockwise).")]
        public float BaseAngle = 0f;
        [Tooltip("Stream: which edge bullets spawn along and travel inward from.")]
        public StreamEdge Edge = StreamEdge.Top;
        [Tooltip("Degrees BaseAngle rotates each burst. Drives Spiral; works on any shape.")]
        public float SpinPerBurst = 0f;

        [Header("Movement")]
        public BulletMovement Movement = BulletMovement.Straight;
        [Tooltip("ZigZag / Wave: sideways reach in pixels.")]
        public float WaveAmplitude = 24f;
        [Tooltip("ZigZag / Wave: weaves per second.")]
        public float WaveFrequency = 3f;

        [Header("Spawn")]
        [Tooltip("Edge / point bullets originate from (Aimed and Fan only). Ring/Spiral always use the box centre; Stream uses its Edge field.")]
        public SpawnOrigin Origin = SpawnOrigin.Top;
        [Tooltip("Seconds before each burst a warning indicator should be shown at the origin. 0 = disabled. (Placeholder — runtime warning not yet implemented.)")]
        public float WarnSeconds = 0f;

        [Header("Appearance")]
        [Tooltip("Leave empty for a plain white square; assign a PNG/Sprite to use art instead.")]
        public Sprite Sprite;
        public Color Color = Color.white;
        [Tooltip("Bullet size in pixels.")]
        public float Size = 16f;
        [Tooltip("Travel speed in pixels per second.")]
        public float Speed = 200f;
        [Tooltip("HP removed from the player on a hit.")]
        public int Damage = 1;
    }

    /// <summary>
    /// A reusable enemy projectile pattern: a list of <see cref="Emitter"/>s on a shared timeline.
    /// Authored with the Projectile Pattern Designer (Tools ▸ Antura ▸ Projectile Pattern Designer),
    /// then assigned to a <c>DodgeWave.Pattern</c> on an <see cref="UndertaleFight"/>.
    /// </summary>
    [CreateAssetMenu(menuName = "AnturaUndertale/Projectile Pattern", fileName = "NewProjectilePattern")]
    public class ProjectilePattern : ScriptableObject
    {
        [Tooltip("Reference dodge-box size used by the preview. Match your in-game DodgeBox for exact WYSIWYG. " +
                 "Spawn positions derive from the actual box at runtime, so a pattern still fits any box.")]
        public Vector2 ReferenceBoxSize = new Vector2(480f, 270f);

        public List<Emitter> Emitters = new List<Emitter>();

        /// <summary>Rough seconds for one play-through: last burst + time for a bullet to cross the box.</summary>
        public float EstimateDuration()
        {
            float lastBurst = 0f;
            float minSpeed = float.MaxValue;
            foreach (var e in Emitters)
            {
                if (e == null) continue;
                lastBurst = Mathf.Max(lastBurst, e.StartTime + Mathf.Max(0, e.Bursts - 1) * Mathf.Max(0f, e.BurstInterval));
                minSpeed = Mathf.Min(minSpeed, Mathf.Max(1f, e.Speed));
            }
            if (minSpeed == float.MaxValue) minSpeed = 200f;
            float cross = ReferenceBoxSize.magnitude / minSpeed;
            return Mathf.Max(0.5f, lastBurst + cross);
        }
    }

    /// <summary>
    /// Pure, engine-light maths shared by the runtime spawner (<see cref="ProjectilePatternRunner"/>)
    /// and the editor preview (ProjectilePatternDesigner). Deterministic, so the preview matches the
    /// real fight exactly.
    /// </summary>
    public static class ProjectilePatternMath
    {
        private const float TwoPi = Mathf.PI * 2f;

        /// <summary>
        /// Converts a <see cref="SpawnOrigin"/> to an anchored position relative to the box centre.
        /// Use this in emitter cases that need a configurable spawn edge.
        /// </summary>
        public static Vector2 GetOriginPos(SpawnOrigin origin, Vector2 half)
        {
            switch (origin)
            {
                case SpawnOrigin.Bottom: return new Vector2(0f, -half.y);
                case SpawnOrigin.Left:   return new Vector2(-half.x, 0f);
                case SpawnOrigin.Right:  return new Vector2(half.x, 0f);
                case SpawnOrigin.Center: return Vector2.zero;
                default:                 return new Vector2(0f, half.y); // Top
            }
        }

        /// <summary>Angle (degrees) to a unit direction. 0 = down, 90 = right, 180 = up, 270 = left.</summary>
        public static Vector2 AngleToDir(float deg)
        {
            float r = deg * Mathf.Deg2Rad;
            return new Vector2(Mathf.Sin(r), -Mathf.Cos(r));
        }

        /// <summary>Sideways offset (pixels) for a bullet's weave at a given age.</summary>
        public static float LateralOffset(BulletMovement movement, float amplitude, float frequency, float age)
        {
            switch (movement)
            {
                case BulletMovement.Wave:
                    return amplitude * Mathf.Sin(TwoPi * frequency * age);
                case BulletMovement.ZigZag:
                    // Triangle wave from a sine: starts at 0, range [-1, 1].
                    return amplitude * (2f / Mathf.PI) * Mathf.Asin(Mathf.Sin(TwoPi * frequency * age));
                default:
                    return 0f;
            }
        }

        /// <summary>Bullet position at <paramref name="age"/> seconds after it spawned.</summary>
        public static Vector2 PositionAt(Spawn spawn, float speed, BulletMovement movement,
                                         float amplitude, float frequency, float age)
        {
            Vector2 perp = new Vector2(-spawn.Dir.y, spawn.Dir.x);
            return spawn.Pos
                 + spawn.Dir * (speed * age)
                 + perp * LateralOffset(movement, amplitude, frequency, age);
        }

        /// <summary>
        /// Resolve the bullets a single burst fires into <paramref name="results"/>. Deterministic for a
        /// given (emitterIndex, burstIndex) so the editor preview never jitters.
        /// </summary>
        public static void GetBurst(Emitter e, int emitterIndex, int burstIndex,
                                    Vector2 half, Vector2 soulPos, List<Spawn> results)
        {
            results.Clear();
            if (e == null) return;
            int count = Mathf.Max(1, e.Count);
            float baseAngle = e.BaseAngle + e.SpinPerBurst * burstIndex;

            switch (e.Shape)
            {
                case EmitterShape.Aimed:
                {
                    Vector2 origin = GetOriginPos(e.Origin, half);
                    Vector2 aim = (soulPos - origin);
                    aim = aim.sqrMagnitude > 0.0001f ? aim.normalized : Vector2.down;
                    for (int i = 0; i < count; i++)
                    {
                        float off = count == 1 ? 0f : Mathf.Lerp(-e.SpreadAngle * 0.5f, e.SpreadAngle * 0.5f, i / (float)(count - 1));
                        results.Add(new Spawn(origin, Rotate(aim, off)));
                    }
                    break;
                }
                case EmitterShape.Fan:
                {
                    Vector2 origin = GetOriginPos(e.Origin, half);
                    for (int i = 0; i < count; i++)
                    {
                        float off = count == 1 ? 0f : Mathf.Lerp(-e.SpreadAngle * 0.5f, e.SpreadAngle * 0.5f, i / (float)(count - 1));
                        results.Add(new Spawn(origin, AngleToDir(baseAngle + off)));
                    }
                    break;
                }
                case EmitterShape.Ring:
                case EmitterShape.Spiral:
                {
                    for (int i = 0; i < count; i++)
                    {
                        float ang = baseAngle + i * (360f / count);
                        results.Add(new Spawn(Vector2.zero, AngleToDir(ang)));
                    }
                    break;
                }
                case EmitterShape.Stream:
                {
                    for (int i = 0; i < count; i++)
                    {
                        StreamEdge edge = e.Edge;
                        if (edge == StreamEdge.Random)
                        {
                            int r = (int)(Rand01(Seed(emitterIndex, burstIndex, i * 31 + 7)) * 4f);
                            edge = (StreamEdge)Mathf.Clamp(r, 0, 3);
                        }
                        results.Add(MakeStreamSpawn(edge, half, Rand01(Seed(emitterIndex, burstIndex, i))));
                    }
                    break;
                }
            }
        }

        private static Spawn MakeStreamSpawn(StreamEdge edge, Vector2 half, float t)
        {
            switch (edge)
            {
                case StreamEdge.Bottom: return new Spawn(new Vector2(Mathf.Lerp(-half.x, half.x, t), -half.y), Vector2.up);
                case StreamEdge.Left:   return new Spawn(new Vector2(-half.x, Mathf.Lerp(-half.y, half.y, t)), Vector2.right);
                case StreamEdge.Right:  return new Spawn(new Vector2(half.x, Mathf.Lerp(-half.y, half.y, t)), Vector2.left);
                default:                return new Spawn(new Vector2(Mathf.Lerp(-half.x, half.x, t), half.y), Vector2.down); // Top
            }
        }

        private static Vector2 Rotate(Vector2 v, float deg)
        {
            float r = deg * Mathf.Deg2Rad;
            float c = Mathf.Cos(r), s = Mathf.Sin(r);
            return new Vector2(v.x * c - v.y * s, v.x * s + v.y * c);
        }

        private static int Seed(int emitterIndex, int burstIndex, int bulletIndex)
            => emitterIndex * 73856093 ^ (burstIndex + 1) * 19349663 ^ (bulletIndex + 1) * 83492791;

        /// <summary>Deterministic unit random in [0, 1) from an integer seed (PCG-style hash).</summary>
        private static float Rand01(int seed)
        {
            uint x = (uint)seed * 747796405u + 2891336453u;
            x = ((x >> (int)((x >> 28) + 4u)) ^ x) * 277803737u;
            x = (x >> 22) ^ x;
            return (x & 0xFFFFFFu) / (float)0x1000000;
        }
    }
}
