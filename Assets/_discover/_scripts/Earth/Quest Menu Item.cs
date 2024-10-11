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
        public TextMeshProUGUI Title;
        public Button SelectBtn;
        public Button ConfirmBtn;

        private QuestData questData;

        public void Init(QuestData _questData)
        {
            questData = _questData;
            SelectBtn.interactable = questData.Active;
            ConfirmBtn.interactable = questData.Active;
            Title.text = _questData.Title;
        }

        public void OnSelectQuest()
        {
            EarthManager.I.SelectQuest(questData);
        }

        public void OnOpenQuest()
        {
            EarthManager.I.SelectQuest(questData);
        }

    }
}
