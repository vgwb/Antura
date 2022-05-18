using Antura.Core;
using Antura.Keeper;
using Antura.Scenes;
using Antura.UI;
using DG.DeExtensions;
using DG.Tweening;
using System;
using System.Collections;
using Antura.Database;
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
            GenderSelection,
            AvatarCreation,
            BgColorSelection,
            AgeSelection
        }

        public enum CategoryType
        {
            SkinColor,
            Avatar,
            HairColor,
            BgColor,
            Gender,
            Age
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
        public PlayerCreationUICategory GenderCategory;
        public PlayerCreationUICategory AgeCategory;

        #endregion

        private bool allAvatarCategoriesSelected
        {
            get
            {
                foreach (var cat in Categories)
                {
                    if (cat.SelectedIndex < 0)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public static UIState State { get; private set; }
        private int selectionStep = 0; // 0: skinColor // 1: avatar // 2: hairColor // 3: bgColor
        private float selectionStepOffsetY;
        private UIButton[] _avatarBts, _hairColorBts;
        private PlayerCreationAvatar[] _avatars;
        private PlayerCreationAvatar[] _hairAvatars;
        private Color[] skinColors, hairColors, bgColors;
        private Color currSkinColor = Color.black, currHairColor = Color.black, currBgColor = Color.black;
        private int currAvatarId = -1;
        private int currAge = -1;
        private PlayerGender currGender = PlayerGender.Undefined;
        private Tween stepTween;

        public ConfirmationPanelUI confirmationPanel;

        #region Unity

        void Awake()
        {
            if (AppManager.I.AppEdition.RequireGender)
            {
                State = UIState.GenderSelection;
            }
            else
            {
                State = UIState.AvatarCreation;
            }
        }

        IEnumerator Start()
        {
            AppManager.I.Player = null; // Remove current player

            _avatars = GetUICategoryObj(CategoryType.Avatar).GetComponentsInChildren<PlayerCreationAvatar>(true);
            _avatarBts = GetUICategoryObj(CategoryType.Avatar).GetComponentsInChildren<UIButton>(true);
            _hairColorBts = GetUICategoryObj(CategoryType.HairColor).GetComponentsInChildren<UIButton>(true);
            hairColors = new Color[_hairColorBts.Length];
            for (int i = 0; i < _hairColorBts.Length; i++)
                hairColors[i] = _hairColorBts[i].Bt.image.color;
            UIButton[] bts = GetUICategoryObj(CategoryType.SkinColor).GetComponentsInChildren<UIButton>(true);
            skinColors = new Color[bts.Length];
            for (int i = 0; i < bts.Length; i++)
            {
                skinColors[i] = bts[i].Bt.image.color;
                bts[i].BtToggleOffColor = bts[i].Bt.image.color;
            }
            bts = GetUICategoryObj(CategoryType.BgColor).GetComponentsInChildren<UIButton>(true);
            bgColors = new Color[bts.Length];
            for (int i = 0; i < bts.Length; i++)
            {
                bgColors[i] = bts[i].Bt.image.color;
                bts[i].BtToggleOffColor = bts[i].Bt.image.color;
            }

            // Create hair avatars
            _hairAvatars = new PlayerCreationAvatar[_hairColorBts.Length];
            for (int i = 0; i < _hairColorBts.Length; ++i)
            {
                UIButton bt = _hairColorBts[i];
                PlayerCreationAvatar avatar = Instantiate(_avatars[0], bt.transform.parent, false);
                avatar.SetHairColor(bt.Bt.image.color);
                Destroy(bt.gameObject);
                _hairColorBts[i] = avatar.GetComponent<UIButton>();
                _hairAvatars[i] = avatar;
            }

            selectionStepOffsetY = StartupOffsetY / (Categories.Length - 1f);
            CategoriesContainer.SetAnchoredPosY(StartupOffsetY);
            for (var i = 0; i < Categories.Length; ++i)
            {
                //Categories[i].gameObject.SetActive(i == 0);
                Categories[i].gameObject.SetActive(false);
            }
            BtContinue.gameObject.SetActive(false);
            BgColorCategory.gameObject.SetActive(false);
            AgeCategory.gameObject.SetActive(false);
            GenderCategory.gameObject.SetActive(true);

            // Initialize categories (wait)
            yield return null;
            foreach (PlayerCreationUICategory cat in Categories)
                cat.Init();
            BgColorCategory.Init();
            AgeCategory.Init();
            GenderCategory.Init();

            // Listeners
            BtContinue.Bt.onClick.AddListener(OnContinue);
            foreach (PlayerCreationUICategory cat in Categories)
            {
                cat.OnSelect += OnSelectCategory;
                cat.OnDeselectAll += OnDeselectAllInCategory;
            }
            BgColorCategory.OnSelect += OnSelectCategory;
            BgColorCategory.OnDeselectAll += OnDeselectAllInCategory;
            GenderCategory.OnSelect += OnSelectCategory;
            GenderCategory.OnDeselectAll += OnDeselectAllInCategory;
            AgeCategory.OnSelect += OnSelectCategory;
            AgeCategory.OnDeselectAll += OnDeselectAllInCategory;

            SwitchState(State, true);
            playAudioDescription(State, 0);
        }

        void OnDestroy()
        {
            BtContinue.Bt.onClick.RemoveAllListeners();
            foreach (PlayerCreationUICategory cat in Categories)
            {
                cat.OnSelect -= OnSelectCategory;
                cat.OnDeselectAll -= OnDeselectAllInCategory;
            }
            BgColorCategory.OnSelect -= OnSelectCategory;
            BgColorCategory.OnDeselectAll -= OnDeselectAllInCategory;
            GenderCategory.OnSelect -= OnSelectCategory;
            GenderCategory.OnDeselectAll -= OnDeselectAllInCategory;
            AgeCategory.OnSelect -= OnSelectCategory;
            AgeCategory.OnDeselectAll -= OnDeselectAllInCategory;
            stepTween.Kill();
        }

        #endregion

        #region Methods

        private void SwitchState(UIState toState, bool init = false)
        {
            if (State == toState && !init)
                return;

            State = toState;
            PlayerCreationUICategory avatarCat = Categories[CategoryIndex.Avatar];
            switch (toState)
            {
                case UIState.GenderSelection:
                    confirmationPanel.onSkip = () =>
                    {
                        SelectGender(PlayerGender.Undefined);
                        OnContinue();
                    };
                    confirmationPanel.Show(LocalizationDataId.Help_GenderSelection);

                    foreach (var cat in Categories)
                        cat.gameObject.SetActive(false);
                    BgColorCategory.gameObject.SetActive(false);
                    GenderCategory.gameObject.SetActive(true);
                    AgeCategory.gameObject.SetActive(false);
                    BtContinue.StopPulsing();
                    BtContinue.gameObject.SetActive(false);
                    if (!init)
                        KeeperManager.I.PlayDialogue(Database.LocalizationDataId.Profile_Gender);
                    break;

                case UIState.AvatarCreation:

                    BtContinue.StopPulsing();
                    BtContinue.gameObject.SetActive(false);

                    KeeperManager.I.PlayDialogue(Database.LocalizationDataId.Profile_SkinColor);

                    BgColorCategory.gameObject.SetActive(false);
                    GenderCategory.gameObject.SetActive(false);
                    AgeCategory.gameObject.SetActive(false);
                    foreach (UIButton catBt in avatarCat.UIButtons)
                    {
                        catBt.gameObject.SetActive(true);
                    }

                    if (!init && currAvatarId != -1)
                    {
                        foreach (PlayerCreationUICategory cat in Categories)
                        {
                            if (cat != avatarCat)
                            {
                                cat.gameObject.SetActive(true);
                            }
                        }

                        _avatars[currAvatarId].SetHairColor(Color.white);
                        OnSelectCategory(avatarCat, avatarCat.UIButtons[avatarCat.SelectedIndex]);
                    }
                    else
                    {
                        foreach (PlayerCreationUICategory cat in Categories)
                        {
                            cat.gameObject.SetActive(false);
                        }
                        Categories[0].gameObject.SetActive(true);
                    }

                    break;

                case UIState.BgColorSelection:
                    for (var i = 0; i < avatarCat.UIButtons.Length; i++)
                    {
                        avatarCat.UIButtons[i].gameObject.SetActive(i == avatarCat.SelectedIndex);
                        if (i == avatarCat.SelectedIndex)
                        {
                            avatarCat.UIButtons[i].transform.localScale = Vector3.one * 1.65f;
                            _avatars[currAvatarId].SetHairColor(currHairColor);
                        }
                    }
                    foreach (var cat in Categories)
                    {
                        if (cat != avatarCat)
                            cat.gameObject.SetActive(false);
                    }
                    BgColorCategory.gameObject.SetActive(true);
                    GenderCategory.gameObject.SetActive(false);
                    AgeCategory.gameObject.SetActive(false);
                    BtContinue.StopPulsing();
                    BtContinue.gameObject.SetActive(false);
                    KeeperManager.I.PlayDialogue(Database.LocalizationDataId.Profile_Color);
                    break;

                case UIState.AgeSelection:
                    foreach (var cat in Categories)
                    {
                        if (cat != avatarCat)
                            cat.gameObject.SetActive(false);
                    }
                    BgColorCategory.gameObject.SetActive(false);
                    GenderCategory.gameObject.SetActive(false);
                    AgeCategory.gameObject.SetActive(true);
                    BtContinue.StopPulsing();
                    BtContinue.gameObject.SetActive(false);
                    KeeperManager.I.PlayDialogue(Database.LocalizationDataId.Profile_Age);
                    break;

            }
        }

        private void playAudioDescription(UIState state, int categoryIndex)
        {
            //Debug.Log("SelectedIndex: " + SelectedIndex);
            switch (state)
            {
                case UIState.GenderSelection:
                    KeeperManager.I.PlayDialogue(Database.LocalizationDataId.Profile_Gender);
                    break;
                case UIState.AgeSelection:
                    KeeperManager.I.PlayDialogue(Database.LocalizationDataId.Profile_Age);
                    break;
                case UIState.BgColorSelection:
                    KeeperManager.I.PlayDialogue(Database.LocalizationDataId.Profile_Color);
                    break;
                case UIState.AvatarCreation:
                    switch (categoryIndex)
                    {
                        case CategoryIndex.SkinColor:
                            KeeperManager.I.PlayDialogue(Database.LocalizationDataId.Profile_SkinColor);
                            break;
                        case CategoryIndex.Avatar:
                            KeeperManager.I.PlayDialogue(Database.LocalizationDataId.Profile_Avatar);
                            break;
                        case CategoryIndex.HairColor:
                            KeeperManager.I.PlayDialogue(Database.LocalizationDataId.Profile_HairColor);
                            break;
                    }
                    break;
            }
        }

        private void FillHairAvatars()
        {
            if (currAvatarId == -1)
                return;
            PlayerCreationAvatar currAvatar = _avatars[currAvatarId];
            foreach (PlayerCreationAvatar avatar in _hairAvatars)
            {
                avatar.SetFace(currAvatar.faceImg.sprite, currSkinColor);
                avatar.SetHair(currAvatar.hairImg.sprite);
            }
        }

        private void AvatarCreation_NextStep()
        {
            selectionStep++;
            if (stepTween != null)
                stepTween.Complete();
            Categories[selectionStep].gameObject.SetActive(true);
            stepTween = CategoriesContainer.DOAnchorPosY(StartupOffsetY - selectionStepOffsetY * selectionStep, 0.4f);

            playAudioDescription(State, selectionStep);
        }

        private void AvatarCreation_StepBackwards(int toStep)
        {
            if (stepTween != null)
            { stepTween.Complete(); }
            for (var i = toStep + 1; i < selectionStep + 1; ++i)
            {
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
            playAudioDescription(State, selectionStep);
        }

        private void CreateProfile()
        {
            PlayerCreationScene.CreatePlayer(currAvatarId, currGender, currSkinColor, currHairColor, currBgColor, currAge);
        }

        private PlayerCreationUICategory GetUICategoryObj(CategoryType type)
        {
            if (type == CategoryType.Age)
                return AgeCategory;
            if (type == CategoryType.Gender)
                return GenderCategory;
            if (type == CategoryType.BgColor)
                return BgColorCategory;
            for (int i = 0; i < Categories.Length; ++i)
            {
                if (Categories[i].CategoryType == type)
                    return Categories[i];
            }
            return null;
        }

        #endregion

        #region Callbacks

        private void SelectGender(PlayerGender _gender)
        {
            currGender = _gender;
            AppManager.I.PlayerProfileManager.TemporaryPlayerGender = currGender;
        }

        private void OnSelectCategory(PlayerCreationUICategory category, UIButton uiButton)
        {
            switch (State)
            {
                case UIState.GenderSelection:
                    BtContinue.gameObject.SetActive(true);
                    BtContinue.Pulse();
                    confirmationPanel.Hide();
                    SelectGender(Array.IndexOf(GenderCategory.UIButtons, uiButton) == 0
                        ? PlayerGender.M
                        : PlayerGender.F);
                    KeeperManager.I.PlayDialogue(Database.LocalizationDataId.Action_PressPlay);
                    break;
                case UIState.AvatarCreation:
                    int catIndex = Array.IndexOf(Categories, category);
                    if (selectionStep < Categories.Length - 1 && catIndex == selectionStep)
                    {
                        AvatarCreation_NextStep();
                    }

                    switch (catIndex)
                    {
                        case CategoryIndex.SkinColor:
                            currSkinColor = uiButton.Bt.image.color;
                            foreach (PlayerCreationAvatar avatar in _avatars)
                            {
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

                    if (allAvatarCategoriesSelected)
                    {
                        BtContinue.gameObject.SetActive(true);
                        BtContinue.Pulse();
                        KeeperManager.I.PlayDialogue(Database.LocalizationDataId.Action_PressPlay);
                    }
                    break;
                case UIState.BgColorSelection:
                    switch (category.CategoryType)
                    {
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
                case UIState.AgeSelection:
                    switch (category.CategoryType)
                    {
                        case CategoryType.Age:
                            BtContinue.gameObject.SetActive(true);
                            BtContinue.Pulse();
                            KeeperManager.I.PlayDialogue(Database.LocalizationDataId.Action_PressPlay);
                            currAge = 4 + Array.IndexOf(AgeCategory.UIButtons, uiButton);
                            break;
                        case CategoryType.Avatar:
                            AgeCategory.Select(-1);
                            _avatars[currAvatarId].GetComponent<UIButton>().Bt.image.color = Color.white;
                            SwitchState(UIState.AvatarCreation);
                            break;
                    }
                    break;
            }
        }

        private void OnDeselectAllInCategory(PlayerCreationUICategory category)
        {
            BtContinue.StopPulsing();
            BtContinue.gameObject.SetActive(false);
            switch (State)
            {
                case UIState.AvatarCreation:
                    var catIndex = Array.IndexOf(Categories, category);
                    if (catIndex < selectionStep)
                    {
                        AvatarCreation_StepBackwards(catIndex);
                    }
                    //                    else if (catIndex == CategoryIndex.Color) {
                    //                        Categories[CategoryIndex.Avatar].ResetColor();
                    //                    }
                    break;
            }
        }

        private void OnContinue()
        {
            switch (State)
            {
                case UIState.GenderSelection:
                    SwitchState(UIState.AvatarCreation);
                    break;
                case UIState.AvatarCreation:
                    SwitchState(UIState.BgColorSelection);
                    break;
                case UIState.BgColorSelection:
                    if (AppManager.I.AppEdition.RequireAge)
                        SwitchState(UIState.AgeSelection);
                    else
                        CreateProfile();
                    break;
                case UIState.AgeSelection:
                    CreateProfile();
                    break;
            }
        }

        #endregion
    }
}
