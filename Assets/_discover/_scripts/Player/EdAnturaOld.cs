using Antura.Dog;
using DG.Tweening;
using System;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Antura.Minigames.DiscoverCountry
{
    // @note: DEPRECATED
    public class EdAnturaOld : MonoBehaviour
    {
        [Header("Movement")]
        public float MaxSpeed = 1f;
        public float WalkSpeed = 1f;
        public float RotationSpeed = 1f;

        [Header("Jump")]
        public float AnimSpeed = 3;
        public float JumpDelay = 0f;
        private float JumpDelayTimer = 0f;
        public float GroundTouchedThreshold = 0.1f;
        public int MaxJumps = 2;
        public float MultiJumpCooldown = 0f;
        private float MultiJumpCooldownTimer = 0f;

        public float JumpStrength = 1f;
        public float MultiJumpStrength = 1f;
        public float WallJumpStrength = 1f;

        public float WallAttachDistance = 1f;
        //public float AccelerationWeight = 1f;
        //private float Acceleration = 0f;

        public float airGravity = 10f;
        public float wallGravity = 10f;

        public float AirControl = 1f;

        public float airDrag = 10f;
        public float groundDrag = 10f;

        private AnturaAnimationController anturaAnimation => anturaPetSwitcher.AnimController;
        public AnturaPetSwitcher anturaPetSwitcher;

        public void Initialize()
        {
            Transform tr;
            (tr = anturaPetSwitcher.transform).SetParent(transform);
            tr.localEulerAngles = new Vector3(0f, 180f);
            tr.localScale = Vector3.one;
        }

        private Vector3 lastMoveDir;
        private Vector3 actualMoveDir;
        private bool isInAir;
        private int nCurrentJump;

        private bool runOn;
        private bool jumpTriggered;
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                jumpTriggered = true;
            runOn = Input.GetKey(KeyCode.LeftShift);
        }

        void FixedUpdate()
        {
            var tr = transform;
            var position = tr.position;
            //float horizontalInput = Input.GetAxis("Horizontal");
            //float verticalInput = Input.GetAxis("Vertical");
            var rb = GetComponent<Rigidbody>();

            if (runOn || isInAir)
                anturaAnimation.animator.speed = AnimSpeed;
            else
                anturaAnimation.animator.speed = 1;

            var groundDistance = Mathf.Infinity;
            var fromPoint = tr.position + tr.GetComponent<SphereCollider>().center;
            if (Physics.Raycast(fromPoint, Vector3.down, out var hitInfo, 100))
            {
                Debug.DrawLine(fromPoint, hitInfo.point, Color.green);
                //Debug.Log("Ground dist: " + hitInfo.distance);
                groundDistance = hitInfo.distance - tr.GetComponent<SphereCollider>().center.y;
            }
            else
            {
                Debug.DrawLine(fromPoint, Vector3.down * 10, Color.yellow);
            }

            var wallDistance = Mathf.Infinity;
            var wallNormal = Vector3.zero;
            bool attachedToWall = false;
            if (isInAir && Physics.Raycast(fromPoint, tr.forward, out hitInfo, WallAttachDistance))
            {
                Debug.DrawLine(tr.position, hitInfo.point, Color.blue);
                //Debug.Log("Fwd dist: " + hitInfo.distance);
                wallDistance = hitInfo.distance;
                wallNormal = hitInfo.normal;
                attachedToWall = true;
                rb.velocity = Vector3.zero;
                actualMoveDir = Vector3.zero;
                //Debug.LogError("WALL HIT");
            }


            // Landing
            if (!isInAir && groundDistance > GroundTouchedThreshold)
            {
                isInAir = true;
            }

            if (isInAir)
            {
                if ((groundDistance <= GroundTouchedThreshold || tr.position.y <= 0)
                    && (rb.velocity.y <= 0f))
                {
                    if (tr.position.y < 0)
                        tr.position = new Vector3(tr.position.x, 0f, tr.position.z);
                    isInAir = false;
                    nCurrentJump = 0;
                    anturaAnimation.OnJumpEnded();
                    //Debug.LogError("JUMP ENDED");
                }
            }


            bool prepareNextJump = false;
            if (jumpTriggered)
            {
                jumpTriggered = false;
                if (nCurrentJump == 0)
                {
                    JumpDelayTimer = JumpDelay;
                    anturaAnimation.OnJumpStart();
                    prepareNextJump = true;
                    //Debug.LogError("FIRST JUMP PREPARED");
                }
                else if (attachedToWall)
                {
                    anturaAnimation.OnJumpStart();
                    prepareNextJump = true;
                }
                else
                {
                    anturaAnimation.State = AnturaAnimationStates.walking;
                    anturaAnimation.OnJumpStart();
                    prepareNextJump = true;
                }
            }

            bool doJump = false;
            if (JumpDelayTimer > 0f || prepareNextJump)
            {
                JumpDelayTimer -= Time.fixedDeltaTime;
                doJump = JumpDelayTimer <= 0f;
                if (doJump)
                    JumpDelayTimer = 0f;
            }

            if (MultiJumpCooldownTimer > 0f)
            {
                MultiJumpCooldownTimer -= Time.deltaTime;
            }


            if (doJump && nCurrentJump < MaxJumps)
            {
                if (nCurrentJump == 0)
                {
                    nCurrentJump++;
                    isInAir = true;
                    rb.AddForce(Vector3.up * JumpStrength, ForceMode.Impulse);
                    MultiJumpCooldownTimer = MultiJumpCooldown; // Start the cooldown
                }
                else if (nCurrentJump < MaxJumps)
                {
                    if (attachedToWall)
                    {
                        // Not adding to the additional jump
                        Debug.LogError("WALL JUMP");
                        var wallJumpDir = Vector3.Lerp(Vector3.up, wallNormal, 0.5f);
                        Debug.DrawLine(tr.position, tr.position + wallJumpDir, Color.green, 10f);
                        rb.velocity = Vector3.zero;
                        rb.AddForce(wallJumpDir * WallJumpStrength, ForceMode.Impulse);
                    }
                    else if (MultiJumpCooldownTimer <= 0f)
                    {
                        nCurrentJump++;
                        rb.AddForce(Vector3.up * MultiJumpStrength, ForceMode.Impulse);
                    }
                }
            }

            var desiredMoveDir = InputManager.CurrWorldMovementVector;
            actualMoveDir = Vector3.Lerp(actualMoveDir, desiredMoveDir, isInAir ? AirControl : 1f);

            var accelerationMagnitude = actualMoveDir.magnitude;
            accelerationMagnitude = Mathf.Clamp01(accelerationMagnitude);
            if (actualMoveDir != Vector3.zero)
            {
                actualMoveDir.Normalize();
                lastMoveDir = actualMoveDir;
            }

            var forwardDir = tr.forward;

            var angle = Vector3.SignedAngle(forwardDir, lastMoveDir, Vector3.up);

            anturaAnimation.State = AnturaAnimationStates.walking;
            if (!isInAir)
            {
                tr.Rotate(Vector3.up, angle * RotationSpeed * Time.fixedDeltaTime);
            }
            var moveDelta = actualMoveDir * (runOn ? MaxSpeed : WalkSpeed);
            rb.AddForce(moveDelta, ForceMode.Acceleration);

            Debug.DrawLine(position, position + desiredMoveDir * accelerationMagnitude * 10f, Color.red);
            Debug.DrawLine(position, position + actualMoveDir * accelerationMagnitude * 10f, Color.magenta);
            Debug.DrawLine(position, position + tr.forward * 10f, Color.yellow);

            if (attachedToWall)
            {
                rb.AddForce(Vector3.up * wallGravity, ForceMode.Acceleration);
            }
            else if (isInAir)
            {
                rb.AddForce(Vector3.up * airGravity, ForceMode.Acceleration);
            }

            if (!isInAir)
            {
                if (accelerationMagnitude > 0f)
                {
                    anturaAnimation.State = AnturaAnimationStates.walking;
                    anturaAnimation.WalkingSpeed = Mathf.Lerp(0, 1, rb.velocity.magnitude / (MaxSpeed * 0.2f));
                }
                else
                {
                    anturaAnimation.State = AnturaAnimationStates.idle;
                }
            }

            if (isInAir)
            {
                rb.velocity = new Vector3(rb.velocity.x - rb.velocity.x * airDrag * Time.fixedDeltaTime,
                    rb.velocity.y,
                    rb.velocity.z - rb.velocity.z * airDrag * Time.fixedDeltaTime);
            }
            else
            {
                rb.velocity = rb.velocity - rb.velocity * groundDrag * Time.fixedDeltaTime;
            }

        }

        public void PlayAnimation(AnturaAnimationStates anim)
        {
            anturaAnimation.State = anim;
        }
    }
}
