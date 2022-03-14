using UnityEngine;

namespace Antura.Minigames.Balloons
{
    public class TestPhysics : MonoBehaviour
    {
        public Rigidbody body;
        public GameObject parent;
        public Rigidbody parentRigidbody;

        public bool floatByTransformPosition;
        public bool floatByTransformLocalPosition;
        public bool floatByRigidbodyPosition;
        public bool floatByRigidbodyVelocity;

        public bool waft;

        public bool spinByTransformRotation;
        public bool spinByTransformLocalRotation;
        public bool spinByRigidbodyAngularVelocity;

        public bool dragByTransformPosition;
        public bool dragByTransformLocalPosition;
        public bool dragByRigidbodyPosition;
        public bool dragParent;

        [Range(0, 100)]
        public float floatSpeed;
        [Range(0, 100)]
        public float floatDistance;
        [Range(0, 100)]
        public float spinSpeed;
        [Range(0, 360)]
        public float spinAngle;

        public bool affectedByExplosions;

        private Vector3 basePosition;
        private Vector3 baseLocalPosition;
        private Vector3 baseRotation;
        private Vector3 baseLocalRotation;
        private float cameraDistance;
        private Vector3 mousePosition = new Vector3();


        void Start()
        {
            basePosition = transform.position;
            baseLocalPosition = transform.localPosition;
            baseRotation = transform.rotation.eulerAngles;
            baseLocalRotation = transform.localRotation.eulerAngles;
            cameraDistance = Mathf.Abs(Camera.main.transform.position.z - transform.position.z);
        }

        void Update()
        {
            if (floatByTransformPosition)
            {
                FloatByTransformPosition();
            }
            if (floatByTransformLocalPosition)
            {
                FloatByTransformLocalPosition();
            }
            if (floatByRigidbodyPosition)
            {
                FloatByRigidbodyPosition();
            }
            if (floatByRigidbodyVelocity)
            {
                FloatByRigidbodyVelocity();
            }
            if (spinByTransformRotation)
            {
                SpinByTransformRotation();
            }
            if (spinByTransformLocalRotation)
            {
                SpinByTransformLocalRotation();
            }
            if (spinByRigidbodyAngularVelocity)
            {
                SpinByRigidbodyAngularVelocity();
            }
            if (waft)
            {
                Waft();
            }
        }

        void OnMouseDrag()
        {
            if (dragByTransformPosition)
            {
                DragByTransformPosition();
            }
            if (dragByTransformLocalPosition)
            {
                DragByTransformLocalPosition();
            }
            if (dragByRigidbodyPosition)
            {
                DragByRigidbodyPosition();
            }
        }


        // Floating ---

        public void FloatByTransformPosition()
        {
            transform.position = basePosition + floatDistance * Mathf.Sin(floatSpeed * Time.time) * Vector3.up;
        }

        public void FloatByTransformLocalPosition()
        {
            transform.localPosition = baseLocalPosition + floatDistance * Mathf.Sin(floatSpeed * Time.time) * Vector3.up;
        }

        public void FloatByRigidbodyPosition()
        {
            body.MovePosition(basePosition + floatDistance * Mathf.Sin(floatSpeed * Time.time) * Vector3.up);
        }

        public void FloatByRigidbodyVelocity()
        {
            body.velocity = floatDistance * Mathf.Sin(floatSpeed * Time.time) * Vector3.up;
        }


        // Wafting

        public void Waft()
        {
            body.velocity = Vector3.right * 1f;
            if (body.transform.position.x > 20f)
            {
                body.transform.position = new Vector3(-20f, body.transform.position.y, body.transform.position.z);
            }
        }


        // Spinning ---

        void SpinByTransformRotation()
        {
            transform.rotation = Quaternion.Euler(baseRotation.x, baseRotation.y + spinAngle * Mathf.Sin(spinSpeed * Time.time), baseRotation.z);
        }

        void SpinByTransformLocalRotation()
        {
            transform.localRotation = Quaternion.Euler(baseLocalRotation.x, baseLocalRotation.y + spinAngle * Mathf.Sin(spinSpeed * Time.time), baseLocalRotation.z);
        }

        void SpinByRigidbodyAngularVelocity()
        {
            body.angularVelocity = Vector3.up * spinAngle * Mathf.Sin(spinSpeed * Time.time);
        }


        // Dragging ---

        public void DragByTransformPosition()
        {
            mousePosition = Input.mousePosition;
            mousePosition.z = cameraDistance;

            var newPosition = Camera.main.ScreenToWorldPoint(mousePosition);

            if (dragParent)
            {
                parent.transform.position = newPosition;
            }
            else
            {
                transform.position = newPosition;
            }
        }

        public void DragByTransformLocalPosition()
        {
            mousePosition = Input.mousePosition;
            mousePosition.z = cameraDistance;

            var newPosition = Camera.main.ScreenToWorldPoint(mousePosition);

            if (dragParent)
            {
                parent.transform.localPosition = newPosition;
            }
            else
            {
                transform.localPosition = newPosition;
            }
        }

        public void DragByRigidbodyPosition()
        {
            mousePosition = Input.mousePosition;
            mousePosition.z = cameraDistance;

            var newPosition = Camera.main.ScreenToWorldPoint(mousePosition);

            if (dragParent)
            {
                parentRigidbody.MovePosition(newPosition);
            }
            else
            {
                body.MovePosition(newPosition);
            }
        }
    }
}
