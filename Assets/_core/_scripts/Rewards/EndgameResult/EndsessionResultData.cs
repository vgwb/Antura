using Antura.Database;

namespace Antura.Rewards
{
    /// <summary>
    /// Data related to a minigame played during a play session.
    /// NOTE: could be a struct, but will probably need to be a class later, so keeping it as class from the beginning
    /// </summary>
    public class EndsessionResultData
    {
        public int Stars;
        public MiniGameData MiniGameData;

        /// <summary>
        /// Data for a minigame played during the session
        /// </summary>
        /// <param name="_stars">Total stars gained</param>
        /// <param name="_miniGameData">Data of the minigame that was played</param>
        public EndsessionResultData(int _stars, MiniGameData _miniGameData)
        {
            Stars = _stars;
            MiniGameData = _miniGameData;
        }
    }
}
