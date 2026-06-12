using UnityEngine;

namespace Antura
{
    /// <summary>
    /// When the legacy Input Manager is disabled (Active Input Handling = Input System Package),
    /// Unity stops invoking the OnMouseDown / OnMouseDrag / OnMouseUp / OnMouseUpAsButton
    /// MonoBehaviour callbacks. This simulator restores them, driven by InputCompat
    /// (so they also work with touch, like the old mouse simulation did).
    ///
    /// Only the four callbacks actually used in this project are simulated.
    /// Hover callbacks (OnMouseEnter/Over/Exit) are NOT simulated.
    /// </summary>
    [DefaultExecutionOrder(-1000)]
    public class LegacyMouseEventSimulator : MonoBehaviour
    {
        private const string MsgDown = "OnMouseDown";
        private const string MsgDrag = "OnMouseDrag";
        private const string MsgUp = "OnMouseUp";
        private const string MsgUpAsButton = "OnMouseUpAsButton";

        private GameObject pressedObject;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void AutoCreate()
        {
            var go = new GameObject("[LegacyMouseEventSimulator]");
            go.AddComponent<LegacyMouseEventSimulator>();
            DontDestroyOnLoad(go);
        }

        private void Update()
        {
            if (InputCompat.GetMouseButtonDown(0))
            {
                pressedObject = PickObjectUnderPointer(InputCompat.mousePosition);
                if (pressedObject != null)
                    pressedObject.SendMessage(MsgDown, SendMessageOptions.DontRequireReceiver);
            }
            else if (pressedObject != null && InputCompat.GetMouseButton(0))
            {
                pressedObject.SendMessage(MsgDrag, SendMessageOptions.DontRequireReceiver);
            }

            if (InputCompat.GetMouseButtonUp(0) && pressedObject != null)
            {
                pressedObject.SendMessage(MsgUp, SendMessageOptions.DontRequireReceiver);
                if (PickObjectUnderPointer(InputCompat.mousePosition) == pressedObject)
                    pressedObject.SendMessage(MsgUpAsButton, SendMessageOptions.DontRequireReceiver);
                pressedObject = null;
            }
        }

        /// <summary>
        /// Replicates legacy picking: every camera (topmost depth first) raycasts within
        /// its viewport, honoring its eventMask, against both 3D and 2D colliders.
        /// </summary>
        private static GameObject PickObjectUnderPointer(Vector3 screenPosition)
        {
            Camera bestCamera = null;
            float bestDepth = float.MinValue;
            GameObject bestObject = null;

            int cameraCount = Camera.allCamerasCount;
            for (int i = 0; i < cameraCount; i++)
            {
                var cam = Camera.allCameras[i];
                if (cam == null || !cam.enabled)
                    continue;
                if (cam.targetTexture != null)
                    continue;
                if (!cam.pixelRect.Contains(screenPosition))
                    continue;
                if (cam.depth < bestDepth && bestObject != null)
                    continue;

                var hitGo = Raycast(cam, screenPosition);
                if (hitGo != null && (bestCamera == null || cam.depth >= bestDepth))
                {
                    bestCamera = cam;
                    bestDepth = cam.depth;
                    bestObject = hitGo;
                }
            }
            return bestObject;
        }

        private static GameObject Raycast(Camera cam, Vector3 screenPosition)
        {
            var ray = cam.ScreenPointToRay(screenPosition);
            int mask = cam.eventMask & cam.cullingMask;

            GameObject result = null;
            float resultDistance = float.MaxValue;

            // 3D colliders
            if (Physics.Raycast(ray, out RaycastHit hit3D, cam.farClipPlane, mask))
            {
                result = hit3D.collider.gameObject;
                resultDistance = hit3D.distance;
            }

            // 2D colliders
            var hit2D = Physics2D.GetRayIntersection(ray, cam.farClipPlane, mask);
            if (hit2D.collider != null && hit2D.distance < resultDistance)
            {
                result = hit2D.collider.gameObject;
            }

            return result;
        }
    }
}
