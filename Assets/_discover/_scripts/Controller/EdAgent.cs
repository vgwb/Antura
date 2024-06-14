using System;
using DG.Tweening;
using Homer;
using UnityEngine;
using UnityEngine.AI;

namespace Antura.Minigames.DiscoverCountry
{
    public class EdAgent : MonoBehaviour
    {
        [Header("Homer")]
        public HomerActors.Actors ActorId;
        public Action<GameObject> OnInteraction;

        public virtual void Start()
        {
            StartMovement();
        }

        public void OnInteractionWith(GameObject otherGo)
        {
            OnInteraction?.Invoke(otherGo);
            QuestManager.I.OnInteract(ActorId);

            // TESTING
            //if (!aiPaused) LookAt(otherGo.transform, stopWalking:true);
            //else StopLookAt();
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
