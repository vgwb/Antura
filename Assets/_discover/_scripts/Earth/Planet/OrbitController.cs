using UnityEngine;

namespace Antura.Discover
{
    public class OrbitController : MonoBehaviour
    {
        public Transform planet; // The planet to orbit around
        public float zoomSpeed = 10f; // Speed of zooming
        public float rotationSpeed = 100f; // Speed of rotation

        private float distance; // Current distance from the planet
        private Vector3 previousMousePosition; // Previous mouse position
        private Vector3 previousTouchPosition; // Previous touch position
        private float previousTouchDistance; // Previous distance between touches

        void Start()
        {
            distance = Vector3.Distance(transform.position, planet.position);
        }

        void Update()
        {
            HandleMouseInput();
            HandleTouchInput();
        }

        void HandleMouseInput()
        {
            // Rotate the camera with right mouse button
            if (InputCompat.GetMouseButton(1))
            {
                Vector3 delta = InputCompat.mousePosition - previousMousePosition;
                float angleX = delta.y * rotationSpeed * Time.deltaTime;
                float angleY = -delta.x * rotationSpeed * Time.deltaTime;

                transform.RotateAround(planet.position, transform.right, angleX);
                transform.RotateAround(planet.position, Vector3.up, angleY);
            }

            // Zoom the camera with scroll wheel
            float scroll = InputCompat.GetAxis("Mouse ScrollWheel");
            if (scroll != 0.0f)
            {
                distance -= scroll * zoomSpeed;
                distance = Mathf.Clamp(distance, 5f, 50f); // Clamp the distance to prevent too much zoom in/out
                transform.position = (transform.position - planet.position).normalized * distance + planet.position;
            }

            previousMousePosition = InputCompat.mousePosition;
        }

        void HandleTouchInput()
        {
            if (InputCompat.touchCount == 1)
            {
                // Rotate the camera with single touch
                var touch = InputCompat.GetTouch(0);
                if (touch.phase == TouchPhase.Moved)
                {
                    Vector3 delta = touch.deltaPosition;
                    float angleX = delta.y * rotationSpeed * Time.deltaTime;
                    float angleY = -delta.x * rotationSpeed * Time.deltaTime;

                    transform.RotateAround(planet.position, transform.right, angleX);
                    transform.RotateAround(planet.position, Vector3.up, angleY);
                }
                previousTouchPosition = touch.position;
            }
            else if (InputCompat.touchCount == 2)
            {
                // Zoom the camera with pinch
                var touch0 = InputCompat.GetTouch(0);
                var touch1 = InputCompat.GetTouch(1);

                Vector2 touch0PrevPos = touch0.position - touch0.deltaPosition;
                Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;

                float prevTouchDeltaMag = (touch0PrevPos - touch1PrevPos).magnitude;
                float touchDeltaMag = (touch0.position - touch1.position).magnitude;

                float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

                distance += deltaMagnitudeDiff * zoomSpeed * Time.deltaTime;
                distance = Mathf.Clamp(distance, 5f, 50f); // Clamp the distance to prevent too much zoom in/out
                transform.position = (transform.position - planet.position).normalized * distance + planet.position;

                previousTouchDistance = touchDeltaMag;
            }
        }
    }
}
