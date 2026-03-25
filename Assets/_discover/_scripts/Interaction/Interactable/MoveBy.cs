using UnityEngine;

namespace Antura.Discover
{
    public class MoveBy : ActableAbstract
    {
        public enum Axis { X, Y, Z }
        public enum MovementType { GlobalAxis = 0, TargetPoint = 1, LocalAxis = 2 }
        public enum TriggerMode { Single, Toggle }

        public bool IsActivated;
        public float speed = 1f;
        [Tooltip("Choose whether the movement uses world axes, this object's local axes, or a target point.")]
        public MovementType movementType = MovementType.GlobalAxis;
        [Tooltip("Single moves only once when triggered. Toggle moves back and forth on repeated triggers.")]
        public TriggerMode triggerMode = TriggerMode.Single;

        [Header("Axis Movement")]
        [Tooltip("Axis used when Movement Type is GlobalAxis or LocalAxis.")]
        public Axis MoveAxis = Axis.Y;
        [Tooltip("Distance to move along the selected axis.")]
        public float distance = 2.0f;

        [Header("Target Point Movement")]
        [Tooltip("Destination used when Movement Type is TargetPoint.")]
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

            Vector3 axisDirection = MoveAxis switch
            {
                Axis.X => Vector3.right * distance,
                Axis.Y => Vector3.up * distance,
                Axis.Z => Vector3.forward * distance,
                _ => Vector3.zero
            };

            if (movementType == MovementType.LocalAxis)
            {
                axisDirection = MoveAxis switch
                {
                    Axis.X => transform.right * distance,
                    Axis.Y => transform.up * distance,
                    Axis.Z => transform.forward * distance,
                    _ => Vector3.zero
                };
            }

            return startingPosition + axisDirection;
        }

    }
}
