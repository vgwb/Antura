using UnityEngine;
using System.Collections;

namespace Antura.Minigames.DiscoverCountry
{
    public class SpaceCameraOrbit : MonoBehaviour
    {
        public int SafeAreaX = 1000;
        public Canvas canvas;
        public RectTransform canvasRectTransform;

        public float MinDistance = 1.0f;
        public float MaxDistance = 1.3f;
        public float amplify = 0.001f;
        public float sensitivity = 0.001f;
        public float zoomSpeed = 1f;
        public float distance = 1000;
        public float distanceTarget;
        public Vector2 mouse;
        public Vector2 mouseOnDown;
        public Vector2 rotation;
        public Vector2 target = new Vector2(Mathf.PI * 3 / 2, Mathf.PI / 6);
        public Vector2 targetOnDown;

        void Start()
        {
            distanceTarget = transform.position.magnitude;
        }

        bool down = false;
        void Update()
        {
            if (Input.touchCount > 0)
            {
                if (Input.touches[0].phase == TouchPhase.Began)
                {
                    down = true;
                    mouseOnDown.x = Input.touches[0].position.x;
                    mouseOnDown.y = -Input.touches[0].position.y;

                    targetOnDown.x = target.x;
                    targetOnDown.y = target.y;
                }
                else if (Input.touches[0].phase == TouchPhase.Canceled ||
                    Input.touches[0].phase == TouchPhase.Ended)
                {
                    down = false;
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    down = true;
                    mouseOnDown.x = Input.mousePosition.x;
                    mouseOnDown.y = -Input.mousePosition.y;

                    targetOnDown.x = target.x;
                    targetOnDown.y = target.y;
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    down = false;
                }
            }
            if (down)
            {
                if (Input.touchCount > 0)
                {
                    mouse.x = Input.touches[0].position.x;
                    mouse.y = -Input.touches[0].position.y;
                }
                else
                {
                    mouse.x = Input.mousePosition.x;
                    mouse.y = -Input.mousePosition.y;
                }
                float zoomDamp = distance / 1;

                target.x = targetOnDown.x + (mouse.x - mouseOnDown.x) * sensitivity * zoomDamp;
                target.y = targetOnDown.y + (mouse.y - mouseOnDown.y) * sensitivity * zoomDamp;

                target.y = Mathf.Clamp(target.y, -Mathf.PI / 2 + 0.01f, Mathf.PI / 2 - 0.01f);
            }

            if (ScreenToCanvasPosition(mouse) > SafeAreaX)
            {
                return;
            }

            distanceTarget -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
            distanceTarget = Mathf.Clamp(distanceTarget, MinDistance, MaxDistance);

            rotation.x += (target.x - rotation.x) * amplify;
            rotation.y += (target.y - rotation.y) * amplify;
            distance += (distanceTarget - distance);
            Vector3 position;
            position.x = distance * Mathf.Sin(rotation.x) * Mathf.Cos(rotation.y);
            position.y = distance * Mathf.Sin(rotation.y);
            position.z = distance * Mathf.Cos(rotation.x) * Mathf.Cos(rotation.y);
            transform.position = position;
            transform.LookAt(Vector3.zero);
        }

        private float ScreenToCanvasPosition(Vector2 screenPosition)
        {
            Vector2 localPoint;

            // Convert screen position to local point in the canvas
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRectTransform,
                screenPosition,
                canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
                out localPoint
            );

            return localPoint.x;
        }
    }
}
