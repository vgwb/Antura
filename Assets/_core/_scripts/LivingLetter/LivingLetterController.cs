using Antura.Core;
using Antura.Language;
using Antura.Helpers;
using Antura.Minigames;
using System.Collections.Generic;
using Antura.Database;
using UnityEngine;

namespace Antura.LivingLetters
{
    /// <summary>
    /// Controller of Living Letter characters. Functions as a view for learning data.
    /// Manages the Living Letter animations and initialises the view content.
    /// </summary>
    public class LivingLetterController : MonoBehaviour
    {
        public const float WALKING_SPEED = 0.0f;
        public const float RUN_SPEED = 1.0f;

        private float idleTimer = 3;

        #region public properties

        [Header("References")]
        public Transform innerTransform;

        public Transform contentTransform;
        public RectTransform textTransform;

        public Transform boneToScaleTransform;

        public UI.TextRender LabelRender;
        public SpriteRenderer ImageSprite;

        private Vector3 startScale;
        private Vector2 startTextScale;

        [Range(1, 2)]
        public float Scale = 1.0f;

        public GameObject[] normalGraphics;
        public GameObject[] limblessGraphics;
        public GameObject poofPrefab;

        public float DancingSpeed = 1;
        private int dancingRefs = 0;

        private LLAnimationStates backState = LLAnimationStates.LL_idle;
        private bool hasToGoBackState;
        private bool inIdleAlternative;
        private bool started;

        private bool outline;
        private bool forceShowAccent;

        #endregion

        #region runtime variables

        /// <summary>
        /// Gets the data.
        /// </summary>
        ILivingLetterData data;

        public ILivingLetterData Data
        {
            get { return data; }
            private set
            {
                data = value;

                if (Data == null)
                {
                    ImageSprite.enabled = false;
                }
                else
                {
                    ImageSprite.enabled = false;
                    LabelRender.enabled = true;
                    LabelRender.SetLetterData(data, outline);
                    // Scale modification
                    switch (data.DataType)
                    {
                        case LivingLetterDataType.Image:
                            Scale = 1f;
                            break;
                        case LivingLetterDataType.Word:
                            Scale = 1.3f;
                            break;
                        case LivingLetterDataType.Phrase:
                            Scale = 2f;
                            break;
                        case LivingLetterDataType.Letter:
                            Scale = 1f;
                            break;
                        default:
                            Scale = 1f;
                            break;
                    }
                }
            }
        }

        public LLAnimationStates State
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

        private LLAnimationStates state = LLAnimationStates.LL_idle;

        Animator animator
        {
            get
            {
                if (!anim)
                {
                    anim = GetComponentInChildren<Animator>();
                }
                return anim;
            }
            set { anim = value; }
        }

        private Animator anim;

        System.Action onTwirlCallback;

        #endregion

        #region Unity events

        void Awake()
        {
            startScale = transform.localScale;
            startTextScale = textTransform.sizeDelta;

            ImageSprite.enabled = false;
        }

        void Start()
        {
            started = true;
        }

        /// <summary>
        /// Initializes object with the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        public void Init(ILivingLetterData data, bool _outline = false)
        {
            idleTimer = Random.Range(3, 8);
            outline = _outline;
            Data = data;
        }

        /// <summary>
        /// Manually initializes object with the specified text.
        /// </summary>
        /// <param name="_data">Used as data reference.</param>
        /// <param name="_customText">The text.</param>
        /// <param name="_scale">The scale used to resize the LL.</param>
        public void Init(ILivingLetterData _data, string _customText, float _scale, bool _outline = false)
        {
            idleTimer = Random.Range(3, 8);
            outline = _outline;
            Data = _data;
            ImageSprite.enabled = false;
            LabelRender.text = _customText;
            Scale = _scale;
        }

        #endregion

        #region events handlers

        void OnStateChanged(LLAnimationStates _oldState, LLAnimationStates _newState)
        {
            //Debug.Log((int)_newState);

            animator.SetBool("walking", false);
            animator.SetBool("dancing", false);
            animator.SetBool("rocketing", false);
            animator.SetBool("dragging", false);
            animator.SetBool("dancing", false);
            animator.SetBool("hanging", false);
            animator.SetBool("tickling", false);
            animator.SetBool("idle", false);

            if (_oldState != LLAnimationStates.LL_limbless && _newState == LLAnimationStates.LL_limbless)
            {
                // going limbless
                if (started)
                {
                    Poof();
                }

                for (int i = 0; i < normalGraphics.Length; ++i)
                {
                    normalGraphics[i].SetActive(false);
                }
                for (int i = 0; i < limblessGraphics.Length; ++i)
                {
                    limblessGraphics[i].SetActive(true);
                }
            }
            else if (_oldState == LLAnimationStates.LL_limbless && _newState != LLAnimationStates.LL_limbless)
            {
                if (started)
                    Poof();

                for (int i = 0; i < normalGraphics.Length; ++i)
                {
                    normalGraphics[i].SetActive(true);
                }
                for (int i = 0; i < limblessGraphics.Length; ++i)
                {
                    limblessGraphics[i].SetActive(false);
                }
            }

            switch (_newState)
            {
                case LLAnimationStates.LL_idle:
                case LLAnimationStates.LL_still:
                    animator.SetBool("idle", true);
                    break;
                case LLAnimationStates.LL_walking:
                    animator.SetBool("walking", true);
                    break;
                case LLAnimationStates.LL_rocketing:
                    animator.SetBool("rocketing", true);
                    break;
                case LLAnimationStates.LL_dancing:
                    animator.SetBool("dancing", true);
                    break;
                case LLAnimationStates.LL_dragging:
                    animator.SetBool("dragging", true);
                    break;
                case LLAnimationStates.LL_hanging:
                    animator.SetBool("hanging", true);
                    break;
                case LLAnimationStates.LL_tickling:
                    animator.SetBool("tickling", true);
                    break;
                default:
                    animator.SetBool("idle", true);
                    break;
            }
        }

