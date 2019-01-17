using UnityEngine;

namespace Antura.Minigames.DancingDots
{
    public class IntroductionGameState : FSM.IState
    {
        DancingDotsGame game;

        float timer = 1.5f;
        public IntroductionGameState(DancingDotsGame game)
        {
            this.game = game;
        }

        public void EnterState()
        {
            this.game.dancingDotsLL.contentGO.SetActive(false);
            game.Context.GetAudioManager().PlayDialogue(Database.LocalizationDataId.DancingDots_letterany_Title);
            game.dancingDotsLL.letterObjectView.DoTwirl(null);
            game.disableInput = true;
        }

        public void ExitState()
        {
        }

        public void Update(float delta)
        {
            timer -= delta;

            if (timer < 0) {
                game.SetCurrentState(game.QuestionState);
            }
        }

        public void UpdatePhysics(float delta)
        {
        }
    }
}