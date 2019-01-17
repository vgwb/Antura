using Antura.Core;
using System;
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

        protected override void Start()
        {
            Debug.Log("BootstrapScene: Start()");
            // GlobalUI.ShowPauseMenu(false);

            if (AppManager.I.AppSettingsManager.IsAppJustUpdatedFromOldVersion()) {
                Debug.Log("BootstrapScene: Updating from Old version");
                if (AppManager.I.AppSettings.SavedPlayers != null) {
                    AppManager.I.PlayerProfileManager.DeleteAllPlayers();
                    PanelAppUpdate.Init();
                } else {
                    GoToHomeScene();
                }
            } else {
                GoToHomeScene();
            }
        }

        private void GoToHomeScene()
        {
            if (AppManager.I.AppSettings.KioskMode) {
                AppManager.I.NavigationManager.GoToKiosk(true);
            } else {
                AppManager.I.NavigationManager.GoToHome();
            }
        }

        public void CloseAppUpdatePanel()
        {
            GoToHomeScene();
        }

    }
}