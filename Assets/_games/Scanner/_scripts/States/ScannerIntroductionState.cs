using Antura.Audio;
using Antura.Keeper;

namespace Antura.Minigames.Scanner
{
    public class ScannerIntroductionState : FSM.IState
    {
        ScannerGame game;

        float timer = 2f;
        public ScannerIntroductionState(ScannerGame game)
        {
            this.game = game;
        }

        public void EnterState()
        {
        }

        public void ExitState()
        {
            game.PlayIntro(null);
        }

        public void Update(float delta)
        {
            timer -= delta;

            if (timer < 0)
            {
                game.SetCurrentState(game.PlayState);
                return;
            }
        }

        public void UpdatePhysics(float delta)
        {
        }
    }
}
