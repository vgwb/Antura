using UnityEngine;

namespace Antura.Minigames.Tobogan
{
    public interface ICollidable
    {
        void OnCollisionEnter(Collision collision);
    }

    public class CollisionForwarder : MonoBehaviour
    {
        public GameObject[] forwardTo;

        public void OnCollisionEnter(Collision collision)
        {
            foreach (var f in forwardTo)
            {
                f.GetComponent<ICollidable>().OnCollisionEnter(collision);
            }
        }
    }
}
