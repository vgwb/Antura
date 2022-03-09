using UnityEngine;

namespace Antura.Intro
{
    /// <summary>
    /// Controls the animation of plants in the Intro scene.
    /// </summary>
    public class IntroPlantWind : MonoBehaviour
    {
        Quaternion startRotation;
        Vector3 cameraForward;

        float max;

        void Start()
        {
            startRotation = transform.rotation;
            cameraForward = Camera.main.transform.forward;

            max = Random.Range(2f, 6f);
        }


        void Update()
        {
            float speed = Mathf.Sin(Time.time * 1.2f);
            float angle = Mathf.Lerp(0, max, (speed * speed));

            Quaternion newRotation = Quaternion.AngleAxis(angle, cameraForward) * startRotation;
            transform.rotation = newRotation;
        }
    }
}
