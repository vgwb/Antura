using Antura.Minigames;
using UnityEngine;

namespace Antura.Minigames.Egg
{
    public class EggResultState : FSM.IState
    {
        EggGame game;

        float nextStateTimer = 0f;
        bool toNextState = false;

        public EggResultState(EggGame game)
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
                if (!game.CurrentQuestion.IsSequence())
                {
                    game.Context.GetCheckmarkWidget().Show(true, new Vector2(0, 250));
                }
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

                    if (game.currentStage >= EggGame.numberOfStage)
                    {
                        game.eggController.Reset();
                        game.runLettersBox.RemoveAllRunLetters();
                        game.eggButtonBox.RemoveButtons();
                        game.Context.GetAudioManager().PlayMusic(Music.Relax);
                        game.EndGame(game.CurrentStars, game.CurrentScore);
                    }
                    else
                    {
                        game.SetCurrentState(game.QuestionState);
                    }
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
