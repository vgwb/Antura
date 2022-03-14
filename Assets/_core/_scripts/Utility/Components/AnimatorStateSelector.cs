using UnityEngine;

namespace Antura.Utilities
{
    public class AnimatorStateSelector : MonoBehaviour
    {
        public string AnimatorState;

        void Start()
        {
            gameObject.GetComponent<Animator>().Play(AnimatorState);
        }
    }
}
