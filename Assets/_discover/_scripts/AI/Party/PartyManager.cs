using System;
using System.Collections.Generic;
using UnityEngine;
using Antura.Utilities;

namespace Antura.Discover
{
    /// <summary>
    /// Owns the Party: leader + followers, current formation, spacing, and slot targets.
    /// - You can place this anywhere (it does NOT need to be a child of the Leader).
    /// - At runtime, it discovers PartyMember components and builds the list (leader must have SlotIndex 0).
    /// - You can Add/Remove members via API.
    /// - It creates one child transform per slot (the "slot targets") under a configurable SlotsRoot and assigns them to PartyFollower components.
    /// - Plug different IFormation implementations to change geometry on the fly.
    ///
    /// Serialization:
    /// - We store member IDs (strings) so you can persist them in your save system later.
    /// - The actual scene references are resolved at runtime (simple sample here).
    /// </summary>
    public class PartyManager : SingletonMonoBehaviour<PartyManager>
    {
        [Header("Setup")]
        [Tooltip("Leader PartyMember (usually the Player with PartyMember.Kind = Player).")]
        public PartyMember Leader;

        [Tooltip("Default formation used on Start.")]
        public string DefaultFormation = "line";

        [Tooltip("Base spacing between followers (meters).")]
        [Range(0.2f, 5f)] public float Spacing = 1.25f;

        [Header("Available Formations")]
        public LineFormation Line = new LineFormation();
        public VFormation V = new VFormation();
        public CircleFormation Circle = new CircleFormation();

        // Internal registry
        private readonly Dictionary<string, IFormation> formations = new Dictionary<string, IFormation>(StringComparer.OrdinalIgnoreCase);

        // Live party list (index 0 == Leader)
        public readonly List<PartyMember> Members = new List<PartyMember>();

        [Header("Slots Root (optional)")]
        [Tooltip("If not assigned, a 'PartySlots' GameObject will be created next to this manager. It will be aligned to the Leader each frame.")]
        public Transform SlotsRoot;

        // Slot targets are child transforms under SlotsRoot (one per party index).
        private readonly List<Transform> slotTargets = new List<Transform>();

        // For a future save system: keep a list of member IDs.
        [Header("Serialization (IDs)")]
        [SerializeField] private List<string> memberIds = new List<string>();

        // Events for UI/audio/FX hooks
        public event Action<PartyMember> OnMemberJoined;
        public event Action<PartyMember> OnMemberLeft;
        public event Action<string> OnFormationChanged;

        private IFormation currentFormation;
        // Reusable buffer to avoid per-frame allocations from formations
        private readonly List<Vector3> offsetsBuffer = new List<Vector3>(16);

        protected override void Init()
        {
            // Register known formations
            formations[Line.Name] = Line;
            formations[V.Name] = V;
            formations[Circle.Name] = Circle;

            // Validate leader
            if (Leader == null)
                Leader = GetComponent<PartyMember>();

            // If we're not on the leader object, try to auto-find a Player kind PartyMember in the scene
            if (Leader == null)
            {
                var all = FindObjectsByType<PartyMember>(FindObjectsSortMode.None);
                for (int i = 0; i < all.Length; i++)
                {
                    if (all[i] != null && all[i].MemberKind == PartyMember.Kind.Player)
                    {
                        Leader = all[i];
                        break;
                    }
                }
            }

            if (Leader == null)
            {
                Debug.LogError("[PartyManager] No Leader assigned and none found on this GameObject.");
                enabled = false;
                return;
            }
        }

        private void Start()
        {
            // Initialize Members list (ensure Leader is slot 0)
            Members.Clear();
            Members.Add(Leader);
            Leader.SlotIndex = 0;
            Leader.Leader = Leader.transform; // leader "leads" itself

            // Optional: auto-find PartyMember followers already in the scene and add them disabled.
            // (We do not auto-join here to keep control explicit.)
            EnsureSlotsRoot();
            BuildSlots(1); // create at least the leader slot target
            SetFormation(DefaultFormation);
            RefreshFollowerTargets();
            RebuildSerializedIds();
        }

