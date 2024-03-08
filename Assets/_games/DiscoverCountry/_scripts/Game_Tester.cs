using System.Collections;
using Antura.Dog;
using Antura.LivingLetters;
using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    public class Game_Tester : MonoBehaviour
    {
        public EdAntura Antura;
        public EdLivingLetter Letter1;
        public EdLivingLetter Letter2;

        public Transform pos1;
        public Transform pos2;

        void Start()
        {
            Antura.Initialize();
            StartCoroutine(AnimateCO());
        }

        private IEnumerator AnimateCO()
        {

            while (true)
            {
                Letter1.ShowHeadProp(false);
                Letter2.ShowHeadProp(false);
                Letter1.ShowImage("ball");
                Letter2.ShowImage("bread");
                yield return new WaitForSeconds(2f);

                Antura.PlayAnimation(AnturaAnimationStates.walking);
                Antura.GoTo(pos1);
                yield return new WaitForSeconds(1f);

                Letter1.ShowHeadProp(true);
                Letter1.PlayAnimation((LLAnimationStates)Random.Range(0,5));
                Letter1.ShowImage("apple");
                yield return new WaitForSeconds(1f);

                Antura.PlayAnimation(AnturaAnimationStates.walking);
                Antura.GoTo(pos2);
                yield return new WaitForSeconds(1f);

                Letter2.ShowHeadProp(true);
                Letter2.PlayAnimation((LLAnimationStates)Random.Range(0,5));
                Letter2.ShowImage("banana");
                yield return new WaitForSeconds(1f);
            }
        }
    }
}
