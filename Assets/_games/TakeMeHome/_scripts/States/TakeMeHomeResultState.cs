using Antura.Audio;
using UnityEngine;

namespace Antura.Minigames.TakeMeHome
{

    public class TakeMeHomeResultState : FSM.IState
    {

        //TakeMeHomeGame game;

        public TakeMeHomeResultState(TakeMeHomeGame game)
        {
            //    this.game = game;
        }

        public void EnterState()
        {

        }

        public void ExitState()
        {
            //game.SetCurrentState(game.EndState);
        }

        public void Update(float delta)
        {

        }

        public void UpdatePhysics(float delta)
        {
        }
    }
}
