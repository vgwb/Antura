using Antura.Discover.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.Discover
{
    public class QuestBookDisplay : MonoBehaviour
    {
        public QuestCardsUI CardsUI;

        [Header("References")]
        public Button btClose;

        private bool isOpen;

        void Start()
        {
            Init();
        }

        public void Init()
        {
            btClose.onClick.AddListener(ClosePanel);
        }

        void OnDestroy()
        {
            btClose.onClick.RemoveListener(ClosePanel);
        }

        private void ClosePanel()
        {
            CloseBook();
        }

        public void OpenBook()
        {
            if (isOpen)
                return;
            DiscoverGameManager.I.ChangeState(GameplayState.Dialogue);
            isOpen = true;
            CardsUI.Init(QuestManager.I.CurrentQuest);
            gameObject.SetActive(true);
        }

        public void CloseBook()
        {
            isOpen = false;
            gameObject.SetActive(false);
            DiscoverGameManager.I.ChangeToPreviousState();
        }
    }
}
