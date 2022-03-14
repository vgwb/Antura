using Antura.Dog;
using UnityEngine;

namespace Antura.Minigames.MakeFriends
{
    public class MakeFriendsAnturaController : MonoBehaviour
    {
        public AnturaAnimationController animationController;
        public Vector3 runDirection;
        public float runSpeed;

        private bool run;


        public void ReactToEndGame()
        {
            animationController.DoCharge(null);
            run = true;
        }

        void FixedUpdate()
        {
            if (run)
            {
                transform.Translate(runDirection * runSpeed);
            }
        }

        public void ReactNegatively()
        {
            animationController.DoShout();
            Audio.AudioManager.I.PlaySound(Sfx.DogBarking);
        }

        public void ReactPositively()
        {
            animationController.DoSniff();
            Audio.AudioManager.I.PlaySound(Sfx.DogSnorting);
        }
    }
}
