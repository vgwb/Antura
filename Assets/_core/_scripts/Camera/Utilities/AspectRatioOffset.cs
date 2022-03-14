using UnityEngine;

namespace Antura.CameraControl
{
    /// <summary>
    /// Applies an offset to a transform based on the current aspect ratio.
    /// </summary>
    public class AspectRatioOffset : MonoBehaviour
    {
        public Vector3 offsetWhen16_9;
        public Vector3 offsetWhen4_3;

        Vector3 initialPosition;

        void Start()
        {
            initialPosition = transform.localPosition;
            UpdateOffset();
        }

#if UNITY_EDITOR
        void Update()
        {
            UpdateOffset();
        }
#endif

        void UpdateOffset()
        {
            var ratio = Screen.width / (float)Screen.height;

            // 4 / 3 = 1.3333
            // 16 / 9 = 1.7777
            float t = (ratio - 1.3333f) / (1.7777f - 1.3333f);

            transform.localPosition = initialPosition + Vector3.LerpUnclamped(offsetWhen4_3, offsetWhen16_9, t);
        }
    }
}
