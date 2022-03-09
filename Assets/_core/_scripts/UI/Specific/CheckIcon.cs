using UnityEngine;
using UnityEngine.UI;

namespace Antura.UI
{
    public class CheckIcon : MonoBehaviour
    {
        public Image Icon;
        public Sprite CheckOn;
        public Sprite CheckOff;

        public void Set(bool status)
        {
            if (status)
            {
                Icon.sprite = CheckOn;
            }
            else
            {
                Icon.sprite = CheckOff;
            }
        }
    }
}
