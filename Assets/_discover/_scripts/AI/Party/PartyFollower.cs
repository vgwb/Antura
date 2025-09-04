using UnityEngine;
using UnityEngine.AI;

namespace Antura.Discover
{
    /// <summary>
    /// Movement brain for any non-leader PartyMember.
    /// - If a NavMeshAgent is present and enabled, we drive it.
    /// - Otherwise we do a lightweight smooth-follow with simple steering and rotation.
    /// - The PartyManager assigns our target transform each frame via SetTarget().
    /// </summary>
    [RequireComponent(typeof(PartyMember))]
    public class PartyFollower : MonoBehaviour
    {
        [Header("Runtime (debug)")]
        [SerializeField] private Transform target;  // assigned by PartyManager; where this follower should go

        private PartyMember member;
        private NavMeshAgent agent;
        private float nextAgentUpdate;
        [SerializeField, Range(0.02f, 0.5f)] private float agentUpdateInterval = 0.08f;

        [Header("Jump/Hop")]
        [Tooltip("Allow follower to perform a short hop when triggered by the PartyManager.")]
        public bool CanJump = true;
        [Range(0.05f, 2f)] public float JumpHeight = 0.6f;
        [Range(0.1f, 1.5f)] public float JumpDuration = 0.35f;
        [Range(0f, 2f)] public float JumpCooldown = 0.2f;
        private bool jumping;
        private float lastJumpEndTime;

        private void Awake()
        {
            member = GetComponent<PartyMember>();
            agent = GetComponent<NavMeshAgent>();

            // If there is an Agent, set some sane defaults to respect PartyMember tuning.
            if (agent != null)
            {
                agent.acceleration = Mathf.Max(0.1f, member.Accel);
                agent.speed = Mathf.Max(0.1f, member.MaxSpeed);
                agent.angularSpeed = Mathf.Max(1f, member.RotationSpeed);
                agent.stoppingDistance = member.StopDistance;
                agent.autoBraking = true;
                agent.updateRotation = true;
                agent.updateUpAxis = true;
            }
        }

        /// <summary>Assigned by PartyManager. May be null (if detached).</summary>
        public void SetTarget(Transform t) => target = t;

        /// <summary>Request a hop with an optional delay. Ignores if leader, disabled, or on cooldown.</summary>
        public void TriggerJump(float delay = 0f)
        {
            if (!CanJump)
                return;
            if (member == null)
                member = GetComponent<PartyMember>();
            if (member != null && member.IsLeader)
                return;
            if (jumping || (Time.time < lastJumpEndTime + JumpCooldown))
                return;
            StartCoroutine(JumpCO(delay));
        }

        private void Update()
        {
            // Leaders do not follow a slot; they are the slot zero.
            if (member.IsLeader || target == null)
                return;

            if (agent != null && agent.enabled && agent.isOnNavMesh)
            {
                // NavMesh pathing (throttled)
                agent.speed = member.MaxSpeed;
                agent.acceleration = member.Accel;
                agent.stoppingDistance = member.StopDistance;
                if (Time.time >= nextAgentUpdate)
                {
                    agent.SetDestination(target.position);
                    nextAgentUpdate = Time.time + agentUpdateInterval;
                }
                return;
            }

            // Lightweight fallback movement (no NavMeshAgent)
            Vector3 toTarget = target.position - transform.position;
            toTarget.y = 0f;
            float dist = toTarget.magnitude;

            if (dist > member.StopDistance)
            {
                // Move towards target with a simple accel-limited step
                float step = Mathf.MoveTowards(0f, member.MaxSpeed, member.Accel * Time.deltaTime);
                Vector3 dir = toTarget.normalized;
                transform.position += dir * step * Time.deltaTime;

                // Rotate to face movement direction
                if (dir.sqrMagnitude > 0.0001f)
                {
                    Quaternion look = Quaternion.LookRotation(dir, Vector3.up);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, look, member.RotationSpeed * Time.deltaTime);
                }
            }
        }

        private System.Collections.IEnumerator JumpCO(float delay)
        {
            if (delay > 0f)
                yield return new WaitForSeconds(delay);
            jumping = true;

            float dur = Mathf.Max(0.01f, JumpDuration);
            float t = 0f;

            if (agent != null && agent.enabled && agent.isOnNavMesh)
            {
                float start = agent.baseOffset;
                while (t < dur)
                {
                    t += Time.deltaTime;
                    float r = Mathf.Clamp01(t / dur);
                    // Smooth hop arc: sin(pi*r)
                    float y = Mathf.Sin(Mathf.PI * r) * JumpHeight;
                    agent.baseOffset = start + y;
                    yield return null;
                }
                agent.baseOffset = start;
            }
            else
            {
                var pos = transform.position;
                float baseY = pos.y;
                while (t < dur)
                {
                    t += Time.deltaTime;
                    float r = Mathf.Clamp01(t / dur);
                    float y = Mathf.Sin(Mathf.PI * r) * JumpHeight;
                    var p = transform.position;
                    transform.position = new Vector3(p.x, baseY + y, p.z);
                    yield return null;
                }
                var pEnd = transform.position;
                transform.position = new Vector3(pEnd.x, baseY, pEnd.z);
            }

            jumping = false;
            lastJumpEndTime = Time.time;
        }
    }
}
