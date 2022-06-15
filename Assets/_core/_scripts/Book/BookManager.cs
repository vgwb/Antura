using Antura.Core;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Antura.Database;
using Antura.Minigames;
using UnityEngine;

namespace Antura.Book
{
    public class BookManager : MonoBehaviour
    {
        public static BookManager I;
        GameObject BookInstance;

        const string RESOURCES_BOOK = "Prefabs/Book/Book";

        void Awake()
        {
            I = this;
        }

        public void OpenBook(BookArea area, MiniGameData directMiniGameData = null)
        {
            // TODO maybe first check if Book is already isntatiated!
            BookInstance = Instantiate(Resources.Load(RESOURCES_BOOK, typeof(GameObject))) as GameObject;
            AppManager.I.ModalWindowActivated = true;
            Book.I.OpenArea(area);

            if (directMiniGameData != null)
            {
                var mainMiniGamesList = MiniGamesUtilities.GetMainMiniGameList();
                var mainMiniGame = mainMiniGamesList.FirstOrDefault(game => game.MainId == directMiniGameData.Main);
                var gamesPanel = FindObjectOfType<GamesPanel>();
                gamesPanel.DetailMainMiniGame(mainMiniGame);
                gamesPanel.DetailMiniGame(mainMiniGame.variations.FirstOrDefault(var => var.data == directMiniGameData));
                gamesPanel.ScrollTo(mainMiniGame.MainId);
            }
        }

        public void CloseBook()
        {
            BookInstance.SetActive(false);
            Destroy(BookInstance);
            AppManager.I.ModalWindowActivated = false;
        }

    }
}
