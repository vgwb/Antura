using UnityEngine;

namespace Antura.LivingLetters
{
    public class AlternativeEventBehaviour : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.GetComponent<LivingLetterController>().OnIdleAlternativeEnter();
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.GetComponent<LivingLetterController>().OnIdleAlternativeExit();
        }
    }
}
