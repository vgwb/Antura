using Antura.Core;
using Antura.Database;
using Antura.LivingLetters;

namespace Antura.Assessment
{
    /// <summary>
    /// Create components for running a Assessment, components are specific
    /// according to Assessment type and some value tweak is also done here.
    ///
    /// Here Arabic assessments are configured, there is no point in
    /// over-abstracting this one. The easiest way is to create another
    /// class LanguageXAssessmentsFactory using this file as template.
    /// </summary>
    public static class ArabicAssessmentsFactory
    {
        /// <summary>
        /// Configuration variables
        /// </summary>
        private static int rounds;
        private static QuestionPlacerOptions placerOptions;

        public enum DragManagerType
        {
            Default,
            Sorting
        }

        public enum LogicInjectorType
        {
            Default,
            Sorting
        }

        public enum AnswerPlacerType
        {
            Line,
            Random
        }

        public static Assessment CreateCompleteWord_FormAssessment(AssessmentContext context)
        {
            //TODO: Maybe need a different description?
            context.GameDescription = LocalizationDataId.Assessment_Match_Letters_Words;
            AssessmentOptions.Instance.PronunceQuestionWhenClicked = true;
            AssessmentOptions.Instance.PronunceAnswerWhenClicked = true;
            AssessmentOptions.Instance.QuestionSpawnedPlaySound = true; // pronunce the complete word
            AssessmentOptions.Instance.QuestionAnsweredPlaySound = false;
            AssessmentOptions.Instance.ShowQuestionAsImage = false;
            AssessmentOptions.Instance.PlayQuestionAlsoAfterTutorial = false;
            AssessmentOptions.Instance.QuestionAnsweredFlip = false;

            Init(context);
            placerOptions.QuestionWideness = ElementsSize.Get(LivingLetterDataType.Word);
            placerOptions.AnswerWideness = ElementsSize.Get(LivingLetterDataType.Letter);
            AssessmentOptions.Instance.CompleteWordOnAnswered = true;

            CreateManagers(context,
                            DragManagerType.Default,
                            LogicInjectorType.Default,
                            AnswerPlacerType.Random
                            );

            context.QuestionGenerator = new DefaultQuestionGenerator(context.Configuration.Questions,
                                                                        context.AudioManager,
                                                                        context.Events,
                                                                        DefaultQuestionType.MissingForm);

            context.QuestionPlacer = new DefaultQuestionPlacer(null, context.AudioManager, placerOptions);

            return CreateAssessment(context);
        }

        public static Assessment CreateMatchLettersToWord_FormAssessment(AssessmentContext context)
        {
            //TODO: Maybe need a different description?
            context.GameDescription = LocalizationDataId.Assessment_Match_Letters_Words;
            AssessmentOptions.Instance.PronunceQuestionWhenClicked = true;
            AssessmentOptions.Instance.PronunceAnswerWhenClicked = true;
            AssessmentOptions.Instance.ShowQuestionAsImage = false;
            AssessmentOptions.Instance.PlayQuestionAlsoAfterTutorial = false;
            AssessmentOptions.Instance.QuestionSpawnedPlaySound = true;
            AssessmentOptions.Instance.QuestionAnsweredPlaySound = false;
            AssessmentOptions.Instance.QuestionAnsweredFlip = false;

            Init(context);
            placerOptions.QuestionWideness = ElementsSize.Get(LivingLetterDataType.Word);
            placerOptions.AnswerWideness = ElementsSize.Get(LivingLetterDataType.Letter);
            CreateManagers(context,
                            DragManagerType.Default,
                            LogicInjectorType.Default,
                            AnswerPlacerType.Random
                            );

            context.QuestionGenerator = new DefaultQuestionGenerator(context.Configuration.Questions,
                                                                        context.AudioManager,
                                                                        context.Events,
                                                                        DefaultQuestionType.VisibleForm);

            context.QuestionPlacer = new DefaultQuestionPlacer(null, context.AudioManager, placerOptions);

            return CreateAssessment(context);
        }

