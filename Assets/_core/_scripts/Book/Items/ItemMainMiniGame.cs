using Antura.Core;
using Antura.Minigames;
using Antura.Database;
using Antura.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.Book
{
    /// <summary>
    /// Displays a main MiniGame item in the MiniGames panel of the Player Book.
    /// </summary>
    public class ItemMainMiniGame : MonoBehaviour
    {
        public MainMiniGame mainGameInfo;

        public Image BackgroundImage;
        public TextRender Title;
        public Image Icon;
        public Image LockIcon;

        public Sprite ImageOff;
        public Sprite ImageOn;

        private bool isSelected;
        private bool isLocked;
        private GamesPanel panelManager;
        private GameObject btnGO;

        public void Init(GamesPanel _manager, MainMiniGame _MainMiniGame)
        {
            mainGameInfo = _MainMiniGame;
            panelManager = _manager;

            var firstMiniGame = mainGameInfo.variations[0].data;
            var icon = AppManager.I.AssetManager.GetMainIcon(firstMiniGame);

            //// @note: we get the minigame saved score, which should be the maximum score achieved
            //// @note: I'm leaving the average-based method commented if we want to return to that logic
            //var score = info.score;
            //var score = GenericHelper.GetAverage(TeacherAI.I.ScoreHelper.GetLatestScoresForMiniGame(info.data.Code, -1));

            //if (score < 0.1f) {
            //    // disabled
            //    //GetComponent<Button>().interactable = false;
            //    //GetComponent<Image>().color = Color.grey;
            //}

            Icon.sprite = icon;

            isLocked = true;
            foreach (var gameVariation in mainGameInfo.variations)
            {
                if (gameVariation.unlocked)
                {
                    isLocked = false;
                }
                //Debug.Log("gameVariation() main game " + mainGameInfo.MainId + " / " + gameVariation.data.Code + "(" + isLocked + ")");
            }
            LockIcon.enabled = isLocked;
        }

        public void OnClicked()
        {
            //Debug.Log("OnClicked() main game " + mainGameInfo.MainId);
            if (!isLocked)
            {
                DetailMiniGame(mainGameInfo);
            }
        }

        public void DetailMiniGame(MainMiniGame mainMiniGameInfo)
        {
            panelManager.DetailMainMiniGame(mainMiniGameInfo);
        }

        public void Select(MainMiniGame gameInfo = null)
        {
            if (gameInfo != null && mainGameInfo != null)
            {
                isSelected = (gameInfo.MainId == mainGameInfo.MainId);
            }
            else
            {
                isSelected = false;
            }
            hightlight(isSelected);
        }

        void hightlight(bool _status)
        {
            if (_status)
            {
                //BackgroundImage.color = new Color(0.9490197f, 0.7215686f, 0.1882353f, 1f);
                BackgroundImage.sprite = ImageOn;
            }
            else
            {
                //BackgroundImage.color = Color.white;
                BackgroundImage.sprite = ImageOff;
            }
        }

    }
}
