namespace Antura.Minigames.MissingLetter
{
    public class MissingLetterResultState : FSM.IState
    {

        public MissingLetterResultState(MissingLetterGame _game)
        {
            this.m_oGame = _game;
        }

        public void EnterState()
        {

            m_oGame.m_oRoundManager.Terminate();
            m_fTimer = 1;
            m_bGoToEndGame = true;

        }

        public void ExitState()
        {
        }

        public void Update(float delta)
        {
            if (m_bGoToEndGame)
            {
                m_fTimer -= delta;
            }
            if (m_fTimer < 0)
            {
                m_oGame.EndGame(m_oGame.m_iCurrentStars, m_oGame.CurrentScore);
            }

        }

        public void UpdatePhysics(float delta)
        {
        }

        MissingLetterGame m_oGame;
        bool m_bGoToEndGame;
        float m_fTimer = 4;
    }
}
