using Antura.Helpers;
using UnityEngine;

namespace Antura.Minigames.FastCrowd
{
    public class LetterCharacterController : MonoBehaviour
    {
        CharacterController controller;

        bool sweep = true;
        public bool EnableSweep
        {
            get { return sweep; }
            set
            {
                sweep = value;
                controller.enabled = value;
            }
        }

        void Start()
        {
            controller = GetComponent<CharacterController>();

        }
        public void MoveAmount(Vector3 deltaPosition)
        {
            if (EnableSweep)
                controller.Move(deltaPosition);
            else
                transform.position += deltaPosition;
        }

        public void MoveTo(Vector3 position)
        {
            MoveAmount(position - transform.position);
        }

        public void LookAt(Vector3 position)
        {
            GameplayHelper.LerpLookAtPlanar(transform, position, 1);
        }

        public void LerpLookAt(Vector3 position, float t)
        {
            GameplayHelper.LerpLookAtPlanar(transform, position, t);
        }
    }
}
