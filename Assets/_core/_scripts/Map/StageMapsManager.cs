using Antura.Audio;
using Antura.CameraEffects;
using Antura.Core;
using Antura.Database;
using Antura.Keeper;
using Antura.UI;
using DG.Tweening;
using System.Collections;
using System.Linq;
using Antura.Book;
using Antura.Minigames;
using Antura.Profile;
using UnityEngine;

namespace Antura.Map
{
    /// <summary>
    ///     General manager for the Map scene.
    ///     Handles the different maps for all Stages of the game.
    ///     Allows navigation from one map to the next (between stages).
    /// </summary>
    public class StageMapsManager : MonoBehaviour
    {
        [Header("Options")]
        public bool MovePlayerWithStageChange = true;
        public bool FollowPlayerWhenMoving = false;
        //public bool ShowStageButtons = false;
        //public bool ShowMovementButtons = false;

        [Header("References")]
        public StageMap[] stageMaps;
        public PlayerPin playerPin;
        public MapCameraController mapCameraController;

        [Header("UI")]

        public Camera UICamera;
        public Camera MapCamera;
        public MapStageIndicator mapStageIndicator;
        public MapPlayInfoPanel playInfoPanel;
        public MapPlayButtonsPanel playButtonsPanel;

        public GameObject playButton;

        // Additional UI for navigation
        public GameObject navigationIconsPanel;
        public GameObject learningBookButton;
        public GameObject minigamesBookButton;
        public GameObject profileBookButton;
        public GameObject anturaSpaceButton;

        #region State

        // The stage that is currently shown to the player
        private int shownStage;
        private bool inTransition;

        #endregion

        #region Properties

        private int PreviousPlayerStage
        {
            get { return PreviousJourneyPosition.Stage; }
        }

        private int CurrentPlayerStage    // @note: this may be different than shownStage as you can preview the next stages
        {
            get { return CurrentJourneyPosition.Stage; }
        }

        public static JourneyPosition PreviousJourneyPosition
        {
            get {
                return AppManager.I.Player.PreviousJourneyPosition;
            }
        }
        private JourneyPosition targetCurrentJourneyPosition;

        public static JourneyPosition CurrentJourneyPosition
        {
            get {
                return AppManager.I.Player.CurrentJourneyPosition;
            }
        }

        private int MaxUnlockedStage
        {
            get {
                return AppManager.I.Player.MaxJourneyPosition.Stage;
            }
        }

        private int FinalStage
        {
            get { return AppManager.I.JourneyHelper.GetFinalJourneyPosition().Stage; }
        }

        public StageMap StageMap(int Stage)
        {
            return stageMaps[Stage - 1];
        }

        public bool IsAtFirstStage
        {
            get { return shownStage == 1; }
        }

        private bool IsAtMaxUnlockedStage
        {
            get { return shownStage == MaxUnlockedStage; }
        }

        public bool IsAtFinalStage
        {
            get { return shownStage == FinalStage; }
        }

        public StageMap CurrentShownStageMap
        {
            get { return StageMap(shownStage); }
        }

        private bool IsStagePlayable(int stage)
        {
            return stage <= MaxUnlockedStage;
        }

        #endregion

        private static bool TEST_JOURNEY_POS = false;

