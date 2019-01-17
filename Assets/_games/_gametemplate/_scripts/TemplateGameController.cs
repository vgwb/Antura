namespace Antura.Minigames.Template
{
    /// <summary>
    /// Sample game, to be used as a starting point for implementing new minigames.
    /// Implements a fake game with an introduction, a question, a gameplay, and a result phase.
    /// </summary>
    public class TemplateGameController : MiniGameController
    {
        public IntroductionGameState IntroductionState { get; private set; }
        public QuestionGameState QuestionState { get; private set; }
        public PlayGameState PlayState { get; private set; }
        public ResultGameState ResultState { get; private set; }

        protected override void OnInitialize(IGameContext context)
        {
            IntroductionState = new IntroductionGameState(this);
            QuestionState = new QuestionGameState(this);
            PlayState = new PlayGameState(this);
            ResultState = new ResultGameState(this);
        }

        protected override FSM.IState GetInitialState()
        {
            return IntroductionState;
        }

        protected override IGameConfiguration GetConfiguration()
        {
            return TemplateConfiguration.Instance;
        }
    }
}
