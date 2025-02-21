using System;
using Antura.Core;
using Antura.Debugging;
using Antura.Profile;
using Demigiant.DemiTools;
using DG.DeInspektor.Attributes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Antura.Scenes
{
    public class PlayerCreationScene : SceneBase
    {
        #region EVENTS

        public static ActionEvent OnCreationComplete = new("PlayerCreationScene.OnCreationComplete");

        #endregion
        
        [DeEmptyAlert]
        [SerializeField] AudioListener audioListener;

        static bool isOverlayMode; // TRUE when this scene is loaded in overlay (by ReservedArea's Classroom)

        protected override void Awake()
        {
            AudioListener[] audioListeners = FindObjectsByType<AudioListener>(FindObjectsSortMode.None);
            if (audioListeners.Length > 1)
            {
                isOverlayMode = true;
                Destroy(audioListener);
            }
            
            // Skip base Awake if in overlay mode otherwise this Component will be destroyed
            if (!isOverlayMode) base.Awake();
        }

        protected override void Start()
        {
            base.Start();

            DebugManager.OnSkipCurrentScene += HandleSkipScene;
        }

        void OnDestroy()
        {
            DebugManager.OnSkipCurrentScene -= HandleSkipScene;
        }

        private void HandleSkipScene()
        {
            CreatePlayer(0, PlayerGender.M, Color.yellow, Color.red, Color.magenta, 4);
        }


        public static void CreatePlayer(int avatarID, PlayerGender gender, Color skinColor, Color hairColor, Color bgColor, int age)
        {
            Debug.Log(string.Format("Will create player of with avatarID {0}, skin color {1}, hair color {2}, bg color {3}, age {4}, gender {5},", avatarID, skinColor, hairColor, bgColor, age, gender));
            AppManager.I.PlayerProfileManager.CreatePlayerProfile(true, avatarID, gender, PlayerTint.None, skinColor, hairColor, bgColor, age,
                                AppManager.I.AppEdition.editionID,
                                AppManager.I.ContentEdition.ContentID,
                                AppManager.I.AppEdition.AppVersion);
            LogManager.I.LogInfo(InfoEvent.AppPlay, JsonUtility.ToJson(new DeviceInfo()));

            if (isOverlayMode)
            {
                // Just dispatch the completion event, ClassroomPanel will take care of the rest
                OnCreationComplete.Dispatch();
            }
            else if (AppManager.PROFILE_INVERSION)
            {
                // For now, we go back home, then we'll get to the content selection screen
                AppManager.I.NavigationManager.GoToHome(debugMode:true); // debug mode to force transition
            }
            else
            {
                AppManager.I.NavigationManager.GoToNextScene();
            }
        }

    }
}
