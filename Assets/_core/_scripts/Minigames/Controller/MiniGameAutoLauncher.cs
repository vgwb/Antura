using System.Collections;
using Antura.Core;
using UnityEngine;

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

        public MinigameLaunchConfiguration Config = new MinigameLaunchConfiguration(0f, 1, false, true);

#if UNITY_EDITOR
        IEnumerator Start()
        {
            while (!AppManager.I.Loaded)
                yield return null;

            if (!AppManager.I.NavigationManager.IsInFirstLoadedScene)
            {
                yield break;
            }

            AppManager.I.Player.SetCurrentJourneyPosition(Stage, LearningBlock, PlaySession);
            Config.DirectGame = true;
            AppManager.I.GameLauncher.LaunchGame(MiniGameCode, Config);
        }
#endif
    }
}