        public static Assessment CreateMatchWordToImageAssessment(AssessmentContext context)
        {
            // Assessment Specific configuration.
            context.GameDescription = LocalizationDataId.Assessment_Match_Word_Image;
            AssessmentOptions.Instance.PronunceQuestionWhenClicked = true;
            AssessmentOptions.Instance.PronunceAnswerWhenClicked = true;
            AssessmentOptions.Instance.ShowQuestionAsImage = true;
            AssessmentOptions.Instance.QuestionSpawnedPlaySound = true; // pronunce the word of the image
            AssessmentOptions.Instance.QuestionAnsweredPlaySound = true;
            AssessmentOptions.Instance.PlayQuestionAlsoAfterTutorial = true;
            AssessmentOptions.Instance.QuestionAnsweredFlip = false;

            // Get references from GameContext (utils)
            Init(context);
            AssessmentOptions.Instance.AnswerType = LivingLetterDataType.Word;
            placerOptions.AnswerWideness = ElementsSize.Get(LivingLetterDataType.Word);
            placerOptions.QuestionWideness = ElementsSize.Get(LivingLetterDataType.Image);

            // Instantiate the correct managers
            CreateManagers(context,
                            DragManagerType.Default,
                            LogicInjectorType.Default,
                            AnswerPlacerType.Random
                            );

            // Create the custom managers
            context.QuestionGenerator = new DefaultQuestionGenerator(context.Configuration.Questions,
                                                                        context.AudioManager,
                                                                        context.Events);
            context.QuestionPlacer = new DefaultQuestionPlacer(null, context.AudioManager, placerOptions);

            // Build the assessment
            return CreateAssessment(context);
        }

        public static Assessment CreateOrderLettersInWordAssessment(AssessmentContext context)
        {
            context.GameDescription = LocalizationDataId.Assessment_Order_Letters;
            AssessmentOptions.Instance.PronunceQuestionWhenClicked = true;
            AssessmentOptions.Instance.PronunceAnswerWhenClicked = true;
            AssessmentOptions.Instance.ShowQuestionAsImage = true;
            AssessmentOptions.Instance.QuestionSpawnedPlaySound = true; // pronunce the word to sort
            AssessmentOptions.Instance.QuestionAnsweredPlaySound = true;
            AssessmentOptions.Instance.PlayQuestionAlsoAfterTutorial = true;
            AssessmentOptions.Instance.QuestionAnsweredFlip = false;

            Init(context);
            placerOptions.AnswerWideness = ElementsSize.Get(LivingLetterDataType.Letter);
            placerOptions.QuestionWideness = ElementsSize.Get(LivingLetterDataType.Word);
            AssessmentOptions.Instance.ShowFullWordOnAnswered = true;

            CreateManagers(context,
                            DragManagerType.Sorting,
                            LogicInjectorType.Sorting,
                            AnswerPlacerType.Line
                            );


            context.QuestionGenerator = new ImageQuestionGenerator(context.Configuration.Questions, false,
                                                                    context.AudioManager,
                                                                    context.Events);
            context.QuestionPlacer = new DefaultQuestionPlacer(null, context.AudioManager, placerOptions);

            return CreateAssessment(context);
        }

        public static Assessment CreateCompleteWordAssessment(AssessmentContext context)
        {
            context.GameDescription = LocalizationDataId.Assessment_Select_Letter_Image;
            AssessmentOptions.Instance.PronunceQuestionWhenClicked = true;
            AssessmentOptions.Instance.PronunceAnswerWhenClicked = true;
            AssessmentOptions.Instance.QuestionSpawnedPlaySound = true; // pronunce the complete word
            AssessmentOptions.Instance.QuestionAnsweredPlaySound = true;
            AssessmentOptions.Instance.ShowQuestionAsImage = false;
            AssessmentOptions.Instance.PlayQuestionAlsoAfterTutorial = true;
            AssessmentOptions.Instance.QuestionAnsweredFlip = false;

            Init(context);
            placerOptions.QuestionWideness = ElementsSize.Get(LivingLetterDataType.Word);
            placerOptions.AnswerWideness = ElementsSize.Get(LivingLetterDataType.Letter);
            placerOptions.SpawnImageWithQuestion = true;
            AssessmentOptions.Instance.CompleteWordOnAnswered = true;

            CreateManagers(context,
                            DragManagerType.Default,
                            LogicInjectorType.Default,
                            AnswerPlacerType.Random
                            );

            context.QuestionGenerator = new ImageQuestionGenerator(context.Configuration.Questions, true,
                                                                    context.AudioManager, context.Events);
            context.QuestionPlacer = new DefaultQuestionPlacer(
                context.Events, context.AudioManager, placerOptions);

            return CreateAssessment(context);
        }

