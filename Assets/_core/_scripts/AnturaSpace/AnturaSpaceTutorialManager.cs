using System;
using Antura.AnturaSpace.UI;
using Antura.Audio;
using Antura.Core;
using Antura.Dog;
using Antura.Profile;
using Antura.Tutorial;
using Antura.UI;
using UnityEngine;
using System.Collections;
using System.Linq;
using Antura.Database;
using Antura.Extensions;
using Antura.Helpers;
using Antura.Keeper;
using DG.Tweening;
using UnityEngine.UI;

namespace Antura.AnturaSpace
{
    /// <summary>
    /// Implements a tutorial for the AnturaSpace scene.
    /// </summary>
    public class AnturaSpaceTutorialManager : TutorialManager
    {
        // References
        public AnturaSpaceScene _mScene;

#pragma warning disable 649
        [SerializeField]
        private Camera m_oCameraUI;
        [SerializeField]
        private Button m_oCustomizationButton;
#pragma warning restore 649

        public AnturaLocomotion m_oAnturaBehaviour;
        public AnturaSpaceUI UI;
        public ShopDecorationsManager ShopDecorationsManager;
        public Button m_oCookieButton;
        public Button m_oPhotoButton;

        private AnturaSpaceCategoryButton m_oCategoryButton;
        private AnturaSpaceItemButton m_oItemButton;
        private AnturaSpaceSwatchButton m_oSwatchButton;

        protected override AppScene CurrentAppScene
        {
            get { return AppScene.AnturaSpace; }
        }

        protected override void InternalHandleStart()
        {
            // References
            TutorialUI.SetCamera(m_oCameraUI);

            // Play sequential tutorial phases
            switch (FirstContactManager.I.CurrentPhaseInSequence)
            {
                case FirstContactPhase.AnturaSpace_TouchAntura:
                    StepTutorialTouchAntura();
                    return;

                case FirstContactPhase.AnturaSpace_Customization:
                    StepTutorialCustomization();
                    return;

                case FirstContactPhase.AnturaSpace_Shop:
                    StepTutorialShop();
                    return;

                case FirstContactPhase.AnturaSpace_Exit:
                    StepTutorialExit();
                    return;
            }

            // Play bonus tutorial phases
            bool isPhaseToBeCompleted = IsPhaseToBeCompleted(FirstContactPhase.AnturaSpace_Photo);
            if (isPhaseToBeCompleted)
            {
                StepTutorialPhoto();
                return;
            }

            // If nothing else is to be done, stop the tutorial
            StopTutorialRunning();
        }

        protected override void SetPhaseUIShown(FirstContactPhase phase, bool choice)
        {
            switch (phase)
            {
                case FirstContactPhase.AnturaSpace_Shop:
                    UI.ShowShopButton(choice);
                    if (choice)
                    {
                        ShopDecorationsManager.SetContextClosed();
                    }
                    else
                    {
                        ShopDecorationsManager.SetContextHidden();
                    }
                    break;
                case FirstContactPhase.AnturaSpace_Customization:
                    m_oCustomizationButton.gameObject.SetActive(choice);
                    break;
                case FirstContactPhase.AnturaSpace_Photo:
                    m_oPhotoButton.gameObject.SetActive(choice);
                    break;
                case FirstContactPhase.AnturaSpace_Exit:
                    if (choice)
                    {
                        _mScene.ShowBackButton();
                    }
                    else
                    {
                        _mScene.HideBackButton();
                    }
                    break;
            }
        }

        #region Touch Antura

