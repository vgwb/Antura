using Antura.Core;
using Antura.Database;
using Antura.Keeper;
using Antura.Minigames;
using Antura.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.Book
{
    /// <summary>
    /// Displays information on minigames that the player has unlocked.
    /// </summary>
    public class GamesPanel : MonoBehaviour
    {
        [Header("Prefabs")]
        public GameObject MainMiniGameItemPrefab;
        public GameObject VariationsContainer;
        public GameObject MiniGameItemPrefab;

        [Header("References")]
        public GameObject DetailPanel;

        public GameObject ElementsContainer;
        public TextRender GameTitle;
        public Image MiniGameLogoImage;
        public Image MiniGameBadgeImage;
        public Button LaunchGameButton;

        GameObject btnGO;
        BookArea currentArea = BookArea.None;
        MiniGameData currentMiniGame;

        void Start()
        {
        }

        void OnEnable()
        {
            OpenArea(BookArea.MiniGames);
        }

        void OpenArea(BookArea newArea)
        {
            currentArea = newArea;
            activatePanel(currentArea, true);
        }

        void activatePanel(BookArea panel, bool status)
        {
            switch (panel) {
                case BookArea.MiniGames:
                    //AudioManager.I.PlayDialog("Book_Games");
                    MiniGamesPanel();
                    break;
            }
        }

        void MiniGamesPanel()
        {
            emptyContainer(ElementsContainer);

            var mainMiniGamesList = MiniGamesUtilities.GetMainMiniGameList();
            foreach (var game in mainMiniGamesList) {
                btnGO = Instantiate(MainMiniGameItemPrefab);
                btnGO.transform.SetParent(ElementsContainer.transform, false);
                //btnGO.transform.SetAsFirstSibling();
                btnGO.GetComponent<ItemMainMiniGame>().Init(this, game);
            }
            DetailMainMiniGame(null);
            DetailMiniGame(null);
        }

        public void DetailMainMiniGame(MainMiniGame selectedMainMiniGame)
        {
            emptyContainer(VariationsContainer);

            if (selectedMainMiniGame == null) {
                return;
            }

            //Debug.Log("DetailMainMiniGame(): " + selectedMainMiniGame.MainId);
            foreach (var gameVariation in selectedMainMiniGame.variations) {
                btnGO = Instantiate(MiniGameItemPrefab);
                btnGO.transform.SetParent(VariationsContainer.transform, false);
                btnGO.GetComponent<ItemMiniGame>().Init(this, gameVariation);
            }

            ElementsContainer.BroadcastMessage("Select", selectedMainMiniGame, SendMessageOptions.DontRequireReceiver);
            DetailMiniGame(selectedMainMiniGame.variations[0]);
        }

        public void DetailMiniGame(MiniGameInfo selectedGameInfo)
        {
            if (selectedGameInfo == null) {
                currentMiniGame = null;
                GameTitle.text = "";
                MiniGameLogoImage.enabled = false;
                MiniGameBadgeImage.enabled = false;
                LaunchGameButton.gameObject.SetActive(false);
                DetailPanel.SetActive(false);
                return;
            }

            DetailPanel.SetActive(true);

            if (currentMiniGame?.Main != selectedGameInfo.data.Main) {
                // only first time we see a new main minigame
                KeeperManager.I.PlayDialogue(selectedGameInfo.data.GetTitleSoundFilename(), false, true, null, KeeperMode.LearningNoSubtitles);
            }
            currentMiniGame = selectedGameInfo.data;

            VariationsContainer.BroadcastMessage("Select", selectedGameInfo, SendMessageOptions.DontRequireReceiver);
            GameTitle.text = selectedGameInfo.data.GetFullTitle(true);

            //var Output = "";
            //Output += "Score: " + selectedGameInfo.score;
            //Output += "\nPlayed: ";
            //ScoreText.text = Output;

            // Launch button
            if (!AppManager.I.NavigationManager.PrevSceneIsReservedArea() &&
                (selectedGameInfo.unlocked || AppManager.I.Player.IsDemoUser)) {
                LaunchGameButton.gameObject.SetActive(true);
                LaunchGameButton.interactable = true;
            } else {
                LaunchGameButton.gameObject.SetActive(false);
                LaunchGameButton.interactable = false;
            }

            // Set icon
            var icoPath = currentMiniGame.GetIconResourcePath();
            var badgePath = currentMiniGame.GetBadgeIconResourcePath();
            MiniGameLogoImage.sprite = Resources.Load<Sprite>(icoPath);
            MiniGameLogoImage.enabled = true;
            if (badgePath != "") {
                MiniGameBadgeImage.enabled = true;
                MiniGameBadgeImage.sprite = Resources.Load<Sprite>(badgePath);
            } else {
                MiniGameBadgeImage.enabled = false;
            }

            Debug.Log($"MiniGame selected: {selectedGameInfo.data.Code}");
        }

        MinigameLaunchConfiguration minigameLaunchConfig = new MinigameLaunchConfiguration(0f, 1, true, insideJourney: false, directGame: true);
        public void OnLaunchMinigame()
        {
            AppManager.I.Player.SetCurrentJourneyPosition(AppManager.I.Player.MaxJourneyPosition, false, true);
            Debug.Log("Playing minigame " + currentMiniGame.Code + " at PS " + AppManager.I.Player.CurrentJourneyPosition);
            AppManager.I.GameLauncher.LaunchGame(currentMiniGame.Code, minigameLaunchConfig);
        }

        void emptyContainer(GameObject container)
        {
            foreach (Transform t in container.transform) {
                Destroy(t.gameObject);
            }
        }
    }
}