using UnityEngine;

namespace Antura.Minigames.ThrowBalls
{
    public class LetterWithPropsController : MonoBehaviour
    {
        public GameObject letter;

        public void AccountForProp(LetterController.PropVariation prop)
        {
            Vector3 position = transform.position;

            switch (prop)
            {
                case LetterController.PropVariation.Nothing:
                    break;
                case LetterController.PropVariation.Bush:
                    break;
                case LetterController.PropVariation.SwervingPileOfCrates:
                    position = transform.position;
                    position.y += 10.51f;
                    transform.position = position;
                    break;
                case LetterController.PropVariation.StaticPileOfCrates:
                    position.y += 10.51f;
                    transform.position = position;
                    break;
            }
        }
    }
}

