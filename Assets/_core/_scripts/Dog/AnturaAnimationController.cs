using UnityEngine;

namespace Antura.Dog
{
    /// <summary>
    /// Handles the animations of Antura.
    /// </summary>
    // TODO conventions: requires renaming of variables
    public class AnturaAnimationController : MonoBehaviour
    {
        public GameObject RewardUI;
        public GameObject AnimatUI;

        public const float WALKING_SPEED = 0.0f;
        public const float RUN_SPEED = 1.0f;

        private AnturaAnimationStates backState = AnturaAnimationStates.idle;
        private bool hasToGoBackState;
        private AnturaAnimationStates state = AnturaAnimationStates.idle;

        public AnturaAnimationStates State
        {
            get { return state; }
            set
            {
                if (state != value)
                {
                    var oldState = state;
                    state = value;
                    OnStateChanged(oldState, state);
                }
                hasToGoBackState = false;
            }
        }

        public bool IsJumping { get; private set; }

        private System.Action onChargeEnded;
        private System.Action onGrabbed;

        private float walkingSpeed;

        public float WalkingSpeed
        {
            get { return walkingSpeed; }
            set { walkingSpeed = value; }
        }

        private bool isAngry;

        public bool IsAngry
        {
            get { return isAngry; }
            set
            {
                isAngry = value;
                animator.SetBool("angry", value);
            }
        }


        private bool isExcited;

        public bool IsExcited
        {
            get { return isExcited; }
            set
            {
                isExcited = value;
                animator.SetBool("excited", value);
            }
        }

        private bool isSad;

        public bool IsSad
        {
            get { return isSad; }
            set
            {
                isSad = value;
                animator.SetBool("sad", value);
            }
        }

        // Check if animation is actually moving legs
        private int walkRefCount;

        private int jumpRefCount;

        public bool IsAnimationActuallyWalking
        {
            get { return walkRefCount > 0; }
        }

        public bool IsAnimationActuallyJumping
        {
            get { return jumpRefCount > 0; }
        }

        private System.Action onSniffStartedCallback;
        private System.Action onSniffEndedCallback;

        private System.Action onShoutStartedCallback;

        public void SetWalkingSpeed(float speed = WALKING_SPEED)
        {
            walkingSpeed = speed;
        }

        public void DoSniff(System.Action onSniffEnded = null, System.Action onSniffStarted = null)
        {
            onSniffEndedCallback = onSniffEnded;
            onSniffStartedCallback = onSniffStarted;
            if (State != AnturaAnimationStates.idle)
            {
                if (!hasToGoBackState)
                {
                    backState = State;
                }
                State = AnturaAnimationStates.idle;
                hasToGoBackState = true;
            }

            animator.SetTrigger("doSniff");
        }

        public void DoShout(System.Action onShoutStarted = null)
        {
            onShoutStartedCallback = onShoutStarted;
            animator.SetFloat("random", Random.value);
            if (State == AnturaAnimationStates.idle)
            {
                animator.SetTrigger("doShout");
            }
            else
            {
                animator.SetTrigger("doShoutAdditive");
            }
        }

        public void DoBurp()
        {
            animator.SetTrigger("doBurp");
        }

        public void DoBite(System.Action onGrabbed = null)
        {
            this.onGrabbed = onGrabbed;
            animator.SetTrigger("doBite");
        }

        public void DoSpit(bool openMouth)
        {
            if (State != AnturaAnimationStates.idle)
            {
                if (!hasToGoBackState)
                {
                    backState = State;
                }
                State = AnturaAnimationStates.idle;
                hasToGoBackState = true;
            }

            if (openMouth)
            {
                animator.SetTrigger("doSpitOpen");
            }
            else
            {
                animator.SetTrigger("doSpitClosed");
            }
        }

        public void DoSmallJumpAndGrab(System.Action onGrabbed = null)
        {
            this.onGrabbed = onGrabbed;
            animator.SetTrigger("doJumpAndGrab");
        }

        public void OnJumpStart()
        {
            if (State != AnturaAnimationStates.idle &&
                State != AnturaAnimationStates.walking)
            {
                if (!hasToGoBackState)
                {
                    backState = State;
                }
                State = AnturaAnimationStates.idle;
                hasToGoBackState = true;
            }

            animator.SetBool("jumping", true);
            animator.SetBool("falling", true);
            IsJumping = true;
        }

        // when Antura grabs something in the air
        public void OnJumpGrab()
        {
            animator.SetTrigger("doAirGrab");
        }

        public void OnJumpMaximumHeightReached()
        {
            animator.SetBool("jumping", false);
            animator.SetBool("falling", true);
        }

        public void OnJumpEnded()
        {
            animator.SetBool("jumping", false);
            animator.SetBool("falling", false);
            IsJumping = false;
        }

