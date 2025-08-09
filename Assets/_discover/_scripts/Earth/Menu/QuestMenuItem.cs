using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Localization;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Antura.Discover
{
    public class QuestMenuItem : MonoBehaviour
    {
        [Header("References")]
        public TextMeshProUGUI Code;
        public TextMeshProUGUI Title;
        public TextMeshProUGUI Location;
        public Button SelectBtn;
        [SerializeField] GameObject[] stars;

        QuestData questData;
        CanvasGroup canvasGroup;

        public void Init(QuestData _questData)
        {
            canvasGroup = this.gameObject.AddComponent<CanvasGroup>();

            questData = _questData;
            SelectBtn.interactable = questData.Status != QuestStatus.Inactive;
            canvasGroup.alpha = SelectBtn.interactable ? 1 : 0.7f;
            Code.text = _questData.NumberCode;
            Title.text = _questData.Title.GetLocalizedString();
            Location.text = _questData.Location.Name.GetLocalizedString();
            SetStars(questData.GetScore());
        }

        public void OnSelectQuest()
        {
            EarthManager.I.SelectQuest(questData);
        }

        public void OnOpenQuest()
        {
            EarthManager.I.OpenQuest(questData);
        }

        void SetStars(int totStars)
        {
            for (int i = 0; i < stars.Length; i++)
            {
                stars[i].SetActive(i < totStars);
            }
        }
    }
}
