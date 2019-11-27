using Antura.Audio;
using Antura.Core;
using Antura.Scenes;
using Antura.UI;
using DG.DeExtensions;
using DG.Tweening;
using System;
using System.Collections;
using Antura.Keeper;
using UnityEngine;
using UnityEngine.UI;

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
            BgColorSelection
        }

        public enum CategoryType
        {
            SkinColor,
            Avatar,
            HairColor,
            BgColor
        }

        static class CategoryIndex
        {
            public const int SkinColor = 0;
            public const int Avatar = 1;
            public const int HairColor = 2;
            public const int BgColor = 3;
        }

        #region Serialized

        [Tooltip("Startup offset of categories")]
        public int StartupOffsetY = -160;

        public UIButton BtContinue;
        public RectTransform CategoriesContainer;
        public PlayerCreationUICategory[] Categories; // 0: skinColor // 1: avatar // 2: hairColor
        public PlayerCreationUICategory BgColorCategory;

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
        int selectionStep = 0; // 0: skinColor // 1: avatar // 2: hairColor // 3: bgColor
        float selectionStepOffsetY;
        UIButton[] _avatarBts, _hairColorBts;
        PlayerCreationAvatar[] _avatars;
        PlayerCreationAvatar[] _hairAvatars;
        Color[] skinColors, hairColors, bgColors;
        Color currSkinColor = Color.black, currHairColor = Color.black, currBgColor = Color.black;
        int currAvatarId = -1;
        Tween stepTween;

        #region Unity

        void Awake()
        {
            State = UIState.AvatarCreation;
        }

        IEnumerator Start()
        {
            _avatars = GetUICategoryObj(CategoryType.Avatar).GetComponentsInChildren<PlayerCreationAvatar>(true);
            _avatarBts = GetUICategoryObj(CategoryType.Avatar).GetComponentsInChildren<UIButton>(true);
            _hairColorBts = GetUICategoryObj(CategoryType.HairColor).GetComponentsInChildren<UIButton>(true);
            hairColors = new Color[_hairColorBts.Length];
            for (int i = 0; i < _hairColorBts.Length; i++) hairColors[i] = _hairColorBts[i].Bt.image.color;
            UIButton[] bts = GetUICategoryObj(CategoryType.SkinColor).GetComponentsInChildren<UIButton>(true);
            skinColors = new Color[bts.Length];
            for (int i = 0; i < bts.Length; i++) {
                skinColors[i] = bts[i].Bt.image.color;
                bts[i].BtToggleOffColor = bts[i].Bt.image.color;
            }
            bts = GetUICategoryObj(CategoryType.BgColor).GetComponentsInChildren<UIButton>(true);
            bgColors = new Color[bts.Length];
            for (int i = 0; i < bts.Length; i++) {
                bgColors[i] = bts[i].Bt.image.color;
                bts[i].BtToggleOffColor = bts[i].Bt.image.color;
            }

            // Create hair avatars
            _hairAvatars = new PlayerCreationAvatar[_hairColorBts.Length];
            for (int i = 0; i < _hairColorBts.Length; ++i) {
                UIButton bt = _hairColorBts[i];
                PlayerCreationAvatar avatar = Instantiate(_avatars[0], bt.transform.parent, false);
                avatar.SetHairColor(bt.Bt.image.color);
                Destroy(bt.gameObject);
                _hairColorBts[i] = avatar.GetComponent<UIButton>();
                _hairAvatars[i] = avatar;
            }

            selectionStepOffsetY = StartupOffsetY / (Categories.Length - 1f);
            CategoriesContainer.SetAnchoredPosY(StartupOffsetY);
            for (var i = 0; i < Categories.Length; ++i) {
                Categories[i].gameObject.SetActive(i == 0);
            }
            BtContinue.gameObject.SetActive(false);
            BgColorCategory.gameObject.SetActive(false);

            // Initialize categories (wait)
            yield return null;
            foreach (PlayerCreationUICategory cat in Categories) cat.Init();
            BgColorCategory.Init();

            // Listeners
            BtContinue.Bt.onClick.AddListener(OnContinue);
            foreach (PlayerCreationUICategory cat in Categories) {
                cat.OnSelect += OnSelectCategory;
                cat.OnDeselectAll += OnDeselectAllInCategory;
            }
            BgColorCategory.OnSelect += OnSelectCategory;
            BgColorCategory.OnDeselectAll += OnDeselectAllInCategory;

            playAudioDescription(0);
        }

        void OnDestroy()
        {
            BtContinue.Bt.onClick.RemoveAllListeners();
            foreach (PlayerCreationUICategory cat in Categories) {
                cat.OnSelect -= OnSelectCategory;
                cat.OnDeselectAll -= OnDeselectAllInCategory;
            }
            BgColorCategory.OnSelect -= OnSelectCategory;
            BgColorCategory.OnDeselectAll -= OnDeselectAllInCategory;
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
                case UIState.BgColorSelection:
                    for (var i = 0; i < avatarCat.UIButtons.Length; i++) {
                        avatarCat.UIButtons[i].gameObject.SetActive(i == avatarCat.SelectedIndex);
                        if (i == avatarCat.SelectedIndex) {
                            avatarCat.UIButtons[i].transform.localScale = Vector3.one * 1.65f;
                            _avatars[currAvatarId].SetHairColor(currHairColor);
                        }
                    }
                    foreach (var cat in Categories) {
                        if (cat != avatarCat) {
                            cat.gameObject.SetActive(false);
                        }
                    }
                    BgColorCategory.gameObject.SetActive(true);
                    BtContinue.StopPulsing();
                    BtContinue.gameObject.SetActive(false);
                    KeeperManager.I.PlayDialogue(Database.LocalizationDataId.Profile_Color);
                    break;
                case UIState.AvatarCreation:
                    BgColorCategory.gameObject.SetActive(false);
                    foreach (UIButton catBt in avatarCat.UIButtons) {
                        catBt.gameObject.SetActive(true);
                    }
                    foreach (PlayerCreationUICategory cat in Categories) {
                        if (cat != avatarCat) {
                            cat.gameObject.SetActive(true);
                        }
                    }
                    _avatars[currAvatarId].SetHairColor(Color.white);
                    OnSelectCategory(avatarCat, avatarCat.UIButtons[avatarCat.SelectedIndex]);
                    break;
            }
        }

        void playAudioDescription(int SelectedIndex)
        {
            //Debug.Log("SelectedIndex: " + SelectedIndex);
            switch (SelectedIndex) {
                case 0:
                    KeeperManager.I.PlayDialogue(Database.LocalizationDataId.Profile_SkinColor);
                break;
                case 1:
                    KeeperManager.I.PlayDialogue(Database.LocalizationDataId.Profile_Avatar);
                break;
                case 2:
                    KeeperManager.I.PlayDialogue(Database.LocalizationDataId.Profile_HairColor); 
                break;
            }
        }

        void FillHairAvatars()
        {
            if (currAvatarId == -1) return;
            PlayerCreationAvatar currAvatar = _avatars[currAvatarId];
            foreach (PlayerCreationAvatar avatar in _hairAvatars) {
                avatar.SetFace(currAvatar.faceImg.sprite, currSkinColor);
                avatar.SetHair(currAvatar.hairImg.sprite);
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
//                if (i == CategoryIndex.Color) {
//                    // Reset avatars colors
//                    Categories[CategoryIndex.Avatar].ResetColor();
//                }
                cat.Select(-1);
                cat.gameObject.SetActive(false);
            }
            selectionStep = toStep;
            stepTween = CategoriesContainer.DOAnchorPosY(StartupOffsetY - selectionStepOffsetY * selectionStep, 0.4f);
            playAudioDescription(selectionStep);
        }

