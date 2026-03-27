using UnityEngine;

namespace Antura.Discover
{
    public class AutoAnimate : MonoBehaviour
    {
        public enum MovementCycle { Loop, PingPong, Once }
        public enum Axis { X, Y, Z }

        public float someRandomness = 0.0f;

        [Header("Axis-based Movement")]
        public bool enableAxisMovement = false;
        public Axis moveAxis = Axis.Y;
        public float axisDistance = 2.0f;
        public MovementCycle axisCycle = MovementCycle.Loop;
        public float axisSpeed = 1f;

        [Header("Target Point Movement")]
        public bool enableTargetMovement = false;
        public Transform targetPoint;
        public MovementCycle targetCycle = MovementCycle.Loop;
        public float targetSpeed = 1f;

        [Header("Bobbing Movement")]
        public bool enableBobbing = false;
        public float bobbingAmount = 0.05f;
        public float bobbingSpeed = 1f;

        [Header("Rotation")]
        public bool enableRotation = false;
        public float rotationSpeed = 100f;

        [Header("General")]
        public float pauseDelay = 1f; // Not yet integrated; add states for pause if desired

        private Vector3 startPosition;
        private float axisTimer;
        private float targetTimer;
        private float bobbingTimer;

        void Start()
        {
            startPosition = transform.position;

            if (someRandomness > 0.0f)
            {
                float randMult = Random.Range(1.0f - someRandomness, 1.0f + someRandomness);
                axisSpeed *= randMult;
                targetSpeed *= randMult;
                bobbingSpeed *= randMult;
                bobbingAmount *= randMult;
                rotationSpeed *= randMult;
            }

            axisTimer = 0f;
            targetTimer = 0f;
            bobbingTimer = 0f;
        }

        void Update()
        {
            Vector3 nextPosition = startPosition;

            // Axis-based movement (independent)
            if (enableAxisMovement)
            {
                axisTimer += Time.deltaTime * axisSpeed;
                float travel = GetTravelDistance(axisCycle, axisTimer, axisDistance);
                Vector3 axisDir = GetAxisVector(moveAxis);
                nextPosition += axisDir * travel;
            }

            // Target point movement (independent; dynamic target support)
            if (enableTargetMovement && targetPoint != null)
            {
                Vector3 endPosition = targetPoint.position;
                Vector3 direction = endPosition - startPosition;
                float dist = direction.magnitude;
                if (dist > Mathf.Epsilon)
                {
                    direction = direction.normalized;
                    targetTimer += Time.deltaTime * targetSpeed;
                    float travel = GetTravelDistance(targetCycle, targetTimer, dist);
                    nextPosition += direction * travel;
                }
            }

            // Bobbing movement (independent, cumulative on Y-axis)
            if (enableBobbing && bobbingSpeed > Mathf.Epsilon && bobbingAmount > Mathf.Epsilon)
            {
                bobbingTimer += Time.deltaTime * bobbingSpeed;
                float bobOffset = Mathf.Sin(bobbingTimer) * bobbingAmount;
                nextPosition += Vector3.up * bobOffset;
            }

            transform.position = nextPosition;

            // Rotation (independent)
            if (enableRotation)
            {
                transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
            }
        }

        public void EnableTargetMovement(bool enable)
        {
            enableTargetMovement = enable;
            enableRotation = false;
            if (!enable)
            {
                targetTimer = 0f; // Reset timer to avoid jumps when re-enabled
            }
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

        private static float GetTravelDistance(MovementCycle cycle, float timer, float distance)
        {
            float magnitude = Mathf.Abs(distance);
            if (magnitude <= Mathf.Epsilon)
            {
                return 0f;
            }

            float travel;
            switch (cycle)
            {
                case MovementCycle.PingPong:
                    travel = Mathf.PingPong(timer, magnitude);
                    break;
                case MovementCycle.Once:
                    travel = Mathf.Clamp(timer, 0f, magnitude);
                    break;
                case MovementCycle.Loop:
                    travel = Mathf.Repeat(timer, magnitude);
                    break;
                default:
                    travel = 0f;
                    break;
            }

            return Mathf.Sign(distance) * travel;
        }

        void OnDrawGizmos()
        {
            if (!Application.isPlaying)
                return;
            Gizmos.color = Color.yellow;
            if (enableAxisMovement)
            {
                Vector3 end = startPosition + GetAxisVector(moveAxis) * axisDistance;
                Gizmos.DrawLine(transform.position, end);
            }
            if (enableTargetMovement && targetPoint)
            {
                Gizmos.DrawLine(transform.position, targetPoint.position);
            }
        }
    }
}
