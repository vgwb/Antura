using System.Collections.Generic;
using Antura.Core;
using Demigiant.DemiTools;
using Antura.Profile;
using DG.DeExtensions;
using DG.DeInspektor.Attributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.UI
{
    public class ClassroomProfileDetailPanel : MonoBehaviour
    {
        #region EVENTS

        public ActionEvent OnBackClicked = new("ClassroomProfileDetailPanel.OnBackClicked");
        public ActionEvent<PlayerIconData> OnDeleteProfileRequested = new("ClassroomProfileDetailPanel.OnDeleteProfileRequested");
        public ActionEvent<PlayerIconData> OnEditProfileRequested = new("ClassroomProfileDetailPanel.OnEditProfileRequested");

        #endregion
        
        #region Serialized

        [DeEmptyAlert]
        [SerializeField] PlayerIcon playerIcon;
        [DeEmptyAlert]
        [SerializeField] TMP_Text tfName;
        [DeEmptyAlert]
        [SerializeField] TMP_Text tfLastAccess;
        [DeEmptyAlert]
        [SerializeField] ClassroomProfileLevelView levelViewPrefab;
        [DeEmptyAlert]
        [SerializeField] ClassroomProfileQuestView questViewPrefab;
        [DeEmptyAlert]
        [SerializeField] Button btEditProfileName;
        [DeEmptyAlert]
        [SerializeField] Button btDeleteProfile;
        [DeEmptyAlert]
        [SerializeField] Button btBack;
        [DeEmptyAlert]
        [SerializeField] ScrollRect scrollRect;

        #endregion

        PlayerIconData currProfile;
        readonly List<ClassroomProfileLevelView> levelViews = new();
        readonly List<ClassroomProfileQuestView> questViews = new();
        
        #region Unity

        void Awake()
        {
            levelViewPrefab.gameObject.SetActive(false);
            questViewPrefab.gameObject.SetActive(false);

            btBack.onClick.AddListener(OnBackClicked.Dispatch);
            btDeleteProfile.onClick.AddListener(() => OnDeleteProfileRequested.Dispatch(currProfile));
            btEditProfileName.onClick.AddListener(() => OnEditProfileRequested.Dispatch(currProfile));
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

        public void Fill(PlayerIconData profile)
        {
            Clear();

            currProfile = profile;
            ClassroomProfileDetail profileDetail = new ClassroomProfileDetail(profile);
            
            playerIcon.Init(profileDetail.Profile);
            RefreshProfileName();
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

        public void AssignNewProfileName(string newName)
        {
            currProfile.PlayerName = newName;
            PlayerProfile playerProfile = AppManager.I.PlayerProfileManager.GetPlayerProfileByUUID(currProfile.Uuid);
            AppManager.I.PlayerProfileManager.SavePlayerProfile(playerProfile);
            AppManager.I.PlayerProfileManager.UpdatePlayerIconDataInSettings(currProfile);
            RefreshProfileName();
        }

        #endregion

        #region Methods

        void Clear()
        {
            foreach (ClassroomProfileLevelView view in levelViews) view.gameObject.SetActive(false);
            foreach (ClassroomProfileQuestView view in questViews) view.gameObject.SetActive(false);
        }
        
        void RefreshProfileName()
        {
            tfName.text = currProfile.PlayerName.IsNullOrEmpty() ? "- - -" : currProfile.PlayerName;
            playerIcon.Init(currProfile);
        }

        #endregion
    }
}