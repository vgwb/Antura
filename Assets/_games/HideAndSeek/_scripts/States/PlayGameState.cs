namespace Antura.Minigames.HideAndSeek
{
    public class PlayGameState : FSM.IState
    {
        HideAndSeekGame game;

        public CountdownTimer gameTime = new CountdownTimer(90.0f);

        int STARS_1_THRESHOLD => HideAndSeekGame.STARS_1_THRESHOLD;
        int STARS_2_THRESHOLD => HideAndSeekGame.STARS_2_THRESHOLD;
        int STARS_3_THRESHOLD => HideAndSeekGame.STARS_3_THRESHOLD;

        public PlayGameState(HideAndSeekGame game)
        {
            this.game = game;

            gameTime.onTimesUp += OnTimesUp;
        }

        public void EnterState()
        {
            game.GameManager.enabled = true;

            game.Context.GetOverlayWidget().Initialize(true, true, true);
            game.Context.GetOverlayWidget().SetStarsThresholds(STARS_1_THRESHOLD, STARS_2_THRESHOLD, STARS_3_THRESHOLD);

            gameTime.Reset();
            game.ResetScore();

            game.Context.GetAudioManager().PlayMusic(Music.MainTheme);

            game.Context.GetOverlayWidget().SetClockDuration(gameTime.Duration);
            game.Context.GetOverlayWidget().SetClockTime(gameTime.Time);

            game.Context.GetOverlayWidget().SetMaxLives(3);

            game.inGame = true;
            game.GameManager.SetTime();

        }

        public void ExitState()
        {
            gameTime.Stop();

            game.inGame = false;
            game.GameManager.enabled = false;
        }

        public void Update(float delta)
        {
            game.Context.GetOverlayWidget().SetClockTime(gameTime.Time);

            gameTime.Update(delta);
        }

        public void UpdatePhysics(float delta) { }

        void OnTimesUp()
        {
            game.Context.GetOverlayWidget().OnClockCompleted();
            game.isTimesUp = true;
            game.SetCurrentState(game.ResultState);
        }
    }
}
