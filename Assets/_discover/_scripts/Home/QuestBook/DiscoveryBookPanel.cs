using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Antura.Discover
{
    public class DiscoveryBookPanel : MonoBehaviour
    {
        private int currentIndex = 0;
        public QuestListData QData;
        public TextMeshProUGUI Title;
        public TextMeshProUGUI Description;
        public GameObject Thumbnail;

        private QuestData questData;

        public void Show()
        {
            gameObject.SetActive(true);
            Display();
        }

        private void Display()
        {
            questData = QData.QuestList[currentIndex];

            Title.text = questData.Id + " | " + questData.Title;
            Description.text = questData.Description + "\n\n";
            Description.text += "<b>Location:</b> " + questData.Location + "\n";
            Description.text += "<b>Categories:</b> " + "" + "\n";
            Description.text += "<b>Duration:</b> " + questData.Duration + " min" + "\n";
            if (questData.Words != null)
            {
                Description.text += "<b>Words used:</b> ";
                foreach (var word in questData.Words)
                {
                    Description.text += "- " + word.GetLocalizedString() + "\n";
                }
            }
            Description.text += "<b>Gameplay:</b> " + questData.Gameplay + "\n\n";
            Description.text += "<b>Didactical Content:</b>\n" + "" + "\n\n";

            if (questData.Thumbnail != null)
            {
                Thumbnail.SetActive(true);
                Thumbnail.GetComponent<Image>().sprite = questData.Thumbnail.Image;
            }
            else
            {
                Thumbnail.SetActive(false);
            }
        }

        public void BtnNext()
        {
            currentIndex++;
            if (currentIndex >= QData.QuestList.Count())
            {
                currentIndex = 0;
            }
            Display();
        }

        public void BtnPrev()
        {
            currentIndex--;
            if (currentIndex < 0)
            {
                currentIndex = QData.QuestList.Count() - 1;
            }
            Display();
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }
    }
}
