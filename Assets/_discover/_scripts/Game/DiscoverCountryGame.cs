namespace Antura.Minigames.DiscoverCountry
{
    public class DiscoverCountryGame : MiniGameController
    {
        #region Minigame Controller Setup
        public DiscoverCountryPlayState PlayState { get; private set; }

        protected override IGameConfiguration GetConfiguration()
        {
            return DiscoverCountryConfiguration.Instance;
        }

        protected override FSM.IState GetInitialState()
        {
            return PlayState;
        }

        protected override void OnInitialize(IGameContext context)
        {
            PlayState = new DiscoverCountryPlayState();
            CurrentScore = 0;
        }
        #endregion
    }
}
