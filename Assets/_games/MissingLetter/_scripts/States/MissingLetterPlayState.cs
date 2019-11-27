using Antura.Audio;

namespace Antura.Minigames.MissingLetter
{
    public class MissingLetterPlayState : FSM.IState
    {
        public MissingLetterPlayState(MissingLetterGame _game)
        {
            this.m_oGame = _game;
            M_oGameTime = new CountdownTimer(_game.m_fGameTime);
            M_oGameTime.onTimesUp += OnTimesUp;
        }

        public void EnterState()
        {
            m_oGame.m_oRoundManager.onAnswered += OnRoundResult;
            m_oGame.m_bIsTimesUp = false;
            m_oGame.ResetScore();

            M_oGameTime.Reset();
            M_oGameTime.Start();

            m_oGame.Context.GetOverlayWidget().Initialize(true, true, false);

            m_oGame.Context.GetOverlayWidget().SetStarsThresholds(m_oGame.STARS_1_THRESHOLD, m_oGame.STARS_2_THRESHOLD, m_oGame.STARS_3_THRESHOLD);
            m_oGame.Context.GetOverlayWidget().SetClockDuration(M_oGameTime.Duration);
            m_oGame.Context.GetOverlayWidget().SetClockTime(M_oGameTime.Time);

            m_oGame.m_oRoundManager.NewRound();
            m_oGame.EnableRepeatPromptButton();
        }

        public void ExitState()
        {
            m_oGame.m_oRoundManager.onAnswered -= OnRoundResult;

            AudioManager.I.StopMusic();

            M_oGameTime.Stop();
            m_oGame.DisableRepeatPromptButton();
        }

        public void Update(float _delta)
        {
            if (m_oGame.Difficulty > 0.66f && !m_oGame.m_AnturaTriggered && m_oGame.IsInIdle())
            {
                m_oGame.m_AnturaTriggered = true;
                m_oGame.m_oAntura.GetComponent<AnturaBehaviour>().EnterScene(m_oGame.m_fAnturaAnimDuration);
                m_oGame.StartCoroutine(Utils.LaunchDelay(m_oGame.m_fAnturaAnimDuration / 6, m_oGame.m_oRoundManager.ShuffleLetters, m_oGame.m_fAnturaAnimDuration / 2));
            }

            m_oGame.Context.GetOverlayWidget().SetClockTime(M_oGameTime.Time);

            M_oGameTime.Update(_delta);
        }

        public void UpdatePhysics(float delta)
        {
        }



        void OnTimesUp()
        {
            // Time's up!
            m_oGame.m_bIsTimesUp = true;
            m_oGame.Context.GetOverlayWidget().OnClockCompleted();
            m_oGame.SetCurrentState(m_oGame.ResultState);
        }


        void OnRoundResult(bool _result)
        {
            m_oGame.m_oRoundManager.NewRound();
        }


        #region VARS

        CountdownTimer M_oGameTime;
        MissingLetterGame m_oGame;

        #endregion
    }
}
