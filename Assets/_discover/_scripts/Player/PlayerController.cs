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
        Sleeping
    }

    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(KinematicCharacterMotor))]
    [RequireComponent(typeof(DiscoverCharacterMotorAdapter))]
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

        [Header("Character Motor")]
        [Tooltip("Use the Kinematic Character Controller instead of the legacy CharacterController for movement")]
        public bool useKinematicCharacterController = true;

        [Space(10)]
        [Tooltip("The height the player can jump")]
        public float JumpHeight = 1.2f;

        [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        public float JumpTimeout = 0.50f;

        [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
        public float Gravity = -15.0f;

        [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        public float FallTimeout = 0.15f;

        [Header("Grounded")]
        [Tooltip("Useful for rough ground")]
        public float GroundedOffset = -0.14f;

        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        public float GroundedRadius = 0.28f;

        [Tooltip("What layers the character uses as ground")]
        public LayerMask GroundLayers;

        [Header("Slope Settings")]
        [Tooltip("Disable all slope-specific movement (no sliding, no special handling)")]
        public bool disableSlopeMovement = false;
        [Tooltip("Speed of sliding down slopes")]
        public float slideSpeed = 5f;

        [Tooltip("Acceleration of sliding (makes it feel more natural)")]
        public float slideAcceleration = 10f;

        [Tooltip("Maximum slide speed")]
        public float maxSlideSpeed = 12f;

        [Tooltip("Extra downward force to keep grounded on slopes")]
        public float slopeForceDown = 5f;

        [Tooltip("Can the player control movement while sliding?")]
        public bool allowSlopeControl = true;

        [Tooltip("How much control player has while sliding (0-1)")]
        [Range(0f, 1f)]
        public float slopeControlAmount = 0.3f;

        [Tooltip("Speed multiplier when walking on slopes (not sliding)")]
        [HideInInspector]
        public AnimationCurve slopeSpeedCurve = AnimationCurve.Linear(0f, 1f, 45f, 1.2f); // Deprecated - kept hidden for backward serialization safety

        [Header("Movement FX")]
        [Tooltip("Trail shown while the cat is running on the ground.")]
        public TrailRenderer runTrail;

        [Tooltip("Particle burst played when sprinting starts while grounded.")]
        public ParticleSystem sprintStartFx;

        [Tooltip("Minimum horizontal speed required to show the run trail.")]
        [Min(0f)] public float runTrailMinSpeed = 2.5f;

        [Tooltip("Seconds of sustained running before the trail appears.")]
        [Min(0f)] public float runTrailActivationDelay = 2f;

        [Header("Simplified Slope Movement Tuning")]
        [Tooltip("Max fractional speed reduction at the slope limit when moving uphill (0 = none, 0.3 = up to 30% slower at limit)")]
        [Range(0f, 0.6f)] public float uphillMaxSlowdown = 0.15f;
        [Tooltip("Max fractional speed boost at the slope limit when moving downhill (0 = none, 0.3 = up to 30% faster at limit)")]
        [Range(0f, 0.6f)] public float downhillMaxBoost = 0.08f;

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
        public bool IsSliding => !disableSlopeMovement && isOnSlope && _slopeAngle > GetSlopeLimit();
        public bool IsAutoSprinting => _isAutoSprinting;
        public Vector2 CurrentMoveInput => _input?.move ?? Vector2.zero;
        public float CurrentSpeed => _speed;
        public Vector3 Velocity => _usingMotor ? (_motor != null ? _motor.Velocity : Vector3.zero) : new Vector3(_moveVelocity.x + _slideVelocity.x, _verticalVelocity, _moveVelocity.z + _slideVelocity.z);
        public float WalkingTime => _walkingTime;

        public bool IsSitting => _isSitting;
        public bool IsSleeping => _isSleeping;
        public float IdleTime => _idleTime;
        // player
        private float _speed;
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;
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
        private CharacterController _controller;
        private StarterAssetsInputs _input;
        private GameObject _mainCamera;

        // Kinematic character controller
        private KinematicCharacterMotor _motor;
        private DiscoverCharacterMotorAdapter _motorAdapter;
        private bool _usingMotor;
        private bool _motorJumpHeld;

        // Slope detection
        private RaycastHit _slopeHit;
        private Vector3 _slopeNormal;
        private float _slopeAngle;

        private float GetSlopeLimit()
        {
            if (_usingMotor && _motor != null)
            {
                return _motor.MaxStableSlopeAngle;
            }

            if (_controller != null)
            {
                return _controller.slopeLimit;
            }

            return 45f;
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
            _controller = GetComponent<CharacterController>();
            _input = GetComponent<StarterAssetsInputs>();
            _playerInput = GetComponent<PlayerInput>();

            _motor = GetComponent<KinematicCharacterMotor>();
            _motorAdapter = GetComponent<DiscoverCharacterMotorAdapter>();
            _usingMotor = useKinematicCharacterController && _motor != null && _motorAdapter != null;

            if (_usingMotor)
            {
                if (_controller != null)
                {
                    _controller.enabled = false;
                    _motor.SetCapsuleDimensions(_controller.radius, _controller.height, _controller.center.y);
                    _motor.MaxStableSlopeAngle = _controller.slopeLimit;
                }
                else
                {
                    _motor.MaxStableSlopeAngle = GetSlopeLimit();
                }

                float gravityMagnitude = Mathf.Abs(Gravity);
                Gravity = gravityMagnitude > 0.01f ? -gravityMagnitude : -9.81f;
                _motor.StableGroundLayers = GroundLayers;
                _motorAdapter.Gravity = Gravity;
                _motorAdapter.DefaultMaxStableMoveSpeed = WalkSpeed;
                _motorAdapter.DefaultMaxAirMoveSpeed = RunSpeed;
                _motorAdapter.AirAccelerationSpeed = Mathf.Max(1f, SpeedChangeRate);
                _motorAdapter.StableMovementSharpness = Mathf.Max(1f, SpeedChangeRate);
                _motorAdapter.JumpUpSpeed = Mathf.Sqrt(Mathf.Max(JumpHeight, 0.01f) * 2f * Mathf.Abs(Gravity));
                _motorAdapter.JumpScalableForwardSpeed = RunSpeed;
                if (_controller != null)
                {
                    _motorAdapter.CrouchedCapsuleHeight = Mathf.Max(0.5f, _controller.height * 0.5f);
                }

                if (_motorAdapter.MeshRoot == null && animationController != null)
                {
                    _motorAdapter.MeshRoot = animationController.transform;
                }
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

            // reset our timeouts on start
            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;
        }

        private void Update()
        {
            GroundedCheck();
            Move();
            JumpAndGravity();
            ApplyMovement();
            UpdateIdleBehavior();
            UpdatePlayerState();
        }

        private void GroundedCheck()
        {
            _wasGrounded = isGrounded;

            if (_usingMotor && _motor != null)
            {
                var grounding = _motor.GroundingStatus;
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

                return;
            }

            // Sphere cast for ground detection - more reliable than CheckSphere
            Vector3 spherePosition = transform.position + (Vector3.up * GroundedRadius);
            float checkDistance = GroundedRadius + GroundedOffset + 0.1f;

            // Use SphereCast for better ground detection
            RaycastHit groundHit;
            isGrounded = Physics.SphereCast(
                spherePosition,
                GroundedRadius,
                Vector3.down,
                out groundHit,
                checkDistance,
                GroundLayers,
                QueryTriggerInteraction.Ignore
            );

            // Store the object we're standing on
            if (isGrounded && groundHit.collider != null)
            {
                _landedOnObject = groundHit.collider.gameObject;
            }

            // Also check CharacterController's grounded state as backup
            if (!isGrounded && _controller.isGrounded)
            {
                isGrounded = true;
            }

            // Slope detection with better raycast
            isOnSlope = false;
            _slopeAngle = 0f;

            if (Physics.Raycast(
                transform.position + (Vector3.up * 0.1f), // Start slightly above ground
                Vector3.down,
                out _slopeHit,
                _controller.height / 2 + 0.5f,
                GroundLayers))
            {
                _slopeNormal = _slopeHit.normal;
                _slopeAngle = Vector3.Angle(_slopeNormal, Vector3.up);

                // Check if we're on a slope (not flat ground, not a wall)
                isOnSlope = _slopeAngle > 5f && _slopeAngle < 85f;
            }

            // Landing detection
            if (isGrounded && !_wasGrounded)
            {
                OnLanded();
            }
            // Start tracking fall
            else if (!isGrounded && _wasGrounded)
            {
                OnStartFalling();
            }

            // Void check
            if (transform.position.y < -30)
            {
                ActionManager.I.RespawnPlayer();
            }
        }

        private void Move()
        {
            if (DiscoverGameManager.I.State != GameplayState.Play3D)
                return;

            // Check for auto-sprint
            bool isSprinting = _input.sprint || _isAutoSprinting;

            // Update walking timer for auto-sprint
            if (_input.move != Vector2.zero && isGrounded && (!isOnSlope || disableSlopeMovement))
            {
                _walkingTime += Time.deltaTime;

                // Start auto-sprint after walking for TimeToAutoSprint seconds
                if (_walkingTime >= TimeToAutoRun && !_isAutoSprinting)
                {
                    _isAutoSprinting = true;
                    Debug.Log("Auto-sprint activated!");
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
                if (!_usingMotor)
                {
                    float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                        RotationSmoothTime);
                    transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
                }
            }

            // Calculate movement direction
            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

            if (_usingMotor)
            {
                UpdateMotorMovement(targetSpeed);
            }
            else
            {
                // Handle slope movement
                if (!disableSlopeMovement && isGrounded && isOnSlope)
                {
                    // Check if on a steep slope that should cause sliding
                    if (_slopeAngle > _controller.slopeLimit)
                    {
                        // Calculate slide direction (down the slope)
                        Vector3 slideDirection = Vector3.ProjectOnPlane(Vector3.down, _slopeNormal).normalized;

                        // Accelerate slide velocity up to max speed (faster sliding with steeper angles)
                        float angleMultiplier = (_slopeAngle - _controller.slopeLimit) / (90f - _controller.slopeLimit);
                        float targetSlideSpeed = Mathf.Lerp(slideSpeed, maxSlideSpeed, angleMultiplier);

                        _slideVelocity = Vector3.Lerp(_slideVelocity, slideDirection * targetSlideSpeed,
                            Time.deltaTime * slideAcceleration);

                        // Allow limited control while sliding
                        if (allowSlopeControl && _input.move != Vector2.zero)
                        {
                            // Project input onto slope plane for some control
                            Vector3 slopeMovement = Vector3.ProjectOnPlane(targetDirection, _slopeNormal).normalized;
                            _moveVelocity = slopeMovement * _speed * slopeControlAmount;
                        }
                        else
                        {
                            _moveVelocity = Vector3.zero;
                        }

                        // Reset sprint timer while sliding
                        _walkingTime = 0f;
                        _isAutoSprinting = false;
                    }
                    else
                    {
                        // Walkable slope movement (simplified): project movement onto slope plane
                        Vector3 slopeDir = Vector3.ProjectOnPlane(targetDirection, _slopeNormal).normalized;

                        // Determine if moving uphill (positive vertical component) or downhill (negative)
                        bool movingUphill = slopeDir.y > 0.01f;
                        bool movingDownhill = slopeDir.y < -0.01f;

                        // Normalized angle factor (0 at flat, 1 at slope limit)
                        float angleFactor = Mathf.Clamp01(_slopeAngle / Mathf.Max(1f, _controller.slopeLimit));
                        float speedMultiplier = 1f;
                        if (movingUphill)
                        {
                            speedMultiplier -= uphillMaxSlowdown * angleFactor; // up to X% slower near limit
                        }
                        else if (movingDownhill)
                        {
                            speedMultiplier += downhillMaxBoost * angleFactor; // small boost downhill
                        }

                        _moveVelocity = slopeDir * _speed * speedMultiplier;

                        // Kill residual slide velocity quickly on walkable slopes
                        _slideVelocity = Vector3.Lerp(_slideVelocity, Vector3.zero, Time.deltaTime * 6f);
                    }
                }
                else
                {
                    // Normal movement (flat ground or in air)
                    _moveVelocity = targetDirection * _speed;

                    // Gradually reduce slide velocity when not on slope
                    if (!isOnSlope || disableSlopeMovement)
                    {
                        _slideVelocity = Vector3.Lerp(_slideVelocity, Vector3.zero, Time.deltaTime * 3f);
                    }
                }
            }

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
            if (_motorAdapter == null || _motor == null)
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

            _motorAdapter.ApplyCharacterInputs(motorInputs, targetSpeed);

            Vector3 motorVelocity = _motor.Velocity;
            _moveVelocity = new Vector3(motorVelocity.x, 0f, motorVelocity.z);
            _verticalVelocity = motorVelocity.y;

            if (!disableSlopeMovement && isOnSlope)
            {
                Vector3 slideDirection = Vector3.ProjectOnPlane(Vector3.down, _slopeNormal).normalized;
                _slideVelocity = Vector3.Project(motorVelocity, slideDirection);
            }
            else
            {
                _slideVelocity = Vector3.zero;
            }

            if (_motorAdapter.JumpedThisFrame)
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
                        if (!shouldEmitTrail)
                        {
                            runTrail.Clear();
                        }
                    }
                }
                else
                {
                    _runTrailTimer = 0f;
                    if (runTrail.emitting)
                    {
                        runTrail.emitting = false;
                        runTrail.Clear();
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
            if (_usingMotor)
            {
                UpdateMotorAirState();
                return;
            }

            if (isGrounded)
            {
                // Reset fall timeout
                _fallTimeoutDelta = FallTimeout;

                // Keep a small downward force when grounded for better ground detection
                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;

                    // Extra downward force on slopes to prevent bouncing
                    if (!disableSlopeMovement && isOnSlope && !_hasJumped)
                    {
                        _verticalVelocity = -slopeForceDown;
                    }
                }

                // Jump (wake up from sitting/sleeping if jumping)
                if (_input.jump && _jumpTimeoutDelta <= 0.0f && !_hasJumped)
                {
                    // Wake up if sitting or sleeping
                    if (_isSitting || _isSleeping)
                    {
                        _isSitting = false;
                        _isSleeping = false;
                        _idleTime = 0f;
                    }

                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
                    _hasJumped = true;
                    animationController.OnJumpStart();
                    AudioManager.I.PlaySound(Sfx.CatMeow);
                    PartyManager.I.TriggerPartyJump(0.0f, 0.08f, includeLeader: false);
                }

                // Jump timeout
                if (_jumpTimeoutDelta >= 0.0f)
                {
                    _jumpTimeoutDelta -= Time.deltaTime;
                }
            }
            else
            {
                // In air
                _jumpTimeoutDelta = JumpTimeout;

                // Fall timeout
                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }

                // Clear jump input when not grounded
                _input.jump = false;

                // Set animation speed for air
                animationController.animator.speed = 2f;
            }

            // Apply gravity - ALWAYS apply gravity, even when grounded
            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * Time.deltaTime;
            }
        }

        private void UpdateMotorAirState()
        {
            if (_motor != null)
            {
                _verticalVelocity = _motor.Velocity.y;
            }

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
            if (!_usingMotor)
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

        private void ApplyMovement()
        {
            if (_usingMotor)
            {
                return;
            }

            // Combine all movement components
            Vector3 horizontalMovement = _moveVelocity + _slideVelocity;
            Vector3 motion = horizontalMovement + Vector3.up * _verticalVelocity;

            // Apply movement
            _controller.Move(motion * Time.deltaTime);

            // Update actual velocity after move (for external systems)
            _moveVelocity.x = _controller.velocity.x;
            _moveVelocity.z = _controller.velocity.z;

            // If the controller says we're grounded after moving, we're grounded
            if (_controller.isGrounded && _verticalVelocity < 0)
            {
                isGrounded = true;
            }
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

            if (_usingMotor && _motorAdapter != null)
            {
                _motorAdapter.TeleportTo(position, targetRotation);
                transform.SetPositionAndRotation(position, targetRotation);
            }
            else if (_controller != null)
            {
                bool wasEnabled = _controller.enabled;
                _controller.enabled = false;
                transform.position = position;
                transform.rotation = targetRotation;
                _controller.enabled = wasEnabled;
            }
            else
            {
                transform.SetPositionAndRotation(position, targetRotation);
            }

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
                if (_usingMotor && _motorAdapter != null)
                {
                    float gravityMagnitude = Mathf.Abs(_motorAdapter.Gravity);
                    float jumpVelocity = Mathf.Sqrt(Mathf.Max(height, 0.01f) * 2f * Mathf.Max(gravityMagnitude, 0.01f));
                    _motorAdapter.ForceUnground();
                    _motorAdapter.AddVelocity(transform.up * jumpVelocity);
                }
                else
                {
                    _verticalVelocity = Mathf.Sqrt(height * -2f * Gravity);
                }

                _hasJumped = true;
                animationController.OnJumpStart();
            }
        }

        private void UpdatePlayerState()
        {
            PlayerState previousState = CurrentState;

            if (!isGrounded)
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
                default:
                    break;
            }
        }

        private void UpdateIdleBehavior()
        {
            // Check if player is idle (no input and grounded, not sliding)
            bool isIdle = _input.move == Vector2.zero && isGrounded && (!isOnSlope || disableSlopeMovement) && !_input.jump;

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

        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (isGrounded)
                Gizmos.color = transparentGreen;
            else
                Gizmos.color = transparentRed;

            // Draw ground check sphere
            Vector3 spherePosition = transform.position + (Vector3.up * GroundedRadius);
            Gizmos.DrawWireSphere(spherePosition, GroundedRadius);

            // Draw slope normal
            if (isOnSlope)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawRay(transform.position, _slopeNormal * 2f);
            }
        }
    }
}
