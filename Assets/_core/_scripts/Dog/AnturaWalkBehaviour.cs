using UnityEngine;

namespace Antura.Dog
{
    /// <summary>
    /// State for Antura's animation. Animates a walk cylce.
    /// </summary>
    public class AnturaWalkBehaviour : StateMachineBehaviour
    {
        // TODO refactor: cache AnturaAnimationController
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var ac = animator.GetComponent<AnturaAnimationController>();
            if (ac == null) ac = animator.GetComponentInParent<AnturaAnimationController>();
            ac.SendMessage("OnAnimationWalkStart", SendMessageOptions.DontRequireReceiver);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var ac = animator.GetComponent<AnturaAnimationController>();
            if (ac == null) ac = animator.GetComponentInParent<AnturaAnimationController>();
            ac.SendMessage("OnAnimationWalkEnd", SendMessageOptions.DontRequireReceiver);
        }
    }
}
