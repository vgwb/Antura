using Antura.UI;

namespace Antura.Minigames.MixedLetters
{
    public class PlayGameState : FSM.IState
    {
        MixedLettersGame game;

        public PlayGameState(MixedLettersGame game)
        {
            this.game = game;
        }

        public void EnterState()
        {
            game.OnRoundStarted();
            game.EnableRepeatPromptButton();
            MinigamesUI.Timer.Play();
        }

        public void ExitState()
        {
        }

        public void Update(float delta)
        {
            if (MinigamesUI.Timer.Elapsed >= MinigamesUI.Timer.Duration || game.WasLastRoundWon)
            {
                game.SetCurrentState(game.ResultState);
            }
        }

        public void UpdatePhysics(float delta)
        {
        }
    }
}
