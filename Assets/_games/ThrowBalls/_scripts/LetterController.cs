using UnityEngine;
using Antura.LivingLetters;
using System.Collections;
using DG.Tweening;

namespace Antura.Minigames.ThrowBalls
{
    public class LetterController : MonoBehaviour
    {
        public enum PropVariation
        {
            Nothing, Bush, SwervingPileOfCrates, StaticPileOfCrates
        }

        public enum MotionVariation
        {
            Idle, Jumping, Popping
        }

        public const float JUMP_VELOCITY_IMPULSE = 50f;
        public const float GRAVITY = -245f;
        public const float POPPING_OFFSET = 5f;
        public const float PROP_UP_TIME = 0.2f;
        public const float PROP_UP_DELAY = 0.3f;
        private const float OBSTRUCTION_TEST_DEPTH = (LetterSpawner.MAX_Z - LetterSpawner.MIN_Z) * 1.25f;

        public CratePileController cratePileController;
        public BushController bushController;

        private float yEquilibrium;

        private ILivingLetterData letterData;

        private IEnumerator jumpCoroutine;

        public Rigidbody rigidBody;
        public BoxCollider boxCollider;

        public LetterWithPropsController letterWithPropsCntrl;

        private Vector3 lastPosition;
        private Vector3 lastRotation;

        private MotionVariation motionVariation;

        public GameObject shadow;

        [HideInInspector]
        public LivingLetterController letterObjectView;

        public GameObject victoryRays;

        private Vector3 velocity;
        private bool isAirborne;

        void Awake()
        {
            letterObjectView = GetComponent<LivingLetterController>();

            foreach (Collider collider in GetComponentsInChildren<Collider>())
            {
                collider.enabled = false;
            }

            SetIsColliderEnabled(true);
            HideVictoryRays();
        }

        public void SetPropVariation(PropVariation propVariation)
        {
            letterWithPropsCntrl.AccountForProp(propVariation);

            ResetProps();
            DisableProps();

            switch (propVariation)
            {
                case PropVariation.StaticPileOfCrates:
                    cratePileController.Enable();
                    shadow.SetActive(false);
                    break;
                case PropVariation.SwervingPileOfCrates:
                    cratePileController.Enable();
                    cratePileController.SetSwerving();
                    shadow.SetActive(false);
                    break;
                case PropVariation.Bush:
                    bushController.Enable();
                    shadow.SetActive(false);
                    break;
            }
        }

        public void SetMotionVariation(MotionVariation motionVariation)
        {
            ResetMotionVariation();

            this.motionVariation = motionVariation;

            switch (motionVariation)
            {
                case MotionVariation.Idle:
                    break;
                case MotionVariation.Jumping:
                    SetIsJumping();
                    break;
                case MotionVariation.Popping:
                    SetIsPoppingUpAndDown();
                    break;
            }
        }

        public void ResetProps()
        {
            cratePileController.Reset();
            bushController.Reset();
        }

        public void DisableProps()
        {
            cratePileController.Disable();
            bushController.Disable();
        }

        public void ResetMotionVariation()
        {
            StopAllCoroutines();
        }

        public void SetData(ILivingLetterData _data)
        {
            letterData = _data;
            letterObjectView.Init(letterData);
        }

        public ILivingLetterData GetLetter()
        {
            return letterData;
        }

