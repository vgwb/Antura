using Antura.Core;
using Antura.Database;
using Antura.LivingLetters;
using Antura.LivingLetters.Sample;
using Antura.Teacher;
using System;

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
        Image = MiniGameCode.FastCrowd_image,
        CategoryForm = MiniGameCode.FastCrowd_categoryform,
        OrderedImage_Numbers = MiniGameCode.FastCrowd_orderedimage_numbers,
        OrderedImage_Colors = MiniGameCode.FastCrowd_orderedimage_colors,
        OrderedImage_Months = MiniGameCode.FastCrowd_orderedimage_months,
        OrderedImage_Days_Seasons = MiniGameCode.FastCrowd_orderedimage_days_seasons,
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
            get
            {
                if (instance == null)
                {
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
        }

        public override IQuestionBuilder SetupBuilder()
        {
            IQuestionBuilder builder = null;

            int nPacks = 10;
            int nCorrect = 4;
            int nWrong = 4;

            var builderParams = InitQuestionBuilderParamaters();

            switch (Variation)
            {
                case FastCrowdVariation.Alphabet:
                    builder = new AlphabetQuestionBuilder();
                    break;
                case FastCrowdVariation.Counting:
                    builderParams.wordFilters.allowedCategories = new[] { WordDataCategory.Numbers };
                    builder = new OrderedWordsQuestionBuilder(builderParams);
                    break;
                case FastCrowdVariation.OrderedImage_Numbers:
                    builder = CreateOrderedImageBuilder(builderParams, WordDataCategory.Numbers);
                    break;
                case FastCrowdVariation.OrderedImage_Colors:
                    builder = CreateOrderedImageBuilder(builderParams, WordDataCategory.Colors);
                    break;
                case FastCrowdVariation.OrderedImage_Months:
                    builder = CreateOrderedImageBuilder(builderParams, WordDataCategory.Months);
                    break;
                case FastCrowdVariation.OrderedImage_Days_Seasons:
                    builder = CreateOrderedImageBuilder(builderParams, WordDataCategory.Days, WordDataCategory.Seasons);
                    break;
                case FastCrowdVariation.LetterName:
                    // Only base letters
                    builderParams.letterFilters.excludeDiacritics = LetterFilters.ExcludeDiacritics.All;
                    builderParams.wrongSeverity = SelectionSeverity.AsManyAsPossible;
                    builder = new RandomLettersQuestionBuilder(nPacks, 5, 0, parameters: builderParams);
                    break;
                case FastCrowdVariation.LetterForm:
                    // @note: we pass 4 as nCorrect, so we get all the four forms of a single letter, which will be shown one after the other
                    builder = new RandomLetterAlterationsQuestionBuilder(nPacks, 4, nWrong, letterAlterationFilters: LetterAlterationFilters.FormsOfSingleLetter, parameters: builderParams, getAllSorted: true);
                    break;
                case FastCrowdVariation.CategoryForm:
                    // @note: we pass 4 as nCorrect, so we get all the four forms from a single category
                    builderParams.wordFilters.allowedCategories = new[] { WordDataCategory.Seasons };
                    builderParams.wordFilters.requireDrawings = true;
                    builder = new RandomWordsQuestionBuilder(nPacks, 4, nWrong, true, builderParams);
                    break;
                case FastCrowdVariation.BuildWord:
                    builderParams.wordFilters.requireDrawings = true;
                    builderParams.wordFilters.excludeDipthongs = true;
                    builderParams.letterFilters.includeSpecialCharacters = true;
                    builderParams.letterFilters.includeAccentedLetters = true;
                    builderParams.letterFilters.excludeDiacritics = AppManager.I.ContentEdition.DiacriticsOnlyOnIsolated ? LetterFilters.ExcludeDiacritics.All : LetterFilters.ExcludeDiacritics.None;
                    builder = new LettersInWordQuestionBuilder(7, nWrong: nWrong, useAllCorrectLetters: true, removeAccents: false,
                        parameters: builderParams);
                    break;
                case FastCrowdVariation.Word:
                case FastCrowdVariation.Image:
                    builderParams.wordFilters.requireDrawings = true;
                    builder = new RandomWordsQuestionBuilder(nPacks, nCorrect, nWrong, parameters: builderParams);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return builder;
        }

        private IQuestionBuilder CreateOrderedImageBuilder(QuestionBuilderParameters builderParams, params WordDataCategory[] categories)
        {
            builderParams.wordFilters.allowedCategories = categories;
            builderParams.wordFilters.requireDrawings = true;
            return new OrderedWordsQuestionBuilder(parameters: builderParams);
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
                case FastCrowdVariation.LetterForm:
                case FastCrowdVariation.BuildWord:
                    strictness = LetterEqualityStrictness.WithVisualForm;
                    break;
                case FastCrowdVariation.Word:
                case FastCrowdVariation.LetterName:
                case FastCrowdVariation.Counting:
                case FastCrowdVariation.Alphabet:
                case FastCrowdVariation.Image:
                case FastCrowdVariation.CategoryForm:
                case FastCrowdVariation.OrderedImage_Numbers:
                case FastCrowdVariation.OrderedImage_Colors:
                case FastCrowdVariation.OrderedImage_Months:
                case FastCrowdVariation.OrderedImage_Days_Seasons:
                    strictness = LetterEqualityStrictness.LetterBase;
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
                case FastCrowdVariation.LetterForm:
                    soundType = AppManager.I.ContentEdition.PlayNameSoundWithForms ? LetterDataSoundType.Name : LetterDataSoundType.Phoneme;
                    break;
                case FastCrowdVariation.Word:
                case FastCrowdVariation.Counting:
                case FastCrowdVariation.Alphabet:
                case FastCrowdVariation.Image:
                case FastCrowdVariation.CategoryForm:
                case FastCrowdVariation.OrderedImage_Numbers:
                case FastCrowdVariation.OrderedImage_Colors:
                case FastCrowdVariation.OrderedImage_Months:
                case FastCrowdVariation.OrderedImage_Days_Seasons:
                    soundType = LetterDataSoundType.Phoneme;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return soundType;
        }

        public bool NeedsWordComposer
        {
            get
            {
                switch (Variation)
                {
                    case FastCrowdVariation.BuildWord:
                    case FastCrowdVariation.LetterForm:
                        return true;
                    case FastCrowdVariation.Word:
                    case FastCrowdVariation.Counting:
                    case FastCrowdVariation.Alphabet:
                    case FastCrowdVariation.LetterName:
                    case FastCrowdVariation.Image:
                    case FastCrowdVariation.CategoryForm:
                    case FastCrowdVariation.OrderedImage_Numbers:
                    case FastCrowdVariation.OrderedImage_Colors:
                    case FastCrowdVariation.OrderedImage_Months:
                    case FastCrowdVariation.OrderedImage_Days_Seasons:
                        return false;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public bool WordComposerInSplitMode
        {
            get
            {
                switch (Variation)
                {
                    case FastCrowdVariation.LetterForm:
                        return true;
                    case FastCrowdVariation.LetterName:
                    case FastCrowdVariation.BuildWord:
                    case FastCrowdVariation.Word:
                    case FastCrowdVariation.Counting:
                    case FastCrowdVariation.Alphabet:
                    case FastCrowdVariation.Image:
                    case FastCrowdVariation.CategoryForm:
                    case FastCrowdVariation.OrderedImage_Numbers:
                    case FastCrowdVariation.OrderedImage_Colors:
                    case FastCrowdVariation.OrderedImage_Months:
                    case FastCrowdVariation.OrderedImage_Days_Seasons:
                        return false;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public bool NeedsFullQuestionCompleted
        {
            get
            {
                switch (Variation)
                {
                    case FastCrowdVariation.BuildWord:
                    case FastCrowdVariation.LetterForm:
                        return true;
                    case FastCrowdVariation.LetterName:
                    case FastCrowdVariation.Word:
                    case FastCrowdVariation.Counting:
                    case FastCrowdVariation.Alphabet:
                    case FastCrowdVariation.Image:
                    case FastCrowdVariation.CategoryForm:
                    case FastCrowdVariation.OrderedImage_Numbers:
                    case FastCrowdVariation.OrderedImage_Colors:
                    case FastCrowdVariation.OrderedImage_Months:
                    case FastCrowdVariation.OrderedImage_Days_Seasons:
                        return false;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public bool IsOrderingVariation =>
            Variation == FastCrowdVariation.OrderedImage_Numbers
            || Variation == FastCrowdVariation.OrderedImage_Colors
            || Variation == FastCrowdVariation.OrderedImage_Months
            || Variation == FastCrowdVariation.OrderedImage_Days_Seasons;
    }
}
