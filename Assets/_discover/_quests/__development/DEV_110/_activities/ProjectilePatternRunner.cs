using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.Discover.DEV110
{
    /// <summary>
    /// Plays a <see cref="ProjectilePattern"/> at runtime: walks the emitters' shared timeline and
    /// spawns <see cref="FightBullet"/>s through the same maths the editor preview uses
    /// (<see cref="ProjectilePatternMath"/>), so the fight matches what was authored.
    ///
    /// Bullets are built from scratch (no prefab needed): a UI <see cref="Image"/> with no sprite draws
    /// a plain white square; assign <see cref="Emitter.Sprite"/> to use a PNG instead.
    /// </summary>
    public static class ProjectilePatternRunner
    {
        private struct BurstEvent
        {
            public float Time;
            public int EmitterIndex;
            public int BurstIndex;
        }

        /// <summary>
        /// Run <paramref name="pattern"/> inside <paramref name="box"/>. Spawned bullets are parented to
        /// <paramref name="container"/> and reported through <paramref name="onSpawned"/> (so the caller
        /// can track / clear them). Stops early when <paramref name="keepRunning"/> returns false (e.g.
        /// the player died). Completes once every burst has fired and its bullets have cleared.
        /// </summary>
        public static IEnumerator Play(ProjectilePattern pattern, RectTransform box, RectTransform container,
                                       FightSoul soul, Action<FightBullet> onSpawned, Func<bool> keepRunning)
        {
            if (pattern == null || box == null)
                yield break;
            if (container == null)
                container = box;

            // Build and time-order the burst schedule.
            var events = new List<BurstEvent>();
            for (int ei = 0; ei < pattern.Emitters.Count; ei++)
            {
                var e = pattern.Emitters[ei];
                if (e == null) continue;
                int bursts = Mathf.Max(1, e.Bursts);
                for (int bi = 0; bi < bursts; bi++)
                {
                    events.Add(new BurstEvent
                    {
                        Time = Mathf.Max(0f, e.StartTime) + bi * Mathf.Max(0f, e.BurstInterval),
                        EmitterIndex = ei,
                        BurstIndex = bi
                    });
                }
            }
            events.Sort((a, b) => a.Time.CompareTo(b.Time));

            var live = new List<FightBullet>();
            var burst = new List<Spawn>();

            // Fire the schedule.
            float elapsed = 0f;
            int next = 0;
            while (next < events.Count)
            {
                if (keepRunning != null && !keepRunning())
                    yield break;

                elapsed += Time.deltaTime;
                while (next < events.Count && events[next].Time <= elapsed)
                {
                    var ev = events[next++];
                    FireBurst(pattern.Emitters[ev.EmitterIndex], ev.EmitterIndex, ev.BurstIndex,
                              box, container, soul, burst, live, onSpawned);
                }
                yield return null;
            }

            // Let the last bullets play out (capped so a stray bullet can never hang the fight).
            float cap = 5f;
            while (cap > 0f)
            {
                if (keepRunning != null && !keepRunning())
                    yield break;
                live.RemoveAll(b => b == null);
                if (live.Count == 0)
                    break;
                cap -= Time.deltaTime;
                yield return null;
            }
        }

        private static void FireBurst(Emitter e, int emitterIndex, int burstIndex,
                                      RectTransform box, RectTransform container, FightSoul soul,
                                      List<Spawn> burst, List<FightBullet> live, Action<FightBullet> onSpawned)
        {
            Vector2 half = box.rect.size * 0.5f;
            Vector2 soulPos = soul != null && soul.Rect != null ? soul.Rect.anchoredPosition : Vector2.zero;
            ProjectilePatternMath.GetBurst(e, emitterIndex, burstIndex, half, soulPos, burst);

            float lifetime = box.rect.size.magnitude / Mathf.Max(1f, e.Speed) + 1.5f;

            foreach (var s in burst)
            {
                var bullet = BuildBullet(e, container);
                bullet.Launch(s.Pos, s.Dir, e.Speed, soul, box, e.Damage, lifetime,
                              e.Movement, e.WaveAmplitude, e.WaveFrequency);
                live.Add(bullet);
                onSpawned?.Invoke(bullet);
            }
        }

        private static FightBullet BuildBullet(Emitter e, RectTransform container)
        {
            var go = new GameObject("PatternBullet", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(FightBullet));
            var rect = (RectTransform)go.transform;
            rect.SetParent(container, false);
            rect.anchorMin = rect.anchorMax = rect.pivot = new Vector2(0.5f, 0.5f);
            rect.sizeDelta = new Vector2(e.Size, e.Size);

            var img = go.GetComponent<Image>();
            img.sprite = e.Sprite;     // null => Unity draws a plain white square
            img.color = e.Color;
            img.raycastTarget = false;

            var fb = go.GetComponent<FightBullet>();
            fb.HitRadius = e.Size * 0.5f;
            return fb;
        }
    }
}
