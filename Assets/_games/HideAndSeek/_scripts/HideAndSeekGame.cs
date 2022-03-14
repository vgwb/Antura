using Antura.LivingLetters;
using Antura.Minigames;
using UnityEngine;

namespace Antura.Minigames.HideAndSeek
{
    public class HideAndSeekGame : MiniGameController
    {
        public IntroductionGameState IntroductionState { get; private set; }
        public QuestionGameState QuestionState { get; private set; }
        public TutorialGameState TutorialState { get; private set; }
        public PlayGameState PlayState { get; private set; }
        public ResultGameState ResultState { get; private set; }

        public HideAndSeekGameManager GameManager;

        public HideAndSeekTutorialManager TutorialManager;

        [HideInInspector]
        public bool isTimesUp;

        public bool inGame = false;

        #region Score

        public static int STARS_1_THRESHOLD = 2;
        public static int STARS_2_THRESHOLD = 5;
        public static int STARS_3_THRESHOLD = 9;

        public int CurrentStars
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

        public override int MaxScore => STARS_3_THRESHOLD;

        #endregion

        public bool TutorialEnabled { get { return GetConfiguration().TutorialEnabled; } }

        protected override void OnInitialize(IGameContext context)
        {
            IntroductionState = new IntroductionGameState(this);
            QuestionState = new QuestionGameState(this);
            TutorialState = new TutorialGameState(this);
            PlayState = new PlayGameState(this);
            ResultState = new ResultGameState(this);
        }

        public void ResetScore()
        {
            CurrentScore = 0;
        }

        protected override FSM.IState GetInitialState()
        {
            return IntroductionState;
        }

        protected override IGameConfiguration GetConfiguration()
        {
            return HideAndSeekConfiguration.Instance;
        }

        public void OnResult(ILivingLetterData data, bool result)
        {
            Context.GetLogManager().OnAnswered(data, result);

            if (result)
            {
                CurrentScore++;
                if (CurrentStars >= 3) // Early end
                {
                    SetCurrentState(ResultState);
                }
            }

        }
    }
}
