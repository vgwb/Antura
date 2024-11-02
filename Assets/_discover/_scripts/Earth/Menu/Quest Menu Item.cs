using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Antura.Minigames.DiscoverCountry
{
    public class QuestMenuItem : MonoBehaviour
    {
        [Header("References")]
        public TextMeshProUGUI Code;
        public TextMeshProUGUI Title;
        public TextMeshProUGUI Location;
        public Button SelectBtn;
        public Button ConfirmBtn;

        private QuestData questData;

        public void Init(QuestData _questData)
        {
            questData = _questData;
            //SelectBtn.interactable = questData.Active;
            ConfirmBtn.interactable = questData.Active;
            Code.text = _questData.Code;
            Title.text = _questData.Title;
            Location.text = _questData.Location;
        }

        public void OnSelectQuest()
        {
            EarthManager.I.SelectQuest(questData);
        }

        public void OnOpenQuest()
        {
            EarthManager.I.OpenQuest(questData);
        }

    }
}
