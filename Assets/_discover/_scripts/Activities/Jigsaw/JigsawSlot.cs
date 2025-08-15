using UnityEngine;

namespace Antura.Discover.Activities
{
    public class JigsawSlot : MonoBehaviour
    {
        public int row;
        public int col;
        public ActivityJigsawPuzzle manager;
        public JigsawPiece currentPiece;
        public RectTransform RectTransform { get; private set; }

        private void Awake()
        {
            RectTransform = GetComponent<RectTransform>();
        }
    }
}