        private void Awake()
        {
            // DEBUG
            if (TEST_JOURNEY_POS) {
                TEST_JOURNEY_POS = false;

                // TEST: already at a PS, later stage
                //AppManager.I.Player.SetMaxJourneyPosition(new JourneyPosition(3, 4, 2), _forced: true);
                //AppManager.I.Player.SetCurrentJourneyPosition(new JourneyPosition(3, 4, 2));
                //AppManager.I.Player.ForcePreviousJourneyPosition(new JourneyPosition(3, 4, 2));

                // TEST: basic PS
                //AppManager.I.Player.SetMaxJourneyPosition(new JourneyPosition(1, 2, 1), _forced: true);
                //AppManager.I.Player.SetCurrentJourneyPosition(new JourneyPosition(1, 2, 1));
                //AppManager.I.Player.ForcePreviousJourneyPosition(new JourneyPosition(1, 1, 100));

                // TEST: basic assessment
                AppManager.I.Player.SetMaxJourneyPosition(new JourneyPosition(1, 1, 100), _forced: true);
                AppManager.I.Player.SetCurrentJourneyPosition(new JourneyPosition(1, 1, 100));
                AppManager.I.Player.ForcePreviousJourneyPosition(new JourneyPosition(1, 1, 1));

                // TEST: next-stage PS
                //AppManager.I.Player.SetMaxJourneyPosition(new JourneyPosition(2, 1, 1), _forced: true);
                //AppManager.I.Player.SetCurrentJourneyPosition(new JourneyPosition(2, 1, 1));
                //AppManager.I.Player.ForcePreviousJourneyPosition(new JourneyPosition(1, 15, 100));

                Debug.Log("FORCED TEST_JOURNEY_POS");
            }

            //Debug.Log("Current JP is " + CurrentJourneyPosition + "\nPrevious JP was " + PreviousJourneyPosition);

            shownStage = PreviousPlayerStage;
            targetCurrentJourneyPosition = CurrentJourneyPosition;

            // Setup stage availability
            for (int stage_i = 1; stage_i <= stageMaps.Length; stage_i++)
            {
                bool isStageUnlocked = stage_i <= MaxUnlockedStage;
                bool isWholeStageUnlocked = stage_i < MaxUnlockedStage;
                StageMap(stage_i).Initialise(isStageUnlocked, isWholeStageUnlocked);
            }

            // If we got here after a direct minigame...
            // Re-open the GameSelector too
            if (AppManager.I.NavigationManager.NavData.DirectMiniGameData != null)
            {
                var directMiniGameData = AppManager.I.NavigationManager.NavData.DirectMiniGameData;
                BookManager.I.OpenBook(BookArea.MiniGames, directMiniGameData);
                AppManager.I.NavigationManager.NavData.DirectMiniGameData = null;
            }
        }

        public bool isLazyInitialised = false;

        IEnumerator LazyInitialiseCO()
        {
            for (int stage_i = 1; stage_i <= stageMaps.Length; stage_i++)
            {
                if (stage_i == shownStage) continue; // skip already seen stage
                yield return StartCoroutine(StageMap(stage_i).LazyInitialiseCO());
            }

            for (int stage_i = 1; stage_i <= stageMaps.Length; stage_i++)
            {
                if (stage_i == shownStage) continue; // skip already seen stage
                yield return StartCoroutine(StageMap(stage_i).LazyInitialiseOptionalsCO());
            }
        }

        private bool initialMovementNeedsAnimation;

        private IEnumerator Start()
        {
            //Debug.Log("SHOWN STAGE: " + shownStage);
            if (shownStage == 0) shownStage = 1;

            // Initialise the first one right away
            var firstBase = StageMap(shownStage).LazyInitialiseCO();
            while (firstBase.MoveNext()) { }
            var firstVisuals = StageMap(shownStage).LazyInitialiseOptionalsCO();
            while (firstVisuals.MoveNext()) { }

            // Show the current stage
            TeleportCameraToShownStage(shownStage);
            UpdateStageIndicatorUI(shownStage);
            //UpdateButtonsForStage(shownStage);

            // Position the player
            playerPin.gameObject.SetActive(true);
            //playerPin.onMoveStart += HidePlayPanel;
            //playerPin.onMoveStart += CheckCurrentStageForPlayerReset;
            //playerPin.onMoveEnd += ShowPlayPanel;
            playerPin.ForceToJourneyPosition(PreviousJourneyPosition, justVisuals: true);
            playerPin.LookAtNextPin(false);

            var isGameCompleted = AppManager.I.Player.HasFinalBeenShown();
            if (!isGameCompleted && WillPlayAssessmentNext()) {
                PlayRandomAssessmentDialog();
            }

            mapCameraController.Initialise(this);

            initialMovementNeedsAnimation = InitialiseMapMovement();

            var tutorialManager = gameObject.GetComponentInChildren<MapTutorialManager>();
            tutorialManager.HandleStart();

            if (!tutorialManager.IsRunning)
            {
                TriggerInitialMovement();
            }

            // Initialise the rest of the stages
            yield return StartCoroutine(LazyInitialiseCO());

            isLazyInitialised = true;
        }

