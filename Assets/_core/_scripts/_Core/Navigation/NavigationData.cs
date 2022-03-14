using Antura.Database;
using Antura.Profile;
using System.Collections.Generic;

namespace Antura.Core
{
    public struct NavigationData
    {
        public PlayerProfile CurrentPlayer;
        public AppScene CurrentScene;
        public bool IsJourneyPlaySession;
        public Stack<AppScene> PrevSceneStack;
        public MiniGameData DirectMiniGameData;

        /// <summary>
        /// List of minigames selected for the current play session
        /// </summary>
        public List<MiniGameData> CurrentPlaySessionMiniGames;

        /// <summary>
        /// Current minigame index in
        /// </summary>
        public int CurrentMiniGameIndexInPlaySession { get; private set; }

        public void Setup()
        {
            CurrentScene = SceneHelper.GetCurrentAppScene();
            PrevSceneStack = new Stack<AppScene>();
        }

        public void Init(PlayerProfile _playerProfile)
        {
            CurrentPlayer = _playerProfile;
        }

        public void SetFirstMinigame()
        {
            CurrentMiniGameIndexInPlaySession = 0;
        }

        public bool SetNextMinigame()
        {
            var NextIndex = CurrentMiniGameIndexInPlaySession + 1;
            if (NextIndex < CurrentPlaySessionMiniGames.Count)
            {
                CurrentMiniGameIndexInPlaySession = NextIndex;
                return true;
            }
            return false;
        }

        public MiniGameData CurrentMiniGameData
        {
            get
            {
                if (CurrentPlaySessionMiniGames == null)
                { return null; }
                if (CurrentPlaySessionMiniGames.Count == 0)
                { return null; }
                return CurrentPlaySessionMiniGames[CurrentMiniGameIndexInPlaySession];
            }
        }
    }
}
