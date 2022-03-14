using Antura.UI;
using UnityEngine;

namespace Antura.Minigames
{
    /// <summary>
    /// Concrete implementation of ICheckmarkWidget. Accessible to minigames.
    /// </summary>
    public class MinigamesCheckmarkWidget : ICheckmarkWidget
    {
        public void Show(bool correct, Vector2 pos = default)
        {
            GlobalUI.I.ActionFeedback.Show(correct, pos);
        }
    }
}
