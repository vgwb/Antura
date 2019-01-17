using System;
using UnityEngine;

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
        #region API
        public void ResetScore()
        {
            m_iCurrentScore = 0;
        }

        public void OnResult(bool _result)
        {
            var question = m_oRoundManager.CurrentQuestion;
            Context.GetLogManager().OnAnswered(question.GetQuestion(), _result);

            Context.GetAudioManager().PlaySound(Sfx.Blip);

            if (_result) {
                Context.GetAudioManager().PlaySound(Sfx.StampOK);
            } else {
                Context.GetAudioManager().PlaySound(Sfx.KO);
            }

            Context.GetCheckmarkWidget().Show(_result);

            if (_result) {
                ++m_iCurrentScore;
            }

            Context.GetOverlayWidget().SetStarsScore(m_iCurrentScore);
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
            CalculateDifficulty();

            m_oRoundManager = new RoundManager(this);
            m_oRoundManager.Initialize();

            IntroductionState = new MissingLetterIntroductionState(this);
            QuestionState = new MissingLetterQuestionState(this);
            PlayState = new MissingLetterPlayState(this);
            ResultState = new MissingLetterResultState(this);
            TutorialState = new MissingLetterTutorialState(this);

            Context.GetOverlayWidget().Initialize(false, false, false);

            m_bInIdle = true;

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
        private void CalculateDifficulty()
        {
            float _diff = MissingLetterConfiguration.Instance.Difficulty;

            //At least, they are all sets to the minimun
            //m_iRoundsLimit = Mathf.RoundToInt(Mathf.Lerp(6, 10, _diff));
            m_iRoundsLimit = MissingLetterConfiguration.Instance.N_ROUNDS;
            //m_iNumberOfPossibleAnswers = Mathf.RoundToInt(Mathf.Lerp(2, 6, _diff));
            m_iNumberOfPossibleAnswers = 4;

            if (MissingLetterConfiguration.Instance.Variation == MissingLetterVariation.Phrase) {
                m_fGameTime = Mathf.Lerp(120, 80, _diff);
            } else {
                m_fGameTime = Mathf.Lerp(90, 60, _diff);
            }
            m_iAnturaTriggersNumber = Mathf.RoundToInt(Mathf.Lerp(1, 4, _diff));

            //Calculating time entry point for Antura based off how many times it should enter
            m_afAnturaEnterTriggers = new float[m_iAnturaTriggersNumber];
            for (int i = 0; i < m_iAnturaTriggersNumber; ++i) {
                m_afAnturaEnterTriggers[i] = ((m_fGameTime - 10.0f) / m_iAnturaTriggersNumber) * (m_iAnturaTriggersNumber - i);
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
        public int STARS_1_THRESHOLD = 2;
        [HideInInspector]
        public int STARS_2_THRESHOLD = 5;
        [HideInInspector]
        public int STARS_3_THRESHOLD = 9;


        [HideInInspector]
        public int m_iNumberOfPossibleAnswers = 4;

        [HideInInspector]
        public float[] m_afAnturaEnterTriggers;

        [HideInInspector]
        public int m_iAnturaTriggersNumber;

        [HideInInspector]
        public int m_iAnturaTriggersIndex = 0;

        [HideInInspector]
        public RoundManager m_oRoundManager;

        [HideInInspector]
        public bool m_bIsTimesUp;

        [HideInInspector]
        public int m_iCurrentScore { get; private set; }

        [HideInInspector]
        public int m_iRoundsLimit;

        [HideInInspector]
        public float m_fGameTime;


        public int m_iCurrentStars
        {
            get {
                if (m_iCurrentScore < STARS_1_THRESHOLD)
                    return 0;
                if (m_iCurrentScore < STARS_2_THRESHOLD)
                    return 1;
                if (m_iCurrentScore < STARS_3_THRESHOLD)
                    return 2;
                return 3;
            }
        }

        private bool m_bInIdle { get; set; }

        public MissingLetterIntroductionState IntroductionState { get; private set; }
        public MissingLetterQuestionState QuestionState { get; private set; }
        public MissingLetterPlayState PlayState { get; private set; }
        public MissingLetterResultState ResultState { get; private set; }
        public MissingLetterTutorialState TutorialState { get; private set; }

        public bool TutorialEnabled
        {
            get { return GetConfiguration().TutorialEnabled; }
        }

        #endregion

    }
}