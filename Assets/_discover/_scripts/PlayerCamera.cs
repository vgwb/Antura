using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    public class PlayerCamera : MonoBehaviour
    {
        private const float LOW_LIMIT = 0.0f;
        private const float HIGH_LIMIT = 85.0f;

        public GameObject theCamera;
        public float followDistance = 5.0f;
        public float mouseSensitivityX = 4.0f;
        public float mouseSensitivityY = 2.0f;
        public float heightOffset = 0.5f;

        void Start()
        {
            theCamera.transform.forward = gameObject.transform.forward;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        void Update()
        {

            if (Input.GetMouseButton(1))
            {
                Vector2 cameraMovement = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
                theCamera.transform.position = gameObject.transform.position + new Vector3(0, heightOffset, 0);
                theCamera.transform.eulerAngles = new Vector3(
                    Mathf.Clamp(theCamera.transform.eulerAngles.x + cameraMovement.y * mouseSensitivityY, LOW_LIMIT, HIGH_LIMIT),
                    theCamera.transform.eulerAngles.y + cameraMovement.x * mouseSensitivityX, 0);
                theCamera.transform.position -= theCamera.transform.forward * followDistance;
            }
        }
    }
}
