using UnityEngine;
using System.Collections;

namespace Antura.Discover
{
    public enum CatAnimationStates
    {
        idle,
        walking,
        sitting,
        sleeping,
        sheeping,
        sucking,
        bellyUp,
        digging,
        dancing,
        bitingTail
    }

    public class CatAnimationController : MonoBehaviour
    {
        public const float WALKING_SPEED = 0.0f;
        public const float RUN_SPEED = 1.0f;

        private CatAnimationStates backState = CatAnimationStates.idle;
        private bool hasToGoBackState;
        private CatAnimationStates state = CatAnimationStates.idle;

        public CatAnimationStates State
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
            if (State != CatAnimationStates.idle)
            {
                if (!hasToGoBackState)
                {
                    backState = State;
                }
                State = CatAnimationStates.idle;
                hasToGoBackState = true;
            }

            animator.SetTrigger("doSniff");
        }

        public void DoShout(System.Action onShoutStarted = null)
        {
            onShoutStartedCallback = onShoutStarted;
            animator.SetFloat("random", Random.value);
            if (State == CatAnimationStates.idle)
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
            if (State != CatAnimationStates.idle)
            {
                if (!hasToGoBackState)
                {
                    backState = State;
                }
                State = CatAnimationStates.idle;
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
            if (State != CatAnimationStates.idle &&
                State != CatAnimationStates.walking)
            {
                if (!hasToGoBackState)
                {
                    backState = State;
                }
                State = CatAnimationStates.idle;
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
            animator.SetBool("jumping", true);
            animator.SetBool("falling", false);
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
            State = CatAnimationStates.idle;
            animator.SetTrigger("doCharge");
            this.onChargeEnded = onChargeEnded;
            IsAngry = true;
        }

        public void OnSlipStarted()
        {
#if UNITY_EDITOR
            if (state != CatAnimationStates.walking)
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
            State = CatAnimationStates.walking;
            SetWalkingSpeed(RUN_SPEED);

            if (onChargeEnded != null)
            {
                onChargeEnded();
            }
            onChargeEnded = null;
        }

        private Animator animator_;

        public Animator animator
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

        public void OnShoutStarted()
        {
            if (onShoutStartedCallback != null)
                onShoutStartedCallback();
        }

        public void OnSniffStart()
        {
            if (onSniffStartedCallback != null)
            {
                onSniffStartedCallback();
            }
        }

        public void OnSniffEnd()
        {
            if (onSniffEndedCallback != null)
            {
                onSniffEndedCallback();
            }
        }

        void OnStateChanged(CatAnimationStates oldState, CatAnimationStates newState)
        {
            //            Debug.Log($"CatAnimationController: OnStateChanged from {oldState} to {newState}");
            if (newState != CatAnimationStates.walking)
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
                case CatAnimationStates.idle:
                    animator.SetBool("idle", true);
                    break;
                case CatAnimationStates.walking:
                    animator.SetBool("walking", true);
                    break;
                case CatAnimationStates.sitting:
                    animator.SetBool("sitting", true);
                    break;
                case CatAnimationStates.sleeping:
                    animator.SetBool("sleeping", true);
                    break;
                case CatAnimationStates.sheeping:
                    animator.SetBool("sheeping", true);
                    break;
                case CatAnimationStates.sucking:
                    animator.SetBool("sucking", true);
                    break;
                case CatAnimationStates.bellyUp:
                    animator.SetBool("bellyingUp", true);
                    break;
                case CatAnimationStates.bitingTail:
                    animator.SetBool("bitingTail", true);
                    break;
                case CatAnimationStates.dancing:
                    animator.SetBool("dancing", true);
                    break;
                case CatAnimationStates.digging:
                    animator.SetBool("digging", true);
                    break;
                default:
                    // No specific visual behaviour for this state
                    break;
            }
        }

        public void OnAnimationWalkStart()
        {
            ++walkRefCount;
        }

        public void OnAnimationWalkEnd()
        {
            --walkRefCount;
        }

        public void OnAnimationJumpStart()
        {
            ++jumpRefCount;
        }

        public void OnAnimationJumpEnd()
        {
            --jumpRefCount;
        }

        public void OnAnimationJumpGrab()
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
        public void OnActionCompleted()
        {
            if (hasToGoBackState)
            {
                hasToGoBackState = false;
                State = backState;
            }
        }

    }
}
