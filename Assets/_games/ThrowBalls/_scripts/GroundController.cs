using UnityEngine;

namespace Antura.Minigames.ThrowBalls
{
    public class GroundController : MonoBehaviour
    {

        public static GroundController instance;

        void Awake()
        {
            instance = this;
        }
    }
}