        // TODO: trigger after the tutorial ends?
        public void TriggerInitialMovement()
        {
            StartCoroutine(InitialMovementCO());
        }

        bool InitialiseMapMovement()
        {
            //HidePlayPanel();
            StageMap(shownStage).FlushAppear(PreviousJourneyPosition);
            TeleportCameraToShownStage(shownStage);

            bool needsAnimation = !Equals(targetCurrentJourneyPosition, PreviousJourneyPosition);
            if (!needsAnimation)
            {
                //Debug.Log("Already at the correct stage " + shownStage);
                SelectPin(StageMap(shownStage).PinForJourneyPosition(targetCurrentJourneyPosition));
                StageMap(shownStage).FlushAppear(AppManager.I.Player.MaxJourneyPosition);
                mapCameraController.SetManualMovementCurrentMap();
            }
            return needsAnimation;
        }

        private IEnumerator InitialMovementCO()
        {
            //Debug.Log("TARGET CURRENT: " + targetCurrentJourneyPosition  + " PREV: " + PreviousJourneyPosition);
            if (initialMovementNeedsAnimation)
            {
                yield return new WaitForSeconds(1.0f);
                StageMap(shownStage).Appear(PreviousJourneyPosition, AppManager.I.Player.MaxJourneyPosition);

                //Debug.Log("Shown stage: " + shownStage + " TargetJourneyPos " + targetCurrentJourneyPosition +   " PreviousJourneyPos " + PreviousJourneyPosition);
                if (shownStage != targetCurrentJourneyPosition.Stage) {
                    //Debug.Log("ANIMATING TO STAGE: " + targetCurrentJourneyPosition.Stage + " THEN MOVING TO " + targetCurrentJourneyPosition);
                    yield return StartCoroutine(SwitchFromToStageCO(shownStage, targetCurrentJourneyPosition.Stage, true));
                    mapCameraController.SetAutoFollowTransformCurrentMap(playerPin.transform);
                    SelectPin(StageMap(shownStage).PinForJourneyPosition(targetCurrentJourneyPosition));
                    playerPin.MoveToJourneyPosition(targetCurrentJourneyPosition, StageMap(shownStage));
                } else {
                    //Debug.Log("JUST MOVING TO " + targetCurrentJourneyPosition);
                    yield return new WaitForSeconds(1.0f);
                    mapCameraController.SetAutoFollowTransformCurrentMap(playerPin.transform);
                    SelectPin(StageMap(shownStage).PinForJourneyPosition(targetCurrentJourneyPosition));
                    playerPin.MoveToJourneyPosition(targetCurrentJourneyPosition, StageMap(shownStage));
                }
            }

            while (playerPin.IsAnimating) {
                yield return null;
            }

            mapCameraController.SetManualMovementCurrentMap();
            //ReSelectCurrentPin();
        }

        private bool WillPlayAssessmentNext()
        {
            return CurrentJourneyPosition.IsAssessment()
                && CurrentJourneyPosition.Stage == AppManager.I.Player.MaxJourneyPosition.Stage &&
                CurrentJourneyPosition.LearningBlock == AppManager.I.Player.MaxJourneyPosition.LearningBlock;
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }

        #region Dialogs

        private void PlayRandomAssessmentDialog()
        {
            var data = new LocalizationDataId[3];
            data[0] = LocalizationDataId.Assessment_Start_1;
            data[1] = LocalizationDataId.Assessment_Start_2;
            data[2] = LocalizationDataId.Assessment_Start_3;
            var n = Random.Range(0, data.Length);
            KeeperManager.I.PlayDialogue(data[n], true, true);
        }

        #endregion

        #region Selection

        private Pin selectedPin = null;

        // Used by the Antura Hint
        public void MoveToPlayerPin()
        {
            // Do not teleport to player while he is animating!
            //if (playerPin.IsAnimating) return;

            var playerStageMap = StageMap(CurrentPlayerStage);
            //Debug.Log("Player current pin is " + playerPin.CurrentPinIndex);
            //Debug.Log("CurrentPlayerStage: " + CurrentPlayerStage);
            //Debug.Log("current player stage map: " + playerStageMap.stageNumber);
            var targetPin = playerStageMap.PinForIndex(playerPin.CurrentPinIndex);
            mapCameraController.SetAutoFollowTransformCurrentMap(targetPin.transform);

            if (targetPin != selectedPin) {
                //Debug.Log("Selecting pin " + targetPin.journeyPosition);
                SelectPin(targetPin);
            }
        }

