using Antura.LivingLetters;

namespace Antura.Minigames
{
    /// <summary>
    /// Provides generic log access to the core and to minigames.
    /// <seealso cref="LogManager"/>
    /// <seealso cref="MinigamesLogManager"/>
    /// </summary>
    public interface ILogManager
    {
        /// <summary>
        /// Called when minigame is finished.
        /// </summary>
        /// <param name="result">The valuation (0 to 3 stars).</param>
        void OnGameEnded(int result);

        /// <summary>
        /// To be called on any action of player linked to learning objective and with positive or negative vote.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="isPositiveResult"></param>
        void OnAnswered(ILivingLetterData data, bool isPositiveResult);

        /* OnGameplaySkillAction is now called by LogAI and not by minigames
        /// <summary>
        /// Called when players perform a [gameplay skill action] action during gameplay.
        /// </summary>
        /// <param name="ability">The ability.</param>
        /// <param name="score">The score (0 to 1).</param>
        void OnGameplaySkillAction(PlaySkill ability, float score);
        */
    }
}
