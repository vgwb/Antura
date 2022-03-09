using Antura.Core;
using Antura.LivingLetters;
using Antura.Minigames;
using Antura.Teacher;
using System;
using Antura.Database;
using UnityEngine;

namespace Antura.Assessment
{
    public class AssessmentConfiguration : IAssessmentConfiguration
    {
        public MiniGameData GameData { get; set; }

        /// <summary>
        /// Externally provided Context: Inject all subsystems needed by this minigame
        /// </summary>
        public IGameContext Context { get; set; }

        /// <summary>
        /// Configured externally: which assessment we need to start.
        /// </summary>
        public AssessmentVariation Variation = AssessmentVariation.Unsetted;

        /// <summary>
        /// Externally provided Question provider
        /// </summary>
        private IQuestionProvider questionProvider;
        public IQuestionProvider Questions
        {
            get
            {
                return GetQuestionProvider();
            }
            set
            {
                questionProvider = value;
            }
        }

        private IQuestionProvider GetQuestionProvider()
        {
            return questionProvider;
        }

        public void SetMiniGameCode(MiniGameCode code)
        {
            Variation = (AssessmentVariation)code;
        }

        /// <summary>
        /// Setted externally: assessments will scale quantity of content (number of questions
        /// and answers mostly) linearly with this value. It is assumed that Difficulty
        /// start with 0 and increase up to 1 as long as the Child progress in the world.
        /// The difficulty should be different for each assessmentType.
        /// </summary>
        public float Difficulty { get; set; }
        public bool TutorialEnabled { get; set; }
        public bool InsideJourney { get; set; }
        public bool IgnoreJourney { get; set; }

        public LocalizationDataId TitleLocalizationId => LocalizationDataId.None;
        public LocalizationDataId IntroLocalizationId => LocalizationDataId.None;
        public LocalizationDataId TutorialLocalizationId => LocalizationDataId.None;
        public bool AutoPlayIntro => false;

        /// <summary>
        /// How many questions showed simultaneously on the screen.
        /// </summary>
        public int SimultaneosQuestions { get; private set; }

        /// <summary>
        /// How many answers should each question have. In Categorize assessments
        /// (The ones where the child should put something in the right category,
        /// like Sun/Moon) this is used to show maximum number of answers even when
        /// each question has a different number of answers (there could be 2 words
        /// to be putted in Moon, and 3 in Sun, in this case 3 placeholders are
        /// showed anyway).
        /// </summary>
        public int Answers { get; private set; } // number of answers in category questions

        /// <summary>
        /// Number of rounds, mostly fixed for each game, this value is provided externally
        /// </summary>
        public int NumberOfRounds { get { return _rounds; } set { _rounds = value; } }
        private int _rounds = 0;

