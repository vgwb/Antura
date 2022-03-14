using System.Collections;
using UnityEngine;

namespace Antura.Minigames.Balloons
{
    public class BalloonsIntroductionState : FSM.IState
    {
        BalloonsGame game;

        bool playTutorial;
        bool takenAction = false;

        public BalloonsIntroductionState(BalloonsGame game, bool PerformTutorial)
        {
            this.game = game;
            this.playTutorial = PerformTutorial;
        }

        public void EnterState()
        {
        }

        public void ExitState()
        {
        }

        public void OnFinishedTutorial()
        {
            game.SetCurrentState(game.QuestionState);
        }

        public void Update(float delta)
        {
            if (takenAction)
            {
                return;
            }
            takenAction = true;

            if (playTutorial)
            {
                game.PlayTutorial();
            }
            else
            {
                game.SetCurrentState(game.QuestionState);
            }
        }

        public void UpdatePhysics(float delta)
        {
        }
    }
}
