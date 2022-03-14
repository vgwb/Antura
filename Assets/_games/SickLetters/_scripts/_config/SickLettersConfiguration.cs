using Antura.Database;
using Antura.Teacher;
using System;

namespace Antura.Minigames.SickLetters
{
    public enum SickLettersVariation
    {
        LetterName = MiniGameCode.SickLetters_lettername,
    }

    public class SickLettersConfiguration : AbstractGameConfiguration
    {
        private SickLettersVariation Variation { get; set; }

        public override void SetMiniGameCode(MiniGameCode code)
        {
            Variation = (SickLettersVariation)code;
        }

        // Singleton Pattern
        static SickLettersConfiguration instance;
        public static SickLettersConfiguration Instance
        {
            get
            {
                if (instance == null)
                    instance = new SickLettersConfiguration();
                return instance;
            }
        }

        private SickLettersConfiguration()
        {
            // Default values
            Context = new MinigamesGameContext(MiniGameCode.SickLetters_lettername, System.DateTime.Now.Ticks.ToString());
            Questions = new SickLettersQuestionProvider();
            TutorialEnabled = true;
        }

        public override IQuestionBuilder SetupBuilder()
        {
            IQuestionBuilder builder = null;

            int nPacks = 20;
            int nCorrect = 1;
            int nWrong = 0;

            var builderParams = InitQuestionBuilderParamaters();
            switch (Variation)
            {
                case SickLettersVariation.LetterName:
                    builderParams.letterFilters.excludeDiacritics = LetterFilters.ExcludeDiacritics.All;
                    builderParams.letterFilters.excludeLetterVariations = LetterFilters.ExcludeLetterVariations.All;
                    builderParams.letterFilters.excludeDiphthongs = true;
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

        public override LetterDataSoundType GetVocabularySoundType()
        {
            return LetterDataSoundType.Name;
        }

        public override bool AutoPlayIntro => false;

    }
}
