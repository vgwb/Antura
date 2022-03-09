using Antura.Database;
using UnityEngine.SceneManagement;

namespace Antura.Core
{
    public static class SceneHelper
    {
        // TODO refactor: scene names should match AppScene so that this can be removed
        public static string GetSceneName(AppScene scene, MiniGameData minigameData = null)
        {
            switch (scene)
            {
                case AppScene.Bootstrap:
                    return "app_Bootstrap";
                case AppScene.Home:
                    return "app_Home";
                case AppScene.AnturaSpace:
                    return "app_AnturaSpace";
                case AppScene.Book:
                    return "app_Book";
                case AppScene.Map:
                    return "app_Map";
                case AppScene.Mood:
                    return "app_Mood";
                case AppScene.GameSelector:
                    return "app_GamesSelector";
                case AppScene.Intro:
                    return "app_Intro";
                case AppScene.MiniGame:
                    return minigameData.Scene;
                case AppScene.PlayerCreation:
                    return "app_PlayerCreation";
                case AppScene.PlaySessionResult:
                    return "app_PlaySessionResult";
                case AppScene.Rewards:
                    return "app_Rewards";
                case AppScene.ReservedArea:
                    return "app_ReservedArea";
                case AppScene.Ending:
                    return "app_Ending";
                case AppScene.DailyReward:
                    return "app_DailyReward";
                case AppScene.Kiosk:
                    return "app_Kiosk";
                default:
                    return "";
            }
        }

        public static AppScene GetCurrentAppScene()
        {
            var currentScene = SceneManager.GetActiveScene().name;
            switch (currentScene)
            {
                case "app_Bootstrap":
                    return AppScene.Bootstrap;
                case "app_Home":
                    return AppScene.Home;
                case "app_AnturaSpace":
                    return AppScene.AnturaSpace;
                case "app_Book":
                    return AppScene.Book;
                case "app_Map":
                    return AppScene.Map;
                case "app_Mood":
                    return AppScene.Mood;
                case "app_GamesSelector":
                    return AppScene.GameSelector;
                case "app_Intro":
                    return AppScene.Intro;
                case "app_PlayerCreation":
                    return AppScene.PlayerCreation;
                case "app_PlaySessionResult":
                    return AppScene.PlaySessionResult;
                case "app_Rewards":
                    return AppScene.Rewards;
                case "app_ReservedArea":
                    return AppScene.ReservedArea;
                case "app_Ending":
                    return AppScene.Ending;
                case "app_DailyReward":
                    return AppScene.DailyReward;
                case "app_Kiosk":
                    return AppScene.Kiosk;
                default:
                    return AppScene.MiniGame;
            }
        }
    }
}
