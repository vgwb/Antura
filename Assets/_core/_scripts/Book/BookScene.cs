using Antura.UI;
using Antura.Core;
using UnityEngine;

namespace Antura.Book
{

    public class BookScene : MonoBehaviour
    {
        [Header("Scene Setup")]
        public Music SceneMusic;
        public BookArea OpeningArea;

        [Header("References")]
        public GameObject[] HideAtStartup;

        void Start()
        {
            GlobalUI.ShowPauseMenu(false);
            foreach (var go in HideAtStartup)
            {
                go.SetActive(false);
            }

            if (OpeningArea != BookArea.None)
            {
                BookManager.I.OpenBook(OpeningArea);
            }

            //GlobalUI.ShowBackButton(true, GoBackCustom);
            //AudioManager.I.PlayMusic(SceneMusic);
            //AudioManager.I.PlayDialogue("Book_Intro");

            //HideAllPanels();
            //if (OverridenOpeningArea != BookArea.None) {
            //    OpenArea(OverridenOpeningArea);
            //} else {
            //    OpenArea(OpeningArea);
            //}

            // Debug.Log("PREV SCENE IS RESERVED AREA: " + AppManager.I.NavigationManager.PrevSceneIsReservedArea());
        }

        public void BtnOpenVocabulary()
        {
            BookManager.I.OpenBook(BookArea.Vocabulary);
        }

        public void BtnOpenPlayer()
        {
            BookManager.I.OpenBook(BookArea.Player);
        }

        public void BtnOpenJourney()
        {
            BookManager.I.OpenBook(BookArea.Journey);
        }

        public void BtnOpenGames()
        {
            BookManager.I.OpenBook(BookArea.MiniGames);
        }

        public void GoBackCustom()
        {
            AppManager.I.NavigationManager.GoBack();
        }
    }
}
