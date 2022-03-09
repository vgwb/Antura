using Antura.LivingLetters;
using Antura.UI;
using Antura.Minigames;

namespace Antura.Minigames.MixedLetters
{
    public class ResultGameState : FSM.IState
    {
        private MixedLettersGame game;

        private const float TWIRL_ANIMATION_BACK_SHOWN_DELAY = 1f;
        private const float END_RESULT_DELAY = 1f;

        private float twirlAnimationDelayTimer;
        private bool wasBackShownDuringTwirlAnimation;
        private float endResultTimer;
        private bool isGameOver;

        public ResultGameState(MixedLettersGame game)
        {
            this.game = game;
        }

        public void EnterState()
        {
            SeparateLettersSpawnerController.instance.SetLettersNonInteractive();

            game.DisableRepeatPromptButton();

            if (game.roundNumber != 0)
            {
                MinigamesUI.Timer.Pause();
                LogRound(game.WasLastRoundWon);
            }

            if (!game.WasLastRoundWon)
            {
                MixedLettersConfiguration.Instance.Context.GetAudioManager().PlaySound(Sfx.Lose);
                SeparateLettersSpawnerController.instance.ShowLoseAnimation(OnResultAnimationEnded);
            }

            else
            {
                MixedLettersConfiguration.Instance.Context.GetAudioManager().PlaySound(Sfx.Win);
                SeparateLettersSpawnerController.instance.ShowWinAnimation(OnVictimLLIsShowingBack, OnResultAnimationEnded);

            }

            twirlAnimationDelayTimer = TWIRL_ANIMATION_BACK_SHOWN_DELAY;
            wasBackShownDuringTwirlAnimation = false;
            endResultTimer = END_RESULT_DELAY;
            isGameOver = false;

            // Increase the round number here so the victim LL loads the prompt of the next round correctly during
            // the twirl animation:
            game.roundNumber++;
        }

        private void OnVictimLLIsShowingBack()
        {
            if (!game.IsGameOver)
            {
                game.GenerateNewWord();
                VictimLLController.instance.HideVictoryRays();
            }

            wasBackShownDuringTwirlAnimation = true;
        }

        public void ExitState()
        {
            game.ResetScene();
        }

        public void OnResultAnimationEnded()
        {
            if (!game.IsGameOver)
            {
                game.SetCurrentState(game.IntroductionState);
            }

            else
            {
                isGameOver = true;

                if (game.WasLastRoundWon)
                {
                    endResultTimer = 0f;
                }
            }
        }

        public void Update(float delta)
        {
            if (isGameOver)
            {
                endResultTimer -= delta;

                if (endResultTimer < 0)
                {
                    game.EndGame(game.CurrentStars, game.CurrentScore);
                }
            }

            else if (game.WasLastRoundWon)
            {
                if (wasBackShownDuringTwirlAnimation)
                {
                    twirlAnimationDelayTimer -= delta;

                    if (twirlAnimationDelayTimer <= 0)
                    {
                        OnResultAnimationEnded();
                    }
                }
            }
        }

        private void LogRound(bool won)
        {
            if (game.IsSpelling)
            {
                game.Context.GetLogManager().OnAnswered(game.Question, won);
            }

            else
            {
                foreach (ILivingLetterData letter in game.PromptLettersInOrder)
                {
                    game.Context.GetLogManager().OnAnswered(letter, won);
                }
            }
        }

        public void UpdatePhysics(float delta)
        {
        }
    }
}
