using UnityEngine;
using System.Collections;

namespace Antura.Minigames.SickLetters
{
    public class SickLettersAntura : MonoBehaviour
    {

        Animator anturaAnimator;


        void Start()
        {
            anturaAnimator = GetComponent<Animator>();
        }


        public void sleep()
        {
            StartCoroutine(coSleep());
        }

        public IEnumerator coSleep()
        {
            anturaAnimator.SetBool("idle", false);
            anturaAnimator.SetBool("angry", false);
            anturaAnimator.SetBool("sleeping", true);
            yield return null;
        }

        public IEnumerator bark(float delay = 0)
        {
            yield return new WaitForSeconds(delay);
            anturaAnimator.SetBool("sleeping", false);
            anturaAnimator.SetBool("idle", true);
            yield return new WaitForSeconds(0.75f);
            anturaAnimator.SetBool("angry", true);
            yield return new WaitForSeconds(1f);
            anturaAnimator.SetTrigger("doShout");
            SickLettersConfiguration.Instance.Context.GetAudioManager().PlaySound(Sfx.DogBarking);
        }
    }
}
