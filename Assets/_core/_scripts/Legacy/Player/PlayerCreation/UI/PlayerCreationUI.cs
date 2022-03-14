#if FALSE
using Antura.Audio;
using Antura.Core;
using Antura.Scenes;
using Antura.UI;
using DG.DeExtensions;
using DG.Tweening;
using System;
using Antura.Keeper;
using UnityEngine;

namespace Antura.Profile
{
    /// <summary>
    /// Player creation UI
    /// </summary>
    public class PlayerCreationUI : MonoBehaviour
    {
        public enum UIState
        {
            AvatarCreation,
            AgeSelection
        }

        public enum CategoryType
        {
            Age,
            Gender,
            Avatar,
            Color
        }

        static class CategoryIndex
        {
            public const int Gender = 0;
            public const int Avatar = 1;
            public const int Color = 2;
        }

#region Serialized

        [Tooltip("Startup offset of categories")]
        public int StartupOffsetY = -160;

        public UIButton BtContinue;
        public RectTransform CategoriesContainer;
        public PlayerCreationUICategory[] Categories; // 0: gender // 1: avatar // 2: color
        public PlayerCreationUICategory AgeCategory;

#endregion

        bool allAvatarCategoriesSelected
        {
            get {
                foreach (var cat in Categories) {
                    if (cat.SelectedIndex < 0) {
                        return false;
                    }
                }
                return true;
            }
        }

        public static UIState State { get; private set; }
        int selectionStep = 0; // 0: age // 1: gender // 2: avatar // 3: color
        float selectionStepOffsetY;
        Tween stepTween;

#region Unity

        void Awake()
        {
            State = UIState.AvatarCreation;
        }

        void Start()
        {
            selectionStepOffsetY = StartupOffsetY / (Categories.Length - 1f);
            CategoriesContainer.SetAnchoredPosY(StartupOffsetY);
            for (var i = 0; i < Categories.Length; ++i) {
                Categories[i].gameObject.SetActive(i == 0);
            }
            BtContinue.gameObject.SetActive(false);
            AgeCategory.gameObject.SetActive(false);

            // Listeners
            BtContinue.Bt.onClick.AddListener(OnContinue);
            foreach (PlayerCreationUICategory cat in Categories) {
                cat.OnSelect += OnSelectCategory;
                cat.OnDeselectAll += OnDeselectAllInCategory;
            }
            AgeCategory.OnSelect += OnSelectCategory;
            AgeCategory.OnDeselectAll += OnDeselectAllInCategory;

            playAudioDescription(0);
        }

        void OnDestroy()
        {
            BtContinue.Bt.onClick.RemoveAllListeners();
            foreach (PlayerCreationUICategory cat in Categories) {
                cat.OnSelect -= OnSelectCategory;
                cat.OnDeselectAll -= OnDeselectAllInCategory;
            }
            AgeCategory.OnSelect -= OnSelectCategory;
            AgeCategory.OnDeselectAll -= OnDeselectAllInCategory;
            stepTween.Kill();
        }

#endregion

#region Methods

        void SwitchState(UIState toState)
        {
            if (State == toState) { return; }

            State = toState;
            PlayerCreationUICategory avatarCat = Categories[CategoryIndex.Avatar];
            switch (toState) {
                case UIState.AgeSelection:
                    for (var i = 0; i < avatarCat.UIButtons.Length; i++) {
                        avatarCat.UIButtons[i].gameObject.SetActive(i == avatarCat.SelectedIndex);
                        if (i == avatarCat.SelectedIndex) avatarCat.UIButtons[i].transform.localScale = Vector3.one * 1.65f;
                    }
                    foreach (var cat in Categories) {
                        if (cat != avatarCat) {
                            cat.gameObject.SetActive(false);
                        }
                    }
                    AgeCategory.gameObject.SetActive(true);
                    BtContinue.StopPulsing();
                    BtContinue.gameObject.SetActive(false);
                    KeeperManager.I.PlayDialogue(Database.LocalizationDataId.Profile_Age);
                    break;
                case UIState.AvatarCreation:
                    AgeCategory.gameObject.SetActive(false);
                    foreach (UIButton catBt in avatarCat.UIButtons) {
                        catBt.gameObject.SetActive(true);
                    }
                    foreach (PlayerCreationUICategory cat in Categories) {
                        if (cat != avatarCat) {
                            cat.gameObject.SetActive(true);
                        }
                    }
                    OnSelectCategory(avatarCat, avatarCat.UIButtons[avatarCat.SelectedIndex]);
                    break;
            }
        }