        public static Assessment CreateMatchLettersWordAssessment(AssessmentContext context)
        {
            context.GameDescription = LocalizationDataId.Assessment_Match_Letters_Words;
            AssessmentOptions.Instance.PronunceQuestionWhenClicked = true;
            AssessmentOptions.Instance.PronunceAnswerWhenClicked = true;
            AssessmentOptions.Instance.ShowQuestionAsImage = false;
            AssessmentOptions.Instance.PlayQuestionAlsoAfterTutorial = false;
            AssessmentOptions.Instance.QuestionSpawnedPlaySound = true;
            AssessmentOptions.Instance.QuestionAnsweredPlaySound = false;
            AssessmentOptions.Instance.QuestionAnsweredFlip = false;

            Init(context);

            placerOptions.QuestionWideness = ElementsSize.Get(LivingLetterDataType.Word);
            placerOptions.AnswerWideness = ElementsSize.Get(LivingLetterDataType.Letter);

            CreateManagers(context,
                            DragManagerType.Default,
                            LogicInjectorType.Default,
                            AnswerPlacerType.Random
                            );

            context.QuestionGenerator = new DefaultQuestionGenerator(context.Configuration.Questions,
                                                                        context.AudioManager,
                                                                        context.Events);
            context.QuestionPlacer = new DefaultQuestionPlacer(null, context.AudioManager, placerOptions);

            return CreateAssessment(context);
        }

        public static Assessment CreateQuestionAndReplyAssessment(AssessmentContext context)
        {
            context.GameDescription = LocalizationDataId.Assessment_Match_Sentences;
            AssessmentOptions.Instance.PlayQuestionAlsoAfterTutorial = false;
            AssessmentOptions.Instance.PronunceQuestionWhenClicked = true;
            AssessmentOptions.Instance.PronunceAnswerWhenClicked = true;
            AssessmentOptions.Instance.ShowQuestionAsImage = false;
            AssessmentOptions.Instance.QuestionSpawnedPlaySound = false;
            AssessmentOptions.Instance.QuestionAnsweredPlaySound = false;
            AssessmentOptions.Instance.QuestionAnsweredFlip = false;

            Init(context);
            placerOptions.AnswerWideness = ElementsSize.Get(LivingLetterDataType.Phrase);
            placerOptions.QuestionWideness = ElementsSize.Get(LivingLetterDataType.Phrase);
            AssessmentOptions.Instance.AnswerType = LivingLetterDataType.Phrase;
            AssessmentOptions.Instance.WideLL = true;
            AssessmentOptions.Instance.ReadQuestionAndAnswer = true;

            CreateManagers(context,
                            DragManagerType.Default,
                            LogicInjectorType.Default,
                            AnswerPlacerType.Random
                            );

            context.QuestionGenerator = new DefaultQuestionGenerator(context.Configuration.Questions,
                                                                        context.AudioManager,
                                                                        context.Events);

            context.QuestionPlacer = new DefaultQuestionPlacer(null, context.AudioManager, placerOptions);

            return CreateAssessment(context);
        }

