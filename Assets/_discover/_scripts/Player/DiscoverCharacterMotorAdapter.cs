using System;
using System.Collections.Generic;
using KinematicCharacterController;
using UnityEngine;

namespace Antura.Discover
{
    public enum DiscoverOrientationMethod
    {
        TowardsCamera,
        TowardsMovement,
    }

    public struct DiscoverCharacterInputs
    {
        public float MoveAxisForward;
        public float MoveAxisRight;
        public Quaternion CameraRotation;
        public bool JumpDown;
        public bool JumpHeld;
        public bool CrouchDown;
        public bool CrouchUp;
    }

    /// <summary>
    /// Adapter around the Kinematic Character Controller motor that exposes a simplified input API for the Discover player.
    /// Based on ExampleCharacterController from the KCC package, trimmed to a single movement state and with runtime-configurable max speed.
    /// </summary>
    [DisallowMultipleComponent]
    [DefaultExecutionOrder(-90)]
    [RequireComponent(typeof(KinematicCharacterMotor))]
    public class DiscoverCharacterMotorAdapter : MonoBehaviour, ICharacterController
    {
        [Header("Stable Movement")]
        public float StableMovementSharpness = 15f;
        public float OrientationSharpness = 10f;
        public DiscoverOrientationMethod OrientationMethod = DiscoverOrientationMethod.TowardsMovement;
        public float DefaultMaxStableMoveSpeed = 4f;

        [Header("Air Movement")]
        public float DefaultMaxAirMoveSpeed = 6f;
        public float AirAccelerationSpeed = 15f;
        public float Drag = 0.1f;

        [Header("Jumping")]
        public bool AllowJumpingWhenSliding = false;
        public float JumpUpSpeed = 10f;
        public float JumpScalableForwardSpeed = 10f;
        public float JumpPreGroundingGraceTime = 0.1f;
        public float JumpPostGroundingGraceTime = 0.2f;

        [Header("Misc")]
        public List<Collider> IgnoredColliders = new List<Collider>();
        public float Gravity = -30f;
        public Transform MeshRoot;
        public Transform CameraFollowPoint;
        public float CrouchedCapsuleHeight = 1f;
        public float MaxBonusOrientationSharpness = 10f;

        public KinematicCharacterMotor Motor { get; private set; }

        public float DesiredMaxStableMoveSpeed { get; set; }
        public float DesiredMaxAirMoveSpeed { get; set; }

        public Vector3 Velocity => Motor != null ? Motor.Velocity : Vector3.zero;
        public bool IsStableOnGround => Motor != null && Motor.GroundingStatus.IsStableOnGround;
        public Vector3 GroundNormal => Motor != null && Motor.GroundingStatus.FoundAnyGround
            ? Motor.GroundingStatus.GroundNormal
            : Vector3.up;
        public Collider GroundCollider => Motor != null ? Motor.GroundingStatus.GroundCollider : null;
        public bool JumpedThisFrame => _jumpedThisFrame;
        public bool Crouching => _isCrouching;

        public float StandingCapsuleHeight => _standingCapsuleHeight;
        public float CapsuleRadius => _standingCapsuleRadius;

        public event Action Jumped;
        public event Action Landed;
        public event Action<Collider, Vector3, Vector3, Vector3> MovementHit;

        private Vector3 _moveInputVector;
        private Vector3 _lookInputVector;
        private bool _jumpRequested;
        private bool _jumpConsumed;
        private bool _jumpedThisFrame;
        private float _timeSinceJumpRequested = Mathf.Infinity;
        private float _timeSinceLastAbleToJump;
        private Vector3 _internalVelocityAdd;
        private bool _shouldBeCrouching;
        private bool _isCrouching;
        private bool _wasOnGroundLastFrame;
        private float _standingCapsuleHeight;
        private float _standingCapsuleRadius;
        private readonly Collider[] _probedColliders = new Collider[8];
        private Vector3 _lastNonZeroLookDirection = Vector3.forward;

