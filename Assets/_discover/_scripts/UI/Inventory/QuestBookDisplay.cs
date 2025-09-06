using System.Collections.Generic;
using UnityEngine;
using DG.DeInspektor.Attributes;
using Antura.Discover.UI;

namespace Antura.Discover
{
    public class QuestBookDisplay : MonoBehaviour
    {
        public QuestCardsUI CardsUI;
        private bool isOpen = false;

        void Start()
        {
            Init();
        }

        public void Init()
        {
            CloseBook();
        }

        public void OnToggleBook()
        {
            isOpen = !isOpen;
            if (isOpen)
                OpenBook();
            else
                CloseBook();
        }

        public void OpenBook()
        {
            isOpen = true;
            CardsUI.Init(QuestManager.I.CurrentQuest);
            gameObject.SetActive(true);
        }

        public void CloseBook()
        {
            isOpen = false;
            gameObject.SetActive(false);
        }


    }
}
