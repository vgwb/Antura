using UnityEngine;

namespace Antura.Minigames.ThrowBalls
{
    public class BallController : MonoBehaviour
    {
        public Vector3 INITIAL_BALL_POSITION = new Vector3(0, 5.25f, -20f);
        private readonly Vector3 REBOUND_DESTINATION = new Vector3(0, 15f, -30f);
        public const float BALL_RESPAWN_TIME = 3f;
        public const float REBOUND_TIME = 0.2f;
        public const float SCREEN_HANG_TIME = 0.5f;
        public const float DROP_TIME = 0.5f;
        public const float TIME_TO_IDLE = 6f;

        public static BallController instance;

        public Rigidbody rigidBody;

        public GameObject shadow;
        private SphereCollider sphereCollider;

        private TrailRenderer trailRenderer;

        private IAudioManager audioManager;

        private enum State
        {
            Chased, Anchored, Dragging, Launched, Intercepted, Rebounding, Hanging, Dropping, Idle
        }

        private State state;
        private float stateTime;

        private float cameraDistance;

        void Awake()
        {
            instance = this;

            rigidBody.maxAngularVelocity = 100;

            sphereCollider = GetComponent<SphereCollider>();

            trailRenderer = GetComponent<TrailRenderer>();
            trailRenderer.Clear();

            audioManager = ThrowBallsConfiguration.Instance.Context.GetAudioManager();
        }

        void Start()
        {
            cameraDistance = Mathf.Abs(Camera.main.transform.position.z - transform.position.z);

            cameraDistance = 26;
            INITIAL_BALL_POSITION.y = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height / 3, cameraDistance)).y;

            //Reset();

            SetState(State.Chased);