        private void Awake()
        {
            Motor = GetComponent<KinematicCharacterMotor>();
            DesiredMaxStableMoveSpeed = DefaultMaxStableMoveSpeed;
            DesiredMaxAirMoveSpeed = DefaultMaxAirMoveSpeed;
            if (Motor != null)
            {
                Motor.CharacterController = this;
                _standingCapsuleHeight = Motor.Capsule.height;
                _standingCapsuleRadius = Motor.Capsule.radius;
                _lastNonZeroLookDirection = Motor.CharacterForward;
            }
            else
            {
                _lastNonZeroLookDirection = transform.forward;
            }
        }

        public void ApplyCharacterInputs(in DiscoverCharacterInputs inputs, float targetMoveSpeed)
        {
            DesiredMaxStableMoveSpeed = Mathf.Max(0f, targetMoveSpeed);
            DesiredMaxAirMoveSpeed = Mathf.Max(DefaultMaxAirMoveSpeed, DesiredMaxStableMoveSpeed);

            var localInputs = inputs;
            SetInputs(ref localInputs);
        }

        public void TeleportTo(Vector3 position, Quaternion rotation)
        {
            if (Motor == null)
            {
                return;
            }

            Motor.SetPositionAndRotation(position, rotation, true);
        }

        public void AddVelocity(Vector3 velocity)
        {
            _internalVelocityAdd += velocity;
        }

        public void ForceUnground()
        {
            Motor?.ForceUnground();
        }

        public void SetInputs(ref DiscoverCharacterInputs inputs)
        {
            // Clamp input
            Vector3 moveInputVector = Vector3.ClampMagnitude(new Vector3(inputs.MoveAxisRight, 0f, inputs.MoveAxisForward), 1f);

            // Calculate camera direction and rotation on the character plane
            Vector3 cameraPlanarDirection = Vector3.ProjectOnPlane(inputs.CameraRotation * Vector3.forward, Motor.CharacterUp).normalized;
            if (cameraPlanarDirection.sqrMagnitude == 0f)
            {
                cameraPlanarDirection = Vector3.ProjectOnPlane(inputs.CameraRotation * Vector3.up, Motor.CharacterUp).normalized;
            }
            Quaternion cameraPlanarRotation = Quaternion.LookRotation(cameraPlanarDirection, Motor.CharacterUp);

            // Move and look inputs
            _moveInputVector = cameraPlanarRotation * moveInputVector;

            switch (OrientationMethod)
            {
                case DiscoverOrientationMethod.TowardsCamera:
                    _lookInputVector = cameraPlanarDirection;
                    break;
                case DiscoverOrientationMethod.TowardsMovement:
                    if (_moveInputVector.sqrMagnitude > 0.0001f)
                    {
                        _lookInputVector = _moveInputVector.normalized;
                        _lastNonZeroLookDirection = _lookInputVector;
                    }
                    else
                    {
                        _lookInputVector = _lastNonZeroLookDirection;
                    }
                    break;
            }

            // Jumping
            if (inputs.JumpDown)
            {
                _timeSinceJumpRequested = 0f;
                _jumpRequested = true;
            }
            else if (!inputs.JumpHeld && _jumpRequested && !_jumpConsumed)
            {
                _jumpRequested = false;
                _timeSinceJumpRequested = Mathf.Infinity;
            }

            // Crouching
            if (inputs.CrouchDown)
            {
                _shouldBeCrouching = true;

                if (!_isCrouching)
                {
                    _isCrouching = true;
                    Motor.SetCapsuleDimensions(0.5f, CrouchedCapsuleHeight, CrouchedCapsuleHeight * 0.5f);
                    if (MeshRoot != null)
                    {
                        MeshRoot.localScale = new Vector3(1f, 0.5f, 1f);
                    }
                }
            }
            else if (inputs.CrouchUp)
            {
                _shouldBeCrouching = false;
            }
        }

        public void BeforeCharacterUpdate(float deltaTime)
        {
        }

