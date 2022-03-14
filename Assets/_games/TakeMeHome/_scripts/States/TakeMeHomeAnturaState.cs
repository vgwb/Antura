using Antura.Minigames;

namespace Antura.Minigames.TakeMeHome
{
    public class TakeMeHomeAnturaState : FSM.IState
    {
        TakeMeHomeGame game;



        public TakeMeHomeAnturaState(TakeMeHomeGame game)
        {
            this.game = game;


        }

        public void EnterState()
        {
            game.followLetter();

        }

        public void ExitState()
        {
        }

        public void Update(float delta)
        {
            if ((game.antura.transform.position - game.currentLetter.transform.position).sqrMagnitude < 100)
            {
                game.currentLetter.panicAndRun();
            }
            if (game.antura.GetComponent<TakeMeHomeAntura>().returnedToIdle())
            {
                game.SetCurrentState(game.ResetState);
            }
        }

        public void UpdatePhysics(float delta)
        {
        }
    }
}
