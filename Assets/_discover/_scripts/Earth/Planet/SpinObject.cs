using UnityEngine;

namespace Antura.Discover
{
    /// <summary>
    /// Spin the object at a specified speed
    /// </summary>
    public class SpinObject : MonoBehaviour
    {
        [Tooltip("Spin: Yes or No")]
        public bool spin;
        [Tooltip("Spin the parent object instead of the object this script is attached to")]
        public bool spinParent;

        public float speed = 10f;

        void Update()
        {

            if (spin)
            {
                if (spinParent)
                {
                    transform.parent.transform.Rotate(Vector3.up, speed * Time.deltaTime);
                }
                else
                {
                    transform.Rotate(Vector3.up, speed * Time.deltaTime);
                }
            }
        }
    }
}
