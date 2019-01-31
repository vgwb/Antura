using Antura.Minigames;

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

            if (game.stagePositiveResult) {
                game.Context.GetAudioManager().PlaySound(Sfx.Win);
                game.Context.GetCheckmarkWidget().Show(true);
                toNextState = true;
            } else {
                game.Context.GetAudioManager().PlaySound(Sfx.Lose);
                game.Context.GetCheckmarkWidget().Show(false);
                toNextState = true;
            }
        }

        public void ExitState() { }

        public void Update(float delta)
        {
            if (toNextState) {
                nextStateTimer -= delta;

                if (nextStateTimer <= 0f) {
                    toNextState = false;

                    if (game.currentStage >= EggGame.numberOfStage) {
                        game.eggController.Reset();
                        game.runLettersBox.RemoveAllRunLetters();
                        game.eggButtonBox.RemoveButtons();
                        game.Context.GetAudioManager().PlayMusic(Music.Relax);
                        game.EndGame(game.CurrentStars, game.correctStages);
                    } else {
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