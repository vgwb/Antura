using UnityEngine;

namespace Antura.Minigames.DancingDots
{
    public class DancingDotsRainbow : MonoBehaviour
    {

        public float rotateSpeed = 200;

        void Start()
        {

        }

        void Update()
        {
            transform.Rotate(0, 0, Time.deltaTime * rotateSpeed);

        }
    }
}
