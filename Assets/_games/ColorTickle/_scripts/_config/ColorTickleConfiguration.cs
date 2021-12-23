using Antura.Database;
using Antura.Teacher;
using System;

namespace Antura.Minigames.ColorTickle
{
    public enum ColorTickleVariation
    {
        LetterName = MiniGameCode.ColorTickle_lettername,
        Image = MiniGameCode.ColorTickle_image
    }

    public class ColorTickleConfiguration : AbstractGameConfiguration
    {
        public ColorTickleVariation Variation { get; set; }

        public override void SetMiniGameCode(MiniGameCode code)
        {
            Variation = (ColorTickleVariation)code;
        }

        // Singleton Pattern
        static ColorTickleConfiguration instance;
        public static ColorTickleConfiguration Instance
        {
            get {
                if (instance == null) {
                    instance = new ColorTickleConfiguration();
                }
                return instance;
            }
        }

        private ColorTickleConfiguration()
        {
            // Default values
            Questions = new ColorTickleLetterProvider();
            Context = new MinigamesGameContext(MiniGameCode.ColorTickle_lettername, System.DateTime.Now.Ticks.ToString());
            TutorialEnabled = true;
            Variation = ColorTickleVariation.LetterName;
        }

        public override IQuestionBuilder SetupBuilder()
        {
            IQuestionBuilder builder = null;

            int nPacks = 6;
            int nCorrect = 1;

            var builderParams = InitQuestionBuilderParamaters();
            switch (Variation) {
                case ColorTickleVariation.LetterName:
                    builderParams.letterFilters.excludeDiacritics = LetterFilters.ExcludeDiacritics.All;
                    builderParams.letterFilters.excludeLetterVariations = LetterFilters.ExcludeLetterVariations.AllButAlefHamza;
                    builderParams.letterFilters.excludeDiphthongs = true;
                    builderParams.wordFilters.excludeDiacritics = true;
                    builder = new RandomLettersQuestionBuilder(nPacks, nCorrect, parameters: builderParams);
                    break;
                case ColorTickleVariation.Image:
                    builderParams.wordFilters.excludeColorWords = true;
                    builderParams.wordFilters.excludeDiacritics = true;
                    builderParams.wordFilters.requireDrawings = true;
                    builder = new RandomWordsQuestionBuilder(nPacks, nCorrect, parameters: builderParams);
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
                case ColorTickleVariation.LetterName:
                case ColorTickleVariation.Image:
                    soundType = LetterDataSoundType.Name;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return soundType;
        }

        public override bool AutoPlayIntro => false;

    }
}
