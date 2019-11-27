using UnityEngine;

namespace Antura.Map
{
    /// <summary>
    /// Controls the animation of plants in the Intro scene.
    /// </summary>
    public class Flag : MonoBehaviour
    {
        Quaternion startRotation;
        Vector3 cameraForward;

        float max;

        void Start()
        {
            startRotation = transform.rotation;

            max = Random.Range(2f, 6f);
        }


        void Update()
        {
            float speed = Mathf.Sin(Time.time * 1.1f);
            float angle = Mathf.Lerp(0, max, (speed * speed));

            Quaternion newRotation = Quaternion.AngleAxis(angle, Vector3.up) * startRotation;
            transform.rotation = newRotation;
        }
    }
}