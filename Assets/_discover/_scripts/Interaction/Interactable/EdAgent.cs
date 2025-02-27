using System;
using DG.Tweening;
// using DG.Tweening;
using Homer;
using UnityEngine;
using UnityEngine.AI;

namespace Antura.Minigames.DiscoverCountry
{
    public class EdAgent : MonoBehaviour
    {
        [Header("AI")]
        public NavMeshAgent NavMeshAgent;

        public Transform[] Waypoints;
        private int sequentialWaypointIndex;
        Tween lookTween;

        public virtual void Start()
        {
            StartMovement();
        }

        void OnDestroy()
        {
            lookTween.Kill();
        }

        public void StartMovement()
        {
            if (Waypoints.Length > 0)
            {
                NavMeshAgent.SetDestination(Waypoints[sequentialWaypointIndex].position);
            }
        }

        public virtual void Update()
        {
            if (Waypoints.Length > 0)
            {
                if (Vector3.Distance(NavMeshAgent.transform.position, Waypoints[sequentialWaypointIndex].position) < 1f)
                {
                    sequentialWaypointIndex++;
                    if (sequentialWaypointIndex == Waypoints.Length)
                        sequentialWaypointIndex = 0;
                    NavMeshAgent.SetDestination(Waypoints[sequentialWaypointIndex].position);
                }
            }

            if (aiPaused)
            {
                // FIXME: this SHOULD NOT be a tween (I'm Daniele so I know :D)
                lookTween.Kill();
                lookTween = transform.DOLookAt(currentTarget.position, 0.5f).Play();
            }
        }

        private bool aiPaused = false;
        private Transform currentTarget;
        public void LookAt(Transform target, bool stopWalking = true)
        {
            if (stopWalking)
            {
                aiPaused = true;
                NavMeshAgent.enabled = false;
            }
            currentTarget = target;
        }

        public void StopLookAt()
        {
            aiPaused = false;
            if (Waypoints.Length > 0)
            {
                NavMeshAgent.enabled = true;
                NavMeshAgent.SetDestination(Waypoints[sequentialWaypointIndex].position);
            }
        }
    }
}
