using Antura.Audio;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Antura.Discover
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Player")]
        public CatAnimationController anturaAnimation;

        [Header("Idle Behavior")]
        [Tooltip("Time before sitting animation plays")]
        public float TimeToSit = 10.0f;

        [Tooltip("Time before sleeping animation plays")]
        public float TimeToSleep = 30.0f;

        [Header("Movement Settings")]
        [Tooltip("Move speed of the character in m/s")]
        public float MoveSpeed = 2.0f;

        [Tooltip("Sprint speed of the character in m/s")]
        public float SprintSpeed = 5.335f;

        [Tooltip("Time of walking before auto-sprint kicks in")]
        public float TimeToAutoSprint = 3.0f;

        [Tooltip("How fast the character accelerates to sprint")]
        public float SprintAcceleration = 2.0f;

        [Tooltip("How fast the character turns to face movement direction")]
        [Range(0.0f, 0.3f)]
        public float RotationSmoothTime = 0.12f;

        [Tooltip("Acceleration and deceleration")]
        public float SpeedChangeRate = 10.0f;

        [Space(10)]
        [Tooltip("The height the player can jump")]
        public float JumpHeight = 1.2f;

        [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
        public float Gravity = -15.0f;

        [Space(10)]
        [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        public float JumpTimeout = 0.50f;

        [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        public float FallTimeout = 0.15f;

        [Header("Player States")]
        public PlayerState CurrentState { get; private set; } = PlayerState.Idle;

        [Header("Player Grounded")]
        [Tooltip("If the character is grounded or not")]
        public bool isGrounded = true;
        public bool isOnSlope = false;

        [Tooltip("Useful for rough ground")]
        public float GroundedOffset = -0.14f;

        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        public float GroundedRadius = 0.28f;

        [Tooltip("What layers the character uses as ground")]
        public LayerMask GroundLayers;

        [Header("Slope Settings")]
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

        // Player states enum
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

        // Public properties for external access
        public bool IsGrounded => isGrounded;
        public bool IsOnSlope => isOnSlope;
        public bool IsMoving => _speed > 0.1f || _slideVelocity.magnitude > 0.1f;
        public bool IsJumping => _hasJumped && !isGrounded;
        public bool IsSliding => isOnSlope && _slopeAngle > _controller.slopeLimit;
        public bool IsAutoSprinting => _isAutoSprinting;
        public Vector2 CurrentMoveInput => _input?.move ?? Vector2.zero;
        public float CurrentSpeed => _speed;
        public Vector3 Velocity => new Vector3(_moveVelocity.x + _slideVelocity.x, _verticalVelocity, _moveVelocity.z + _slideVelocity.z);
        public float WalkingTime => _walkingTime;

        // player
        private float _speed;
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;
        private Vector3 _moveVelocity;
        private Vector3 _slideVelocity; // Separate slide velocity for smooth sliding

        // Auto-sprint
        private float _walkingTime = 0f;
        private bool _isAutoSprinting = false;

        // Idle behavior
        private float _idleTime = 0f;
        private bool _isSitting = false;
        private bool _isSleeping = false;

        // timeout deltatime
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;
        private bool _hasJumped;
        private bool _wasGrounded;

        private PlayerInput _playerInput;
        private CharacterController _controller;
        private StarterAssetsInputs _input;
        private GameObject _mainCamera;

        // Slope detection
        private RaycastHit _slopeHit;
        private Vector3 _slopeNormal;
        private float _slopeAngle;

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

            // Sphere cast for ground detection - more reliable than CheckSphere
            Vector3 spherePosition = transform.position + (Vector3.up * GroundedRadius);
            float checkDistance = GroundedRadius + GroundedOffset + 0.1f;

            // Use SphereCast for better ground detection
            isGrounded = Physics.SphereCast(
                spherePosition,
                GroundedRadius,
                Vector3.down,
                out RaycastHit groundHit,
                checkDistance,
                GroundLayers,
                QueryTriggerInteraction.Ignore
            );

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
            if (_input.move != Vector2.zero && isGrounded && !isOnSlope)
            {
                _walkingTime += Time.deltaTime;

                // Start auto-sprint after walking for TimeToAutoSprint seconds
                if (_walkingTime >= TimeToAutoSprint && !_isAutoSprinting)
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

            if (_input.move != Vector2.zero)
            {
                if (isSprinting)
                {
                    targetSpeed = SprintSpeed;
                }
                else
                {
                    targetSpeed = MoveSpeed;

                    // Gradual acceleration to sprint during auto-sprint transition
                    if (_walkingTime > 0 && _walkingTime < TimeToAutoSprint)
                    {
                        float sprintProgress = _walkingTime / TimeToAutoSprint;
                        targetSpeed = Mathf.Lerp(MoveSpeed, SprintSpeed * 0.8f, sprintProgress * sprintProgress);
                    }
                }

                targetSpeed *= _input.move.magnitude;
            }

            // Current horizontal speed
            float currentHorizontalSpeed = new Vector3(_moveVelocity.x, 0.0f, _moveVelocity.z).magnitude;
            float speedOffset = 0.1f;
            float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

            // Use different acceleration rates for sprint
            float currentSpeedChangeRate = isSprinting && _walkingTime > TimeToAutoSprint ?
                SprintAcceleration : SpeedChangeRate;

            // Accelerate or decelerate to target speed
            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                    Time.deltaTime * currentSpeedChangeRate);
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
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                    RotationSmoothTime);
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }

            // Calculate movement direction
            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

            // Handle slope movement
            if (isGrounded && isOnSlope)
            {
                // Check if on a steep slope that should cause sliding
                if (_slopeAngle > _controller.slopeLimit)
                {
                    // Calculate slide direction (down the slope)
                    Vector3 slideDirection = Vector3.ProjectOnPlane(Vector3.down, _slopeNormal).normalized;

                    // Accelerate slide velocity up to max speed
                    float targetSlideSpeed = Mathf.Min(slideSpeed + (_slopeAngle - _controller.slopeLimit) * 0.2f, maxSlideSpeed);
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
                    // Normal slope movement (walkable slopes)
                    targetDirection = Vector3.ProjectOnPlane(targetDirection, _slopeNormal).normalized;
                    _moveVelocity = targetDirection * _speed;

                    // Gradually reduce slide velocity on walkable slopes
                    _slideVelocity = Vector3.Lerp(_slideVelocity, Vector3.zero, Time.deltaTime * 5f);
                }
            }
            else
            {
                // Normal movement (flat ground or in air)
                _moveVelocity = targetDirection * _speed;

                // Gradually reduce slide velocity when not on slope
                if (!isOnSlope)
                {
                    _slideVelocity = Vector3.Lerp(_slideVelocity, Vector3.zero, Time.deltaTime * 3f);
                }
            }

            // Update animation
            if (_speed > 0f || _slideVelocity.magnitude > 0.1f)
            {
                anturaAnimation.State = CatAnimationStates.walking;

                // Animation speed based on movement type
                float animSpeed = 0f;
                if (_slideVelocity.magnitude > 1f)
                {
                    animSpeed = 0.5f; // Sliding animation
                }
                else if (isSprinting)
                {
                    animSpeed = Mathf.Lerp(0.3f, 1f, (_speed - MoveSpeed) / (SprintSpeed - MoveSpeed));
                }
                else
                {
                    animSpeed = Mathf.Lerp(0f, 0.3f, _speed / MoveSpeed);
                }

                anturaAnimation.WalkingSpeed = animSpeed;
            }
            else
            {
                anturaAnimation.State = CatAnimationStates.idle;
            }

            if (targetSpeed > 0)
                DiscoverNotifier.Game.OnPlayerMoved.Dispatch();
        }

        private void JumpAndGravity()
        {
            if (isGrounded)
            {
                // Reset fall timeout
                _fallTimeoutDelta = FallTimeout;

                // Keep a small downward force when grounded for better ground detection
                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;

                    // Extra downward force on slopes to prevent bouncing
                    if (isOnSlope && !_hasJumped)
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
                    anturaAnimation.OnJumpStart();
                    AudioManager.I.PlaySound(Sfx.CatMeow);
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
                anturaAnimation.animator.speed = 2f;
            }

            // Apply gravity - ALWAYS apply gravity, even when grounded
            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * Time.deltaTime;
            }
        }

        private void ApplyMovement()
        {
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
                anturaAnimation.OnJumpEnded();
                anturaAnimation.animator.speed = 1f;
            }

            // Sound
            AudioManager.I.PlaySound(Sfx.BushRustlingIn);

            // Reset vertical velocity
            _verticalVelocity = -2f;
        }

        public void SpawnToNewLocation(Transform newLocation)
        {
            SetPosition(newLocation.position, newLocation.rotation);
        }

        public void SetPosition(Vector3 position, Quaternion? rotation = null)
        {
            _controller.enabled = false;
            transform.position = position;
            if (rotation.HasValue)
            {
                transform.rotation = rotation.Value;
            }
            _controller.enabled = true;

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
            anturaAnimation.State = CatAnimationStates.idle;
        }

        public void ForceJump(float? customHeight = null)
        {
            if (isGrounded)
            {
                float height = customHeight ?? JumpHeight;
                _verticalVelocity = Mathf.Sqrt(height * -2f * Gravity);
                _hasJumped = true;
                anturaAnimation.OnJumpStart();
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
            else if (isOnSlope && _slopeAngle > _controller.slopeLimit)
            {
                CurrentState = PlayerState.Sliding;
            }
            else if (_speed > 0.1f)
            {
                bool isSprinting = _input.sprint || _isAutoSprinting || _speed > (MoveSpeed + 0.5f);
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
                        Debug.Log("Transitioned to auto-sprint!");
                    }
                    break;
                case PlayerState.Walking:
                    break;
                case PlayerState.Sitting:
                    Debug.Log("Cat is sitting!");
                    break;
                case PlayerState.Sleeping:
                    Debug.Log("Cat is sleeping!");
                    break;
                case PlayerState.Idle:
                    // Reset auto-sprint when stopping
                    _walkingTime = 0f;
                    _isAutoSprinting = false;
                    break;
            }
        }

        private void UpdateIdleBehavior()
        {
            // Check if player is idle (no input and grounded, not sliding)
            bool isIdle = _input.move == Vector2.zero && isGrounded && !isOnSlope && !_input.jump;

            if (isIdle)
            {
                _idleTime += Time.deltaTime;

                // Check for sleeping (30 seconds)
                if (_idleTime >= TimeToSleep && !_isSleeping)
                {
                    _isSleeping = true;
                    _isSitting = false;
                    anturaAnimation.State = CatAnimationStates.sleeping;
                    Debug.Log("Cat is now sleeping!");
                }
                // Check for sitting (10 seconds)
                else if (_idleTime >= TimeToSit && !_isSitting && !_isSleeping)
                {
                    _isSitting = true;
                    anturaAnimation.State = CatAnimationStates.sitting;
                    Debug.Log("Cat is now sitting!");
                }
                // Maintain the sitting/sleeping state if already in it
                else if (_isSleeping)
                {
                    // Ensure sleeping animation continues
                    if (anturaAnimation.State != CatAnimationStates.sleeping)
                        anturaAnimation.State = CatAnimationStates.sleeping;
                }
                else if (_isSitting)
                {
                    // Ensure sitting animation continues
                    if (anturaAnimation.State != CatAnimationStates.sitting)
                        anturaAnimation.State = CatAnimationStates.sitting;
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
                    Debug.Log("Cat woke up!");

                    // Return to idle before moving
                    anturaAnimation.State = CatAnimationStates.idle;
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
