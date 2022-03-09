using UnityEngine;

namespace Antura.Minigames.HideAndSeek
{
    public class HideAndSeekTreeController : MonoBehaviour
    {
        public delegate void TouchAction(int i);
        public event TouchAction onTreeTouched;

        void OnMouseDown()
        {
            if (onTreeTouched != null)
            {
                onTreeTouched(id);
            }
        }

        public int id;
    }

}
