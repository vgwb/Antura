using Antura.Core;
using Antura.UI;
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
        public TextRender Title;
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

            bool isTeacher = DiscoverAppManager.I.CurrentProfile.profile.godMode; // Application.isEditor &&
            bool locked = questData.Status == Status.Standby ||
                        questData.Status == Status.Development ||
                          (!isTeacher && questData.Status == Status.NeedsReview);

            SelectBtn.interactable = !locked;
            canvasGroup.alpha = SelectBtn.interactable ? 1 : 0.7f;

            Lock.SetActive(locked);

            Code.text = _questData.IdDisplay;
            Title.text = LocalizationSystem.I.GetLocalizedString(_questData.Title, true);

            if (isTeacher)
            {
                Location.text = _questData.Status.ToString() + " - " + _questData.Id;
            }
            else
            {
                Location.text = "";
            }

            if (_questData.Location != null)
            {
                Location.text += LocalizationSystem.I.GetLocalizedString(_questData.Location.Name, true);
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