        /// <summary>Adds a PartyMember as follower at the end of the list.</summary>
        public void AddMember(PartyMember follower)
        {
            if (follower == null || Members.Contains(follower))
                return;
            if (follower == Leader)
                return; // don't add leader as follower
            Members.Add(follower);
            follower.SlotIndex = Members.Count - 1;
            follower.Leader = Leader.transform;

            // Ensure we have enough slot targets
            BuildSlots(Members.Count);

            RefreshFollowerTargets();
            RebuildSerializedIds();
            OnMemberJoined?.Invoke(follower);
        }

        /// <summary>Remove a member (by reference). Leader cannot be removed here.</summary>
        public void RemoveMember(PartyMember member)
        {
            if (member == null || member == Leader)
                return;
            if (Members.Remove(member))
            {
                member.SlotIndex = -1;
                member.Leader = null;
                // Adjust slot targets to the new size
                BuildSlots(Members.Count);
                RefreshFollowerTargets();
                RebuildSerializedIds();
                OnMemberLeft?.Invoke(member);
            }
        }

        /// <summary>Find and remove by Id (safe for narrative hooks).</summary>
        public void RemoveMemberById(string id)
        {
            var m = Members.Find(x => x != null && x.Id == id);
            if (m != null)
                RemoveMember(m);
        }

        /// <summary>
        /// Remove all followers from the party while keeping the leader (index 0).
        /// Efficient: updates slots and IDs once at the end and fires OnMemberLeft for each.
        /// </summary>
        public void RemoveAllFollowers()
        {
            if (Members.Count <= 1) { BuildSlots(1); RefreshFollowerTargets(); RebuildSerializedIds(); return; }

            // Remove from the end to avoid shifting costs; skip index 0 (leader)
            for (int i = Members.Count - 1; i >= 1; i--)
            {
                var m = Members[i];
                Members.RemoveAt(i);
                if (m != null)
                {
                    m.SlotIndex = -1;
                    m.Leader = null;
                    OnMemberLeft?.Invoke(m);
                }
            }

            // Keep only one slot for the leader and re-target
            BuildSlots(1);
            RefreshFollowerTargets();
            RebuildSerializedIds();
        }

        /// <summary>Switch to a formation by name (case-insensitive).</summary>
        public void SetFormation(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                name = DefaultFormation;
            if (!formations.TryGetValue(name, out var f))
            {
                Debug.LogWarning($"[PartyManager] Formation '{name}' not found. Falling back to 'line'.");
                f = Line;
            }
            currentFormation = f;
            RefreshFollowerTargets();
            OnFormationChanged?.Invoke(currentFormation.Name);
        }

        private void Update()
        {
            // Update slot target local positions from the current formation, every frame.
            if (currentFormation == null)
                return;

            // Align SlotsRoot to Leader so local offsets are applied in leader-space even if root is elsewhere in hierarchy
            if (SlotsRoot == null)
                EnsureSlotsRoot();
            if (Leader != null && SlotsRoot != null)
            {
                SlotsRoot.SetPositionAndRotation(Leader.transform.position, Leader.transform.rotation);
            }

            currentFormation.FillLocalOffsets(offsetsBuffer, Members.Count, Spacing);
            EnsureOffsetsLength(offsetsBuffer, Members.Count);

            for (int i = 0; i < Members.Count; i++)
            {
                var target = slotTargets[i];
                target.localPosition = offsetsBuffer[i];
                target.localRotation = Quaternion.identity;
            }
        }

