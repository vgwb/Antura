using UnityEngine;

namespace Antura.Assessment
{
    public class OffsetFromScreenSide : MonoBehaviour
    {
        public float minX;
        public float maxX;

        void Start()
        {
            float minRatio = 4.0f / 3.0f;
            float maxRatio = 16.0f / 9.0f;
            float currentRatio = Screen.width / (float)Screen.height;
            float t = (currentRatio - minRatio) / (maxRatio - minRatio);
            Vector3 pos = transform.localPosition;
            pos.x = Mathf.Lerp(minX, maxX, t);
            transform.localPosition = pos;
        }
    }
}
