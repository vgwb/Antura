using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum PianoPlayMode { Freeplay, Repeat }
public enum RepeatDifficulty { Tutorial, Easy, Normal, Difficult }

namespace Antura.Discover.Activities
{
    public class ActivityPiano : ActivityBase
    {
        [Header("Activity Piano Settings")]
        public PianoPlayMode playMode = PianoPlayMode.Freeplay;
        public RepeatDifficulty repeatDifficulty = RepeatDifficulty.Easy;

        [Header("References")]

        public PianoKeyboard keyboard;
        public MelodySequenceSO melody;

        public Button playButton;
        public Button stopButton;
        public TextMeshProUGUI statusLabel;

        public Color tutorialFlash = new Color(1f, 0.95f, 0.3f, 1f);

        private Coroutine currentRoutine;
        private List<NoteName> targetNotes;
        private int inputIndex = 0;
        private bool acceptingInput = false;
        private float beatSeconds => 60f / Mathf.Max(1, melody != null ? melody.tempoBPM : 100);

        private void Start()
        {
            if (keyboard)
                keyboard.Build();
            WireUI();
            ApplyDifficultyVisuals();
            BuildTargetNotes();
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
            if (melody == null)
                return;
            foreach (var ev in melody.sequence)
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
            if (statusLabel)
                statusLabel.text = "Ready";
        }

        private IEnumerator PlaySequenceThenAwait()
        {
            acceptingInput = false;
            inputIndex = 0;
            if (statusLabel)
                statusLabel.text = "Listening...";

            if (melody == null)
                yield break;

            foreach (var ev in melody.sequence)
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
                    if (repeatDifficulty == RepeatDifficulty.Tutorial)
                        key.Flash(seconds);
                    yield return new WaitForSeconds(seconds);
                }
                else
                {
                    var anyKey = keyboard.AllKeys().FirstOrDefault(k => k.noteName == n);
                    if (anyKey != null)
                    {
                        anyKey.Play();
                        if (repeatDifficulty == RepeatDifficulty.Tutorial)
                            anyKey.Flash(seconds);
                    }
                    yield return new WaitForSeconds(seconds);
                }
            }

            if (statusLabel)
                statusLabel.text = "Your turn!";
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
                    if (statusLabel)
                        statusLabel.text = "Correct!";
                    acceptingInput = false;
                }
            }
            else
            {
                if (statusLabel)
                    statusLabel.text = "Wrong, try again!";
                inputIndex = 0;
            }
        }

        public void ApplyDifficultyVisuals()
        {
            foreach (var key in keyboard.AllKeys())
            {
                bool showLabel = (repeatDifficulty == RepeatDifficulty.Tutorial) || (repeatDifficulty == RepeatDifficulty.Easy) || (repeatDifficulty == RepeatDifficulty.Normal);
                key.SetLabelActive(showLabel);
                if (repeatDifficulty == RepeatDifficulty.Difficult)
                    key.SetLabelActive(false);
            }
        }
    }
}
