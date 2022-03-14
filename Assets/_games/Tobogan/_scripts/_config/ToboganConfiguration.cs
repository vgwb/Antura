using System;
using Antura.Core;
using Antura.Database;
using Antura.LivingLetters.Sample;
using Antura.Teacher;

namespace Antura.Minigames.Tobogan
{
    public enum ToboganVariation
    {
        LetterInWord = MiniGameCode.Tobogan_letterinword,
        SunMoon = MiniGameCode.Tobogan_sunmoon
    }

    public class ToboganConfiguration : AbstractGameConfiguration
    {
        public ToboganVariation Variation { get; private set; }

        public override void SetMiniGameCode(MiniGameCode code)
        {
            Variation = (ToboganVariation)code;
        }

        // Singleton Pattern
        static ToboganConfiguration instance;
        public static ToboganConfiguration Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ToboganConfiguration();
                }
                return instance;
            }
        }

        private ToboganConfiguration()
        {
            // Default values
            Questions = new SampleQuestionProvider();
            //Questions = new SunMoonQuestionProvider();

            //Variation = ToboganVariation.SunMoon;
            Variation = ToboganVariation.LetterInWord;

            Context = new MinigamesGameContext(MiniGameCode.Tobogan_letterinword, System.DateTime.Now.Ticks.ToString());
            TutorialEnabled = true;
        }

        public override IQuestionBuilder SetupBuilder()
        {
            IQuestionBuilder builder = null;

            int nPacks = 10;
            int nCorrect = 1;
            int nWrong = 5;

            var builderParams = InitQuestionBuilderParamaters();
            switch (Variation)
            {
                case ToboganVariation.LetterInWord:
                    builderParams.wordFilters.excludeLetterVariations = true;
                    builderParams.wordFilters.excludeDipthongs = true;
                    builderParams.letterFilters.excludeDiacritics = AppManager.I.ContentEdition.DiacriticsOnlyOnIsolated ? LetterFilters.ExcludeDiacritics.All : LetterFilters.ExcludeDiacritics.None;
                    builder = new LettersInWordQuestionBuilder(nPacks, nCorrect: nCorrect, nWrong: nWrong, parameters: builderParams);
                    break;
                case ToboganVariation.SunMoon:
                    builder = new WordsBySunMoonQuestionBuilder(nPacks, parameters: builderParams);
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

        public override LetterDataSoundType GetVocabularySoundType()
        {
            LetterDataSoundType soundType;
            switch (Variation)
            {
                case ToboganVariation.LetterInWord:
                    soundType = AppManager.I.ContentEdition.PlayNameSoundWithForms ? LetterDataSoundType.Name : LetterDataSoundType.Phoneme;
                    break;
                case ToboganVariation.SunMoon:
                    soundType = LetterDataSoundType.Phoneme;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return soundType;
        }
    }
}
