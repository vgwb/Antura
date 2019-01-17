namespace Antura.Minigames.SickLetters
{
    public class QuestionGameState : FSM.IState
    {
        SickLettersGame game;

        public QuestionGameState(SickLettersGame game)
        {
            this.game = game;
        }

        public void EnterState()
        {
            game.SetCurrentState(game.PlayState);

            //game.Context.GetPopupWidget().Show(OnQuestionCompleted, TextID.ASSESSMENT_RESULT_GOOD, true, null);
        }

        public void ExitState()
        {
            game.Context.GetPopupWidget().Hide();
        }

        void OnQuestionCompleted()
        {
            game.SetCurrentState(game.PlayState);
        }

        public void Update(float delta)
        {

        }

        public void UpdatePhysics(float delta)
        {
        }
    }
}
