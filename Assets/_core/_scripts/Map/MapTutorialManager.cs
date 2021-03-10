using System;
using Antura.Core;
using Antura.Database;
using Antura.Keeper;
using Antura.Profile;
using Antura.Tutorial;
using System.Collections;
using UnityEngine;

namespace Antura.Map
{
    public class MapTutorialManager : TutorialManager
    {
        public StageMapsManager _stageMapsManager;

        protected override AppScene CurrentAppScene
        {
            get {
                return AppScene.Map;
            }
        }

        protected override void InternalHandleStart()
        {
            // References

            // All UI is deactivated, for starters
            _stageMapsManager.DeactivateAllUI();

            // Always activate these buttons
            _stageMapsManager.SetExitButtonActivation(true);
            _stageMapsManager.SetStageUIActivation(true);

            // TODO: see AnturaSpaceTutorialManager and copy it for the unlock and skip steps!

            // Antura Space (auto-unlocked from the start)
            if (CheckNewUnlockPhaseAt(FirstContactPhase.Map_GoToAnturaSpace, LocalizationDataId.Map_Intro_AnturaSpace))
                return;

            // Profile (auto-unlocked from the start)
            if (CheckNewUnlockPhaseAt(FirstContactPhase.Map_GoToProfile, LocalizationDataId.Map_Intro_AnturaSpace))
                return;

            // TODO: at the end, call CompleteCurrentPhase, if we need more phases in the same scene
            // Check for sequential phases
            bool isPlayingSequentialPhase = false;
            switch (FirstContactManager.I.CurrentPhaseInSequence)
            {
                case FirstContactPhase.Map_PlayFirstSession:

                    StepTutorialPlayFirstSession();

                    // @note: this phase is completed not on Play, but when we come back after the results
                    // check the FirstContactManager navigation filtering
                    isPlayingSequentialPhase = true;
                    break;

            }
            if (isPlayingSequentialPhase) return;

            // New features unlocking
            /*
            // Book
            if (CheckNewUnlockPhaseAt(FirstContactPhase.Map_GoToBook, LocalizationDataId.Map_Intro_AnturaSpace))
                return;

            // MiniGames
            if (CheckNewUnlockPhaseAt(FirstContactPhase.Map_GoToMinigames, LocalizationDataId.Map_Intro_AnturaSpace))
                return;
                */

            // If nothing new is being unlocked, let the player play
            _stageMapsManager.SetPlayUIActivation(true);
            StopTutorialRunning();
        }

        protected override void SetPhaseUIShown(FirstContactPhase phase, bool choice)
        {
            _stageMapsManager.SetUIActivationByContactPhase(phase, choice);
        }

        #region Play First Session

        private void StepTutorialPlayFirstSession()
        {
            CurrentRunningPhase = FirstContactPhase.Map_PlayFirstSession;

            _stageMapsManager.SetPlayUIActivation(true);

            KeeperManager.I.PlayDialogue(LocalizationDataId.Map_Intro, autoClose:false, _callback:
                () =>
                {
                    IntroDialogues.PlayCurrentAudio();
                    var firstPinTr = _stageMapsManager.stageMaps[0].Pins[0].transform;
                    StartCoroutine(TutorialHintClickCO(firstPinTr, _stageMapsManager.MapCamera));
                }
            );

        }

        #endregion


        private bool CheckNewUnlockPhaseAt(FirstContactPhase phase, LocalizationDataId localizationDataId)
        {
            UnlockPhaseIfReachedJourneyPosition(phase);

            bool isPhaseToBeCompleted = IsPhaseToBeCompleted(phase);
            if (isPhaseToBeCompleted)
            {
                KeeperManager.I.PlayDialogue(localizationDataId, true, true, () =>
                {
                    _stageMapsManager.SetUIActivationByContactPhase(phase, true);
                    StartCoroutine(TutorialHintClickCO(_stageMapsManager.GetGameObjectByContactPhase(phase).transform, _stageMapsManager.UICamera));
                });
                return true;
            }
            else if (IsPhaseUnlocked(phase))
            {
                _stageMapsManager.SetUIActivationByContactPhase(phase, true);
            }
            return false;
        }

        private IEnumerator TutorialHintClickCO(Transform targetTr, Camera camera)
        {
            TutorialUI.SetCamera(camera);
            while (true)
            {
                var viewportPos = camera.WorldToViewportPoint(targetTr.position);
                if (viewportPos.x > 0 && viewportPos.x < 1)
                    TutorialUI.Click(targetTr.position);
                yield return new WaitForSeconds(0.85f);
            }
        }
    }
}