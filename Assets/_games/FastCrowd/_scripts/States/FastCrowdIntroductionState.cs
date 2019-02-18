
namespace Antura.Minigames.FastCrowd
{
    public class FastCrowdIntroductionState : FSM.IState
    {
        FastCrowdGame game;

        bool nextState;
        bool playIntro;

        public FastCrowdIntroductionState(FastCrowdGame game)
        {
            this.game = game;
        }

        public void EnterState()
        {
            nextState = false;
            playIntro = false;

            game.Context.GetAudioManager().PlayDialogue(FastCrowdConfiguration.Instance.TitleLocalizationId, () => { playIntro = true; });
            game.Context.GetAudioManager().PlayMusic(Music.Theme10);
        }

        public void ExitState()
        {
        }

        public void Update(float delta)
        {
            if (nextState) {
                nextState = false;
                game.SetCurrentState(game.QuestionState);
            }

            if (playIntro) {
                playIntro = false;

                game.Context.GetAudioManager().PlayDialogue(FastCrowdConfiguration.Instance.IntroLocalizationId, () => { nextState = true; });
            }
        }

        public void UpdatePhysics(float delta) { }

    }
}