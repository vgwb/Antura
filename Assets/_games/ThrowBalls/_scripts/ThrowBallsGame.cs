using UnityEngine;

namespace Antura.Minigames.ThrowBalls
{
    public class ThrowBallsGame : MiniGameController
    {
        public GameState GameState { get; private set; }

        public static ThrowBallsGame instance;

        #region Score

        public override int MaxScore => 6;

        #endregion

        public bool TutorialEnabled => GetConfiguration().TutorialEnabled;

        public GameObject ball;
        public BallController ballController;
        public GameObject letterWithPropsPrefab;
        public GameObject poofPrefab;
        public GameObject cratePoofPrefab;
        public GameObject environment;

        protected override void OnInitialize(IGameContext context)
        {
            instance = this;
            GameState = new GameState(this);
            
        }

        protected override IGameConfiguration GetConfiguration()
        {
            return ThrowBallsConfiguration.Instance;
        }

        protected override FSM.IState GetInitialState()
        {
            return GameState;
        }
    }
}
