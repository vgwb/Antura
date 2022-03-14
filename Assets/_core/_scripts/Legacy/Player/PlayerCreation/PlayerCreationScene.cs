#if FALSE
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
            CreatePlayer(4, PlayerGender.M, 0, PlayerTint.Blue);
        }

        public static void CreatePlayer(int age, PlayerGender gender, int avatarID, PlayerTint color)
        {
            Debug.Log(string.Format("Will create player of age {0}, gender {1}, avatarID {2}, color {3}", age, gender, avatarID, color));
            AppManager.I.PlayerProfileManager.CreatePlayerProfile(age, gender, avatarID, color);
            LogManager.I.LogInfo(InfoEvent.AppPlay, JsonUtility.ToJson(new DeviceInfo()));
            AppManager.I.NavigationManager.GoToNextScene();
        }
    }
}
#endif
