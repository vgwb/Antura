using System;
using Antura.LivingLetters.Sample;
using Antura.Teacher;

namespace Antura.Minigames.Balloons
{
    public enum BalloonsVariation
    {
        Spelling = MiniGameCode.Balloons_spelling,
        Words = MiniGameCode.Balloons_word,
        LetterInWord = MiniGameCode.Balloons_letterinword,
        Counting = MiniGameCode.Balloons_counting,
        Image = MiniGameCode.Balloons_image
    }

    public class BalloonsConfiguration : AbstractGameConfiguration
    {
        public BalloonsVariation Variation { get; set; }

        public override void SetMiniGameCode(MiniGameCode code)
        {
            Variation = (BalloonsVariation)code;
        }

        // Singleton Pattern
        static BalloonsConfiguration instance;
        public static BalloonsConfiguration Instance
        {
            get
            {
                if (instance == null) {
                    instance = new BalloonsConfiguration();
                }
                return instance;
            }
        }

        private BalloonsConfiguration()
        {
            // Default values
            Questions = new SampleQuestionProvider();
            Variation = BalloonsVariation.Spelling;
            TutorialEnabled = true;
            Context = new MinigamesGameContext(MiniGameCode.Balloons_spelling, System.DateTime.Now.Ticks.ToString());
            Difficulty = 0.5f;
        }

        public override IQuestionBuilder SetupBuilder()
        {
            IQuestionBuilder builder = null;

            int nPacks = 6;
            int nCorrect = 3;
            int nWrong = 8;

            var builderParams = new QuestionBuilderParameters();

            switch (Variation) {
                case BalloonsVariation.Spelling:
                    builderParams.wordFilters.excludeColorWords = true;
                    builderParams.wordFilters.requireDrawings = true;
                    builder = new LettersInWordQuestionBuilder(nPacks, useAllCorrectLetters: true, nWrong: nWrong, parameters: builderParams);
                    break;
                case BalloonsVariation.Words:
                case BalloonsVariation.Image:
                    builderParams.wordFilters.excludeColorWords = true;
                    builderParams.wordFilters.requireDrawings = true;
                    builder = new RandomWordsQuestionBuilder(nPacks, 1, nWrong, firstCorrectIsQuestion: true, parameters: builderParams);
                    break;
                case BalloonsVariation.LetterInWord:
                    builderParams.wordFilters.excludeDipthongs = true;
                    builder = new WordsWithLetterQuestionBuilder(nPacks, nPacksPerRound: 1, nCorrect: nCorrect, nWrong: nWrong, parameters: builderParams);
                    break;
                case BalloonsVariation.Counting:
                    builder = new OrderedWordsQuestionBuilder(Database.WordDataCategory.Number, null, true);
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
