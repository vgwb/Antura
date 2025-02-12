using System.Collections.Generic;
using Demigiant.DemiTools;
using DG.DeInspektor.Attributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.Minigames.DiscoverCountry
{
    public class ClassroomProfileDetailPanel : MonoBehaviour
    {
        #region EVENTS

        public ActionEvent OnBackClicked = new("ClassroomProfileDetailPanel.OnBackClicked");

        #endregion
        
        #region Serialized

        [DeEmptyAlert]
        [SerializeField] Image profileImg;
        [DeEmptyAlert]
        [SerializeField] TMP_Text tfName;
        [DeEmptyAlert]
        [SerializeField] TMP_Text tfLastAccess;
        [DeEmptyAlert]
        [SerializeField] ClassroomProfileLevelView levelViewPrefab;
        [DeEmptyAlert]
        [SerializeField] ClassroomProfileQuestView questViewPrefab;
        [DeEmptyAlert]
        [SerializeField] Button btBack;
        [DeEmptyAlert]
        [SerializeField] ScrollRect scrollRect;

        #endregion

        readonly List<ClassroomProfileLevelView> levelViews = new();
        readonly List<ClassroomProfileQuestView> questViews = new();
        
        #region Unity

        void Awake()
        {
            levelViewPrefab.gameObject.SetActive(false);
            questViewPrefab.gameObject.SetActive(false);
            
            btBack.onClick.AddListener(OnBackClicked.Dispatch);
        }

        #endregion

        #region Public Methods

        public void Open(bool doOpen)
        {
            if (doOpen)
            {
                this.gameObject.SetActive(true);
                scrollRect.verticalNormalizedPosition = 1;
            }
            else
            {
                this.gameObject.SetActive(false);
            }
        }

        public void Fill(UserProfileDetail profileDetail)
        {
            Clear();
            
            profileImg.sprite = profileDetail.ProfilePic;
            tfName.text = profileDetail.Name;
            tfLastAccess.text = $"Last access: {profileDetail.LastAccess.Day:00}/{profileDetail.LastAccess.Month:00}/{profileDetail.LastAccess.Year} - {profileDetail.LastAccess.Hour:00}:{profileDetail.LastAccess.Minute:00}";
            
            int totLevels = profileDetail.Levels.Count;
            while (levelViews.Count < totLevels)
            {
                ClassroomProfileLevelView view = Instantiate(levelViewPrefab, levelViewPrefab.transform.parent);
                view.transform.SetSiblingIndex(levelViewPrefab.transform.GetSiblingIndex() + levelViews.Count + 1);
                levelViews.Add(view);
            }
            for (int i = 0; i < totLevels; i++)
            {
                ClassroomProfileLevelView view = levelViews[i];
                view.Fill(profileDetail.Levels[i]);
                view.gameObject.SetActive(true);
            }
            
            int totQuests = profileDetail.Quests.Count;
            while (questViews.Count < totQuests)
            {
                ClassroomProfileQuestView view = Instantiate(questViewPrefab, questViewPrefab.transform.parent);
                view.transform.SetSiblingIndex(questViewPrefab.transform.GetSiblingIndex() + questViews.Count + 1);
                questViews.Add(view);
            }
            for (int i = 0; i < totQuests; i++)
            {
                ClassroomProfileQuestView view = questViews[i];
                view.Fill(profileDetail.Quests[i]);
                view.gameObject.SetActive(true);
            }
        }

        #endregion

        #region Methods

        void Clear()
        {
            foreach (ClassroomProfileLevelView view in levelViews) view.gameObject.SetActive(false);
            foreach (ClassroomProfileQuestView view in questViews) view.gameObject.SetActive(false);
        }

        #endregion
    }
}