namespace Antura.Minigames.Template
{
    /// <summary>
    /// Sample game state used by the TemplateGame.
    /// Implements the play-state of a minigame, where actual gameplay is performed.
    /// </summary>
    public class PlayGameState : FSM.IState
    {
        TemplateGameController game;

        float timer = 4;
        public PlayGameState(TemplateGameController game)
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
                game.SetCurrentState(game.ResultState);
            }
        }

        public void UpdatePhysics(float delta)
        {
        }
    }
}