        public static Assessment CreateSunMoonWordAssessment(AssessmentContext context)
        {
            context.GameDescription = LocalizationDataId.Assessment_Classify_Words_Article;
            AssessmentOptions.Instance.PronunceQuestionWhenClicked = true;
            AssessmentOptions.Instance.PronunceAnswerWhenClicked = true;
            AssessmentOptions.Instance.ShowQuestionAsImage = false;
            AssessmentOptions.Instance.PlayQuestionAlsoAfterTutorial = false;
            AssessmentOptions.Instance.QuestionSpawnedPlaySound = false;
            AssessmentOptions.Instance.QuestionAnsweredPlaySound = false;
            AssessmentOptions.Instance.QuestionAnsweredFlip = false;

            Init(context);
            placerOptions.QuestionWideness = ElementsSize.Get(LivingLetterDataType.Letter);
            placerOptions.AnswerWideness = ElementsSize.Get(LivingLetterDataType.Word);
            AssessmentOptions.Instance.AnswerType = LivingLetterDataType.Word;

            CreateManagers(context,
                            DragManagerType.Default,
                            LogicInjectorType.Default,
                            AnswerPlacerType.Random
                            );

            ArabicCategoryProvider categoryProvider = new ArabicCategoryProvider(CategoryType.SunMoon);
            context.QuestionGenerator = new CategoryQuestionGenerator(context.Configuration.Questions,
                                                                        categoryProvider,
                                                                        context.AudioManager,
                                                                        2, rounds);
            context.QuestionPlacer = new CategoryQuestionPlacer(context.AudioManager, placerOptions);

            return CreateAssessment(context);
        }

        public static Assessment CreateSingularDualPluralAssessment(AssessmentContext context)
        {
            context.GameDescription = LocalizationDataId.Assessment_Classify_Word_Nouns;
            AssessmentOptions.Instance.PronunceQuestionWhenClicked = true;
            AssessmentOptions.Instance.PronunceAnswerWhenClicked = true;
            AssessmentOptions.Instance.ShowQuestionAsImage = false;
            AssessmentOptions.Instance.PlayQuestionAlsoAfterTutorial = false;
            AssessmentOptions.Instance.QuestionSpawnedPlaySound = false;
            AssessmentOptions.Instance.QuestionAnsweredPlaySound = false;
            AssessmentOptions.Instance.QuestionAnsweredFlip = false;

            Init(context);
            placerOptions.QuestionWideness = ElementsSize.Get(LivingLetterDataType.Letter);
            placerOptions.AnswerWideness = ElementsSize.Get(LivingLetterDataType.Word);
            AssessmentOptions.Instance.AnswerType = LivingLetterDataType.Word;

            CreateManagers(context,
                            DragManagerType.Default,
                            LogicInjectorType.Default,
                            AnswerPlacerType.Random
                            );

            ArabicCategoryProvider categoryProvider = new ArabicCategoryProvider(CategoryType.SingularDualPlural);
            context.QuestionGenerator = new CategoryQuestionGenerator(context.Configuration.Questions,
                                                                        categoryProvider,
                                                                        context.AudioManager,
                                                                        2, rounds);
            context.QuestionPlacer = new CategoryQuestionPlacer(context.AudioManager, placerOptions);

            return CreateAssessment(context);
        }

        public static Assessment CreateWordArticleAssessment(AssessmentContext context)
        {
            context.GameDescription = LocalizationDataId.Assessment_Classify_Word_Article;
            AssessmentOptions.Instance.PronunceQuestionWhenClicked = true;
            AssessmentOptions.Instance.PronunceAnswerWhenClicked = true;
            AssessmentOptions.Instance.ShowQuestionAsImage = false;
            AssessmentOptions.Instance.PlayQuestionAlsoAfterTutorial = false;
            AssessmentOptions.Instance.QuestionSpawnedPlaySound = false;
            AssessmentOptions.Instance.QuestionAnsweredPlaySound = false;
            AssessmentOptions.Instance.QuestionAnsweredFlip = false;

            Init(context);
            AssessmentOptions.Instance.AnswerType = LivingLetterDataType.Word;
            placerOptions.AnswerWideness = ElementsSize.Get(LivingLetterDataType.Word);
            placerOptions.QuestionWideness = ElementsSize.Get(LivingLetterDataType.Letter);

            CreateManagers(context,
                            DragManagerType.Default,
                            LogicInjectorType.Default,
                            AnswerPlacerType.Random
                            );

            ArabicCategoryProvider categoryProvider = new ArabicCategoryProvider(CategoryType.WithOrWithoutArticle);
            context.QuestionGenerator = new CategoryQuestionGenerator(context.Configuration.Questions,
                                                                        categoryProvider,
                                                                        context.AudioManager,
                                                                        2, rounds);
            context.QuestionPlacer = new CategoryQuestionPlacer(context.AudioManager, placerOptions);

            return CreateAssessment(context);
        }

