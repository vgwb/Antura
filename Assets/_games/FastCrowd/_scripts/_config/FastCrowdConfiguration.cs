using Antura.Database;
using Antura.LivingLetters;
using Antura.LivingLetters.Sample;
using Antura.Teacher;
using System;
using UnityEngine.SocialPlatforms;

namespace Antura.Minigames.FastCrowd
{
    public enum FastCrowdVariation
    {
        BuildWord = MiniGameCode.FastCrowd_buildword,
        Word = MiniGameCode.FastCrowd_word,
        LetterName = MiniGameCode.FastCrowd_lettername,
        LetterForm = MiniGameCode.FastCrowd_letterform,
        Counting = MiniGameCode.FastCrowd_counting,
        Alphabet = MiniGameCode.FastCrowd_alphabet,
        Image = MiniGameCode.FastCrowd_image
    }

    public class FastCrowdConfiguration : AbstractGameConfiguration
    {
        public FastCrowdVariation Variation { get; private set; }

        public override void SetMiniGameCode(MiniGameCode code)
        {
            Variation = (FastCrowdVariation)code;
        }

        // Singleton Pattern
        static FastCrowdConfiguration instance;

        public static FastCrowdConfiguration Instance
        {
            get {
                if (instance == null) {
                    instance = new FastCrowdConfiguration();
                }
                return instance;
            }
        }

        private FastCrowdConfiguration()
        {
            // Default values
            Questions = new SampleQuestionProvider();
            //Variation = FastCrowdVariation.Letter;
            //Variation = FastCrowdVariation.Alphabet;
            Variation = FastCrowdVariation.BuildWord;

            //Questions = new SampleQuestionWithWordsProvider();
            //Variation = FastCrowdVariation.Counting;

            //Questions = new SampleQuestionWordsVariationProvider();
            //Variation = FastCrowdVariation.Words;
            TutorialEnabled = true;

            Context = new MinigamesGameContext(MiniGameCode.FastCrowd_buildword, System.DateTime.Now.Ticks.ToString());
            Difficulty = 0.5f;
        }

