namespace Antura.Minigames.TakeMeHome
{
    public class TakeMeHomeEndState : FSM.IState
    {

        TakeMeHomeGame game;

        float timer = 0.5f;
        public TakeMeHomeEndState(TakeMeHomeGame game)
        {
            this.game = game;
        }

        public void EnterState()
        {

        }

        public void ExitState()
        {
        }

        public void Update(float delta)
        {
            timer -= delta;

            if (timer < 0)
            {
                game.EndGame(game.CurrentStars, game.CurrentScore);
            }
        }

        public void UpdatePhysics(float delta)
        {
        }
    }
}
