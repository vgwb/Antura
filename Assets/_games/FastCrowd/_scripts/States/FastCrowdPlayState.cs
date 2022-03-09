using Antura.LivingLetters;
using Antura.Minigames;

namespace Antura.Minigames.FastCrowd
{
    public class FastCrowdPlayState : FSM.IState
    {
        CountdownTimer gameTime;
        FastCrowdGame game;

        float anturaTimer;
        bool isAnturaRunning = false;

        bool initializeOveralyWidget;

        public FastCrowdPlayState(FastCrowdGame game)
        {
            this.game = game;

            initializeOveralyWidget = true;
        }

        public void EnterState()
        {
            game.QuestionManager.OnCompleted += OnQuestionCompleted;
            game.QuestionManager.OnDropped += OnAnswerDropped;

            /*
            List<ILivingLetterData> wrongAnswers = new List<ILivingLetterData>();

            for (int i = 0;
                i < FastCrowdConfiguration.Instance.MaxNumbOfWrongLettersNoise &&
                i < game.QuestionNumber &&
                i < game.NoiseData.Count; i++)
            {
                wrongAnswers.Add(game.NoiseData[i]);
            }
            */


            if (game.CurrentChallenge != null)
            {
                game.QuestionManager.StartQuestion(game.CurrentChallenge, game.NoiseData);

                if (gameTime != null)
                    gameTime.onTimesUp -= OnTimesUp;
                if (FastCrowdConfiguration.Instance.Variation == FastCrowdVariation.Alphabet)
                {
                    gameTime = new CountdownTimer(game.CurrentChallenge.Count * 4f);
                }
                else if (FastCrowdConfiguration.Instance.IsOrderingVariation)
                {
                    gameTime = new CountdownTimer(game.CurrentChallenge.Count * 5f);
                }
                else
                {
                    gameTime = new CountdownTimer(UnityEngine.Mathf.Lerp(90.0f, 60.0f, game.Difficulty));
                }
                gameTime.onTimesUp += OnTimesUp;
            }
            else
                game.QuestionManager.Clean();

            // Reset game timer
            gameTime.Start();

            if (initializeOveralyWidget)
            {
                initializeOveralyWidget = false;
                game.InitializeOverlayWidget();
            }

            game.Context.GetOverlayWidget().SetClockDuration(gameTime.Duration);
            game.Context.GetOverlayWidget().SetClockTime(gameTime.Time);

            StopAntura();

            game.QuestionManager.wordComposer.gameObject.SetActive(FastCrowdConfiguration.Instance.NeedsWordComposer);

        }

        public void ExitState()
        {
            StopAntura();

            gameTime.Stop();
            game.QuestionManager.OnCompleted -= OnQuestionCompleted;
            game.QuestionManager.OnDropped -= OnAnswerDropped;
            game.QuestionManager.Clean();
            game.QuestionManager.wordComposer.gameObject.SetActive(false);
        }

        void OnQuestionCompleted()
        {
            if (FastCrowdConfiguration.Instance.NeedsFullQuestionCompleted)
            {
                // In spelling and letter, increment score only when the full question is completed
                for (int i = 0; i < game.CurrentChallenge.Count; ++i)
                    game.IncrementScore();
            }

            if (FastCrowdConfiguration.Instance.Variation == FastCrowdVariation.BuildWord)
            {
                var question = game.CurrentQuestion;

                if (question != null && question.GetQuestion() != null)
                    game.Context.GetLogManager().OnAnswered(question.GetQuestion(), true);
            }

            game.SetCurrentState(game.ResultState);
        }

        void OnAnswerDropped(ILivingLetterData data, bool result)
        {
            game.Context.GetCheckmarkWidget().Show(result);

            if (!FastCrowdConfiguration.Instance.NeedsFullQuestionCompleted)
            {
                if (result)
                {
                    // In spelling and letter, increment score only when the full question is completed
                    game.IncrementScore();
                }

                game.Context.GetLogManager().OnAnswered(data, result);
            }

            game.Context.GetAudioManager().PlaySound(result ? Sfx.OK : Sfx.KO);

            if (game.CurrentStars == 3)
                game.SetCurrentState(game.EndState);
        }

        void StopAntura()
        {
            isAnturaRunning = false;
            game.antura.SetAnturaTime(false);
            // Schedule next exit
            anturaTimer = UnityEngine.Mathf.Lerp(20, 10, game.Difficulty);

            game.Context.GetAudioManager().PlayMusic(Music.Theme10);
        }

        void StartAntura()
        {
            isAnturaRunning = true;
            game.antura.SetAnturaTime(true);
            // Schedule next duration
            anturaTimer = UnityEngine.Mathf.Lerp(5, 15, game.Difficulty);

            game.Context.GetAudioManager().PlayMusic(Music.MainTheme);
        }

        public void Update(float delta)
        {
            anturaTimer -= delta;

            if (anturaTimer <= 0.0f)
            {
                if (isAnturaRunning)
                    StopAntura();
                else
                    StartAntura();
            }

            gameTime.Update(delta);
            game.Context.GetOverlayWidget().SetClockTime(gameTime.Time);
        }

        public void UpdatePhysics(float delta)
        {
        }

        void OnTimesUp()
        {
            // Time's up!
            game.Context.GetOverlayWidget().OnClockCompleted();
            game.SetCurrentState(game.EndState);
        }
    }
}
