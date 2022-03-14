using Antura.Dog;
using System.Collections;
using UnityEngine;

namespace Antura.Minigames.DancingDots
{
    public enum AnturaContollerState
    {
        LEFTHOME, RIGHTHOME, DANCING, WALKING, FINISHINGDANICING
    }

    public class DancingDotsAnturaController : MonoBehaviour
    {
        private bool movingToDestination = false; //When true Antura will move towards the setted destination
        private float movementSpeed = 10; //Movement speed
        private AnturaAnimationController antura;
        private bool rotatingToTarget;
        private Vector3 destination;

        private Transform targetToLookAt;
        private AnturaContollerState status;

        private float danceDuration;

        public Transform leftHome;
        public Transform rightHome;
        public Transform[] dancingSpots;

        void Awake()
        {
            antura = gameObject.GetComponent<AnturaAnimationController>();

            antura.State = AnturaAnimationStates.sitting;
            antura.WalkingSpeed = 0; //walk-0, run-1
            status = AnturaContollerState.LEFTHOME;

            StartCoroutine(co_AnturaNextDesicion());

        }

        private void MoveToNewDestination(Transform dest)
        {
            destination = dest.position;
            gameObject.transform.rotation = dest.rotation;
            movingToDestination = true;
            status = AnturaContollerState.WALKING;
        }

        private void MoveTo(Vector3 dest)
        {
            Vector3 maxMovement = dest - gameObject.transform.position;
            Vector3 partialMovement = maxMovement.normalized * movementSpeed * Time.deltaTime;

            if (partialMovement.sqrMagnitude >= maxMovement.sqrMagnitude) //if we reached the destination
            {
                //position on the destination
                gameObject.transform.Translate(maxMovement, Space.World);

                movingToDestination = false;

                if (leftHome.position == dest)
                {
                    status = AnturaContollerState.LEFTHOME;
                }
                else
                {
                    status = AnturaContollerState.RIGHTHOME;
                }

                StartCoroutine(co_AnturaNextDesicion());

            }
            else //make the progress for this frame
            {
                gameObject.transform.Translate(partialMovement, Space.World);
            }
        }

        IEnumerator co_AnturaNextDesicion()
        {
            switch (status)
            {
                case AnturaContollerState.LEFTHOME:
                    yield return new WaitForSeconds(UnityEngine.Random.Range(1, 5));
                    MoveToNewDestination(rightHome);
                    antura.State = AnturaAnimationStates.walking;
                    status = AnturaContollerState.WALKING;

                    break;
                case AnturaContollerState.RIGHTHOME:
                    yield return new WaitForSeconds(UnityEngine.Random.Range(1, 5));
                    MoveToNewDestination(leftHome);
                    antura.State = AnturaAnimationStates.walking;
                    status = AnturaContollerState.WALKING;
                    break;
            }

        }

        void OnTriggerEnter(Collider other)
        {
            //Debug.Log("DD Antura ontrigger");
            if (other.tag == "Marker")
            {
                danceDuration = 3.5f * UnityEngine.Random.Range(1, 3);
                status = AnturaContollerState.DANCING;
                antura.State = AnturaAnimationStates.dancing;
                movingToDestination = false;
            }
        }

        private float finishDance;

        void Update()
        {

            if (movingToDestination)
            {
                MoveTo(destination);
            }
            else if (status == AnturaContollerState.DANCING)
            {
                danceDuration -= Time.deltaTime;
                if (danceDuration < 0)
                {
                    status = AnturaContollerState.FINISHINGDANICING;
                    antura.State = AnturaAnimationStates.walking;
                    finishDance = 1f;
                }
            }
            else if (status == AnturaContollerState.FINISHINGDANICING)
            {
                finishDance -= Time.deltaTime;
                if (finishDance < 0)
                {
                    status = AnturaContollerState.WALKING;
                    movingToDestination = true;
                }
            }
        }

        void RestartMoving()
        {
        }
    }
}
