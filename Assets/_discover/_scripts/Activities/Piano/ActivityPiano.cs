using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum PianoPlayMode { Freeplay, Repeat }

namespace Antura.Discover.Activities
{
    public class ActivityPiano : ActivityBase, IActivity
    {
        [Header("Activity Piano Settings")]
        public PianoSettingsData Settings;

        [Header("Override Settings")]
        public Difficulty ActivityDifficulty = Difficulty.Default;
        public PianoPlayMode playMode = PianoPlayMode.Freeplay;

        [Header("References")]
        public PianoKeyboard keyboard;
        public Button playButton;
        public Button stopButton;

        public Color tutorialFlash = new Color(1f, 0.95f, 0.3f, 1f);

        private Coroutine currentRoutine;
        private List<NoteName> targetNotes;
        private int inputIndex = 0;
        private bool acceptingInput = false;
        private float beatSeconds => 60f / Mathf.Max(1, Settings != null ? Settings.tempoBPM : 100);

        public override void ConfigureSettings(ActivitySettingsAbstract settings)
        {
            base.ConfigureSettings(settings);
            if (settings is PianoSettingsData csd)
                Settings = csd;
        }
        protected override ActivitySettingsAbstract GetSettings() => Settings;

        public override void InitActivity()
        {
            if (keyboard)
                keyboard.Build();
            WireUI();
            ApplyDifficultyVisuals();
            BuildTargetNotes();
        }

        private void Win()
        {
            Debug.Log("🏆 Memory: all pairs found!");
            EnableValidateButton(true);
        }

        public override bool DoValidate()
        {
            return true;
        }

        protected override void Update()
        {
            base.Update();
        }

        private void WireUI()
        {
            if (playButton)
            {
                playButton.onClick.RemoveAllListeners();
                playButton.onClick.AddListener(() =>
                {
                    if (currentRoutine != null)
                        StopCoroutine(currentRoutine);
                    currentRoutine = StartCoroutine(PlaySequenceThenAwait());
                });
            }
            if (stopButton)
            {
                stopButton.onClick.RemoveAllListeners();
                stopButton.onClick.AddListener(ResetState);
            }
        }

        private void BuildTargetNotes()
        {
            targetNotes = new List<NoteName>();
            if (Settings == null)
                return;
            foreach (var ev in Settings.sequence)
            {
                if (!ev.IsRest)
                    targetNotes.Add(ev.Note);
            }
        }

        private void ResetState()
        {
            if (currentRoutine != null)
                StopCoroutine(currentRoutine);
            currentRoutine = null;
            acceptingInput = false;
            inputIndex = 0;
            DisplayFeedback("Ready");
        }

        private IEnumerator PlaySequenceThenAwait()
        {
            acceptingInput = false;
            inputIndex = 0;
            DisplayFeedback("Listening...");

            if (Settings == null)
                yield break;

            foreach (var ev in Settings.sequence)
            {
                float durBeats = DurationToBeats(ev.Duration);
                float seconds = durBeats * beatSeconds * 0.5f;

                if (ev.IsRest)
                {
                    yield return new WaitForSeconds(seconds);
                    continue;
                }

                var n = ev.Note;
                var key = keyboard.GetKey(n, ev.Octave);
                if (key != null)
                {
                    key.Play();
                    if (ActivityDifficulty == Difficulty.Tutorial)
                        key.Flash(seconds);
                    yield return new WaitForSeconds(seconds);
                }
                else
                {
                    var anyKey = keyboard.AllKeys().FirstOrDefault(k => k.noteName == n);
                    if (anyKey != null)
                    {
                        anyKey.Play();
                        if (ActivityDifficulty == Difficulty.Tutorial)
                            anyKey.Flash(seconds);
                    }
                    yield return new WaitForSeconds(seconds);
                }
            }

            DisplayFeedback("Your turn!");
            acceptingInput = true;
        }

        private float DurationToBeats(NoteDuration d) => (int)d;

        public void OnKeyPressed(NoteName note)
        {
            if (!acceptingInput)
                return;
            if (targetNotes == null || targetNotes.Count == 0)
                return;

            var expected = targetNotes[inputIndex];
            if (note == expected)
            {
                inputIndex++;
                if (inputIndex >= targetNotes.Count)
                {
                    DisplayFeedback("Correct!");
                    acceptingInput = false;
                    Win();
                }
            }
            else
            {
                DisplayFeedback("Wrong, try again!");
                inputIndex = 0;
            }
        }

        public void ApplyDifficultyVisuals()
        {
            foreach (var key in keyboard.AllKeys())
            {
                bool showLabel = (ActivityDifficulty == Difficulty.Tutorial) || (ActivityDifficulty == Difficulty.Easy) || (ActivityDifficulty == Difficulty.Normal);
                key.SetLabelActive(showLabel);
                if (ActivityDifficulty == Difficulty.Expert)
                    key.SetLabelActive(false);
            }
        }
    }
}
