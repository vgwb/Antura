using Antura.Dog;
using Antura.Helpers;
using System;
using UnityEngine;

namespace Antura.AnturaSpace
{
    public class AnturaLocomotion : MonoBehaviour
    {
        Transform target;
        bool rotateAsTarget;
        Transform rotatingBase;

        float runningTime;
        bool isSliping;

        const float WALK_SPEED = 5.0f;
        const float RUN_SPEED = 15.0f;

        public event Action onTouched;

        [NonSerialized]
        public AnturaAnimationController AnimationController;

        Vector3 lastVelocity;
        Vector3 lastPosition;

        public bool HasReachedTarget
        {
            get { return !isSliping && IsNearTargetPosition && IsNearTargetRotation; }
        }

        bool wasNearPosition;

        public bool IsNearTargetPosition
        {
            get
            {
                if (target == null)
                {
                    return true;
                }

                var distance = target.position - transform.position;
                distance.y = 0;

                return distance.magnitude < (wasNearPosition ? 1.0f : 0.5f);
            }
        }

        bool IsNearTargetRotation
        {
            get
            {
                if (target == null || !rotateAsTarget)
                {
                    return true;
                }

                var dot = Mathf.Max(0, Vector3.Dot(target.forward.normalized, transform.forward.normalized));
                return dot > 0.9f;
            }
        }

        public bool IsSliping
        {
            get { return isSliping; }
        }

        public bool IsSleeping
        {
            get { return AnimationController.State == AnturaAnimationStates.sleeping; }
        }

        public bool IsJumping
        {
            get { return AnimationController.IsJumping || AnimationController.IsAnimationActuallyJumping; }
        }

        public bool Excited;

        public float PlanarDistanceFromTarget
        {
            get
            {
                if (target == null)
                {
                    return 0;
                }

                var distance = target.position - transform.position;
                distance.y = 0;

                return distance.magnitude;
            }
        }

        public float DistanceFromTarget
        {
            get
            {
                if (target == null)
                {
                    return 0;
                }

                var distance = target.position - transform.position;

                return distance.magnitude;
            }
        }

        public float TargetHeight
        {
            get
            {
                if (target == null)
                {
                    return 0;
                }

                return target.position.y;
            }
        }

        public void SetTarget(Transform target, bool rotateAsTarget, Transform rotatingBase = null)
        {
            this.target = target;
            this.rotateAsTarget = rotateAsTarget;
            this.rotatingBase = rotatingBase;

            if (rotatingBase == null)
            {
                transform.SetParent(null);
            }
        }


        void Awake()
        {
            AnimationController = GetComponent<AnturaAnimationController>();
        }

        void Update()
        {
            if (isSliping)
            {
                transform.position += lastVelocity * Time.deltaTime;

                var velMagnitude = lastVelocity.magnitude;

                if (velMagnitude > 1)
                {
                    lastVelocity -= 10 * lastVelocity.normalized * Time.deltaTime;
                }
                else
                {
                    lastVelocity = Vector3.Lerp(lastVelocity, Vector3.zero, 4 * Time.deltaTime);
                }

                if (lastVelocity.magnitude < 0.2f)
                {
                    isSliping = false;
                    runningTime = 0;
                    AnimationController.OnSlipEnded();
                }

                return;
            }

            if (!IsSleeping && !IsJumping && target != null)
            {
                var distance = target.position - transform.position;
                distance.y = 0;

                var distMagnitude = distance.magnitude;
                float speed = 0;

                if (!IsNearTargetPosition)
                {
                    wasNearPosition = false;
                    float speedFactor = Mathf.Lerp(0, 1, distMagnitude / 10);
                    speed = Mathf.Lerp(WALK_SPEED, RUN_SPEED, speedFactor) * Mathf.Lerp(0, 1, distMagnitude);
                    AnimationController.SetWalkingSpeed(speedFactor);

                    if (speedFactor > 0.75f)
                    {
                        runningTime += Time.deltaTime;
                    }
                    else
                    {
                        if (runningTime > 1.3f && Excited)
                        {
                            // Slip!
                            runningTime = 0;
                            isSliping = true;
                            AnimationController.OnSlipStarted();
                            Update();
                            return;
                        }

                        runningTime = 0;
                    }
                }
                else
                {
                    wasNearPosition = true;
                }

                if (speed > 0.05f)
                {
                    AnimationController.State = AnturaAnimationStates.walking;

                    if (AnimationController.IsAnimationActuallyWalking)
                    {
                        distance.Normalize();

                        var steeringMovement = transform.forward * speed * Time.deltaTime;
                        var normalMovement = distance * Mathf.Abs(Vector3.Dot(distance, transform.forward)) * speed * Time.deltaTime;

                        transform.position += Vector3.Lerp(steeringMovement, normalMovement,
                            10.0f * Vector3.Dot(transform.forward.normalized, distance.normalized));

                        GameplayHelper.LerpLookAtPlanar(transform, target.position, Time.deltaTime * 2);
                    }
                }
                else
                {
                    var dot = Mathf.Max(0, Vector3.Dot(target.forward.normalized, transform.forward.normalized));

                    if (rotatingBase)
                    {
                        Quaternion targetRotation;

                        transform.SetParent(rotatingBase);

                        if (rotateAsTarget)
                        {
                            targetRotation = target.rotation * rotatingBase.rotation;
                        }
                        else
                        {
                            targetRotation = rotatingBase.rotation;
                        }
                        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 4);
                    }
                    else
                    {
                        if (rotateAsTarget)
                        {
                            transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation,
                                Time.deltaTime * 4 * (0.2f + 0.8f * dot));
                        }
                        if ((!rotateAsTarget || dot > 0.9f) && AnimationController.State == AnturaAnimationStates.walking)
                        {
                            AnimationController.State = AnturaAnimationStates.idle;
                        }
                    }
                }
            }
            lastVelocity = (transform.position - lastPosition) / Time.deltaTime;
            lastPosition = transform.position;
        }

        void OnMouseDown()
        {
            if (onTouched != null)
            {
                onTouched();
            }
        }

        public void BoneSmell()
        {
            if (onTouched != null)
            {
                onTouched();
            }
        }
    }
}