        public static Assessment CreateSunMoonLetterAssessment(AssessmentContext context)
        {
            context.GameDescription = LocalizationDataId.Assessment_Classify_Letters_Article;
            AssessmentOptions.Instance.PronunceQuestionWhenClicked = true;
            AssessmentOptions.Instance.PronunceAnswerWhenClicked = true;

            AssessmentOptions.Instance.ShowQuestionAsImage = false;
            AssessmentOptions.Instance.PlayQuestionAlsoAfterTutorial = false;
            AssessmentOptions.Instance.QuestionSpawnedPlaySound = false;
            AssessmentOptions.Instance.QuestionAnsweredPlaySound = false;
            AssessmentOptions.Instance.QuestionAnsweredFlip = false;

            Init(context);
            placerOptions.QuestionWideness = ElementsSize.Get(LivingLetterDataType.Letter);
            placerOptions.AnswerWideness = ElementsSize.Get(LivingLetterDataType.Letter);

            CreateManagers(context,
                            DragManagerType.Default,
                            LogicInjectorType.Default,
                            AnswerPlacerType.Random
                            );

            ArabicCategoryProvider categoryProvider = new ArabicCategoryProvider(CategoryType.SunMoon);
            context.QuestionGenerator = new CategoryQuestionGenerator(context.Configuration.Questions,
                                                                       categoryProvider,
                                                                       context.AudioManager,
                                                                       2, rounds);
            context.QuestionPlacer = new CategoryQuestionPlacer(context.AudioManager, placerOptions);

            return CreateAssessment(context);
        }

        public static Assessment CreateLetterAssessment(AssessmentContext context)
        {
            context.GameDescription = LocalizationDataId.Assessment_Select_Letter_Listen;
            AssessmentOptions.Instance.PronunceQuestionWhenClicked = true;
            AssessmentOptions.Instance.PronunceAnswerWhenClicked = true;
            AssessmentOptions.Instance.QuestionSpawnedPlaySound = true; // pronunce the word to sort
            AssessmentOptions.Instance.QuestionAnsweredPlaySound = true;
            AssessmentOptions.Instance.QuestionAnsweredFlip = true;
            AssessmentOptions.Instance.ShowQuestionAsImage = false;
            AssessmentOptions.Instance.PlayQuestionAlsoAfterTutorial = true;

            Init(context);
            placerOptions.QuestionWideness = ElementsSize.Get(LivingLetterDataType.Letter);
            placerOptions.AnswerWideness = ElementsSize.Get(LivingLetterDataType.Letter);

            CreateManagers(context,
                            DragManagerType.Default,
                            LogicInjectorType.Default,
                            AnswerPlacerType.Random
                            );

            context.QuestionGenerator = new DefaultQuestionGenerator(context.Configuration.Questions,
                                                                        context.AudioManager,
                                                                        context.Events);
            context.QuestionPlacer = new DefaultQuestionPlacer(null, context.AudioManager, placerOptions);

            return CreateAssessment(context);
        }

        public static Assessment CreateLetterFormAssessment(AssessmentContext context)
        {
            // TODO new MiniGame variation (these are copied from LetterForm)
            context.GameDescription = LocalizationDataId.Assessment_Select_Letter_Listen;
            AssessmentOptions.Instance.PronunceQuestionWhenClicked = true;
            AssessmentOptions.Instance.PronunceAnswerWhenClicked = true;
            AssessmentOptions.Instance.QuestionSpawnedPlaySound = true; // pronunce the word to sort
            AssessmentOptions.Instance.QuestionAnsweredPlaySound = true;
            AssessmentOptions.Instance.QuestionAnsweredFlip = true;
            AssessmentOptions.Instance.ShowQuestionAsImage = false;
            AssessmentOptions.Instance.PlayQuestionAlsoAfterTutorial = true;

            Init(context);
            placerOptions.QuestionWideness = ElementsSize.Get(LivingLetterDataType.Letter);
            placerOptions.AnswerWideness = ElementsSize.Get(LivingLetterDataType.Letter);

            CreateManagers(context,
                            DragManagerType.Default,
                            LogicInjectorType.Default,
                            AnswerPlacerType.Random
                            );

            context.QuestionGenerator = new DefaultQuestionGenerator(context.Configuration.Questions,
                                                                        context.AudioManager,
                                                                        context.Events);
            context.QuestionPlacer = new DefaultQuestionPlacer(null, context.AudioManager, placerOptions);

            return CreateAssessment(context);
        }

