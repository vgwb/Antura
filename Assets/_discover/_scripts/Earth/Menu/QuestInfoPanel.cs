using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Antura.Minigames.DiscoverCountry
{
    public class QuestInfoPanel : MonoBehaviour
    {
        private int currentIndex = 0;
        public Quests QData;

        public TextMeshProUGUI Title;
        public TextMeshProUGUI Description;
        public Button PlayBtn;
        public Button PlayManual;
        [SerializeField] QuestInfoPanelSlideshow slideshow;

        private QuestData currentQuestData;

        void Start()
        {

        }

        public void Show(QuestData questData)
        {
            gameObject.SetActive(true);
            currentQuestData = questData;
            Title.text = questData.Code + " | " + questData.Title;
            Description.text = questData.Description + "\n\n";
            Description.text += "<b>Location:</b> " + questData.Location + "\n";
            if (questData.Categories != "")
                Description.text += "<b>Categories:</b> " + questData.Categories + "\n";
            if (questData.Duration > 0)
                Description.text += "<b>Duration:</b> " + questData.Duration + " min" + "\n";
            if (questData.LanguageRef != "")
                Description.text += "<b>References:</b> " + questData.LanguageRef + "\n";
            if (questData.Gameplay != "")
                Description.text += "<b>Gameplay:</b> " + questData.Gameplay + "\n\n";
            if (questData.Content != "")
                Description.text += "<b>Didactical Content:</b>\n" + questData.Content + "\n\n";

            if (currentQuestData.Thumbnail != null)
            {
                slideshow.SetImages(new List<Sprite>() { currentQuestData.Thumbnail });
            }
            else
            {
                slideshow.SetImages(null);
            }

            if (currentQuestData.Status != QuestStatus.Inactive)
            {
                PlayBtn.interactable = true;
            }
            else
            {
                PlayBtn.interactable = false;
            }

            if (currentQuestData.manualPage != "")
            {
                PlayManual.gameObject.SetActive(true);
            }
            else
            {
                PlayManual.gameObject.SetActive(false);
            }

        }

        public void Play()
        {
            EarthManager.I.OpenQuest(currentQuestData);
        }

        public void OpenManual()
        {
            Application.OpenURL("https://antura.org/manual/quests/" + currentQuestData.manualPage);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        public void BtnNextQuest()
        {
            currentIndex++;
            if (currentIndex >= QData.AvailableQuests.Count())
            {
                currentIndex = 0;
            }
            Show(QData.AvailableQuests[currentIndex]);
        }

        public void BtnPrevQuest()
        {
            currentIndex--;
            if (currentIndex < 0)
            {
                currentIndex = QData.AvailableQuests.Count() - 1;
            }
            Show(QData.AvailableQuests[currentIndex]);
        }

    }
}
