using UnityEngine;

namespace Antura.Dog
{
    /// <summary>
    /// State for Antura's animation. Animates a walk cylce.
    /// </summary>
    public class AnturaJumpBehaviour : StateMachineBehaviour
    {
        // TODO refactor: cache AnturaAnimationController
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.GetComponent<AnturaAnimationController>()
                .SendMessage("OnAnimationJumpStart", SendMessageOptions.DontRequireReceiver);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.GetComponent<AnturaAnimationController>()
                .SendMessage("OnAnimationJumpEnd", SendMessageOptions.DontRequireReceiver);
        }
    }
}