        public static Assessment CreatePronouncedWordAssessment(AssessmentContext context)
        {
            context.GameDescription = LocalizationDataId.Assessment_Select_Word_Listen;
            AssessmentOptions.Instance.PronunceQuestionWhenClicked = true;
            AssessmentOptions.Instance.PronunceAnswerWhenClicked = true;
            AssessmentOptions.Instance.QuestionSpawnedPlaySound = true; // pronunce the word to sort
            AssessmentOptions.Instance.QuestionAnsweredPlaySound = true;
            AssessmentOptions.Instance.QuestionAnsweredFlip = true;
            AssessmentOptions.Instance.ShowQuestionAsImage = false;
            AssessmentOptions.Instance.PlayQuestionAlsoAfterTutorial = true;

            Init(context);
            AssessmentOptions.Instance.AnswerType = LivingLetterDataType.Word;
            placerOptions.QuestionWideness = ElementsSize.Get(LivingLetterDataType.Word);
            placerOptions.AnswerWideness = ElementsSize.Get(LivingLetterDataType.Word);

            CreateManagers(context,
                            DragManagerType.Default,
                            LogicInjectorType.Default,
                            AnswerPlacerType.Random
                            );

            context.QuestionGenerator = new DefaultQuestionGenerator(context.Configuration.Questions,
                                                                        context.AudioManager,
                                                                        context.Events);
            context.QuestionPlacer = new DefaultQuestionPlacer(null, context.AudioManager, placerOptions);

            return CreateAssessment(context);
        }

        public static Assessment CreatePronouncedWordByImageAssessment(AssessmentContext context)
        {
            context.GameDescription = LocalizationDataId.Assessment_Select_Image_Listen;
            AssessmentOptions.Instance.PronunceQuestionWhenClicked = true;
            AssessmentOptions.Instance.PronunceAnswerWhenClicked = true;
            AssessmentOptions.Instance.QuestionSpawnedPlaySound = true; // pronunce the word to sort
            AssessmentOptions.Instance.QuestionAnsweredPlaySound = true;
            AssessmentOptions.Instance.QuestionAnsweredFlip = true;
            AssessmentOptions.Instance.ShowQuestionAsImage = false;
            AssessmentOptions.Instance.ShowAnswersAsImages = true;
            AssessmentOptions.Instance.PlayQuestionAlsoAfterTutorial = true;

            Init(context);
            AssessmentOptions.Instance.AnswerType = LivingLetterDataType.Image;
            placerOptions.QuestionWideness = ElementsSize.Get(LivingLetterDataType.Image);
            placerOptions.AnswerWideness = ElementsSize.Get(LivingLetterDataType.Word);

            CreateManagers(context,
                DragManagerType.Default,
                LogicInjectorType.Default,
                AnswerPlacerType.Random
            );

            context.QuestionGenerator = new DefaultQuestionGenerator(context.Configuration.Questions,
                context.AudioManager,
                context.Events);
            context.QuestionPlacer = new DefaultQuestionPlacer(null, context.AudioManager, placerOptions);

            return CreateAssessment(context);
        }

