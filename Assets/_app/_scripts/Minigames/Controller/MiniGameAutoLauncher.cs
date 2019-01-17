using Antura.Core;
using UnityEngine;

#if UNITY_EDITOR

namespace Antura.Minigames
{
    /// <summary>
    /// Utility component to launch a minigame with a pre-set configuration. 
    /// Enabled only in editor mode and only when the player starts from the current scene (minigame scene) 
    /// </summary>
    public class MiniGameAutoLauncher : MonoBehaviour
    {
        public MiniGameCode MiniGameCode;
        public int Stage = 1;
        public int LearningBlock = 1;
        public int PlaySession = 1;

        public float Difficulty = 0;
        public bool TutorialEnabled = false;
        public int NumberOfRounds = 1;

        void Start()
        {
            if (!AppManager.I.NavigationManager.IsInFirstLoadedScene) {
                return;
            }
            AppManager.I.Player.SetCurrentJourneyPosition(Stage, LearningBlock, PlaySession);
            var config = new MinigameLaunchConfiguration(Difficulty, NumberOfRounds, TutorialEnabled);
            AppManager.I.GameLauncher.LaunchGame(MiniGameCode, config, true);


        }
    }
}

#endif