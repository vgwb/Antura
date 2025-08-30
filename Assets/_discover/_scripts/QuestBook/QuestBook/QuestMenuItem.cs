using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Antura.Discover.UI
{
    public class QuestMenuItem : MonoBehaviour
    {
        [Header("References")]
        public TextMeshProUGUI Code;
        public TextMeshProUGUI Title;
        public TextMeshProUGUI Location;
        public Button SelectBtn;
        public GameObject Lock;
        [SerializeField] GameObject[] stars;

        QuestData questData;
        CanvasGroup canvasGroup;

        public void Init(QuestData _questData)
        {
            canvasGroup = this.gameObject.AddComponent<CanvasGroup>();

            questData = _questData;
            SelectBtn.interactable = questData.Status != Status.Standby;
            canvasGroup.alpha = SelectBtn.interactable ? 1 : 0.7f;
            Lock.SetActive(questData.Status == Status.Standby);
            Code.text = _questData.IdDisplay;
            // Debug.Log("QuestMenuItem Init: " + _questData.Code);
            Title.text = _questData.Title.GetLocalizedString();
            if (_questData.Location != null)
                Location.text = _questData.Location.Name.GetLocalizedString();
            SetStars(questData.GetBestStars());
        }

        public void OnSelectQuest()
        {
            UIQuestMenuManager.I.SelectQuest(questData);
        }

        public void OnOpenQuest()
        {
            UIQuestMenuManager.I.OpenQuest(questData);
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
