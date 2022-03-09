namespace Antura.Minigames.FastCrowd
{
    public class FastCrowdEndState : FSM.IState
    {
        FastCrowdGame game;

        float timer = 0;
        public FastCrowdEndState(FastCrowdGame game)
        {
            this.game = game;
        }

        public void EnterState()
        {
            game.QuestionManager.wordComposer.gameObject.SetActive(false);
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
