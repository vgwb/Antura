using Antura.Core;
using UnityEngine;

namespace Antura.Profile
{
    /// <summary>
    /// Handles cleanup of player profiles.
    /// </summary>
    public class PlayerProfileCleaner : MonoBehaviour
    {
        public void ResetAllPlayerProfiles()
        {
            AppManager.I.PlayerProfileManager.ResetEverything();
        }

        public void TotalResetPlayerPref()
        {
            PlayerPrefs.DeleteAll();
            UnityEngine.SceneManagement.SceneManager.LoadScene(
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().name,
                UnityEngine.SceneManagement.LoadSceneMode.Single);
        }
    }
}
