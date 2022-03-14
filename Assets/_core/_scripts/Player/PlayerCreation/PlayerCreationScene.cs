using System;
using Antura.Core;
using Antura.Debugging;
using Antura.Profile;
using UnityEngine;

namespace Antura.Scenes
{
    public class PlayerCreationScene : SceneBase
    {
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
            AppManager.I.NavigationManager.GoToNextScene();
        }

    }
}
