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

            bool isEditor = Application.isEditor && DiscoverAppManager.I.CurrentProfile.profile.godMode;
            bool locked = questData.Status == Status.Standby ||
                          (!isEditor && questData.Status == Status.Development);

            SelectBtn.interactable = !locked;
            canvasGroup.alpha = SelectBtn.interactable ? 1 : 0.7f;

            Lock.SetActive(locked);

            Code.text = _questData.IdDisplay;
            // Debug.Log("QuestMenuItem Init: " + _questData.Code);
            Title.text = _questData.Title.GetLocalizedString();

            if (isEditor)
            {
                Location.text = _questData.Status.ToString() + " - " + _questData.Id;
            }
            else
            {
                Location.text = "";
            }

            if (_questData.Location != null)
            {
                Location.text += _questData.Location.Name.GetLocalizedString();

            }

            SetStars(questData.GetBestStars());
        }

        public void OnSelectQuest()
        {
            UIDiscoverHome.I.SelectQuest(questData);
        }

        public void OnOpenQuest()
        {
            UIDiscoverHome.I.OpenQuest(questData);
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
