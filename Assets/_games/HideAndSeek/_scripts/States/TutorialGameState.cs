namespace Antura.Minigames.HideAndSeek
{
    public class TutorialGameState : FSM.IState
    {
        HideAndSeekGame game;

        public TutorialGameState(HideAndSeekGame game)
        {
            this.game = game;
        }

        public void EnterState()
        {
            game.Context.GetAudioManager().PlayMusic(Music.Lullaby);
            game.TutorialManager.enabled = true;
        }

        public void ExitState()
        {
            game.TutorialManager.enabled = false;
            game.Context.GetAudioManager().StopMusic();
        }

        public void Update(float delta) { }

        public void UpdatePhysics(float delta) { }
    }
}
