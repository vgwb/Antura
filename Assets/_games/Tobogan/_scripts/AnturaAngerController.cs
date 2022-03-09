using Antura.Dog;
using UnityEngine;

namespace Antura.Minigames.Tobogan
{
    [RequireComponent(typeof(AnturaAnimationController))]
    public class AnturaAngerController : MonoBehaviour
    {
        float barkingTimer = 0.0f;
        public bool IsWaken { get { return barkingTimer > 0; } }

        public void WakeUp(bool bark)
        {
            GetComponent<AnturaAnimationController>().State = AnturaAnimationStates.idle;

            if (bark)
            {
                GetComponent<AnturaAnimationController>().DoShout(() =>
                {
                    ToboganConfiguration.Instance.Context.GetAudioManager().PlaySound(Sfx.DogBarking);
                });
            }

            barkingTimer = 3.0f;
        }


        void Update()
        {
            if (IsWaken)
            {
                GetComponent<AnturaAnimationController>().State = AnturaAnimationStates.idle;

                barkingTimer -= Time.deltaTime;
            }
            else
                GetComponent<AnturaAnimationController>().State = AnturaAnimationStates.sleeping;
        }
    }
}
