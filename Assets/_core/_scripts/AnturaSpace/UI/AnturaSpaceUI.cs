using Antura.Core;
using Antura.Dog;
using Antura.Extensions;
using Antura.Helpers;
using Antura.Rewards;
using Antura.Tutorial;
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
using Antura.Profile;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.AnturaSpace.UI
{
    /// <summary>
    /// General controller of the UI in the Antura Space scene.
    /// </summary>
    public class AnturaSpaceUI : MonoBehaviour
    {
        public int MaxItems = 10;
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


        public event System.Action onEnterCustomization;
        public event System.Action onExitCustomization;

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
                    foreach (Transform container in rewardsContainers)
                    {
                        foreach (Transform child in container)
                            Destroy(child.gameObject);
                    }
                });
            showSwatchesTween = SwatchesContainer.DOAnchorPosY(-100, duration).From().SetEase(Ease.OutBack).SetAutoKill(false).Pause()
                .OnRewind(() => SwatchesContainer.gameObject.SetActive(false));

            CategoriesContainer.gameObject.SetActive(false);
            ShopPanelContainer.gameObject.SetActive(false);
            ItemsContainer.gameObject.SetActive(false);
            SwatchesContainer.gameObject.SetActive(false);

            // Listeneres
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
            return btsSwatches.Where(x => x.Data != null).ToList().GetRange(15, 4).Where(x => !x.Data.IsSelected).ToList().RandomSelectOne();
        }

        #endregion

        #region Methods

        public void ShowShopButton(bool show)
        {
            BtBonesShop.gameObject.SetActive(show);
        }

        void SelectCategory(AnturaSpaceCategoryButton.AnturaSpaceCategory _category)
        {
            StopAllCoroutines();
            // Save configuration
            //AnturaModelManager.Instance.SaveAnturaCustomization();

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

            RewardBaseItem selectedRewardBaseData = RefreshItems();
            ItemsContainer.gameObject.SetActive(true);
            showItemsTween.PlayForward();

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

        void SelectReward(RewardBaseItem rewardBaseData)
        {
            var scene = AnturaSpaceScene.I as AnturaSpaceScene;
            if (rewardBaseData == null && scene.TutorialMode)
                return;

            showSwatchesTween.Rewind();
            bool isTextureOrDecal = currCategory == AnturaSpaceCategoryButton.AnturaSpaceCategory.Texture
                                    || currCategory == AnturaSpaceCategoryButton.AnturaSpaceCategory.Decal;
            BTRemoveMods.gameObject.SetActive(!isTextureOrDecal && rewardBaseData != null);
            if (rewardBaseData == null)
            {
                foreach (AnturaSpaceItemButton item in btsItems)
                {
                    item.Toggle(false);
                }
                if (currCategory == AnturaSpaceCategoryButton.AnturaSpaceCategory.Ears)
                {
                    AnturaModelManager.I.ClearLoadedRewardInCategory("EAR_L");
                    AnturaModelManager.I.ClearLoadedRewardInCategory("EAR_R");
                }
                else
                {
                    AnturaModelManager.I.ClearLoadedRewardInCategory(currCategory.ToString());
                }
                return;
            }

            _currSelectedRewardBaseItem = rewardBaseData;
            currRewardColorItems = AppManager.I.RewardSystemManager.GetRewardColorItemsForBase(rewardBaseData.data);
            if (currRewardColorItems.Count == 0)
            {
                Debug.Log("No color swatches for the selected reward!");
                return;
            }

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

            ReloadRewardsDatas();
            RefreshCategories();
            RefreshItems(true);
        }

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
            if (currCategory == AnturaSpaceCategoryButton.AnturaSpaceCategory.Ears)
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
                    case AnturaSpaceCategoryButton.AnturaSpaceCategory.Ears:
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
        RewardBaseItem RefreshItems(bool toggleOnly = false)
        {
            bool useImages = currCategory == AnturaSpaceCategoryButton.AnturaSpaceCategory.Texture ||
                             currCategory == AnturaSpaceCategoryButton.AnturaSpaceCategory.Decal;
            // Hide non-existent items
            for (int i = currRewardBaseItems.Count - 1; i < btsItems.Length; ++i)
            {
                btsItems[i].gameObject.SetActive(false);
            }
            // Setup and show items
            RewardBaseItem selectedRewardBaseData = null;
            for (int i = 0; i < currRewardBaseItems.Count; ++i)
            {
                RewardBaseItem rewardBaseItem = currRewardBaseItems[i];
                AnturaSpaceItemButton item = btsItems[i];
                item.gameObject.SetActive(true);
                item.Data = rewardBaseItem;
                if (rewardBaseItem != null)
                {
                    if (!useImages && !toggleOnly)
                    {
                        item.RewardContainer.gameObject.SetLayerRecursive(GenericHelper.LayerMaskToIndex(RewardsLayer));
                        CameraHelper.FitRewardToUICamera(item.RewardContainer.GetChild(0), item.RewardCamera, FlipRewards);
                    }
                    item.SetAsNew(rewardBaseItem.IsNew);
                    item.Toggle(rewardBaseItem.IsSelected);
                    if (rewardBaseItem.IsSelected)
                    {
                        selectedRewardBaseData = rewardBaseItem;
                    }
                }
                else
                {
                    item.Toggle(false);
                }
                item.Lock(rewardBaseItem == null || !AppManager.I.RewardSystemManager.IsRewardBaseUnlocked(rewardBaseItem.data));
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

            SelectReward(_bt.Data);
            RewardBase rewardBase = _bt.Data.data;
            if (rewardBase != null && onRewardSelectedInCustomization != null)
            {
                onRewardSelectedInCustomization(rewardBase.ID);
            }

            RewardProp rewardProp = rewardBase as RewardProp;
            if (rewardProp != null)
            {
                if (rewardProp.Category == "EAR_R" || rewardProp.Category == "EAR_L"
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
                RefreshItems(true);
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
