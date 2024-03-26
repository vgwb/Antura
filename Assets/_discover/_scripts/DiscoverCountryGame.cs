using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    public class DiscoverCountryGame : MiniGameController
    {
        private bool tutorialFlag;
        public bool ShowTutorial => tutorialFlag;
        public void EndTutorial()
        {
            tutorialFlag = false;
        }

        public bool stagePositiveResult;

        public DiscoverCountryIntroductionState IntroductionState { get; private set; }
        public DiscoverCountryQuestionState QuestionState { get; private set; }
        public DiscoverCountryPlayState PlayState { get; private set; }
        public DiscoverCountryResultState ResultState { get; private set; }

        protected override IGameConfiguration GetConfiguration()
        {
            return DiscoverCountryConfiguration.Instance;
        }

        protected override FSM.IState GetInitialState()
        {
            return IntroductionState;
        }

        protected override void OnInitialize(IGameContext context)
        {
            IntroductionState = new DiscoverCountryIntroductionState(this);
            QuestionState = new DiscoverCountryQuestionState(this);
            PlayState = new DiscoverCountryPlayState(this);
            ResultState = new DiscoverCountryResultState(this);

            tutorialFlag = GetConfiguration().TutorialEnabled;
            overlayWidgetInitialized = false;

            CurrentScore = 0;
        }

        private bool overlayWidgetInitialized;

        public void InitializeOverlayWidget()
        {
            if (!overlayWidgetInitialized)
            {
                overlayWidgetInitialized = true;
                Context.GetOverlayWidget().Initialize(true, false, false);
                Context.GetOverlayWidget().SetStarsThresholds(1, 2, 4);
            }
        }

    }
}
