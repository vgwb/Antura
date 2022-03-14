using Antura.Audio;
using Antura.Keeper;

namespace Antura.Minigames.Scanner
{
    public class ScannerResultState : FSM.IState
    {
        ScannerGame game;

        float timer = 2;

        public ScannerResultState(ScannerGame game)
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
                game.EndGame(game.CurrentStars, game.CurrentScoreRecord);

                if (game.CurrentStars == 0)
                    KeeperManager.I.PlayDialogue("Reward_0Star");
                else
                    KeeperManager.I.PlayDialogue("Reward_" + game.CurrentStars + "Star_" + UnityEngine.Random.Range(1, 4));
            }
        }

        public void UpdatePhysics(float delta)
        {
        }
    }
}
