using Antura.Minigames;

namespace Antura.Minigames.Tobogan
{
    public class ToboganQuestionState : FSM.IState
    {
        ToboganGame game;

        bool nextState;

        public ToboganQuestionState(ToboganGame game)
        {
            this.game = game;
        }

        public void EnterState()
        {
            game.questionsManager.Initialize();
            nextState = false;

            game.Context.GetAudioManager().PlayMusic(Music.Theme6);
            game.PlayIntro(() => nextState = true);
        }

        public void ExitState() { }

        public void Update(float delta)
        {
            if (nextState)
            {
                if (game.showTutorial)
                {
                    game.SetCurrentState(game.TutorialState);
                }
                else
                {
                    game.SetCurrentState(game.PlayState);
                }
            }
        }

        public void UpdatePhysics(float delta) { }
    }
}
