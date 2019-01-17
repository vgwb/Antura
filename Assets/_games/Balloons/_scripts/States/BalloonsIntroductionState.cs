namespace Antura.Minigames.Balloons
{
    public class BalloonsIntroductionState : FSM.IState
    {
        BalloonsGame game;

        float timer = 1.5f;
        bool playTutorial;
        bool takenAction = false;

        public BalloonsIntroductionState(BalloonsGame game, bool PerformTutorial)
        {
            this.game = game;
            this.playTutorial = PerformTutorial;
        }

        public void EnterState()
        {
            game.PlayTitleVoiceOver();
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
            if (takenAction) {
                return;
            }

            timer -= delta;

            if (timer < 0) {
                takenAction = true;

                if (playTutorial) {
                    this.game.PlayTutorial();
                } else {
                    game.SetCurrentState(game.QuestionState);
                }
            }
        }

        public void UpdatePhysics(float delta)
        {
        }
    }
}