            ArrowBodyController.instance.Disable();
            ArrowHeadController.instance.Disable();
        }

        public bool IsDragging()
        {
            return state == State.Dragging;
        }

        public void CancelDragging()
        {
            if (IsDragging())
            {
                SetState(State.Anchored);
            }
        }

        private void OnMouseDown()
        {
            OnBallTugged();
        }

        public void OnBallTugged()
        {
            if (!IsLaunched())
            {
                SetState(State.Dragging);
            }
        }

        public void OnCollisionEnter(Collision collision)
        {
            if (state != State.Chased)
            {
                audioManager.PlaySound(Sfx.BallHit);
            }
        }

        public void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.tag == "Bush")
            {
                stateTime += 2f;
            }
        }

        public void Launch(Vector3 forceToApply)
        {
            rigidBody.isKinematic = false;
            rigidBody.AddForceAtPosition(forceToApply, transform.position + Vector3.up * 0.5f, ForceMode.VelocityChange);
            SetState(State.Launched);
        }

        public void Reset()
        {
            transform.position = INITIAL_BALL_POSITION;

            rigidBody.isKinematic = true;
            rigidBody.angularVelocity = new Vector3(0, 0, 0);
            rigidBody.velocity = new Vector3(0, 0, 0);
            rigidBody.isKinematic = false;
            rigidBody.useGravity = false;
            transform.rotation = Quaternion.Euler(new Vector3(-90f, 0f, 0f));
            sphereCollider.enabled = true;
            SetState(State.Anchored);

            ArrowBodyController.instance.Reset();
            ArrowHeadController.instance.Reset();

            ArrowBodyController.instance.Enable();
            ArrowHeadController.instance.Enable();

            trailRenderer.Clear();
            trailRenderer.enabled = false;

            Catapult.instance.EnableCollider();
        }

        public void Enable()
        {
            gameObject.SetActive(true);
            shadow.SetActive(true);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
            shadow.SetActive(false);
        }

        private void SetState(State state)
        {
            this.state = state;

            if (state != State.Idle || state != State.Chased)
            {
                Physics.IgnoreLayerCollision(10, 12);
            }

            switch (state)
            {
                case State.Anchored:
                    rigidBody.isKinematic = false;
                    sphereCollider.enabled = true;
                    break;
                case State.Dragging:
                    rigidBody.isKinematic = false;
                    break;
                case State.Launched:
                    rigidBody.isKinematic = false;
                    trailRenderer.enabled = true;
                    trailRenderer.Clear();

                    audioManager.PlaySound(Sfx.ThrowObj);

                    break;
                case State.Intercepted:
                    rigidBody.isKinematic = true;
                    sphereCollider.enabled = false;
                    break;
                case State.Rebounding:
                    rigidBody.isKinematic = false;
                    sphereCollider.enabled = false;
                    break;
                case State.Hanging:
                    rigidBody.isKinematic = true;
                    break;
                case State.Dropping:
                    rigidBody.isKinematic = false;
                    break;
                case State.Idle:
                    rigidBody.isKinematic = false;

                    if (!GameState.instance.IsTutorialRound())
                    {
                        AnturaController.instance.Enable();
                        AnturaController.instance.Reset();
                        AnturaController.instance.EnterScene();
                    }

                    Physics.IgnoreLayerCollision(10, 12, false);

                    break;
                case State.Chased:
                    trailRenderer.enabled = false;
                    Physics.IgnoreLayerCollision(10, 12, false);
                    break;
            }

            stateTime = 0;
        }

        public bool IsLaunched()
        {
            return !(state == State.Anchored || state == State.Dragging || state == State.Idle);
        }

        public bool IsIdle()
        {
            return state == State.Idle;
        }

        public bool IsAnchored()
        {
            return state == State.Anchored;
        }

        public void OnIntercepted()
        {
            if (state != State.Intercepted)
            {
                SetState(State.Intercepted);
            }
        }

        public void OnRebounded()
        {
            if (state != State.Rebounding)
            {
                SetState(State.Rebounding);

                Vector3 initialVelocity = new Vector3();
                initialVelocity.x = (REBOUND_DESTINATION.x - transform.position.x) / REBOUND_TIME;
                initialVelocity.y = (REBOUND_DESTINATION.y - transform.position.y) / REBOUND_TIME;
                initialVelocity.z = (REBOUND_DESTINATION.z - transform.position.z) / REBOUND_TIME;

                rigidBody.velocity = new Vector3(0, 0, 0);
                rigidBody.AddForce(initialVelocity, ForceMode.VelocityChange);
            }
        }

        void FixedUpdate()
        {
            if (state == State.Launched)
            {
                rigidBody.AddForce(Constants.GRAVITY, ForceMode.Acceleration);

                if (transform.position.y < -9 || stateTime > BALL_RESPAWN_TIME)
                {
                    if (ThrowBallsGame.instance.GameState.isRoundOngoing)
                    {
                        GameState.instance.OnBallLost();
                        Reset();
                    }
                    else
                    {
                        Disable();
                    }
                }
            }
            else if (state == State.Rebounding)
            {
                if (transform.position.z + rigidBody.velocity.z * Time.fixedDeltaTime <= REBOUND_DESTINATION.z)
                {
                    transform.position = REBOUND_DESTINATION;
                    UIController.instance.OnScreenCracked();
                    SetState(State.Hanging);
                }
            }
            else if (state == State.Anchored)
            {
                if (stateTime >= TIME_TO_IDLE)
                {
                    SetState(State.Idle);
                }
            }
            else if (state == State.Hanging)
            {
                if (stateTime >= SCREEN_HANG_TIME)
                {
                    SetState(State.Dropping);
                }
            }
            else if (state == State.Dropping)
            {
                rigidBody.AddForce(Constants.GRAVITY, ForceMode.Acceleration);

                if (stateTime >= DROP_TIME)
                {
                    GameState.instance.OnBallLost();
                    Reset();
                }
            }
        }

        public void DampenVelocity()
        {
            rigidBody.velocity = rigidBody.velocity * 0.5f;
            rigidBody.angularVelocity = rigidBody.angularVelocity * 0.5f;

            stateTime = BALL_RESPAWN_TIME - 0.75f;
        }

        void Update()
        {
            stateTime += Time.deltaTime;
        }
    }
}
