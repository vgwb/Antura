using Antura.Audio;
using UnityEngine;

namespace Antura.Dog
{
    public class TouchToBark : MonoBehaviour
    {
        private float animationTimer;

        public void OnMouseDown()
        {
            if (animationTimer > 0)
            {
                return;
            }
            var rnd = Random.value;

            if (rnd < 0.3f)
            {
                GetComponent<AnturaAnimationController>().DoSniff(null, () => { AudioManager.I.PlaySound(Sfx.DogSnorting); });
            }
            else if (rnd < 0.5f)
            {
                GetComponent<AnturaAnimationController>().State = AnturaAnimationStates.digging;
                animationTimer = 2;
            }
            else if (rnd < 0.7f)
            {
                GetComponent<AnturaAnimationController>().State = AnturaAnimationStates.sheeping;
                animationTimer = 2;
            }
            else
            {
                GetComponent<AnturaAnimationController>().DoShout(() => { AudioManager.I.PlaySound(Sfx.DogBarking); });
            }
        }

        public void Update()
        {
            if (animationTimer > 0)
            {
                animationTimer -= Time.deltaTime;
                if (animationTimer <= 0)
                {
                    GetComponent<AnturaAnimationController>().State = AnturaAnimationStates.sitting;
                }
            }
        }
    }
}
