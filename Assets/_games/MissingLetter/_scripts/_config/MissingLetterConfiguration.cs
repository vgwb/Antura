using Antura.Database;
using Antura.LivingLetters.Sample;
using Antura.Teacher;
using System;

namespace Antura.Minigames.MissingLetter
{
    public enum MissingLetterVariation
    {
        Phrase = MiniGameCode.MissingLetter_phrase,
        LetterForm = MiniGameCode.MissingLetter_letterform,
        LetterInWord = MiniGameCode.MissingLetter_letterinword
    }

    public class MissingLetterConfiguration : AbstractGameConfiguration
    {
        public MissingLetterVariation Variation { get; private set; }

        public override void SetMiniGameCode(MiniGameCode code)
        {
            Variation = (MissingLetterVariation)code;
        }

        // Singleton Pattern
        static MissingLetterConfiguration instance;
        public static MissingLetterConfiguration Instance
        {
            get {
                if (instance == null) {
                    instance = new MissingLetterConfiguration();
                }
                return instance;
            }
        }

        private MissingLetterConfiguration()
        {
            // Default values
            // THESE SETTINGS ARE FOR SAMPLE PURPOSES, THESE VALUES MUST BE SET BY GAME CORE
            Questions = new SampleQuestionProvider();
            Context = new MinigamesGameContext(MiniGameCode.MissingLetter_letterinword, System.DateTime.Now.Ticks.ToString());

            Difficulty = 0.5f;
            //Variation = MissingLetterVariation.MissingLetter;
            Variation = MissingLetterVariation.LetterInWord;
            TutorialEnabled = true;
        }

        public int N_ROUNDS = 15;   // a few more than the base line to allow for more errors

        public override IQuestionBuilder SetupBuilder()
        {
            IQuestionBuilder builder = null;

            int nPacks = N_ROUNDS;
            int nCorrect = 1;
            int nWrong = 5;

            var builderParams = new QuestionBuilderParameters();

            switch (Variation) {
                case MissingLetterVariation.LetterInWord:
                    // Find a letter with the given form inside the word (no diacritics)
                    // wrong answers are other letters in different forms & diacritics
                    builderParams.letterFilters.excludeDiacritics = LetterFilters.ExcludeDiacritics.All;
                    builderParams.letterFilters.excludeDiphthongs = true;
                    builderParams.wordFilters.excludeDipthongs = true;
                    builder = new LetterAlterationsInWordsQuestionBuilder(nPacks, 1, parameters: builderParams, letterAlterationFilters: LetterAlterationFilters.FormsAndPhonemesOfMultipleLetters);
                    break;

                case MissingLetterVariation.LetterForm:
                    // Find the correct form of the letter in the given word
                    // wrong answers are the other forms of the same letter (not the same visually, tho)
                    builder = new LetterAlterationsInWordsQuestionBuilder(nPacks, 1, parameters: builderParams, letterAlterationFilters: LetterAlterationFilters.VisualFormsOfSingleLetter);
                    break;

                case MissingLetterVariation.Phrase:
                    builderParams.phraseFilters.requireWords = true;
                    builderParams.phraseFilters.requireAtLeastTwoWords = true;
                    builder = new WordsInPhraseQuestionBuilder(nPacks, nCorrect, nWrong, parameters: builderParams);
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

        public override LocalizationDataId TitleLocalizationId
        {
            get {
                switch (Variation) {
                    case MissingLetterVariation.Phrase:
                        return LocalizationDataId.MissingLetter_phrases_Title;
                    case MissingLetterVariation.LetterForm:
                        return LocalizationDataId.MissingLetter_letterform_Title;
                    case MissingLetterVariation.LetterInWord:
                        return LocalizationDataId.MissingLetter_letterinword_Title;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}
