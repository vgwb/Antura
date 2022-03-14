using UnityEngine;

namespace Antura.Minigames
{
    /// <summary>
    /// Provides generic input access to the core and to minigames.
    /// <seealso cref="MinigamesInputManager"/>
    /// </summary>
    public interface IInputManager
    {
        // does not raise events when disabled
        bool Enabled { set; }

        /// <summary>
        /// Pixel position of the touch
        /// </summary>
        Vector2 LastPointerPosition { get; }

        /// <summary>
        /// Normalized position of the touch: (0, 0) -> bottom left of the screen, (1, 1) -> top right of the screen
        /// </summary>
        Vector2 LastPointerPositionNormalized { get; }

        /// <summary>
        /// Physical position of the touch: it's equal to LastPointerPosition / ScreenDPI.
        /// The result is position in inches from screen's bottom left.
        /// Warning: on Android it is not a very precise measure on all devices.
        /// </summary>
        Vector2 LastPointerPositionPhysical { get; }

        /// <summary>
        /// Difference, in pixels, of current and previous values of pointer position
        /// </summary>
        Vector2 LastPointerDelta { get; }

        /// <summary>
        /// Difference, in pixels, of current and previous values of pointer position
        /// </summary>
        Vector2 LastPointerDeltaNormalized { get; }

        /// <summary>
        /// Physical distance between current and previous values of pointer position. It equals to LastPointerDelta / ScreenDPI.
        /// Result is in inches.
        /// Warning: on Android it is not a very precise measure on all devices.
        /// </summary>
        Vector2 LastPointerDeltaPhysical { get; }

        /// <summary>
        /// Time passed from the last update.
        /// </summary>
        float LastDeltaTime { get; }

        event System.Action onPointerDown;
        event System.Action onPointerUp;
        event System.Action onPointerDrag;

        void Update(float delta);
        void Reset();
    }
}