        void Update()
        {
            if (State == LLAnimationStates.LL_idle)
            {
                idleTimer -= Time.deltaTime;

                if (idleTimer < 0.0f)
                {
                    idleTimer = Random.Range(3, 8);
                    animator.SetFloat("random", Random.value);
                    animator.SetTrigger("doAlternative");
                }
            }

            float oldSpeed = animator.GetFloat("walkSpeed");

            animator.SetFloat("walkSpeed", Mathf.Lerp(oldSpeed, walkingSpeed, Time.deltaTime * 6.0f));

            if (dancingRefs > 0)
            {
                animator.speed = Mathf.Lerp(animator.speed, DancingSpeed, Time.deltaTime * 10.0f);
            }
            else
            {
                animator.speed = Mathf.Lerp(animator.speed, 1, Time.deltaTime * 10.0f);
            }
        }

        void LateUpdate()
        {
            //if (Scale != lastScale && Scale >= 1.0f)
            {
                if (contentTransform)
                {
                    boneToScaleTransform.localScale = new Vector3(startScale.x, startScale.y, startScale.z * Scale);
                    contentTransform.localScale = new Vector3(1 / Scale, 1, 1);
                    textTransform.sizeDelta = new Vector3(startTextScale.x * Scale, startTextScale.y);
                }
                //lastScale = Scale;
            }
        }

        #endregion

        public void SetState(LLAnimationStates _newState)
        {
            State = _newState;
        }

        public LLAnimationStates GetState()
        {
            return State;
        }

        bool crouch;

        public bool Crouching
        {
            get { return crouch; }
            set
            {
                crouch = value;
                animator.SetBool("crouch", value);
            }
        }

        bool jumping;
        bool falling;

        public bool Falling
        {
            get { return falling; }
            set
            {
                falling = value;
                animator.SetBool("falling", value);
            }
        }

        bool fear;

        public bool HasFear
        {
            get { return fear; }
            set
            {
                fear = value;
                animator.SetBool("fear", value);
            }
        }


        bool hooraying;

        public bool Horraying
        {
            get { return hooraying; }
            set
            {
                animator.SetBool("holdHorray", value);
                if (value)
                {
                    DoHorray();
                }
                hooraying = value;
            }
        }

        /// <summary>
        /// Force a reset to idle
        /// </summary>
        public void Reset()
        {
            animator.SetTrigger("doReset");
            SetState(LLAnimationStates.LL_idle);
        }


        /// <summary>
        /// Speed is 0 (walk) to 1 (running).
        /// </summary>
        float walkingSpeed;

        public void SetWalkingSpeed(float speed = WALKING_SPEED)
        {
            walkingSpeed = speed;
        }


        public void SetDancingSpeed(float speed)
        {
            DancingSpeed = speed;
        }

        public void DoHorray()
        {
            if ((State != LLAnimationStates.LL_still) &&
                (State != LLAnimationStates.LL_idle) &&
                (State != LLAnimationStates.LL_rocketing))
            {
                if (!hasToGoBackState)
                {
                    backState = State;
                }
                SetState(LLAnimationStates.LL_still);
                hasToGoBackState = true;
            }

            if (!hooraying)
            {
                if (inIdleAlternative)
                {
                    animator.SetTrigger("stopAlternative");
                }
                animator.SetTrigger("doHorray");
            }
        }

        public void DoChestStop()
        {
            if ((State != LLAnimationStates.LL_still) &&
                (State != LLAnimationStates.LL_idle))
            {
                if (!hasToGoBackState)
                {
                    backState = State;
                }
                SetState(LLAnimationStates.LL_still);
                hasToGoBackState = true;
            }

            if (inIdleAlternative)
            {
                animator.SetTrigger("stopAlternative");
            }
            animator.SetTrigger("doChestStop");
        }

        public void MarkData(Color color)
        {
            LabelRender.color = color;
        }

