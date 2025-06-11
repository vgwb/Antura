using Antura.Utilities;

namespace Antura.Minigames.DiscoverCountry
{
    public enum GameplayState
    {
        None, // no state defined
        Setup, // setup here we set things useful for gameplay
        Intro, // intro state
        Play3D, // here we play in the world
        Play2D, // here we play a 2D game
        Dialogue, // all dialogues magic dialogs are here
        End, // end the game, show results
    }

    public class DiscoverGameManager : SingletonMonoBehaviour<DiscoverGameManager>
    {
        public GameplayState State { get; private set; }

        // when we pause the game we use this global var
        public bool isPaused;

        void Start()
        {
            State = GameplayState.None;
        }

        public void ChangeState(GameplayState newState)
        {
            State = newState;
        }

    }
}
