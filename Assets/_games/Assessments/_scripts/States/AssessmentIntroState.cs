namespace Antura.Assessment
{
    public class AssessmentIntroState : FSM.IState
    {
        private AssessmentGame assessmentGame;
        private AssessmentGameState gameState;
        private AssessmentAudioManager audioManager;

        public AssessmentIntroState(AssessmentGame assessmentGame,
                                        AssessmentGameState gameState,
                                        AssessmentAudioManager audioManager)
        {
            this.assessmentGame = assessmentGame;
            this.gameState = gameState;
            this.audioManager = audioManager;
        }

        public void InitAllStates()
        {

        }

        public void EnterState()
        {
            if (audioManager != null)
                audioManager.PlayAssessmentMusic();
        }

        public void ExitState()
        {
        }

        private void SetNextState()
        {
            assessmentGame.SetCurrentState(gameState);
        }

        float timer = 0.6f; // Gives Time to show the first question appearing

        public void Update(float delta)
        {
            timer -= delta;
            if (timer <= 0)
            {
                SetNextState();
            }
        }

        public void UpdatePhysics(float delta)
        {
        }
    }
}
