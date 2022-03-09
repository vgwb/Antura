
namespace Antura.Minigames.MissingLetter
{
    public class MissingLetterIntroductionState : FSM.IState
    {
        float m_fTimer = 2;

        public MissingLetterIntroductionState(MissingLetterGame _game)
        {
            this.m_oGame = _game;
        }

        public void EnterState()
        {
        }

        public void ExitState()
        {
        }

        public void Update(float _delta)
        {
            m_fTimer -= _delta;
            if (m_fTimer < 0)
            {
                if (m_oGame.TutorialEnabled)
                {
                    m_oGame.SetCurrentState(m_oGame.TutorialState);
                }
                else
                {
                    m_oGame.SetCurrentState(m_oGame.QuestionState);
                }
            }
        }

        public void UpdatePhysics(float _delta)
        {
        }

        MissingLetterGame m_oGame;
    }
}
