using Antura.Database;
using Antura.LivingLetters.Sample;
using Antura.Teacher;
using System;

namespace Antura.Minigames.Maze
{
    public enum MazeVariation
    {
        LetterName = MiniGameCode.Maze_lettername
    }

    public class MazeConfiguration : AbstractGameConfiguration
    {
        private MazeVariation Variation { get; set; }

        public override void SetMiniGameCode(MiniGameCode code)
        {
            Variation = (MazeVariation)code;
        }

        // Singleton Pattern
        static MazeConfiguration instance;
        public static MazeConfiguration Instance
        {
            get {
                if (instance == null) {
                    instance = new MazeConfiguration();
                }
                return instance;
            }
        }

        private MazeConfiguration()
        {
            // Default values
            Questions = new SampleQuestionProvider();
            Variation = MazeVariation.LetterName;

            Context = new MinigamesGameContext(MiniGameCode.Maze_lettername, System.DateTime.Now.Ticks.ToString());
            Difficulty = 0.5f;
            TutorialEnabled = false;
        }

        public override IQuestionBuilder SetupBuilder()
        {
            IQuestionBuilder builder = null;

            var builderParams = new QuestionBuilderParameters();
            switch (Variation) {
                case MazeVariation.LetterName:
                    builderParams.letterFilters.excludeDiacritics = LetterFilters.ExcludeDiacritics.All;
                    builderParams.wordFilters.excludeDiacritics = true;
                    builderParams.letterFilters.excludeLetterVariations = LetterFilters.ExcludeLetterVariations.AllButAlefHamza;
                    builder = new RandomLettersQuestionBuilder(7, 1, parameters: builderParams);
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

        public override LetterDataSoundType GetVocabularySoundType()
        {
            LetterDataSoundType soundType;
            switch (Variation) {
                case MazeVariation.LetterName:
                    soundType = LetterDataSoundType.Name;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return soundType;
        }
    }
}
