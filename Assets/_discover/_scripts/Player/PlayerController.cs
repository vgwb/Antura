using System.Collections;
using Antura.Audio;
using KinematicCharacterController;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Antura.Discover
{
    public enum PlayerState
    {
        Idle,
        Walking,
        Running,
        Jumping,
        Falling,
        Sliding,
        Sitting,
        Sleeping,
        Teleporting
    }

    [RequireComponent(typeof(CharacterMotorController))]
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Player Status (debug)")]
        [ReadOnly]
        public PlayerState CurrentState = PlayerState.Idle;
        [ReadOnly]
        public bool isGrounded = true;
        [ReadOnly]
        public bool isOnSlope = false;

        [Header("Idle")]
        [Tooltip("Time before sitting animation plays")]
        public float TimeToSit = 10.0f;

        [Tooltip("Time before sleeping animation plays")]
        public float TimeToSleep = 30.0f;

        [Header("Movement")]
        [Tooltip("Walk speed of the character in m/s")]
        public float WalkSpeed = 2.0f;

        [Tooltip("Run speed of the character in m/s")]
        public float RunSpeed = 4.0f;

        [Tooltip("Time of walking before auto-run")]
        public float TimeToAutoRun = 3.0f;

        [Tooltip("How fast the character accelerates to run")]
        public float RunAcceleration = 2.0f;

        [Tooltip("How fast the character turns to face movement direction")]
        [Range(0.0f, 0.3f)]
        public float RotationSmoothTime = 0.12f;

        [Tooltip("Acceleration and deceleration")]
        public float SpeedChangeRate = 10.0f;

        [Space(10)]
        [Tooltip("The height the player can jump")]
        public float JumpHeight = 1.2f;

        [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        public float JumpTimeout = 0.50f;

        [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        public float FallTimeout = 0.15f;

        [Header("Movement FX")]
        [Tooltip("Trail shown while the cat is running on the ground.")]
        public TrailRenderer runTrail;

        [Tooltip("Particle burst played when sprinting starts while grounded.")]
        public ParticleSystem sprintStartFx;

        [Tooltip("Minimum horizontal speed required to show the run trail.")]
        [Min(0f)] public float runTrailMinSpeed = 2.5f;

        [Tooltip("Seconds of sustained running before the trail appears.")]
        [Min(0f)] public float runTrailActivationDelay = 2f;

        [Header("Spawn")]
        [Tooltip("If true, snap the player to the ActionManager spawn point as soon as it becomes available when the scene starts.")]
        public bool snapToSpawnOnStart = true;

        [Header("Respawn / Teleport")]
        [Tooltip("Time (seconds) to travel back to the spawn point once a killzone is touched.")]
        [Min(0.05f)] public float respawnTravelDuration = 1.5f;

        [Tooltip("Maximum height (meters) of the arc travelled during respawn.")]
        [Min(0f)] public float respawnArcHeight = 2f;

        [Tooltip("Curve describing horizontal progress during the respawn travel. X = normalized time, Y = normalized progress.")]
        public AnimationCurve respawnProgressCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

        [Tooltip("Curve describing vertical offset during the respawn travel. X = normalized time, Y = [0,1] height coefficient.")]
        public AnimationCurve respawnHeightCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

        [Header("Fall Damage")]
        [Tooltip("Minimum fall height to trigger fall damage event (meters)")]
        public float minFallHeight = 5f;

        [Tooltip("Damage multiplier based on fall height")]
        public float fallDamageMultiplier = 10f;

        [Tooltip("Layer mask for destructible objects")]
        public LayerMask destructibleLayers;

        [Header("References")]
        public CatAnimationController animationController;


        public delegate void FallDamageEvent(float fallHeight, float damage, GameObject hitObject);
        public event FallDamageEvent OnFallDamage;

        // Public properties for external access
        public bool IsGrounded => isGrounded;
        public bool IsOnSlope => isOnSlope;
        public bool IsMoving => _speed > 0.1f || _slideVelocity.magnitude > 0.1f;
        public bool IsJumping => _hasJumped && !isGrounded;
        public bool IsSliding => isOnSlope && _slopeAngle > GetSlopeLimit();
        public bool IsAutoSprinting => _isAutoSprinting;
        public Vector2 CurrentMoveInput => _input?.move ?? Vector2.zero;
        public float CurrentSpeed => _speed;
        public Vector3 Velocity => _character.Motor.Velocity;
        public float WalkingTime => _walkingTime;

        public bool IsSitting => _isSitting;
        public bool IsSleeping => _isSleeping;
        public float IdleTime => _idleTime;
        // player
        private float _speed;
        private float _targetRotation = 0.0f;
        private float _verticalVelocity;
        private Vector3 _moveVelocity;
        private Vector3 _slideVelocity; // Separate slide velocity for smooth sliding

        // Auto-run
        private float _walkingTime = 0f;
        private bool _isAutoSprinting = false;
        private bool _wasSprinting;
        private float _runTrailTimer;

        // Idle behavior
        private float _idleTime = 0f;
        private bool _isSitting = false;
        private bool _isSleeping = false;

        // Fall tracking
        private float _fallStartHeight = 0f;
        private bool _isTrackingFall = false;
        private GameObject _landedOnObject = null;

        // timeout deltatime
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;
        private bool _hasJumped;
        private bool _wasGrounded;

        private PlayerInput _playerInput;
        //private CharacterController _controller;
        private StarterAssetsInputs _input;
        private GameObject _mainCamera;

        // Kinematic character controller
        //private KinematicCharacterMotor _motor;
        private CharacterMotorController _character;
        private bool _motorJumpHeld;
        private bool _isTeleporting;
        private Coroutine _teleportRoutine;
        private Coroutine _spawnSnapRoutine;

        // Slope detection
        private Vector3 _slopeNormal;
        private float _slopeAngle;

        private float GetSlopeLimit()
        {
            return _character.Motor.MaxStableSlopeAngle;
        }

        private void Awake()
        {
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }
        }

        private void Start()
        {
            _input = GetComponent<StarterAssetsInputs>();
            _playerInput = GetComponent<PlayerInput>();
            _character = GetComponent<CharacterMotorController>();

            if (_character.MeshRoot == null && animationController != null)
            {
                _character.MeshRoot = animationController.transform;
            }

            if (runTrail != null)
            {
                runTrail.emitting = false;
                runTrail.Clear();
            }

            _runTrailTimer = 0f;

            if (sprintStartFx != null)
            {
                sprintStartFx.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }

            _isTeleporting = false;
            _teleportRoutine = null;

            if (snapToSpawnOnStart)
            {
                if (_spawnSnapRoutine != null)
                {
                    StopCoroutine(_spawnSnapRoutine);
                }

                _spawnSnapRoutine = StartCoroutine(CoSnapToSpawnPointOnStart());
            }

            // reset our timeouts on start
            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;
        }

        private void Update()
        {
            GroundedCheck();
            Move();
            JumpAndGravity();
            UpdateIdleBehavior();
            UpdatePlayerState();
        }

        private void GroundedCheck()
        {
            _wasGrounded = isGrounded;

            var grounding = _character.Motor.GroundingStatus;
            isGrounded = grounding.IsStableOnGround;

            _slopeNormal = grounding.FoundAnyGround ? grounding.GroundNormal : Vector3.up;
            _slopeAngle = grounding.FoundAnyGround ? Vector3.Angle(_slopeNormal, Vector3.up) : 0f;
            isOnSlope = grounding.FoundAnyGround && _slopeAngle > 5f && _slopeAngle < 85f;

            if (grounding.GroundCollider != null)
            {
                _landedOnObject = grounding.GroundCollider.gameObject;
            }

            if (isGrounded && !_wasGrounded)
            {
                OnLanded();
            }
            else if (!isGrounded && _wasGrounded)
            {
                OnStartFalling();
            }

            if (transform.position.y < -30)
            {
                ActionManager.I.RespawnPlayer();
            }
        }

        private void Move()
        {
            if (_isTeleporting)
            {
                _speed = 0f;
                _moveVelocity = Vector3.zero;
                _slideVelocity = Vector3.zero;
                return;
            }

            if (DiscoverGameManager.I.State != GameplayState.Play3D)
                return;

            // Check for auto-sprint
            bool isSprinting = _input.sprint || _isAutoSprinting;

            // Update walking timer for auto-sprint
            if (_input.move != Vector2.zero && isGrounded && (!isOnSlope))
            {
                _walkingTime += Time.deltaTime;

                // Start auto-sprint after walking for TimeToAutoSprint seconds
                if (_walkingTime >= TimeToAutoRun && !_isAutoSprinting)
                {
                    _isAutoSprinting = true;
                    //Debug.Log("Auto-sprint activated!");
                }
            }
            else if (_input.move == Vector2.zero)
            {
                // Reset auto-sprint when stopping
                _walkingTime = 0f;
                _isAutoSprinting = false;
            }

            // Set target speed based on movement state
            float targetSpeed = 0f;
            float inputMagnitude = Mathf.Clamp01(_input.move.magnitude);

            if (_input.move != Vector2.zero)
            {
                float baseSpeed;

                if (isSprinting)
                {
                    baseSpeed = RunSpeed;
                }
                else
                {
                    baseSpeed = WalkSpeed;

                    // Gradual acceleration to sprint during auto-sprint transition
                    if (_walkingTime > 0 && _walkingTime < TimeToAutoRun)
                    {
                        float sprintProgress = _walkingTime / TimeToAutoRun;
                        baseSpeed = Mathf.Lerp(WalkSpeed, RunSpeed * 0.8f, sprintProgress * sprintProgress);
                    }
                }

                float magnitudeFactor = _input.analogMovement ? inputMagnitude : 1f;
                targetSpeed = baseSpeed * magnitudeFactor;
            }

            // Current horizontal speed
            float currentHorizontalSpeed = new Vector3(_moveVelocity.x, 0.0f, _moveVelocity.z).magnitude;
            float speedOffset = 0.1f;
            float accelerationMagnitude = _input.analogMovement ? Mathf.Max(inputMagnitude, 0.1f) : 1f;

            // Use different acceleration rates for sprint
            float currentSpeedChangeRate = isSprinting && _walkingTime > TimeToAutoRun ?
                RunAcceleration : SpeedChangeRate;

            // Accelerate or decelerate to target speed
            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed,
                    Time.deltaTime * currentSpeedChangeRate * accelerationMagnitude);
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }

            // Input direction
            Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;
            InputManager.SetCurrMovementVector(inputDirection);

            // Rotation
            if (_input.move != Vector2.zero)
            {
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                  _mainCamera.transform.eulerAngles.y;
            }

            // Calculate movement direction
            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

            UpdateMotorMovement(targetSpeed);

            // Update animation
            if (_speed > 0f || _slideVelocity.magnitude > 0.1f)
            {
                animationController.State = CatAnimationStates.walking;

                // Animation speed based on movement type
                float animSpeed = 0f;
                if (_slideVelocity.magnitude > 1f)
                {
                    animSpeed = 0.5f; // Sliding animation
                }
                else if (isSprinting)
                {
                    animSpeed = Mathf.Lerp(0.3f, 1f, (_speed - WalkSpeed) / (RunSpeed - WalkSpeed));
                }
                else
                {
                    animSpeed = Mathf.Lerp(0f, 0.3f, _speed / WalkSpeed);
                }

                animationController.WalkingSpeed = animSpeed;
            }
            else
            {
                animationController.State = CatAnimationStates.idle;
            }

            if (targetSpeed > 0)
                DiscoverNotifier.Game.OnPlayerMoved.Dispatch();

            UpdateMovementEffects(isSprinting);
        }

        private void UpdateMotorMovement(float targetSpeed)
        {
            if (_isTeleporting)
            {
                return;
            }

            bool jumpDown = false;
            if (_input.jump)
            {
                if (!_motorJumpHeld)
                {
                    jumpDown = true;
                }
                _motorJumpHeld = true;
            }
            else
            {
                _motorJumpHeld = false;
            }

            Quaternion cameraRotation = _mainCamera != null ? _mainCamera.transform.rotation : Quaternion.identity;
            DiscoverCharacterInputs motorInputs = new DiscoverCharacterInputs
            {
                MoveAxisForward = _input.move.y,
                MoveAxisRight = _input.move.x,
                CameraRotation = cameraRotation,
                JumpDown = jumpDown,
                JumpHeld = _motorJumpHeld,
                CrouchDown = false,
                CrouchUp = false,
            };

            _character.ApplyCharacterInputs(motorInputs, targetSpeed);

            Vector3 motorVelocity = _character.Motor.Velocity;
            _moveVelocity = new Vector3(motorVelocity.x, 0f, motorVelocity.z);
            _verticalVelocity = motorVelocity.y;

            if (isOnSlope)
            {
                Vector3 slideDirection = Vector3.ProjectOnPlane(Vector3.down, _slopeNormal).normalized;
                _slideVelocity = Vector3.Project(motorVelocity, slideDirection);
            }
            else
            {
                _slideVelocity = Vector3.zero;
            }

            if (_character.JumpedThisFrame)
            {
                OnMotorJumped();
            }

            if (IsSliding)
            {
                _walkingTime = 0f;
                _isAutoSprinting = false;
            }
        }

        private void UpdateMovementEffects(bool isSprinting)
        {
            if (_isTeleporting)
            {
                if (runTrail != null && runTrail.emitting)
                {
                    runTrail.emitting = false;
                    runTrail.Clear();
                }

                if (sprintStartFx != null)
                {
                    sprintStartFx.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                }

                _wasSprinting = false;
                return;
            }

            bool grounded = isGrounded;

            if (runTrail != null)
            {
                bool meetsSpeed = grounded && _speed >= runTrailMinSpeed;
                if (meetsSpeed)
                {
                    _runTrailTimer += Time.deltaTime;
                    bool shouldEmitTrail = _runTrailTimer >= runTrailActivationDelay;
                    if (runTrail.emitting != shouldEmitTrail)
                    {
                        runTrail.emitting = shouldEmitTrail;
                    }
                }
                else
                {
                    _runTrailTimer = 0f;
                    if (runTrail.emitting)
                    {
                        runTrail.emitting = false;
                    }
                }
            }

            if (sprintStartFx != null)
            {
                bool sprintingNow = grounded && isSprinting;
                if (sprintingNow && !_wasSprinting)
                {
                    sprintStartFx.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                    sprintStartFx.Play();
                }
                _wasSprinting = sprintingNow;
            }
            else
            {
                _wasSprinting = grounded && isSprinting;
            }
        }

        private void JumpAndGravity()
        {
            if (_isTeleporting)
            {
                return;
            }

            UpdateMotorAirState();
        }

        private void UpdateMotorAirState()
        {
            _verticalVelocity = _character.Motor.Velocity.y;

            if (isGrounded)
            {
                _fallTimeoutDelta = FallTimeout;
                if (_jumpTimeoutDelta > 0.0f)
                {
                    _jumpTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    _jumpTimeoutDelta = 0.0f;
                }
            }
            else
            {
                _jumpTimeoutDelta = JumpTimeout;
                if (_fallTimeoutDelta > 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }

                _input.jump = false;
                _motorJumpHeld = false;
                animationController.animator.speed = 2f;
            }
        }

        private void OnMotorJumped()
        {
            if (_isTeleporting)
            {
                return;
            }

            if (_isSitting || _isSleeping)
            {
                _isSitting = false;
                _isSleeping = false;
                _idleTime = 0f;
            }

            _hasJumped = true;
            _jumpTimeoutDelta = JumpTimeout;
            animationController.OnJumpStart();
            AudioManager.I.PlaySound(Sfx.CatMeow);
            PartyManager.I.TriggerPartyJump(0.0f, 0.08f, includeLeader: false);
        }

        private void OnLanded()
        {
            // Reset jump flag
            _hasJumped = false;

            // Animation - only change if not sitting/sleeping
            if (!_isSitting && !_isSleeping)
            {
                animationController.OnJumpEnded();
                animationController.animator.speed = 1f;
            }

            // Sound
            AudioManager.I.PlaySound(Sfx.BushRustlingIn);

            // Reset vertical velocity
            _verticalVelocity = -2f;

            // Fall damage evaluation
            if (_isTrackingFall)
            {
                _isTrackingFall = false;
                float fallHeight = _fallStartHeight - transform.position.y;
                if (fallHeight >= minFallHeight)
                {
                    float damage = (fallHeight - minFallHeight) * fallDamageMultiplier;
                    if (damage < 0)
                        damage = 0;

                    // Determine if landed on a destructible object (layer mask check)
                    GameObject hitObject = _landedOnObject;
                    if (hitObject != null)
                    {
                        int layerMask = 1 << hitObject.layer;
                        if ((destructibleLayers.value & layerMask) == 0)
                        {
                            // Not destructible; keep object reference anyway
                        }
                    }

                    OnFallDamage?.Invoke(fallHeight, damage, hitObject);
                    //                    Debug.Log($"FallDamage: height={fallHeight:F2} damage={damage:F1}");
                }
            }
        }

        private void OnStartFalling()
        {
            // Start tracking only if we are beginning to descend
            _fallStartHeight = transform.position.y;
            _isTrackingFall = true;
        }

        public void SpawnToLocation(Transform newLocation)
        {
            SetPosition(newLocation.position, newLocation.rotation);
        }

        private void SetPosition(Vector3 position, Quaternion? rotation = null)
        {
            Quaternion targetRotation = rotation ?? transform.rotation;

            _character.TeleportTo(position, targetRotation);

            // Reset velocities and states
            _verticalVelocity = 0f;
            _speed = 0f;
            _hasJumped = false;
            _moveVelocity = Vector3.zero;
            _slideVelocity = Vector3.zero;
            _walkingTime = 0f;
            _isAutoSprinting = false;
            _idleTime = 0f;
            _isSitting = false;
            _isSleeping = false;

            // Reset animation to idle
            animationController.State = CatAnimationStates.idle;
        }

        public void ForceJump(float? customHeight = null)
        {
            if (isGrounded)
            {
                float height = customHeight ?? JumpHeight;
                float gravityMagnitude = Mathf.Abs(_character.Gravity);
                float jumpVelocity = Mathf.Sqrt(Mathf.Max(height, 0.01f) * 2f * Mathf.Max(gravityMagnitude, 0.01f));
                _character.ForceUnground();
                _character.AddVelocity(transform.up * jumpVelocity);

                _hasJumped = true;
                animationController.OnJumpStart();
            }
        }

        private void UpdatePlayerState()
        {
            PlayerState previousState = CurrentState;

            if (_isTeleporting)
            {
                CurrentState = PlayerState.Teleporting;
            }
            else if (!isGrounded)
            {
                CurrentState = _verticalVelocity > 0 ? PlayerState.Jumping : PlayerState.Falling;
            }
            else if (_isSleeping)
            {
                CurrentState = PlayerState.Sleeping;
            }
            else if (_isSitting)
            {
                CurrentState = PlayerState.Sitting;
            }
            else if (IsSliding)
            {
                CurrentState = PlayerState.Sliding;
            }
            else if (_speed > 0.1f)
            {
                bool isSprinting = _input.sprint || _isAutoSprinting || _speed > (WalkSpeed + 0.5f);
                CurrentState = isSprinting ? PlayerState.Running : PlayerState.Walking;
            }
            else
            {
                CurrentState = PlayerState.Idle;
            }

            if (CurrentState != previousState)
            {
                OnStateChanged(previousState, CurrentState);
            }
        }

        private void OnStateChanged(PlayerState fromState, PlayerState toState)
        {
            // Handle state transitions
            switch (toState)
            {
                case PlayerState.Jumping:
                    break;
                case PlayerState.Falling:
                    break;
                case PlayerState.Sliding:
                    Debug.Log("Started sliding down slope!");
                    break;
                case PlayerState.Running:
                    if (_isAutoSprinting && fromState == PlayerState.Walking)
                    {
                        // Debug.Log("Transitioned to auto-sprint!");
                    }
                    break;
                case PlayerState.Walking:
                    break;
                case PlayerState.Sitting:
                    //                    Debug.Log("Cat is sitting!");
                    break;
                case PlayerState.Sleeping:
                    //                    Debug.Log("Cat is sleeping!");
                    break;
                case PlayerState.Idle:
                    // Reset auto-sprint when stopping
                    _walkingTime = 0f;
                    _isAutoSprinting = false;
                    break;
                case PlayerState.Teleporting:
                    _walkingTime = 0f;
                    _isAutoSprinting = false;
                    break;
                default:
                    break;
            }
        }

        private void UpdateIdleBehavior()
        {
            bool isIdle = _input.move == Vector2.zero && isGrounded && !isOnSlope && !_input.jump;

            if (_isTeleporting)
            {
                return;
            }

            if (isIdle)
            {
                _idleTime += Time.deltaTime;

                // Check for sleeping (30 seconds)
                if (_idleTime >= TimeToSleep && !_isSleeping)
                {
                    _isSleeping = true;
                    _isSitting = false;
                    animationController.State = CatAnimationStates.sleeping;
                    //                    Debug.Log("Cat is now sleeping!");
                }
                // Check for sitting (10 seconds)
                else if (_idleTime >= TimeToSit && !_isSitting && !_isSleeping)
                {
                    _isSitting = true;
                    animationController.State = CatAnimationStates.sitting;
                    //                    Debug.Log("Cat is now sitting!");
                }
                // Maintain the sitting/sleeping state if already in it
                else if (_isSleeping)
                {
                    // Ensure sleeping animation continues
                    if (animationController.State != CatAnimationStates.sleeping)
                        animationController.State = CatAnimationStates.sleeping;
                }
                else if (_isSitting)
                {
                    // Ensure sitting animation continues
                    if (animationController.State != CatAnimationStates.sitting)
                        animationController.State = CatAnimationStates.sitting;
                }
            }
            else if (_input.move != Vector2.zero || _input.jump)
            {
                // Only reset when there's actual input (movement or jump)
                if (_isSitting || _isSleeping)
                {
                    _idleTime = 0f;
                    _isSitting = false;
                    _isSleeping = false;
                    // Debug.Log("Cat woke up!");

                    // Return to idle before moving
                    animationController.State = CatAnimationStates.idle;
                }
                else
                {
                    _idleTime = 0f;
                }
            }
        }

        // private void OnDrawGizmosSelected()
        // {
        //     Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
        //     Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

        //     if (isGrounded)
        //         Gizmos.color = transparentGreen;
        //     else
        //         Gizmos.color = transparentRed;

        //     // Draw ground check sphere
        //     Vector3 spherePosition = transform.position + (Vector3.up * GroundedRadius);
        //     Gizmos.DrawWireSphere(spherePosition, GroundedRadius);

        //     // Draw slope normal
        //     if (isOnSlope)
        //     {
        //         Gizmos.color = Color.blue;
        //         Gizmos.DrawRay(transform.position, _slopeNormal * 2f);
        //     }
        // }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player_Killzone"))
            {
                return;
            }

            BeginTeleportToSpawn();
        }

        private void BeginTeleportToSpawn()
        {
            if (_isTeleporting || !isActiveAndEnabled)
            {
                return;
            }

            Transform target = ActionManager.I != null ? ActionManager.I.CurrentPlayerSpawnTransform : null;
            if (target == null)
            {
                Debug.LogWarning("PlayerController: respawn requested but no spawn transform is configured. Falling back to ActionManager.RespawnPlayer().", this);
                // Fallback: immediate respawn without animation
                ActionManager.I?.RespawnPlayer();
                return;
            }

            if (_teleportRoutine != null)
            {
                StopCoroutine(_teleportRoutine);
            }

            _teleportRoutine = StartCoroutine(CoTeleportToSpawn(target));
        }

        private IEnumerator CoTeleportToSpawn(Transform target)
        {
            _isTeleporting = true;
            _input.SetInputsEnabled(false);
            _speed = 0f;
            _moveVelocity = Vector3.zero;
            _slideVelocity = Vector3.zero;
            _walkingTime = 0f;
            _isAutoSprinting = false;
            _motorJumpHeld = false;
            _runTrailTimer = 0f;
            _hasJumped = false;

            if (runTrail != null)
            {
                runTrail.emitting = false;
                runTrail.Clear();
            }

            if (sprintStartFx != null)
            {
                sprintStartFx.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }

            Vector3 startPosition = transform.position;
            Quaternion startRotation = transform.rotation;
            Vector3 endPosition = target.position;
            Quaternion endRotation = target.rotation;

            float duration = Mathf.Max(0.05f, respawnTravelDuration);
            float elapsed = 0f;

            // Force unground to avoid sticking during travel
            _character.ForceUnground();

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float normalized = Mathf.Clamp01(elapsed / duration);
                float progress = respawnProgressCurve != null ? respawnProgressCurve.Evaluate(normalized) : normalized;
                float height = respawnHeightCurve != null ? respawnHeightCurve.Evaluate(normalized) : 0f;

                Vector3 position = Vector3.Lerp(startPosition, endPosition, progress);
                position.y += height * respawnArcHeight;
                Quaternion rotation = Quaternion.Slerp(startRotation, endRotation, progress);

                _character.TeleportTo(position, rotation);

                yield return null;
            }

            _character.TeleportTo(endPosition, endRotation);
            _character.ForceUnground();

            _isTeleporting = false;
            _input.SetInputsEnabled(true);
            _teleportRoutine = null;

            _idleTime = 0f;
            _isSitting = false;
            _isSleeping = false;
            animationController.State = CatAnimationStates.idle;
        }

        private void OnDisable()
        {
            if (_teleportRoutine != null)
            {
                StopCoroutine(_teleportRoutine);
                _teleportRoutine = null;
            }

            if (_spawnSnapRoutine != null)
            {
                StopCoroutine(_spawnSnapRoutine);
                _spawnSnapRoutine = null;
            }

            if (_isTeleporting)
            {
                _isTeleporting = false;
                _input?.SetInputsEnabled(true);
            }
        }

        private IEnumerator CoSnapToSpawnPointOnStart()
        {
            // Wait until ActionManager and the spawn transform are available.
            while (ActionManager.I == null || ActionManager.I.CurrentPlayerSpawnTransform == null)
            {
                yield return null;
            }

            SpawnToLocation(ActionManager.I.CurrentPlayerSpawnTransform);

            _spawnSnapRoutine = null;
        }
    }
}