        private void StepTutorialTouchAntura()
        {
            CurrentRunningPhase = FirstContactPhase.AnturaSpace_TouchAntura;

            // Push the player to touch Antura

            TutorialUI.Clear(false);

            // Reset antura as sleeping
            _mScene.Antura.transform.position = _mScene.SceneCenter.position;
            _mScene.Antura.AnimationController.State = AnturaAnimationStates.sleeping;
            _mScene.CurrentState = _mScene.Sleeping;

            m_oAnturaBehaviour.onTouched += HandleAnturaTouched;

            Vector3 clickOffset = m_oAnturaBehaviour.IsSleeping ? Vector3.down * 2 : Vector3.zero;
            TutorialUI.ClickRepeat(m_oAnturaBehaviour.gameObject.transform.position + clickOffset + Vector3.forward * -2 + Vector3.up,
                float.MaxValue, 1);

            Dialogue(LocalizationDataId.AnturaSpace_Intro);
        }

        public void HandleAnturaTouched()
        {
            m_oAnturaBehaviour.onTouched -= HandleAnturaTouched;

            DialogueThen(LocalizationDataId.AnturaSpace_Intro_Touch, () =>
            {
                TutorialUI.Clear(false);
                CompleteTutorialPhase();
            }
              );
        }

        #endregion

        #region Customization

        private enum CustomizationTutorialStep
        {
            START,
            OPEN_CUSTOMIZE,
            SELECT_CATEGORY,
            SELECT_ITEM,
            SELECT_COLOR,
            CLOSE_CUSTOMIZE,
            FINISH
        }

        private CustomizationTutorialStep _currentCustomizationStep = CustomizationTutorialStep.START;
        private void StepTutorialCustomization()
        {
            CurrentRunningPhase = FirstContactPhase.AnturaSpace_Customization;

            if (_currentCustomizationStep < CustomizationTutorialStep.FINISH)
                _currentCustomizationStep += 1;

            //Debug.Log("CURRENT STEP IS " + _currentCustomizationStep);
            TutorialUI.Clear(false);

            switch (_currentCustomizationStep)
            {
                case CustomizationTutorialStep.OPEN_CUSTOMIZE:
                    AudioManager.I.StopDialogue(false);

                    // Reset state for the tutorial
                    var anturaModelManager = FindObjectOfType<AnturaModelManager>();
                    if (anturaModelManager)
                        anturaModelManager.ClearLoadedRewardPacks();
                    AppManager.I.Player.CurrentAnturaCustomizations.ClearEquippedProps();

                    DialogueThen(
                        LocalizationDataId.AnturaSpace_Custom_1,
                        () =>
                        {
                            m_oCustomizationButton.gameObject.SetActive(true);
                            m_oCustomizationButton.onClick.AddListener(StepTutorialCustomization);
                            TutorialUI.ClickRepeat(m_oCustomizationButton.transform.position, float.MaxValue, 1);
                            CurrentTutorialFocus = m_oCustomizationButton;

                            Dialogue(LocalizationDataId.AnturaSpace_Custom_2);
                        });

                    break;

                case CustomizationTutorialStep.SELECT_CATEGORY:

                    m_oCustomizationButton.onClick.RemoveListener(StepTutorialCustomization);
                    CurrentTutorialFocus = null;

                    StartCoroutine(DelayedCallbackCO(
                        () =>
                        {
                            m_oCategoryButton = _mScene.UI.GetNewCategoryButton();
                            if (m_oCategoryButton == null)
                            {
                                m_oCategoryButton = _mScene.UI.GetFirstUnlockedCategoryButton();
                            }
                            m_oCategoryButton.Bt.onClick.AddListener(StepTutorialCustomization);
                            CurrentTutorialFocus = m_oCategoryButton;

                            TutorialUI.ClickRepeat(m_oCategoryButton.transform.position, float.MaxValue, 1);
                        }));
                    break;

                case CustomizationTutorialStep.SELECT_ITEM:

                    // Unregister from category button
                    m_oCategoryButton.Bt.onClick.RemoveListener(StepTutorialCustomization);
                    CurrentTutorialFocus = null;

                    StartCoroutine(DelayedCallbackCO(
                        () =>
                        {
                            // Register on item button
                            m_oItemButton = _mScene.UI.GetNewItemButton();
                            if (m_oItemButton == null)
                            {
                                m_oItemButton = _mScene.UI.GetFirstUnlockedItemButton();
                            }
                            m_oItemButton.Bt.onClick.AddListener(StepTutorialCustomization);
                            CurrentTutorialFocus = m_oItemButton;

                            TutorialUI.ClickRepeat(m_oItemButton.transform.position, float.MaxValue, 1);
                        }));
                    break;

                case CustomizationTutorialStep.SELECT_COLOR:

                    Dialogue(LocalizationDataId.AnturaSpace_Custom_3);
                    CurrentTutorialFocus = null;

                    // Cleanup last step
                    m_oItemButton.Bt.onClick.RemoveListener(StepTutorialCustomization);
                    CurrentTutorialFocus = null;

                    StartCoroutine(DelayedCallbackCO(
                        () =>
                        {
                            // Register on item button
                            m_oSwatchButton = _mScene.UI.GetRandomUnselectedSwatch();
                            m_oSwatchButton.Bt.onClick.AddListener(StepTutorialCustomization);

                            TutorialUI.ClickRepeat(m_oSwatchButton.transform.position, float.MaxValue, 1);
                        }));
                    break;

                case CustomizationTutorialStep.CLOSE_CUSTOMIZE:

                    Dialogue(LocalizationDataId.AnturaSpace_Custom_4);

                    // Cleanup last step
                    m_oSwatchButton.Bt.onClick.RemoveListener(StepTutorialCustomization);

                    // New step
                    m_oAnturaBehaviour.onTouched += StepTutorialCustomization;
                    m_oCustomizationButton.onClick.AddListener(StepTutorialCustomization);
                    CurrentTutorialFocus = m_oCustomizationButton;

                    StartCoroutine(DelayedCallbackCO(
                     () =>
                     {
                         TutorialUI.ClickRepeat(m_oCustomizationButton.transform.position, float.MaxValue, 1);
                     }));

                    /*
                    // New step
                    StartCoroutine(WaitAnturaInCenterCO(
                        () => {
                            Vector3 clickOffset = m_oAnturaBehaviour.IsSleeping ? Vector3.down * 2 : Vector3.down * 1.5f;
                            TutorialUI.ClickRepeat(
                                m_oAnturaBehaviour.gameObject.transform.position + clickOffset + Vector3.forward * -2 + Vector3.up,
                                float.MaxValue, 1);
                        }));
                        */
                    break;

                case CustomizationTutorialStep.FINISH:

                    // Cleanup last step
                    m_oCustomizationButton.onClick.RemoveListener(StepTutorialCustomization);
                    m_oAnturaBehaviour.onTouched -= StepTutorialCustomization;
                    CurrentTutorialFocus = null;

                    CompleteTutorialPhase();
                    break;
            }
        }

