using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace Antura.Discover.Activities
{
    public class ActivityQuiz : ActivityBase
    {
        [Header("Activity Quiz Settings")]
        public QuizSettingsData Settings;

        [Header("Override Settings")]
        public Difficulty ActivityDifficulty = Difficulty.Default;

        [Header("UI References")]
        public TextMeshProUGUI QuestionLabel;
        public Transform AnswersContainer; // children contain buttons/toggles
        public Button ValidateButton;

        private readonly List<Toggle> _answerToggles = new List<Toggle>();
        private bool _allowMultiple; // if multiple correct entries exist

        public override void Init()
        {
            base.Init();
            WireUI();
            BuildRound();
        }

        protected override ActivitySettingsAbstract GetSettings() => Settings;

        private void WireUI()
        {
            _answerToggles.Clear();
            if (AnswersContainer != null)
            {
                for (int i = 0; i < AnswersContainer.childCount; i++)
                {
                    var child = AnswersContainer.GetChild(i);
                    var toggle = child.GetComponentInChildren<Toggle>();
                    if (toggle != null)
                    {
                        toggle.onValueChanged.RemoveAllListeners();
                        toggle.isOn = false;
                        toggle.onValueChanged.AddListener(OnSelectionChanged);
                        _answerToggles.Add(toggle);
                    }
                }
            }

            if (ValidateButton != null)
            {
                ValidateButton.onClick.RemoveAllListeners();
                ValidateButton.onClick.AddListener(Validate);
            }

            SetValidateEnabled(false);
        }

        private void BuildRound()
        {
            if (Settings == null)
                return;

            // Determine if multiple answers are valid in this round
            int correctCount = 0;
            if (Settings.Answers != null)
                foreach (var a in Settings.Answers)
                    if (a != null && a.IsCorrect)
                        correctCount++;
            _allowMultiple = correctCount > 1;

            if (QuestionLabel != null)
                QuestionLabel.text = Settings.Question ?? "";

            // Map settings answers to UI toggles (assumes same or fewer toggles than data)
            int n = Mathf.Min(_answerToggles.Count, Settings.Answers != null ? Settings.Answers.Count : 0);
            for (int i = 0; i < _answerToggles.Count; i++)
            {
                var toggle = _answerToggles[i];
                if (i < n && Settings.Answers[i] != null)
                {
                    var label = toggle.GetComponentInChildren<TextMeshProUGUI>();
                    if (label != null)
                    {
                        var item = Settings.Answers[i].Item; // CardItem is a struct
                        label.text = string.IsNullOrEmpty(item.Name) ? "" : item.Name;
                    }
                    toggle.gameObject.SetActive(true);
                    toggle.isOn = false;
                }
                else
                {
                    toggle.gameObject.SetActive(false);
                }
            }

            SetValidateEnabled(false);
        }

        private void OnSelectionChanged(bool _)
        {
            // Enable validate when at least one selection for single-answer, or any selection for multi-answer
            bool any = _answerToggles.Exists(t => t.isOn);
            SetValidateEnabled(any);
        }

        public override bool DoValidate()
        {
            // Compute correctness
            if (Settings == null || Settings.Answers == null || Settings.Answers.Count == 0)
                return false;

            int n = Mathf.Min(_answerToggles.Count, Settings.Answers.Count);
            bool allMatch = true;
            int correctChosen = 0;
            int totalCorrect = 0;
            for (int i = 0; i < n; i++)
            {
                bool shouldBeOn = Settings.Answers[i].IsCorrect;
                if (shouldBeOn)
                    totalCorrect++;
                bool isOn = _answerToggles[i].isOn;
                if (isOn && shouldBeOn)
                    correctChosen++;
                if (isOn != shouldBeOn)
                    allMatch = false;
            }

            // Allow partial success -> used by GetRoundScore01
            _lastScore01 = totalCorrect > 0 ? Mathf.Clamp01((float)correctChosen / totalCorrect) : 0f;
            return allMatch;
        }

        private float _lastScore01 = 0f;
        protected override float GetRoundScore01() => _lastScore01;
    }
}