        public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
        {
            if (_lookInputVector.sqrMagnitude > 0f && OrientationSharpness > 0f)
            {
                Vector3 smoothedLookInputDirection = Vector3.Slerp(Motor.CharacterForward, _lookInputVector, 1f - Mathf.Exp(-OrientationSharpness * deltaTime)).normalized;
                currentRotation = Quaternion.LookRotation(smoothedLookInputDirection, Motor.CharacterUp);
            }

            Vector3 currentUp = currentRotation * Vector3.up;
            Vector3 smoothedGravityDir = Vector3.Slerp(currentUp, Vector3.up, 1 - Mathf.Exp(-MaxBonusOrientationSharpness * deltaTime));
            currentRotation = Quaternion.FromToRotation(currentUp, smoothedGravityDir) * currentRotation;
        }

        public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            float maxStableSpeed = DesiredMaxStableMoveSpeed > 0f ? DesiredMaxStableMoveSpeed : DefaultMaxStableMoveSpeed;
            float maxAirSpeed = DesiredMaxAirMoveSpeed > 0f ? DesiredMaxAirMoveSpeed : DefaultMaxAirMoveSpeed;

            // Ground movement
            if (Motor.GroundingStatus.IsStableOnGround)
            {
                _timeSinceLastAbleToJump = 0f;

                float currentVelocityMagnitude = currentVelocity.magnitude;
                Vector3 effectiveGroundNormal = Motor.GroundingStatus.GroundNormal;

                currentVelocity = Motor.GetDirectionTangentToSurface(currentVelocity, effectiveGroundNormal) * currentVelocityMagnitude;

                Vector3 inputRight = Vector3.Cross(_moveInputVector, Motor.CharacterUp);
                Vector3 reorientedInput = Vector3.Cross(effectiveGroundNormal, inputRight).normalized * _moveInputVector.magnitude;
                Vector3 targetMovementVelocity = reorientedInput * maxStableSpeed;

                currentVelocity = Vector3.Lerp(currentVelocity, targetMovementVelocity, 1f - Mathf.Exp(-StableMovementSharpness * deltaTime));
                float targetMagnitude = Mathf.Max(targetMovementVelocity.magnitude, 0f);
                if (targetMagnitude > 0.0001f)
                {
                    float maxAllowed = Mathf.Max(maxStableSpeed, 0f);
                    currentVelocity = Vector3.ClampMagnitude(currentVelocity, maxAllowed);
                }
            }
            else // Air movement
            {
                _timeSinceLastAbleToJump += deltaTime;

                if (_moveInputVector.sqrMagnitude > 0f)
                {
                    Vector3 addedVelocity = _moveInputVector * AirAccelerationSpeed * deltaTime;
                    Vector3 currentVelocityOnInputsPlane = Vector3.ProjectOnPlane(currentVelocity, Motor.CharacterUp);

                    if (currentVelocityOnInputsPlane.magnitude < maxAirSpeed)
                    {
                        Vector3 newTotal = Vector3.ClampMagnitude(currentVelocityOnInputsPlane + addedVelocity, maxAirSpeed);
                        addedVelocity = newTotal - currentVelocityOnInputsPlane;
                    }
                    else
                    {
                        if (Vector3.Dot(currentVelocityOnInputsPlane, addedVelocity) > 0f)
                        {
                            addedVelocity = Vector3.ProjectOnPlane(addedVelocity, currentVelocityOnInputsPlane.normalized);
                        }
                    }

                    if (Motor.GroundingStatus.FoundAnyGround)
                    {
                        if (Vector3.Dot(currentVelocity + addedVelocity, addedVelocity) > 0f)
                        {
                            Vector3 perpendicularObstructionNormal = Vector3.Cross(Vector3.Cross(Motor.CharacterUp, Motor.GroundingStatus.GroundNormal), Motor.CharacterUp).normalized;
                            addedVelocity = Vector3.ProjectOnPlane(addedVelocity, perpendicularObstructionNormal);
                        }
                    }

                    currentVelocity += addedVelocity;
                }

                currentVelocity += Motor.CharacterUp * Gravity * deltaTime;
                currentVelocity *= (1f / (1f + (Drag * deltaTime)));

                Vector3 planarVelocity = Vector3.ProjectOnPlane(currentVelocity, Motor.CharacterUp);
                if (planarVelocity.magnitude > maxAirSpeed)
                {
                    Vector3 limitedPlanar = planarVelocity.normalized * maxAirSpeed;
                    Vector3 verticalComponent = currentVelocity - planarVelocity;
                    currentVelocity = limitedPlanar + verticalComponent;
                }
            }

