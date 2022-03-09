namespace Antura.Minigames.MissingLetter
{
    public class MissingLetterQuestionState : FSM.IState
    {

        public MissingLetterQuestionState(MissingLetterGame _game)
        {
            this.M_oGgame = _game;
        }

        public void EnterState()
        {
        }

        public void ExitState()
        {
            M_oGgame.Context.GetPopupWidget().Hide();
        }

        void OnQuestionCompleted()
        {
            if (M_oGgame.TutorialEnabled)
            {
                M_oGgame.SetCurrentState(M_oGgame.TutorialState);
            }
            else
            {
                M_oGgame.SetCurrentState(M_oGgame.PlayState);
            }
        }

        public void Update(float delta)
        {
            if (M_oGgame.TutorialEnabled)
            {
                M_oGgame.SetCurrentState(M_oGgame.TutorialState);
            }
            else
            {
                M_oGgame.SetCurrentState(M_oGgame.PlayState);
            }
        }

        public void UpdatePhysics(float delta)
        {
        }

        MissingLetterGame M_oGgame;
    }
}
