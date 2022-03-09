using Antura.Minigames;

namespace Antura.Minigames.FastCrowd
{
    public class FastCrowdResultState : FSM.IState
    {
        FastCrowdGame game;

        public FastCrowdResultState(FastCrowdGame game)
        {
            this.game = game;
        }

        public void EnterState()
        {
            if (game.CurrentChallenge != null)
            {
                if (!game.ShowChallengePopupWidget(true, OnPopupCloseRequested))
                    OnPopupCloseRequested();
            }
        }

        void OnPopupCloseRequested()
        {
            if (game.CurrentStars == 3)
                game.SetCurrentState(game.EndState);
            else
                game.SetCurrentState(game.QuestionState);
        }

        public void ExitState()
        {
            game.Context.GetPopupWidget().Hide();
        }

        public void Update(float delta)
        {
        }

        public void UpdatePhysics(float delta)
        {
        }
    }
}
