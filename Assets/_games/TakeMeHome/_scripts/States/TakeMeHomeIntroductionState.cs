using Antura.Minigames;

namespace Antura.Minigames.TakeMeHome
{
    public class TakeMeHomeIntroductionState : FSM.IState
    {
        TakeMeHomeGame game;

        public TakeMeHomeIntroductionState(TakeMeHomeGame game)
        {
            this.game = game;


        }

        public void EnterState()
        {
            game.initUI();

            game.IncrementRound();

            if (game.currentRound > 6)
            {
                game.SetCurrentState(game.EndState);
            }
        }

        public void ExitState()
        {
        }

        public void Update(float delta)
        {
            if (game.currentLetter != null && !game.currentLetter.isMoving)
            {
                game.currentLetter.sayLetter();

                game.currentLetter.isDraggable = true;
                game.SetCurrentState(game.PlayState);
            }
        }

        public void UpdatePhysics(float delta)
        {
        }
    }
}
