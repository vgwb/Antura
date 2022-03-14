using UnityEngine;

namespace Antura.Minigames
{
    /// <summary>
    /// Shows a shadow with a fixed height.
    /// May be used inside minigames.
    /// </summary>
    // refactor: maybe move to _games/_common/_scripts/_utils
    public class FixedHeightShadow : MonoBehaviour
    {
        public Transform toFollow;
        public float y;

        void Update()
        {
            if (toFollow == null)
            {
                return;
            }

            var pos = transform.position;
            pos = toFollow.position;
            pos.y = y;
            transform.position = pos;
        }

        public void Initialize(Transform toFollow, float y)
        {
            this.toFollow = toFollow;
            this.y = y;
        }
    }
}
