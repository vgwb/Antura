using Antura.Core;
using Antura.Dog;
using Antura.Extensions;
using Antura.Helpers;
using Antura.Rewards;
using Antura.UI;
using DG.DeInspektor.Attributes;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Antura.Audio;
using Antura.Database;
using Antura.Keeper;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.AnturaSpace.UI
{
    /// <summary>
    /// General controller of the UI in the Antura Space scene.
    /// </summary>
    public class AnturaSpaceUI : MonoBehaviour
    {
        private static int MaxItems = 12;
        public LayerMask RewardsLayer;

        [DeToggleButton]
        public bool FlipRewards = true;

        [DeToggleButton]
        public bool HideLockedSwatchesColors;

        [Header("References")]
        public AnturaSpaceModsButton BtOpenModsPanel;
        public Image ToggledModsUI;
        public UIButton BtBonesShop;
        public Image ToggledShopUI;

        public UIButton BTRemoveMods;
        public RectTransform CategoriesContainer, ItemsContainer, SwatchesContainer;
        public AnturaSpaceItemButton BtItemMain;
        public RectTransform ShopPanelContainer;
        public ShopPanelUI ShopPanelUI;
        public TMPro.TextMeshProUGUI bonesNumber;
        public AnturaPetSwitcher petSwitcher => (AnturaSpaceScene.I as AnturaSpaceScene).AnturaMain.PetSwitcher;

        public event Action onEnterCustomization;
        public event Action onExitCustomization;

        public static bool MERGE_EARS = true;
        public static bool MERGE_REMOVE_INTO_PROPS = true;
        public static bool REWARDS_CAN_BE_BOUGHT = true;

        public delegate void AnturaSpaceUIEvent(string _id);

        public static event AnturaSpaceUIEvent onRewardCategorySelectedInCustomization;

        public static event AnturaSpaceUIEvent onRewardSelectedInCustomization;
        //        public static event AnturaSpaceUIEvent onRewardColorSelectedInCustomization;

        public static AnturaSpaceUI I { get; private set; }
        public bool IsModsPanelOpen { get; private set; }
        //public bool IsShopPanelOpen { get; private set; }

        AnturaSpaceCategoryButton[] btsCategories;
        AnturaSpaceItemButton[] btsItems;
        AnturaSpaceSwatchButton[] btsSwatches;
        List<Transform> rewardsContainers;
        List<Transform> rewardsImagesContainers; // Passed with texture and decal reward types
        RewardBaseType _currRewardBaseType;
        RewardBaseItem _currSelectedRewardBaseItem;
        List<RewardBaseItem> currRewardBaseItems;
        List<RewardColorItem> currRewardColorItems;
        AnturaSpaceCategoryButton.AnturaSpaceCategory currCategory;
        Tween showCategoriesTween, showShopTween, showItemsTween, showSwatchesTween;

        #region Unity

        int bonesCount = -1;

        public int BonesCount
        {
            get { return bonesCount; }
            set
            {
                if (value == bonesCount)
                {
                    return;
                }

                bonesCount = value;
                bonesNumber.text = value.ToString();
            }
        }

        void Awake()
        {
            I = this;
        }

        public void Initialise()
        {
            btsCategories = CategoriesContainer.GetComponentsInChildren<AnturaSpaceCategoryButton>(true);
            btsSwatches = SwatchesContainer.GetComponentsInChildren<AnturaSpaceSwatchButton>(true);
            SelectCategory(AnturaSpaceCategoryButton.AnturaSpaceCategory.Unset);
            BtOpenModsPanel.SetAsNew(AppManager.I.RewardSystemManager.IsThereSomeNewReward());


            // Create items
            rewardsContainers = new List<Transform>();
            rewardsImagesContainers = new List<Transform>();
            btsItems = new AnturaSpaceItemButton[MaxItems];
            btsItems[0] = BtItemMain;
            rewardsContainers.Add(BtItemMain.RewardContainer);
            rewardsImagesContainers.Add(BtItemMain.RewardImage.transform);
            for (int i = 1; i < MaxItems; ++i)
            {
                AnturaSpaceItemButton item = Instantiate(BtItemMain);
                item.transform.SetParent(BtItemMain.transform.parent, false);
                item.Setup();
                btsItems[i] = item;
                rewardsContainers.Add(item.RewardContainer);
                rewardsImagesContainers.Add(item.RewardImage.transform);
            }
            BtItemMain.Setup();

            const float duration = 0.3f;
            showCategoriesTween = DOTween.Sequence().SetAutoKill(false).Pause()
                .Append(CategoriesContainer.DOAnchorPosY(150, duration).From().SetEase(Ease.OutBack))
                .Join(BtBonesShop.RectT.DOAnchorPosY(-830, duration))
                .OnRewind(() => CategoriesContainer.gameObject.SetActive(false));
            showShopTween = DOTween.Sequence().SetAutoKill(false).Pause()
                .Append(ShopPanelContainer.DOAnchorPosY(-830, duration).From().SetEase(Ease.OutQuad))
                .Join(BtOpenModsPanel.RectT.DOAnchorPosY(150, duration))
                .OnRewind(() => ShopPanelContainer.gameObject.SetActive(false));
            showItemsTween = ItemsContainer.DOAnchorPosX(-350, duration).From().SetEase(Ease.OutBack).SetAutoKill(false).Pause()
                .OnRewind(() =>
                {
                    ItemsContainer.gameObject.SetActive(false);
                    // Clear items containers children
                    /*foreach (Transform container in rewardsContainers)
                    {
                        foreach (Transform child in container)
                            Destroy(child.gameObject);
                    }*/
                });
            showSwatchesTween = SwatchesContainer.DOAnchorPosY(-100, duration).From().SetEase(Ease.OutBack).SetAutoKill(false).Pause()
                .OnRewind(() => SwatchesContainer.gameObject.SetActive(false));

            CategoriesContainer.gameObject.SetActive(false);
            ShopPanelContainer.gameObject.SetActive(false);
            ItemsContainer.gameObject.SetActive(false);
            SwatchesContainer.gameObject.SetActive(false);

            // Listeners
            BtOpenModsPanel.Bt.onClick.AddListener(() => OnClick(BtOpenModsPanel));
            BtBonesShop.Bt.onClick.AddListener(() => OnClick(BtBonesShop));
            BTRemoveMods.Bt.onClick.AddListener(() => OnClick(BTRemoveMods));
            foreach (var bt in btsCategories)
            {
                var b = bt;
                b.Bt.onClick.AddListener(() => OnClickCategory(b));
            }
            foreach (var bt in btsItems)
            {
                var b = bt;
                b.Bt.onClick.AddListener(() => OnClickItem(b));
            }
            foreach (var bt in btsSwatches)
            {
                var b = bt;
                b.Bt.onClick.AddListener(() => OnClickSwatch(b));
            }

            ShopPanelUI.Initialise();

            ToggledModsUI.gameObject.SetActive(false);
            ToggledShopUI.gameObject.SetActive(false);
        }

        void OnDestroy()
        {
            if (I == this)
                I = null;

            (AnturaSpaceScene.I as AnturaSpaceScene).TriggerSceneExit();

            StopAllCoroutines();
            showCategoriesTween.Kill();
            showItemsTween.Kill();
            showSwatchesTween.Kill();
            BtOpenModsPanel.Bt.onClick.RemoveAllListeners();
            BTRemoveMods.Bt.onClick.RemoveAllListeners();
            foreach (var bt in btsCategories)
            {
                bt.Bt.onClick.RemoveAllListeners();
            }
            foreach (var bt in btsItems)
            {
                bt.Bt.onClick.RemoveAllListeners();
            }
            foreach (var bt in btsSwatches)
            {
                bt.Bt.onClick.RemoveAllListeners();
            }
        }

        #endregion

        #region Public Methods

        private bool alreadyCommentedZeroBones = false;

        public void ToggleShopPanel()
        {
            // Cannot close the shop panel while we are confirming a purchase
            if (ShopDecorationsManager.I.ShopContext != ShopContext.Closed
                && ShopDecorationsManager.I.ShopContext != ShopContext.Purchase)
            {
                AudioManager.I.PlaySound(Sfx.KO);
                return;
            }

            // Cannot interact with the shop panel while we do the tutorial
            var scene = AnturaSpaceScene.I as AnturaSpaceScene;
            if (scene.TutorialMode
                && scene.tutorialManager.CurrentTutorialFocus != BtBonesShop.Bt
            )
            {
                return;
            }

            // If we have no bones, we mention that
            if (AppManager.I.Player.TotalNumberOfBones == 0 && !alreadyCommentedZeroBones)
            {
                alreadyCommentedZeroBones = true;
                KeeperManager.I.PlayDialogue(LocalizationDataId.AnturaSpace_Tuto_Cookie_3);
            }

            if (ShopDecorationsManager.I.ShopContext == ShopContext.Closed)
            {
                ShopPanelContainer.gameObject.SetActive(true);
                ShopDecorationsManager.I.SetContextPurchase();
                showShopTween.PlayForward();
            }
            else if (ShopDecorationsManager.I.ShopContext == ShopContext.Purchase)
            {
                ShopDecorationsManager.I.SetContextClosed();
                showShopTween.PlayBackwards();
            }

        }

        public void ToggleModsPanel()
        {
            var scene = AnturaSpaceScene.I as AnturaSpaceScene;
            if (scene.TutorialMode && scene.tutorialManager.CurrentTutorialFocus != BtOpenModsPanel.Bt)
                return;

            IsModsPanelOpen = !IsModsPanelOpen;
            if (IsModsPanelOpen)
            {
                BtOpenModsPanel.SetAsNew(false);
                CategoriesContainer.gameObject.SetActive(true);
                showCategoriesTween.PlayForward();
                ToggledModsUI.gameObject.SetActive(true);
                RefreshCategories();

                ShopDecorationsManager.I.SetContextCustomization();

                if (onEnterCustomization != null)
                {
                    onEnterCustomization();
                }
            }
            else
            {
                BtOpenModsPanel.SetAsNew(AppManager.I.RewardSystemManager.IsThereSomeNewReward());
                SelectCategory(AnturaSpaceCategoryButton.AnturaSpaceCategory.Unset);
                showCategoriesTween.PlayBackwards();
                ToggledModsUI.gameObject.SetActive(false);
                showItemsTween.PlayBackwards();
                showSwatchesTween.PlayBackwards();

                ShopDecorationsManager.I.SetContextClosed();

                if (onExitCustomization != null)
                {
                    onExitCustomization();
                }
            }
        }

        /// <summary>
        /// Returns the first category button marked as NEW (meaning it has new content).
        /// Return NULL if the mods panel is not open.
        /// </summary>
        public AnturaSpaceCategoryButton GetNewCategoryButton()
        {
            if (!IsModsPanelOpen)
            {
                Debug.LogWarning("AnturaSpaceUI.GetNewCategoryButton > Mods Panel is not open");
                return null;
            }
            foreach (AnturaSpaceCategoryButton bt in btsCategories)
            {
                if (bt.IsNew)
                {
                    return bt;
                }
            }
            return null;
        }

        /// <summary>
        /// Returns the first item button marked as NEW (meaning it has new content).
        /// Return NULL if the mods panel is not open or a category is not selected.
        /// </summary>
        public AnturaSpaceItemButton GetNewItemButton()
        {
            if (!IsModsPanelOpen || !ItemsContainer.gameObject.activeSelf)
            {
                Debug.LogWarning("AnturaSpaceUI.GetNewItemButton > Mods Panel is not open or category is not selected");
                return null;
            }
            foreach (var bt in btsItems)
            {
                if (bt.IsNew)
                {
                    return bt;
                }
            }
            return null;
        }

        public AnturaSpaceCategoryButton GetSpecificCategory(AnturaSpaceCategoryButton.AnturaSpaceCategory category)
        {
            if (!IsModsPanelOpen)
            {
                Debug.LogWarning("AnturaSpaceUI.GetNewCategoryButton > Mods Panel is not open");
                return null;
            }
            foreach (AnturaSpaceCategoryButton bt in btsCategories)
            {
                if (bt.Category == category)
                {
                    return bt;
                }
            }
            return null;
        }

        /// <summary>
        /// Returns a random category button.
        /// Return NULL if the mods panel is not open.
        /// </summary>
        public AnturaSpaceCategoryButton GetFirstUnlockedCategoryButton()
        {
            if (!IsModsPanelOpen)
            {
                Debug.LogWarning("AnturaSpaceUI.GetNewCategoryButton > Mods Panel is not open");
                return null;
            }
            foreach (AnturaSpaceCategoryButton bt in btsCategories)
            {
                if (bt.Unlocked)
                {
                    return bt;
                }
            }
            return null;
        }

        /// <summary>
        /// Returns a specific item button with the provided Shared ID
        /// Return NULL if the mods panel is not open or a category is not selected.
        /// </summary>
        public AnturaSpaceItemButton GetSpecificItemButton(string sharedID)
        {
            if (!IsModsPanelOpen || !ItemsContainer.gameObject.activeSelf)
            {
                Debug.LogWarning("AnturaSpaceUI.GetRandomItemButton > Mods Panel is not open or category is not selected");
                return null;
            }
            foreach (var bt in btsItems)
            {
                if (bt.Data != null && Equals(bt.Data.data.SharedID, sharedID))
                {
                    return bt;
                }
            }
            return null;
        }

        /// <summary>
        /// Returns a random item button.
        /// Return NULL if the mods panel is not open or a category is not selected.
        /// </summary>
        public AnturaSpaceItemButton GetFirstUnlockedItemButton()
        {
            if (!IsModsPanelOpen || !ItemsContainer.gameObject.activeSelf)
            {
                Debug.LogWarning("AnturaSpaceUI.GetRandomItemButton > Mods Panel is not open or category is not selected");
                return null;
            }
            foreach (var bt in btsItems)
            {
                if (!bt.IsItemLocked)
                {
                    return bt;
                }
            }
            return null;
        }

        public AnturaSpaceSwatchButton GetRandomUnselectedSwatch()
        {
            return btsSwatches.Where(x => x.Data != null).ToList().Where(x => !x.Data.IsSelected).ToList().RandomSelectOne();
        }

        public AnturaSpaceSwatchButton[] GetAllSwatches()
        {
            return btsSwatches;
        }

        #endregion

        #region Methods

        public void ShowShopButton(bool show)
        {
            BtBonesShop.gameObject.SetActive(show);
        }

        private RewardBaseItem prevReward;
        void SelectCategory(AnturaSpaceCategoryButton.AnturaSpaceCategory _category)
        {
            StopAllCoroutines();
            // Save configuration
            //AnturaModelManager.Instance.SaveAnturaCustomization();

            if (_currSelectedRewardBaseItem != null && !_currSelectedRewardBaseItem.IsBought)
            {
                CancelPurchase();
            }
            _currSelectedRewardBaseItem = null;

            // Toggle buttons
            foreach (AnturaSpaceCategoryButton bt in btsCategories)
            {
                if (bt.Category == _category)
                {
                    bt.Toggle(true, true);
                }
                else
                {
                    bt.Toggle(false);
                }
            }
            if (_category == AnturaSpaceCategoryButton.AnturaSpaceCategory.Unset)
            {
                return;
            }

            showItemsTween.Rewind();
            StartCoroutine(CO_SelectCategory(_category));
        }

        IEnumerator CO_SelectCategory(AnturaSpaceCategoryButton.AnturaSpaceCategory _category)
        {
            BTRemoveMods.gameObject.SetActive(false);

            // Get rewards list
            currCategory = _category;
            _currRewardBaseType = CategoryToRewardBaseType(_category);
            bool useImages = _category == AnturaSpaceCategoryButton.AnturaSpaceCategory.Texture ||
                             _category == AnturaSpaceCategoryButton.AnturaSpaceCategory.Decal;

            foreach (var item in btsItems)
            {
                item.SetImage(!useImages);
            }
            ReloadRewardsDatas();
            yield return null;

            RewardBaseItem selectedRewardBaseData = RefreshItems(petType:AppManager.I.Player.PetData.SelectedPet);
            ItemsContainer.gameObject.SetActive(true);
            showItemsTween.PlayForward();

            // Change number of columns based on reward type
            bool useTwoColumns = //(SEPARATE_EARS && _category == AnturaSpaceCategoryButton.AnturaSpaceCategory.Ears) ||
                                 currRewardBaseItems.Count <= 8;
            btsItems[0].GetComponentInParent<GridLayoutGroup>(true).constraintCount = useTwoColumns ? 2 : 3;

            // Select eventual reward
            if (selectedRewardBaseData != null)
            {
                SelectReward(selectedRewardBaseData);
            }
            else
            {
                showSwatchesTween.Rewind();
            }
        }

        void SelectReward(RewardBaseItem rewardBaseData, bool backToPrev = false)
        {
            var scene = AnturaSpaceScene.I as AnturaSpaceScene;
            if (rewardBaseData == null && scene.TutorialMode)
                return;

            if (!backToPrev && _currSelectedRewardBaseItem != null && !_currSelectedRewardBaseItem.IsBought)
            {
                CancelPurchase();
            }

            showSwatchesTween.Rewind();
            bool isTextureOrDecal = currCategory == AnturaSpaceCategoryButton.AnturaSpaceCategory.Texture
                                    || currCategory == AnturaSpaceCategoryButton.AnturaSpaceCategory.Decal;
            BTRemoveMods.gameObject.SetActive(!MERGE_REMOVE_INTO_PROPS && !isTextureOrDecal && rewardBaseData != null);
            if (rewardBaseData == null)
            {
                _currSelectedRewardBaseItem = null;
                prevReward = null;
                ClearRewardsOfCategory(currCategory);
                return;
            }

            _currSelectedRewardBaseItem = rewardBaseData;
            currRewardColorItems = AppManager.I.RewardSystemManager.GetRewardColorItemsForBase(rewardBaseData.data);
            if (currRewardColorItems.Count == 0)
            {
                Debug.Log("No color swatches for the selected reward!");
                return;
            }

            var bought = rewardBaseData.IsBought;
            bool hasEnoughBones =  AppManager.I.Player.GetTotalNumberOfBones() >= rewardBaseData.data.Cost;
            if (!bought)
            {
                if (hasEnoughBones)
                {
                    // @note: we use the Decoration Shop logic to handle the purchase
                    ShopDecorationsManager.I.CurrentDecorationCost = rewardBaseData.data.Cost;

                    ShopPanelUI.HandleCustomizationShopPurchaseConfirmationRequested(ConfirmPurchase, CancelPurchase);

                    // Mount anyway (preview)
                    AppManager.I.RewardSystemManager.PreviewReward(_currSelectedRewardBaseItem.data, currRewardColorItems[0].data);
                }
                else
                {
                    // Cannot select at all
                }
            }
            else
            {
                // This reward is selected
                prevReward = rewardBaseData;

                // Hide non-existent swatches
                for (int i = currRewardColorItems.Count - 1; i < btsSwatches.Length; ++i)
                    btsSwatches[i].gameObject.SetActive(false);
                // Setup and show swatches
                RewardColorItem selectedSwatchData = null;
                for (int i = 0; i < currRewardColorItems.Count; ++i)
                {
                    RewardColorItem swatchData = currRewardColorItems[i];
                    AnturaSpaceSwatchButton swatch = btsSwatches[i];
                    swatch.gameObject.SetActive(true);
                    swatch.Data = swatchData;
                    if (swatchData != null)
                    {
                        swatch.SetAsNew(false); // @note: we force the swatch colors to never be shown as new
                        //swatch.SetAsNew(!swatchData.IsSelected && swatchData.IsNew);
                        swatch.Toggle(swatchData.IsSelected);
                        Color hexColor1 = GenericHelper.HexToColor(swatchData.data.Color1RGB);
                        Color hexColor2 = swatchData.data.Color2RGB == null ? hexColor1 : GenericHelper.HexToColor(swatchData.data.Color2RGB);
                        swatch.SetColors(hexColor1, hexColor2);
                        if (swatchData.IsSelected)
                        {
                            selectedSwatchData = swatchData;
                        }
                    }
                    else
                    {
                        swatch.Toggle(false);
                        swatch.SetColors(GenericHelper.HexToColor("787878FF"), GenericHelper.HexToColor("494949FF"));
                    }
                    swatch.Lock(swatchData == null);
                }

                SwatchesContainer.gameObject.SetActive(true);
                showSwatchesTween.PlayForward();

                // Select eventual color
                if (selectedSwatchData != null)
                {
                    SelectSwatch(selectedSwatchData);
                }

            }

            ReloadRewardsDatas();
            RefreshCategories();
            RefreshItems(true, AppManager.I.Player.PetData.SelectedPet);
        }

        private void ClearRewardsOfCategory(AnturaSpaceCategoryButton.AnturaSpaceCategory category)
        {
            for (int index = 1; index < btsItems.Length; index++)
            {
                AnturaSpaceItemButton item = btsItems[index];
                item.Toggle(false);
            }
            btsItems[0].Toggle(true);

            if (MERGE_EARS && category == AnturaSpaceCategoryButton.AnturaSpaceCategory.Ears)
            {
                petSwitcher.ModelManager.ClearLoadedRewardInCategory("EAR_L");
                petSwitcher.ModelManager.ClearLoadedRewardInCategory("EAR_R");
            }
            else if (category == AnturaSpaceCategoryButton.AnturaSpaceCategory.Decal)
            {
                SelectReward(currRewardBaseItems[0], backToPrev:true);
            }
            else if (category == AnturaSpaceCategoryButton.AnturaSpaceCategory.Texture)
            {
                SelectReward(currRewardBaseItems[0], backToPrev:true);
            }
            else
            {
                petSwitcher.ModelManager.ClearLoadedRewardInCategory(category.ToString());
            }
        }

        #region New Shop

        public void ConfirmPurchase()
        {
            var dialogues = new[]
            {
                LocalizationDataId.Keeper_Good_1,
                LocalizationDataId.Keeper_Good_2,
                LocalizationDataId.Keeper_Good_3,
                LocalizationDataId.Keeper_Good_4,
                LocalizationDataId.Keeper_Good_5,
                LocalizationDataId.Keeper_Good_6,
                LocalizationDataId.Keeper_Good_7,
                LocalizationDataId.Keeper_Good_8,
                LocalizationDataId.Keeper_Good_9,
                LocalizationDataId.Keeper_Good_10,
                LocalizationDataId.Keeper_Good_11,
                LocalizationDataId.Keeper_Good_12,
            };
            AudioManager.I.PlayDialogue(dialogues.GetRandom().ToString());
            var bonesCost = _currSelectedRewardBaseItem.data.Cost;
            AppManager.I.Services.Analytics.TrackItemBought(bonesCost, _currSelectedRewardBaseItem.data.SharedID);
            AppManager.I.Player.RemoveBones(bonesCost);
            AppManager.I.Player.CustomizationShopState.ConfirmPurchase(_currSelectedRewardBaseItem.data.SharedID);
            AppManager.I.Player.Save();
            ShopPanelUI.showConfirmationPanelTween.PlayBackwards();
            SelectReward(_currSelectedRewardBaseItem); // Refresh so swatches can be seen
        }


        public void CancelPurchase()
        {
            ShopPanelUI.showConfirmationPanelTween.PlayBackwards();
            ClearRewardsOfCategory(currCategory);

            if (prevReward != null && _currSelectedRewardBaseItem != prevReward)
            {
                SelectReward(prevReward, backToPrev:true);
            }

            _currSelectedRewardBaseItem = prevReward;
        }

        #endregion

        void SelectSwatch(RewardColorItem _colorData)
        {
            foreach (var item in btsSwatches)
            {
                item.Toggle(item.Data == _colorData);
            }
            if (_colorData != null)
            {
                AppManager.I.RewardSystemManager.SelectRewardColorItem(_currSelectedRewardBaseItem.data, _colorData.data);
            }
            else
            {
                Debug.Log("SelectSwatch > _colorData is NULL!");
            }
        }

        void ReloadRewardsDatas()
        {
            bool useImages = currCategory == AnturaSpaceCategoryButton.AnturaSpaceCategory.Texture ||
                             currCategory == AnturaSpaceCategoryButton.AnturaSpaceCategory.Decal;
            if (MERGE_EARS && currCategory == AnturaSpaceCategoryButton.AnturaSpaceCategory.Ears)
            {
                currRewardBaseItems = AppManager.I.RewardSystemManager.GetRewardBaseItems(_currRewardBaseType, rewardsContainers, "EAR_L");
                List<Transform> altRewardContainers = new List<Transform>(rewardsContainers);
                altRewardContainers.RemoveRange(0, currRewardBaseItems.Count);
                currRewardBaseItems.AddRange(AppManager.I.RewardSystemManager.GetRewardBaseItems(_currRewardBaseType, altRewardContainers, "EAR_R"));
            }
            else
            {
                currRewardBaseItems = AppManager.I.RewardSystemManager.GetRewardBaseItems(_currRewardBaseType,
                    useImages ? rewardsImagesContainers : rewardsContainers, currCategory.ToString());
            }
        }

        void RefreshCategories()
        {
            var rewardSystemManager = AppManager.I.RewardSystemManager;
            foreach (AnturaSpaceCategoryButton btCat in btsCategories)
            {
                var rewardBaseType = CategoryToRewardBaseType(btCat.Category);
                var categoryStrings = new List<string>();
                switch (btCat.Category)
                {
                    case AnturaSpaceCategoryButton.AnturaSpaceCategory.Ears when MERGE_EARS:
                        categoryStrings.Add("EAR_L");
                        categoryStrings.Add("EAR_R");
                        break;
                    case AnturaSpaceCategoryButton.AnturaSpaceCategory.Decal:
                    case AnturaSpaceCategoryButton.AnturaSpaceCategory.Texture:
                        categoryStrings.Add("");
                        break;
                    default:
                        categoryStrings.Add(btCat.Category.ToString());
                        break;
                }

                bool isNew = false;
                bool isUnlocked = false;
                foreach (var categoryString in categoryStrings)
                {
                    isNew = isNew || rewardSystemManager.DoesRewardCategoryContainNewElements(rewardBaseType, categoryString);
                    isUnlocked = isUnlocked || rewardSystemManager.DoesRewardCategoryContainUnlockedElements(rewardBaseType, categoryString);
                }
                btCat.SetAsNew(isNew);
                btCat.Unlocked = isUnlocked;
            }
        }

        // Returns eventual selected reward item
        RewardBaseItem RefreshItems(bool toggleOnly = false, AnturaPetType petType = AnturaPetType.Dog)
        {
            bool useImages = currCategory == AnturaSpaceCategoryButton.AnturaSpaceCategory.Texture ||
                             currCategory == AnturaSpaceCategoryButton.AnturaSpaceCategory.Decal;

            int from = 0;
            if (MERGE_REMOVE_INTO_PROPS && !useImages) from = 1;

            // Hide non-existent items
            for (int i = Mathf.Max(0, currRewardBaseItems.Count - 1 + from); i < btsItems.Length; ++i)
            {
                btsItems[i].gameObject.SetActive(false);
            }

            if (MERGE_REMOVE_INTO_PROPS && !useImages)
            {
                // First item will be the "remove" item instead
                var item = btsItems[0];
                item.gameObject.SetActive(true);
                item.Lock(false);
                item.Data = null;
                item.IcoRemove.SetActive(true);
                item.amountUI.transform.parent.gameObject.SetActive(false);
                item.Toggle(!AppManager.I.Player.CurrentSingleAnturaCustomization.HasSomethingEquipped(currCategory));
            }

            // Setup and show items
            RewardBaseItem selectedRewardBaseData = null;
            for (int i = from; i < currRewardBaseItems.Count + from; ++i)
            {
                RewardBaseItem rewardBaseItem = currRewardBaseItems[i-from];
                AnturaSpaceItemButton item = btsItems[i];

                if (rewardBaseItem == null || rewardBaseItem.data == null)
                {
                    // Unknown item
                    item.gameObject.SetActive(true);
                    item.Toggle(false);
                    item.Lock(false);
                    item.Data = null;
                    item.IcoRemove.SetActive(true);
                    item.amountUI.transform.parent.gameObject.SetActive(false);
                    continue;
                }
                item.IcoRemove.SetActive(false);
                item.gameObject.SetActive(true);
                item.Data = rewardBaseItem;
                if (!useImages && !toggleOnly)
                {
                    item.RewardContainer.gameObject.SetLayerRecursive(GenericHelper.LayerMaskToIndex(RewardsLayer));
                    if (item.RewardContainer.childCount > 0) CameraHelper.FitRewardToUICamera(item.RewardContainer.GetChild(0), item.RewardCamera, FlipRewards, petType);
                }
                item.SetAsNew(rewardBaseItem.IsNew);
                item.Toggle(rewardBaseItem.IsSelected);
                if (rewardBaseItem.IsSelected)
                {
                    selectedRewardBaseData = rewardBaseItem;
                }

                bool hasEnoughBones = true;
                if (rewardBaseItem != null && !rewardBaseItem.IsBought)
                {
                    item.amountUI.transform.parent.gameObject.SetActive(true);
                    item.amountUI.text = rewardBaseItem.data.Cost.ToString();

                    hasEnoughBones = AppManager.I.Player.GetTotalNumberOfBones() >= rewardBaseItem.data.Cost;
                    item.BtImg.color = hasEnoughBones ? Color.white : Color.gray;
                }
                else
                {
                    item.amountUI.transform.parent.gameObject.SetActive(false);
                }

                item.Lock(!hasEnoughBones);
            }
            return selectedRewardBaseData;
        }

        #endregion

        #region Helpers

        RewardBaseType CategoryToRewardBaseType(AnturaSpaceCategoryButton.AnturaSpaceCategory _category)
        {
            switch (_category)
            {
                case AnturaSpaceCategoryButton.AnturaSpaceCategory.Texture:
                    return RewardBaseType.Texture;
                case AnturaSpaceCategoryButton.AnturaSpaceCategory.Decal:
                    return RewardBaseType.Decal;
                default:
                    return RewardBaseType.Prop;
            }
        }

        #endregion

        #region Callbacks

        void OnClick(UIButton _bt)
        {
            if (_bt == BtOpenModsPanel)
            {
                ToggleModsPanel();
            }
            else if (_bt == BTRemoveMods)
            {
                SelectReward(null);
            }
            else if (_bt == BtBonesShop)
            {
                ToggleShopPanel();
            }
        }

        void OnClickCategory(AnturaSpaceCategoryButton _bt)
        {
            if (showItemsTween.IsPlaying())
                return;

            var scene = AnturaSpaceScene.I as AnturaSpaceScene;
            if (scene.TutorialMode && scene.tutorialManager.CurrentTutorialFocus != _bt)
                return;

            _bt.AnimateClick();
            _bt.PlayClickFx();
            SelectCategory(_bt.Category);
            if (onRewardCategorySelectedInCustomization != null)
            {
                onRewardCategorySelectedInCustomization(_bt.Category.ToString());
            }
        }

        void OnClickItem(AnturaSpaceItemButton _bt)
        {
            var scene = AnturaSpaceScene.I as AnturaSpaceScene;
            if (scene.TutorialMode && scene.tutorialManager.CurrentTutorialFocus != _bt)
                return;

            if (_bt.Data == _currSelectedRewardBaseItem) return; // Already clicked

            SelectReward(_bt.Data);

            if (_bt.Data == null)
            {
                // This is the "remove" button, no prop is added
                return;
            }

            RewardBase rewardBase = _bt.Data.data;
            if (rewardBase != null && onRewardSelectedInCustomization != null)
            {
                onRewardSelectedInCustomization(rewardBase.ID);
            }

            RewardProp rewardProp = rewardBase as RewardProp;
            if (rewardProp != null)
            {
                if (MERGE_EARS && rewardProp.Category == "EAR_R" || rewardProp.Category == "EAR_L"
                    && onRewardCategorySelectedInCustomization != null)
                {
                    onRewardCategorySelectedInCustomization(rewardProp.Category);
                }
                AudioManager.I.PlayDialogue(rewardProp.LocID);
            }
        }

        void OnClickSwatch(AnturaSpaceSwatchButton _bt)
        {
            // Play audio
            AudioManager.I.PlayDialogue(_bt.Data.data.LocID);

            SelectSwatch(_bt.Data);

            // Is new check
            if (_bt.IcoNew.activeSelf)
            {
                _bt.SetAsNew(false);
                ReloadRewardsDatas();
                RefreshCategories();
                RefreshItems(true, AppManager.I.Player.PetData.SelectedPet);
            }
        }

        #endregion

        public CanvasScaler canvasScaler;
        public Camera uiCamera;

        public Vector3 ScreenToUIPoint(Vector3 pos)
        {
            float resolutionRatio = Screen.height / canvasScaler.referenceResolution.y;
            return (pos - new Vector3(Screen.width / 2, Screen.height / 2)) / resolutionRatio;
        }

        public Vector3 WorldToUIPoint(Vector3 pos)
        {
            return ScreenToUIPoint(uiCamera.WorldToScreenPoint(pos));
        }

    }
}