//        void AvatarCreation_SetGender()
//        {
//            Categories[CategoryIndex.Avatar].AvatarSetIcon(Categories[CategoryIndex.Gender].SelectedIndex == 1);
//            if (AppManager.I.Player != null) {
//                AppManager.I.Player.Gender = Categories[CategoryIndex.Gender].SelectedIndex == 0 ? PlayerGender.M : PlayerGender.F;
//            } else {
//                AppManager.I.PlayerProfileManager.TemporaryPlayerGender = Categories[CategoryIndex.Gender].SelectedIndex == 0 ? PlayerGender.M : PlayerGender.F;
//            }
//            //    Debug.Log("AvatarCreation_SetGender " + AppManager.I.PlayerProfileManager.TemporaryPlayerGender);
//        }

        void CreateProfile()
        {
            PlayerCreationScene.CreatePlayer(currAvatarId, currSkinColor, currHairColor, currBgColor);
        }

        PlayerCreationUICategory GetUICategoryObj(CategoryType type)
        {
            if (type == CategoryType.BgColor) return BgColorCategory;
            for (int i = 0; i < Categories.Length; ++i) {
                if (Categories[i].CategoryType == type) return Categories[i];
            }
            return null;
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
                        case CategoryIndex.SkinColor:
                            currSkinColor = uiButton.Bt.image.color;
                            foreach (PlayerCreationAvatar avatar in _avatars) {
                                avatar.SetFaceColor(currSkinColor);
                            }
                            FillHairAvatars();
                            break;
                        case CategoryIndex.Avatar:
                            currAvatarId = Array.IndexOf(_avatarBts, uiButton);
                            FillHairAvatars();
                            break;
                        case CategoryIndex.HairColor:
                            currHairColor = uiButton.GetComponent<PlayerCreationAvatar>().hairImg.color;
                            break;
                    }

                    if (allAvatarCategoriesSelected) {
                        BtContinue.gameObject.SetActive(true);
                        BtContinue.Pulse();
                        KeeperManager.I.PlayDialogue(Database.LocalizationDataId.Action_PressPlay);
                    }
                    break;
                case UIState.BgColorSelection:
                    switch (category.CategoryType) {
                        case CategoryType.BgColor:
                            BtContinue.gameObject.SetActive(true);
                            BtContinue.Pulse();
                            currBgColor = uiButton.Bt.image.color;
                            _avatars[currAvatarId].GetComponent<UIButton>().Bt.image.color = currBgColor;
                            KeeperManager.I.PlayDialogue(Database.LocalizationDataId.Action_PressPlay);
                            break;
                        case CategoryType.Avatar:
                            BgColorCategory.Select(-1);
                            _avatars[currAvatarId].GetComponent<UIButton>().Bt.image.color = Color.white;
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
                    }
//                    else if (catIndex == CategoryIndex.Color) {
//                        Categories[CategoryIndex.Avatar].ResetColor();
//                    }
                    break;
            }
        }

        void OnContinue()
        {
            switch (State) {
                case UIState.AvatarCreation:
                    SwitchState(UIState.BgColorSelection);
                    break;
                case UIState.BgColorSelection:
                    CreateProfile();
                    break;
            }
        }

        #endregion
    }
}