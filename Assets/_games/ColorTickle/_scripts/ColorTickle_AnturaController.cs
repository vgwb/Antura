using UnityEngine;
using System;
using Antura.Dog;

namespace Antura.Minigames.ColorTickle
{
    public enum AnturaContollerState
    {
        SLEEPING, REACHINGLETTER, COMINGBACK, TURN_TO_BARK, ROTATION, BARKING
    }

    public class ColorTickle_AnturaController : MonoBehaviour
    {
        #region EXPOSED MEMBERS
        [Header("Movement")]
#pragma warning disable 649
        [SerializeField]
        private float m_fMovementSpeed = 10; //Movement speed
        [SerializeField]
        private float m_fRotationSpeed = 180; //Rotation speed by degree
        [SerializeField]
        private Vector3 m_v3StartPosition;
        [SerializeField]
        private Transform m_oDestination; //placeoholder for the destination
        [SerializeField]
        private Transform m_oTargetToLook;
        [SerializeField]
        private bool m_bMovingToDestination = false; //When true Antura will move towards the setted destination
        [SerializeField]
        private bool m_bRotatingToTarget = false; //When true Antura will rotate towards the setted target point

        [Header("Behaviour")]
        [SerializeField]
        private float m_fBarkTime = 0.5f; //Time spent by barking at the LL

        [SerializeField]
        private AnturaAnimationStates m_eAnimationOnStandby = AnturaAnimationStates.idle; //Animation to execute on reaching destination
        [SerializeField]
        private AnturaAnimationStates m_eAnimationOnMoving = AnturaAnimationStates.idle; //Animation to execute while moving
        [SerializeField]
        private AnturaAnimationStates m_eAnimationOnLLReached = AnturaAnimationStates.idle; //Animation to execute on reaching destination
#pragma warning restore 649
        #endregion

        #region PRIVATE MEMBERS
        private AnturaAnimationController m_oAntura;
        private AnturaContollerState m_eAnturaState = AnturaContollerState.SLEEPING;
        private Vector3 m_v3Destination;
        private float m_fBarkTimeProgress = 0;
        #endregion

        #region EVENTS
        public Action<AnturaContollerState> OnStateChanged;
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

