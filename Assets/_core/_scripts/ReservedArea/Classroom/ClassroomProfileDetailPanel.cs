using Antura.Core;
using Demigiant.DemiTools;
using Antura.Profile;
using DG.DeExtensions;
using DG.DeInspektor.Attributes;
using Demigiant.DemiTools.DeUnityExtended;
using TMPro;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.UI
{
    public class ClassroomProfileDetailPanel : MonoBehaviour
    {
        #region EVENTS

        public ActionEvent OnBackClicked = new("ClassroomProfileDetailPanel.OnBackClicked");
        public ActionEvent<PlayerProfilePreview> OnDeleteProfileRequested = new("ClassroomProfileDetailPanel.OnDeleteProfileRequested");
        public ActionEvent<PlayerProfilePreview> OnEditProfileRequested = new("ClassroomProfileDetailPanel.OnEditProfileRequested");

        #endregion

        #region Serialized

        [DeEmptyAlert]
        [SerializeField] PlayerIcon playerIcon;
        [DeEmptyAlert]
        [SerializeField] TMP_Text tfName;
        [DeEmptyAlert]
        [SerializeField] TMP_Text tfUserInfo;
        [DeEmptyAlert]
        [SerializeField] ClassroomProfileLevelView levelViewPrefab;
        [DeEmptyAlert]
        [SerializeField] ClassroomProfileQuestView questViewPrefab;
        [DeEmptyAlert]
        [SerializeField] Button btEditProfileName;
        [DeEmptyAlert]
        [SerializeField] Button btDeleteProfile;
        [DeEmptyAlert]
        [SerializeField] DeUIButton btEasyMode;
        [DeEmptyAlert]
        [SerializeField] Button btChangeLangMode;

        [SerializeField] Button btBack;
        [DeEmptyAlert]
        [SerializeField] ScrollRect scrollRect;

        #endregion

        PlayerProfilePreview currProfile;
        readonly List<ClassroomProfileLevelView> levelViews = new();
        readonly List<ClassroomProfileQuestView> questViews = new();

        #region Unity

        void Awake()
        {
            levelViewPrefab.gameObject.SetActive(false);
            questViewPrefab.gameObject.SetActive(false);

            btBack.onClick.AddListener(OnBackClicked.Dispatch);
            btDeleteProfile.onClick.AddListener(
                () => OnDeleteProfileRequested.Dispatch(currProfile)
                );
            btEditProfileName.onClick.AddListener(
                () => OnEditProfileRequested.Dispatch(currProfile)
                );

            btEasyMode.onClick.AddListener(() =>
                {
                    // Debug.Log($"Setting Easy Mode to {btEasyMode.isOn} for profile {currProfile.Uuid}");
                    currProfile.EasyMode = !btEasyMode.isOn;
                    ClassroomHelper.SaveProfile(currProfile);
                    playerIcon.Init(currProfile);
                });

            btChangeLangMode.onClick.AddListener(() => OpenSelectLangMode());

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

        public void Fill(PlayerProfilePreview profile)
        {
            Clear();

            currProfile = profile;
            ClassroomProfileDetail profileDetail = new ClassroomProfileDetail(profile);

            playerIcon.Init(profileDetail.PlayerPreviewData);
            RefreshProfileInfo();
            tfUserInfo.text = $"Last access: {profileDetail.LastAccess.Day:00}/{profileDetail.LastAccess.Month:00}/{profileDetail.LastAccess.Year} - {profileDetail.LastAccess.Hour:00}:{profileDetail.LastAccess.Minute:00}";

            btEasyMode.Toggle(currProfile.EasyMode, false);

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
            ClassroomHelper.SaveProfile(currProfile);
            RefreshProfileInfo();
        }

        public void OpenSelectLangMode(bool showCloseButton = true)
        {
            var popup_title = LocalizationManager.GetNewLocalized("profile.chooseclasse");
            var talkToPlayerModeStrings = new List<string>();
            foreach (TalkToPlayerMode mode in Enum.GetValues(typeof(TalkToPlayerMode)))
            {
                string localizedString = LocalizationManager.GetNewLocalized($"profile.TalkToPlayerMode.{(int)mode}");
                talkToPlayerModeStrings.Add(localizedString);
            }

            GlobalPopups.OpenSelector(popup_title, talkToPlayerModeStrings, SelectLangMode, showCloseButton, (int)currProfile.TalkToPlayerStyle);
            RefreshProfileInfo();
        }

        private void SelectLangMode(int talkModeIndex)
        {
            TalkToPlayerMode selectedMode = (TalkToPlayerMode)talkModeIndex;
            currProfile.TalkToPlayerStyle = selectedMode;
            ClassroomHelper.SaveProfile(currProfile);
            RefreshProfileInfo();
        }

        #endregion

        #region Methods

        void Clear()
        {
            foreach (ClassroomProfileLevelView view in levelViews)
                view.gameObject.SetActive(false);
            foreach (ClassroomProfileQuestView view in questViews)
                view.gameObject.SetActive(false);
        }

        void RefreshProfileInfo()
        {
            tfName.text = currProfile.PlayerName.IsNullOrEmpty() ? "- - -" : currProfile.PlayerName;
            playerIcon.Init(currProfile);
        }

        #endregion
    }
}
