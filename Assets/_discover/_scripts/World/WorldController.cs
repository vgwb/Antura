using UnityEngine;
namespace Antura.Discover
{
    public class WorldController : MonoBehaviour
    {
        public static WorldController I;

        void Awake()
        {
            I = this;
        }
    }
}
