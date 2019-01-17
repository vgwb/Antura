using UnityEngine;

namespace Antura.Minigames.ColorTickle
{
    public class ColorTickleGame : MiniGameController
    {
        #region PUBLIC MEMBERS

        [SerializeField]
        private GameObject m_LetterPrefab;
        [SerializeField]
        private Canvas m_ColorsCanvas;
        [SerializeField]
        private ColorTickle_AnturaController m_AnturaController;
        [SerializeField]
        private int m_Rounds = 3;
        [SerializeField]
        private int m_MaxLives = 3;
        [SerializeField]
        private GameObject m_oWinParticle;
        [SerializeField]
        private Music m_oBackgroundMusic;

        // GAME STATES
        public IntroductionGameState IntroductionState { get; private set; }
        public TutorialGameState TutorialState { get; private set; }
        public PlayGameState PlayState { get; private set; }
        public ResultGameState ResultState { get; private set; }

        #endregion

        #region PRIVATE MEMBERS
        GameObject[] m_MyLetters;
        GameObject m_TutorialLetter;
        IOverlayWidget m_GameUI;
        TutorialUIManager m_TutorialUIManager;

        #endregion

        #region GETTER/SETTER

        public GameObject letterPrefab
        {
            get { return m_LetterPrefab; }
            set { m_LetterPrefab = value; }
        }

        public GameObject[] myLetters
        {
            get { return m_MyLetters; }
            set { m_MyLetters = value; }
        }

        public GameObject tutorialLetter
        {
            get { return m_TutorialLetter; }
            set { m_TutorialLetter = value; }
        }

        public Canvas colorsCanvas
        {
            get { return m_ColorsCanvas; }
        }

        public ColorTickle_AnturaController anturaController
        {
            get { return m_AnturaController; }
        }

        public int lives
        {
            get { return m_MaxLives; }
        }

        public int score { get; set; }

        public int starsAwarded
        {
            get { return Mathf.CeilToInt(score / 2f); }
        }

        public int rounds
        {
            get { return m_Rounds; }
            set { m_Rounds = value; }
        }

        public IOverlayWidget gameUI
        {
            get { return m_GameUI; }
            set { m_GameUI = value; }
        }

        public GameObject winParticle
        {
            get { return m_oWinParticle; }
            set { m_oWinParticle = value; }
        }

        public Music backgroundMusic
        {
            get { return m_oBackgroundMusic; }
            set { m_oBackgroundMusic = value; }
        }

        public TutorialUIManager tutorialUIManager
        {
            get { return m_TutorialUIManager; }
            set { m_TutorialUIManager = value; }
        }

        #endregion

        protected override void OnInitialize(IGameContext context)
        {
            IntroductionState = new IntroductionGameState(this, GetConfiguration().TutorialEnabled);
            TutorialState = new TutorialGameState(this);
            PlayState = new PlayGameState(this);
            ResultState = new ResultGameState(this);

            //Difficulty is decided like this: 0 <= easy <= 0.333 < medium <= 0.666 < hard <=1
            var difficulty = GetConfiguration().Difficulty;

            //Adjust parameters accordingly:
            //- max lives: 2 on hard, 5 on very easy
            m_MaxLives = 2 + Mathf.RoundToInt(3 * (1 - difficulty));

        }

        protected override FSM.IState GetInitialState()
        {
            return IntroductionState;
        }

        protected override IGameConfiguration GetConfiguration()
        {
            return ColorTickleConfiguration.Instance;
        }

    }
}
