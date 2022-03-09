using Antura.Minigames;

namespace Antura.Assessment
{
    public class AssessmentGameState : FSM.IState
    {
        private Assessment assessment;
        private AssessmentResultState resultState;
        private AssessmentGame assessmentGame;
        private Updater updater;
        private IDragManager dragManager;

        public AssessmentGameState(IDragManager dragManager, Assessment assessment,
                                    AssessmentResultState resultState, AssessmentGame game)
        {
            this.assessment = assessment;
            this.resultState = resultState;
            this.assessmentGame = game;
            this.dragManager = dragManager;
        }

        public void EnterState()
        {
            updater = Updater.Instance;
            updater.AddTimedUpdate(dragManager);
            assessment.StartGameSession(SetNextState);
        }

        public void SetNextState()
        {
            assessmentGame.SetCurrentState(resultState);
        }

        public void ExitState()
        {
            updater.Clear();
            updater = null;
        }

        public void Update(float delta)
        {
            updater.UpdateDelta(delta);
        }

        public void UpdatePhysics(float delta)
        {

        }
    }
}
