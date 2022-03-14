using Antura.Database;
using Antura.Minigames.ColorTickle;
using Antura.Teacher;
using System;

namespace Antura.Minigames.DancingDots
{
    public enum DancingDotsVariation
    {
        LetterName = MiniGameCode.DancingDots_lettername,
        LetterAny = MiniGameCode.DancingDots_letterany
    }

    public class DancingDotsConfiguration : AbstractGameConfiguration
    {
        private DancingDotsVariation Variation { get; set; }

        public override void SetMiniGameCode(MiniGameCode code)
        {
            Variation = (DancingDotsVariation)code;
        }

        // Singleton Pattern
        static DancingDotsConfiguration instance;
        public static DancingDotsConfiguration Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DancingDotsConfiguration();
                }
                return instance;
            }
        }

        private DancingDotsConfiguration()
        {
            // Default values
            Context = new MinigamesGameContext(MiniGameCode.DancingDots_lettername, DateTime.Now.Ticks.ToString());
            Variation = DancingDotsVariation.LetterName;
            Questions = new DancingDotsQuestionProvider();
            TutorialEnabled = true;
        }

        public override IQuestionBuilder SetupBuilder()
        {
            IQuestionBuilder builder = null;

            int nPacks = TutorialEnabled ? 7 : 6; // extra one for the tutorial
            int nCorrect = 1;
            int nWrong = 0;

            var builderParams = InitQuestionBuilderParamaters();

            switch (Variation)
            {
                case DancingDotsVariation.LetterName:
                    throw new NotImplementedException("This variation has been removed!");
                case DancingDotsVariation.LetterAny:
                    // This variation selects the main diacritics only
                    builderParams.letterFilters.excludeDiacritics = LetterFilters.ExcludeDiacritics.AllButMain;
                    builderParams.letterFilters.excludeLetterVariations = LetterFilters.ExcludeLetterVariations.All;
                    builderParams.letterFilters.requireDiacritics = true;
                    builder = new RandomLettersQuestionBuilder(nPacks, nCorrect, nWrong, parameters: builderParams);
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

        public override bool AutoPlayIntro => false;

    }
}
