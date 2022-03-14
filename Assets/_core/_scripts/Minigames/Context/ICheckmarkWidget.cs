using UnityEngine;

namespace Antura.Minigames
{
    /// <summary>
    /// Provides access to the MinigamesCheckmarkWidget UI element for minigames.
    /// <seealso cref="MinigamesCheckmarkWidget"/>
    /// </summary>
    public interface ICheckmarkWidget
    {
        void Show(bool correct, Vector2 pos = default);
    }
}
