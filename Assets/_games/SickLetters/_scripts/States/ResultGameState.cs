using Antura.Keeper;

namespace Antura.Minigames.SickLetters
{
    public class ResultGameState : FSM.IState
    {
        SickLettersGame game;

        float timer = 0;
        public ResultGameState(SickLettersGame game)
        {
            this.game = game;
        }

        public void EnterState()
        {
            game.LLPrefab.jumpOut(0, true);

            if (game.scale.counter < game.targetScale)
            {
                game.manager.failure();
                timer = 6;
            }

            if (game.scale.counter >= game.targetScale)
            {
                timer = 4;
            }
        }

        public void ExitState()
        {
        }

        public void Update(float delta)
        {
            timer -= delta;

            if (timer < 0)
            {
                game.EndGame(game.CurrentStars, game.maxReachedCounter);
                game.buttonRepeater.SetActive(false);

            }
        }

        public void UpdatePhysics(float delta)
        {
        }

    }
}