            // Jumping
            _jumpedThisFrame = false;
            _timeSinceJumpRequested += deltaTime;
            if (_jumpRequested)
            {
                if (!_jumpConsumed && ((AllowJumpingWhenSliding ? Motor.GroundingStatus.FoundAnyGround : Motor.GroundingStatus.IsStableOnGround) || _timeSinceLastAbleToJump <= JumpPostGroundingGraceTime))
                {
                    Vector3 jumpDirection = Motor.CharacterUp;
                    if (Motor.GroundingStatus.FoundAnyGround && !Motor.GroundingStatus.IsStableOnGround)
                    {
                        jumpDirection = Motor.GroundingStatus.GroundNormal;
                    }

                    Motor.ForceUnground();

                    currentVelocity += (jumpDirection * JumpUpSpeed) - Vector3.Project(currentVelocity, Motor.CharacterUp);
                    currentVelocity += (_moveInputVector * JumpScalableForwardSpeed);
                    _jumpRequested = false;
                    _jumpConsumed = true;
                    _jumpedThisFrame = true;
                    Jumped?.Invoke();
                }
            }

            // Additive velocity
            if (_internalVelocityAdd.sqrMagnitude > 0f)
            {
                currentVelocity += _internalVelocityAdd;
                _internalVelocityAdd = Vector3.zero;
            }
        }

        public void AfterCharacterUpdate(float deltaTime)
        {
            if (_shouldBeCrouching && !_isCrouching)
            {
                _isCrouching = true;
                Motor.SetCapsuleDimensions(_standingCapsuleRadius, CrouchedCapsuleHeight, CrouchedCapsuleHeight * 0.5f);
                if (MeshRoot != null)
                {
                    MeshRoot.localScale = new Vector3(1f, 0.5f, 1f);
                }
            }
            else if (!_shouldBeCrouching && _isCrouching)
            {
                Motor.SetCapsuleDimensions(_standingCapsuleRadius, _standingCapsuleHeight, _standingCapsuleHeight * 0.5f);
                if (Motor.CharacterOverlap(
                        Motor.TransientPosition,
                        Motor.TransientRotation,
                        _probedColliders,
                        Motor.CollidableLayers,
                        QueryTriggerInteraction.Ignore) > 0)
                {
                    Motor.SetCapsuleDimensions(_standingCapsuleRadius, CrouchedCapsuleHeight, CrouchedCapsuleHeight * 0.5f);
                }
                else
                {
                    _isCrouching = false;
                    if (MeshRoot != null)
                    {
                        MeshRoot.localScale = Vector3.one;
                    }
                }
            }

            if (_jumpConsumed && _timeSinceJumpRequested > JumpPreGroundingGraceTime)
            {
                _jumpConsumed = false;
            }

            if (_jumpedThisFrame)
            {
                _jumpRequested = false;
            }

            if (!_wasOnGroundLastFrame && Motor.GroundingStatus.IsStableOnGround)
            {
                Landed?.Invoke();
            }
            _wasOnGroundLastFrame = Motor.GroundingStatus.IsStableOnGround;
        }

        public void PostGroundingUpdate(float deltaTime)
        {
            if (Motor.GroundingStatus.IsStableOnGround)
            {
                _timeSinceLastAbleToJump = 0f;
            }
            else
            {
                _timeSinceLastAbleToJump += deltaTime;
            }
        }

        public bool IsColliderValidForCollisions(Collider coll)
        {
            if (IgnoredColliders == null)
                return true;

            for (int i = 0; i < IgnoredColliders.Count; i++)
            {
                if (IgnoredColliders[i] == coll)
                    return false;
            }
            return true;
        }

        public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
        {
        }

        public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
        {
            MovementHit?.Invoke(hitCollider, hitNormal, hitPoint, Motor != null ? Motor.Velocity : Vector3.zero);
        }

        public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport)
        {
        }

        public void OnDiscreteCollisionDetected(Collider hitCollider)
        {
        }
    }
}
