using Antura.Audio;
using Antura.Dog;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Antura.Minigames.DiscoverCountry
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PlayerInput))]

    public class EdPlayer : MonoBehaviour
    {
        [Header("Player")]
        public AnturaPetSwitcher anturaPetSwitcher;
        private AnturaAnimationController anturaAnimation => anturaPetSwitcher.AnimController;

        [Tooltip("Move speed of the character in m/s")]
        public float MoveSpeed = 2.0f;

        [Tooltip("Sprint speed of the character in m/s")]
        public float SprintSpeed = 5.335f;

        [Tooltip("How fast the character turns to face movement direction")]
        [Range(0.0f, 0.3f)]
        public float RotationSmoothTime = 0.12f;

        [Tooltip("Acceleration and deceleration")]
        public float SpeedChangeRate = 10.0f;

        [Space(10)]
        [Tooltip("The height the player can jump")]
        public float JumpHeight = 1.2f;

        [Space(10)]
        [Tooltip("The height the player can jump mid air")]
        public float MidAirJumpHeight = 1.2f;

        [Tooltip("Number of jumps the player can perform before landing")]
        public int MaxJumps = 2;

        [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
        public float Gravity = -15.0f;

        [Space(10)]
        [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        public float JumpTimeout = 0.50f;

        [Tooltip("Time required to pass before being able to jump again mid air. Set to 0f to instantly jump again")]
        public float MidAirJumpTimeout = 0.50f;

        [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        public float FallTimeout = 0.15f;

        [Header("Player Grounded")]
        [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        public bool Grounded = true;

        [Tooltip("Useful for rough ground")]
        public float GroundedOffset = -0.14f;

        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        public float GroundedRadius = 0.28f;

        [Tooltip("What layers the character uses as ground")]
        public LayerMask GroundLayers;

        // player
        private float _speed;
        private float _animationBlend;
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;

        // timeout deltatime
        private float _jumpTimeoutDelta;
        private float _midAirJumpTimeoutDelta;
        private float _fallTimeoutDelta;
        private int _nCurrentJump;

        private PlayerInput _playerInput;
        private CharacterController _controller;
        private StarterAssetsInputs _input;
        private GameObject _mainCamera;

        private bool IsCurrentDeviceMouse
        {
            get
            {
                return _playerInput.currentControlScheme == "KeyboardMouse";
            }
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

            // reset our timeouts on start
            _jumpTimeoutDelta = JumpTimeout;
            _midAirJumpTimeoutDelta = MidAirJumpTimeout;
            _fallTimeoutDelta = FallTimeout;
        }

        private void Update()
        {
            JumpAndGravity();
            GroundedCheck();
            Move();
        }

        Vector3 spherePosition;
        private void GroundedCheck()
        {
            // set sphere position, with offset
            spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);
            // why this custom methid instead of using native:
            // Grounded = _controller.isGrounded;
        }

        private void Move()
        {
            bool isSprinting = _input.sprint;

            // set target speed based on move speed, sprint speed and if sprint is pressed
            float targetSpeed = isSprinting ? SprintSpeed : MoveSpeed;

            // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

            // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is no input, set the target speed to 0
            if (_input.move == Vector2.zero)
            {
                targetSpeed = 0.0f;
            }
            else
            {
                targetSpeed *= _input.move.magnitude;
                if (targetSpeed >= SprintSpeed)
                {
                    targetSpeed = SprintSpeed;
                    isSprinting = true;
                }
            }

            // a reference to the players current horizontal velocity
            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

            float speedOffset = 0.1f;
            float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

            // accelerate or decelerate to target speed
            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                // creates curved result rather than a linear one giving a more organic speed change
                // note T in Lerp is clamped, so we don't need to clamp our speed
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                    Time.deltaTime * SpeedChangeRate);

                // round speed to 3 decimal places
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }

            //_animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
            // if (_animationBlend < 0.01f)
            // _animationBlend = 0f;

            // normalise input direction
            Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;
            InputManager.SetCurrMovementVector(inputDirection); // Very important since now it's set from here, so other elements can check the current movement vector from InputManager

            // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is a move input rotate player when the player is moving
            if (_input.move != Vector2.zero)
            {
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                  _mainCamera.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                    RotationSmoothTime);

                // rotate to face input direction relative to camera position
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }

            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

            // move the player
            _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
                             new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

            // update animator if using character
            if (_speed > 0f)
            {
                anturaAnimation.State = AnturaAnimationStates.walking;
                anturaAnimation.WalkingSpeed = isSprinting ? Mathf.Lerp(0, 1, _speed * 1f) : 0f;
            }
            else
            {
                anturaAnimation.State = AnturaAnimationStates.idle;
            }

            // if (_hasAnimator)
            // {
            //     _animator.SetFloat(_animIDSpeed, _animationBlend);
            //     _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
            // }
        }
        bool inAir;
        bool justJumped;
        private void JumpAndGravity()
        {
            if (Grounded)
            {
                anturaAnimation.animator.speed = 1f;
                if (inAir)
                {
                    inAir = false;
                    justJumped = false;
                    AudioManager.I.PlaySound(Sfx.BushRustlingIn);
                }
            }
            else
            {
                anturaAnimation.animator.speed = 2f;
                inAir = true;
            }

            if (Grounded)
            {
                if (_nCurrentJump > 0)
                {
                    _nCurrentJump = 0;
                    anturaAnimation.OnJumpEnded();
                }

                // reset the fall timeout timer
                _fallTimeoutDelta = FallTimeout;

                // update animator if using character
                // if (_hasAnimator)
                // {
                //     _animator.SetBool(_animIDJump, false);
                //     _animator.SetBool(_animIDFreeFall, false);
                // }

                // stop our velocity dropping infinitely when grounded
                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }

                // Jumps
                if (_input.jump && _jumpTimeoutDelta <= 0.0f)
                {
                    // the square root of H * -2 * G = how much velocity needed to reach desired height
                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
                    _nCurrentJump = 1;
                    _midAirJumpTimeoutDelta = JumpTimeout;
                    anturaAnimation.OnJumpStart();

                    // update animator if using character
                    // if (_hasAnimator)
                    // {
                    //     _animator.SetBool(_animIDJump, true);
                    // }
                }

                // jump timeout
                if (_jumpTimeoutDelta >= 0.0f)
                {
                    _jumpTimeoutDelta -= Time.deltaTime;
                }

                if (_verticalVelocity > 0.0f && !justJumped)
                {
                    justJumped = true;
                    AudioManager.I.PlaySound(Sfx.CatMeow);
                }
            }
            else
            {
                // reset the jump timeout timer
                _jumpTimeoutDelta = JumpTimeout;

                // fall timeout
                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    // update animator if using character
                    // if (_hasAnimator)
                    // {
                    //     _animator.SetBool(_animIDFreeFall, true);
                    // }
                }

                if (_nCurrentJump > 0 && _nCurrentJump < MaxJumps && _input.jump && _midAirJumpTimeoutDelta <= 0.0f)
                {
                    // Double-jump
                    _verticalVelocity = Mathf.Sqrt(MidAirJumpHeight * -2f * Gravity);
                    _nCurrentJump++;
                }
                else
                {
                    // if we are not grounded, do not jump
                    _input.jump = false;
                }

                if (_midAirJumpTimeoutDelta >= 0.0f)
                {
                    _midAirJumpTimeoutDelta -= Time.deltaTime;
                }
            }

            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * Time.deltaTime;
            }
        }

        public void SpawnToNewLocation(Transform newLocation)
        {
            // TODO MAYBE A TRANSITION?
            transform.position = newLocation.position;
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f)
                lfAngle += 360f;
            if (lfAngle > 360f)
                lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (Grounded)
                Gizmos.color = transparentGreen;
            else
                Gizmos.color = transparentRed;

            // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
            Gizmos.DrawSphere(
                new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
                GroundedRadius);
        }

    }
}
