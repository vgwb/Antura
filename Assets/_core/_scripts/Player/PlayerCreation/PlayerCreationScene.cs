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
            CreatePlayer(0, Color.yellow, Color.red, Color.magenta);
        }

        public static void CreatePlayer(int avatarID, Color skinColor, Color hairColor, Color bgColor)
        {
            Debug.Log(string.Format("Will create player of with avatarID {0}, skin color {1}, hair color {2}, bg color {3}", avatarID, skinColor, hairColor, bgColor));
            AppManager.I.PlayerProfileManager.CreatePlayerProfile(avatarID, skinColor, hairColor, bgColor);
            LogManager.I.LogInfo(InfoEvent.AppPlay, JsonUtility.ToJson(new DeviceInfo()));
            AppManager.I.NavigationManager.GoToNextScene();
        }

        [Obsolete("Use new overload which accepts skinColor, hairColor, bgColor")]
        public static void CreatePlayer(int age, PlayerGender gender, int avatarID, PlayerTint color)
        {
            Debug.Log(string.Format("Will create player of age {0}, gender {1}, avatarID {2}, color {3}", age, gender, avatarID, color));
            AppManager.I.PlayerProfileManager.CreatePlayerProfile(age, gender, avatarID, color);
            LogManager.I.LogInfo(InfoEvent.AppPlay, JsonUtility.ToJson(new DeviceInfo()));
            AppManager.I.NavigationManager.GoToNextScene();
        }
    }
}