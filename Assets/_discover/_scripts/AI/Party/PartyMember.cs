using UnityEngine;

namespace Antura.Discover
{
    /// <summary>
    /// Tag & tuning component for any entity that can be part of a Party.
    /// - Put this on the Player (as leader) and on each follower (living letters, pets, etc).
    /// - Contains identity data (Id) and movement tuning (speed, accel, stop distance).
    /// - Id is generated once and then locked from Inspector edits (read-only property).
    /// </summary>
    public class PartyMember : MonoBehaviour
    {
        public enum Kind { Player, Pet, LivingLetter, Companion, Other }

        [Header("Identity (generated once, then locked)")]
        [SerializeField, HideInInspector] private string id;   // serialized but hidden; not user-editable
        public string Id => id;                                // public read-only accessor

        [Header("Metadata")]
        public Kind MemberKind = Kind.LivingLetter;

        [Header("Follow Tuning")]
        [Tooltip("Maximum ground speed in units/second (used by follower).")]
        [Range(0f, 20f)] public float MaxSpeed = 4f;

        [Tooltip("Acceleration in units/second^2 (used by follower).")]
        [Range(0f, 20f)] public float Accel = 8f;

        [Tooltip("Acceptable distance to target slot before we consider 'arrived'.")]
        [Range(0f, 2f)] public float StopDistance = 0.25f;

        [Tooltip("Angular speed used to face movement direction (deg/sec).")]
        [Range(0f, 720f)] public float RotationSpeed = 360f;

        // These are runtime-assigned by the PartyManager / PartyFollower
        [HideInInspector] public int SlotIndex = -1;
        [HideInInspector] public Transform Leader;

        /// <summary>Convenience check for the member at slot 0 (the leader).</summary>
        public bool IsLeader => SlotIndex == 0;

        private void OnEnable()
        {
            // Generate a stable ID exactly once on first creation / import.
            if (string.IsNullOrEmpty(id))
            {
                id = System.Guid.NewGuid().ToString();
            }
        }
    }
}
