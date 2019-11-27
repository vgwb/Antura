using Antura.Audio;
using Antura.Keeper;
using UnityEngine;

namespace Antura.Minigames.SickLetters
{
    public class IntroductionGameState : FSM.IState
    {
        SickLettersGame game;

        public IntroductionGameState(SickLettersGame game)
        {
            this.game = game;
        }

        public void EnterState()
        {
            game.gameDuration = 70;
            game.targetScale = 18;

            game.ProcessDifficulty(game.Difficulty);
            game.antura.sleep();
            game.disableInput = true;

            game.PlayIntro(EndIntro);
        }

        private void EndIntro()
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