        public Transform targetToLook
        {
            get { return m_oTargetToLook; }
            set { m_oTargetToLook = value; }
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

        public float barkTime
        {
            get { return m_fBarkTime; }
            set { m_fBarkTime = value; }
        }

        public AnturaAnimationStates animationOnMoving
        {
            get { return m_eAnimationOnMoving; }
            set { m_eAnimationOnMoving = value; }
        }

        public AnturaAnimationStates animationOnDestReached
        {
            get { return m_eAnimationOnLLReached; }
            set { m_eAnimationOnLLReached = value; }
        }

        public AnturaAnimationStates animationOnStandby
        {
            get { return m_eAnimationOnStandby; }
            set { m_eAnimationOnStandby = value; }
        }

        public AnturaContollerState anturaState
        {
            get { return m_eAnturaState; }
        }
        #endregion

        #region INTERNALS
        void Awake()
        {
            /*m_Antura = gameObject.GetComponent<Antura>();
            m_StartPosition = gameObject.transform.position;
            m_Antura.SetAnimation(AnturaAnim.SitBreath);*/
            m_oAntura = gameObject.GetComponent<AnturaAnimationController>();
            m_v3StartPosition = m_oAntura.gameObject.transform.position;
            m_eAnturaState = AnturaContollerState.SLEEPING;
            m_fBarkTimeProgress = 0;
            m_v3Destination = m_oDestination.position;

            m_oAntura.State = AnturaAnimationStates.sitting;
            m_oAntura.WalkingSpeed = 0; //walk-0, run-1

            m_eAnturaState = AnturaContollerState.ROTATION;
            m_bRotatingToTarget = true;
        }

        void Update()
        {
            if (m_bMovingToDestination)
            {
                MoveTo(m_v3Destination);
            }

            if (m_bRotatingToTarget)
            {
                RotateTowards(m_oTargetToLook);
            }

            if (m_eAnturaState == AnturaContollerState.BARKING)
            {
                m_fBarkTimeProgress += Time.deltaTime;
                if (m_fBarkTimeProgress >= m_fBarkTime)
                {
                    AnturaNextTransition();
                }
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

        /// <summary>
        /// Using the given probability to launch antura action in the scene.
        /// </summary>
        /// <returns>True when the action has succeed, false otherwise</returns>
        public bool LaunchAnturaDisruption()
        {
            if (m_eAnturaState == AnturaContollerState.SLEEPING) //check for success
            {
                AnturaNextTransition();

                return true;
            }

            return false;
        }

        /// <summary>
        /// Force Antura to interrupt current action and go back.
        /// </summary>
        public void ForceAnturaToGoBack()
        {
            //We do not care about states of Sleeping, RotatingOnStart and ComingBack
            if (m_eAnturaState == AnturaContollerState.SLEEPING ||
                m_eAnturaState == AnturaContollerState.ROTATION ||
                m_eAnturaState == AnturaContollerState.COMINGBACK)
            {
                return;
            }
            // handled cases
            else if (m_eAnturaState == AnturaContollerState.REACHINGLETTER)
            {
                //swap dest and start
                Vector3 _v3Temp = m_v3StartPosition;
                m_v3StartPosition = m_v3Destination;
                m_v3Destination = _v3Temp;

                m_bMovingToDestination = true;

                //set new state
                m_eAnturaState = AnturaContollerState.COMINGBACK;

            }
            else if (m_eAnturaState == AnturaContollerState.TURN_TO_BARK)
            {
                m_bMovingToDestination = true;

                //m_oAntura.SetAnimation(m_eAnimationOnMoving);
                m_oAntura.State = m_eAnimationOnMoving;

                //set new state
                m_eAnturaState = AnturaContollerState.COMINGBACK;
            }
            else if (m_eAnturaState == AnturaContollerState.BARKING)
            {
                //m_oAntura.IsBarking = false;
                //m_oAntura.SetAnimation(m_eAnimationOnMoving);
                m_oAntura.State = m_eAnimationOnMoving;

                m_bMovingToDestination = true;

                //set new state
                m_eAnturaState = AnturaContollerState.COMINGBACK;
            }

            //launch event
            if (OnStateChanged != null)
            {
                OnStateChanged(m_eAnturaState);
            }

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
                gameObject.transform.Translate(_v3MaxMovement, Space.World);

                m_bMovingToDestination = false;

                AnturaNextTransition();

            }
            else //make the progress for this frame
            {

                //m_oAntura.SetAnimation(m_eAnimationOnMoving);

                gameObject.transform.Translate(_v3PartialMovement, Space.World);
                gameObject.transform.rotation = Quaternion.RotateTowards(gameObject.transform.rotation, Quaternion.LookRotation(_v3MaxMovement), m_fRotationSpeed * Time.deltaTime);

            }
        }

        /// <summary>
        /// Rotate the object from the current position to make it look at the target withe the setted speed.
        /// </summary>
        /// <param name="v3Target"></param>
        private void RotateTowards(Transform oTarget)
        {
            Quaternion _qMaxRot = Quaternion.RotateTowards(gameObject.transform.rotation, Quaternion.LookRotation(oTarget.position - transform.position), 180);
            Quaternion _qPartialRot = Quaternion.RotateTowards(gameObject.transform.rotation, Quaternion.LookRotation(oTarget.position - transform.position), m_fRotationSpeed * Time.deltaTime);
            Vector3 _v3Temp;
            float _fMaxAngle = 0;
            float _fPartialAngle = 0;

            _qMaxRot.ToAngleAxis(out _fMaxAngle, out _v3Temp);
            _qPartialRot.ToAngleAxis(out _fPartialAngle, out _v3Temp);

            if (_fPartialAngle >= _fMaxAngle) //if we reached the destination
            {
                //rotate on the destination
                gameObject.transform.rotation = _qMaxRot;

                m_bRotatingToTarget = false;

                AnturaNextTransition();

            }
            else //make the progress for this frame
            {
                //rotate
                gameObject.transform.rotation = _qPartialRot;
            }
        }

        /// <summary>
        /// Progress through the states for Antura: standby->movetoletter->rotationback->bark->movetostandby->rotation->...
        /// </summary>
        private void AnturaNextTransition()
        {
            if (m_eAnturaState == AnturaContollerState.SLEEPING) //go to the letter
            {
                m_bMovingToDestination = true;
                //m_oAntura.SetAnimation(m_eAnimationOnMoving);
                m_oAntura.State = m_eAnimationOnMoving;

                m_eAnturaState = AnturaContollerState.REACHINGLETTER;
            }
            else if (m_eAnturaState == AnturaContollerState.REACHINGLETTER)//letter reached, rotate
            {
                //swap dest and start
                Vector3 _v3Temp = m_v3StartPosition;
                m_v3StartPosition = m_v3Destination;
                m_v3Destination = _v3Temp;

                //rotate towards letter
                m_bRotatingToTarget = true;

                //set new state
                m_eAnturaState = AnturaContollerState.TURN_TO_BARK;

            }
            else if (m_eAnturaState == AnturaContollerState.TURN_TO_BARK)//now bark
            {
                //change animation and play sound
                //m_oAntura.SetAnimation(m_eAnimationOnLLReached);
                //m_oAntura.IsBarking = true;
                m_oAntura.State = m_eAnimationOnLLReached;
                m_oAntura.DoShout();
                ColorTickleConfiguration.Instance.Context.GetAudioManager().PlaySound(Sfx.DogBarking);

                m_fBarkTimeProgress = 0;

                //set new state
                m_eAnturaState = AnturaContollerState.BARKING;

            }
            else if (m_eAnturaState == AnturaContollerState.BARKING) //return back
            {
                m_bMovingToDestination = true;
                //m_oAntura.SetAnimation(m_eAnimationOnMoving);
                //m_oAntura.IsBarking = false;
                m_oAntura.State = m_eAnimationOnMoving;

                m_eAnturaState = AnturaContollerState.COMINGBACK;
            }
            else if (m_eAnturaState == AnturaContollerState.COMINGBACK) //rotate towards letter again
            {
                //swap dest and start
                Vector3 _v3Temp = m_v3StartPosition;
                m_v3StartPosition = m_v3Destination;
                m_v3Destination = _v3Temp;

                //rotate
                m_bRotatingToTarget = true;

                //set new state
                m_eAnturaState = AnturaContollerState.ROTATION;

            }
            else if (m_eAnturaState == AnturaContollerState.ROTATION) //gone back to start
            {
                //change animation
                //m_oAntura.SetAnimation(m_eAnimationOnStandby);
                m_oAntura.State = m_eAnimationOnStandby;

                //set new state
                m_eAnturaState = AnturaContollerState.SLEEPING;

            }

            //launch event
            if (OnStateChanged != null)
            {
                OnStateChanged(m_eAnturaState);
            }
        }
        #endregion
    }
}
