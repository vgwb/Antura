namespace Antura.Minigames.DiscoverCountry
{
    public class DiscoverCountryIntroductionState : FSM.IState
    {
        private DiscoverCountryGame game;

        private float timer = 1f;
        public DiscoverCountryIntroductionState(DiscoverCountryGame game) { this.game = game; }

        public void EnterState()
        {
        }

        public void ExitState()
        {
            game.Context.GetAudioManager().PlayMusic(Music.Theme8);
        }

        public void Update(float delta)
        {
            timer -= delta;

            if (timer <= 0f)
            {
                game.SetCurrentState(game.QuestionState);
            }
        }

        public void UpdatePhysics(float delta) { }
    }
}
