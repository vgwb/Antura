using UnityEngine;

namespace Antura.Minigames.ThrowBalls
{
    public class LetterColliderController : MonoBehaviour
    {
        public LetterController letterController;

        void Start()
        {
        }

        void Update()
        {
        }

        public void OnCollisionEnter(Collision collision)
        {
            letterController.OnCollisionEnter(collision);
        }
    }
}
