namespace Antura.Minigames.Template
{
    /// <summary>
    /// Sample game state used by the TemplateGame.
    /// Implements a phase in whic the results of the play session are gathered.
    /// Note that EndGame is called to enter the final common state: OutcomeState.
    /// </summary>
    public class ResultGameState : FSM.IState
    {
        TemplateGameController game;

        float timer = 4;
        public ResultGameState(TemplateGameController game)
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
                game.EndGame(2, 100);
            }
        }

        public void UpdatePhysics(float delta)
        {
        }
    }
}
