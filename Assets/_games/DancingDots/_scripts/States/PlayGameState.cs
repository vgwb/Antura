using Antura.Keeper;

namespace Antura.Minigames.DancingDots
{
    public class PlayGameState : FSM.IState
    {
        DancingDotsGame game;

        float timer;
        bool alarmIsTriggered = false;

        public PlayGameState(DancingDotsGame game)
        {
            this.game = game;
        }

        public void EnterState()
        {
            game.disableInput = false;
            game.dancingDotsLL.contentGO.SetActive(true);
            game.StartRound();
            timer = game.gameDuration;
        }

        public void ExitState()
        {
            //game.DancingDotsEndGame();
        }

        public void Update(float delta)
        {
            if (!game.isTutRound)
            {
                timer -= delta;
                game.Context.GetOverlayWidget().SetClockTime(timer);
            }

            if (timer < 0)
            {
                game.Context.GetOverlayWidget().OnClockCompleted();
                game.SetCurrentState(game.ResultState);
                game.Context.GetAudioManager().PlayDialogue(Database.LocalizationDataId.Keeper_TimeUp);
            }
            else if (!alarmIsTriggered && timer < 20)
            {
                alarmIsTriggered = true;
                KeeperManager.I.PlayDialogue("Keeper_Time_" + UnityEngine.Random.Range(1, 4));
            }
        }

        public void UpdatePhysics(float delta)
        {
        }
    }
}
