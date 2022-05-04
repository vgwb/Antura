using System;
using Antura.Core;
using Antura.Database;
using Antura.LivingLetters;
using Antura.LivingLetters.Sample;
using Antura.Teacher;

namespace Antura.Minigames.MixedLetters
{
    public enum MixedLettersVariation
    {
        Alphabet = MiniGameCode.MixedLetters_alphabet,
        BuildWord = MiniGameCode.MixedLetters_buildword
    }

    public class MixedLettersConfiguration : AbstractGameConfiguration
    {
        public MixedLettersVariation Variation { get; private set; }

        public override void SetMiniGameCode(MiniGameCode code)
        {
            Variation = (MixedLettersVariation)code;
        }

        // Singleton Pattern
        static MixedLettersConfiguration instance;
        public static MixedLettersConfiguration Instance
        {
            get
            {
                if (instance == null)
                    instance = new MixedLettersConfiguration();
                return instance;
            }
        }

        private MixedLettersConfiguration()
        {
            // Default values
            Questions = new SampleQuestionProvider();
            Variation = MixedLettersVariation.Alphabet;
            Context = new MinigamesGameContext(MiniGameCode.MixedLetters_alphabet, DateTime.Now.Ticks.ToString());
            TutorialEnabled = true;
        }

        public override IQuestionBuilder SetupBuilder()
        {
            IQuestionBuilder builder = null;

            var builderParams = InitQuestionBuilderParamaters();
            switch (Variation)
            {
                case MixedLettersVariation.Alphabet:
                    builderParams.useJourneyForCorrect = false; // Force no journey, or the minigame will block
                    builder = new AlphabetQuestionBuilder(parameters: builderParams);
                    break;
                case MixedLettersVariation.BuildWord:
                    builderParams.wordFilters.excludeDipthongs = true;
                    builderParams.wordFilters.requireDrawings = true;
                    builderParams.letterFilters.includeSpecialCharacters = true;
                    builderParams.letterFilters.excludeDiacritics = AppManager.I.ContentEdition.DiacriticsOnlyOnIsolated ? LetterFilters.ExcludeDiacritics.All : LetterFilters.ExcludeDiacritics.None;
                    builder = new LettersInWordQuestionBuilder(6, maximumWordLength: 8, useAllCorrectLetters: true, parameters: builderParams);
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

        public override bool IsDataMatching(ILivingLetterData data1, ILivingLetterData data2)
        {
            LetterEqualityStrictness strictness;
            switch (Variation)
            {
                case MixedLettersVariation.Alphabet:
                case MixedLettersVariation.BuildWord:
                    strictness = LetterEqualityStrictness.WithVisualForm;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return DataMatchingHelper.IsDataMatching(data1, data2, strictness);
        }

        public override bool AutoPlayIntro => false;

        public override LetterDataSoundType GetVocabularySoundType()
        {
            LetterDataSoundType soundType;
            switch (Variation)
            {
                case MixedLettersVariation.Alphabet:
                    soundType = LetterDataSoundType.Name;
                    break;
                case MixedLettersVariation.BuildWord:
                    soundType = AppManager.I.ContentEdition.PlayNameSoundWithForms ? LetterDataSoundType.Name : LetterDataSoundType.Phoneme;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return soundType;
        }
    }
}
