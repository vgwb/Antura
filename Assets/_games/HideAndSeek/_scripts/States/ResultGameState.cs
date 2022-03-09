namespace Antura.Minigames.HideAndSeek
{
    public class ResultGameState : FSM.IState
    {
        HideAndSeekGame game;

        bool goToEndGame;

        float timer = 2;
        public ResultGameState(HideAndSeekGame game)
        {
            this.game = game;
        }

        public void EnterState()
        {
            goToEndGame = false;

            if (game.isTimesUp)
            {
                game.Context.GetPopupWidget().Hide();
                timer = 0;
                goToEndGame = true;
            }
        }

        public void ExitState()
        {
        }

        public void Update(float delta)
        {
            if (!game.isTimesUp || goToEndGame)
            {
                timer -= delta;
            }

            if (timer < 0)
            {
                game.EndGame(game.CurrentStars, game.CurrentScore);
            }
        }

        public void UpdatePhysics(float delta) { }
    }
}
