using System;
using System.Collections.Generic;
using System.Linq;
using Antura.Database;
using Antura.UI;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Antura.AnturaSpace
{
    public class ShopPanelUI : MonoBehaviour
    {
        public GameObject shopActionUIPrefab;

        public Transform bottomButtonsPivotTr;
        public Transform sideButtonsPivotTr;

        public RectTransform purchasePanelBottom;
        public RectTransform purchasePanelSide;
        public RectTransform purchasePanelAlwaysAvailableUI;
        public RectTransform dragPanel;
        public RectTransform confirmationPanel;
        public ShopConfirmationPanelUI confirmationPanelUI;
        public ScrollRect scrollRect;

        public Button confirmationYesButton;
        public Button confirmationNoButton;

        public ShopActionUI photoActionUI;
        private List<ShopActionUI> actionUIs;
        Tween scrollShowTween;

        private Tween showShopPanelTween,
            showDragPanelTween,
            showConfirmationPanelTween,
            showPurchasePanelAlwaysAvailableTween;


        public ShopActionUI GetActionUIByName(string actionName)
        {
            return actionUIs.FirstOrDefault(x => String.Equals(x.ShopAction.name, actionName, StringComparison.CurrentCultureIgnoreCase));
        }


        public void SetActions(ShopAction[] shopActions)
        {
            actionUIs = new List<ShopActionUI>();
            foreach (var shopAction in shopActions)
            {
                var shopActionUIgo = Instantiate(shopActionUIPrefab);
                var parentTr = shopAction.IsOnTheSide ? sideButtonsPivotTr : bottomButtonsPivotTr;
                shopActionUIgo.transform.SetParent(parentTr);
                shopActionUIgo.transform.localScale = Vector3.one;
                var actionUI = shopActionUIgo.GetComponent<ShopActionUI>();
                actionUI.scrollRect = this.scrollRect;
                actionUI.SetAction(shopAction);
                actionUIs.Add(actionUI);
            }
        }

        private AnturaSpaceScene scene;

        public void Initialise()
        {
            if (!scene)
                scene = FindObjectOfType<AnturaSpaceScene>();

            const float duration = 0.3f;
            showShopPanelTween = DOTween.Sequence().SetAutoKill(false).Pause()
                .Append(purchasePanelBottom.DOAnchorPosY(-150, duration).From().SetEase(Ease.OutQuad));
            //                    .Join(purchasePanelSide.DOAnchorPosX(1250, duration).From().SetEase(Ease.OutBack));
            showDragPanelTween = DOTween.Sequence().SetAutoKill(false).Pause()
                    .Append(dragPanel.DOAnchorPosY(-350, duration).From().SetEase(Ease.OutQuad));
            showConfirmationPanelTween =
                confirmationPanel.DOAnchorPosY(-350, duration).From().SetEase(Ease.Linear).SetAutoKill(false).Pause();
            showPurchasePanelAlwaysAvailableTween =
                purchasePanelAlwaysAvailableUI.DOAnchorPosX(200, duration)
                    .From()
                    .SetEase(Ease.OutBack)
                    .SetAutoKill(false);
            scrollRect.horizontalNormalizedPosition = 0;
            scrollShowTween = scrollRect.DOHorizontalNormalizedPos(1, 0.6f).SetAutoKill(false).Pause().SetDelay(0.15f);
            scrollShowTween.ForceInit();

            ShopDecorationsManager.I.OnContextChange += HandleContextChange;
            ShopDecorationsManager.I.OnPurchaseConfirmationRequested += HandlePurchaseConfirmationRequested;
            ShopDecorationsManager.I.OnDeleteConfirmationRequested += HandleDeleteConfirmationRequested;
            ShopPhotoManager.I.OnPhotoConfirmationRequested += HandlePhotoConfirmationRequested;
        }


        private void OnEnable()
        {
            if (!scene)
                scene = FindObjectOfType<AnturaSpaceScene>();
            HandleContextChange(ShopContext.Purchase);
            scrollShowTween.Restart();
        }

        void OnDestroy()
        {
            scrollShowTween.Kill();
        }

        private void HandleContextChange(ShopContext shopContext)
        {
            //Debug.Log("CONTEXT UI SHOP: " + shopContext);
            switch (shopContext)
            {
                case ShopContext.Purchase:
                    scene.HideBackButton();
                    showPurchasePanelAlwaysAvailableTween.PlayForward();
                    showShopPanelTween.PlayForward();
                    showDragPanelTween.PlayBackwards();
                    showConfirmationPanelTween.PlayBackwards();
                    break;
                case ShopContext.NewPlacement:
                    scene.HideBackButton();
                    showPurchasePanelAlwaysAvailableTween.PlayBackwards();
                    showShopPanelTween.PlayBackwards();
                    showDragPanelTween.PlayBackwards();
                    showConfirmationPanelTween.PlayBackwards();
                    break;
                case ShopContext.MovingPlacement:
                    scene.HideBackButton();
                    showPurchasePanelAlwaysAvailableTween.PlayBackwards();
                    showShopPanelTween.PlayBackwards();
                    if (!AnturaSpaceScene.I.TutorialMode)
                        showDragPanelTween.PlayForward();
                    showConfirmationPanelTween.PlayBackwards();
                    break;
                case ShopContext.SpecialAction:
                    scene.HideBackButton();
                    showShopPanelTween.PlayBackwards();
                    showDragPanelTween.PlayBackwards();
                    showConfirmationPanelTween.PlayBackwards();
                    break;
                case ShopContext.Closed:
                    if (!scene.TutorialMode)
                        scene.ShowBackButton();
                    showPurchasePanelAlwaysAvailableTween.PlayForward();
                    showConfirmationPanelTween.PlayBackwards();
                    break;
                case ShopContext.Customization:
                    scene.HideBackButton();
                    showPurchasePanelAlwaysAvailableTween.PlayBackwards();
                    showConfirmationPanelTween.PlayBackwards();
                    break;
                case ShopContext.Hidden:
                    if (!scene.TutorialMode)
                        scene.ShowBackButton();
                    showPurchasePanelAlwaysAvailableTween.PlayBackwards();
                    showConfirmationPanelTween.PlayBackwards();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("shopContext", shopContext, null);
            }
        }


        #region Confirmation

        private void HandlePurchaseConfirmationRequested()
        {
            showDragPanelTween.PlayBackwards();
            showPurchasePanelAlwaysAvailableTween.PlayBackwards();

            confirmationPanelUI.SetupForPurchase();

            ShopDecorationsManager.I.ResetSlotHighlights();

            if (AnturaSpaceScene.I.TutorialMode)
            {
                AskForConfirmation(ShopDecorationsManager.I.ConfirmPurchase, null);
            }
            else
            {
                AskForConfirmation(ShopDecorationsManager.I.ConfirmPurchase, ShopDecorationsManager.I.CancelPurchase);
            }
        }

        private void HandleDeleteConfirmationRequested()
        {
            showPurchasePanelAlwaysAvailableTween.PlayBackwards();

            confirmationPanelUI.SetupForDeletion();

            ShopDecorationsManager.I.ResetSlotHighlights();

            GlobalUI.ShowPrompt(LocalizationDataId.UI_AreYouSure, ShopDecorationsManager.I.ConfirmDeletion, ShopDecorationsManager.I.CancelDeletion, Language.LanguageUse.Learning);
        }

        private void HandlePhotoConfirmationRequested()
        {
            showShopPanelTween.PlayBackwards();
            showDragPanelTween.PlayBackwards();
            showPurchasePanelAlwaysAvailableTween.PlayBackwards();

            confirmationPanelUI.SetupForPhoto();

            if (AnturaSpaceScene.I.TutorialMode)
            {
                AskForConfirmation(ShopPhotoManager.I.ConfirmPhoto, null);
            }
            else
            {
                AskForConfirmation(ShopPhotoManager.I.ConfirmPhoto, ShopPhotoManager.I.CancelPhoto);
            }
        }

        private UnityAction currentYesAction;
        private UnityAction currentNoAction;
        private void AskForConfirmation(UnityAction yesAction, UnityAction noAction)
        {
            currentYesAction = yesAction;
            currentNoAction = noAction;

            confirmationYesButton.onClick.AddListener(yesAction);
            confirmationYesButton.onClick.AddListener(ResetConfirmationButtons);

            if (noAction != null)
            {
                confirmationNoButton.onClick.AddListener(noAction);
                confirmationNoButton.onClick.AddListener(ResetConfirmationButtons);
            }

            showConfirmationPanelTween.PlayForward();
        }

        private void ResetConfirmationButtons()
        {
            confirmationYesButton.onClick.RemoveListener(currentYesAction);
            confirmationYesButton.onClick.RemoveListener(ResetConfirmationButtons);
            if (currentNoAction != null)
            {
                confirmationNoButton.onClick.RemoveListener(currentNoAction);
                confirmationNoButton.onClick.RemoveListener(ResetConfirmationButtons);
            }
        }

        #endregion

        public void UpdateAllActionButtons()
        {
            foreach (var actionUI in actionUIs)
            {
                actionUI.UpdateAction();
            }
            photoActionUI.UpdateAction();
        }
    }
}
