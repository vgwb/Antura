using System;
using Antura.LivingLetters.Sample;
using Antura.Teacher;

namespace Antura.Minigames.HideAndSeek
{
    public enum HideAndSeekVariation
    {
        LetterPhoneme = MiniGameCode.HideSeek_letterphoneme,
        Image = MiniGameCode.HideSeek_image
    }

    public class HideAndSeekConfiguration : AbstractGameConfiguration
    {
        public HideAndSeekVariation Variation { get; private set; }

        public override void SetMiniGameCode(MiniGameCode code)
        {
            Variation = (HideAndSeekVariation)code;
        }

        // Singleton Pattern
        static HideAndSeekConfiguration instance;
        public static HideAndSeekConfiguration Instance
        {
            get
            {
                if (instance == null)
                    instance = new HideAndSeekConfiguration();
                return instance;
            }
        }

        private HideAndSeekConfiguration()
        {
            // Default values
            Context = new MinigamesGameContext(MiniGameCode.HideSeek_letterphoneme, System.DateTime.Now.Ticks.ToString());
            Questions = new SampleQuestionProvider();
            Difficulty = 0.5f;
            TutorialEnabled = true;
        }

        public override IQuestionBuilder SetupBuilder()
        {
            IQuestionBuilder builder = null;

            int nPacks = 10;
            int nCorrect = 1;
            int nWrong = 6;

            var builderParams = new QuestionBuilderParameters();
            switch (Variation)
            {
                case HideAndSeekVariation.LetterPhoneme:
                    var letterAlterationFilters = LetterAlterationFilters.FormsAndPhonemesOfMultipleLetters_OneForm;
                    builder = new RandomLetterAlterationsQuestionBuilder(nPacks, nCorrect, nWrong: nWrong, letterAlterationFilters: letterAlterationFilters, parameters: builderParams,
                            avoidWrongLettersWithSameSound: true);
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
