using UnityEngine;

namespace Antura.Minigames.DancingDots
{
    public class IntroductionGameState : FSM.IState
    {
        DancingDotsGame game;

        public IntroductionGameState(DancingDotsGame game)
        {
            this.game = game;
        }

        public void EnterState()
        {
            this.game.dancingDotsLL.contentGO.SetActive(false);
            game.dancingDotsLL.letterObjectView.DoTwirl(null);
            game.disableInput = true;

            game.PlayIntro(EndIntro);
        }
        public void EndIntro()
        {
            game.SetCurrentState(game.QuestionState);
        }

        public void ExitState()
        {
        }

        public void Update(float delta)
        {
        }

        public void UpdatePhysics(float delta)
        {
        }
    }
}
