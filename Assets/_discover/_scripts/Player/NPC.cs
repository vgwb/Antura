using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

namespace Antura.Minigames.DiscoverCountry
{
    public class NPC : MonoBehaviour
    {
        public virtual void Start()
        {
            StartMovement();
        }

        [Header("AI")]
        public NavMeshAgent NavMeshAgent;

        public Transform[] Waypoints;
        private int sequentialWaypointIndex;

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
                transform.DOLookAt(currentTarget.position, 0.5f).Play();
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
            NavMeshAgent.enabled = true;
            if (Waypoints.Length > 0)
                NavMeshAgent.SetDestination(Waypoints[sequentialWaypointIndex].position);
        }
    }
}
