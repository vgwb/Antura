using UnityEngine;

namespace Antura.Minigames.Tobogan
{
    public class Cloud : MonoBehaviour
    {
        public float tiltSpeed = 0.25f;
        public float tiltAmount = 5.0f;
        public float blobSpeed = 2.0f;

        float tiltOffset;

        float startScale = 1;
        void Start()
        {
            startScale = transform.localScale.x;
            tiltOffset = Random.value;
        }

        void Update()
        {
            float tilt = Mathf.Sin(Time.time * tiltSpeed + tiltOffset * 3.14f);
            float blob = Mathf.Sin(Time.time * blobSpeed);

            transform.localEulerAngles = new Vector3(0, 0, tilt * tiltAmount);

            float blobX = 1.0f + blob * 0.05f;
            transform.localScale = new Vector3(blobX, 1.0f / blobX, 1.0f) * startScale;

        }
    }
}