        /////////////////
        // Singleton Pattern
        static AssessmentConfiguration instance;
        public static AssessmentConfiguration Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AssessmentConfiguration();
                }
                return instance;
            }
        }

        protected QuestionBuilderParameters InitQuestionBuilderParamaters()
        {
            var builderParams = new QuestionBuilderParameters { insideJourney = InsideJourney };
            builderParams.useJourneyForCorrect = !IgnoreJourney;
            builderParams.useJourneyForWrong = !IgnoreJourney;

            return builderParams;
        }

        /////////////////

        /// <summary>
        /// This is called by MiniGameAPI to create QuestionProvider, that means that if I start game
        /// from debug scene, I need a custom test Provider.
        /// </summary>
        /// <returns>Custom question data for the assessment</returns>
        public IQuestionBuilder SetupBuilder()
        {
            switch (Variation)
            {
                case AssessmentVariation.LetterName:
                    return Setup_LetterName_Builder();

                case AssessmentVariation.LetterAny:
                    return Setup_LetterAny_Builder();

                case AssessmentVariation.MatchLettersToWord:
                    return Setup_MatchLettersToWord_Builder();

                case AssessmentVariation.WordsWithLetter:
                    return Setup_WordsWithLetter_Builder();

                case AssessmentVariation.SunMoonWord:
                    return Setup_SunMoonWords_Builder();

                case AssessmentVariation.SunMoonLetter:
                    return Setup_SunMoonLetter_Builder();

                case AssessmentVariation.QuestionAndReply:
                    return Setup_QuestionAnReply_Builder();

                case AssessmentVariation.SelectPronouncedWord:
                    return Setup_SelectPronuncedWord_Builder();

                case AssessmentVariation.SelectPronouncedWordByImage:
                    return Setup_SelectPronuncedWordByImage_Builder();

                case AssessmentVariation.SingularDualPlural:
                    return Setup_SingularDualPlural_Builder();

                case AssessmentVariation.WordArticle:
                    return Setup_WordArticle_Builder();

                case AssessmentVariation.MatchWordToImage:
                    return Setup_MatchWordToImage_Builder();

                case AssessmentVariation.CompleteWord:
                    return Setup_CompleteWord_Builder();

                case AssessmentVariation.OrderLettersOfWord:
                    return Setup_OrderLettersOfWord_Builder();

                case AssessmentVariation.CompleteWord_Form:
                    return Setup_CompleteWord_Form_Builder();

                case AssessmentVariation.MatchLettersToWord_Form:
                    return Setup_MatchLettersToWord_Form_Builder();

                case AssessmentVariation.Unsetted:
                case AssessmentVariation.VowelOrConsonant:
                default:
                    throw new NotImplementedException("NotImplemented Yet!");
            }
        }

        private IQuestionBuilder Setup_CompleteWord_Form_Builder()
        {
            SimultaneosQuestions = 2;
            var builderParams = InitQuestionBuilderParamaters();
            builderParams.correctChoicesHistory = PackListHistory.RepeatWhenFull;
            builderParams.sortPacksByDifficulty = false;

            // TODO: handle forms using the returned letters instead
            return new LetterFormsInWordsQuestionBuilder(
                nPacksPerRound: SimultaneosQuestions,
                nRounds: NumberOfRounds,
                parameters: builderParams);
        }

        private IQuestionBuilder Setup_MatchLettersToWord_Form_Builder()
        {
            SimultaneosQuestions = 2;
            var builderParams = InitQuestionBuilderParamaters();
            builderParams.correctChoicesHistory = PackListHistory.RepeatWhenFull;
            builderParams.sortPacksByDifficulty = false;

            // TODO: handle forms using the returned letters instead
            return new LetterFormsInWordsQuestionBuilder(
                nPacksPerRound: SimultaneosQuestions,
                nRounds: NumberOfRounds,
                parameters: builderParams);
        }

        private IQuestionBuilder Setup_OrderLettersOfWord_Builder()
        {
            SimultaneosQuestions = 1;

            var builderParams = InitQuestionBuilderParamaters();
            builderParams.wordFilters.requireDrawings = true;
            builderParams.sortPacksByDifficulty = false;

            // Maximum number of letters depends on the screen.
            float screenRatio = Screen.width / Screen.height;
            int maxLetters = 8;

            if (screenRatio > 1.4999f)
            {
                maxLetters = 9;
            }

            if (screenRatio > 1.7777f)
            {
                maxLetters = 10;
            }

            builderParams.letterFilters.excludeDiacritics = AppManager.I.ContentEdition.DiacriticsOnlyOnIsolated ? LetterFilters.ExcludeDiacritics.All : LetterFilters.ExcludeDiacritics.None;
            return new LettersInWordQuestionBuilder(
                NumberOfRounds,
                nCorrect: 2,
                useAllCorrectLetters: true,
                parameters: builderParams,
                maximumWordLength: maxLetters
                );
        }

        private IQuestionBuilder Setup_CompleteWord_Builder()
        {
            SimultaneosQuestions = 1;

            var builderParams = InitQuestionBuilderParamaters();
            builderParams.correctChoicesHistory = PackListHistory.RepeatWhenFull;
            builderParams.wrongChoicesHistory = PackListHistory.RepeatWhenFull;
            builderParams.wrongSeverity = SelectionSeverity.MayRepeatIfNotEnough;
            builderParams.wordFilters.requireDrawings = true;
            builderParams.sortPacksByDifficulty = false;

            builderParams.letterFilters.excludeDiacritics = AppManager.I.ContentEdition.DiacriticsOnlyOnIsolated ? LetterFilters.ExcludeDiacritics.All : LetterFilters.ExcludeDiacritics.None;
            return new LettersInWordQuestionBuilder(

                SimultaneosQuestions * NumberOfRounds,  // Total Answers
                nCorrect: 1,            // Always one!
                nWrong: 4,            // WrongAnswers
                useAllCorrectLetters: false,
                parameters: builderParams);
        }

        private IQuestionBuilder Setup_MatchWordToImage_Builder()
        {
            var builderParams = InitQuestionBuilderParamaters();
            builderParams.correctChoicesHistory = PackListHistory.RepeatWhenFull;
            builderParams.wrongChoicesHistory = PackListHistory.RepeatWhenFull;
            builderParams.wrongSeverity = SelectionSeverity.MayRepeatIfNotEnough;
            builderParams.wordFilters.requireDrawings = true;
            builderParams.sortPacksByDifficulty = false;
            SimultaneosQuestions = 1;

            int nCorrect = 1;
            int nWrong = 3;

            return new RandomWordsQuestionBuilder(
                SimultaneosQuestions * NumberOfRounds,
                nCorrect,
                nWrong,
                firstCorrectIsQuestion: true,
                parameters: builderParams);
        }

        private IQuestionBuilder Setup_WordArticle_Builder()
        {
            SimultaneosQuestions = 2;

            Answers = 2;
            var builderParams = InitQuestionBuilderParamaters();
            builderParams.correctChoicesHistory = PackListHistory.RepeatWhenFull;
            builderParams.wordFilters.excludeArticles = false;
            builderParams.sortPacksByDifficulty = false;

            return new WordsByArticleQuestionBuilder(
                Answers * NumberOfRounds * 3,
                builderParams);
        }

        private IQuestionBuilder Setup_SingularDualPlural_Builder()
        {
            SimultaneosQuestions = 3;
            Answers = 2;

            var builderParams = InitQuestionBuilderParamaters();
            builderParams.correctChoicesHistory = PackListHistory.RepeatWhenFull;
            builderParams.wordFilters.excludePluralDual = false;
            builderParams.sortPacksByDifficulty = false;

            return new WordsByFormQuestionBuilder(
                SimultaneosQuestions * NumberOfRounds * 4,
                builderParams);
        }

        private IQuestionBuilder Setup_SelectPronuncedWord_Builder()
        {
            var builderParams = InitQuestionBuilderParamaters();
            builderParams.correctChoicesHistory = PackListHistory.RepeatWhenFull;
            builderParams.wrongChoicesHistory = PackListHistory.RepeatWhenFull;
            builderParams.wrongSeverity = SelectionSeverity.MayRepeatIfNotEnough;
            builderParams.sortPacksByDifficulty = false;

            SimultaneosQuestions = 1;
            int nCorrect = 1;
            int nWrong = 3;
            return new RandomWordsQuestionBuilder(
                SimultaneosQuestions * NumberOfRounds,
                nCorrect,
                nWrong,
                firstCorrectIsQuestion: true,
                parameters: builderParams);
        }

        private IQuestionBuilder Setup_SelectPronuncedWordByImage_Builder()
        {
            var builderParams = InitQuestionBuilderParamaters();
            builderParams.correctChoicesHistory = PackListHistory.RepeatWhenFull;
            builderParams.wrongChoicesHistory = PackListHistory.RepeatWhenFull;
            builderParams.wrongSeverity = SelectionSeverity.MayRepeatIfNotEnough;
            builderParams.sortPacksByDifficulty = false;
            builderParams.wordFilters.requireDrawings = true;

            SimultaneosQuestions = 1;
            int nCorrect = 1;
            int nWrong = 3;
            return new RandomWordsQuestionBuilder(
                SimultaneosQuestions * NumberOfRounds,
                nCorrect,
                nWrong,
                firstCorrectIsQuestion: true,
                parameters: builderParams);
        }

        private IQuestionBuilder Setup_QuestionAnReply_Builder()
        {
            var builderParams = InitQuestionBuilderParamaters();
            builderParams.sortPacksByDifficulty = false;

            SimultaneosQuestions = 1;
            int nWrongs = 4;

            return new PhraseQuestionsQuestionBuilder(
                        SimultaneosQuestions * NumberOfRounds, // totale questions
                        nWrongs,     // wrong additional answers
                parameters: builderParams);
        }

        private IQuestionBuilder Setup_SunMoonLetter_Builder()
        {
            SimultaneosQuestions = 2;
            Answers = 2;

            var builderParams = InitQuestionBuilderParamaters();
            builderParams.correctChoicesHistory = PackListHistory.RepeatWhenFull;
            builderParams.sortPacksByDifficulty = false;

            return new LettersBySunMoonQuestionBuilder(
                        SimultaneosQuestions * NumberOfRounds * 2,
                        builderParams
            );
        }

        private IQuestionBuilder Setup_WordsWithLetter_Builder()
        {
            // This assessment changes behaviour based on the current stage
            var jp = AppManager.I.Player.CurrentJourneyPosition;
            switch (jp.Stage)
            {
                case 1:
                    SimultaneosQuestions = 1;
                    break;
                case 2:
                case 3:
                    SimultaneosQuestions = 2;
                    break;
                case 4:
                case 5:
                case 6:
                default:
                    SimultaneosQuestions = 3;
                    break;
            }
            int nWrong = 6 - SimultaneosQuestions;

            var builderParams = InitQuestionBuilderParamaters();
            builderParams.correctChoicesHistory = PackListHistory.RepeatWhenFull;
            builderParams.wrongChoicesHistory = PackListHistory.RepeatWhenFull;
            builderParams.wrongSeverity = SelectionSeverity.MayRepeatIfNotEnough;
            builderParams.sortPacksByDifficulty = false;
            builderParams.letterEqualityStrictness = LetterEqualityStrictness.WithVisualForm;

            return new WordsWithLetterQuestionBuilder(
                NumberOfRounds,
                SimultaneosQuestions,
                1,                  // Correct Answers
                nWrong,             // Wrong Answers
                parameters: builderParams
                );

        }

        private IQuestionBuilder Setup_SunMoonWords_Builder()
        {
            var builderParams = InitQuestionBuilderParamaters();
            builderParams.sortPacksByDifficulty = false;

            SimultaneosQuestions = 2;
            Answers = 2;

            return new WordsBySunMoonQuestionBuilder(SimultaneosQuestions * NumberOfRounds * 2, parameters: builderParams);
        }

        private IQuestionBuilder Setup_MatchLettersToWord_Builder()
        {
            // This assessment changes behaviour based on the current stage
            var jp = AppManager.I.Player.CurrentJourneyPosition;
            switch (jp.Stage)
            {
                case 1:
                    SimultaneosQuestions = 1;
                    break;
                case 2:
                case 3:
                    SimultaneosQuestions = 2;
                    break;
                case 4:
                case 5:
                case 6:
                default:
                    SimultaneosQuestions = 3;
                    break;
            }
            int nWrong = 6 - SimultaneosQuestions;

            var builderParams = InitQuestionBuilderParamaters();
            builderParams.correctChoicesHistory = PackListHistory.RepeatWhenFull;
            builderParams.wrongChoicesHistory = PackListHistory.RepeatWhenFull;
            builderParams.wrongSeverity = SelectionSeverity.MayRepeatIfNotEnough;
            builderParams.sortPacksByDifficulty = false;

            builderParams.letterFilters.excludeDiacritics = AppManager.I.ContentEdition.DiacriticsOnlyOnIsolated ? LetterFilters.ExcludeDiacritics.All : LetterFilters.ExcludeDiacritics.None;
            return new LettersInWordQuestionBuilder(
                NumberOfRounds,
                SimultaneosQuestions,
                nCorrect: 1,
                nWrong: nWrong,
                useAllCorrectLetters: false,
                parameters: builderParams);
        }

        private IQuestionBuilder Setup_LetterName_Builder()
        {
            SimultaneosQuestions = 1;

            var builderParams = InitQuestionBuilderParamaters();
            builderParams.correctChoicesHistory = PackListHistory.RepeatWhenFull;
            builderParams.wrongChoicesHistory = PackListHistory.RepeatWhenFull;
            builderParams.wrongSeverity = SelectionSeverity.MayRepeatIfNotEnough;
            builderParams.sortPacksByDifficulty = false;
            builderParams.letterFilters.excludeDiacritics = LetterFilters.ExcludeDiacritics.All;

            return new RandomLettersQuestionBuilder(
                SimultaneosQuestions * NumberOfRounds,  // Total Answers
                1,                              // CorrectAnswers
                4,                              // WrongAnswers
                firstCorrectIsQuestion: true,
                parameters: builderParams);
        }

        private IQuestionBuilder Setup_LetterAny_Builder()
        {
            SimultaneosQuestions = 1;

            var builderParams = InitQuestionBuilderParamaters();
            builderParams.correctChoicesHistory = PackListHistory.RepeatWhenFull;
            builderParams.wrongChoicesHistory = PackListHistory.RepeatWhenFull;
            builderParams.wrongSeverity = SelectionSeverity.MayRepeatIfNotEnough;
            builderParams.sortPacksByDifficulty = false;

            var letterAlterationFilters = LetterAlterationFilters.FormsAndPhonemesOfMultipleLetters_OneForm;
            if (AppManager.I.ContentEdition.DiacriticsOnlyOnIsolated)
                letterAlterationFilters = LetterAlterationFilters.DiacriticsOfMultipleLetters;

            return new RandomLetterAlterationsQuestionBuilder(
                SimultaneosQuestions * NumberOfRounds,  // Total Answers
                1,                              // CorrectAnswers
                4,                              // WrongAnswers
                letterAlterationFilters: letterAlterationFilters,
                avoidWrongLettersWithSameSound: true,
                parameters: builderParams);
        }

        public MiniGameLearnRules SetupLearnRules()
        {
            switch (Variation)
            {
                case AssessmentVariation.LetterName:
                case AssessmentVariation.LetterAny:
                    return Setup_LetterAny_LearnRules();

                case AssessmentVariation.MatchLettersToWord:
                    return Setup_MatchLettersToWord_LearnRules();

                case AssessmentVariation.WordsWithLetter:
                    return Setup_WordsWithLetter_LearnRules();

                case AssessmentVariation.SunMoonWord:
                    return Setup_SunMoonWords_LearnRules();

                case AssessmentVariation.SunMoonLetter:
                    return Setup_SunMoonLetter_LearnRules();

                case AssessmentVariation.QuestionAndReply:
                    return Setup_QuestionAnReply_LearnRules();

                case AssessmentVariation.SelectPronouncedWord:
                    return Setup_SelectPronuncedWord_LearnRules();

                case AssessmentVariation.SelectPronouncedWordByImage:
                    return Setup_SelectPronuncedWordByImage_LearnRules();

                case AssessmentVariation.SingularDualPlural:
                    return Setup_SingularDualPlural_LearnRules();

                case AssessmentVariation.WordArticle:
                    return Setup_WordArticle_LearnRules();

                case AssessmentVariation.MatchWordToImage:
                    return Setup_MatchWordToImage_LearnRules();

                case AssessmentVariation.CompleteWord:
                    return Setup_CompleteWord_LearnRules();

                case AssessmentVariation.OrderLettersOfWord:
                    return Setup_OrderLettersOfWord_LearnRules();

                default:
                    throw new NotImplementedException("NotImplemented Yet!");
            }
        }

        private MiniGameLearnRules Setup_OrderLettersOfWord_LearnRules()
        {
            return new MiniGameLearnRules();
        }

        private MiniGameLearnRules Setup_CompleteWord_LearnRules()
        {
            return new MiniGameLearnRules();
        }

        private MiniGameLearnRules Setup_MatchWordToImage_LearnRules()
        {
            return new MiniGameLearnRules();
        }

        private MiniGameLearnRules Setup_WordArticle_LearnRules()
        {
            return new MiniGameLearnRules();
        }

        private MiniGameLearnRules Setup_SingularDualPlural_LearnRules()
        {
            return new MiniGameLearnRules();
        }

        private MiniGameLearnRules Setup_SelectPronuncedWord_LearnRules()
        {
            return new MiniGameLearnRules();
        }

        private MiniGameLearnRules Setup_SelectPronuncedWordByImage_LearnRules()
        {
            return new MiniGameLearnRules();
        }

        private MiniGameLearnRules Setup_QuestionAnReply_LearnRules()
        {
            return new MiniGameLearnRules();
        }

        private MiniGameLearnRules Setup_SunMoonLetter_LearnRules()
        {
            return new MiniGameLearnRules();
        }

        private MiniGameLearnRules Setup_SunMoonWords_LearnRules()
        {
            return new MiniGameLearnRules();
        }

        private MiniGameLearnRules Setup_WordsWithLetter_LearnRules()
        {
            return new MiniGameLearnRules();
        }

        private MiniGameLearnRules Setup_MatchLettersToWord_LearnRules()
        {
            return new MiniGameLearnRules();
        }

        private MiniGameLearnRules Setup_LetterAny_LearnRules()
        {
            return new MiniGameLearnRules();
        }

        public LetterDataSoundType GetVocabularySoundType()
        {
            LetterDataSoundType soundType;
            switch (Variation)
            {
                case AssessmentVariation.CompleteWord_Form:
                case AssessmentVariation.MatchLettersToWord_Form:
                case AssessmentVariation.CompleteWord:
                case AssessmentVariation.WordsWithLetter:
                case AssessmentVariation.MatchLettersToWord:
                    soundType = AppManager.I.ContentEdition.PlayNameSoundWithForms ? LetterDataSoundType.Name : LetterDataSoundType.Phoneme;
                    break;

                case AssessmentVariation.LetterName:
                    soundType = LetterDataSoundType.Name;
                    break;
                default:
                    soundType = LetterDataSoundType.Phoneme;
                    break;
            }
            return soundType;
        }

        public bool IsDataMatching(ILivingLetterData data1, ILivingLetterData data2)
        {
            LetterEqualityStrictness strictness;
            switch (Variation)
            {
                case AssessmentVariation.LetterName:
                    strictness = LetterEqualityStrictness.LetterBase;
                    break;
                case AssessmentVariation.LetterAny:
                case AssessmentVariation.MatchLettersToWord:
                case AssessmentVariation.WordsWithLetter:
                case AssessmentVariation.CompleteWord:
                case AssessmentVariation.OrderLettersOfWord:
                case AssessmentVariation.CompleteWord_Form:
                case AssessmentVariation.MatchLettersToWord_Form:
                    strictness = LetterEqualityStrictness.WithVisualForm;
                    break;
                default:
                    strictness = LetterEqualityStrictness.WithVisualForm;
                    break;
            }
            return DataMatchingHelper.IsDataMatching(data1, data2, strictness);
        }

    }
}