        public override IQuestionBuilder SetupBuilder()
        {
            IQuestionBuilder builder = null;

            int nPacks = 10;
            int nCorrect = 4;
            int nWrong = 4;

            var builderParams = new QuestionBuilderParameters();

            switch (Variation) {
                case FastCrowdVariation.Alphabet:
                    builder = new AlphabetQuestionBuilder();
                    break;
                case FastCrowdVariation.Counting:
                    builder = new OrderedWordsQuestionBuilder(WordDataCategory.Number, builderParams, false);
                    break;
                case FastCrowdVariation.LetterName:
                    // Only base letters
                    builderParams.letterFilters.excludeDiacritics = LetterFilters.ExcludeDiacritics.All;
                    builderParams.wrongSeverity = SelectionSeverity.AsManyAsPossible;
                    builder = new RandomLettersQuestionBuilder(nPacks, 5, 0, parameters: builderParams);
                    break;
                case FastCrowdVariation.LetterForm:
                    // @note: we pass 4 as nCorrect, so we get all the four forms of a single letter, which will be shown one after the other
                    builder = new RandomLetterAlterationsQuestionBuilder(nPacks, 4, nWrong, letterAlterationFilters: LetterAlterationFilters.FormsOfSingleLetter);
                    break;
                case FastCrowdVariation.BuildWord:
                    builderParams.wordFilters.excludeColorWords = true;
                    builderParams.wordFilters.requireDrawings = true;
                    builderParams.wordFilters.excludeDipthongs = true;
                    builder = new LettersInWordQuestionBuilder(nPacks, nWrong: nWrong, useAllCorrectLetters: true,
                        parameters: builderParams);
                    break;
                case FastCrowdVariation.Word:
                case FastCrowdVariation.Image:
                    builderParams.wordFilters.excludeColorWords = true;
                    builderParams.wordFilters.requireDrawings = true;
                    builder = new RandomWordsQuestionBuilder(nPacks, nCorrect, nWrong, parameters: builderParams);
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
            switch (Variation) {
                case FastCrowdVariation.LetterForm:
                    strictness = LetterEqualityStrictness.WithVisualForm;
                    break;
                case FastCrowdVariation.BuildWord:
                case FastCrowdVariation.Word:
                case FastCrowdVariation.LetterName:
                case FastCrowdVariation.Counting:
                case FastCrowdVariation.Alphabet:
                case FastCrowdVariation.Image:
                    strictness = LetterEqualityStrictness.LetterOnly;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return DataMatchingHelper.IsDataMatching(data1, data2, strictness);
        }

        public override LetterDataSoundType GetVocabularySoundType()
        {
            LetterDataSoundType soundType;
            switch (Variation)
            {
                case FastCrowdVariation.LetterName:
                    soundType = LetterDataSoundType.Name;
                    break;
                case FastCrowdVariation.BuildWord:
                case FastCrowdVariation.Word:
                case FastCrowdVariation.LetterForm:
                case FastCrowdVariation.Counting:
                case FastCrowdVariation.Alphabet:
                case FastCrowdVariation.Image:
                    soundType = LetterDataSoundType.Phoneme;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return soundType;
        }

        public override LocalizationDataId TitleLocalizationId
        {
            get {
                switch (Variation) {
                    case FastCrowdVariation.BuildWord:
                        return LocalizationDataId.FastCrowd_buildword_Title;
                    case FastCrowdVariation.Word:
                        return LocalizationDataId.FastCrowd_word_Title;
                    case FastCrowdVariation.LetterName:
                        return LocalizationDataId.FastCrowd_letterform_Title;
                    case FastCrowdVariation.LetterForm:
                        return LocalizationDataId.FastCrowd_letterform_Title;   // TODO: add the correct one here
                    case FastCrowdVariation.Counting:
                        return LocalizationDataId.FastCrowd_counting_Title;
                    case FastCrowdVariation.Alphabet:
                        return LocalizationDataId.FastCrowd_lettername_Title;
                    case FastCrowdVariation.Image:
                        return LocalizationDataId.FastCrowd_word_Title; // TODO: correct one
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public LocalizationDataId IntroLocalizationId
        {
            get {
                switch (Variation) {
                    case FastCrowdVariation.BuildWord:
                        return LocalizationDataId.FastCrowd_buildword_Intro;
                    case FastCrowdVariation.Word:
                        return LocalizationDataId.FastCrowd_word_Intro;
                    case FastCrowdVariation.LetterName:
                        return LocalizationDataId.FastCrowd_letterform_Intro;
                    case FastCrowdVariation.LetterForm:
                        return LocalizationDataId.FastCrowd_letterform_Intro;   // TODO: add the correct one here
                    case FastCrowdVariation.Counting:
                        return LocalizationDataId.FastCrowd_counting_Intro;
                    case FastCrowdVariation.Alphabet:
                        return LocalizationDataId.FastCrowd_lettername_Intro;
                    case FastCrowdVariation.Image:
                        return LocalizationDataId.FastCrowd_word_Intro;  // TODO: add the correct one here
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public bool NeedsWordComposer
        {
            get {
                switch (Variation) {
                    case FastCrowdVariation.BuildWord:
                    case FastCrowdVariation.LetterForm:
                        return true;
                    case FastCrowdVariation.Word:
                    case FastCrowdVariation.Counting:
                    case FastCrowdVariation.Alphabet:
                    case FastCrowdVariation.LetterName:
                    case FastCrowdVariation.Image:
                        return false;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public bool WordComposerInSplitMode
        {
            get {
                switch (Variation) {
                    case FastCrowdVariation.LetterForm:
                        return true;
                    case FastCrowdVariation.LetterName:
                    case FastCrowdVariation.BuildWord:
                    case FastCrowdVariation.Word:
                    case FastCrowdVariation.Counting:
                    case FastCrowdVariation.Alphabet:
                    case FastCrowdVariation.Image:
                        return false;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public bool NeedsFullQuestionCompleted
        {
            get {
                switch (Variation) {
                    case FastCrowdVariation.BuildWord:
                    case FastCrowdVariation.LetterForm:
                        return true;
                    case FastCrowdVariation.LetterName:
                    case FastCrowdVariation.Word:
                    case FastCrowdVariation.Counting:
                    case FastCrowdVariation.Alphabet:
                    case FastCrowdVariation.Image:
                        return false;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public LocalizationDataId TutorialLocalizationId
        {
            get {
                switch (Variation) {
                    case FastCrowdVariation.BuildWord:
                        return LocalizationDataId.FastCrowd_buildword_Tuto;
                    case FastCrowdVariation.Word:
                        return LocalizationDataId.FastCrowd_word_Tuto;
                    case FastCrowdVariation.LetterName:
                        return LocalizationDataId.FastCrowd_letterform_Tuto;
                    case FastCrowdVariation.LetterForm:
                        return LocalizationDataId.FastCrowd_letterform_Tuto;   // TODO: add the correct one here
                    case FastCrowdVariation.Counting:
                        return LocalizationDataId.FastCrowd_counting_Tuto;
                    case FastCrowdVariation.Alphabet:
                        return LocalizationDataId.FastCrowd_lettername_Tuto;
                    case FastCrowdVariation.Image:
                        return LocalizationDataId.FastCrowd_word_Tuto;  // TODO: add the correct one here
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}
