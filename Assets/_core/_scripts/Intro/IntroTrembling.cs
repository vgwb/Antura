using UnityEngine;

namespace Antura.Intro
{
    // TODO refactor: could not find any reference to this script in the Intro scene
    public class IntroTrembling : MonoBehaviour
    {
        public Transform tremblingCenter;

        public float minDistance = 30f;
        public float maxDistance = 40f;

        float tremblingAmount;
        Vector3 tremblingOffset;

        public float tremblingSpeedX = 1;
        public float tremblingSpeedY = 1;
        public float tremblingSpeedZ = 1;
        public float tremblingAmountX = 0.1f;
        public float tremblingAmountY = 0.1f;
        public float tremblingAmountZ = 0.1f;

        Vector3 startPosition;

        void Start()
        {
            startPosition = transform.localPosition;
        }

        void Update()
        {
            float distance = Vector3.Distance(transform.position, tremblingCenter.position);

            if (distance > minDistance && distance < maxDistance)
            {
                tremblingAmount = 1f - ((distance - minDistance) / (maxDistance - minDistance));
            }
            else
            {
                tremblingAmount = 0f;
            }

            // Trembling
            Vector3 noise = new Vector3(
                tremblingAmountX * Mathf.Cos(Mathf.Repeat(Time.realtimeSinceStartup * tremblingSpeedX, 2 * Mathf.PI)),
                tremblingAmountY * Mathf.Cos(Mathf.Repeat(Time.realtimeSinceStartup * tremblingSpeedY, 2 * Mathf.PI)),
                tremblingAmountZ * Mathf.Cos(Mathf.Repeat(Time.realtimeSinceStartup * tremblingSpeedZ, 2 * Mathf.PI))
            );

            tremblingOffset = Vector3.Lerp(tremblingOffset, noise, 40.0f * Time.deltaTime);

            transform.localPosition = Vector3.Lerp(startPosition, startPosition + tremblingOffset, tremblingAmount);
        }
    }
}