        void playAudioDescription(int SelectedIndex)
        {
            //Debug.Log("SelectedIndex: " + SelectedIndex);
            switch (SelectedIndex) {
                case 0:
                    KeeperManager.I.PlayDialogue(Database.LocalizationDataId.Profile_Gender);
                    break;
                case 1:
                    KeeperManager.I.PlayDialogue(Database.LocalizationDataId.Profile_Avatar);
                    break;
                case 2:
                    KeeperManager.I.PlayDialogue(Database.LocalizationDataId.Profile_Color);
                    break;
            }
        }

        void AvatarCreation_NextStep()
        {
            selectionStep++;
            if (stepTween != null) stepTween.Complete();
            Categories[selectionStep].gameObject.SetActive(true);
            stepTween = CategoriesContainer.DOAnchorPosY(StartupOffsetY - selectionStepOffsetY * selectionStep, 0.4f);

            playAudioDescription(selectionStep);
        }

        void AvatarCreation_StepBackwards(int toStep)
        {
            if (stepTween != null) { stepTween.Complete(); }
            for (var i = toStep + 1; i < selectionStep + 1; ++i) {
                PlayerCreationUICategory cat = Categories[i];
                if (i == CategoryIndex.Color) {
                    // Reset avatars colors
                    Categories[CategoryIndex.Avatar].ResetColor();
                }
                cat.Select(-1);
                cat.gameObject.SetActive(false);
            }
            selectionStep = toStep;
            stepTween = CategoriesContainer.DOAnchorPosY(StartupOffsetY - selectionStepOffsetY * selectionStep, 0.4f);
            playAudioDescription(selectionStep);
        }

        void AvatarCreation_SetGender()
        {
            Categories[CategoryIndex.Avatar].AvatarSetIcon(Categories[CategoryIndex.Gender].SelectedIndex == 1);
            if (AppManager.I.Player != null) {
                AppManager.I.Player.Gender = Categories[CategoryIndex.Gender].SelectedIndex == 0 ? PlayerGender.M : PlayerGender.F;
            } else {
                AppManager.I.PlayerProfileManager.TemporaryPlayerGender = Categories[CategoryIndex.Gender].SelectedIndex == 0 ? PlayerGender.M : PlayerGender.F;
            }
            //    Debug.Log("AvatarCreation_SetGender " + AppManager.I.PlayerProfileManager.TemporaryPlayerGender);
        }

        void CreateProfile()
        {
            PlayerCreationScene.CreatePlayer(
                AgeCategory.SelectedIndex + 4,
                Categories[CategoryIndex.Gender].SelectedIndex == 0 ? PlayerGender.M : PlayerGender.F,
                Categories[CategoryIndex.Avatar].SelectedIndex + 1,
                (PlayerTint)(Categories[CategoryIndex.Color].SelectedIndex + 1)
            );
        }

#endregion

#region Callbacks

        void OnSelectCategory(PlayerCreationUICategory category, UIButton uiButton)
        {
            switch (State) {
                case UIState.AvatarCreation:
                    int catIndex = Array.IndexOf(Categories, category);
                    if (selectionStep < Categories.Length - 1 && catIndex == selectionStep) {
                        AvatarCreation_NextStep();
                    }

                    switch (catIndex) {
                        case CategoryIndex.Gender:
                            AvatarCreation_SetGender();
                            break;
                        case CategoryIndex.Color:
                            Categories[CategoryIndex.Avatar].SetColor(uiButton.DefaultColor);
                            break;
                    }

                    if (allAvatarCategoriesSelected) {
                        BtContinue.gameObject.SetActive(true);
                        BtContinue.Pulse();
                        KeeperManager.I.PlayDialogue(Database.LocalizationDataId.Action_PressPlay);
                    }
                    break;
                case UIState.AgeSelection:
                    switch (category.CategoryType) {
                        case CategoryType.Age:
                            BtContinue.gameObject.SetActive(true);
                            BtContinue.Pulse();
                            var age_selected = AgeCategory.SelectedIndex + 4;
                            KeeperManager.I.PlayDialogue("Profile_Years_" + age_selected.ToString());
                            break;
                        case CategoryType.Avatar:
                            SwitchState(UIState.AvatarCreation);
                            break;
                    }
                    break;
            }
        }

        void OnDeselectAllInCategory(PlayerCreationUICategory category)
        {
            BtContinue.StopPulsing();
            BtContinue.gameObject.SetActive(false);
            switch (State) {
                case UIState.AvatarCreation:
                    var catIndex = Array.IndexOf(Categories, category);
                    if (catIndex < selectionStep) {
                        AvatarCreation_StepBackwards(catIndex);
                    } else if (catIndex == CategoryIndex.Color) {
                        Categories[CategoryIndex.Avatar].ResetColor();
                    }
                    break;
            }
        }

        void OnContinue()
        {
            switch (State) {
                case UIState.AvatarCreation:
                    SwitchState(UIState.AgeSelection);
                    break;
                case UIState.AgeSelection:
                    CreateProfile();
                    break;
            }
        }

#endregion
    }
}
#endif
