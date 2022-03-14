using System.Collections.Generic;
using Antura.LivingLetters;
using Antura.Minigames;
using UnityEngine;

namespace Antura.Minigames.Tobogan
{
    public class ToboganGame : MiniGameController
    {
        public static readonly Color32 LETTER_MARK_COLOR = new Color32(0x4C, 0xAF, 0x50, 0xFF);
        public static readonly Color32 LETTER_MARK_PIPE_COLOR = new Color32(0xFF, 0xFF, 0xFF, 0xFF);

        public Material textMaterial;
        public Material drawingMaterial;
        public Material markedTextMaterial;
        public Material markedDrawingMaterial;

        public PipesAnswerController pipesAnswerController;
        public GameObject questionLivingLetterPrefab;
        public FixedHeightShadow shadowPrefab;
        public QuestionLivingLettersBox questionLivingLetterBox;
        public Camera tubesCamera;
        public ToboganFeedbackGraphics feedbackGraphics;

        public QuestionsManager questionsManager;

        public int CurrentScoreRecord { get; private set; }

        [HideInInspector]
        public bool isTimesUp;

        public const int MAX_ANSWERS_RECORD = 15;

        const int STARS_1_THRESHOLD = 5;
        const int STARS_2_THRESHOLD = 8;
        const int STARS_3_THRESHOLD = 12;

        public IQuestionProvider SunMoonQuestions { get; set; }

        public int CurrentStars
        {
            get
            {
                if (CurrentScoreRecord < STARS_1_THRESHOLD)
                    return 0;
                if (CurrentScoreRecord < STARS_2_THRESHOLD)
                    return 1;
                if (CurrentScoreRecord < STARS_3_THRESHOLD)
                    return 2;
                return 3;
            }
        }

        bool tutorialFlag;
        public bool showTutorial { get { if (tutorialFlag) { tutorialFlag = false; return true; } else return false; } }

        public ToboganQuestionState QuestionState { get; private set; }
        public ToboganPlayState PlayState { get; private set; }
        public ToboganResultGameState ResultState { get; private set; }
        public ToboganTutorialState TutorialState { get; private set; }

        public void ResetScore()
        {
            CurrentScoreRecord = 0;
            CurrentScore = 0;
        }

        protected override IGameConfiguration GetConfiguration()
        {
            return ToboganConfiguration.Instance;
        }

        protected override FSM.IState GetInitialState()
        {
            return QuestionState;
        }

        protected override void OnInitialize(IGameContext context)
        {
            tutorialFlag = GetConfiguration().TutorialEnabled;

            pipesAnswerController.SetSignHidingProbability(Difficulty);
            SunMoonQuestions = new SunMoonTutorialQuestionProvider(ToboganConfiguration.Instance.Questions);

            QuestionState = new ToboganQuestionState(this);
            PlayState = new ToboganPlayState(this);
            ResultState = new ToboganResultGameState(this);
            TutorialState = new ToboganTutorialState(this);

            questionsManager = new QuestionsManager(this);

            feedbackGraphics.Initialize();

            feedbackGraphics.onTowerHeightIncreased += () =>
            {
                Context.GetAudioManager().PlaySound(Sfx.Transition);
            };
        }

        public void OnResult(bool result)
        {
            if (result)
                Context.GetAudioManager().PlaySound(Sfx.StampOK);
            else
            {
                Context.GetAudioManager().PlaySound(Sfx.KO);
                Context.GetAudioManager().PlaySound(Sfx.Lose);
            }

            Context.GetCheckmarkWidget().Show(result);
            feedbackGraphics.OnResult(result);

            if (result)
            {
                ++CurrentScore;
                if (CurrentScore > CurrentScoreRecord)
                    CurrentScoreRecord = CurrentScore;
            }
            else
            {
                CurrentScore = 0;
            }

            Context.GetOverlayWidget().SetStarsScore(CurrentScoreRecord);
        }

        public void InitializeOverlayWidget()
        {
            Context.GetOverlayWidget().Initialize(true, true, false);
            Context.GetOverlayWidget().SetStarsThresholds(STARS_1_THRESHOLD, STARS_2_THRESHOLD, STARS_3_THRESHOLD);
        }

        protected override Vector3 GetGravity()
        {
            return Vector3.up * (-80);
        }
    }
}