        /// <summary>
        /// Do an angry charge. The Dog makes an angry charging animation (it must stay in the same position during this animation);
        /// IsAngry is set to true automatically (needed to use the angry run).
        /// After such animation ends, onChargeEnded will be called to inform you, and passes automatically into running state.
        /// You should use onChargeEnded to understand when you should begin to move the antura's transform.
        /// </summary>
        public void DoCharge(System.Action onChargeEnded)
        {
            State = AnturaAnimationStates.idle;
            animator.SetTrigger("doCharge");
            this.onChargeEnded = onChargeEnded;
            IsAngry = true;
        }

        public void OnSlipStarted()
        {
#if UNITY_EDITOR
            if (state != AnturaAnimationStates.walking)
            { Debug.LogError("You should call on slip started/ended only in walking state"); }
#endif
            animator.SetBool("slipping", true);
        }

        public void OnSlipEnded()
        {
            animator.SetBool("slipping", false);
        }

        void OnCharged()
        {
            State = AnturaAnimationStates.walking;
            SetWalkingSpeed(RUN_SPEED);

            if (onChargeEnded != null)
            {
                onChargeEnded();
            }
            onChargeEnded = null;
        }

        private Animator animator_;

        Animator animator
        {
            get
            {
                if (!animator_)
                {
                    animator_ = GetComponentInChildren<Animator>();
                }
                return animator_;
            }
        }

        void Update()
        {
            float oldSpeed = animator.GetFloat("walkSpeed");

            animator.SetFloat("walkSpeed", Mathf.Lerp(oldSpeed, walkingSpeed, Time.deltaTime * 4.0f));
        }

        void OnShoutStarted()
        {
            if (onShoutStartedCallback != null)
                onShoutStartedCallback();
        }

        void OnSniffStart()
        {
            if (onSniffStartedCallback != null)
            {
                onSniffStartedCallback();
            }
        }

        void OnSniffEnd()
        {
            if (onSniffEndedCallback != null)
            {
                onSniffEndedCallback();
            }
        }

        void OnStateChanged(AnturaAnimationStates oldState, AnturaAnimationStates newState)
        {
            if (newState != AnturaAnimationStates.walking)
            {
                animator.SetBool("slipping", false);
            }

            animator.SetBool("idle", false);
            animator.SetBool("walking", false);
            animator.SetBool("sitting", false);
            animator.SetBool("sleeping", false);
            animator.SetBool("sheeping", false);
            animator.SetBool("sucking", false);

            animator.SetBool("bellyingUp", false);
            animator.SetBool("digging", false);
            animator.SetBool("dancing", false);
            animator.SetBool("bitingTail", false);

            switch (newState)
            {
                case AnturaAnimationStates.idle:
                    animator.SetBool("idle", true);
                    break;
                case AnturaAnimationStates.walking:
                    animator.SetBool("walking", true);
                    break;
                case AnturaAnimationStates.sitting:
                    animator.SetBool("sitting", true);
                    break;
                case AnturaAnimationStates.sleeping:
                    animator.SetBool("sleeping", true);
                    break;
                case AnturaAnimationStates.sheeping:
                    animator.SetBool("sheeping", true);
                    break;
                case AnturaAnimationStates.sucking:
                    animator.SetBool("sucking", true);
                    break;
                case AnturaAnimationStates.bellyUp:
                    animator.SetBool("bellyingUp", true);
                    break;
                case AnturaAnimationStates.bitingTail:
                    animator.SetBool("bitingTail", true);
                    break;
                case AnturaAnimationStates.dancing:
                    animator.SetBool("dancing", true);
                    break;
                case AnturaAnimationStates.digging:
                    animator.SetBool("digging", true);
                    break;
                default:
                    // No specific visual behaviour for this state
                    break;
            }
        }

        void OnAnimationWalkStart()
        {
            ++walkRefCount;
        }

        void OnAnimationWalkEnd()
        {
            --walkRefCount;
        }

        void OnAnimationJumpStart()
        {
            ++jumpRefCount;
        }

        void OnAnimationJumpEnd()
        {
            --jumpRefCount;
        }

        void OnAnimationJumpGrab()
        {
            if (onGrabbed != null)
            {
                onGrabbed();
            }
        }

        void OnEnable()
        {
            OnStateChanged(state, state);
        }

        /// <summary>
        /// Used by SpecialStateEventBehaviour
        /// </summary>
        void OnActionCompleted()
        {
            if (hasToGoBackState)
            {
                hasToGoBackState = false;
                State = backState;
            }
        }

        public void ChangeTo(int target)
        {
            switch (target)
            {
                case 1:
                    RewardUI.SetActive(false);
                    AnimatUI.SetActive(true);
                    break;
                case 2:
                    AnimatUI.SetActive(false);
                    RewardUI.SetActive(true);
                    break;
            }
        }
    }
}
