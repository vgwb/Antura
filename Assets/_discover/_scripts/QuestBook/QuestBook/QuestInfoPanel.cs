using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Antura.UI;

namespace Antura.Discover.UI
{
    public class QuestInfoPanel : MonoBehaviour
    {
        private int currentIndex = 0;
        public QuestListData QData;

        public TextMeshProUGUI Title;
        public TextMeshProUGUI Description;
        public Button PlayBtn;
        [SerializeField] QuestInfoPanelSlideshow slideshow;

        private QuestData currentQuestData;
        public QuestCardsUI QuestCardsUI;

        void Start()
        {
            QuestCardsUI = GetComponent<QuestCardsUI>();
        }

        public void Show(QuestData questData)
        {
            GlobalUI.ShowPauseMenu(false);
            Description.text = "";
            gameObject.SetActive(true);
            currentQuestData = questData;

            QuestCardsUI.Init(questData);


            Title.text = questData.Title.GetLocalizedString() + " | " + questData.Id;

            Description.text = "";
            if (questData.Description != null && !questData.Description.IsEmpty)
                Description.text += questData.Description.GetLocalizedString() + "\n";

            if (questData.Location != null)
                Description.text += "Location: " + questData.Location.Name.GetLocalizedString() + "\n";

            Description.text += "Subjects: " + questData.SubjectsListText + "\n";

            Description.text += "Difficulty: " + questData.Difficulty + "\n";

            if (questData.Duration > 0)
                Description.text += "Duration: " + questData.Duration + " min" + "\n";
            // if (questData.Words != null)
            // {
            //     Description.text += "<b>Words used:</b> ";
            //     foreach (var word in questData.Words)
            //     {
            //         Description.text += "- " + word.GetLocalizedString() + "\n";
            //     }
            // }
            // if (questData.Gameplay != null)
            //     Description.text += "<b>Gameplay:</b> " + questData.Gameplay + "\n\n";
            // if (questData.Content != "")
            //     Description.text += "<b>Didactical Content:</b>\n" + questData.Content + "\n\n";

            if (currentQuestData.Thumbnail != null)
            {
                slideshow.SetImages(new List<Sprite>() { currentQuestData.Thumbnail.Image });
            }
            else
            {
                slideshow.SetImages(null);
            }

            if (currentQuestData.Status != Status.Standby)
            {
                PlayBtn.interactable = true;
            }
            else
            {
                PlayBtn.interactable = false;
            }

        }

        public void Play()
        {
            UIQuestMenuManager.I.OpenQuest(currentQuestData);
        }

        public void OpenManual()
        {
            // Application.OpenURL("https://antura.org/manual/quests/" + currentQuestData.manualPage);
        }

        public void Close()
        {
            gameObject.SetActive(false);
            GlobalUI.ShowPauseMenu(true);
        }

        public void BtnNextQuest()
        {
            currentIndex++;
            if (currentIndex >= QData.QuestList.Count())
            {
                currentIndex = 0;
            }
            Show(QData.QuestList[currentIndex]);
        }

        public void BtnPrevQuest()
        {
            currentIndex--;
            if (currentIndex < 0)
            {
                currentIndex = QData.QuestList.Count() - 1;
            }
            Show(QData.QuestList[currentIndex]);
        }

    }
}
