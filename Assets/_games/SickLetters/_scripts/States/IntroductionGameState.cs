using Antura.Audio;
using UnityEngine;

namespace Antura.Minigames.SickLetters
{
    public class IntroductionGameState : FSM.IState
    {
        SickLettersGame game;

        float timer = 2.5f;
        public IntroductionGameState(SickLettersGame game)
        {
            this.game = game;
        }

        public void EnterState()
        {
            Debug.Log("enter intro");
            game.processDifiiculties(SickLettersConfiguration.Instance.Difficulty);
            AudioManager.I.PlayDialogue(Database.LocalizationDataId.SickLetters_lettername_Title);
            game.antura.sleep();
            game.disableInput = true;
        }

        public void ExitState()
        {
            Debug.Log("exit intro");
            AudioManager.I.PlayDialogue(Database.LocalizationDataId.SickLetters_lettername_Intro);
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