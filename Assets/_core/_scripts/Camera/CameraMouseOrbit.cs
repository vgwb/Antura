using UnityEngine;

namespace Antura.CameraControl
{
    public class CameraMouseOrbit : MonoBehaviour
    {
        public Transform target;
        public float distance = 5.0f;
        public float xSpeed = 120.0f;
        public float ySpeed = 120.0f;

        public float yMinLimit = -20f;
        public float yMaxLimit = 80f;

        public float distanceMin = .5f;
        public float distanceMax = 15f;

        float x = 0.0f;
        float y = 0.0f;

        [Space(20)]
        public bool autoMovement = false;

        public float autoSpeedX = 0.2f;
        public float autoSpeedY = 0.1f;
        public float autoSpeedDistance = -0.1f;

        // Use this for initialization
        void Start()
        {
            Vector3 angles = transform.eulerAngles;
            x = angles.y;
            y = angles.x;

            Rigidbody rigidbody = GetComponent<Rigidbody>();

            // Make the rigid body not change rotation
            if (rigidbody != null)
            {
                rigidbody.freezeRotation = true;
            }
        }

        void LateUpdate()
        {
            if (target)
            {
                if (Input.GetMouseButton(0))
                {
                    x += Input.GetAxis("Mouse X") * xSpeed * distance * 0.02f;
                    y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
                }
                else if (autoMovement)
                {
                    x += autoSpeedX * distance * 0.2f;
                    y += autoSpeedY;
                    distance += autoSpeedDistance;
                }


                y = ClampAngle(y, yMinLimit, yMaxLimit);

                Quaternion rotation = Quaternion.Euler(y, x, 0);

                distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * 5, distanceMin, distanceMax);

                RaycastHit hit;
                if (Physics.Linecast(target.position, transform.position, out hit))
                {
                    distance -= hit.distance;
                }
                Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
                Vector3 position = rotation * negDistance + target.position;

                transform.rotation = rotation;
                transform.position = position;
            }
        }

        public static float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360F)
            {
                angle += 360F;
            }
            if (angle > 360F)
            {
                angle -= 360F;
            }
            return Mathf.Clamp(angle, min, max);
        }
    }
}