        private void ReSelectCurrentPin()
        {
            var playerStageMap = StageMap(CurrentPlayerStage);
            SelectPin(playerStageMap.PinForIndex(playerPin.CurrentPinIndex));

            // Make sure to move the camera too
            mapCameraController.SetAutoFollowTransformCurrentMap(playerPin.transform);
        }

        public Pin SelectedPin { get { return selectedPin; } }

        public void SelectPin(Pin pin)
        {
            if (!pin.IsInteractionEnabled) return;

            if (selectedPin == pin) {
                // Already selected: PLAY directly (if not locked)
                if (selectedPin.isLocked) {
                    HandleLockedButton();
                } else {
                    PlayCurrentPlaySession();
                }
            } else {
                // New selection
                selectedPin = pin;

                // Check for stage change (can happen at the sides!)
                if (shownStage != pin.journeyPosition.Stage) {
                    shownStage = pin.journeyPosition.Stage;
                    ColorCameraToShownStage(shownStage);
                }

                ResetSelections();
                selectedPin.Select(true);

                playInfoPanel.SetPin(pin);
                playButtonsPanel.SetPin(pin);

                // Optionally move Antura there
                if (!pin.isLocked) {
                    playerPin.MoveToPin(pin.pinIndex, shownStage);
                }
            }
        }

        void PlayCurrentPlaySession()
        {
            (MapScene.I as MapScene).Play();
        }

        #endregion

        #region Selection

        public void ResetSelections()
        {
            foreach (var stageMap in stageMaps) {
                // Deselect all pins
                if (stageMap.Pins != null)
                {
                    foreach (var pin in stageMap.Pins) {
                         pin.Select(false);
                    }
                }

                // Select the current pin
                //var correctPin = stageMap.PinForJourneyPosition(CurrentJourneyPosition);
                //if (correctPin != null) correctPin.Select(true);
            }
        }

        #endregion

        // TODO: check if something called this
        /*private void PlayDialogStages(LocalizationDataId data)
        {
            KeeperManager.I.PlayDialog(data);
        }*/

        #region Stage Navigation

        /// <summary>
        /// Move to the next Stage map
        /// Called by buttons.
        /// </summary>
        public void MoveToNextStageMap()
        {
            if (inTransition) return;
            if (IsAtFinalStage) return;

            int fromStage = shownStage;
            int toStage = shownStage + 1;

            SwitchFromToStage(fromStage, toStage, true);

            //HideTutorial();
        }

        /// <summary>
        /// Move to the previous Stage map
        /// Called by buttons.
        /// </summary>
        public void MoveToPreviousStageMap()
        {
            if (inTransition) return;
            if (IsAtFirstStage) return;

            int fromStage = shownStage;
            int toStage = shownStage - 1;

            SwitchFromToStage(fromStage, toStage, true);

            //if (IsAtFirstStage) {
            //    ShowTutorial();
            //}
        }

        public void MoveToStageMap(int toStage, bool animateCamera = false)
        {
            if (inTransition) return;

            int fromStage = shownStage;
            if (toStage == fromStage) return;

            //Debug.Log("FROM " + fromStage + " TO " + toStage);
            SwitchFromToStage(fromStage, toStage, animateCamera);
        }

        /*private void UpdateButtonsForStage(int stage)
        {
            bool playable = IsStagePlayable(stage);
        }*/

        /*private void CheckCurrentStageForPlayerReset()
        {
            //Debug.Log("ShownStage: " + shownStage + " Current: " + CurrentPlayerStage);
            if (shownStage != CurrentPlayerStage)
            {
                bool comingFromHigherStage = CurrentPlayerStage > shownStage;
                playerPin.ResetPlayerPositionAfterStageChange(comingFromHigherStage);
            }
        }*/

        private void SwitchFromToStage(int fromStage, int toStage, bool animateCamera = false)
        {
            if (!isLazyInitialised) return; // wait for initialisition
            StartCoroutine(SwitchFromToStageCO(fromStage, toStage, animateCamera));
        }

