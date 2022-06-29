using Antura.Audio;
using Antura.Keeper;
using Antura.LivingLetters;
using Antura.Tutorial;
using UnityEngine;

namespace Antura.Minigames.MissingLetter
{

    public class MissingLetterTutorialState : FSM.IState
    {

        public MissingLetterTutorialState(MissingLetterGame _game)
        {
            this.m_oGame = _game;
        }

        public void EnterState()
        {
            AudioManager.I.PlayMusic(Music.Theme6);
            m_oGame.m_oRoundManager.SetTutorial(true);
            m_oGame.m_oRoundManager.NewRound();
            m_oGame.m_oRoundManager.onAnswered += OnRoundResult;
        }


        public void ExitState()
        {
            TutorialUI.Clear(true);
            m_oGame.m_oRoundManager.SetTutorial(false);
            m_oGame.m_oRoundManager.onAnswered -= OnRoundResult;
        }

        void OnRoundResult(bool _result)
        {
            if (_result)
            {
                m_oGame.SetCurrentState(m_oGame.PlayState);
            }
            else
            {
                var _LL = m_oGame.m_oRoundManager.GetCorrectLLObject();
                _LL.GetComponent<LetterBehaviour>().PlayAnimation(LLAnimationStates.LL_dancing);
            }
        }

        public void Update(float delta)
        {
            m_fDelayTime -= delta;
            if (m_fDelayTime < 0 && !m_bSuggested && !m_oGame.m_oRoundManager.CurrentLetterBehaviour.IsSpeaking)
            {
                m_bSuggested = true;
                KeeperManager.I.PlayDialogue(MissingLetterConfiguration.Instance.IntroLocalizationId, _callback: () =>
                {
                    KeeperManager.I.PlayDialogue(MissingLetterConfiguration.Instance.TutorialLocalizationId);

                    m_oGame.m_oRoundManager.GetCorrectLLObject().GetComponent<LetterBehaviour>().PlayAnimation(LLAnimationStates.LL_dancing);
                    Vector3 pos = m_oGame.m_oRoundManager.GetCorrectLLObject().transform.position + Vector3.back * 0.8f + Vector3.up * 3;
                    TutorialUI.ClickRepeat(pos, 90, 1.5f);
                });
            }
        }

        public void UpdatePhysics(float delta)
        {

        }

        MissingLetterGame m_oGame;
        float m_fDelayTime = 3f;
        bool m_bSuggested = false;
    }
}
