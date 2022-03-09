using Antura.Core;
using Antura.LivingLetters;
using UnityEngine;

namespace Antura.Minigames.FastCrowd
{
    public class FastCrowdQuestionState : FSM.IState
    {
        private FastCrowdGame game;
        private float idleTime;
        private const int idleTimeDuration = 60;

        public FastCrowdQuestionState(FastCrowdGame game)
        {
            this.game = game;
        }

        public void EnterState()
        {
            if (game.showTutorial)
            {
                game.QuestionManager.crowd.MaxConcurrentLetters = 2;
            }
            else
            {
                game.QuestionManager.crowd.MaxConcurrentLetters = UnityEngine.Mathf.RoundToInt(4 + game.Difficulty * 4);
            }

            // Forced number of letters
            if (FastCrowdConfiguration.Instance.Variation == FastCrowdVariation.CategoryForm)
            {
                game.QuestionManager.crowd.MaxConcurrentLetters = 4;
            }

            game.CurrentChallenge.Clear();
            game.NoiseData.Clear();

            var provider = FastCrowdConfiguration.Instance.Questions;
            var question = provider.GetNextQuestion();
            game.CurrentQuestion = question;

            if (question == null)
            {
                game.SetCurrentState(game.EndState);
                return;
            }

            // Add correct data
            foreach (var l in question.GetCorrectAnswers())
                game.CurrentChallenge.Add(l);

            // Add wrong data
            if (question.GetWrongAnswers() != null)
            {
                foreach (var l in question.GetWrongAnswers())
                {
                    game.NoiseData.Add(l);
                }
            }

            if (game.CurrentChallenge.Count > 0)
            {
                // Show question
                if (!game.ShowChallengePopupWidget(false, OnPopupCloseRequested))
                {
                    if (game.showTutorial)
                    {
                        game.SetCurrentState(game.TutorialState);
                    }
                    else
                    {
                        game.SetCurrentState(game.PlayState);
                    }
                }
            }
            else
            {
                // no more questions
                game.SetCurrentState(game.EndState);
            }
            idleTime = Time.time + idleTimeDuration;
        }

        void OnPopupCloseRequested()
        {
            if (game.GetCurrentState() == this)
            {
                if (game.showTutorial)
                {
                    game.SetCurrentState(game.TutorialState);
                }
                else
                {
                    game.SetCurrentState(game.PlayState);
                }
            }
        }

        public void ExitState()
        {
            game.Context.GetPopupWidget().Hide();
        }

        public void Update(float delta)
        {
            if (AppManager.I.AppSettings.KioskMode)
            {
                if (idleTime > 0 && Time.time > idleTime)
                {
                    idleTime = -1;
                    AppManager.I.NavigationManager.ExitToMainMenu();
                }
            }
        }

        public void UpdatePhysics(float delta)
        {
        }
    }
}
