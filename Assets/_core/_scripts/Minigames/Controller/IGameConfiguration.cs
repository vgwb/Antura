using Antura.Database;
using Antura.LivingLetters;
using Antura.Teacher;

namespace Antura.Minigames
{
    /// <summary>
    /// Interface for all configuration containers of minigames.
    /// Holds data passed from the core application to the minigame at a given instance.
    /// All minigames define their own configuration class.
    /// </summary>
    public interface IGameConfiguration
    {
        /// <summary>
        /// Gets pr sets the context.
        /// </summary>
        /// <value>
        /// The context.
        /// </value>
        MiniGameData GameData { get; set; }

        /// <summary>
        /// Gets pr sets the context.
        /// </summary>
        /// <value>
        /// The context.
        /// </value>
        IGameContext Context { get; set; }

        /// <summary>
        /// Gets or sets the questions.
        /// </summary>
        /// <value>
        /// The questions.
        /// </value>
        IQuestionProvider Questions { get; set; }

        /// <summary>
        /// Return the builder that defines the rules to build question packs
        /// </summary>
        /// <returns></returns>
        IQuestionBuilder SetupBuilder();

        /// <summary>
        /// Return the rules for learning related to this minigame
        /// </summary>
        /// <returns></returns>
        MiniGameLearnRules SetupLearnRules();

        /// <summary>
        /// Setups the variation to use in the MiniGame's logic given a MiniGameCode
        /// </summary>
        void SetMiniGameCode(MiniGameCode code);

        /// <summary>
        /// Gets or sets the difficulty.
        /// </summary>
        /// <value>
        /// The difficulty.
        /// </value>
        float Difficulty { get; set; }

        /// <summary>
        /// Should this MiniGame perform the tutorial when starting?
        /// </summary>
        bool TutorialEnabled { get; set; }

        /// <summary>
        /// Is the MiniGame part of the journey?
        /// </summary>
        bool InsideJourney { get; set; }

        /// <summary>
        /// Are we ignoring journey data completely?
        /// </summary>
        bool IgnoreJourney { get; set; }

        LocalizationDataId TitleLocalizationId { get; }
        LocalizationDataId IntroLocalizationId { get; }
        LocalizationDataId TutorialLocalizationId { get; }
        bool AutoPlayIntro { get; }
    }
}
