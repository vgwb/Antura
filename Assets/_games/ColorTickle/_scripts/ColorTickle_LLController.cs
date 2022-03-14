using UnityEngine;
using System;
using Antura.LivingLetters;

namespace Antura.Minigames.ColorTickle
{
    public class ColorTickle_LLController : MonoBehaviour
    {
        #region EXPOSED MEMBERS
#pragma warning disable 649
        [Header("Movement")]
        [SerializeField]
        private float m_fMovementSpeed = 10; //Movement speed
        [SerializeField]
        private float m_fRotationSpeed = 180; //Rotation speed by degree
        [SerializeField]
        private bool m_bSetStartPosition = false;
        [SerializeField]
        private Vector3 m_v3StartPosition;
        [SerializeField]
        private Transform m_Destination;
        [SerializeField]
        private bool m_bMovingToDestination = false; //When true the letter will move towards the setted destination

        [SerializeField]
        private LLAnimationStates m_eAnimationOnMoving = LLAnimationStates.LL_still; //Animation to execute while moving
        [SerializeField]
        private LLAnimationStates m_eAnimationOnDestReached = LLAnimationStates.LL_still; //Animation to execute on reaching destination
#pragma warning restore 649
        #endregion

        #region PRIVATE MEMBERS
        private LivingLetterController m_oLetter;
        private Vector3 m_v3Destination;
        #endregion

        #region EVENTS
        public Action OnDestinationReached;
        #endregion

        #region GETTER/SETTER
        public Vector3 startPosition
        {
            get { return m_v3StartPosition; }
        }

        public Vector3 destination
        {
            get { return m_v3Destination; }
        }

        public float movementSpeed
        {
            get { return m_fMovementSpeed; }
            set { m_fMovementSpeed = value; }
        }

        public float rotationSpeed
        {
            get { return m_fRotationSpeed; }
            set { m_fRotationSpeed = value; }
        }

        public bool movingToDestination
        {
            get { return m_bMovingToDestination; }
            set { m_bMovingToDestination = value; }
        }

        public LLAnimationStates animationOnMoving
        {
            get { return m_eAnimationOnMoving; }
            set { m_eAnimationOnMoving = value; }
        }

        public LLAnimationStates animationOnDestReached
        {
            get { return m_eAnimationOnDestReached; }
            set { m_eAnimationOnDestReached = value; }
        }

        #endregion

        #region INTERNALS
        void Start()
        {
            m_oLetter = gameObject.GetComponent<LivingLetterController>();
            if (!m_bSetStartPosition)
            {
                m_v3StartPosition = m_oLetter.gameObject.transform.position;
            }
            else
            {
                m_oLetter.gameObject.transform.position = m_v3StartPosition;
            }
            m_v3Destination = m_Destination.position;
        }

        void Update()
        {
            if (m_bMovingToDestination)
            {
                MoveTo(m_v3Destination);
            }

        }
        #endregion

        #region PUBLIC FUNCTIONS
        /// <summary>
        /// Set a new destination and start moving towards it from the current position.
        /// </summary>
        /// <param name="v3Destination">The final world position</param>
        public void MoveToNewDestination(Vector3 v3Destination)
        {
            m_v3Destination = v3Destination;
            m_bMovingToDestination = true;
        }

        /// <summary>
        /// Set a new destination and start position and begin to travel it.
        /// </summary>
        /// <param name="v3Start">The start world position</param>
        /// <param name="v3Destination">The final world position</param>
        public void MoveOnNewPath(Vector3 v3Start, Vector3 v3Destination)
        {
            m_v3StartPosition = v3Start;
            m_v3Destination = v3Destination;
            MoveToNewDestination(m_v3Destination);
        }
        #endregion

        #region PRIVATE FUNCTIONS
        /// <summary>
        /// Move the object from the current position to the final destination withe the setted speed.
        /// </summary>
        /// <param name="v3Destination">The final world position</param>
        private void MoveTo(Vector3 v3Destination)
        {
            Vector3 _v3MaxMovement = v3Destination - gameObject.transform.position;
            Vector3 _v3PartialMovement = _v3MaxMovement.normalized * m_fMovementSpeed * Time.deltaTime;

            if (_v3PartialMovement.sqrMagnitude >= _v3MaxMovement.sqrMagnitude) //if we reached the destination
            {
                //position on the destination
                //gameObject.transform.position = v3Destination;
                gameObject.transform.Translate(_v3MaxMovement, Space.World);
                m_bMovingToDestination = false;

                //change animation and play sound
                m_oLetter.SetWalkingSpeed(0);
                m_oLetter.SetState(m_eAnimationOnDestReached);
                //AudioManager.I.PlayLetter(m_oLetter.Data.Id);

                if (OnDestinationReached != null) //launch event
                {
                    OnDestinationReached();
                }
            }
            else //make the progress for this frame
            {
                m_oLetter.SetWalkingSpeed(1);

                m_oLetter.SetState(m_eAnimationOnMoving);
                gameObject.transform.Translate(_v3PartialMovement, Space.World);

                if (_v3MaxMovement.sqrMagnitude == 0)
                    gameObject.transform.rotation = Quaternion.RotateTowards(gameObject.transform.rotation, Quaternion.LookRotation(Vector3.back), m_fRotationSpeed * Time.deltaTime);
                else
                    gameObject.transform.rotation = Quaternion.RotateTowards(gameObject.transform.rotation, Quaternion.LookRotation(_v3MaxMovement), m_fRotationSpeed * Time.deltaTime);

                //gameObject.transform.position += _v3PartialMovement;
                //m_bMovingToDestination = true;
            }
        }
        #endregion


    }
}
