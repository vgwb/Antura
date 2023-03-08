using UnityEngine;

namespace Antura.Dog
{
    public class AnturaAnimEventCatcher : MonoBehaviour
    {
        void OnAnimationJumpGrab()
        {
            GetComponentInParent<AnturaAnimationController>().OnAnimationJumpGrab();
        }
    }
}
