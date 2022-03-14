using Antura.Minigames;
using Antura.Rewards;
using UnityEngine;

namespace Antura.Assessment
{
    /// <summary>
    /// Extends MiniGame to interact with Antura's Hub, this class creates a different
    /// Assessment instance according to which assessment code was provided and
    /// Setup the MiniGameStates.
    /// </summary>
    public class AssessmentGame : MiniGameController
    {
        [HideInInspector]
        public AssessmentVariation AssessmentVariation;

        public AssessmentIntroState IntroState { get; private set; }
        public AssessmentGameState GameState { get; private set; }
        public AssessmentResultState ResultState { get; private set; }

        private Assessment assessment;

        private Assessment CreateConfiguredAssessment(AssessmentContext context)
        {
            AssessmentOptions.Reset();

            switch (AssessmentConfiguration.Instance.Variation)
            {
                case AssessmentVariation.MatchLettersToWord:
                    return ArabicAssessmentsFactory.CreateMatchLettersWordAssessment(context);

                case AssessmentVariation.LetterName:
                    return ArabicAssessmentsFactory.CreateLetterAssessment(context);

                case AssessmentVariation.LetterAny:
                    return ArabicAssessmentsFactory.CreateLetterFormAssessment(context);

                case AssessmentVariation.WordsWithLetter:
                    return ArabicAssessmentsFactory.CreateWordsWithLetterAssessment(context);

                case AssessmentVariation.SunMoonWord:
                    return ArabicAssessmentsFactory.CreateSunMoonWordAssessment(context);

                case AssessmentVariation.SunMoonLetter:
                    return ArabicAssessmentsFactory.CreateSunMoonLetterAssessment(context);

                case AssessmentVariation.QuestionAndReply:
                    return ArabicAssessmentsFactory.CreateQuestionAndReplyAssessment(context);

                case AssessmentVariation.SelectPronouncedWord:
                    return ArabicAssessmentsFactory.CreatePronouncedWordAssessment(context);

                case AssessmentVariation.SelectPronouncedWordByImage:
                    return ArabicAssessmentsFactory.CreatePronouncedWordByImageAssessment(context);

                case AssessmentVariation.SingularDualPlural:
                    return ArabicAssessmentsFactory.CreateSingularDualPluralAssessment(context);

                case AssessmentVariation.WordArticle:
                    return ArabicAssessmentsFactory.CreateWordArticleAssessment(context);

                case AssessmentVariation.MatchWordToImage:
                    return ArabicAssessmentsFactory.CreateMatchWordToImageAssessment(context);

                case AssessmentVariation.CompleteWord:
                    return ArabicAssessmentsFactory.CreateCompleteWordAssessment(context);

                case AssessmentVariation.OrderLettersOfWord:
                    return ArabicAssessmentsFactory.CreateOrderLettersInWordAssessment(context);

                case AssessmentVariation.CompleteWord_Form:
                    return ArabicAssessmentsFactory.CreateCompleteWord_FormAssessment(context);

                case AssessmentVariation.MatchLettersToWord_Form:
                    return ArabicAssessmentsFactory.CreateMatchLettersToWord_FormAssessment(context);
            }

            return null;
        }

        protected override void OnInitialize(IGameContext gameContext)
        {
            AssessmentContext context = new AssessmentContext();
            context.Utils = gameContext;
            context.Game = this;
            assessment = CreateConfiguredAssessment(context);

            ResultState = new AssessmentResultState(this, context.AudioManager);
            GameState = new AssessmentGameState(context.DragManager, assessment, ResultState, this);
            IntroState = new AssessmentIntroState(this, GameState, context.AudioManager);
        }

        protected override FSM.IState GetInitialState()
        {
            return IntroState;
        }

        protected override IGameConfiguration GetConfiguration()
        {
            return AssessmentConfiguration.Instance;
        }

        protected override void HandleSceneSkip()
        {
            if (StateManager.CurrentState != ResultState)
            {
                ResultState.EnterState();
            }
            else
            {
                EndgameResultPanel.I.Continue();
            }
        }
    }
}
