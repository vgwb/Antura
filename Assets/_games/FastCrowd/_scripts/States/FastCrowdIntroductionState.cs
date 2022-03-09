
namespace Antura.Minigames.FastCrowd
{
    public class FastCrowdIntroductionState : FSM.IState
    {
        FastCrowdGame game;

        public FastCrowdIntroductionState(FastCrowdGame game)
        {
            this.game = game;
        }

        public void EnterState()
        {
            game.Context.GetAudioManager().PlayMusic(Music.Theme10);
            game.SetCurrentState(game.QuestionState);
        }

        public void ExitState()
        {
        }

        public void Update(float delta)
        {
        }

        public void UpdatePhysics(float delta) { }

    }
}
