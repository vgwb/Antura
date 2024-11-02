using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Antura.Minigames.DiscoverCountry
{
    public class QuestInfoPanel : MonoBehaviour
    {
        public TextMeshProUGUI Title;
        public TextMeshProUGUI Description;

        void Start()
        {

        }

        public void Show(QuestData questData)
        {
            Title.text = questData.Code + " | " + questData.Title;
            Description.text = questData.Description + "\n\n";
            Description.text += "<b>Location:</b> " + questData.Location + "\n";
            Description.text += "<b>Categories:</b> " + questData.Categories + "\n";
            Description.text += "<b>Duration:</b> " + questData.Duration + " min" + "\n";
            Description.text += "<b>References:</b> " + questData.LanguageRef + "\n";
            Description.text += "<b>Gameplay:</b> " + questData.Gameplay + "\n\n";
            Description.text += "<b>Didactical Content:</b>\n" + questData.Content + "\n\n";
            gameObject.SetActive(true);
        }

        public void OpenManual()
        {
            Application.OpenURL("https://docs.antura.org");
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

    }
}
