using System;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.Minigames.MissingLetter
{
    [System.Serializable]
    public struct LLOffset
    {
        public float fINOffset;
        public float fOUTOffset;
        public float fHeightOffset;
    }

    public class MissingLetterGame : MiniGameController
    {
        public static bool VISUAL_DEBUG = false;
        public static Color helpColor = Color.magenta;

        #region API
        public void ResetScore()
        {
            CurrentScore = 0;
        }

        public void OnResult(bool _result)
        {
            var question = m_oRoundManager.CurrentQuestionPack;
            Context.GetLogManager().OnAnswered(question.GetQuestion(), _result);

            Context.GetAudioManager().PlaySound(Sfx.Blip);

            if (_result)
            {
                Context.GetAudioManager().PlaySound(Sfx.StampOK);
            }
            else
            {
                Context.GetAudioManager().PlaySound(Sfx.KO);
            }

            Context.GetCheckmarkWidget().Show(_result);

            if (_result)
            {
                ++CurrentScore;
            }
        }

        public void SetInIdle(bool _idle)
        {
            m_oFeedBackDisableLetters.enabled = !_idle;
            m_bInIdle = _idle;
        }

        public bool IsInIdle()
        {
            return m_bInIdle;
        }
        #endregion

        #region PROTECTED_FUNCTION
        protected override void OnInitialize(IGameContext context)
        {
            PrepareParameters();

            m_oRoundManager = new RoundManager(this);
            m_oRoundManager.Initialize();

            IntroductionState = new MissingLetterIntroductionState(this);
            QuestionState = new MissingLetterQuestionState(this);
            PlayState = new MissingLetterPlayState(this);
            ResultState = new MissingLetterResultState(this);
            TutorialState = new MissingLetterTutorialState(this);

            Context.GetOverlayWidget().Initialize(false, false, false);

            m_bInIdle = true;

            DisableRepeatPromptButton();
        }

        protected override FSM.IState GetInitialState()
        {
            return IntroductionState;
        }

        protected override IGameConfiguration GetConfiguration()
        {
            return MissingLetterConfiguration.Instance;
        }
        #endregion

        #region PRIVATE_FUNCTION
        private void PrepareParameters()
        {
            m_iRoundsLimit = MissingLetterConfiguration.Instance.N_ROUNDS;

            if (MissingLetterConfiguration.Instance.Variation == MissingLetterVariation.Phrase)
            {
                m_fGameTime = 120;// Mathf.Lerp(120, 80, _diff);
            }
            else
            {
                m_fGameTime = 90; // m_fGameTime = Mathf.Lerp(90, 60, _diff);
            }

            // Stars threshold is computed as if we had 10 rounds
            int nBaseRounds = 10;
            STARS_1_THRESHOLD = (int)(nBaseRounds * 0.3);
            STARS_2_THRESHOLD = (int)(nBaseRounds * 0.6);
            STARS_3_THRESHOLD = (int)(nBaseRounds * 0.9);
        }
        #endregion

        #region VARS
        public GameObject m_oLetterPrefab;
        public GameObject m_oAntura;

        public GameObject m_oEmoticonsController;
        public MissingLetterEmoticonsMaterials m_oEmoticonsMaterials;

        public GameObject m_oParticleSystem;
        public GameObject m_oSuggestionLight;
        public Collider m_oFeedBackDisableLetters;

        public Transform m_oQuestionCamera;
        public Transform m_oAnswerCamera;

        public int m_iMaxSentenceSize;

        public float m_fAnturaAnimDuration;

        public float m_fDistanceBetweenLetters;

        public LLOffset m_sQuestionOffset;
        public LLOffset m_sAnswerOffset;


        [HideInInspector]
        public bool m_AnturaTriggered = false;

        [HideInInspector]
        public RoundManager m_oRoundManager;

        [HideInInspector]
        public bool m_bIsTimesUp;

        [HideInInspector]
        public int m_iRoundsLimit;

        [HideInInspector]
        public float m_fGameTime;

        #region Score

        public override int MaxScore => STARS_3_THRESHOLD;

        // Difficulty parameters
        public int m_iNumberOfPossibleAnswers => (int)Mathf.Lerp(2, 5, Difficulty);

        // Stars
        [HideInInspector]
        public int STARS_1_THRESHOLD = 2;
        [HideInInspector]
        public int STARS_2_THRESHOLD = 5;
        [HideInInspector]
        public int STARS_3_THRESHOLD = 9;

        public int m_iCurrentStars
        {
            get
            {
                if (CurrentScore < STARS_1_THRESHOLD)
                    return 0;
                if (CurrentScore < STARS_2_THRESHOLD)
                    return 1;
                if (CurrentScore < STARS_3_THRESHOLD)
                    return 2;
                return 3;
            }
        }

        #endregion

        private bool m_bInIdle { get; set; }

        public MissingLetterIntroductionState IntroductionState { get; private set; }
        public MissingLetterQuestionState QuestionState { get; private set; }
        public MissingLetterPlayState PlayState { get; private set; }
        public MissingLetterResultState ResultState { get; private set; }
        public MissingLetterTutorialState TutorialState { get; private set; }

        public bool TutorialEnabled => GetConfiguration().TutorialEnabled;

        #endregion

        #region Repeat Button

        public Button repeatPromptButton;

        public void EnableRepeatPromptButton()
        {
            repeatPromptButton.gameObject.SetActive(true);
        }

        public void DisableRepeatPromptButton()
        {
            repeatPromptButton.gameObject.SetActive(false);
        }

        public void SayQuestion()
        {
            var question = m_oRoundManager.CurrentQuestionPack.GetQuestion();
            MissingLetterConfiguration.Instance.Context.GetAudioManager().PlayVocabularyData(question, soundType: MissingLetterConfiguration.Instance.GetVocabularySoundType());
        }

        #endregion
    }
}
