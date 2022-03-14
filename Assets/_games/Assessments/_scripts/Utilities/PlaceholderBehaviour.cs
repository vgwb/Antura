using UnityEngine;

namespace Antura.Assessment
{
    public class PlaceholderBehaviour : MonoBehaviour
    {
        public IPlaceholder Placeholder { get; set; }

        public DroppableBehaviour LinkedDroppable { get; set; }

        void Awake()
        {
            LinkedDroppable = null;
        }
    }
}
