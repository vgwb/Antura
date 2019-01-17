using Antura.Teacher;

namespace Antura.Minigames.ReadingGame
{
    // TODO: to be standardized
    public enum ReadingGameVariation
    {
        ReadAndAnswer = MiniGameCode.ReadingGame_word,
        Alphabet = MiniGameCode.Song_alphabet,
        DiacriticSong = MiniGameCode.Song_diacritics,
    }

    public class ReadingGameConfiguration : AbstractGameConfiguration
    {
        public ReadingGameVariation Variation { get; private set; }

        public override void SetMiniGameCode(MiniGameCode code)
        {
            Variation = (ReadingGameVariation)code;
        }

        // Singleton Pattern
        static ReadingGameConfiguration instance;
        public static ReadingGameConfiguration Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ReadingGameConfiguration();
                }
                return instance;
            }
        }

        private ReadingGameConfiguration()
        {
            // Default values
            Questions = new SampleReadingGameQuestionProvider();
            Variation = ReadingGameVariation.ReadAndAnswer;
            //Variation = ReadingGameVariation.AlphabetSong;

            Context = new MinigamesGameContext(MiniGameCode.ReadingGame_word, System.DateTime.Now.Ticks.ToString());
            Difficulty = 0.0f;
            TutorialEnabled = true;
        }

        public override IQuestionBuilder SetupBuilder()
        {
            IQuestionBuilder builder = null;

            var builderParams = new QuestionBuilderParameters();
            switch (Variation)
            {
                case ReadingGameVariation.Alphabet:
                case ReadingGameVariation.DiacriticSong:
                    builder = new EmptyQuestionBuilder();
                    break;
                case ReadingGameVariation.ReadAndAnswer:
                    builderParams.wordFilters.excludeColorWords = true;
                    builderParams.wordFilters.requireDrawings = true;
                    builderParams.phraseFilters.requireAnswersOrWords = true;
                    builder = new WordsInPhraseQuestionBuilder(nPacks: 10, nCorrect: 1, nWrong: 6, usePhraseAnswersIfFound: true, parameters: builderParams);
                    break;
            }
            return builder;
        }

        public override MiniGameLearnRules SetupLearnRules()
        {
            var rules = new MiniGameLearnRules();
            // example: a.minigameVoteSkewOffset = 1f;
            return rules;
        }

        public int GetDiscreteDifficulty(int maximum)
        {
            int d = (int)Difficulty * (maximum + 1);

            if (d > maximum)
            {
                return maximum;
            }
            return d;
        }

    }
}
