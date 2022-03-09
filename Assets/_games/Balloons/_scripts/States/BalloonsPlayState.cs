using Antura.UI;

namespace Antura.Minigames.Balloons
{
    public class BalloonsPlayState : FSM.IState
    {
        BalloonsGame game;

        public BalloonsPlayState(BalloonsGame game)
        {
            this.game = game;
        }

        public void EnterState()
        {
            game.InitializeMinigameUI();

            game.Play(true);
        }

        public void ExitState()
        {
        }

        public void OnResult()
        {
            game.SetCurrentState(game.ResultState);
        }

        public void Update(float delta)
        {
        }

        public void UpdatePhysics(float delta)
        {
        }
    }
}
