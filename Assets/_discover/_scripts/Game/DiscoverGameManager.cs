using Antura.Utilities;

namespace Antura.Minigames.DiscoverCountry
{
    public enum GameplayState
    {
        None, // no state defined
        Setup, // setup here we set things useful for gameplay
        Intro, // intro state, here efx like camera on regions are played
        Play3D, // here we play!
        Play2D, // counting the score after a tile is placed
        Dialogue,
        End, // end the game
    }

    public class DiscoverGameManager : SingletonMonoBehaviour<DiscoverGameManager>
    {
        public GameplayState State { get; private set; }

        public bool isPaused;

        void Start()
        {
            State = GameplayState.None;

        }
    }
}
