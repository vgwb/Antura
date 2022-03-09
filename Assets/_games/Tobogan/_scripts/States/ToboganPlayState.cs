using Antura.LivingLetters;
using Antura.Minigames;

namespace Antura.Minigames.Tobogan
{
    public class ToboganPlayState : FSM.IState
    {
        CountdownTimer gameTime = new CountdownTimer(90.0f);
        ToboganGame game;

        float nextQuestionTimer;

        const float NEXT_QUESTION_TIME = 1f;
        const float REVEAL_TIME = 1f;

        bool requestNextQueston;

        public ToboganPlayState(ToboganGame game)
        {
            this.game = game;

            gameTime.onTimesUp += OnTimesUp;
        }

        public void EnterState()
        {
            game.InitializeOverlayWidget();

            game.questionsManager.StartNewQuestion();

            game.isTimesUp = false;
            game.ResetScore();

            // Reset game timer
            gameTime.Reset();
            gameTime.Start();

            game.Context.GetOverlayWidget().SetClockDuration(gameTime.Duration);
            game.Context.GetOverlayWidget().SetClockTime(gameTime.Time);

            game.questionsManager.Enabled = true;

            nextQuestionTimer = 0f;
            requestNextQueston = false;

            game.questionsManager.onAnswered += OnAnswered;
        }


        public void ExitState()
        {
            game.questionsManager.onAnswered -= OnAnswered;

            game.questionsManager.Enabled = false;

            gameTime.Stop();
            game.pipesAnswerController.HidePipes();
        }

        public void Update(float delta)
        {
            game.questionsManager.Update(delta);

            if (game.CurrentScoreRecord >= ToboganGame.MAX_ANSWERS_RECORD)
            {
                // Maximum tower height reached
                game.SetCurrentState(game.ResultState);
                return;
            }

            game.Context.GetOverlayWidget().SetClockTime(gameTime.Time);

            gameTime.Update(delta);

            if (requestNextQueston)
            {
                nextQuestionTimer -= delta;

                if (nextQuestionTimer <= 0f)
                {
                    game.questionsManager.StartNewQuestion();
                    requestNextQueston = false;
                }
            }
        }

        public void UpdatePhysics(float delta) { }

        void OnAnswered(IQuestionPack pack, bool result)
        {
            requestNextQueston = true;
            nextQuestionTimer = NEXT_QUESTION_TIME + REVEAL_TIME;

            game.questionsManager.OnQuestionEnd(REVEAL_TIME);

            game.OnResult(result);

            game.Context.GetLogManager().OnAnswered(pack.GetQuestion(), result);
        }

        void OnTimesUp()
        {
            // Time's up!
            game.isTimesUp = true;
            game.Context.GetOverlayWidget().OnClockCompleted();
            game.SetCurrentState(game.ResultState);
        }
    }
}