        public static Assessment CreateWordsWithLetterAssessment(AssessmentContext context)
        {
            context.GameDescription = LocalizationDataId.Assessment_Select_Words;
            AssessmentOptions.Instance.PronunceQuestionWhenClicked = true;
            AssessmentOptions.Instance.PronunceAnswerWhenClicked = true;
            AssessmentOptions.Instance.ShowQuestionAsImage = false;
            AssessmentOptions.Instance.PlayQuestionAlsoAfterTutorial = false;
            AssessmentOptions.Instance.QuestionSpawnedPlaySound = true;
            AssessmentOptions.Instance.QuestionAnsweredPlaySound = false;
            AssessmentOptions.Instance.QuestionAnsweredFlip = false;

            Init(context);

            AssessmentOptions.Instance.AnswerType = LivingLetterDataType.Word;
            placerOptions.AnswerWideness = ElementsSize.Get(LivingLetterDataType.Word);
            placerOptions.QuestionWideness = ElementsSize.Get(LivingLetterDataType.Letter);

            CreateManagers(context,
                            DragManagerType.Default,
                            LogicInjectorType.Default,
                            AnswerPlacerType.Random
                            );

            context.QuestionGenerator = new DefaultQuestionGenerator(context.Configuration.Questions,
                                                                        context.AudioManager,
                                                                        context.Events,
                                                                        DefaultQuestionType.WordsWithLetter);
            context.QuestionPlacer = new DefaultQuestionPlacer(null, context.AudioManager, placerOptions);

            return CreateAssessment(context);
        }

        /// <summary>
        /// Perform common initialization
        /// </summary>
        private static void Init(AssessmentContext context)
        {
            // ARABIC SETTINGS
            AssessmentOptions.Instance.ReadQuestionAndAnswer = false;
            AssessmentOptions.Instance.CompleteWordOnAnswered = false;
            AssessmentOptions.Instance.ShowFullWordOnAnswered = false;
            AssessmentOptions.Instance.WideLL = false;
            AssessmentOptions.Instance.AnswerType = LivingLetterDataType.Letter;
            AssessmentOptions.Instance.PlayCorrectAnswer = false;
            AssessmentOptions.Instance.PlayAllCorrectAnswers = false;

            placerOptions = QuestionPlacerOptions.Instance;
            placerOptions.AnswerWideness = ElementsSize.Get(LivingLetterDataType.Letter);
            placerOptions.AnswerWideness = ElementsSize.Get(LivingLetterDataType.Letter);

            context.Configuration = AssessmentConfiguration.Instance;
            context.Events = new AssessmentEvents();
            context.Utils = AssessmentConfiguration.Instance.Context;
            context.CheckMarkWidget = context.Utils.GetCheckmarkWidget();
            context.AudioManager = new AssessmentAudioManager(context.Utils.GetAudioManager(),
                                                               context.Utils.GetSubtitleWidget(),
                                                               context.GameDescription);

            context.AnswerChecker = new AnswerChecker(context.AudioManager, context.Events);

            rounds = AssessmentConfiguration.Instance.NumberOfRounds;
        }

        /// <summary>
        /// Create Assessment from context
        /// </summary>
        /// <param name="context"> managers used to configure the game</param>
        private static Assessment CreateAssessment(AssessmentContext context)
        {
            return new Assessment(context.AnswerPlacer, context.QuestionPlacer, context.QuestionGenerator,
                                   context.LogicInjector, context.Configuration, context.AudioManager);
        }

        /// <summary>
        /// Create managers depending on Assessment type
        /// </summary>
        private static void CreateManagers(
                AssessmentContext context,
                DragManagerType dragManager,
                LogicInjectorType logicInjector,
                AnswerPlacerType answerPlacer)
        {
            if (dragManager == DragManagerType.Default)
            {
                context.DragManager = new DefaultDragManager(context.AudioManager, context.AnswerChecker);
            }
            else
            {
                context.DragManager = new SortingDragManager(context.AudioManager, context.CheckMarkWidget);
            }

            if (logicInjector == LogicInjectorType.Default)
            {
                context.LogicInjector = new DefaultLogicInjector(context.DragManager, context.Events);
            }
            else
            {
                context.LogicInjector = new SortingLogicInjector(context.DragManager, context.Events);
            }

            if (answerPlacer == AnswerPlacerType.Line)
            {
                context.AnswerPlacer = new LineAnswerPlacer(context.AudioManager, 3);
            }
            else
            {
                context.AnswerPlacer = new OrderedAnswerPlacer(context.AudioManager, placerOptions);
            }
        }
    }
}
