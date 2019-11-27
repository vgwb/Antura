using Antura.Animation;
using Antura.Core;
using Antura.UI;
using DG.DeExtensions;
using DG.Tweening;
using System.Collections.Generic;
using Antura.Teacher;
using UnityEngine;

namespace Antura.Map
{
    /// <summary>
    /// A pin on the map. 
    /// Defines a PlaySession. Either assessment or minigame.
    /// </summary>
    public class Pin : MonoBehaviour, IMapLocation
    {
        [HideInInspector]
        public JourneyPosition journeyPosition;

        [Header("References")]
        public Dot mainDot;
        [HideInInspector]
        public Rope rope;
        [HideInInspector]
        public List<Dot> dots = new List<Dot>();

        public GameObject pinV1;
        public GameObject pinV2;
        public GameObject pinAssessment;
        public GameObject pinRecap;
        public GameObject pinEndGame;

        public GameObject roadSignGO;
        public TextRender roadSignTextUI;

        [HideInInspector]
        public GameObject currentPinGO;

        public PlaySessionStateFeedback playSessionFeedback;

        [HideInInspector]
        public bool isLocked;

        private Transform shadowTr;

        [Header("Animation")]
        private Vector3 startPinPosition;
        private Vector3 startRopeScale;

        public Vector3 Position => transform.position;

        public JourneyPosition JourneyPos => journeyPosition;

        // Sequential index for the stage map this pin is in
        public int pinIndex;

        public void Initialise(int _pinIndex, JourneyPosition _journeyPosition)
        {
            pinIndex = _pinIndex;
            journeyPosition = _journeyPosition;

            name = "Pin_" + _journeyPosition;

            // Road sign when a new LB starts
            var prevJP = AppManager.I.JourneyHelper.FindPreviousJourneyPosition(journeyPosition);
            roadSignGO.SetActive(prevJP == null || prevJP.LearningBlock != journeyPosition.LearningBlock);
            roadSignTextUI.SetText(journeyPosition.ToDisplayedString(false));

            // Choosing the correct PIN based on the journey position
            pinV1.gameObject.SetActive(false);
            pinV2.gameObject.SetActive(false);
            pinAssessment.gameObject.SetActive(false);

            if (journeyPosition.IsEndGame())
            {
                currentPinGO = pinEndGame;
            }
            else if (journeyPosition.IsRecap())
            {
                currentPinGO = pinRecap;
            }
            else if (journeyPosition.IsAssessment())
            {
                currentPinGO = pinAssessment;
            }
            else
            {
                currentPinGO = journeyPosition.LearningBlock % 2 == 0 ? pinV1 : pinV2;
            }
            currentPinGO.SetActive(true);

            shadowTr = transform.Find("shadow");

            HandlePlayerClose(false);
        }

        #region Appear / Disappear

        private bool appeared = false;

        public bool Appeared { get { return appeared; } }

        public void InitPinHidden()
        {
            appeared = false;
            startPinPosition = currentPinGO.transform.position;

            if (!journeyPosition.IsAssessment()) {
                currentPinGO.transform.position = startPinPosition + Vector3.up * 60;
            }
            currentPinGO.gameObject.SetActive(false);

            mainDot.Highlight(false);

            shadowTr.SetLocalScale(0);
        }

        public void InitOptionalsHidden()
        {
            if (rope != null) {
                startRopeScale = rope.meshRenderer.transform.localScale;
            }

            if (rope != null) {
                rope.meshRenderer.transform.SetLocalScale(0);
            }
            foreach (var dot in dots) {
                dot.Disappear();
            }
        }

        public void Appear(float duration)
        {
            if (appeared) { return; }
            appeared = true;

            currentPinGO.gameObject.SetActive(true);

            if (journeyPosition.IsAssessment()) {
                currentPinGO.transform.localScale = Vector3.zero;
                currentPinGO.transform.DOScale(Vector3.one, duration * 0.5f).SetEase(Ease.OutElastic);
            } else {
                currentPinGO.transform.DOMove(startPinPosition, duration * 0.1f);
            }

            mainDot.transform.DOScale(Vector3.one * 8, duration * 0.5f).SetEase(Ease.OutElastic).SetDelay(duration * 0.25f).OnComplete(
                () =>
                playSessionFeedback.gameObject.SetActive(true)  // make the feedback appear at the end
                );
            shadowTr.DOScale(Vector3.one * 12.5f, duration * 0.5f).SetEase(Ease.OutElastic).SetDelay(duration * 0.25f);

            if (rope != null) { rope.meshRenderer.transform.DOScale(startRopeScale, duration * 0.5f).SetDelay(duration * 0.25f); }
        }

        public void FlushAppear()
        {
            if (appeared) { return; }
            appeared = true;
            currentPinGO.gameObject.SetActive(true);
            currentPinGO.transform.position = startPinPosition;
            currentPinGO.transform.localScale = Vector3.one;
            mainDot.transform.localScale = Vector3.one * 8;
            shadowTr.transform.localScale = Vector3.one * 12.5f;
            playSessionFeedback.gameObject.SetActive(true);

            if (rope != null) { rope.meshRenderer.transform.localScale = startRopeScale; }
        }

        #endregion

        #region Locking

        public void SetUnlocked()
        {
            isLocked = false;
            //mainDot.gameObject.SetActive(true);

            playSessionFeedback.ShowUnhighlightedInfo();
        }

        public void SetLocked()
        {
            isLocked = true;
            //mainDot.gameObject.SetActive(false);

            playSessionFeedback.HideAllInfo();
        }

        #endregion

        #region PlayerPin Interaction

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player")) {
                HandlePlayerClose(true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Player")) {
                HandlePlayerClose(false);
            }
        }

        private Sequence playerCloseTween;

        void HandlePlayerClose(bool choice)
        {
            if (playerCloseTween == null) {
                float duration = 0.5f;
                playerCloseTween = DOTween.Sequence();
                playerCloseTween.Join(currentPinGO.transform.DOScale(Vector3.zero, duration).SetEase(Ease.InOutCubic));
                playerCloseTween.Join(playSessionFeedback.surpriseGO.transform.DOMove(playSessionFeedback.surpriseGO.transform.position + Vector3.up * 15, duration).SetEase(Ease.OutElastic));
                playerCloseTween.Pause();
                playerCloseTween.SetAutoKill(false);
            }

            playSessionFeedback.surpriseGO.GetComponent<Surprise3D>().SetPulsing(choice);

            if (choice) {
                playerCloseTween.PlayForward();
            } else {
                playerCloseTween.PlayBackwards();
            }
        }

        #endregion

        #region Highlight

        bool interactionEnabled = true;
        public void EnableInteraction(bool choice)
        {
            interactionEnabled = choice;
            HighlightFeedback(interactionEnabled);
        }

        public bool IsInteractionEnabled { get { return interactionEnabled; } }

        public void Select(bool choice)
        {
            if (!interactionEnabled) {
                mainDot.SetAsNothing();
                return;
            }

            HighlightFeedback(choice);

            if (!isLocked) {
                playSessionFeedback.Highlight(choice);
            }
        }

        void HighlightFeedback(bool choice)
        {
            mainDot.Highlight(choice);
            if (choice) {
                if (isLocked) {
                    mainDot.SetAsLock();
                } else {
                    mainDot.SetAsPlay();
                }
            } else {
                mainDot.SetAsNothing();
            }
        }

        #endregion

        #region Play Session State

        public void SetPlaySessionState(PlaySessionState playSessionState)
        {
            playSessionFeedback.Initialise(journeyPosition, playSessionState);
        }

        #endregion
    }
}