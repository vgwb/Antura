using UnityEngine;

namespace Antura.LivingLetters
{
    public class SpecialStateEventBehaviour : StateMachineBehaviour
    {
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.gameObject.SendMessage("OnActionCompleted");
        }
    }
}
