using DG.DeInspektor.Attributes;
using UnityEngine;

namespace Antura.Discover.DEV110
{
    /// <summary>
    /// Convenience launcher to start the fight without running the whole quest.
    /// Either enable <see cref="AutoLaunch"/> or press the play-mode button in the inspector.
    /// Mirrors the framework's DebugActivityLauncher.
    /// </summary>
    public class DebugFightLauncher : MonoBehaviour
    {
        public UndertaleFight Fight;

        [Tooltip("Begin the fight automatically on Start().")]
        public bool AutoLaunch = false;

        private void Start()
        {
            if (AutoLaunch)
                LaunchFight();
        }

        [DeMethodButton(mode = DeButtonMode.PlayModeOnly)]
        public void LaunchFight()
        {
            if (Fight == null)
            {
                Debug.LogWarning("DebugFightLauncher: no UndertaleFight assigned.");
                return;
            }
            Fight.Begin();
        }
    }
}
