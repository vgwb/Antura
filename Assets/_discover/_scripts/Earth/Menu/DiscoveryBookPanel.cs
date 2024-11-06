using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Antura.Minigames.DiscoverCountry
{
    public class DiscoveryBookPanel : MonoBehaviour
    {
        private int currentIndex = 0;
        public Quests QData;
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
            questData = QData.AvailableQuests[currentIndex];

            Title.text = questData.Code + " | " + questData.Title;
            Description.text = questData.Description + "\n\n";
            Description.text += "<b>Location:</b> " + questData.Location + "\n";
            Description.text += "<b>Categories:</b> " + questData.Categories + "\n";
            Description.text += "<b>Duration:</b> " + questData.Duration + " min" + "\n";
            Description.text += "<b>References:</b> " + questData.LanguageRef + "\n";
            Description.text += "<b>Gameplay:</b> " + questData.Gameplay + "\n\n";
            Description.text += "<b>Didactical Content:</b>\n" + questData.Content + "\n\n";

            if (questData.Thumbnail != null)
            {
                Thumbnail.SetActive(true);
                Thumbnail.GetComponent<Image>().sprite = questData.Thumbnail;
            }
            else
            {
                Thumbnail.SetActive(false);
            }
        }

        public void BtnNext()
        {
            currentIndex++;
            if (currentIndex >= QData.AvailableQuests.Count())
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
                currentIndex = QData.AvailableQuests.Count() - 1;
            }
            Display();
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }
    }
}