        private IEnumerator SwitchFromToStageCO(int fromStage, int toStage, bool animateCamera = false)
        {
            shownStage = toStage;

            inTransition = true;
            //Debug.Log("Switch from " + fromStage + " to " + toStage);

            // HidePlayPanel();

            // Change stage reference
            StageMap(toStage).FlushAppear(AppManager.I.Player.MaxJourneyPosition);

            // Update Player Stage too, if needed
            /*if (MovePlayerWithStageChange)
            {
                if (IsStagePlayable(toStage) && toStage != shownStage)
                {
                    bool comingFromHigherStage = fromStage > toStage;
                    playerPin.ResetPlayerPositionAfterStageChange(comingFromHigherStage);
                }
            }*/

            // Animate the switch
            if (animateCamera) {
                AnimateCameraToShownStage(toStage);
                yield return new WaitForSeconds(0.8f);
            } else {
                ColorCameraToShownStage(toStage);
            }

            // Update the stage map for the player too
            if (MovePlayerWithStageChange) SwitchStageMapForPlayer(StageMap(toStage));

            // Show the new stage
            UpdateStageIndicatorUI(toStage);
            //UpdateButtonsForStage(toStage);

            /*
            if (MovePlayerWithStageChange) {
                ShowPlayPanel();
            } else {
                if (toStage == CurrentPlayerStage) {
                    ShowPlayPanel();
                }
            }
            */

            // Hide the last stage
            //StageMap(fromStage).Hide();

            // End transition
            inTransition = false;

            //Debug.Log("We are at stage " + shownStage + ". Player current is " + CurrentPlayerStage);

            yield return null;
        }

        #endregion

        #region Camera

        private void TeleportCameraToShownStage(int stage)
        {
            var stageMap = StageMap(stage);
            stageMap.Show();

            // We'll look at the current player position, if possible.
            bool playable = IsStagePlayable(stage);
            if (playable) {
                mapCameraController.TeleportToLookAtFree(playerPin.transform, stageMap.cameraPivotStart);
            } else {
                mapCameraController.TeleportTo(stageMap.cameraPivotStart);
            }

            Camera.main.backgroundColor = stageMap.color;
            Camera.main.GetComponent<CameraFog>().color = stageMap.color;

            SwitchStageMapForPlayer(stageMap, true);
        }

        private void AnimateCameraToShownStage(int stage)
        {
            //Debug.Log("Animating to stage " + stage);
            var stageMap = StageMap(stage);
            stageMap.Show();
            stageMap.ResetStageOnShow(CurrentPlayerStage == stage);

            // We'll look at the current player position, if possible
            // AND if the player is in that stage
            bool playable = IsStagePlayable(stage);
            if (playable && playerPin.currentStageMap == stageMap) {
                mapCameraController.SetAutoMoveToLookAtFree(playerPin.transform, stageMap.cameraPivotStart, 0.6f);
            } else {
                mapCameraController.SetAutoMoveToTransformFree(stageMap.cameraPivotStart, 0.6f);
            }

            Camera.main.DOColor(stageMap.color, 1);
            Camera.main.GetComponent<CameraFog>().color = stageMap.color;
        }

        private void ColorCameraToShownStage(int stage)
        {
            var stageMap = StageMap(stage);
            Camera.main.DOColor(stageMap.color, 1);
            Camera.main.GetComponent<CameraFog>().color = stageMap.color;
        }

        private void SwitchStageMapForPlayer(StageMap newStageMap, bool init = false)
        {
            if (playerPin.IsAnimating) playerPin.StopAnimation(stopWhereItIs: false);
            playerPin.currentStageMap = newStageMap;

            // Move the player too, if the stage is unlocked
            if (!init && !newStageMap.FirstPin.isLocked && MovePlayerWithStageChange) {
                playerPin.ForceToJourneyPosition(CurrentJourneyPosition);
            }
        }

        #endregion

        #region Stage Indicator UI

        private void UpdateStageIndicatorUI(int stage)
        {
            mapStageIndicator.Init(stage - 1, FinalStage);
        }

        #endregion

        #region UI Activation

