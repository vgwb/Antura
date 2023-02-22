using Antura.Dog;
using UnityEngine;
using UnityEngine.Serialization;

namespace Antura.Minigames.MakeFriends
{
    public class MakeFriendsAnturaController : MonoBehaviour
    {
        public AnturaPetSwitcher petSwitcher;
        public Vector3 runDirection;
        public float runSpeed;

        private bool run;


        public void ReactToEndGame()
        {
            petSwitcher.AnimController.DoCharge(null);
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
            petSwitcher.AnimController.DoShout();
            Audio.AudioManager.I.PlaySound(Sfx.DogBarking);
        }

        public void ReactPositively()
        {
            petSwitcher.AnimController.DoSniff();
            Audio.AudioManager.I.PlaySound(Sfx.DogSnorting);
        }
    }
}
