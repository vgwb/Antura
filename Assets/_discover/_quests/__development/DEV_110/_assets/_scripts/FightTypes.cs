using System;
using UnityEngine;

namespace Antura.Discover.DEV110
{
    /// <summary>
    /// How a <see cref="DodgeWave"/> places newly spawned bullets around the dodge box.
    /// </summary>
    public enum SpawnShape
    {
        RandomEdges, // spawn on a random edge, moving across the box
        TopDown,     // spawn along the top, falling down (Undertale "rain")
        Sides        // spawn on left/right edges, moving horizontally
    }

    /// <summary>
    /// One entry in the ACT menu. The actual dialogue / branching / "is this the correct act"
    /// logic lives in the referenced Yarn node, not here (content is Yarn-driven).
    /// </summary>
    [Serializable]
    public class ActOption
    {
        [Tooltip("Label shown on the ACT button. If a localization key is preferred, handle it in the Yarn node instead.")]
        public string Label = "ACT";

        [Tooltip("Yarn node started when this option is chosen. That node drives the dialogue, may <<activity ...>>, <<fight_dodge ...>>, <<fight_spare_enable>> and must end with <<fight_menu>> or <<fight_end ...>>.")]
        public string YarnNode = "";

        [Tooltip("If true, this option disappears from the ACT menu after being used once.")]
        public bool DisabledAfterUse = false;

        [NonSerialized] public bool Used;
    }

    /// <summary>
    /// A bullet-hell "enemy attack" wave, run by <c>&lt;&lt;fight_dodge "Id"&gt;&gt;</c>.
    /// Tune everything in the inspector; the script owns the mechanics.
    /// </summary>
    [Serializable]
    public class DodgeWave
    {
        [Tooltip("Identifier referenced from Yarn: <<fight_dodge \"Id\">>")]
        public string Id = "wave1";

        [Tooltip("Bullet prefab (must have a FightBullet + RectTransform). Spawned as a child of the dodge box.")]
        public FightBullet BulletPrefab;

        [Tooltip("Bullets spawned per spawn tick.")]
        public int BulletsPerSpawn = 1;

        [Tooltip("Seconds between spawn ticks.")]
        public float SpawnInterval = 0.45f;

        [Tooltip("Bullet travel speed in canvas units per second.")]
        public float Speed = 220f;

        [Tooltip("How long the wave keeps spawning (seconds). After this, it waits for live bullets to clear.")]
        public float Duration = 5f;

        [Tooltip("HP removed from the player on a hit.")]
        public int DamagePerHit = 1;

        [Tooltip("Where bullets spawn / which direction they travel.")]
        public SpawnShape Shape = SpawnShape.RandomEdges;
    }
}
