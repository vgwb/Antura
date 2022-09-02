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
            get
            {
                if (instance == null)
                {
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
            TutorialEnabled = false;
        }

        public override IQuestionBuilder SetupBuilder()
        {
            IQuestionBuilder builder = null;

            int nPacks = 6;

            var builderParams = InitQuestionBuilderParamaters();
            switch (Variation)
            {
                case MazeVariation.LetterName:
                    builderParams.letterFilters.excludeDiacritics = LetterFilters.ExcludeDiacritics.All;
                    builderParams.wordFilters.excludeDiacritics = true;
                    builderParams.letterFilters.excludeLetterVariations = LetterFilters.ExcludeLetterVariations.None;
                    builder = new RandomLettersQuestionBuilder(nPacks, 1, parameters: builderParams);
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
            switch (Variation)
            {
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
