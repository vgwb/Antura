using Antura.LivingLetters.Sample;
using Antura.Teacher;
using System;
using Antura.Database;
using Antura.Minigames.ReadingGame;

namespace Antura.Minigames.Scanner
{
    public enum ScannerVariation
    {
        OneWord = MiniGameCode.Scanner_word,
        MultipleWords = MiniGameCode.Scanner_phrase
    }

    public class ScannerConfiguration : AbstractGameConfiguration
    {
        public ScannerVariation Variation { get; private set; }

        public override void SetMiniGameCode(MiniGameCode code)
        {
            Variation = (ScannerVariation)code;
        }

        // Singleton Pattern
        static ScannerConfiguration instance;
        public static ScannerConfiguration Instance
        {
            get
            {
                if (instance == null)
                    instance = new ScannerConfiguration();
                return instance;
            }
        }

        private ScannerConfiguration()
        {
            // Default values
            Variation = ScannerVariation.OneWord;

            Questions = new SampleQuestionProvider();
            Context = new MinigamesGameContext(MiniGameCode.Scanner_word, System.DateTime.Now.Ticks.ToString());
            TutorialEnabled = true;
        }

        public int nCorrect = 1;

        public override IQuestionBuilder SetupBuilder()
        {
            IQuestionBuilder builder = null;

            int nPacks = 7; // One Extra for tutorial
            nCorrect = 1;
            int nWrong = 4;

            var builderParams = InitQuestionBuilderParamaters();
            builderParams.wordFilters.excludeColorWords = true;
            builderParams.wordFilters.requireDrawings = true;

            switch (Variation)
            {
                case ScannerVariation.OneWord:
                    nCorrect = 1;
                    nWrong = 4;
                    builder = new RandomWordsQuestionBuilder(nPacks, nCorrect, nWrong, parameters: builderParams);
                    break;
                case ScannerVariation.MultipleWords:
                    if (ScannerGame.I.Difficulty < 0.5f)
                    {
                        nCorrect = 3;
                    }
                    else
                    {
                        nCorrect = 5;
                    }
                    nWrong = 0;
                    builder = new RandomWordsQuestionBuilder(nPacks, nCorrect, nWrong, parameters: builderParams);
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
