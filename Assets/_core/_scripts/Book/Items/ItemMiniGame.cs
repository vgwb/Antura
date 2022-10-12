using Antura.Database;
using Antura.Core;
using Antura.UI;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Antura.Book
{
    /// <summary>
    /// Displays a MiniGame variation item in the MiniGames panel of the Player Book.
    /// </summary>
    public class ItemMiniGame : MonoBehaviour, IPointerClickHandler
    {
        public MiniGameInfo miniGameInfo;

        public UIMinigameVariationBadge Badge;
        public Image LockIcon;
        public Image BackgroundImage;

        public Sprite ImageOff;
        public Sprite ImageOn;

        private bool isSelected;
        private bool isLocked;
        private GamesPanel myManager;

        public void Init(GamesPanel _manager, MiniGameInfo _MiniGameInfo)
        {
            miniGameInfo = _MiniGameInfo;
            myManager = _manager;

            if (miniGameInfo.unlocked || AppManager.I.Player.IsDemoUser)
            {
                isLocked = false;
            }
            else
            {
                isLocked = true;
            }
            LockIcon.enabled = isLocked;

            //var icoPath = miniGameInfo.data.GetIconResourcePath();
            //Debug.Log("resource icon for " + miniGameInfo.data.GetId() + ":" + icoPath);

            //// @note: we get the minigame saved score, which should be the maximum score achieved
            //// @note: I'm leaving the average-based method commented if we want to return to that logic
            var score = miniGameInfo.score;
            //var score = GenericHelper.GetAverage(TeacherAI.I.ScoreHelper.GetLatestScoresForMiniGame(miniGameInfo.data.Code, -1));

            if (score < 0.1f)
            {
                // disabled
                // GetComponent<Button>().interactable = false;
                //GetComponent<Image>().color = Color.grey;
            }

            //Icon.sprite = Resources.Load<Sprite>(icoPath);
            Badge.Assign(miniGameInfo.data);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!isLocked)
            {
                myManager.DetailMiniGame(miniGameInfo);
            }
        }

        public void Select(MiniGameInfo gameInfo = null)
        {
            if (gameInfo != null && miniGameInfo != null)
            {
                isSelected = (gameInfo.data.GetId() == miniGameInfo.data.GetId());
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
                Badge.GetComponent<Image>().sprite = ImageOn;
                // BackgroundImage.color = new Color(0.9490197f, 0.7215686f, 0.1882353f, 1f);
            }
            else
            {
                Badge.GetComponent<Image>().sprite = ImageOff;
                // BackgroundImage.color = new Color(0.8862746f, 0.8862746f, 0.8862746f, 1f);
            }
        }
    }
}
