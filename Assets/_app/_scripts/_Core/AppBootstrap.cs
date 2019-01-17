using Antura.Audio;
using Antura.UI;
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

        void Awake()
        {
            if (FindObjectOfType(typeof(AudioManager)) == null) {
                Instantiate(AudioManager);
            }

            if (FindObjectOfType(typeof(EventSystem)) == null) {
                Instantiate(EventsManager);
            }

            // init the mighty GlobalUI
            GlobalUI.Init();
        }
    }
}