        public void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == Constants.TAG_POKEBALL)
            {
                if (tag == Constants.CORRECT_LETTER_TAG)
                {
                    GameState.instance.OnCorrectLetterHit(this);
                    ThrowBallsConfiguration.Instance.Context.GetAudioManager().PlaySound(Sfx.Poof);
                }
                else
                {
                    if (ThrowBallsGame.instance.GameState.isRoundOngoing)
                    {
                        GameState.instance.OnWrongLetterHit(this);
                        letterObjectView.DoTwirl(null);
                        BallController.instance.OnRebounded();
                    }
                }
            }
        }

        public void MoveTo(float x, float y, float z)
        {
            Vector3 position = transform.position;
            position.x = x;
            position.y = y;
            position.z = z;
            transform.position = position;
        }

        private void SetIsJumping()
        {
            letterObjectView.SetState(LLAnimationStates.LL_still);
            shadow.transform.parent = null;
            jumpCoroutine = Jump();
            StartCoroutine(jumpCoroutine);
        }

        private IEnumerator Jump()
        {
            yield return new WaitForSeconds(Random.Range(0.4f, 1f));

            for (; ; )
            {
                velocity = new Vector3(0f, 30f, 0f);
                letterObjectView.DoSmallJump();
                var position = transform.position;
                yEquilibrium = position.y;
                transform.Translate(velocity * Time.fixedDeltaTime);

                isAirborne = true;

                while (transform.position.y > yEquilibrium)
                {
                    velocity.y += GRAVITY * 0.33f * Time.fixedDeltaTime;
                    transform.Translate(velocity * Time.fixedDeltaTime);
                    yield return new WaitForFixedUpdate();
                }

                position.y = yEquilibrium;
                transform.position = position;

                velocity = Vector3.zero;
                isAirborne = false;

                letterObjectView.SetState(LLAnimationStates.LL_still);

                yield return new WaitForSeconds(1.5f);
            }
        }

        public void StopJumping()
        {
            StopCoroutine(jumpCoroutine);
        }

        public bool IsJumping()
        {
            return motionVariation == MotionVariation.Jumping;
        }

        public void SetIsKinematic(bool isKinematic)
        {
            rigidBody.isKinematic = isKinematic;
        }

        public void SetIsColliderEnabled(bool isEnabled)
        {
            boxCollider.enabled = isEnabled;
        }

        private bool PassesEquilibriumOnNextFrame(float _velocity, float deltaPos, float equilibrium)
        {
            return (_velocity < 0 && transform.position.y + deltaPos < equilibrium)
                    || (_velocity > 0 && transform.position.y + deltaPos > equilibrium);
        }


        private void SetIsPoppingUpAndDown()
        {
            StartCoroutine(PopUpAndDown());
        }

        private IEnumerator PopUpAndDown()
        {
            yield return new WaitForSeconds(Random.Range(0.25f, 1f));

            bool isPoppingUp = true;

            IAudioManager audioManager = ThrowBallsConfiguration.Instance.Context.GetAudioManager();

            for (; ; )
            {
                yEquilibrium = isPoppingUp ? transform.position.y + POPPING_OFFSET : transform.position.y - POPPING_OFFSET;
                float yVelocity = isPoppingUp ? JUMP_VELOCITY_IMPULSE : -JUMP_VELOCITY_IMPULSE;

                float yDelta = 0;

                if (isPoppingUp)
                {
                    audioManager.PlaySound(Sfx.BushRustlingIn);
                }
                else
                {
                    audioManager.PlaySound(Sfx.BushRustlingOut);
                }

                while (!PassesEquilibriumOnNextFrame(yVelocity, yDelta, yEquilibrium))
                {
                    yDelta = yVelocity * Time.fixedDeltaTime;

                    transform.position = new Vector3(transform.position.x, transform.position.y + yDelta, transform.position.z);

                    yield return new WaitForFixedUpdate();
                }

                transform.position = new Vector3(transform.position.x, yEquilibrium, transform.position.z);

                isPoppingUp = !isPoppingUp;

                yield return new WaitForSeconds(0.75f);
            }
        }

        public void Show()
        {
            GameObject poof = Instantiate(ThrowBallsGame.instance.poofPrefab, transform.position, Quaternion.identity);
            Destroy(poof, 10);
            gameObject.SetActive(true);
        }

        public void Vanish()
        {
            GameObject poof = Instantiate(ThrowBallsGame.instance.poofPrefab, transform.position, Quaternion.identity);
            Destroy(poof, 10);
            transform.position = new Vector3(0, 0, -100f);

            ThrowBallsConfiguration.Instance.Context.GetAudioManager().PlaySound(Sfx.Poof);
        }

        public void Enable()
        {
            gameObject.SetActive(true);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }

        public void Reset()
        {
            ResetMotionVariation();
            ResetProps();
            DisableProps();
            yEquilibrium = transform.position.y;
            transform.rotation = Quaternion.Euler(0, 180, 0);
            SetIsKinematic(true);
            SetIsColliderEnabled(true);
            shadow.transform.parent = transform;
            shadow.transform.localPosition = Vector3.zero;
            shadow.SetActive(true);
            letterObjectView.SetState(LLAnimationStates.LL_idle);
            velocity = Vector3.zero;
            isAirborne = false;
        }

        public void ShowVictoryRays()
        {
            victoryRays.SetActive(true);
        }

        public void HideVictoryRays()
        {
            victoryRays.SetActive(false);
        }

        void OnMouseDown()
        {
            if (GameState.instance.isRoundOngoing)
            {
                ThrowBallsConfiguration.Instance.Context.GetAudioManager().PlayVocabularyData(
                    letterData, true, soundType: ThrowBallsConfiguration.Instance.GetVocabularySoundType());
            }
        }

        public bool IsObstructedByOtherLetter()
        {
            Collider[] collidersWithinObstructionTest = Physics.OverlapBox(new Vector3(transform.position.x, transform.position.y, transform.position.z - OBSTRUCTION_TEST_DEPTH / 2), new Vector3(10f, 10f, OBSTRUCTION_TEST_DEPTH / 2));

            foreach (Collider collider in collidersWithinObstructionTest)
            {
                if (collider.gameObject.tag == Constants.WRONG_LETTER_TAG && collider.gameObject != gameObject && collider.transform.position.z < transform.position.z)
                {
                    return true;
                }
            }

            return false;
        }

        public void JumpOffOfCrate()
        {
            StartCoroutine(JumpOffOfCrateCoroutine());
        }

        private IEnumerator JumpOffOfCrateCoroutine()
        {
            if (!isAirborne)
            {
                transform.DORotate(new Vector3(0, 180f, 0), 0.33f);
                letterObjectView.DoTwirl(null);

                rigidBody.isKinematic = true;

                var zAngle = transform.rotation.eulerAngles.z;
                zAngle = zAngle > 180 ? zAngle - 360 : zAngle;

                velocity = new Vector3(zAngle * 0.25f, 25f, 0f);

                isAirborne = true;
            }

            var position = transform.position;

            while (transform.position.y > 0.51f)
            {
                transform.position = position;
                velocity.y += GRAVITY * 0.33f * Time.fixedDeltaTime;
                position += velocity * Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }

            isAirborne = false;

            position.y = 0.51f;
            transform.position = position;

            letterObjectView.SetState(LLAnimationStates.LL_idle);

            shadow.transform.parent = transform;
            shadow.transform.localPosition = Vector3.zero;
            shadow.SetActive(true);
        }
    }
}
