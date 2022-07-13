using Antura.Database;
using Antura.LivingLetters.Sample;
using Antura.Teacher;
using System;
using Antura.Core;

namespace Antura.Minigames.MissingLetter
{
    public enum MissingLetterVariation
    {
        Phrase = MiniGameCode.MissingLetter_phrase,
        LetterForm = MiniGameCode.MissingLetter_letterform,
        LetterInWord = MiniGameCode.MissingLetter_letterinword,
        Image = MiniGameCode.MissingLetter_image
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
            get
            {
                if (instance == null)
                {
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

            var builderParams = InitQuestionBuilderParamaters();

            switch (Variation)
            {
                case MissingLetterVariation.LetterInWord:
                    // Find a letter with the given form inside the word (no diacritics inside the form)
                    // wrong answers are other letters in different forms & diacritics
                    builderParams.wordFilters.excludeDipthongs = true;
                    builderParams.letterFilters.excludeDiphthongs = true;
                    var letterAlterationFilters = LetterAlterationFilters.FormsAndPhonemesOfMultipleLetters;
                    if (AppManager.I.ContentEdition.DiacriticsOnlyOnIsolated)
                        letterAlterationFilters = LetterAlterationFilters.FormsOfMultipleLetters;
                    builder = new LetterAlterationsInWordsQuestionBuilder(nPacks, 1, parameters: builderParams, keepBasesOnly: false, letterAlterationFilters: letterAlterationFilters);
                    break;

                case MissingLetterVariation.LetterForm:
                    // Find the correct form of the letter in the given word
                    // wrong answers are the other forms of the same letter (not the same visually, tho)
                    builder = new LetterAlterationsInWordsQuestionBuilder(nPacks, 1, parameters: builderParams, keepBasesOnly: false, letterAlterationFilters: LetterAlterationFilters.VisualFormsOfSingleLetter);
                    break;

                case MissingLetterVariation.Phrase:
                    builderParams.phraseFilters.requireWords = true;
                    builderParams.phraseFilters.maxWords = 6;
                    builderParams.wordFilters.requireDrawings = true;
                    builderParams.wordFilters.excludeSpaces = true;
                    builder = new WordsInPhraseQuestionBuilder(nPacks, nCorrect, nWrong, usePhraseAnswersIfFound: true, parameters: builderParams);
                    break;

                case MissingLetterVariation.Image:
                    builderParams.wordFilters.requireDrawings = true;
                    builder = new RandomWordsQuestionBuilder(nPacks, nCorrect, nWrong, firstCorrectIsQuestion: true, parameters: builderParams);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
            return builder;
        }

        public override bool AutoPlayIntro => false;

        public override MiniGameLearnRules SetupLearnRules()
        {
            var rules = new MiniGameLearnRules();
            // example: a.minigameVoteSkewOffset = 1f;
            return rules;
        }

        public bool VariationIsMissingLetter
        {
            get
            {
                return Variation == MissingLetterVariation.LetterForm ||
                       Variation == MissingLetterVariation.LetterInWord;
            }
        }

        public override LetterDataSoundType GetVocabularySoundType()
        {
            LetterDataSoundType soundType;
            switch (Variation)
            {

                case MissingLetterVariation.LetterForm:
                case MissingLetterVariation.LetterInWord:
                    soundType = AppManager.I.ContentEdition.PlayNameSoundWithForms ? LetterDataSoundType.Name : LetterDataSoundType.Phoneme;
                    break;
                default:
                    soundType = LetterDataSoundType.Phoneme;
                    break;
            }
            return soundType;
        }
    }
}
