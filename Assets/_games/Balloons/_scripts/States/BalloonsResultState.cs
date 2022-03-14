namespace Antura.Minigames.Balloons
{
    public class BalloonsResultState : FSM.IState
    {
        BalloonsGame game;

        float timer = 0f;
        public BalloonsResultState(BalloonsGame game)
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
                game.endGameCanvas.gameObject.SetActive(true);
                game.EndGame(game.CurrentStars, game.CurrentScore);
            }
        }

        public void UpdatePhysics(float delta)
        {
        }
    }
}
