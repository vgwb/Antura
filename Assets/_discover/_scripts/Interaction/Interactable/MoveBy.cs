using UnityEngine;

namespace Antura.Discover
{
    public class MoveBy : ActableAbstract
    {
        public enum Axis { X, Y, Z }
        public enum MovementType { Axis, TargetPoint }
        public enum TriggerMode { Single, Toggle }

        public bool IsActivated;
        public float speed = 1f;
        public MovementType movementType = MovementType.Axis;
        public TriggerMode triggerMode = TriggerMode.Single;

        [Header("For Axis movement")]
        public Axis MoveAxis = Axis.Y;
        public float distance = 2.0f;

        [Header("For TargetPoint movement")]
        public Transform targetPoint;

        Vector3 startingPosition;
        Vector3 endingPosition;
        Vector3 currentTargetPosition;
        bool isMoving;

        void Start()
        {
            startingPosition = transform.position;
            endingPosition = GetEndingPosition();

            if (IsActivated)
            {
                transform.position = endingPosition;
                currentTargetPosition = endingPosition;
            }
            else
            {
                currentTargetPosition = startingPosition;
            }
        }

        void Update()
        {
            if (!isMoving)
                return;

            transform.position = Vector3.MoveTowards(transform.position, currentTargetPosition, speed * Time.deltaTime);

            if (transform.position == currentTargetPosition)
            {
                isMoving = false;
                IsActivated = currentTargetPosition == endingPosition;
            }
        }

        public void Activate(bool status)
        {
            endingPosition = GetEndingPosition();

            if (!status && triggerMode == TriggerMode.Single)
                return;

            currentTargetPosition = status ? endingPosition : startingPosition;
            isMoving = true;
        }

        public override void OnTrigger()
        {
            if (isMoving)
                return;

            if (triggerMode == TriggerMode.Toggle)
            {
                Activate(!IsActivated);
                return;
            }

            if (!IsActivated)
            {
                Activate(true);
            }
        }

        Vector3 GetEndingPosition()
        {
            if (movementType == MovementType.TargetPoint && targetPoint != null)
                return targetPoint.position;

            Vector3 axisOffset = MoveAxis switch
            {
                Axis.X => Vector3.right * distance,
                Axis.Y => Vector3.up * distance,
                Axis.Z => Vector3.forward * distance,
                _ => Vector3.zero
            };
            return startingPosition + axisOffset;
        }

    }
}