        public void MarkLetters(List<ILivingLetterData> toMark, Color color)
        {
            if (!(Data is LL_WordData word))
                return;
            List<StringPart> parts = new List<StringPart>();

            foreach (var markedLetter in toMark)
                parts.AddRange(LanguageSwitcher.LearningHelper.FindLetter(AppManager.I.DB, word.Data, (markedLetter as LL_LetterData).Data, LetterEqualityStrictness.WithVisualForm));

            if (parts.Count > 0)
            {
                LabelRender.text = LanguageSwitcher.LearningHelper.GetWordWithMarkedLettersText(word.Data, parts, color);
            }
        }

        public void DoAngry()
        {
            if ((State != LLAnimationStates.LL_still) &&
                (State != LLAnimationStates.LL_idle))
            {
                if (!hasToGoBackState)
                {
                    backState = State;
                }
                SetState(LLAnimationStates.LL_still);
                hasToGoBackState = true;
            }

            if (inIdleAlternative)
            {
                animator.SetTrigger("stopAlternative");
            }
            animator.SetFloat("random", Random.value);
            animator.SetTrigger("doAngry");
        }

        public void DoHighFive()
        {
            if ((State != LLAnimationStates.LL_still) &&
                (State != LLAnimationStates.LL_idle))
            {
                if (!hasToGoBackState)
                {
                    backState = State;
                }
                SetState(LLAnimationStates.LL_still);
                hasToGoBackState = true;
            }

            if (inIdleAlternative)
            {
                animator.SetTrigger("stopAlternative");
            }
            animator.SetTrigger("doHighFive");
        }

        /// <summary>
        /// Used by SpecialStateEventBehaviour
        /// </summary>
        void OnActionCompleted()
        {
            if (hasToGoBackState)
            {
                hasToGoBackState = false;
                SetState(backState);
            }
        }

        public void DoDancingWin()
        {
            animator.SetTrigger("doDancingWin");
        }

        public void DoDancingLose()
        {
            animator.SetTrigger("doDancingLose");
        }

        /// <summary>
        /// onLetterShowingBack is called when the letter is twirling and it shows you the back;
        /// so you can swap letter in that moment!
        /// </summary>
        public void DoTwirl(System.Action onLetterShowingBack)
        {
            if ((State != LLAnimationStates.LL_still) &&
                (State != LLAnimationStates.LL_idle) &&
                (State != LLAnimationStates.LL_dancing))
            {
                if (!hasToGoBackState)
                {
                    backState = State;
                }
                SetState(LLAnimationStates.LL_still);
                hasToGoBackState = true;
            }

            if (inIdleAlternative)
            {
                animator.SetTrigger("stopAlternative");
            }

            onTwirlCallback = onLetterShowingBack;
            animator.SetTrigger("doTwirl");
        }

        public void ToggleDance()
        {
            State = LLAnimationStates.LL_dancing;
            animator.SetTrigger("toggleDancing");
        }

        public void OnJumpStart()
        {
            if ((State != LLAnimationStates.LL_still) &&
                (State != LLAnimationStates.LL_idle) &&
                (State != LLAnimationStates.LL_walking))
            {
                if (!hasToGoBackState)
                {
                    backState = State;
                }
                SetState(LLAnimationStates.LL_still);
                hasToGoBackState = true;
            }

            if (inIdleAlternative)
            {
                animator.SetTrigger("stopAlternative");
            }
            animator.SetBool("jumping", true);
            animator.SetBool("falling", true);
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
        }

        public void DoSmallJump()
        {
            if ((State != LLAnimationStates.LL_still) &&
                (State != LLAnimationStates.LL_idle))
            {
                if (!hasToGoBackState)
                {
                    backState = State;
                }
                SetState(LLAnimationStates.LL_still);
                hasToGoBackState = true;
            }

            if (inIdleAlternative)
            {
                animator.SetTrigger("stopAlternative");
            }
            animator.SetTrigger("doSmallJump");
        }

        /// <summary>
        /// Produces a poof nearby the LL
        /// </summary>
        public void Poof(float verticalOffset = 0, float scale = 0.75f)
        {
            var puffGo = Instantiate(poofPrefab);
            puffGo.AddComponent<AutoDestroy>().duration = 2;
            puffGo.SetActive(true);
            var position = transform.position + transform.up * 3 + transform.forward * 2;
            position.y += verticalOffset;
            puffGo.transform.position = position;
            puffGo.transform.localScale *= scale;
        }

        void OnTwirlBack()
        {
            if (onTwirlCallback != null)
            {
                onTwirlCallback();
                onTwirlCallback = null;
            }
        }

        public void OnIdleAlternativeEnter()
        {
            inIdleAlternative = true;
        }

        public void OnIdleAlternativeExit()
        {
            inIdleAlternative = false;
        }

        void OnEnable()
        {
            OnStateChanged(state, state);
            started = true;
        }

        void OnDisable()
        {
            started = false;
        }

        void OnDancingStart()
        {
            ++dancingRefs;
        }

        void OnDancingEnd()
        {
            --dancingRefs;
        }

        public void TransformIntoImage()
        {
            Init(new LL_ImageData(data.Id), outline);
        }
    }
}
