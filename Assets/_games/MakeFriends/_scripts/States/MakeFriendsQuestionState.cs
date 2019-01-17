namespace Antura.Minigames.MakeFriends
{
    public class MakeFriendsQuestionState : FSM.IState
    {
        MakeFriendsGame game;

        public MakeFriendsQuestionState(MakeFriendsGame game)
        {
            this.game = game;
        }

        public void EnterState()
        {
            //game.Context.GetPopupWidget().Show(OnQuestionCompleted, TextID.ASSESSMENT_RESULT_GOOD, true, null);
            // Show questions description
            //var popupWidget = game.Context.GetPopupWidget();
            //popupWidget.Show();
            //popupWidget.SetButtonCallback(OnQuestionCompleted);
            //popupWidget.SetMessage("", true);
            game.SetCurrentState(game.PlayState);
        }

        public void ExitState()
        {
            game.Context.GetPopupWidget().Hide();
        }

        //        void OnQuestionCompleted()
        //        {
        //            game.SetCurrentState(game.PlayState);
        //        }

        public void Update(float delta)
        {

        }

        public void UpdatePhysics(float delta)
        {
        }
    }
}
