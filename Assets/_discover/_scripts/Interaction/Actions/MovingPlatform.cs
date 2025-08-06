using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Antura.Discover
{
    public class MovingPlatform : MonoBehaviour
    {
        public enum Axis { X, Y, Z }
        public enum MovementType { Axis, TargetPoint }

        public bool IsActivated;
        public float speed = 1f;
        public float pauseDelay = 1f;
        public MovementType movementType = MovementType.Axis;
        [Header("For Axis movement")]
        public Axis MoveAxis = Axis.Y;
        public float distance = 2.0f;

        [Header("For TargetPoint movement")]
        public Transform targetPoint;

        private float currentPause = 0f;
        private Vector3 startingPosition;
        private Vector3 endingPosition;
        private bool movingPositive = true;
        private float moveTimer = 0f;

        void Start()
        {
            startingPosition = transform.position;
            if (movementType == MovementType.TargetPoint && targetPoint != null)
            {
                endingPosition = targetPoint.position;
            }
            else if (movementType == MovementType.Axis)
            {
                switch (MoveAxis)
                {
                    case Axis.X:
                        endingPosition = startingPosition + Vector3.right * distance;
                        break;
                    case Axis.Y:
                        endingPosition = startingPosition + Vector3.up * distance;
                        break;
                    case Axis.Z:
                        endingPosition = startingPosition + Vector3.forward * distance;
                        break;
                }
            }
            moveTimer = 0f;
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

            if (movementType == MovementType.Axis)
            {
                Vector3 direction = Vector3.zero;
                switch (MoveAxis)
                {
                    case Axis.X:
                        direction = Vector3.right;
                        break;
                    case Axis.Y:
                        direction = Vector3.up;
                        break;
                    case Axis.Z:
                        direction = Vector3.forward;
                        break;
                }

                float sign = movingPositive ? 1f : -1f;
                transform.position += direction * sign * speed * Time.deltaTime;

                float moved = 0f;
                switch (MoveAxis)
                {
                    case Axis.X:
                        moved = transform.position.x - startingPosition.x;
                        break;
                    case Axis.Y:
                        moved = transform.position.y - startingPosition.y;
                        break;
                    case Axis.Z:
                        moved = transform.position.z - startingPosition.z;
                        break;
                }

                if (movingPositive && moved >= distance)
                {
                    movingPositive = false;
                    currentPause = pauseDelay;
                }
                else if (!movingPositive && moved <= 0f)
                {
                    movingPositive = true;
                    currentPause = pauseDelay;
                }
            }
            else if (movementType == MovementType.TargetPoint && targetPoint != null)
            {
                float totalDistance = Vector3.Distance(startingPosition, endingPosition);
                float duration = totalDistance / Mathf.Max(speed, 0.01f); // Avoid div by zero

                // Move timer from 0 to duration
                moveTimer += Time.deltaTime * (movingPositive ? 1f : -1f);
                float t = Mathf.Clamp01(moveTimer / duration);

                // Lerp between start and end
                transform.position = Vector3.Lerp(startingPosition, endingPosition, t);

                // Check for end points and handle pause/reverse
                if (movingPositive && t >= 1f)
                {
                    moveTimer = duration;
                    movingPositive = false;
                    currentPause = pauseDelay;
                }
                else if (!movingPositive && t <= 0f)
                {
                    moveTimer = 0f;
                    movingPositive = true;
                    currentPause = pauseDelay;
                }
            }
        }

        public void Activate(bool status)
        {
            IsActivated = status;
        }
    }
}
