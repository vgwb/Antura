using Antura.Audio;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Antura.Core
{
    /// <summary>
    /// Takes care of generating managers before the AppManger is awoken.
    /// Tied to the AppManager.
    /// </summary>
    public class AppBootstrap : MonoBehaviour
    {
        public GameObject AudioManager;
        public GameObject EventsManager;

        public void InitManagers()
        {
            if (FindFirstObjectByType(typeof(AudioManager)) == null)
            {
                Instantiate(AudioManager);
            }

            if (FindFirstObjectByType(typeof(EventSystem)) == null)
            {
                Instantiate(EventsManager);
            }
        }
    }
}
