using System.Collections.Generic;
using UnityEngine;
using DG.DeInspektor.Attributes;
using Antura.Discover.UI;

namespace Antura.Discover
{
    public class QuestBookDisplay : MonoBehaviour
    {
        public QuestCardsUI CardsUI;

        void Start()
        {

        }

        public void OnOpenBook()
        {
            CardsUI.Init(QuestManager.I.CurrentQuest);
            gameObject.SetActive(true);
        }

        public void OnCloseBook()
        {
            gameObject.SetActive(false);
        }


    }
}
