using UnityEngine;

namespace Antura.Dog
{
    public class AnturaAnimEventCatcher : MonoBehaviour
    {

        public void OnShoutStarted()
        {
            GetComponentInParent<AnturaAnimationController>().OnShoutStarted();
        }

        public void OnSniffStart()
        {
            GetComponentInParent<AnturaAnimationController>().OnSniffStart();
        }

        public void OnSniffEnd()
        {
            GetComponentInParent<AnturaAnimationController>().OnSniffEnd();
        }

        public void OnAnimationWalkStart()
        {
            GetComponentInParent<AnturaAnimationController>().OnAnimationWalkStart();
        }

        public void OnAnimationWalkEnd()
        {
            GetComponentInParent<AnturaAnimationController>().OnAnimationWalkEnd();
        }

        public void OnAnimationJumpStart()
        {
            GetComponentInParent<AnturaAnimationController>().OnAnimationJumpStart();
        }

        public  void OnAnimationJumpEnd()
        {
            GetComponentInParent<AnturaAnimationController>().OnAnimationJumpEnd();
        }

        public void OnAnimationJumpGrab()
        {
            GetComponentInParent<AnturaAnimationController>().OnAnimationJumpGrab();
        }

        public void OnActionCompleted()
        {
            GetComponentInParent<AnturaAnimationController>().OnActionCompleted();
        }

    }
}
