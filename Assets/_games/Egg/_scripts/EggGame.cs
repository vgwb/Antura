using UnityEngine;

namespace Antura.Minigames.Egg
{
    public class EggGame : MiniGameController
    {
        public EggBox eggBox;
        public EggController eggController;
        public EggButtonsBox eggButtonBox;
        public GameObject eggButtonPrefab;
        public AnturaEggController antura;
        public GameObject letterObjectPrefab;
        public EggRunLettersBox runLettersBox;
        public GameObject anturaPrefab;
        public GameObject shadowPrefab;

        public UnityEngine.UI.Button HintButton;

        public const int numberOfStage = 4;
        public int currentStage { get; set; }

        public int correctStages { get; set; }

        public int CurrentStars
        {
            get {
                if (correctStages == 0)
                    return 0;

                if (correctStages == 1)
                    return 1;

                if (correctStages == 2 || correctStages == 3)
                    return 2;

                return 3;
            }
        }

        public float GameDifficulty
        {
            get { return EggConfiguration.Instance.Difficulty; }
        }

        public bool stagePositiveResult { get; set; }

        private bool tutorialFlag;

        public bool ShowTutorial
        {
            get {
                if (tutorialFlag) {
                    tutorialFlag = false;
                    return true;
                } else {
                    return false;
                }
            }
        }

        public EggChallenge CurrentQuestion;

        public EggIntroductionState IntroductionState { get; private set; }
        public EggQuestionState QuestionState { get; private set; }
        public EggPlayState PlayState { get; private set; }
        public EggResultState ResultState { get; private set; }

        protected override IGameConfiguration GetConfiguration()
        {
            return EggConfiguration.Instance;
        }

        protected override FSM.IState GetInitialState()
        {
            return IntroductionState;
        }

        protected override void OnInitialize(IGameContext context)
        {
            IntroductionState = new EggIntroductionState(this);
            QuestionState = new EggQuestionState(this);
            PlayState = new EggPlayState(this);
            ResultState = new EggResultState(this);

            CurrentQuestion = null;

            tutorialFlag = GetConfiguration().TutorialEnabled;
            overlayWidgetInitialized = false;

            currentStage = 0;
            correctStages = 0;

            bool isSingleVariation = EggConfiguration.Instance.IsSingleVariation();

            eggController.Initialize(letterObjectPrefab, shadowPrefab, eggBox.GetEggLocalPositions(), eggBox.GetLocalLettersMaxPositions(),
                EggConfiguration.Instance.Context.GetAudioManager());
            eggButtonBox.Initialize(eggButtonPrefab, context.GetAudioManager(), isSingleVariation ? 30 : 20, isSingleVariation);
            runLettersBox.Initialize(letterObjectPrefab, shadowPrefab);
            antura.Initialize(anturaPrefab);
        }

        private bool overlayWidgetInitialized;

        public void InitializeOverlayWidget()
        {
            if (!overlayWidgetInitialized) {
                overlayWidgetInitialized = true;
                Context.GetOverlayWidget().Initialize(true, false, false);
                Context.GetOverlayWidget().SetStarsThresholds(1, 2, 4);
            }
        }
    }
}