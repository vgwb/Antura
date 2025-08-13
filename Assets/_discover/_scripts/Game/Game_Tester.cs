using System.Collections;
using Antura.Dog;
using Antura.LivingLetters;
using UnityEngine;

namespace Antura.Discover
{
    public class Game_Tester : MonoBehaviour
    {
        public PlayerController Antura;
        public EdLivingLetter LetterMAJOR;
        public EdLivingLetter LetterTEACHER;
        public EdLivingLetter LetterGUIDE;

        void Start()
        {
            //            Antura.Initialize();
            //LetterMAJOR.ShowImage("ball");
            //LetterTEACHER.ShowImage("bread");

            //StartCoroutine(AnimateCO());
        }

        private IEnumerator AnimateCO()
        {

            while (true)
            {
                yield return new WaitForSeconds(2f);

                //                Antura.PlayAnimation(AnturaAnimationStates.walking);
                //Antura.GoTo(pos1);
                yield return new WaitForSeconds(1f);

                LetterTEACHER.PlayAnimation((LLAnimationStates)Random.Range(0, 5));
                yield return new WaitForSeconds(1f);

                //                Antura.PlayAnimation(AnturaAnimationStates.walking);
                //Antura.GoTo(pos2);
                yield return new WaitForSeconds(1f);

                LetterTEACHER.PlayAnimation((LLAnimationStates)Random.Range(0, 5));
                yield return new WaitForSeconds(1f);
            }
        }
    }
}
