using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Discover
{
    public enum RotationAxis { X, Y, Z }

    public class RotatingPlatform : MonoBehaviour
    {
        public bool IsActivated = true;
        [Tooltip("Speed in degrees per second")]
        public float speed = 90f; // degrees per second
        [Tooltip("Pause duration after each rotation cycle")]
        public float pauseDelay = 1f;
        public RotationAxis rotationAxis = RotationAxis.Y;
        public bool pingPong = false;
        public float angle = 90f; // max angle for pingpong

        private float currentPause = 0f;
        private float rotationTimer = 0f;
        private bool rotatingPositive = true;
        private float startAngle;
        private Vector3 axisVector;

        void Start()
        {
            switch (rotationAxis)
            {
                case RotationAxis.X:
                    axisVector = Vector3.right;
                    break;
                case RotationAxis.Y:
                    axisVector = Vector3.up;
                    break;
                case RotationAxis.Z:
                    axisVector = Vector3.forward;
                    break;
            }
            startAngle = GetCurrentAngle();
            rotationTimer = 0f;
        }

        void Update()
        {
            if (!IsActivated)
                return;

            if (currentPause > 0f)
            {
                currentPause -= Time.deltaTime;
                return;
            }

            if (pingPong)
            {
                float duration = angle / Mathf.Max(speed, 0.01f);
                rotationTimer += Time.deltaTime * (rotatingPositive ? 1f : -1f);
                float t = Mathf.Clamp01(rotationTimer / duration);
                float targetAngle = Mathf.Lerp(startAngle, startAngle + angle, t);

                ApplyRotation(targetAngle);

                if (rotatingPositive && t >= 1f)
                {
                    rotationTimer = duration;
                    rotatingPositive = false;
                    currentPause = pauseDelay;
                }
                else if (!rotatingPositive && t <= 0f)
                {
                    rotationTimer = 0f;
                    rotatingPositive = true;
                    currentPause = pauseDelay;
                }
            }
            else
            {
                float deltaAngle = speed * Time.deltaTime;
                transform.Rotate(axisVector, deltaAngle, Space.Self);
            }
        }

        float GetCurrentAngle()
        {
            switch (rotationAxis)
            {
                case RotationAxis.X:
                    return transform.localEulerAngles.x;
                case RotationAxis.Y:
                    return transform.localEulerAngles.y;
                case RotationAxis.Z:
                    return transform.localEulerAngles.z;
                default:
                    return 0f;
            }
        }

        void ApplyRotation(float targetAngle)
        {
            Vector3 euler = transform.localEulerAngles;
            switch (rotationAxis)
            {
                case RotationAxis.X:
                    euler.x = targetAngle;
                    break;
                case RotationAxis.Y:
                    euler.y = targetAngle;
                    break;
                case RotationAxis.Z:
                    euler.z = targetAngle;
                    break;
            }
            transform.localEulerAngles = euler;
        }

        public void Activate(bool status)
        {
            IsActivated = status;
        }
    }
}