        /// <summary>Create/ensure required slot target transforms (children) exist.</summary>
        private void BuildSlots(int needed)
        {
            EnsureSlotsRoot();
            while (slotTargets.Count < needed)
            {
                var go = new GameObject($"PartySlot_{slotTargets.Count:00}");
                go.transform.SetParent(SlotsRoot, false);
                // Slot transforms live under SlotsRoot; we align SlotsRoot to Leader each frame.
                slotTargets.Add(go.transform);
            }
            // Trim extras when party shrinks
            for (int i = slotTargets.Count - 1; i >= needed; i--)
            {
                if (slotTargets[i] != null)
                {
#if UNITY_EDITOR
                    if (!Application.isPlaying)
                        DestroyImmediate(slotTargets[i].gameObject);
                    else
                        Destroy(slotTargets[i].gameObject);
#else
                    Destroy(slotTargets[i].gameObject);
#endif
                }
                slotTargets.RemoveAt(i);
            }
        }

        /// <summary>Assign each follower their target Transform and update SlotIndex.</summary>
        private void RefreshFollowerTargets()
        {
            for (int i = 0; i < Members.Count; i++)
            {
                var m = Members[i];
                m.SlotIndex = i;
                m.Leader = Leader.transform;

                // Attach/Sync a PartyFollower brain and give it a slot target (skip leader at index 0)
                if (i == 0)
                    continue;
                var follower = m.GetComponent<PartyFollower>();
                if (follower == null)
                    follower = m.gameObject.AddComponent<PartyFollower>();
                follower.SetTarget(slotTargets[i]);
            }
        }

        /// <summary>
        /// Trigger a staggered hop of followers, like a queue: leader optional, then each follower after a small delay.
        /// Call this from your player jump event.
        /// </summary>
        /// <param name="baseDelay">Delay before the first follower hops.</param>
        /// <param name="stepDelay">Additional delay added per follower index (creates the queue effect).</param>
        /// <param name="includeLeader">If true, leader will hop too (index 0). Otherwise followers only.</param>
        public void TriggerPartyJump(float baseDelay = 0.0f, float stepDelay = 0.08f, bool includeLeader = false)
        {
            for (int i = 0; i < Members.Count; i++)
            {
                var m = Members[i];
                if (m == null)
                    continue;
                if (!includeLeader && i == 0)
                    continue;
                var follower = m.GetComponent<PartyFollower>();
                if (follower == null)
                    continue;
                float d = baseDelay + stepDelay * i;
                follower.TriggerJump(d);
            }
        }

        /// <summary>Keep our serialized ID list in sync with current members order.</summary>
        private void RebuildSerializedIds()
        {
            memberIds.Clear();
            foreach (var m in Members)
                memberIds.Add(m != null ? m.Id : string.Empty);
        }

        private static void EnsureOffsetsLength(List<Vector3> offsets, int count)
        {
            // Defensive: formations should return exactly 'count' offsets.
            if (offsets.Count == count)
                return;
            if (offsets.Count < count)
            {
                while (offsets.Count < count)
                    offsets.Add(Vector3.zero);
            }
            else
            {
                offsets.RemoveRange(count, offsets.Count - count);
            }
        }

        private void EnsureSlotsRoot()
        {
            if (SlotsRoot != null)
                return;
            var go = new GameObject("PartySlots");
            // Create next to this manager in hierarchy so PartyManager can live outside the Leader
            go.transform.SetParent(transform.parent, true);
            go.transform.position = transform.position;
            go.transform.rotation = Quaternion.identity;
            SlotsRoot = go.transform;
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            // Visualize slot positions in the Scene view (editor only).
            if ((Leader == null && SlotsRoot == null) || slotTargets == null)
                return;

            // Draw in the same local space used to position slots
            var basis = SlotsRoot != null ? SlotsRoot : (Leader != null ? Leader.transform : transform);
            Gizmos.matrix = basis.localToWorldMatrix;
            for (int i = 0; i < slotTargets.Count; i++)
            {
                Vector3 p = slotTargets[i].localPosition;
                Gizmos.DrawWireSphere(basis.TransformPoint(p), 0.1f);
#if UNITY_EDITOR
                UnityEditor.Handles.Label(basis.TransformPoint(p), $"Slot {i}");
#endif
            }
        }
#endif
    }
}
