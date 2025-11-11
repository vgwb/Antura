using Demigiant.DemiTools;
using Demigiant.DemiTools.DeUnityExtended;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using DG.DeInspektor.Attributes;
using Antura.UI;

namespace Antura.Discover.Activities
{
    public class ActivityOverlay : MonoBehaviour
    {
        [Header("References")]
        [DeEmptyAlert]
        [SerializeField] ActivityTimer timer;
        [DeEmptyAlert]
        [SerializeField] DeUIButton btClose;
        [DeEmptyAlert]
        [SerializeField] DeUIButton btHelp;
        [DeEmptyAlert]
        [SerializeField] DeUIButton btValidate;

        [SerializeField] TextRender labelActivity;
        [SerializeField] TextRender labelTopic;

        [SerializeField] GameObject feedbackBox;
        [SerializeField] TextRender feedbackText;

        [Header("Result Prompt")]
        [SerializeField] GameObject resultPromptRoot;
        [SerializeField] TextRender resultPromptLabel;
        [SerializeField] TextRender resultPromptLabelTranslated;

        [SerializeField] Button resultContinueButton;
        [SerializeField] Button resultRetryButton;

        public ActivityTimer Timer => timer;
        public DeUIButton BtClose => btClose;
        public DeUIButton BtHelp => btHelp;
        public DeUIButton BtValidate => btValidate;

        Action onContinueCallback;
        Action onRetryCallback;
        UnityAction continueHandler;
        UnityAction retryHandler;

        public void SetFeedback(string message)
        {
            feedbackBox.SetActive(!string.IsNullOrEmpty(message));
            feedbackText.SetText(message ?? string.Empty);
        }

        public void SetActivityLabels(string activityName, string topic = "")
        {
            if (labelActivity != null)
                labelActivity.SetText(activityName ?? string.Empty);
            if (labelTopic != null)
                labelTopic.SetText(topic ?? string.Empty);
        }

        public void SetTimer(bool hasTimer, int seconds)
        {
            if (timer == null)
                return;

            timer.gameObject.SetActive(hasTimer);
            if (hasTimer)
                timer.RestartTimer(seconds);
            else
                timer.CancelTimer();
        }

        public void ShowResultPrompt(string message, string messageTranslated, Color color, Action onContinue, Action onRetry)
        {
            if (resultPromptRoot == null)
            {
                onContinue?.Invoke();
                return;
            }

            ClearPromptListeners();

            resultPromptLabel.text = message ?? string.Empty;
            resultPromptLabelTranslated.text = messageTranslated ?? string.Empty;

            onContinueCallback = onContinue;
            onRetryCallback = onRetry;

            if (resultContinueButton != null)
            {
                continueHandler = () => HandlePromptSelection(onContinueCallback);
                resultContinueButton.onClick.AddListener(continueHandler);
            }
            if (resultRetryButton != null)
            {
                retryHandler = () => HandlePromptSelection(onRetryCallback);
                resultRetryButton.onClick.AddListener(retryHandler);
            }

            resultPromptRoot.SetActive(true);
        }

        public void HideResultPrompt()
        {
            if (resultPromptRoot != null)
                resultPromptRoot.SetActive(false);
            ClearPromptListeners();
        }

        void HandlePromptSelection(Action callback)
        {
            HideResultPrompt();
            callback?.Invoke();
        }

        void ClearPromptListeners()
        {
            if (resultContinueButton != null && continueHandler != null)
            {
                resultContinueButton.onClick.RemoveListener(continueHandler);
                continueHandler = null;
            }
            if (resultRetryButton != null && retryHandler != null)
            {
                resultRetryButton.onClick.RemoveListener(retryHandler);
                retryHandler = null;
            }

            onContinueCallback = null;
            onRetryCallback = null;
        }
    }
}
