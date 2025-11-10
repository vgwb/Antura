using UnityEngine;

namespace Antura.Discover
{
    public class AutoAnimate : MonoBehaviour
    {
        public enum MovementType { Loop, PingPong }
        public enum Axis { X, Y, Z }

        public float someRandomness = 0.0f;

        [Header("Movement Settings")]
        public bool enableMovement = false; // Enable or disable movement
        public float movementSpeed = 1f; // Movement speed in units per second
        public Axis MoveAxis = Axis.Y;
        public float distance = 2.0f;
        public MovementType movementType = MovementType.Loop;

        [Header("Rotation Settings")]
        public bool enableRotation = false; // Enable or disable rotation
        public float rotationSpeed = 100f; // Rotation speed in degrees per second

        [Header("Bobbing Settings")]
        public bool enableBobbing = false;
        public float bobbingAmount = 0.05f; // Amplitude of bobbing motion
        public float bobbingSpeed = 1f; // Speed of bobbing motion

        private Vector3 startPosition;
        private float bobbingTimer;
        private float movementTimer;

        void Start()
        {
            startPosition = transform.position;
            if (someRandomness > 0.0f)
            {
                bobbingAmount *= Random.Range(1.0f - someRandomness, 1.0f + someRandomness);
                bobbingSpeed *= Random.Range(1.0f - someRandomness, 1.0f + someRandomness);
                rotationSpeed *= Random.Range(1.0f - someRandomness, 1.0f + someRandomness);
                movementSpeed *= Random.Range(1.0f - someRandomness, 1.0f + someRandomness);
            }
        }

        void Update()
        {
            if (enableRotation)
            {
                // Rotate the object around its up axis
                transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
            }

            var nextPosition = startPosition;

            if (enableMovement)
            {
                movementTimer += Time.deltaTime * movementSpeed;
                float travel;
                if (movementType == MovementType.PingPong)
                {
                    travel = Mathf.PingPong(movementTimer, distance);
                }
                else
                {
                    travel = Mathf.Repeat(movementTimer, distance);
                }

                var axisVector = GetAxisVector(MoveAxis);
                nextPosition += axisVector * travel;
            }

            if (enableBobbing && bobbingSpeed > Mathf.Epsilon && bobbingAmount != 0f)
            {
                // Create a bobbing motion up and down
                bobbingTimer += Time.deltaTime * bobbingSpeed;
                float bobOffset = Mathf.Sin(bobbingTimer) * bobbingAmount;
                nextPosition.y += bobOffset;
            }

            transform.position = nextPosition;
        }

        private static Vector3 GetAxisVector(Axis axis)
        {
            switch (axis)
            {
                case Axis.X:
                    return Vector3.right;
                case Axis.Y:
                    return Vector3.up;
                case Axis.Z:
                    return Vector3.forward;
                default:
                    return Vector3.up;
            }
        }
    }
}
