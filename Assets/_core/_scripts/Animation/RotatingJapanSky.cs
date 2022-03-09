using UnityEngine;

namespace Antura.Animation
{
    public class RotatingJapanSky : MonoBehaviour
    {
        public float speed = 20f;

        private Vector3 rotationEuler;

        void Update()
        {
            rotationEuler += Vector3.forward * speed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(rotationEuler);
        }
    }
}
