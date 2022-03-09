using Antura.Core;
using Kore.Coroutines;
using System.Collections;

namespace Antura.Assessment
{
    /// <summary>
    /// Result state. notify the LogManager of game ended and play final animation.
    /// Also teleport to main map.
    /// </summary>
    public class AssessmentResultState : FSM.IState
    {
        private AssessmentGame assessmentGame;
        private AssessmentAudioManager dialogueManager;

        public AssessmentResultState(AssessmentGame assessmentGame, AssessmentAudioManager dialogueManager)
        {
            this.assessmentGame = assessmentGame;
            this.dialogueManager = dialogueManager;
        }

        public void EnterState()
        {
            AssessmentConfiguration.Instance.Context.GetLogManager().OnGameEnded(3);
            LogManager.I.LogPlaySessionScore(AppManager.I.JourneyHelper.GetCurrentPlaySessionData().Id, 3);

            var audioManager = assessmentGame.Context.GetAudioManager();

            audioManager.PlayMusic(Music.Relax);
            audioManager.PlaySound(Sfx.TickAndWin);

            Koroutine.Run(QuitAfterYieldable(dialogueManager.PlayAssessmentCompleteSound()));
        }

        IEnumerator QuitAfterYieldable(IYieldable yieldable)
        {
            yield return yieldable;
            ExitState();
        }

        IEnumerator QuitAfterSomeTime(float seconds)
        {
            yield return Wait.For(seconds);
            ExitState();
        }

        bool exited = false;
        public void ExitState()
        {
            if (exited == false)
            {
                AppManager.I.NavigationManager.GoToNextScene();// AppScene.Rewards
                exited = true;
            }
        }

        public void Update(float delta)
        {

        }

        public void UpdatePhysics(float delta)
        {

        }
    }
}
