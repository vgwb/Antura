using Antura.Discover.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Antura.Discover
{
    public class QuestBookDisplay : MonoBehaviour
    {
        public QuestCardsUI CardsUI;

        [Header("References")]
        public TextMeshProUGUI Title;
        public TextMeshProUGUI Description;

        public Button btClose;

        private bool isOpen;

        void Start()
        {
            Init();
        }

        public void Init()
        {
            btClose.onClick.AddListener(ClosePanel);
            Title.text = QuestManager.I.CurrentQuest.Title.GetLocalizedString() + " | " + QuestManager.I.CurrentQuest.Id;
            Description.text = QuestManager.I.CurrentQuest.Description.GetLocalizedString();
        }

        void OnDestroy()
        {
            btClose.onClick.RemoveListener(ClosePanel);
        }

        private void ClosePanel()
        {
            CloseBook();
        }

        public void ToggleBook()
        {
            if (isOpen)
                CloseBook();
            else
                OpenBook();
        }

        public void OpenBook()
        {
            if (isOpen)
                return;
            DiscoverGameManager.I.ChangeState(GameplayState.Dialogue);
            isOpen = true;
            CardsUI.Init(QuestManager.I.CurrentQuest);
            gameObject.SetActive(true);
            DiscoverNotifier.Game.OnActivityPanelToggled.Dispatch(true);
        }

        public void CloseBook()
        {
            var wasOpen = isOpen;
            isOpen = false;
            gameObject.SetActive(false);
            DiscoverGameManager.I.ChangeToPreviousState();
            if (wasOpen)
                DiscoverNotifier.Game.OnActivityPanelToggled.Dispatch(false);
        }
    }
}