        #endregion

        #region Shop

        private enum ShopTutorialStep
        {
            START,
            ENTER_SHOP,
            DRAG_BONE,
            PLACE_NEW_DECORATION,
            CONFIRM_BUY_DECORATION,
            MOVE_DECORATION,
            EXIT_SHOP,
            FINISH
        }

        public void FakeAdvanceTutorialShop()
        {
            StepTutorialShop();
        }

        private ShopTutorialStep _currentShopStep = ShopTutorialStep.START;
        private void StepTutorialShop()
        {
            CurrentRunningPhase = FirstContactPhase.AnturaSpace_Shop;
            if (_currentShopStep < ShopTutorialStep.FINISH)
                _currentShopStep += 1;

            TutorialUI.Clear(false);
            AudioManager.I.StopDialogue(false);

            // Hide other UIs
            SetPhaseUIShown(FirstContactPhase.AnturaSpace_Customization, false);
            SetPhaseUIShown(FirstContactPhase.AnturaSpace_Exit, false);

            ShopActionUI actionUI;
            Button yesButton;

            switch (_currentShopStep)
            {
                case ShopTutorialStep.ENTER_SHOP:

                    // Start from a clean state
                    AppManager.I.Player.MakeSureHasEnoughBones(20);
                    ShopDecorationsManager.I.DeleteAllDecorations();

                    // Dialog -> Appear button
                    DialogueThen(
                        LocalizationDataId.AnturaSpace_Shop_Intro,
                        () =>
                        {
                            ShopDecorationsManager.SetContextClosed();
                            UI.ShowShopButton(true);
                            m_oCookieButton.onClick.AddListener(StepTutorialShop);
                            TutorialUI.ClickRepeat(m_oCookieButton.transform.position, float.MaxValue, 1);
                            Dialogue(LocalizationDataId.AnturaSpace_Shop_Open);
                            CurrentTutorialFocus = m_oCookieButton;
                        }
                        );
                    break;

                case ShopTutorialStep.DRAG_BONE:

                    // Clean last step
                    m_oCookieButton.onClick.RemoveListener(StepTutorialShop);

                    // New step
                    Dialogue(LocalizationDataId.AnturaSpace_Shop_Cookie);
                    actionUI = UI.ShopPanelUI.GetActionUIByName("ShopAction_Bone");
                    _mScene.onEatObject += StepTutorialShop;
                    CurrentTutorialFocus = actionUI;

                    // Start drag line
                    StartCoroutine(DelayedCallbackCO(
                        () =>
                        {
                            StartDrawDragLineFrom(actionUI.transform);
                        }, 1.5f
                    ));

                    // Stop dragging as soon as we get a bone
                    actionUI.ShopAction.OnActionCommitted += StopDrawDragLine;

                    break;
                case ShopTutorialStep.PLACE_NEW_DECORATION:

                    // Cleanup last step
                    StopDrawDragLine();
                    actionUI = UI.ShopPanelUI.GetActionUIByName("ShopAction_Bone");
                    actionUI.ShopAction.OnActionCommitted -= StopDrawDragLine;
                    _mScene.onEatObject -= StepTutorialShop;


                    DialogueThen(LocalizationDataId.AnturaSpace_Intro_Cookie,
                        () =>
                        {
                            Dialogue(LocalizationDataId.AnturaSpace_Shop_BuyItem);

                            // New step
                            var leftmostUnassignedSlot =
                                ShopDecorationsManager.GetDecorationSlots()
                                    .Where(x => !x.Assigned && x.slotType == ShopDecorationSlotType.Prop)
                                    .MinBy(x => x.transform.position.x);
                            actionUI = UI.ShopPanelUI.GetActionUIByName("ShopAction_Decoration_Prop2");
                            StartDrawDragLineFromTo(actionUI.transform, leftmostUnassignedSlot.transform);
                            ShopDecorationsManager.OnPurchaseConfirmationRequested += StepTutorialShop;
                            CurrentTutorialFocus = actionUI;
                        }
                    );

                    break;

                case ShopTutorialStep.CONFIRM_BUY_DECORATION:

                    // Cleanup last step
                    StopDrawDragLine();
                    ShopDecorationsManager.OnPurchaseConfirmationRequested -= StepTutorialShop;

                    // New step
                    ShopDecorationsManager.OnPurchaseComplete += StepTutorialShop;

                    yesButton = UI.ShopPanelUI.confirmationYesButton;

                    CurrentTutorialFocus = yesButton;

                    DialogueThen(
                        LocalizationDataId.AnturaSpace_Shop_ConfirmBuy,
                        () =>
                        {
                            TutorialUI.ClickRepeat(yesButton.transform.position, float.MaxValue, 1);
                        }
                        );
                    break;

                case ShopTutorialStep.MOVE_DECORATION:

                    // Cleanup last step
                    ShopDecorationsManager.OnPurchaseComplete -= StepTutorialShop;

                    // New step

                    // Slot we assigned
                    var assignedSlot = ShopDecorationsManager.GetDecorationSlots().FirstOrDefault(x => x.Assigned && x.slotType == ShopDecorationSlotType.Prop);
                    var rightmostUnassignedSlot = ShopDecorationsManager.GetDecorationSlots()
                        .Where(x => !x.Assigned && x.slotType == ShopDecorationSlotType.Prop)
                        .MaxBy(x => x.transform.position.x);
                    StartDrawDragLineFromTo(assignedSlot.transform, rightmostUnassignedSlot.transform, Vector3.up * 2f);
                    ShopDecorationsManager.OnDragStop += StepTutorialShop;

                    DialogueThen(
                        LocalizationDataId.AnturaSpace_Shop_MoveItem,
                        () =>
                        {
                        }
                    );

                    break;

                case ShopTutorialStep.EXIT_SHOP:

                    // Cleanup last step
                    ShopDecorationsManager.OnDragStop -= StepTutorialShop;
                    StopDrawDragLine();

                    yesButton = UI.ShopPanelUI.confirmationYesButton;
                    yesButton.onClick.RemoveListener(StepTutorialShop);

                    // New step
                    DialogueThen(
                        LocalizationDataId.AnturaSpace_Shop_Close,
                        () =>
                        {
                            m_oCookieButton.onClick.AddListener(StepTutorialShop);
                            TutorialUI.ClickRepeat(m_oCookieButton.transform.position, float.MaxValue, 1);

                            CurrentTutorialFocus = m_oCookieButton;
                        }
                    );

                    break;

                case ShopTutorialStep.FINISH:

                    // Cleanup last step
                    m_oCookieButton.onClick.RemoveListener(StepTutorialShop);

                    // New step
                    _mScene.ShowBackButton();
                    Dialogue(LocalizationDataId.AnturaSpace_PlayAndExit);

                    CurrentTutorialFocus = null;
                    CompleteTutorialPhase();
                    break;
            }
        }

