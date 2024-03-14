using Antura.Minigames;
using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    public class DiscoverCountryResultState : FSM.IState
    {
        DiscoverCountryGame game;

        float nextStateTimer = 0f;
        bool toNextState = false;

        public DiscoverCountryResultState(DiscoverCountryGame game)
        {
            this.game = game;
        }

        public void EnterState()
        {
            nextStateTimer = 5f;
            toNextState = false;

            if (game.stagePositiveResult)
            {
                game.Context.GetAudioManager().PlaySound(Sfx.Win);
                toNextState = true;
            }
            else
            {
                game.Context.GetAudioManager().PlaySound(Sfx.Lose);
                game.Context.GetCheckmarkWidget().Show(false);
                toNextState = true;
            }
        }

        public void ExitState() { }

        public void Update(float delta)
        {
            if (toNextState)
            {
                nextStateTimer -= delta;

                if (nextStateTimer <= 0f)
                {
                    toNextState = false;

                    game.SetCurrentState(game.QuestionState);
                }
            }
        }

        public void UpdatePhysics(float delta) { }

        void OnPopupCloseRequested()
        {
            game.Context.GetPopupWidget().Hide();
            game.SetCurrentState(game.QuestionState);
        }
    }
}
