using System;
using Antura.LivingLetters;
using Antura.LivingLetters.Sample;
using Antura.Teacher;

namespace Antura.Minigames.TakeMeHome
{
    public enum TakeMeHomeVariation
    {
        LetterName = MiniGameCode.TakeMeHome_lettername,
    }

    public class TakeMeHomeConfiguration : AbstractGameConfiguration
    {
        private TakeMeHomeVariation Variation { get; set; }

        public override void SetMiniGameCode(MiniGameCode code)
        {
            Variation = (TakeMeHomeVariation)code;
        }

        // Singleton Pattern
        static TakeMeHomeConfiguration instance;
        public static TakeMeHomeConfiguration Instance
        {
            get
            {
                if (instance == null)
                    instance = new TakeMeHomeConfiguration();
                return instance;
            }
        }

        private TakeMeHomeConfiguration()
        {
            // Default values
            Context = new MinigamesGameContext(MiniGameCode.TakeMeHome_lettername, System.DateTime.Now.Ticks.ToString());
            Questions = new SampleQuestionProvider();
            TutorialEnabled = true;
        }

        public override IQuestionBuilder SetupBuilder()
        {
            IQuestionBuilder builder = null;

            var builderParams = InitQuestionBuilderParamaters();
            switch (Variation)
            {
                case TakeMeHomeVariation.LetterName:
                    builderParams.letterFilters.excludeDiacritics = LetterFilters.ExcludeDiacritics.All;
                    builderParams.letterFilters.excludeLetterVariations = LetterFilters.ExcludeLetterVariations.All;
                    builderParams.wordFilters.excludeDiacritics = true;
                    builderParams.wordFilters.excludeLetterVariations = true;
                    builder = new RandomLettersQuestionBuilder(1, 7, parameters: builderParams);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return builder;
        }

        public override MiniGameLearnRules SetupLearnRules()
        {
            var rules = new MiniGameLearnRules();
            // example: a.minigameVoteSkewOffset = 1f;
            return rules;
        }

    }
}