        #endregion

        #region Photo

        private enum PhotoTutorialStep
        {
            START,
            CLICK_PHOTO,
            CONFIRM_PHOTO,
            FINISH
        }

        public ShopAction_Photo photoAction;

        private PhotoTutorialStep _currentPhotoStep = PhotoTutorialStep.START;
        private void StepTutorialPhoto()
        {
            CurrentRunningPhase = FirstContactPhase.AnturaSpace_Photo;

            if (_currentPhotoStep < PhotoTutorialStep.FINISH)
                _currentPhotoStep += 1;

            TutorialUI.Clear(false);
            //Debug.Log("CURRENT STEP IS " + _currentPhotoStep);

            // Hide other UIs
            SetPhaseUIShown(FirstContactPhase.AnturaSpace_Customization, false);
            SetPhaseUIShown(FirstContactPhase.AnturaSpace_Exit, false);

            ShopActionUI photoActionUI;
            switch (_currentPhotoStep)
            {
                case PhotoTutorialStep.CLICK_PHOTO:

                    DialogueThen(LocalizationDataId.AnturaSpace_Photo_Intro,
                        () =>
                        {
                            // We need to find the shop action photo UI which is attached with the ShopAction_Photo
                            photoActionUI = photoAction.GetComponent<ShopActionUI>();
                            CurrentTutorialFocus = photoActionUI;

                            // Makes sure you have enough bones
                            var photoCost = photoAction.bonesCost;
                            AppManager.I.Player.AddBones(photoCost);

                            // Focus on the photo button
                            m_oPhotoButton.gameObject.SetActive(true);
                            m_oPhotoButton.onClick.AddListener(StepTutorialPhoto);
                            TutorialUI.ClickRepeat(m_oPhotoButton.transform.position, float.MaxValue, 1);

                            Dialogue(LocalizationDataId.AnturaSpace_Photo_Take);
                        });

                    break;

                case PhotoTutorialStep.CONFIRM_PHOTO:

                    CurrentTutorialFocus = m_oCookieButton; // HACK: focus is actually photo action, but the shop has no reference to it, so we re-use the shop button instead

                    // Cleanup last step
                    m_oPhotoButton.onClick.RemoveListener(StepTutorialPhoto);

                    // New step
                    ShopPhotoManager.I.OnPurchaseCompleted += StepTutorialPhoto;
                    var yesButton = UI.ShopPanelUI.confirmationYesButton;

                    StartCoroutine(DelayedCallbackCO(() =>
                    {
                        TutorialUI.ClickRepeat(yesButton.transform.position, float.MaxValue, 1);
                    }));

                    Dialogue(
                        LocalizationDataId.AnturaSpace_Shop_ConfirmBuy
                    );

                    break;
                case PhotoTutorialStep.FINISH:

                    // Cleanup last step
                    ShopPhotoManager.I.OnPurchaseCompleted -= StepTutorialPhoto;

                    // New step
                    _mScene.ShowBackButton();

                    Dialogue(LocalizationDataId.AnturaSpace_Photo_Gallery);

                    CompleteTutorialPhase();

                    CurrentTutorialFocus = null;
                    break;
            }
        }
        #endregion

