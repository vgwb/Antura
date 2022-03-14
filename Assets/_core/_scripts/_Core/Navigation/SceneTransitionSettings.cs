namespace Antura.Core
{
    /// <summary>
    /// Settings for the transition between scenes.
    /// </summary>
    public class SceneTransitionSettings
    {
        /// <summary>
        /// Duration of transition.
        /// </summary>
        public int Duration = 1;

        /// <summary>
        /// Transition mode
        /// </summary>
        public enum Mode
        {
            Default, // fade to/from black.
        }
    }
}
