using UnityEngine;

namespace Antura.Discover
{
    public class FaceToPlayer : MonoBehaviour
    {
        [Tooltip("If false, this component disables itself on start so it can be enabled by other scripts")]
        public bool ActivateOnStart = false;

        [Tooltip("The transform to rotate; defaults to this transform.")]
        public Transform Pivot;

        [Tooltip("Optional explicit target to face. If null, uses InteractionManager.I.player.")]
        public Transform Target;

        [Tooltip("Only rotate around Y")]
        public bool LockYOnly = true;

        [Tooltip("Degrees per second. 0 = snap instantly.")]
        public float TurnSpeed = 180f;

        [Header("Return on Disable")]
        [Tooltip("When this component is disabled (e.g., player exits), ease back to the initial rotation instead of snapping.")]
        public bool SmoothReturnOnDisable = true;

        [Tooltip("Degrees per second used when returning to the initial rotation when disabled.")]
        public float ReturnSpeed = 180f;

        private Quaternion _initialRotation;
        private SmoothReturnDriver _returnDriver;

        private void Awake()
        {
            if (Pivot == null)
                Pivot = transform;
        }

        private void OnEnable()
        {
            if (Pivot == null)
                Pivot = transform;
            _initialRotation = Pivot.rotation;

            // If a previous smooth return was in progress, cancel it.
            if (Pivot != null)
            {
                _returnDriver = Pivot.GetComponent<SmoothReturnDriver>();
                if (_returnDriver != null)
                {
                    Destroy(_returnDriver);
                    _returnDriver = null;
                }
            }
        }

        private void Start()
        {
            if (!ActivateOnStart)
                enabled = false;
        }

        private void OnDisable()
        {
            if (Pivot == null)
                return;

            if (SmoothReturnOnDisable)
            {
                // Ensure only one driver exists
                var existing = Pivot.GetComponent<SmoothReturnDriver>();
                if (existing != null)
                {
                    Destroy(existing);
                }

                _returnDriver = Pivot.gameObject.AddComponent<SmoothReturnDriver>();
                _returnDriver.Init(Pivot, _initialRotation, ReturnSpeed);
            }
            else
            {
                Pivot.rotation = _initialRotation;
            }
        }

        private void Update()
        {
            if (!enabled)
                return;

            var tgt = Target;
            if (tgt == null)
            {
                if (InteractionManager.I != null && InteractionManager.I.player != null)
                    tgt = InteractionManager.I.player.transform;
            }
            if (tgt == null || Pivot == null)
                return;

            Vector3 toTarget = tgt.position - Pivot.position;
            if (LockYOnly)
            {
                toTarget.y = 0f;
                if (toTarget.sqrMagnitude < 0.0001f)
                    return;
            }

            Quaternion desired = Quaternion.LookRotation(toTarget.normalized, Vector3.up);
            if (TurnSpeed <= 0f)
            {
                Pivot.rotation = desired;
            }
            else
            {
                Pivot.rotation = Quaternion.RotateTowards(Pivot.rotation, desired, TurnSpeed * Time.deltaTime);
            }
        }

        private class SmoothReturnDriver : MonoBehaviour
        {
            public Transform Pivot;
            public Quaternion Target;
            public float Speed;

            public void Init(Transform pivot, Quaternion target, float speed)
            {
                Pivot = pivot;
                Target = target;
                Speed = Mathf.Max(0f, speed);
            }

            private void Update()
            {
                if (Pivot == null)
                {
                    Destroy(this);
                    return;
                }

                if (Speed <= 0f)
                {
                    Pivot.rotation = Target;
                    Destroy(this);
                    return;
                }

                Pivot.rotation = Quaternion.RotateTowards(Pivot.rotation, Target, Speed * Time.deltaTime);
                if (Quaternion.Angle(Pivot.rotation, Target) <= 0.1f)
                {
                    Pivot.rotation = Target;
                    Destroy(this);
                }
            }
        }
    }
}