        #region Exit

        private void StepTutorialExit()
        {
            CurrentRunningPhase = FirstContactPhase.AnturaSpace_Exit;

            TutorialUI.Clear(false);
            AudioManager.I.StopDialogue(false);

            _mScene.ShowBackButton();

            Dialogue(LocalizationDataId.AnturaSpace_Exit);

            TutorialUI.ClickRepeat(
                Vector3.down * 0.025f + m_oCameraUI.ScreenToWorldPoint(new Vector3(GlobalUI.I.BackButton.RectT.position.x,
                    GlobalUI.I.BackButton.RectT.position.y, m_oCameraUI.nearClipPlane)), float.MaxValue, 1);
        }

        #endregion

        #region Utility functions

        IEnumerator DelayedCallbackCO(Action callback, float delay = 0.6f)
        {
            yield return new WaitForSeconds(delay);

            if (callback != null)
            {
                callback();
            }
        }

        /*
        IEnumerator WaitAnturaInCenterCO(System.Action callback)
        {
            while (!_mScene.Antura.IsNearTargetPosition || _mScene.Antura.IsSliping)
                yield return null;

            if (callback != null) {
                callback();
            }
        }*/

        private TutorialUIAnimation dragLineAnimation;

        private void StartDrawDragLineFromTo(Transform fromTr, Transform toTr)
        {
            StartDrawDragLineFromTo(fromTr, toTr, Vector3.zero);
        }
        private void StartDrawDragLineFromTo(Transform fromTr, Transform toTr, Vector3 offset)
        {
            TutorialUI.Clear(false);

            Vector3[] path = new Vector3[3];
            path[0] = fromTr.position + offset;
            path[2] = toTr.position + offset;
            path[1] = (path[0] + path[2]) / 2f + Vector3.up * 4 + Vector3.left * 2;

            dragLineAnimation = TutorialUI.DrawLine(path, TutorialUI.DrawLineMode.Finger, false, true);
            dragLineAnimation.MainTween.timeScale = 0.8f;
            dragLineAnimation.OnComplete(delegate
            {
                StartDrawDragLineFromTo(fromTr, toTr, offset);
            });
        }

        private void StartDrawDragLineFrom(Transform fromTr)
        {
            TutorialUI.Clear(false);

            Vector3[] path = new Vector3[3];
            path[0] = fromTr.position;
            path[1] = path[0] + Vector3.up * 4 + Vector3.left * 2;
            path[2] = m_oCameraUI.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2));
            path[2].z = path[1].z;

            dragLineAnimation = TutorialUI.DrawLine(path, TutorialUI.DrawLineMode.Finger);
            dragLineAnimation.MainTween.timeScale = 0.8f;
            dragLineAnimation.OnComplete(delegate
            {
                StartDrawDragLineFrom(fromTr);
            });
        }

        private void StopDrawDragLine()
        {
            if (dragLineAnimation != null)
            {
                dragLineAnimation.MainTween.Kill();
                dragLineAnimation = null;
            }
        }

        #endregion
    }
}
