using UnityEngine;

namespace Antura.Minigames.DiscoverCountry.Interaction
{
    public class Rotator : MonoBehaviour
    {
        public float speed = 10f;

        void Update()
        {
            transform.Rotate(Vector3.up, speed * Time.deltaTime);
        }
    }
}