        /*
        private void ShowPlayButton()
        {
            playButton.SetActive(true);
        }

        private void HidePlayButton()
        {
            playButton.SetActive(false);
        }

        private void ShowPlayPanel()
        {
            //playButtonsPanel.gameObject.SetActive(true);
            playInfoPanel.gameObject.SetActive(true);
            playerPin.CheckMovementButtonsEnabling();
        }

        private void HidePlayPanel()
        {
            playButtonsPanel.gameObject.SetActive(false);
            playInfoPanel.gameObject.SetActive(false);
        }
        */

        public void DeactivateAllUI()
        {
            SetStageUIActivation(false);
            SetLearningBookUIActivation(false);
            SetMinigamesBookUIActivation(false);
            SetProfileBookUIActivation(false);
            SetAnturaSpaceUIActivation(false);
            SetPlayUIActivation(false);
            SetPauseUIActivation(false);
            SetExitButtonActivation(false);
        }

        public void ActivateAllUI()
        {
            SetStageUIActivation(true);
            SetLearningBookUIActivation(true);
            SetMinigamesBookUIActivation(true);
            SetProfileBookUIActivation(true);
            SetAnturaSpaceUIActivation(true);
            SetPlayUIActivation(true);
            SetPauseUIActivation(true);
            SetExitButtonActivation(true);
        }

        private void HandleBackButton()
        {
            AppManager.I.NavigationManager.ExitToMainMenu();
        }

        public void SetPlayUIActivation(bool choice)
        {
            if (selectedPin) { selectedPin.EnableInteraction(choice); }
        }


        public void SetUIActivationByContactPhase(FirstContactPhase phase, bool choice)
        {
            switch (phase)
            {
                 case FirstContactPhase.Map_GoToProfile:
                    SetProfileBookUIActivation(choice);
                    break;
                case FirstContactPhase.Map_GoToBook:
                    SetLearningBookUIActivation(choice);
                    break;
                case FirstContactPhase.Map_GoToMinigames:
                    SetMinigamesBookUIActivation(choice);
                    break;
                case FirstContactPhase.Map_GoToAnturaSpace:
                    SetAnturaSpaceUIActivation(choice);
                    break;
            }
        }

        public GameObject GetGameObjectByContactPhase(FirstContactPhase phase)
        {
            switch (phase)
            {
                case FirstContactPhase.Map_GoToProfile:
                    return profileBookButton;
                case FirstContactPhase.Map_GoToBook:
                    return learningBookButton;
                case FirstContactPhase.Map_GoToMinigames:
                    return minigamesBookButton;
                case FirstContactPhase.Map_GoToAnturaSpace:
                    return anturaSpaceButton;
            }
            return null;
        }

        public void SetExitButtonActivation(bool choice)
        {
            if (choice)
            {
                GlobalUI.ShowBackButton(true, HandleBackButton);
            }
            else
            {
                GlobalUI.ShowBackButton(false);
            }
        }

        public void SetStageUIActivation(bool choice)
        {
            mapStageIndicator.gameObject.SetActive(choice);
        }

        public void SetLearningBookUIActivation(bool choice)
        {
            learningBookButton.gameObject.SetActive(choice);
        }

        public void SetProfileBookUIActivation(bool choice)
        {
            profileBookButton.gameObject.SetActive(choice);
        }

        public void SetMinigamesBookUIActivation(bool choice)
        {
            minigamesBookButton.gameObject.SetActive(choice);
        }

        public void SetAnturaSpaceUIActivation(bool choice)
        {
            anturaSpaceButton.gameObject.SetActive(choice);
        }

        public void SetPauseUIActivation(bool choice)
        {
            GlobalUI.ShowPauseMenu(choice);
        }

        #endregion

        public void HandleLockedButton()
        {
            AudioManager.I.PlaySound(Sfx.KO);
        }

        #region Static Utilities

        public static int GetPosIndexFromJourneyPosition(StageMap stageMap, JourneyPosition journeyPos)
        {
            var st = journeyPos.Stage;

            if (stageMap.stageNumber > st)
                return 0;

            if (stageMap.stageNumber < st)
                return stageMap.MaxUnlockedPinIndex;

            var pin = stageMap.PinForJourneyPosition(journeyPos);
            return pin.pinIndex;
        }

        #endregion

    }
}