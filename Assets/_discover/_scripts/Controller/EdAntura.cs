using Antura.Dog;
using DG.Tweening;
using System;
using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    public class EdAntura : MonoBehaviour
    {
        [Header("Movement")]
        public float MaxSpeed = 1f;
        public float RotationSpeed = 1f;
        private float Speed;
        //public float AccelerationWeight = 1f;
        //private float Acceleration = 0f;

        public float jumpStrength = 1f;
        public float jumpDeceleration = 1f;
        public float gravity = 10f;


        private AnturaAnimationController anturaAnimation => anturaPetSwitcher.AnimController;
        public AnturaPetSwitcher anturaPetSwitcher;

        public void Initialize()
        {
            Transform tr;
            (tr = anturaPetSwitcher.transform).SetParent(transform);
            tr.localEulerAngles = new Vector3(0f, 180f);
            tr.localScale = Vector3.one;
        }

        private Vector3 lastDesiredDir;
        private bool isJumping;
        private float jumpSpeed;
        void Update()
        {
            var tr = transform;
            var position = tr.position;
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            bool doJump = Input.GetKeyDown(KeyCode.Space);

            // var desiredDir = new Vector3(horizontalInput, 0, verticalInput);
            var desiredDir = CameraManager.I.CamController.CurrMovementVector;
            var accelerationMagnitude = desiredDir.magnitude;
            accelerationMagnitude = Mathf.Clamp01(accelerationMagnitude);
            if (desiredDir != Vector3.zero)
            {
                desiredDir.Normalize();
                lastDesiredDir = desiredDir;
            }

            var forwardDir = tr.forward;

            var angle = Vector3.SignedAngle(forwardDir, lastDesiredDir, Vector3.up);

            anturaAnimation.State = AnturaAnimationStates.walking;
            tr.Rotate(Vector3.up, angle * RotationSpeed * Time.deltaTime);
            //Acceleration = accelerationMagnitude;
            //Speed += Acceleration * accelerationMagnitude * AccelerationWeight * Time.deltaTime;
            //Speed = Mathf.Clamp(Speed, 0f, MaxSpeed);
            tr.transform.position += desiredDir * MaxSpeed * Time.deltaTime;

            Debug.DrawLine(position, position + desiredDir * accelerationMagnitude*10f, Color.red);
            Debug.DrawLine(position, position + tr.forward*10f, Color.yellow);

            if (doJump && !isJumping)
            {
                isJumping = true;
                jumpSpeed = jumpStrength;
                //anturaAnimation.OnJumpStart();
            }

            if (isJumping)
            {
                transform.position += Vector3.up * (jumpSpeed * Time.deltaTime - gravity * Time.deltaTime * Time.deltaTime);
                jumpSpeed -= Time.deltaTime * jumpDeceleration;
                if (transform.position.y < 0)
                {
                    transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
                    isJumping = false;
                    jumpSpeed = 0f;
                    //anturaAnimation.OnJumpEnded();
                }
            }


            if (!isJumping)
            {
                if (accelerationMagnitude > 0f)
                {
                    anturaAnimation.State = AnturaAnimationStates.walking;
                    anturaAnimation.WalkingSpeed = accelerationMagnitude;
                }
                else
                {
                    anturaAnimation.State = AnturaAnimationStates.idle;
                }
            }
        }

        public void PlayAnimation(AnturaAnimationStates anim)
        {
            anturaAnimation.State = anim;
        }

        public void GoTo(Transform posTr, Action callback = null)
        {
            anturaAnimation.State = AnturaAnimationStates.walking;
            Move(posTr.position, 1f, callback);
        }

        Tween moveTween;
        Action moveEndCallback;
        void Move(Vector3 position, float duration, Action callback)
        {
            moveTween?.Kill();
            moveEndCallback = callback;
            moveTween = transform.DOMove(position, duration).OnComplete(() =>
            {
                moveEndCallback?.Invoke();
            });
        }
    }
}
