namespace Antura.Minigames.HideAndSeek
{
    public class QuestionGameState : FSM.IState
    {
        HideAndSeekGame game;

        public QuestionGameState(HideAndSeekGame game)
        {
            this.game = game;
        }

        public void EnterState()
        {
            game.Context.GetAudioManager().PlayMusic(Music.Relax);
            // Show questions description
            var popupWidget = game.Context.GetPopupWidget();
            popupWidget.Show();
            popupWidget.SetButtonCallback(OnQuestionCompleted);
            popupWidget.SetMessage("");
        }

        public void ExitState()
        {
            game.Context.GetPopupWidget().Hide();
            game.Context.GetAudioManager().StopMusic();
        }

        void OnQuestionCompleted()
        {
            if (game.TutorialEnabled)
            {
                game.SetCurrentState(game.TutorialState);
            }
            else
            {
                game.SetCurrentState(game.PlayState);
            }
        }

        public void Update(float delta) { }

        public void UpdatePhysics(float delta) { }
    }
}
