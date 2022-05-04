using Antura.Core;
using Antura.Minigames;
using Antura.UI;
using System.Collections;
using UnityEngine;

namespace Antura.Scenes
{
    /// <summary>
    /// Controls the _Start scene, providing an entry point for all users prior to having selected a player profile.
    /// </summary>
    public class BootstrapScene : SceneBase
    {

        [Header("References")]
        public PanelAppUpdate PanelAppUpdate;
        public MiniGameAutoLauncher AutoLauncher;

        protected override void Start()
        {
            Debug.Log("BootstrapScene: Start()");
            GlobalUI.ShowPauseMenu(false);

            StartCoroutine(StartCO());

            //if (AppManager.I.AppSettingsManager.IsAppJustUpdatedFromOldVersion()) {
            //    Debug.Log("BootstrapScene: Updating from Old version");
            //    if (AppManager.I.AppSettings.SavedPlayers != null) {
            //        AppManager.I.PlayerProfileManager.DeleteAllPlayers();
            //        PanelAppUpdate.Init();
            //    } else {
            //        GoToHomeScene();
            //    }
            //} else {
            //    GoToHomeScene();
            //}
        }

        private IEnumerator StartCO()
        {
            while (!AppManager.I.Loaded)
                yield return null;
            if (AutoLauncher.enabled)
                yield break;

            GoToHomeScene();
        }

        private void GoToHomeScene()
        {
            if (AppManager.I.AppSettings.KioskMode)
            {
                AppManager.I.NavigationManager.GoToKiosk(true);
            }
            else
            {
                AppManager.I.NavigationManager.GoToHome();
            }
        }

        public void CloseAppUpdatePanel()
        {
            GoToHomeScene();
        }

    }
}
