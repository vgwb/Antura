using Antura.Dog;
using UnityEngine;
using DG.Tweening;

namespace Antura.Minigames.ThrowBalls
{
    public class AnturaController : MonoBehaviour
    {
        public static AnturaController instance;

        private const float RUNNING_SPEED = 25f;
        private const float JUMP_INIT_VELOCITY = 20f;
        private const float JUMP_THRESHOLD = 12f;

        private Vector3 velocity;
        private Vector3 ballOffset;
        private AnturaAnimationController animator;
        private bool ballGrabbed;
        private bool jumped;

        private AnturaModelManager modelManager;
        private Rigidbody rigidBody;

        private enum State
        {
            Running, Jumping, Falling
        }

        private State state;

        void Awake()
        {
            instance = this;

            modelManager = GetComponent<AnturaModelManager>();
            rigidBody = GetComponent<Rigidbody>();
        }

        void Start()
        {
            animator = GetComponent<AnturaAnimationController>();

            state = State.Running;
            animator.State = AnturaAnimationStates.walking;
        }

        public void EnterScene()
        {
            Vector3 ballPosition = BallController.instance.transform.position;
            Vector3 anturaPosition = ballPosition;
            anturaPosition.y = GroundController.instance.transform.position.y;

            float frustumHeight = 2.0f * Mathf.Abs(anturaPosition.z - Camera.main.transform.position.z) * Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad);
            float frustumWidth = frustumHeight * Camera.main.aspect;

            anturaPosition.x = frustumWidth / 2 + 2f;

            if (Random.value <= 0.5f)
            {
                anturaPosition.x *= -1;
            }

            velocity.x = Mathf.Sign(ballPosition.x - anturaPosition.x) * RUNNING_SPEED;

            if (velocity.x > 0)
            {
                transform.rotation = Quaternion.Euler(0, 270, 0);
            }

            else
            {
                transform.rotation = Quaternion.Euler(0, 90, 0);
            }

            transform.position = anturaPosition;

            ballGrabbed = false;
            jumped = false;

            state = State.Running;

            ThrowBallsConfiguration.Instance.Context.GetAudioManager().PlaySound(Sfx.DogBarking);
        }

        public void DoneChasing()
        {
            CapsuleCollider capsuleCollider = GetComponent<CapsuleCollider>();
            capsuleCollider.center = new Vector3(0, 8, -3);

            rigidBody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
            rigidBody.useGravity = false;
        }

        void FixedUpdate()
        {
            switch (state)
            {
                case State.Running:
                    if (IsOffScreen() && jumped && velocity.x * transform.position.x > 0)
                    {
                        if (ballGrabbed)
                        {
                            ballGrabbed = false;
                            GameState.instance.OnBallLost();
                            BallController.instance.Reset();
                        }

                        Disable();
                    }

                    else if (!jumped && Mathf.Abs(BallController.instance.transform.position.x - transform.position.x) < JUMP_THRESHOLD)
                    {
                        jumped = true;
                        state = State.Jumping;
                        velocity.y = JUMP_INIT_VELOCITY;
                        animator.OnJumpStart();
                    }

                    break;
                case State.Jumping:
                    velocity.y += Constants.GRAVITY.y * Time.fixedDeltaTime;

                    if (velocity.y < 0)
                    {
                        state = State.Falling;
                    }

                    break;
                case State.Falling:
                    velocity.y += Constants.GRAVITY.y * Time.fixedDeltaTime;
                    break;
            }

            transform.Translate(velocity * Time.fixedDeltaTime, Space.World);

            if (ballGrabbed)
            {
                BallController.instance.transform.position = modelManager.Dog_jaw.position + ballOffset;
            }
        }

        public void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == Constants.TAG_POKEBALL)
            {
                if (BallController.instance.IsIdle() && !ballGrabbed)
                {
                    animator.OnJumpGrab();
                    BallController.instance.OnIntercepted();
                    ballOffset = new Vector3(Mathf.Sign(velocity.x) * 4f, 0f, 0f);
                    ballGrabbed = true;
                    Catapult.instance.DisableCollider();
                    BallController.instance.transform.DOPunchScale(-Vector3.one * 0.5f, 0.75f, 10, 0.1f);
                }
            }

            else if (collision.gameObject.tag == "Ground" && state == State.Falling)
            {
                state = State.Running;

                animator.OnJumpEnded();
                animator.State = AnturaAnimationStates.walking;

                velocity.y = 0f;
            }
        }

        private bool IsOffScreen()
        {
            float frustumHeight = 2.0f * Mathf.Abs(transform.position.z - Camera.main.transform.position.z) * Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad);
            float frustumWidth = frustumHeight * Camera.main.aspect;
            float halfFrustumWidth = frustumWidth / 2;

            if (Mathf.Abs(transform.position.x) - 12f > halfFrustumWidth)
            {
                return true;
            }

            else
            {
                return false;
            }
        }

        public void Reset()
        {
            velocity = Vector3.zero;
            rigidBody.velocity = Vector3.zero;
            rigidBody.angularVelocity = Vector3.zero;
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            ballGrabbed = false;
            jumped = false;

            animator.State = AnturaAnimationStates.walking;
            animator.SetWalkingSpeed(1f);

            state = State.Running;
        }

        public void Enable()
        {
            gameObject.SetActive(true);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }
    }
}
