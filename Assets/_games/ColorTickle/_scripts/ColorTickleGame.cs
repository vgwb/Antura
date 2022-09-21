using UnityEngine;

namespace Antura.Minigames.ColorTickle
{
    public class ColorTickleGame : MiniGameController
    {
        #region PUBLIC MEMBERS

#pragma warning disable 649
        [SerializeField]
        private GameObject m_LetterPrefab;
        [SerializeField]
        private Canvas m_ColorsCanvas;
        [SerializeField]
        private ColorTickle_AnturaController m_AnturaController;
        [SerializeField]
        private int m_Rounds = 3;

        [SerializeField]
        private GameObject m_oWinParticle;
        [SerializeField]
        private Music m_oBackgroundMusic;
#pragma warning restore 649

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

        #region Score

        public override int MaxScore => 5; // means 3 stars

        public int starsAwarded => Mathf.CeilToInt(CurrentScore / 2f);

        // Difficulty-controlled parameters
        private int m_MaxLives
        {
            get
            {
                if (Difficulty < 0.2f) return 5;
                if (Difficulty < 0.7f) return 4;
                return 3;
            }
        }

        public int MaxLives => m_MaxLives;

        #endregion


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
