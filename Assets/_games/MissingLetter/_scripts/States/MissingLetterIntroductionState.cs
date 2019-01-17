using Antura.Audio;

namespace Antura.Minigames.MissingLetter
{
    public class MissingLetterIntroductionState : FSM.IState
    {
        bool dialogueEnded;
        float startIntroDelay = 0;
        public MissingLetterIntroductionState(MissingLetterGame _game)
        {
            this.m_oGame = _game;
        }

        public void EnterState()
        {
            AudioManager.I.PlayDialogue(MissingLetterConfiguration.Instance.TitleLocalizationId, OnTitleEnded);
        }

        public void ExitState()
        {
        }

        void OnTitleEnded()
        {
            startIntroDelay = 0.25f;
        }

        public void Update(float _delta)
        {
            if (startIntroDelay > 0)
            {
                startIntroDelay -= _delta;

                if (startIntroDelay <= 0)
                {
                    m_oGame.Context.GetAudioManager().PlayDialogue(Database.LocalizationDataId.MissingLetter_Intro, () => dialogueEnded = true);
                }
            }

            m_fTimer -= _delta;

            if (dialogueEnded && m_fTimer < 0)
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
        float m_fTimer = 2;
    }
}